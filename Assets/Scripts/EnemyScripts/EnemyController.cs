using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject heartPrefab;
    public GameObject rupeePrefab;
    public GameObject bombPrefab;
    private float heartDropChance = 0.1f;
    private float rupeeDropChance = 0.25f;
    private float bombDropChance = 0.25f;

    protected bool active = true;
    protected Transform t;

    private float linkDetectionLength = 3;
    private float rayConst = 3f;

    void Start()
    {
        t = GetComponent<Transform>();
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "Player")
        {
            PlayerController pc = coll.gameObject.GetComponent<PlayerController>();
            pc.TakeDamage(1, getLinkDirection());
        }
    }

    void OnTriggerStay(Collider coll)
    {

        if (coll.tag == "Player")
        {
            //Debug.Log(getLinkDirection());
            PlayerController pc = coll.gameObject.GetComponent<PlayerController>();
            pc.TakeDamage(1, getLinkDirection());
        }
    }

    public virtual void takeDamage(int amount, bool onlyStun = false)
    {
        
    }

    protected virtual void DestroyEnemy()
    {

        //Debug.Log(t.parent);
        //Debug.Log(t.parent.gameObject);

        t.parent.gameObject.transform.parent.gameObject.GetComponent<RoomController>().DestroyAnEnemy();
        if (Random.value < heartDropChance)
        {
            Instantiate(heartPrefab, t.position, Quaternion.identity);
        }
        else if (Random.value < rupeeDropChance)
        {
            Instantiate(rupeePrefab, t.position, Quaternion.identity);
        }
        else if (Random.value < bombDropChance)
        {
            Instantiate(bombPrefab, t.position, Quaternion.identity);
        }

    }

    protected GameController.Direction getLinkDirection()
    {
        BoxCollider collider = GetComponent<BoxCollider>();

        if (castRay(t.position + new Vector3(-collider.size.x * t.localScale.x / rayConst, +collider.size.y * t.localScale.y / rayConst, 0), GameController.getDirectionVector3(GameController.Direction.Up), linkDetectionLength, LayerMask.GetMask("Player")) ||
            castRay(t.position + new Vector3(collider.size.x / rayConst, +collider.size.y * t.localScale.y / rayConst, 0), GameController.getDirectionVector3(GameController.Direction.Up), linkDetectionLength, LayerMask.GetMask("Player")) ||
            castRay(t.position, GameController.getDirectionVector3(GameController.Direction.Up), linkDetectionLength, LayerMask.GetMask("Player")) ||
            castRay(t.position + new Vector3(collider.bounds.extents.x - collider.bounds.size.x, collider.bounds.extents.y + 0.01f, 0), GameController.getDirectionVector3(GameController.Direction.Right), collider.bounds.size.x, LayerMask.GetMask("Player")))
        {
            return GameController.Direction.Up;
        }
        if (castRay(t.position + new Vector3(-collider.size.x * t.localScale.x / rayConst, -collider.size.y * t.localScale.y / rayConst, 0), GameController.getDirectionVector3(GameController.Direction.Down), linkDetectionLength, LayerMask.GetMask("Player")) ||
            castRay(t.position + new Vector3(collider.size.x * t.localScale.x / rayConst, -collider.size.y * t.localScale.y / rayConst, 0), GameController.getDirectionVector3(GameController.Direction.Down), linkDetectionLength, LayerMask.GetMask("Player")) ||
            castRay(t.position, GameController.getDirectionVector3(GameController.Direction.Down), linkDetectionLength, LayerMask.GetMask("Player")) ||
            castRay(t.position + new Vector3(collider.bounds.extents.x - collider.bounds.size.x, collider.bounds.extents.y - collider.bounds.size.y - 0.01f, 0), GameController.getDirectionVector3(GameController.Direction.Right), collider.bounds.size.x, LayerMask.GetMask("Player")))
        {
            return GameController.Direction.Down;
        }
        if (castRay(t.position + new Vector3(-collider.size.x * t.localScale.x / rayConst, +collider.size.y * t.localScale.y / rayConst, 0), GameController.getDirectionVector3(GameController.Direction.Left), linkDetectionLength, LayerMask.GetMask("Player")) ||
            castRay(t.position + new Vector3(-collider.size.x * t.localScale.x / rayConst, -collider.size.y * t.localScale.y / rayConst, 0), GameController.getDirectionVector3(GameController.Direction.Left), linkDetectionLength, LayerMask.GetMask("Player")) ||
            castRay(t.position, GameController.getDirectionVector3(GameController.Direction.Left), linkDetectionLength, LayerMask.GetMask("Player")) ||
            castRay(t.position + new Vector3(collider.bounds.extents.x - collider.bounds.size.x - 0.01f, collider.bounds.extents.y - collider.bounds.size.y, 0), GameController.getDirectionVector3(GameController.Direction.Up), collider.bounds.size.x, LayerMask.GetMask("Player")))
        {
            return GameController.Direction.Left;
        }
        if (castRay(t.position + new Vector3(collider.size.x * t.localScale.x / rayConst, +collider.size.y * t.localScale.y / rayConst, 0), GameController.getDirectionVector3(GameController.Direction.Right), linkDetectionLength, LayerMask.GetMask("Player")) ||
            castRay(t.position + new Vector3(collider.size.x * t.localScale.x / rayConst, -collider.size.y * t.localScale.y / rayConst, 0), GameController.getDirectionVector3(GameController.Direction.Right), linkDetectionLength, LayerMask.GetMask("Player")) ||
            castRay(t.position, GameController.getDirectionVector3(GameController.Direction.Right), linkDetectionLength, LayerMask.GetMask("Player")) ||
            castRay(t.position + new Vector3(collider.bounds.extents.x + 0.01f, collider.bounds.extents.y - collider.bounds.size.y, 0), GameController.getDirectionVector3(GameController.Direction.Up), collider.bounds.size.x, LayerMask.GetMask("Player")))
        {
            return GameController.Direction.Right;
        }
        return GameController.Direction.None;
    }

    protected bool castRay(Vector3 position, Vector3 direction, float length, LayerMask mask)
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
