using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GelMovement : EnemyMovement
{
    public float maxWaitDuration;

    private Transform t;
    // Start is called before the first frame update
    void Start()
    {
        t = GetComponent<Transform>();
    }

    public override IEnumerator Move(float moveDuration, float waitDuration, Vector3 targetPos, GameController.Direction direction = GameController.Direction.None)
    {
        if (inProgress)
        {
            yield break;
        }
        inProgress = true;
        float initial_time = Time.time;
        float progress = (Time.time - initial_time) / moveDuration;
        Vector3 initialPos = t.position;

        while (progress < 1.0f)
        {
            progress = (Time.time - initial_time) / moveDuration;
            t.position = Vector3.Lerp(initialPos, targetPos, progress);

            yield return null;
        }

        yield return new WaitForSeconds(Random.Range(0, waitDuration));
        inProgress = false;
    }
}
