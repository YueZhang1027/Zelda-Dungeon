using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingSword : MonoBehaviour
{
    // Start is called before the first frame update
    public static ThrowingSword instance;
    public AudioClip sword_shoot_sound_clip;
    public float throwingSpeed = 10.0f;

    Transform trans;
    GameController.Direction throwingDirection;
    BoxCollider boxCol;
    Animator animator;
    // Start is called before the first frame update
    Rigidbody rb;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        boxCol = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        trans = GetComponent<Transform>();
        gameObject.SetActive(false);
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    public void Attack(GameController.Direction direction)
    {
        throwingDirection = direction;
        //set sword to be active
        gameObject.SetActive(true);
        AudioSource.PlayClipAtPoint(sword_shoot_sound_clip, Camera.main.transform.position);


        Transform playerTransform = transform.parent.gameObject.GetComponent<Transform>();
        boxCol.center = Vector3.zero;
        switch(direction)
        {
            case GameController.Direction.Up:
                boxCol.size = new Vector3(0.46f, 1.0f, 1.0f);
                trans.position = playerTransform.position + new Vector3(0.0f, 1.0f, 0.0f);
            break;
            case GameController.Direction.Down:
                boxCol.size = new Vector3(0.46f, 1.0f, 1.0f);
                trans.position = playerTransform.position + new Vector3(0.0f, -1.0f, 0.0f);
            break;
            case GameController.Direction.Right:
                boxCol.size = new Vector3(1.0f, 0.46f, 1.0f);
                trans.position = playerTransform.position + new Vector3(1.0f, 0.0f, 0.0f);
            break;
            case GameController.Direction.Left:
                boxCol.size = new Vector3(1.0f, 0.46f, 1.0f);
                trans.position = playerTransform.position + new Vector3(-1.0f, 0.0f, 0.0f);
            break;
        }
        StartCoroutine(setActive());
        //animator.SetBool("isActive", true);
    }

    public IEnumerator setActive()
    {
        yield return 0;
        yield return 0;
        animator.SetBool("isActive", true);
    }

    void Update()
    {
        switch(throwingDirection){
        case GameController.Direction.Up:
            rb.velocity = new Vector3(0.0f, throwingSpeed, 0.0f);
        break;
        case GameController.Direction.Down:
            rb.velocity = new Vector3(0.0f, -throwingSpeed, 0.0f);
        break;
        case GameController.Direction.Right:
            rb.velocity = new Vector3(throwingSpeed, 0.0f, 0.0f);
        break;
        case GameController.Direction.Left:
            rb.velocity = new Vector3(-throwingSpeed, 0.0f, 0.0f);
        break;
        }
    }


    void OnTriggerEnter(Collider collider)
    {
        Collector collector = transform.parent.gameObject.GetComponent<Collector>();
        if (!collider.gameObject.CompareTag("triforce"))
        {
            collector.collect(collider.gameObject);
        }
        if(collider.gameObject.CompareTag("enemy") || collider.gameObject.CompareTag("wall") || collider.gameObject.CompareTag("upperwall") || collider.gameObject.CompareTag("door"))
        {
            if (collider.gameObject.CompareTag("enemy"))
            {
                collider.gameObject.GetComponent<EnemyController>().takeDamage(1);
            }
            animator.SetBool("isActive", false);
            WeaponController weaponController = transform.parent.gameObject.GetComponent<WeaponController>();
            weaponController.SwordThrowEnd();
        }
    }
}
