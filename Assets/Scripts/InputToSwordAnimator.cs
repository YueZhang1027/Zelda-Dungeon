using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputToSwordAnimator : MonoBehaviour
{
    // Start is called before the first frame update
    ArrowKeyMovement movement;
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        movement = transform.parent.gameObject.GetComponent<ArrowKeyMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetInteger("Direction", (int) movement.GetDirection());
        //if(ThrowingSword.instance.throwing)
        //{
        //    animator.speed = 0.0f;
        //}
        //else
        //{
        //    animator.speed = 1.0f;
        //}
    }
}
