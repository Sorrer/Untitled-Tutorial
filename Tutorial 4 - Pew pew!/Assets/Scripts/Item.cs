using UnityEngine;

public abstract class Item : MonoBehaviour {

    public Collider2D collider;
    public Rigidbody2D rb;
    public SpriteRenderer sprite;

    protected Transform playerTransform;

    protected bool held;

    public float throwForce = 5f;

    public void Pickup(Transform playerT) {
        playerTransform = playerT;
        held = true;
        
        // Disable the collider so if we hit "E" again we don't detect this weapon as one that can be picked up
        collider.enabled = false; 
        rb.velocity = Vector2.zero;
    }
    
    public void Drop() {
        held = false;
        collider.enabled = true;

        // Base the direction we throw on the direction the player sprite is facing
        Vector2 playerDir;
        if (playerTransform.gameObject.GetComponentInChildren<SpriteRenderer>().flipX) { 
            playerDir = Vector2.left;
        } else {
            playerDir = Vector2.right;
        }
        
        // Add a force to the gun to throw it. Using vector math, we see the result is 45 degrees up either
        // left or right, depending on playerDir
        rb.AddForce(playerDir * throwForce + Vector2.up * throwForce, ForceMode2D.Impulse);
    }
}