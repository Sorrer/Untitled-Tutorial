using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Item {
    public Transform doorTransform;
    private bool used;

    private void Update() {
        if (used) {
            this.enabled = false;
        }
        if (held) {
            used = true;
            Drop();
            doorTransform.position += Vector3.up * 5f;
        }
    }
}
