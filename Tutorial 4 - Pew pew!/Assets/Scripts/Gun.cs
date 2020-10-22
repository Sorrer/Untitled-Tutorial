using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Game.Collider {
    public class Gun : Item {

        public float holdDist = 0.75f;

        public float bulletAliveTime = 0.2f;

        public Bullet bullet;

        private Camera cam;
        private void Start() {
            cam = Camera.main;
        }

        private void LateUpdate() {
            if (held) {
                Vector3 rot = (cam.ScreenToWorldPoint(Input.mousePosition) - playerTransform.position);
                rot = new Vector3(rot.x, rot.y, 0).normalized;

                transform.position = playerTransform.position + rot * holdDist;
                transform.position += Vector3.back;

                transform.right = rot;

                if (rot.x < 0) {
                    sprite.flipY = true;
                } else {
                    sprite.flipY = false;
                }

                if (Input.GetMouseButtonDown(0)) {
                    Bullet newBullet = Instantiate(bullet, transform.position, Quaternion.LookRotation(rot, Vector3.up));
                    Destroy(newBullet.gameObject, bulletAliveTime);
                }
            }
        }
    }
}