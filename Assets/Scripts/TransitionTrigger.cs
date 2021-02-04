using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionTrigger : MonoBehaviour
{
    public Vector2 transitionTarget;

    private GameObject camera;

    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider coll)
    {

        if (coll.tag == "Player" && GameController.instance.gameState != GameController.GameStates.Transition)
        {
            StartCoroutine(camera.GetComponent<TransitionController>().SlideTransition(transitionTarget));
            coll.gameObject.GetComponent<WeaponController>().DestroyPortals();
        }
    }
}
