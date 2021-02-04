using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoriyaMovement : EnemyMovement
{
    public Boomerang boomerangPrefab;
    public float attackProbability;

    private Boomerang boomerang;
    private Transform t;
    private Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        t = GetComponent<Transform>();
        boomerang = Instantiate(boomerangPrefab, t.position, Quaternion.identity);
        boomerang.transform.SetParent(t, false);
        boomerang.GetComponent<InputToSwordAnimator>().enabled = false;
        boomerang.isPlayer = false;
        animator.speed = 1f;
        boomerang.gameObject.SetActive(true);
        
    }

    public override IEnumerator Move(float moveDuration, float waitDuration, Vector3 targetPos, GameController.Direction direction = GameController.Direction.None)
    {
        if (inProgress)
        {
            yield break;
        }
        inProgress = true;
        animator.SetInteger("Direction", (int)direction);
        boomerang.GetComponent<Animator>().SetInteger("Direction", (int)direction);
        float initial_time = Time.time;
        float progress = (Time.time - initial_time) / moveDuration;
        Vector3 initialPos = t.position;

        while (progress < 1.0f)
        {
            progress = (Time.time - initial_time) / moveDuration;
            t.position = Vector3.Lerp(initialPos, targetPos, progress);

            yield return null;
        }
        yield return new WaitForSeconds(waitDuration);
        if (Random.value <= attackProbability)
        {
            animator.speed = 0f;
            yield return StartCoroutine(AnimateAttack(direction));
            animator.speed = 1f;
        }
        inProgress = false;
    }

    public IEnumerator AnimateAttack(GameController.Direction direction)
    {
        yield return new WaitForSeconds(1f);
        boomerang.Attack(direction);
        boomerang.transform.localScale = new Vector3(0.4f, 0.6f, 1);
    }
}
