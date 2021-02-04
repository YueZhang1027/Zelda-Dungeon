using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionController : MonoBehaviour
{

    public int slideTransitionSteps = 30;
    public float slideTransitionDuration = 0.5f;
    public Vector2 startingRoom;
    public Rigidbody rb;
    public Transform rooms;
    public Transform currentRoomTransform;
    public Vector2 currentRoom;

    Transform t;
    // Start is called before the first frame update
    void Start()
    {
        t = GetComponent<Transform>();
        currentRoom = startingRoom;
        currentRoomTransform = rooms.Find(GetRoomName(currentRoom));
        currentRoomTransform.GetComponent<RoomController>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator SlideTransition(Vector2 numShiftRoom)
    {
        rb.velocity = Vector2.zero;
        GameController.instance.gameState = GameController.GameStates.Transition;
        Vector3 distance = new Vector3(numShiftRoom.x * GameController.roomWidth, numShiftRoom.y * GameController.roomHeight, 0f);
        for (int i = 0; i < slideTransitionSteps; i++)
        {
            t.position += distance / (float) slideTransitionSteps;
            yield return new WaitForSecondsRealtime(slideTransitionDuration / (float) slideTransitionSteps);
        }
        rb.velocity = numShiftRoom * 6;
        yield return new WaitForSeconds(0.35f);
        rb.velocity = Vector2.zero;

        changeRoom(numShiftRoom);

        GameController.instance.gameState = GameController.GameStates.Play;


    }

    public void changeRoom(Vector2 shiftAmount)
    {
        currentRoomTransform.GetComponent<RoomController>().enabled = false;
        if (shiftAmount == Vector2.zero)
        {
            currentRoom = startingRoom;
        } else
        {
            currentRoom += shiftAmount;
        }
        currentRoomTransform = rooms.Find(GetRoomName(currentRoom));
        currentRoomTransform.GetComponent<RoomController>().enabled = true;
    }

    string GetRoomName(Vector2 room)
    {
        return "Room (" + room.x + "," + room.y + ")";
    }
}
