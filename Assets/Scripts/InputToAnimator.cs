using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputToAnimator : MonoBehaviour
{
    public float swordTime = 0.5f;
    public float arrowTime = 0.25f;
    public float bombTime = 0.25f;

    ArrowKeyMovement movement;
    Rigidbody rb;
    Animator animator;
    PlayerController playerController;
    WeaponController weaponController;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        movement = GetComponent<ArrowKeyMovement>();
        playerController = GetComponent<PlayerController>();
        weaponController = GetComponent<WeaponController>();
        animator.SetLayerWeight(2, 0);

    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.state == PlayerController.PlayerStates.Unresponsive)
        {
            animator.speed = 0.0f;
            return;
        }
        animator.SetFloat("horizontal_input", rb.velocity.x);
        animator.SetFloat("vertical_input", rb.velocity.y);
        animator.SetBool("A_pressed", Input.GetButtonDown("Fire1"));
        animator.SetInteger("Direction", (int) movement.GetDirection());
        if (Input.GetButtonDown("Menu"))
        {
            GameController.LoadMenu();
        }
    
        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0 && !Input.GetButtonDown("Fire1"))
        {
            animator.speed = 0.0f;
            //animator.SetLayerWeight(1, 0);
        }
        else
        {
            
            animator.speed = 1.0f;
            //animator.SetLayerWeight(1, 1);
        }
    }

    public IEnumerator AnimateSwordAttack(bool isThrow, GameController.Direction direction)
    {
        playerController.state = PlayerController.PlayerStates.Attack;
        // TODO: fix player position when acting animation
        animator.SetLayerWeight(2, 1);
        yield return new WaitForSeconds(swordTime);
        playerController.state = PlayerController.PlayerStates.Idle;
        animator.SetLayerWeight(2, 0);
        
        weaponController.SwordAttackEnd();
        if (isThrow)
        {
            ThrowingSword.instance.Attack(direction);
        }
    }

    public IEnumerator AnimateBWeaponAttack(GameController.Direction direction, WeaponController.WeaponSpecies weapon)
    {
        playerController.state = PlayerController.PlayerStates.Attack;
        animator.SetLayerWeight(3, 1);
        yield return new WaitForSeconds(arrowTime);
        playerController.state = PlayerController.PlayerStates.Idle;
        animator.SetLayerWeight(3, 0);
        
        // weaponController.BWeaponAttackEnd();
        switch(weapon)
        {
            case WeaponController.WeaponSpecies.Bow: 
                Bow.instance.Attack(direction);
            break;
            case WeaponController.WeaponSpecies.Boomerang:
                weaponController.boomerang.Attack(direction);
            break;
            case WeaponController.WeaponSpecies.PortalGun:
                PortalGun.instance.Attack(direction);
            break;
        }
    }

    public IEnumerator AnimateBombAttack(GameController.Direction direction)
    {
        playerController.state = PlayerController.PlayerStates.Attack;
        animator.SetLayerWeight(3, 1);
        yield return new WaitForSeconds(bombTime);
        playerController.state = PlayerController.PlayerStates.Idle;
        animator.SetLayerWeight(3, 0);

        Bomb.instance.Attack(direction);
    }
}
