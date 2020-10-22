using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private ContactFilter2D enemiesAndWalls;

    public float shotForce = 5f;

    public int damage = 10;
    void Start() {
        enemiesAndWalls = new ContactFilter2D();
        enemiesAndWalls.SetLayerMask(LayerMask.GetMask("Enemies", "Default"));
        enemiesAndWalls.useLayerMask = true;
        
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        rb.AddForce(transform.forward * shotForce, ForceMode2D.Impulse);
    }
}
