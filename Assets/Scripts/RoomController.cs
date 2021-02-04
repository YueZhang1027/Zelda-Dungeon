using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public bool levelClear = false;
    public bool levelActive = false;
    public GameObject roomTriggeredObject;

    Transform levelTransform;
    Transform roomEnemiesTransform;
    private int numEnemies;
    void Start()
    {
        levelTransform = GetComponent<Transform>();
        roomEnemiesTransform = levelTransform.Find("RoomEnemies");
        numEnemies = roomEnemiesTransform.childCount;
        SetRoomTriggeredObjectActive(false);
        SetLevelActive(true);
    }

    void OnEnable()
    {
        if (roomEnemiesTransform == null)
        {
            return;
        }
        SetLevelActive(true);
    }

    void OnDisable()
    {
        if (roomEnemiesTransform == null)
        {
            return;
        }
        SetLevelActive(false);
    }

    void Update()
    {
    }

    public void DestroyAnEnemy()
    {
        numEnemies--;
        if (numEnemies == 0)
        {
            levelClear = true;
            SetRoomTriggeredObjectActive(false);
            Debug.Log(this.name + " Cleared!");
        }
    }

    private void SetLevelActive(bool active)
    {
        levelActive = active;
        roomEnemiesTransform.gameObject.SetActive(active);
        if (!levelClear)
        {
            SetRoomTriggeredObjectActive(active);
        }
        
    }

    private void SetRoomTriggeredObjectActive(bool active)
    {
        if (roomTriggeredObject != null)
        {
            roomTriggeredObject.SetActive(active);
        }
    }
}
