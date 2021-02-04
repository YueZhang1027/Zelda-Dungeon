using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivateOldMan : MonoBehaviour
{
    public Text oldManText;
    Transform playerTransform;
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isInRoom())
        {
            oldManText.gameObject.SetActive(true);
        }
        else
        {
            oldManText.gameObject.SetActive(false);
        }
    }


    bool isInRoom()
    {
        float playerX = playerTransform.position.x;
        float playerY = playerTransform.position.y;
        if (playerX >= 0 && playerX <= 14 && playerY >= 33 && playerY <= 43)
        {
            return true;
        }
        return false;
    }

}
