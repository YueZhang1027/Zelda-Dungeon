using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExplosionController : MonoBehaviour
{
    public float duration;
    void Start()
    {
        StartCoroutine(Kill());
    }

    IEnumerator Kill()
    {
        yield return new WaitForSeconds(duration);
        Destroy(this.gameObject);
    }


}
