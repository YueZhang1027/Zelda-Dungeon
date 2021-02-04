using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    public GameController.Direction portalDir;

    private PortalGun.PortalType type;
    private Animator animator;
    private GameObject attachedWall;
    private Transform t;

    void Awake()
    {
        animator = GetComponent<Animator>();
        t = GetComponent<Transform>();
    }

    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            PortalGun.instance.TriggerPortalEntry(type);
        }
    }

    void OnDestroy()
    {
        if (attachedWall == null)
        {
            return;
        }
        attachedWall.GetComponent<BoxCollider>().enabled = true;
    }

    public void InitPortal(GameController.Direction direction, ref GameObject wallObject, PortalGun.PortalType portalType)
    {
        attachedWall = wallObject;
        t.position = wallObject.transform.position;

        portalDir = direction;

        type = portalType;

        if (portalType == PortalGun.PortalType.Blue)
        {
            animator.SetBool("isRed", false);
        }
        else
        {
            animator.SetBool("isRed", true);
        }
    }
}
