using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeTrapController : InvincibleEnemyController
{
    public float attackSpeed;
    public float retractSpeed;

    private Vector3 initialPosition;
    private State state;
    private float rightDetectionLength;
    private float leftDetectionLength;
    private float upDetectionLength;
    private float downDetectionLength;
    private float rayConstant = 3.3f;

    private enum State
    {
        Idle,
        Retracting,
        Attacking
    }

    void Start()
    {
        t = GetComponent<Transform>();
        initialPosition = t.position;
        rightDetectionLength = GameController.maxX - t.localPosition.x + 0.25f;
        leftDetectionLength = t.localPosition.x - GameController.minX + 0.25f;
        upDetectionLength = GameController.maxY - t.localPosition.y + 0.25f;
        downDetectionLength = t.localPosition.y - GameController.minY + 0.25f;
    }

    void Update()
    {
        if (state == State.Idle)
        {
            BoxCollider collider = GetComponent<BoxCollider>();
            if (castRay(t.position + new Vector3(collider.size.x / rayConstant, collider.size.y / rayConstant, 0), GameController.getDirectionVector3(GameController.Direction.Right), rightDetectionLength, LayerMask.GetMask("Player")) ||
                castRay(t.position + new Vector3(collider.size.x / rayConstant, -collider.size.y / rayConstant, 0), GameController.getDirectionVector3(GameController.Direction.Right), rightDetectionLength, LayerMask.GetMask("Player")) ||
                castRay(t.position + new Vector3(collider.size.x / rayConstant, 0), GameController.getDirectionVector3(GameController.Direction.Right), rightDetectionLength, LayerMask.GetMask("Player")))
            {
                StartCoroutine(Move(GameController.Direction.Right));
            }
            else if (castRay(t.position + new Vector3(-collider.size.x / rayConstant, +collider.size.y / rayConstant, 0), GameController.getDirectionVector3(GameController.Direction.Left), leftDetectionLength, LayerMask.GetMask("Player")) ||
                     castRay(t.position + new Vector3(-collider.size.x / rayConstant, -collider.size.y / rayConstant, 0), GameController.getDirectionVector3(GameController.Direction.Left), leftDetectionLength, LayerMask.GetMask("Player")) ||
                     castRay(t.position + new Vector3(-collider.size.x / rayConstant, 0), GameController.getDirectionVector3(GameController.Direction.Left), leftDetectionLength, LayerMask.GetMask("Player")))
            {
                StartCoroutine(Move(GameController.Direction.Left));
            }
            else if (castRay(t.position + new Vector3(-collider.size.x / rayConstant, collider.size.y / rayConstant, 0), GameController.getDirectionVector3(GameController.Direction.Up), upDetectionLength, LayerMask.GetMask("Player")) ||
                     castRay(t.position + new Vector3(collider.size.x / rayConstant, collider.size.y / rayConstant, 0), GameController.getDirectionVector3(GameController.Direction.Up), upDetectionLength, LayerMask.GetMask("Player")) ||
                     castRay(t.position + new Vector3(0, collider.size.y / rayConstant, 0), GameController.getDirectionVector3(GameController.Direction.Up), upDetectionLength, LayerMask.GetMask("Player")))
            {
                StartCoroutine(Move(GameController.Direction.Up));
            }
            else if (castRay(t.position + new Vector3(-collider.size.x / rayConstant, -collider.size.y / rayConstant, 0), GameController.getDirectionVector3(GameController.Direction.Down), downDetectionLength, LayerMask.GetMask("Player")) ||
                     castRay(t.position + new Vector3(collider.size.x / rayConstant, -collider.size.y / rayConstant, 0), GameController.getDirectionVector3(GameController.Direction.Down), downDetectionLength, LayerMask.GetMask("Player")) ||
                     castRay(t.position + new Vector3(0, -collider.size.y / rayConstant, 0), GameController.getDirectionVector3(GameController.Direction.Down), downDetectionLength, LayerMask.GetMask("Player")))
            {
                StartCoroutine(Move(GameController.Direction.Down));
            }
        }
    }

    void OnEnable()
    {
        if (t == null)
        {
            return;
        }
        active = true;
        t.position = initialPosition;
        state = State.Idle;
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "Player")
        {
            PlayerController pc = coll.gameObject.GetComponent<PlayerController>();
            pc.TakeDamage(1, getLinkDirection());
        }
        else if ((coll.tag == "enemy" || coll.tag == "shortwall") && state == State.Attacking)
        {
            state = State.Retracting;
        }
    }

    private IEnumerator Move(GameController.Direction direction)
    {
        if (state != State.Idle)
        {
            state = State.Idle;
            yield break;
        }

        //Attack
        state = State.Attacking;
        Vector3 directionVector = GameController.getDirectionVector3(direction);
        Vector3 distancenVector = getDistanceVector(direction);
        float moveDuration = distancenVector.magnitude / attackSpeed;
        float initial_time = Time.time;
        float progress = (Time.time - initial_time) / moveDuration;
        Vector3 initialPos = t.position;

        while (progress < 1.0f && state == State.Attacking)
        {
            progress = (Time.time - initial_time) / moveDuration;
            t.position = Vector3.Lerp(initialPos, initialPos + distancenVector, progress);
            yield return null;
        }

        //Retract
        moveDuration = (t.position - initialPosition).magnitude / retractSpeed;
        initial_time = Time.time;
        progress = (Time.time - initial_time) / moveDuration;
        initialPos = t.position;
        
        while (progress < 1.0f)
        {
            progress = (Time.time - initial_time) / moveDuration;
            t.position = Vector3.Lerp(initialPos, initialPosition, progress);
            yield return null;

        }

        yield return null;
        state = State.Idle;
    }

    private Vector3 getDistanceVector(GameController.Direction direction)
    {
        if (direction == GameController.Direction.Right)
        {
            return new Vector3(rightDetectionLength, 0, 0);
        }
        if (direction == GameController.Direction.Left)
        {
            return new Vector3(-leftDetectionLength, 0, 0);
        }
        if (direction == GameController.Direction.Up)
        {
            return new Vector3(0, upDetectionLength, 0);
        }
        return new Vector3(0, -downDetectionLength,0);
    }
}


