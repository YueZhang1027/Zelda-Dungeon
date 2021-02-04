using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeeseMovement : EnemyMovement
{
    public float flySpeed;
    public AnimationCurve start;
    public AnimationCurve end;

    private Transform t;
    private Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        t = GetComponent<Transform>();
    }

    public override IEnumerator Move(float moveDuration, float waitDuration, Vector3 directionVector, GameController.Direction direction = GameController.Direction.None)
    {
        if (inProgress)
        {
            yield break;
        }
        if (directionVector == Vector3.zero)
        {
            animator.speed = 0f;
            yield return new WaitForSeconds(waitDuration);
            yield break;
        }
        animator.speed = 1f;

        t = GetComponent<Transform>();
        inProgress = true;
        float currentFlySpeed = flySpeed;
        float randomDuration = Random.Range(0.2f, moveDuration);
        float initial_time = Time.time;
        float progress = (Time.time - initial_time) / randomDuration;

        while (progress < 1.0f)
        {
            if(t.localPosition.x < GameController.minX || t.localPosition.x > GameController.maxX)
            {
                directionVector = new Vector3(-directionVector.x, directionVector.y, directionVector.z);
            }
            if (t.localPosition.y < GameController.minY || t.localPosition.y > GameController.maxY)
            {
                directionVector = new Vector3(directionVector.x, -directionVector.y, directionVector.z);
            }

            progress = (Time.time - initial_time) / randomDuration;

            if ( direction == GameController.Direction.Up)
            {
                currentFlySpeed = start.Evaluate(progress) * flySpeed;
            } else if (direction == GameController.Direction.Down)
            {
                currentFlySpeed = end.Evaluate(progress) * flySpeed;
            }
            
            t.localPosition += directionVector * currentFlySpeed;

            yield return null;
        }
        inProgress = false;
    }

    //private void CorrectPos()
    //{
    //    if (t.localPosition.x < minX)
    //    {
    //        t.localPosition = new Vector3(minX, t.localPosition.y, t.localPosition.x);
    //    }
    //    if (t.localPosition.x > maxX)
    //    {
    //        t.localPosition = new Vector3(maxX, t.localPosition.y, t.localPosition.x);
    //    }
    //    if (t.localPosition.y < minY)
    //    {
    //        t.localPosition = new Vector3(t.localPosition.x, minY, t.localPosition.x);
    //    }
    //    if (t.localPosition.y > maxY)
    //    {
    //        t.localPosition = new Vector3(t.localPosition.x, maxY, t.localPosition.x);
    //    }
    //}

}
