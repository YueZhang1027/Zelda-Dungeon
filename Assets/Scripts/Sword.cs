using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public static Sword instance;
    public AudioClip sword_attack_sound_clip;
    BoxCollider boxCol;
    // Start is called before the first frame update

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
        gameObject.SetActive(false);
    }

    public void Attack(GameController.Direction direction)
    {
        //set sword to be active
        gameObject.SetActive(true);
        AudioSource.PlayClipAtPoint(sword_attack_sound_clip, Camera.main.transform.position);

        //adjust relative position to player in different directions
        switch (direction)
        {
            case GameController.Direction.Up:
                boxCol.size = new Vector3(0.27f, 0.72f, 1.0f);
                boxCol.center = new Vector3(-0.1f, 0.53f, 0.0f);
                break;
            case GameController.Direction.Down:
                boxCol.size = new Vector3(0.27f, 0.72f, 1.0f);
                boxCol.center = new Vector3(0.02f, -0.5f, 0.0f);
                break;
            case GameController.Direction.Left:
                boxCol.size = new Vector3(0.72f, 0.27f, 1.0f);
                boxCol.center = new Vector3(-0.52f, -0.06f, 0.0f);
                break;
            case GameController.Direction.Right:
                boxCol.size = new Vector3(0.72f, 0.27f, 1.0f);
                boxCol.center = new Vector3(0.51f, 0.06f, 0.0f);
                break;

        }
    }

    // void OnCollisionEnter(Collision collision)
    // {
    //     // destroy enemy collided
    //     // TODO: check health data of enemy
    //     if (collision.collider.gameObject.CompareTag("enemy"))
    //     {
    //         Destroy(collision.collider.gameObject);
    //     }
    // }

    void OnTriggerEnter(Collider collider)
    {   
        Collector collector = transform.parent.gameObject.GetComponent<Collector>();
        collector.collect(collider.gameObject);
        if(collider.gameObject.CompareTag("enemy"))
        {
            collider.gameObject.GetComponent<EnemyController>().takeDamage(1);
        }
        
    }
}
