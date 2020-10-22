using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastBullet : MonoBehaviour {

    private ContactFilter2D enemiesAndWalls;

    public float maxShotRange = 50f;

    public int damage = 10;
    void Start() {
        enemiesAndWalls = new ContactFilter2D();
        enemiesAndWalls.SetLayerMask(LayerMask.GetMask("Enemies", "Default"));
        enemiesAndWalls.useLayerMask = true;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.forward, maxShotRange,
            enemiesAndWalls.layerMask);

        LineRenderer lr = GetComponent<LineRenderer>();

        Vector3 lastPoint;
        if (hit.collider == null) {
            lastPoint = transform.position + transform.forward * maxShotRange;
        } else {
            lastPoint = hit.point;
            Enemy enemy = hit.collider.gameObject.GetComponent<Enemy>();
            if (enemy != null) {
                enemy.TakeDamage(damage);
            }
        }
        
        lr.SetPositions(new []{ transform.position, lastPoint });
    }
}
