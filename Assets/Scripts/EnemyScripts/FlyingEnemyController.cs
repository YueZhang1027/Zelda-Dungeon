using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyController : RegularEnemyController
{
    public float moveDuration;
    public float waitDuration;
    public int maxPatterns;
    public EnemyMovement mover;

    private bool moving = false;
    private int currentPattern;
    private Vector3 initialPos;

    void Start()
    {
        t = GetComponent<Transform>();
        currentPattern = 0;
        initialPos = t.position;
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
        t.position = initialPos;
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
            if (currentPattern == maxPatterns)
            {
                currentPattern = 0;
                StartCoroutine(DoMove(Vector3.zero));
            }
            else {
                if (currentPattern == maxPatterns - 1)
                {
                    StartCoroutine(DoMove(getRandomDirection(), GameController.Direction.Down));
                } else if (currentPattern == 0)
                {
                    StartCoroutine(DoMove(getRandomDirection(), GameController.Direction.Up));
                } else
                {
                    StartCoroutine(DoMove(getRandomDirection()));
                }
                currentPattern++;
            }
            moving = true;
        }
    }

    static Vector3 getRandomDirection()
    {
        Vector3 newDirection = new Vector3(0, 0, 0);
        while (newDirection == Vector3.zero)
        {
            if (Random.value < 0.5f)
            {
                if (Random.value < 0.5f)
                {
                    newDirection.x = 1;
                }
                else
                {
                    newDirection.x = -1;
                }
            }
            if (Random.value < 0.5f)
            {
                if (Random.value < 0.5f)
                {
                    newDirection.y = 1;
                }
                else
                {
                    newDirection.y = -1;
                }
            }
        }
        return newDirection;
    }


    IEnumerator DoMove(Vector3 direction, GameController.Direction animState = GameController.Direction.None)
    {
        yield return StartCoroutine(mover.Move(moveDuration, waitDuration, direction, animState));
        moving = false;
    }


}
