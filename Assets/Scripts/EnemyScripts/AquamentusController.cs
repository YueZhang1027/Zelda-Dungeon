using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AquamentusController : GroundEnemyController
{
    public int minRange;
    public int maxRange;
    public float fireDuration;
    public GameObject projectilePrefab;
    public AudioClip bossScream;
    public float fireCooldown;

    private int currentPos = 0;
    private Animator anim;
    private Transform playerTransform;
    private bool isShooting = false;
    private bool isCooldown = false;
    void Start()
    {
        t = GetComponent<Transform>();
        anim = GetComponent<Animator>();
        targetPos = t.position;
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        StartCoroutine(Hitstun());
    }

    protected override List<GameController.Direction> getAvailableDirections()
    {
        List<GameController.Direction> availableDirections = new List<GameController.Direction>();
        if (currentPos != minRange)
        {
            availableDirections.Add(GameController.Direction.Left);
        }
        if (currentPos != maxRange)
        {
            availableDirections.Add(GameController.Direction.Right);
        }
        if (Random.value < 0.75f)
        {
            availableDirections.Add(GameController.Direction.None);
        }
        if(availableDirections.Count == 0)
        {
            availableDirections.Add(GameController.Direction.None);
        }
        return availableDirections;
    }

    protected override void setTarget(GameController.Direction targetDirection)
    {
        //Debug.Log(GameController.getDirectionVector3(targetDirection));
        //Debug.Log(currentPos);
        targetPos = t.position + GameController.getDirectionVector3(targetDirection);
        currentDirection = targetDirection;
        if (targetDirection == GameController.Direction.Left)
        {
            currentPos--;
        } else if (targetDirection == GameController.Direction.Right)
        {
            currentPos++;
        }
    }

    protected override IEnumerator DoMove(Vector3 targetPos)
    {
        if (isShooting)
        {
            yield break;
        }
        isShooting = true;
        if (!isCooldown && targetPos == t.position)
        {
            AudioSource.PlayClipAtPoint(bossScream, Camera.main.transform.position);
            anim.SetBool("isFire", true);
            isCooldown = true;
            FireProjectile();
            yield return new WaitForSeconds(fireDuration);
            anim.SetBool("isFire", false);
            moving = false;
            isShooting = false;
            yield break;
        }
        else
        {
            yield return StartCoroutine(mover.Move(moveDuration, waitDuration, targetPos, currentDirection));
            moving = false;
        }
        isShooting = false;
    }

    IEnumerator DoCooldown()
    {
        yield return new WaitForSeconds(fireCooldown);
        isCooldown = false;
    }

    void FireProjectile()
    {
        StartCoroutine(DoCooldown());
        Vector3 directionVector = playerTransform.position - t.position;
        GameObject g = Instantiate(projectilePrefab, Vector3.zero, Quaternion.identity);
        g.GetComponent<AquamentusProjectileController>().SetDirection(directionVector);
        g.transform.SetParent(t, false);
        g.SetActive(true);
        g = Instantiate(projectilePrefab, Vector3.zero, Quaternion.identity);
        g.GetComponent<AquamentusProjectileController>().SetDirection(Quaternion.AngleAxis(30, Vector3.forward) * directionVector);
        g.transform.SetParent(t, false);
        g.SetActive(true);
        g = Instantiate(projectilePrefab, Vector3.zero, Quaternion.identity);
        g.GetComponent<AquamentusProjectileController>().SetDirection(Quaternion.AngleAxis(-30, Vector3.forward) * directionVector);
        g.transform.SetParent(t, false);
        g.SetActive(true);
    }
}
