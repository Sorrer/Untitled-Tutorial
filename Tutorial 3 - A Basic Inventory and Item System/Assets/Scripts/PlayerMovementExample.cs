using System.Collections;
using System.Collections.Generic;
using Game.Collider;
using UnityEngine;



// Try the following!

// - Coyote Time
// - Crouching

public class PlayerMovementExample : MonoBehaviour {

    public RaycastCollider2D raycastCollider2D; //Custom made script

    public float MovementSpeed;

    public float Gravity = -9.7f;

    public float JumpHeight;

    public SpriteRenderer sRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }


    public bool IsMoving;
    public bool IsRising;
    public bool IsFalling;

    private float VerticalVelocity = 0;

    // Update is called once per frame
    void Update() {
        IsMoving = false;
        IsRising = false;
        IsFalling = false;
        
        //Horizontal Player Movement
        float horizontalDisplacement = 0;
        
        if (Input.GetKey(KeyCode.A)) {
            horizontalDisplacement -= 1;
            IsMoving = true;
        }
        
        if (Input.GetKey(KeyCode.D)) {
            horizontalDisplacement += 1;
            IsMoving = true;
        }

        horizontalDisplacement *= MovementSpeed * Time.deltaTime;
        
        if (horizontalDisplacement < 0) {
            sRenderer.flipX = true;
        } else if(horizontalDisplacement > 0){
            sRenderer.flipX = false;
        }
        
        raycastCollider2D.Move(horizontalDisplacement * Vector3.right);
        
        
        
        //Vertical Movement - Jumping/Falling

        
        VerticalVelocity += Gravity * Time.deltaTime;

        if (raycastCollider2D.isGrounded || raycastCollider2D.isCollidingBottom) {
            VerticalVelocity = 0;
        } else {
            if (VerticalVelocity > 0) {
                IsRising = true;
            } else {
                IsFalling = true;
            }
        }
        
        if ((raycastCollider2D.isGrounded || raycastCollider2D.isCollidingBottom) && Input.GetKeyDown(KeyCode.Space)) {
            VerticalVelocity = GetJumpHeight(JumpHeight);
            Debug.Log("Jump " + VerticalVelocity);
        }
        
        raycastCollider2D.Move(VerticalVelocity * Time.deltaTime * Vector3.up);
        
        
    }


    public float GetJumpHeight(float jumpHeight) {
        return Mathf.Sqrt(Mathf.Abs(2 * Gravity * JumpHeight));
    }
    
    
    
    
    
}
