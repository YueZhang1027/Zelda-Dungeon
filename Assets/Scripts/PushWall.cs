using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushWall : MonoBehaviour
{
    GameObject newWall;
    GameObject newGround;
    GameObject ground;

    public GameObject tileWall;

    public GameObject tileGround;
    
    Transform t;
    private float rayConst = 1f;
    private float linkDetectionLength = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        t = GetComponent<Transform>();
        float x = t.position.x;
        float y = t.position.y;

        newGround = Instantiate(tileGround, new Vector3(x, y, 0), Quaternion.identity);
        ground = Instantiate(tileGround, new Vector3(x, y - 1, 0), Quaternion.identity);
        newWall = Instantiate(tileWall, new Vector3(x, y - 1, 0), Quaternion.identity);

        newWall.SetActive(false);
        newGround.SetActive(false);
        ground.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        if (checkDirection())
        {
            Invoke("push", 1f);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //&& collision.collider.gameObject.CompareTag("Player")
    }

    bool checkDirection()
    {
        if (castRay(t.position + new Vector3(-GetComponent<BoxCollider>().size.x / rayConst, +GetComponent<BoxCollider>().size.y / rayConst, 0), GameController.getDirectionVector3(GameController.Direction.Up), linkDetectionLength, LayerMask.GetMask("Player")) ||
            castRay(t.position + new Vector3(GetComponent<BoxCollider>().size.x / rayConst, +GetComponent<BoxCollider>().size.y / rayConst, 0), GameController.getDirectionVector3(GameController.Direction.Up), linkDetectionLength, LayerMask.GetMask("Player")) ||
            castRay(t.position, GameController.getDirectionVector3(GameController.Direction.Up), linkDetectionLength, LayerMask.GetMask("Player")))
            {
                return true;
            }
        return false;
    }

    void push()
    {
        ground.SetActive(false);
        newWall.SetActive(true);
        newGround.SetActive(true);
        gameObject.SetActive(false);
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
