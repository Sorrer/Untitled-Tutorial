using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour {

    private Item holding;
    private List<Collider2D> nearbyItems = new List<Collider2D>();

    public Transform playerTransform;
    public float pickupRadius = 3f;

    private ContactFilter2D cfilter;
    private ContactFilter2D wallFilter;
    
    void Start() {
        // A ContactFilter that, when used, will only collide with objects on the "Items" layer
        cfilter = new ContactFilter2D();
        cfilter.SetLayerMask(LayerMask.GetMask("Items")); 
        cfilter.useLayerMask = true;
        
        // A ContactFilter that, when used, will only collide with objects on the "Default" layer
        wallFilter = new ContactFilter2D();
        wallFilter.SetLayerMask(LayerMask.GetMask("Default"));
        wallFilter.useLayerMask = true;
    }
    
    void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            // Draw a circle around the player to the item and return any items within
            Physics2D.OverlapCircle(playerTransform.position, pickupRadius, cfilter, nearbyItems);
            
            // Sorts all the results from the OverlapCircle by proximity to player. This is a fairly complex but elegant way to do it. If you've taken
            // data structures you can probably make your own version ;)
            nearbyItems.OrderBy(x => Vector2.Distance(x.transform.position, playerTransform.position)).ToList();

            Item closest = null;
            List<RaycastHit2D> hits = new List<RaycastHit2D>();
            
            foreach (var nearbyItem in nearbyItems) {
                // Draw a line from the player to the item and return any collisions with walls
                Physics2D.Linecast(playerTransform.position, nearbyItem.transform.position, wallFilter, hits);
                
                if (hits.Count == 0) {
                    // We hit 0 walls!
                    closest = nearbyItem.gameObject.GetComponent<Item>();
                    break;
                }
            }

            // Check if there is an item in range that is also not blocked by a wall
            if (closest != null) {
                
                // If we are already holding an item, drop it
                if (holding != null && !closest.GetComponent<Lever>()) {
                    holding.Drop();
                }

                holding = closest;
                holding.Pickup(playerTransform);
            }
        }
    }
    
    // Special function to draw a circle around the player with the same radius as the pickup radius, great for debugging!
#if UNITY_EDITOR
    private void OnDrawGizmos() {
        UnityEditor.Handles.color = Color.green;
        UnityEditor.Handles.DrawWireDisc(playerTransform.position, Vector3.forward, pickupRadius);
    }
#endif
}
