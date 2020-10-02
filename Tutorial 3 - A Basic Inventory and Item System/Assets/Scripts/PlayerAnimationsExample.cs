using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationsExample : MonoBehaviour {

    public Animator animator;

    public PlayerMovementExample movement;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("IsMoving", movement.IsMoving);
        animator.SetBool("IsRising", movement.IsRising);
        animator.SetBool("IsFalling", movement.IsFalling);
        
        if(Input.GetKeyDown(KeyCode.Space)) animator.SetTrigger("Jumped");
    }
}
