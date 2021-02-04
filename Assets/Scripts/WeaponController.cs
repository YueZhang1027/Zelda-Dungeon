using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public enum WeaponSpecies
    {   
        None,
        Bow,
        Boomerang,
        PortalGun
    }

    public bool[] obtainBWeapon;

    public bool SwordAttacking = false;
    public bool SwordThrowing = false;
    public bool BWeaponAttacking = false;
    public bool BombAttacking = false;
    public Boomerang boomerang;

    public AudioClip bow_sound_clip;

    //Switching Weapon for B key
    public WeaponSpecies weaponSpecies;
    Animator animator;
    ArrowKeyMovement movement;
    InputToAnimator animatorInput;
    PlayerController playerController;
    Inventory inventory;
    public bool portalGunAcquired;
    // Start is called before the first frame update
    void Start()
    {
        obtainBWeapon = new bool[4]{true, true, true, true};

        weaponSpecies = WeaponSpecies.Boomerang;
        
        animator = GetComponent<Animator>();
        movement = GetComponent<ArrowKeyMovement>();
        animatorInput = GetComponent<InputToAnimator>();
        playerController = GetComponent<PlayerController>();
        inventory = GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        //TODO: add throw mechanic when player with full health
        if (playerController.state == PlayerController.PlayerStates.Idle && GameController.instance.gameState == GameController.GameStates.Play)
        {
            if (Input.GetButtonDown("Switch"))
            {
                // Debug.Log("Switching Weapon.");
                for (int i = 1; i < 5; i++)
                {   
                    int newWeaponIndex = ((int)weaponSpecies + i) % 4;
                    if (obtainBWeapon[newWeaponIndex]){
                        Debug.Log(newWeaponIndex);
                        weaponSpecies = (WeaponSpecies) newWeaponIndex;
                        break;
                    }
                }
            }

            if (Input.GetButtonDown("Fire1"))
            {
                GameController.Direction direction = movement.GetDirection();
                if(!SwordThrowing && playerController.hp == playerController.maxHp)
                {
                    //Throw sword
                    //Debug.Log("Sword throwing!");
                    SwordThrowing = true;
                    //ThrowingSword.instance.Attack(direction);
                    StartCoroutine(animatorInput.AnimateSwordAttack(true, direction));

                }
                else if(!SwordAttacking)
                {
                    //Attack sword
                    //Debug.Log("Sword attacking!");
                    Sword.instance.Attack(direction);
                    StartCoroutine(animatorInput.AnimateSwordAttack(false, direction));
                }
            }
            else if (!BWeaponAttacking && Input.GetButtonDown("Fire2"))
            {
                GameController.Direction direction = movement.GetDirection();
                switch(weaponSpecies)
                {
                    case WeaponSpecies.Bow:
                        if(inventory.GetRupees() <= 0 && !inventory.isGodMode)
                        {
                            return;
                        }
                        BWeaponAttacking = true;
                        if (!inventory.isGodMode)
                        {
                            inventory.AddRupees(-1);
                        }
                        //Debug.Log("Arrow attacking!");
                        AudioSource.PlayClipAtPoint(bow_sound_clip, Camera.main.transform.position);
                        StartCoroutine(animatorInput.AnimateBWeaponAttack(direction, WeaponSpecies.Bow));
                    break;

                    case WeaponSpecies.Boomerang:
                        //Debug.Log("Boomerang attacking!");
                        BWeaponAttacking = true;
                        AudioSource.PlayClipAtPoint(bow_sound_clip, Camera.main.transform.position);
                        StartCoroutine(animatorInput.AnimateBWeaponAttack(direction, WeaponSpecies.Boomerang));
                    break;

                    // Mark
                    case WeaponSpecies.PortalGun:
                        if (!portalGunAcquired)
                        {
                            return;
                        }
                        BWeaponAttacking = true;
                        StartCoroutine(animatorInput.AnimateBWeaponAttack(direction, WeaponSpecies.PortalGun));
                    break;
                }
            }
            else if (!BombAttacking && Input.GetButtonDown("Fire3"))
            {
                if (inventory.GetBombs() <= 0 && !inventory.isGodMode)
                {
                    return;
                }
                BombAttacking = true;
                if (!inventory.isGodMode)
                {
                    inventory.AddBombs(-1);
                }
                GameController.Direction direction = movement.GetDirection();
                StartCoroutine(animatorInput.AnimateBombAttack(direction));
            }
        }
        
    }

    public void SwordAttackEnd()
    {
        SwordAttacking = false;
        Sword.instance.gameObject.SetActive(false);
    }

    public void SwordThrowEnd()
    {
        SwordThrowing = false;
        ThrowingSword.instance.gameObject.SetActive(false);
    }
    public void BombAttackEnd()
    {
        BombAttacking = false;
        Bomb.instance.gameObject.SetActive(false);
    }

    public void PortalThrowEnd()
    {

    }

    public void DestroyPortals()
    {
        PortalGun.instance.DestroyPortals();
    }

    public void BWeaponAttackEnd()
    {
        BWeaponAttacking = false;
        switch(weaponSpecies)
        {
            case WeaponSpecies.Bow:
                Bow.instance.gameObject.SetActive(false);
            break;
            case WeaponSpecies.Boomerang:
                boomerang.gameObject.SetActive(false);
            break;
            case WeaponSpecies.PortalGun:
                PortalGun.instance.gameObject.SetActive(false);
            break;
        }
    }


    public void collectWeapon(WeaponSpecies weapon)
    {

    }

}
