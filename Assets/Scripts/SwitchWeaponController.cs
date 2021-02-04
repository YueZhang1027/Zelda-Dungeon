using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchWeaponController : MonoBehaviour
{
    public WeaponController weaponController;

    Image image;
    public Sprite None;
    public Sprite Bow;
    public Sprite Boomerang;
    public Sprite PortalGun;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        switchWeaponImage();
    }

    // Update is called once per frame
    void Update()
    {
        switchWeaponImage();
    }

    void switchWeaponImage()
    {
        switch (weaponController.weaponSpecies)
        {
            case WeaponController.WeaponSpecies.None:
                GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                image.sprite = None;
            break;
            case WeaponController.WeaponSpecies.Bow:
                GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                image.sprite = Bow;
            break;
            case WeaponController.WeaponSpecies.Boomerang:
                GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                image.sprite = Boomerang;
            break;
            case WeaponController.WeaponSpecies.PortalGun:
                if (GameObject.Find("Player").GetComponent<WeaponController>().portalGunAcquired)
                {
                    GetComponent<RectTransform>().localScale = new Vector3(2, 1, 1);
                }
                else
                {
                    GetComponent<RectTransform>().localScale = Vector3.zero;
                }
                
                image.sprite = PortalGun;
            break;
        }
    }
}
