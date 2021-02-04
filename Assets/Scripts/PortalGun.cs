using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalGun : MonoBehaviour
{
    public static PortalGun instance;
    public float shootingSpeed = 10.0f;
    public GameObject portalPrefab;

    public AudioClip portalThrow;
    public AudioClip portalHit;
    public AudioClip portalEnterRed;
    public AudioClip portalEnterBlue;

    Transform trans;
    GameController.Direction throwingDirection;
    BoxCollider boxCol;
    Animator animator;

    PortalController redPortal;
    PortalController bluePortal;
    PortalType nextPortal = PortalType.Red;
    
    public enum PortalType
    {
        Red,
        Blue,
        None
    }

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
    // Start is called before the first frame update
    void Start()
    {
        boxCol = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        trans = GetComponent<Transform>();
        gameObject.SetActive(false);
        animator = GetComponent<Animator>();
    }

    public void Attack(GameController.Direction direction)
    {
        throwingDirection = direction;
        //set sword to be active
        gameObject.SetActive(true);
        AudioSource.PlayClipAtPoint(portalThrow, Camera.main.transform.position, 0.35f);

        Transform playerTransform = transform.parent.gameObject.GetComponent<Transform>();
        boxCol.center = Vector3.zero;
        switch(direction)
        {
            case GameController.Direction.Up:
                boxCol.size = new Vector3(0.05f, 0.7f, 1.0f);
                trans.position = playerTransform.position + new Vector3(0.0f, 0.4f, 0.0f);
                break;
            case GameController.Direction.Down:
                boxCol.size = new Vector3(0.05f, 0.7f, 1.0f);
                trans.position = playerTransform.position + new Vector3(0.0f, -0.4f, 0.0f);
                break;
            case GameController.Direction.Right:
                boxCol.size = new Vector3(0.7f, 0.05f, 1.0f);
                trans.position = playerTransform.position + new Vector3(0.4f, -0.15f, 0.0f);
                break;
            case GameController.Direction.Left:
                boxCol.size = new Vector3(0.7f, 0.05f, 1.0f);
                trans.position = playerTransform.position + new Vector3(-0.4f, -0.15f, 0.0f);
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

    // Update is called once per frame
    void Update()
    {
        switch(throwingDirection){
        case GameController.Direction.Up:
            rb.velocity = new Vector3(0.0f, shootingSpeed, 0.0f);
        break;
        case GameController.Direction.Down:
            rb.velocity = new Vector3(0.0f, -shootingSpeed, 0.0f);
        break;
        case GameController.Direction.Right:
            rb.velocity = new Vector3(shootingSpeed, 0.0f, 0.0f);
        break;
        case GameController.Direction.Left:
            rb.velocity = new Vector3(-shootingSpeed, 0.0f, 0.0f);
        break;
        }

    }

    void OnTriggerEnter(Collider collider)
    {
        if (!GameController.instance.IsPlayerInBounds())
        {
            WeaponController weaponController = transform.parent.gameObject.GetComponent<WeaponController>();
            weaponController.BWeaponAttackEnd();
            return;
        }
        if (collider.gameObject.CompareTag("wall"))
        {
            AudioSource.PlayClipAtPoint(portalHit, Camera.main.transform.position, 0.35f);
            collider.enabled = false;
            //collider.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Foreground";
            animator.SetBool("isActive", false);
            WeaponController weaponController = transform.parent.gameObject.GetComponent<WeaponController>();
            weaponController.BWeaponAttackEnd();
            CreatePortal(collider.gameObject);
            // Open a portal door on unshooted wall
            // Close portal door on wall with portal door
            // End shooting
        }
        else if (collider.gameObject.CompareTag("toughwall") || collider.gameObject.CompareTag("upperwall") || collider.gameObject.CompareTag("shortwall") || collider.gameObject.CompareTag("door"))
        {
            animator.SetBool("isActive", false);
            WeaponController weaponController = transform.parent.gameObject.GetComponent<WeaponController>();
            weaponController.BWeaponAttackEnd();
        }
    }

    public void TriggerPortalEntry(PortalType type)
    {
        if (redPortal == null || bluePortal == null)
        {
            return;
        }
        PortalController exit;
        GameObject player = trans.parent.gameObject;
        ArrowKeyMovement playerMovement = player.GetComponent<ArrowKeyMovement>();
        if (playerMovement.portalExit)
        {
            return;
        }

        if (type == PortalType.Red)
        {
            AudioSource.PlayClipAtPoint(portalEnterRed, Camera.main.transform.position, 0.35f);
            exit = bluePortal;
        }
        else
        {
            AudioSource.PlayClipAtPoint(portalEnterBlue, Camera.main.transform.position, 0.35f);
            exit = redPortal;
        }

        player.transform.position = exit.transform.position;
        playerMovement.portalExit = true;
        playerMovement.SetDirection(GameController.getReverseDirection(exit.portalDir));
        
    }

    public void DestroyPortals()
    {
        if (redPortal != null)
        {
            Destroy(redPortal.gameObject);
        }
        if (bluePortal != null)
        {
            Destroy(bluePortal.gameObject);
        }
    }

    void CreatePortal(GameObject collidedWall)
    {
        GameObject g = Instantiate(portalPrefab, collidedWall.transform.position, Quaternion.identity);
        if (nextPortal == PortalType.Red)
        {
            if (redPortal != null)
            {
                Destroy(redPortal.gameObject);
            }
            redPortal = g.GetComponent<PortalController>();
            redPortal.InitPortal(throwingDirection, ref collidedWall, PortalType.Red);
            if (bluePortal == null)
            {
                redPortal.GetComponent<Animator>().speed = 0f;
            } else
            {
                bluePortal.GetComponent<Animator>().speed = 1f;
            }
            nextPortal = PortalType.Blue;
        }
        else if (nextPortal == PortalType.Blue)
        {
            if (bluePortal != null)
            {
                Destroy(bluePortal.gameObject);
            }
            bluePortal = g.GetComponent<PortalController>();
            bluePortal.InitPortal(throwingDirection, ref collidedWall, PortalType.Blue);
            if (redPortal == null)
            {
                bluePortal.GetComponent<Animator>().speed = 0f;
            }
            else
            {
                redPortal.GetComponent<Animator>().speed = 1f;
            }
            nextPortal = PortalType.Red;
        }

        
        //g.transform.SetParent(trans, false);

        PortalController portalController = g.GetComponent<PortalController>();
        BoxCollider portalCollider = g.GetComponent<BoxCollider>();
        portalCollider.size = new Vector3(0.9f, 0.1f, 0);
        portalCollider.center = new Vector3(0, 0.2f, 0);

        switch (throwingDirection)
        {
            case GameController.Direction.Up:
                
                break;
            case GameController.Direction.Down:
                Vector3 rotD = g.transform.rotation.eulerAngles;
                rotD = new Vector3(rotD.x, rotD.y, rotD.z + 180);
                g.transform.rotation = Quaternion.Euler(rotD);
                break;
            case GameController.Direction.Right:
                Vector3 rotR = g.transform.rotation.eulerAngles;
                rotR = new Vector3(rotR.x, rotR.y, rotR.z - 90);
                g.transform.rotation = Quaternion.Euler(rotR);
                break;
            case GameController.Direction.Left:
                Vector3 rotL = g.transform.rotation.eulerAngles;
                rotL = new Vector3(rotL.x, rotL.y, rotL.z + 90);
                g.transform.rotation = Quaternion.Euler(rotL);
                break;
        }
        
    }
}
