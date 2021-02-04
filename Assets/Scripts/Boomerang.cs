using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : MonoBehaviour
{
    public float boomerangSpeed = 10.0f;
    public bool isPlayer = true;
    public GameController.Direction throwingDirection;
    // public AudioClip boomerang_sound_clip;

    bool comingBack = false;
    Vector3 startPosition;
    Transform trans;
    Transform playerTransform;
    BoxCollider boxCol;
    Animator animator;
    Rigidbody rb;

    void Awake()
    {
    }
    // Start is called before the first frame update
    void Start()
    {
        boxCol = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        trans = GetComponent<Transform>();
        gameObject.SetActive(false);
        animator = GetComponent<Animator>();

        playerTransform = transform.parent.gameObject.GetComponent<Transform>();
    }

    public void Attack(GameController.Direction direction)
    {
        throwingDirection = direction;
        //set sword to be active
        gameObject.SetActive(true);
        // AudioSource.PlayClipAtPoint(boomerang_sound_clip, Camera.main.transform.position);
        boxCol.center = Vector3.zero;
        

        switch(direction)
        {
            case GameController.Direction.Up:
                boxCol.size = new Vector3(0.25f, 0.17f, 1.0f);
                trans.localScale = new Vector3(3.0f, 2.0f, 1.0f);
                trans.position = playerTransform.position + new Vector3(0.0f, 1f, 0.0f);
            break;

            case GameController.Direction.Down:
                boxCol.size = new Vector3(0.25f, 0.17f, 1.0f);
                trans.localScale = new Vector3(3.0f, 2.0f, 1.0f);
                trans.position = playerTransform.position + new Vector3(0.0f, -1f, 0.0f);
            break;

            case GameController.Direction.Left:
                boxCol.size = new Vector3(0.17f, 0.25f, 1.0f);
                trans.localScale = new Vector3(2.0f, 3.0f, 1.0f);
                trans.position = playerTransform.position + new Vector3(-1f, 0.0f, 0.0f);
            break;

            case GameController.Direction.Right:
                boxCol.size = new Vector3(0.17f, 0.25f, 1.0f);
                trans.localScale = new Vector3(2.0f, 3.0f, 1.0f);
                trans.position = playerTransform.position + new Vector3(1f, 0.0f, 0.0f);
            break;
        }
        startPosition = trans.position;
        StartCoroutine(setActive());
    }

    public GameController.Direction getThrowDirection()
    {
        return throwingDirection;
    }

    public IEnumerator setActive()
    {
        yield return 0;
        yield return 0;
        yield return 0;
        animator.SetBool("isActive", true);
    }

    // Update is called once per frame
    void Update()
    {
        trans.Rotate(0, 0, 16);
        if (!comingBack) 
        {
            switch(throwingDirection)
            {
                case GameController.Direction.Up:
                    rb.velocity = new Vector3(0.0f, boomerangSpeed, 0.0f);
                break;
                case GameController.Direction.Down:
                    rb.velocity = new Vector3(0.0f, -boomerangSpeed, 0.0f);
                break;
                case GameController.Direction.Right:
                    rb.velocity = new Vector3(boomerangSpeed, 0.0f, 0.0f);
                break;
                case GameController.Direction.Left:
                    rb.velocity = new Vector3(-boomerangSpeed, 0.0f, 0.0f);
                break;
            }
            //TODO: check the range of the boomerang
            if (Vector3.Distance(rb.position, startPosition) >= 5.0f)
            {
                comingBack = true;
            }
        }
        else
        {
            rb.velocity = Vector3.Normalize(- rb.position + playerTransform.position) * boomerangSpeed;
        }

    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("enemy") || collider.gameObject.CompareTag("wall") || collider.gameObject.CompareTag("upperwall") || collider.gameObject.CompareTag("door"))
        {
            if (collider.gameObject.CompareTag("enemy"))
            {
                if (isPlayer)
                {
                    // rb.velocity = Vector3.zero;
                    comingBack = true;
                    collider.gameObject.GetComponent<EnemyController>().takeDamage(1, true);
                } 
                else
                {
                    if(this.transform.parent.transform == collider.transform)
                    {
                        comingBack = false;
                        gameObject.SetActive(false);
                    }
                }
            } 
            else
            {
                rb.velocity = Vector3.zero;
                comingBack = true;
            }

            animator.SetBool("isActive", false);
            WeaponController weaponController = transform.parent.gameObject.GetComponent<WeaponController>();
            // if (weaponController != null)
            // {
            //     weaponController.BWeaponAttackEnd();
            // }
            
        }

        if (collider.gameObject.CompareTag("Player"))
        {
            if (isPlayer)
            {
                comingBack = false;
                WeaponController weaponController = transform.parent.gameObject.GetComponent<WeaponController>();
                weaponController.BWeaponAttackEnd();
                // gameObject.SetActive(false);

            } else
            {
                //This is buggy for returning boomerang but good enough for now
                collider.gameObject.GetComponent<PlayerController>().TakeDamage(1, throwingDirection);
                rb.velocity = Vector3.zero;
                comingBack = true;
            }
        }

        if (isPlayer)
        {
            Collector collector = transform.parent.gameObject.GetComponent<Collector>();
            if (!collider.gameObject.CompareTag("triforce"))
            {
                collector.collect(collider.gameObject);
            }
        }
        
    }
}
