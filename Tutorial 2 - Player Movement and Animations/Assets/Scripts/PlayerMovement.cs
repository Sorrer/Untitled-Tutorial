using System.Collections;
using System.Collections.Generic;
using Game.Collider;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    
    public RaycastCollider2D raycastCollider;

    public float MovementSpeed;
    public float JumpHeight;

    public float Gravity = -9.8f;

    private float VerticalVelocity;
    
    
    public SpriteRenderer sRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }


    public bool IsMoving;
    public bool IsRising;
    public bool IsFalling;
    
    // Update is called once per frame
    void Update() {
        IsMoving = false;
        IsRising = false;
        IsFalling = false;
        
        Vector2 Displacement = Vector2.zero;
        
        if (Input.GetKey(KeyCode.A)) {
            IsMoving = true;
            Displacement += Vector2.left;
            sRenderer.flipX = true;

        }
        
        if (Input.GetKey(KeyCode.D)) {
            IsMoving = true;
            Displacement += Vector2.right;
            
            sRenderer.flipX = false;
            
        }

        raycastCollider.Move(Displacement * Time.deltaTime * MovementSpeed);
        
        //Vertical Movement

        VerticalVelocity += Gravity * Time.deltaTime;

        if (raycastCollider.isGrounded) VerticalVelocity = 0;

        if (!raycastCollider.isGrounded) {
            if (VerticalVelocity > 0) {
                IsRising = true;
            }else if (VerticalVelocity < 0) {
                IsFalling = true;
            }
        }
        
        if (raycastCollider.isGrounded && Input.GetKeyDown(KeyCode.Space)) {
            VerticalVelocity = GetJumpVelocity(JumpHeight);
        }
        
        raycastCollider.Move(VerticalVelocity * Vector2.up * Time.deltaTime);

    }


    public float GetJumpVelocity(float jumpHeight) {
        return Mathf.Sqrt(2 * Mathf.Abs(Gravity) * JumpHeight);
    }
}
