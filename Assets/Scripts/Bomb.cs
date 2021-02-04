using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public static Bomb instance;
    public AudioClip bomb_set_clip;
    public AudioClip bomb_explode_clip;
    public GameObject bombExplosionPrefab;

    Transform trans;
    Transform playerTransform;
    //SphereCollider sphereCol;
    private bool exploding = false;

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

    // Start is called before the first frame update
    void Start()
    {
        //sphereCol = GetComponent<SphereCollider>();
        trans = GetComponent<Transform>();
        playerTransform = transform.parent.gameObject.GetComponent<Transform>();
        gameObject.SetActive(false);
    }

    public void Attack(GameController.Direction direction)
    {
        //set sword to be active
        gameObject.SetActive(true);
        StartCoroutine(SetBomb());

        switch(direction)
        {
            case GameController.Direction.Up:
                trans.position = playerTransform.position + new Vector3(0.0f, 1f, 0.0f);
            break;
            case GameController.Direction.Down:
                trans.position = playerTransform.position + new Vector3(0.0f, -1f, 0.0f);
            break;
            case GameController.Direction.Left:
                trans.position = playerTransform.position + new Vector3(-1f, 0.0f, 0.0f);
            break;
            case GameController.Direction.Right:
                trans.position = playerTransform.position + new Vector3(1f, 0.0f, 0.0f);
            break;
        }
        
    }

    IEnumerator SetBomb()
    {
        AudioSource.PlayClipAtPoint(bomb_set_clip, Camera.main.transform.position);
        yield return new WaitForSeconds(1f);
        exploding = true;
        AudioSource.PlayClipAtPoint(bomb_explode_clip, Camera.main.transform.position);
        Instantiate(bombExplosionPrefab, trans.position + new Vector3(-1, 1, 0), Quaternion.identity);
        Instantiate(bombExplosionPrefab, trans.position + new Vector3(-1, 0, 0), Quaternion.identity);
        Instantiate(bombExplosionPrefab, trans.position + new Vector3(-1, -1, 0), Quaternion.identity);

        Instantiate(bombExplosionPrefab, trans.position + new Vector3(0, 1, 0), Quaternion.identity);
        Instantiate(bombExplosionPrefab, trans.position + new Vector3(0, -1, 0), Quaternion.identity);

        Instantiate(bombExplosionPrefab, trans.position + new Vector3(1, 1, 0), Quaternion.identity);
        Instantiate(bombExplosionPrefab, trans.position + new Vector3(1, 0, 0), Quaternion.identity);
        Instantiate(bombExplosionPrefab, trans.position + new Vector3(1, -1, 0), Quaternion.identity);
        yield return null;
        exploding = false;
        WeaponController weaponController = transform.parent.gameObject.GetComponent<WeaponController>();
        weaponController.BombAttackEnd();
    }

    void Explosion()
    {
        AudioSource.PlayClipAtPoint(bomb_explode_clip, Camera.main.transform.position);
        WeaponController weaponController = transform.parent.gameObject.GetComponent<WeaponController>();
        weaponController.BombAttackEnd();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider collider)
    {
       // Debug.Log(exploding);
        if(collider.gameObject.CompareTag("enemy") && exploding)
        {
            collider.gameObject.GetComponent<EnemyController>().takeDamage(3);
        }
    }
}
