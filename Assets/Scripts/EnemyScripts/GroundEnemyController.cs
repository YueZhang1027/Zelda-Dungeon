using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemyController : RegularEnemyController
{
    public float moveDuration;
    public float waitDuration;
    public EnemyMovement mover;
    public bool followsLink;
    public int currentDirectionProbabilityMultiplier = 1;

    protected Vector3 targetPos;
    protected float wallDetectionLength = 1;
    protected bool moving = false;
    protected GameController.Direction currentDirection;
    
    void Start()
    {
        t = GetComponent<Transform>();
        targetPos = t.position;
        StartCoroutine(Hitstun());
    }

    void OnEnable()
    {
        if (t == null)
        {
            return;
        }
        moving = false;
        active = true;
        t.position = targetPos;
        mover.inProgress = false;
        StartCoroutine(Hitstun());
    }

    void Update()
    {
        if (!active)
        {
            return;
        }
        if (!moving)
        {
            List<GameController.Direction> directions = getAvailableDirections();
            if (followsLink)
            {
                GameController.Direction linkDirection = getLinkDirection();
                if (linkDirection != GameController.Direction.None && directions.Contains(linkDirection))
                {
                    setTarget(linkDirection);
                }
                else
                {
                    setTarget(directions[(int)Random.Range(0, directions.Count - 0.01f)]);
                }
            }
            else
            {
                setTarget(directions[(int)Random.Range(0, directions.Count - 0.01f)]);
            }
            moving = true;
            StartCoroutine(DoMove(targetPos));
        }
    }

    protected virtual void setTarget(GameController.Direction targetDirection)
    {
        targetPos = t.position + GameController.getDirectionVector3(targetDirection);
        currentDirection = targetDirection;
    }

    protected virtual List<GameController.Direction> getAvailableDirections()
    {
        List<GameController.Direction> availableDirections = new List<GameController.Direction>();
        if (t.localPosition.y != 8.0f && !castRay(t.position, GameController.getDirectionVector3(GameController.Direction.Up), wallDetectionLength, LayerMask.GetMask("Walls")))
        {
            addDirection(GameController.Direction.Up, ref availableDirections);
        }
        if (t.localPosition.y != 2.0f && !castRay(t.position, GameController.getDirectionVector3(GameController.Direction.Down), wallDetectionLength, LayerMask.GetMask("Walls")))
        {
            addDirection(GameController.Direction.Down, ref availableDirections);
        }
        if (t.localPosition.x != 2.0f && !castRay(t.position, GameController.getDirectionVector3(GameController.Direction.Left), wallDetectionLength, LayerMask.GetMask("Walls")))
        {
            addDirection(GameController.Direction.Left, ref availableDirections);
        }
        if (t.localPosition.x != 13.0f && !castRay(t.position, GameController.getDirectionVector3(GameController.Direction.Right), wallDetectionLength, LayerMask.GetMask("Walls")))
        {
            addDirection(GameController.Direction.Right, ref availableDirections);
        }
        return availableDirections;
    }

    void addDirection(GameController.Direction direction, ref List<GameController.Direction> list)
    {
        if (currentDirection == direction)
        {
            for (int i = 0; i < currentDirectionProbabilityMultiplier; ++i)
            {
                list.Add(direction);
            }
        }
        else
        {   
            list.Add(direction);
        }
    }

    protected virtual IEnumerator DoMove(Vector3 targetPos)
    {
        yield return StartCoroutine(mover.Move(moveDuration, waitDuration, targetPos, currentDirection));
        moving = false;
    }
}
