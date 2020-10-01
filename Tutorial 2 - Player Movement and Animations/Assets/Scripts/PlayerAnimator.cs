using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour {

    public Animator animator;

    public PlayerMovement movement;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        animator.SetBool("IsMoving", movement.IsMoving);
        animator.SetBool("Rising", movement.IsRising);
        animator.SetBool("Falling", movement.IsFalling);
        if(Input.GetKeyDown(KeyCode.Space) && movement.raycastCollider.isGrounded) animator.SetTrigger("Jumped");

    }
}
