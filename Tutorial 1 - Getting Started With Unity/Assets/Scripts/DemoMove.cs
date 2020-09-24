using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoMove : MonoBehaviour {

    public float MovementSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        Vector3 currentPosition = this.transform.position;

        if (Input.GetKey(KeyCode.A)) {

            currentPosition.x -= 1 * Time.deltaTime * MovementSpeed;

        }
        if (Input.GetKey(KeyCode.D)) {

            currentPosition.x += 1 * Time.deltaTime * MovementSpeed;

        }
        
        
        if (Input.GetKey(KeyCode.W)) {

            currentPosition.y += 1 * Time.deltaTime * MovementSpeed;

        }
        if (Input.GetKey(KeyCode.S)) {

            currentPosition.y -= 1 * Time.deltaTime * MovementSpeed;

        }
    
        
        this.transform.position = currentPosition;
    }
}
