using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public class GameController : MonoBehaviour
{
    public enum Direction
    {
        Up,
        Left,
        Right,
        Down,
        None
    }

    public enum Axis
    {
        X,
        Y,
        None
    }

    public enum GameStates
    {
        Play,
        Transition
    }

    public static GameController instance;
    public GameStates gameState = GameStates.Play;
    public static readonly int minX = 2;
    public static readonly int maxX = 13;
    public static readonly int minY = 2;
    public static readonly int maxY = 8;
    public static readonly float roomWidth = 16;
    public static readonly float roomHeight = 11;
    public GameObject cameraObject;
    public AudioClip triforceEnding;

    private GameObject player;
    private Vector3 playerStartPosition;
    private Vector3 cameraStartPosition;

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
        Application.targetFrameRate = 60;
        player = GameObject.Find("Player");
        playerStartPosition = player.transform.position;
        cameraStartPosition = cameraObject.GetComponent<RectTransform>().position;
    }


    public void RestartCurrentScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void SendToStart()
    {
        cameraObject.GetComponent<RectTransform>().position = cameraStartPosition;
        TransitionController tc = cameraObject.GetComponent<TransitionController>();
        tc.changeRoom(Vector2.zero);
        player.transform.position = playerStartPosition;
        PlayerController pc = player.GetComponent<PlayerController>();
        pc.state = PlayerController.PlayerStates.Idle;
    }

    public bool IsPlayerInBounds()
    {
        Vector3 playerLocalPos = player.transform.position - cameraObject.GetComponent<TransitionController>().currentRoomTransform.position;
        if (playerLocalPos.x >= minX - 0.06f && playerLocalPos.x <= maxX + 0.06
            && playerLocalPos.y >= minY && playerLocalPos.y <= maxY + 0.7f)
        {
            return true;
        }
        return false;
    }

    public IEnumerator DoEndingSequence()
    {
        player.GetComponent<PlayerController>().state = PlayerController.PlayerStates.Unresponsive;
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        cameraObject.GetComponent<AudioSource>().Stop();
        AudioSource.PlayClipAtPoint(triforceEnding, Camera.main.transform.position);
        yield return new WaitForSeconds(9);
        RestartCurrentScene();
    }

    public static void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public static Axis GetAxis(Direction direction){
        if (direction == Direction.Left || direction == Direction.Right){
            return Axis.X;
        }
        else if (direction == Direction.Up || direction == Direction.Down){
            return Axis.Y;
        }
        return Axis.None;
    }

    public static Vector3 getDirectionVector3(Direction d)
    {
        if (d == Direction.Up)
        {
            return Vector3.up;
        }
        else if (d == Direction.Down)
        {
            return Vector3.down;
        }
        else if (d == Direction.Right)
        {
            return Vector3.right;
        }
        else if(d == Direction.Left)
        {
            return Vector3.left;
        }
        else
        {
            return Vector3.zero;
        }
    }

    public static GameController.Direction getDirectionObject(Vector3 v)
    {
        if (v.magnitude != 1)
        {
            return GameController.Direction.None;
        }else
        {
            if (v == Vector3.right)
            {
                return GameController.Direction.Right;
            }
            else if (v == Vector3.left)
            {
                return GameController.Direction.Left;
            }
            else if (v == Vector3.down)
            {
                return GameController.Direction.Down;
            }
            else if (v == Vector3.up)
            {
                return GameController.Direction.Up;
            }
            return GameController.Direction.None;
        }
    }

    public static Direction getReverseDirection(Direction direction)
    {
        if (direction == Direction.Up)
        {
            return Direction.Down;
        }
        if (direction == Direction.Down)
        {
            return Direction.Up;
        }
        if (direction == Direction.Left)
        {
            return Direction.Right;
        }
        if (direction == Direction.Right)
        {
            return Direction.Left;
        }
        return Direction.None;
    }
}
