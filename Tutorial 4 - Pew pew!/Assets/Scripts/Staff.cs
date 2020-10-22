using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : Item {
    private SpriteRenderer playerSr;
    private void LateUpdate() {
        if (held) {
            playerSr = playerTransform.GetComponentInChildren<SpriteRenderer>();

            transform.position = playerTransform.position;

            if (playerSr.flipX) {
                transform.position += Vector3.left;
            } else {
                transform.position += Vector3.right;
            }
            
            
        }
    }
}
