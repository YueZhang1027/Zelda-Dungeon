using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyMovement : MonoBehaviour
{
    public bool inProgress = false;
    virtual public IEnumerator Move(float moveDuration, float waitDuration, Vector3 vector, GameController.Direction direction = GameController.Direction.None)
    {
        Debug.Log("DONTCALL");
        yield return null;
    }
}
