using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallmasterRoomController : EnemyController
{
    public int numWallmasters;
    public GameObject wallmasterPrefab;
    public float cooldownTime;

    private List<GameObject> wallmasterList;
    private bool cooldown = false;
    private GameObject player;
    private int nextWallmasterIndex = 0;
    void Start()
    {
        cooldown = false;
        t = GetComponent<Transform>();
        wallmasterList = new List<GameObject>();
        for (int i = 0; i < numWallmasters; i++)
        {
            GameObject g = Instantiate(wallmasterPrefab, t.localPosition, Quaternion.identity);
            g.transform.SetParent(t, false);
            g.SetActive(false);
            wallmasterList.Add(g);
        }
        player = GameObject.Find("Player");
    }

    void Update()
    {
        if (!cooldown)
        {
            if (castRay(t.position + new Vector3(1.5f, 1.8f, 0), GameController.getDirectionVector3(GameController.Direction.Right), GameController.roomWidth - 4, LayerMask.GetMask("Player")))
            {
                StartCoroutine(spawnWallmaster(GameController.Direction.Down));
            }
            else if (castRay(t.position + new Vector3(1.5f, 8.2f, 0), GameController.getDirectionVector3(GameController.Direction.Right), GameController.roomWidth - 4, LayerMask.GetMask("Player")))
            {
                StartCoroutine(spawnWallmaster(GameController.Direction.Up));
            }
            else if (castRay(t.position + new Vector3(2f, 1.5f, 0), GameController.getDirectionVector3(GameController.Direction.Up), GameController.roomHeight - 4, LayerMask.GetMask("Player")))
            {
                StartCoroutine(spawnWallmaster(GameController.Direction.Left));
            }
            else if (castRay(t.position + new Vector3(13f, 1.5f, 0), GameController.getDirectionVector3(GameController.Direction.Up), GameController.roomHeight - 4, LayerMask.GetMask("Player")))
            {
                StartCoroutine(spawnWallmaster(GameController.Direction.Right));
            }
        }
    }

    IEnumerator DoCooldown()
    {
        cooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        cooldown = false;
    }

    IEnumerator spawnWallmaster(GameController.Direction direction)
    {
        StartCoroutine(DoCooldown());
        //Spawn
        GameController.Direction playerDirection = player.GetComponent<ArrowKeyMovement>().GetDirection();
        Vector3 hitDirectionVector = GameController.getDirectionVector3(direction);
        Vector3 playerDirectionVector = GameController.getDirectionVector3(playerDirection);
        Vector3 sumVector = hitDirectionVector + playerDirectionVector;
        Vector3 spawnDirectionVector;
        if (sumVector.magnitude == 0 || sumVector.magnitude == 2)
        {
            spawnDirectionVector = Quaternion.AngleAxis(90, Vector3.forward) * hitDirectionVector;
        } else
        {
            spawnDirectionVector = playerDirectionVector;
        }
        Vector3 spawnLocation = player.transform.localPosition + spawnDirectionVector * 4 + hitDirectionVector;

        wallmasterList[nextWallmasterIndex].transform.position = spawnLocation;
        wallmasterList[nextWallmasterIndex].SetActive(true);
        //Animate
        Vector3 emergeDirection = hitDirectionVector * -1;
        Vector3 followDirection = spawnDirectionVector * -1;
        int currIndex = nextWallmasterIndex;
        IncrementWallmasterIndex();
        yield return StartCoroutine(wallmasterList[currIndex].GetComponent<WallmasterController>().Move(emergeDirection, followDirection*4));
    }

    void IncrementWallmasterIndex()
    {
        if (nextWallmasterIndex == numWallmasters - 1)
        {
            nextWallmasterIndex = 0;
        } else
        {
            nextWallmasterIndex++;
        }
    }
}
