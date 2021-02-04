using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class typer : MonoBehaviour
{
    Text oldManText;
    // Start is called before the first frame update
    void Start()
    {
        oldManText = GetComponent<Text>();
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(TypeTextEffect());
    }

    public IEnumerator TypeTextEffect(float letterPause = 0f)
    {
        string texts = "A totally original game by copycat co.";

        oldManText.text = "";
        
        foreach (char letter in texts.ToCharArray())
        {
            oldManText.text += letter;
            yield return new WaitForSeconds(letterPause);
        }
    }
}
