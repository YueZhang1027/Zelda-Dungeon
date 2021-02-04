using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AquamentusProjectileController : InvincibleEnemyController
{
    public float speed;
    public float lifetime;
    private Vector3 directionVector;
    private Vector3 speedVector;
    // Start is called before the first frame update
    void Start()
    {
        t = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lifetime <= 0)
        {
            Destroy(this.gameObject);
        }
        if (directionVector != null)
        {
            lifetime -= Time.deltaTime;
            t.position += speedVector;
        }
    }

    public void SetDirection(Vector3 vector)
    {
        directionVector = vector.normalized;
        speedVector = directionVector * speed;
    }
}
