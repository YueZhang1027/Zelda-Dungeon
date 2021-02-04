using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    public AudioClip rupee_collection_sound_clip;
    public AudioClip key_collection_sound_clip;
    public AudioClip fanfare_Clip;

    // public AudioClip
    Inventory inventory;
    PlayerController playerController;
    WeaponController weaponController;

    void Start()
    {
        inventory = GetComponent<Inventory>();
        playerController = GetComponent<PlayerController>();
        weaponController = GetComponent<WeaponController>();
        if (inventory == null)
        {
            Debug.LogWarning("WARNING: Gameobject with a collector has no inventory to store things in!");
        }
    }

    void OnTriggerEnter(Collider coll)
    {
        GameObject object_collided_with = coll.gameObject;
        collect(object_collided_with);
    }

    public void collect(GameObject object_collided_with)
    {
        if (object_collided_with.tag == "rupee")
        {
            inventory.AddRupees(1);
            Debug.Log("Collected rupee!");
            Destroy(object_collided_with);

            AudioSource.PlayClipAtPoint(rupee_collection_sound_clip, Camera.main.transform.position);
        }
        if (object_collided_with.tag == "key")
        {
            inventory.AddKey();
            Debug.Log("Collected key!");
            Destroy(object_collided_with);
            AudioSource.PlayClipAtPoint(key_collection_sound_clip, Camera.main.transform.position);
        }
        if (object_collided_with.tag == "heart")
        {
            playerController.CollectHeart();
            Debug.Log("Collected heart!");
            Destroy(object_collided_with);
            AudioSource.PlayClipAtPoint(key_collection_sound_clip, Camera.main.transform.position);
        }
        if (object_collided_with.tag == "collectableBomb")
        {
            playerController.CollectBomb();
            Debug.Log("Collected bomb!");
            Destroy(object_collided_with);
            AudioSource.PlayClipAtPoint(key_collection_sound_clip, Camera.main.transform.position);
        }
        if (object_collided_with.tag == "triforce")
        {
            Debug.Log("Collected triforce!");
            StartCoroutine(GameController.instance.DoEndingSequence());
            AudioSource.PlayClipAtPoint(key_collection_sound_clip, Camera.main.transform.position);
        }
        if (object_collided_with.tag == "collectablePortalGun")
        {
            Debug.Log("Portal Gun!");
            Destroy(object_collided_with);
            weaponController.weaponSpecies = WeaponController.WeaponSpecies.PortalGun;
            weaponController.portalGunAcquired = true;
            AudioSource.PlayClipAtPoint(fanfare_Clip, Camera.main.transform.position);
        }

        //TODO: collect weapon
        // if (object_collided_with.tag == "weapon")
        // {
        //     weaponController.collectWeapon(WeaponController.WeaponSpecies.)
        // }
    }

    
}
