using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularEnemyController : EnemyController
{
    // Start is called before the first frame update
    public AudioClip enemyHit;
    public AudioClip enemyDead;
    public int damage = 1;
    public int hp = 3;
    public float stunDuration;
    public Material flash;
    public Material noFlash;
    public bool small;

    void Start()
    {
        t = GetComponent<Transform>();
    }

    public override void takeDamage(int amount, bool onlyStun = false)
    {
        if (!small && onlyStun)
        {
            AudioSource.PlayClipAtPoint(enemyHit, Camera.main.transform.position);
            StartCoroutine(Hitstun(3f));
            return;
        }
        hp -= amount;
        if (hp <= 0)
        {
            AudioSource.PlayClipAtPoint(enemyDead, Camera.main.transform.position);
            DestroyEnemy();
            this.gameObject.SetActive(false);
        }
        else
        {
            AudioSource.PlayClipAtPoint(enemyHit, Camera.main.transform.position);
            StartCoroutine(Hitstun());
        }
    }

    protected IEnumerator Hitstun(float hitstunMultiplier = 1)
    {
        active = false; 
        GetComponent<SpriteRenderer>().material = flash;
        yield return new WaitForSeconds(stunDuration * hitstunMultiplier);
        GetComponent<SpriteRenderer>().material = noFlash;
        active = true;
    }

}
