using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyDisplayer : MonoBehaviour
{
    public Inventory inventory;
    Text TextComponent;
    // Start is called before the first frame update
    void Start()
    {
        TextComponent = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inventory != null && TextComponent != null)
        {
            if(inventory.isGodMode)
            {
                TextComponent.text = "×∞";
            }
            else
            {
                TextComponent.text = "×" + inventory.GetKeys().ToString();
            }
        }
    }
}
