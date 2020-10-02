using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour {

    void Start() {
        
    }
    
    void Update() {
       
    }
    
#if UNITY_EDITOR
    private void OnDrawGizmos() {
        UnityEditor.Handles.color = Color.green;
    }
#endif
}
