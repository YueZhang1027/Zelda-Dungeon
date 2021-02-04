using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HeartDisplayer : MonoBehaviour
{
    public PlayerController playerController;
    public GameObject heartPrefab;
    public Material fullHeart;
    public Material halfHeart;
    public Material noHeart;

    private GameObject[] hearts;
    private int currHp;
    // Start is called before the first frame update
    void Start()
    {
        hearts = new GameObject[playerController.maxHp / 2];
        currHp = playerController.maxHp;
        for (int i = 0; i < playerController.maxHp / 2; i++)
        {
            hearts[i] = (GameObject)Instantiate(heartPrefab, GetComponent<Transform>());
            hearts[i].transform.position = new Vector2(i * 30 + hearts[i].transform.position.x, hearts[i].transform.position.y);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currHp != playerController.hp)
        {
            for (int i = 0; i < playerController.hp / 2; i++)
            {
               hearts[i].GetComponent<Image>().material = fullHeart;
            }
            if (playerController.hp % 2 == 1)
            {
               hearts[playerController.hp / 2].GetComponent<Image>().material = halfHeart;
            }
            for (int i = 0; i <  (playerController.maxHp - playerController.hp) / 2; i++)
            {
                hearts[hearts.Length - 1 - i].GetComponent<Image>().material = noHeart;
            }
            currHp = playerController.hp;
        }
    }
}
