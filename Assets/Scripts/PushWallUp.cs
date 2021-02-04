using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushWallUp : MonoBehaviour
{
    GameObject newWall;
    GameObject newGround;
    GameObject ground;

    public GameObject tileWall;

    public GameObject tileGround;

    public GameObject blockTriggeredObject;

    public AudioClip secretFound;

    Transform t;
    private float rayConst = 1f;
    private float linkDetectionLength = 1;
    private bool alreadyPushed = false;
    // Start is called before the first frame update
    void Start()
    {
        t = GetComponent<Transform>();
        float x = t.position.x;
        float y = t.position.y;

        newGround = Instantiate(tileGround, new Vector3(x, y, 0), Quaternion.identity);
        ground = Instantiate(tileGround, new Vector3(x, y + 1, 0), Quaternion.identity);
        newWall = Instantiate(tileWall, new Vector3(x, y + 1, 0), Quaternion.identity);

        newWall.SetActive(false);
        newGround.SetActive(false);
        ground.SetActive(true);

        if (blockTriggeredObject != null)
        {
            blockTriggeredObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (checkDirection() && !alreadyPushed)
        {
            alreadyPushed = true;
            Invoke("push", 1f);
        }
    }

    bool checkDirection()
    {
        BoxCollider collider = GetComponent<BoxCollider>();
        if (castRay(t.position + new Vector3(-collider.size.x * t.localScale.x / rayConst, -collider.size.y * t.localScale.y / rayConst, 0), GameController.getDirectionVector3(GameController.Direction.Down), linkDetectionLength, LayerMask.GetMask("Player")) ||
            castRay(t.position + new Vector3(collider.size.x * t.localScale.x / rayConst, -collider.size.y * t.localScale.y / rayConst, 0), GameController.getDirectionVector3(GameController.Direction.Down), linkDetectionLength, LayerMask.GetMask("Player")) ||
            castRay(t.position, GameController.getDirectionVector3(GameController.Direction.Down), linkDetectionLength, LayerMask.GetMask("Player")) ||
            castRay(t.position + new Vector3(collider.bounds.extents.x - collider.bounds.size.x, collider.bounds.extents.y - collider.bounds.size.y - 0.01f, 0), GameController.getDirectionVector3(GameController.Direction.Right), collider.bounds.size.x, LayerMask.GetMask("Player")))
        {
            return true;
        }
        return false;
    }

    void push()
    {
        AudioSource.PlayClipAtPoint(secretFound, Camera.main.transform.position);
        ground.SetActive(false);
        newWall.SetActive(true);
        newGround.SetActive(true);
        gameObject.SetActive(false);
        if (blockTriggeredObject != null)
        {
            blockTriggeredObject.SetActive(false);
        }
    }

    bool castRay(Vector3 position, Vector3 direction, float length, LayerMask mask)
    {
        if (Physics.Raycast(position, transform.TransformDirection(direction), length, mask))
        {
            Debug.DrawRay(position, transform.TransformDirection(direction) * length, Color.yellow);
            return true;
        }
        else
        {
            Debug.DrawRay(position, transform.TransformDirection(direction) * length, Color.white);
            return false;
        }
    }
}
