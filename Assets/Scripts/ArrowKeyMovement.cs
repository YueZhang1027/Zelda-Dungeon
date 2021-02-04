using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowKeyMovement : MonoBehaviour
{
    public float movement_speed = 4.85f;

    Rigidbody rb;
    Animator animator;
    PlayerController playerController;
    Vector2 previousInput = new Vector2(0.0f,0.0f);
    Vector2 velocity = new Vector2(0, 0);

    Transform playerTransform;
    GameController.Direction previousDirection;

    public bool portalExit = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();

        playerTransform = GetComponent<Transform>();
        previousDirection = GameController.Direction.Up;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.instance.gameState != GameController.GameStates.Play)
        {
            return;
        }
        if(playerController.state == PlayerController.PlayerStates.Unresponsive)
        {
            if (!playerController.isInvincible)
            {
                rb.velocity = Vector2.zero;
            }
            return;
        }
        if(playerController.state == PlayerController.PlayerStates.Attack)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        Vector2 current_input = GetInput();

        if (portalExit && current_input == previousInput)
        {
            //if (current_input - previousInput == Vector2.zero)
            //{
            //    Debug.Log("ZERO");
            //    Debug.Log(previousInput);
            //    rb.velocity = previousInput * movement_speed * -1;
            //    StartCoroutine(DoPortalExit());
            //    //previousInput = current_input;
            //    return;
            //} else if (current_input != previousInput)
            //{
            //    Debug.Log("NOZERO");
            //    rb.velocity = (current_input - previousInput) * movement_speed;
            //    StartCoroutine(DoPortalExit());
            //    previousInput = current_input;
            //    return;
            //}
            velocity = GameController.getDirectionVector3(previousDirection);
            rb.velocity = velocity * movement_speed;
            StartCoroutine(DoPortalExit());
            return;
        }
        else if (current_input != previousInput)
        {
            if (portalExit)
            {
                portalExit = false;
            }

            velocity = current_input;
            if (current_input.x != 0.0f && current_input.y != 0.0f)
            {
                if (previousInput.x - current_input.x != 0.0f && previousInput.y - current_input.y != 0.0f)
                {
                    velocity.y = 0;
                }
                else if (previousInput.x != 0.0f)
                {
                    //Debug.Log("X GONE");
                    velocity.x = 0;
                }
                else if (previousInput.y != 0.0f)
                {
                    //Debug.Log("Y GONE");
                    velocity.y = 0;
                }
            }
            previousInput = current_input;
        }
        rb.velocity = velocity * movement_speed;
        if (GameController.GetAxis(previousDirection) != GameController.GetAxis(GetDirection())){
            alignGrid(GameController.GetAxis(previousDirection));
        }

        previousDirection = GetDirection();

    }

    IEnumerator DoPortalExit()
    {
        bool curr = portalExit;
        yield return new WaitForSeconds(0.2f);
        if (curr == portalExit)
        {
            portalExit = false;
        }
    }

    void alignGrid(GameController.Axis axis)
    {
        if (axis == GameController.Axis.X){
                float newX = Mathf.Round(playerTransform.position.x * 2) / 2;
                playerTransform.position = new Vector3(newX, playerTransform.position.y, playerTransform.position.z);
        } else {
            
                float newY = Mathf.Round(playerTransform.position.y * 2) / 2;
                playerTransform.position = new Vector3(playerTransform.position.x,  newY, playerTransform.position.z);
        }
    }

    Vector2 GetInput()
    {
        float horizontal_input = Input.GetAxisRaw("Horizontal");
        float vertical_input = Input.GetAxisRaw("Vertical");

        return new Vector2(horizontal_input, vertical_input);
    }

    public GameController.Direction GetDirection()
    {
        if (animator.GetCurrentAnimatorStateInfo(1).IsName("idle_up"))
        {
            return GameController.Direction.Up;
        }
        else if (animator.GetCurrentAnimatorStateInfo(1).IsName("idle_down"))
        {
            return GameController.Direction.Down;
        }
        else if (animator.GetCurrentAnimatorStateInfo(1).IsName("idle_right"))
        {
            return GameController.Direction.Right;
        }
        else
        {
            return GameController.Direction.Left;
        }
    }

    public void SetDirection(GameController.Direction direction)
    {
        previousDirection = direction;
    }
}
