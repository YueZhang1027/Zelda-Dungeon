using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum PlayerStates
    {
        Idle,
        Attack,
        Unresponsive
    }

    public int maxHp = 6;
    public int hp;
    public float hitstunTime = 0.2f;
    public float invincibleTimeAfterHitstun = 0.2f;
    public float knockBackForce = 2000;
    public PlayerStates state;

    public AudioClip death_sound_clip;
    public AudioClip hurt_sound_clip;
    public Material flash;
    public Material noFlash;
    public Material dead;

    ArrowKeyMovement movement;
    Rigidbody rb;
    Inventory inventory;
    public bool isInvincible = false;
    
    // Start is called before the first frame update
    void Start()
    {
        //hp = maxHp;
        state = PlayerStates.Idle;
        movement = GetComponent<ArrowKeyMovement>();
        rb = GetComponent<Rigidbody>();
        inventory = GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inventory.isGodMode)
        {
            hp = maxHp;
        }
    }

    public void TakeDamage(int Damage, GameController.Direction direction)
    {
       if (state != PlayerController.PlayerStates.Unresponsive && !inventory.isGodMode && !isInvincible)
        {
            hp -= Damage;
            Debug.Log(hp);
            
            if (hp == 0)
            {
                state =  PlayerController.PlayerStates.Unresponsive;
                AudioSource audioSource = Camera.main.GetComponent<AudioSource>();
                audioSource.Stop();
                AudioSource.PlayClipAtPoint(death_sound_clip, Camera.main.transform.position);
                StartCoroutine(restart());
            } else if (hp < 0) {
                state = PlayerController.PlayerStates.Unresponsive;
                rb.velocity = Vector3.zero;
            } else
            {
                StartCoroutine(Hitstun(direction));
            }
        }
        
    }

    IEnumerator restart()
    {
        GetComponent<SpriteRenderer>().material = flash;
        yield return new WaitForSeconds(1f);
        GetComponent<SpriteRenderer>().material = dead;
        yield return new WaitForSeconds(0.5f);
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(0.5f);
        GameController.instance.RestartCurrentScene();
    }

    public void CollectHeart()
    {
        if(hp != maxHp){
            hp += 2;
            if (hp > maxHp)
            {
                hp = maxHp;
            }
        }
    }

    public void CollectBomb()
    {
        inventory.AddBombs(1);
    }

    IEnumerator Hitstun(GameController.Direction direction)
    {
        AudioSource.PlayClipAtPoint(hurt_sound_clip, Camera.main.transform.position);
        GetComponent<SpriteRenderer>().material = flash;
        state = PlayerController.PlayerStates.Unresponsive;
        isInvincible = true;

        //Physics.Raycast(position, transform.TransformDirection(direction), length, LayerMask.GetMask("Enemy"))

        rb.velocity = GameController.getDirectionVector3(direction) * 10;
        //addForce(knockBackForce / 3, direction);

        //yield return new WaitForSeconds(0.005f);
        //addForce(knockBackForce / 3, direction);
        
        //yield return new WaitForSeconds(0.005f);
        //addForce(knockBackForce / 3, direction);

        yield return new WaitForSeconds(hitstunTime);
        

        rb.velocity = GameController.getDirectionVector3(GameController.getReverseDirection(direction)) * 10;

        yield return new WaitForSeconds(0.015f);

        rb.velocity = Vector3.zero;
        state = PlayerController.PlayerStates.Idle;
        yield return new WaitForSeconds(invincibleTimeAfterHitstun);
        GetComponent<SpriteRenderer>().material = noFlash;
        isInvincible = false;

    }

    void addForce(float magnitude, GameController.Direction direction)
    {
        if (direction == GameController.Direction.None)
        {
            direction = movement.GetDirection();
        }
        if (direction == GameController.Direction.Down)
        {
            rb.AddForce(new Vector3(0, -magnitude, 0));
        }
        else if (direction == GameController.Direction.Up)
        {
            rb.AddForce(new Vector3(0, magnitude, 0));
        }
        else if (direction == GameController.Direction.Right)
        {
            rb.AddForce(new Vector3(magnitude, 0, 0));
        }
        else if (direction == GameController.Direction.Left)
        {
            rb.AddForce(new Vector3(-magnitude, 0, 0));
        }
    }
}
