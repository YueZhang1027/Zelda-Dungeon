using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallmasterController : RegularEnemyController
{
    public float emergeDuration;
    public float followDuration;

    private bool caughtPlayer = false;
    private int maxHp;

    void Awake()
    {
        maxHp = hp;
    }
    void Start()
    {
        t = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "Player")
        {
            
            PlayerController pc = coll.gameObject.GetComponent<PlayerController>();
            if (coll.gameObject.GetComponent<Inventory>() != null && coll.gameObject.GetComponent<Inventory>().isGodMode)
            {
                return;
            }
            caughtPlayer = true;
            pc.state = PlayerController.PlayerStates.Unresponsive;
            coll.transform.position = t.position;
        }
    }

    void OnTriggerStay(Collider coll)
    {
        if (coll.tag == "Player" && !caughtPlayer)
        {
            PlayerController pc = coll.gameObject.GetComponent<PlayerController>();
            if (coll.gameObject.GetComponent<Inventory>() != null && coll.gameObject.GetComponent<Inventory>().isGodMode)
            {
                return;
            }
            caughtPlayer = true;
            pc.state = PlayerController.PlayerStates.Unresponsive;
            coll.transform.position = t.position;
        }
        if (caughtPlayer && coll.tag == "Player")
        {
            coll.transform.position = t.position;
        }
    }

    void OnEnable()
    {
        caughtPlayer = false;
        hp = maxHp;
    }

    void OnDisable()
    {
        if (t != null)
        {
            t.position = new Vector3(-3, -3, -3);
        }
    }


    protected override void DestroyEnemy()
    {

    }

    public IEnumerator Move(Vector3 emergeVector, Vector3 followVector)
    {
        this.gameObject.SetActive(true);
        t = GetComponent<Transform>();
        float initial_time = Time.time;
        float progress = (Time.time - initial_time) / emergeDuration;
        Vector3 initialPos = t.position;

        while (progress < 1.0f)
        {
            progress = (Time.time - initial_time) / emergeDuration;
            t.position = Vector3.Lerp(initialPos, initialPos + emergeVector, progress);

            yield return null;
        }

        initial_time = Time.time;
        progress = (Time.time - initial_time) / followDuration;
        initialPos = t.position;

        while (progress < 1.0f)
        {
            progress = (Time.time - initial_time) / followDuration;
            t.position = Vector3.Lerp(initialPos, initialPos + followVector, progress);

            yield return null;
        }

        initial_time = Time.time;
        progress = (Time.time - initial_time) / emergeDuration;
        initialPos = t.position;
        while (progress < 1.0f)
        {
            progress = (Time.time - initial_time) / emergeDuration;
            t.position = Vector3.Lerp(initialPos, initialPos - emergeVector, progress);

            yield return null;
        }

        if (caughtPlayer)
        {
            GameController.instance.SendToStart();
        }
        this.gameObject.SetActive(false);
    }
}
