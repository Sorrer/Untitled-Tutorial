// Developed by Alexander Xie (Sorrer)
// Note: This is a basic raycast collider that allows for oneway platforms.
// Though we will not touch that in the tutorials, I highly recommend playing around with it!
// (Its as simple as adding a oneway collision layer and adding a edge collider with that layer)

// Also since this is not maintained there will be some unwanted features with it.
// For instance, if you are trying to add crouching and uncrouching capabilities,
// if you improperly change the collision bounds the raycaster will collide into the terrain and not properly get unstuck


using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
namespace Game.Collider {

    public class RaycastCollider2D : MonoBehaviour {
        [HideInInspector]
        public RaycastPositions raycastPositions;
        private Collider2D PlayerCollider;

        public float SkinThickness;
        public int WidthRaycastsCount = 8;
        public int HeightRaycastsCount = 8;

        public LayerMask collisionMask;
        public LayerMask onewayColliderMask;
        private float WidthRaycastSpacing;
        private float HeightRaycastSpacing;
        public bool AutoUnstuck = true;

        public bool PassthroughOneways = false;

        //private bool UseJobs = false;

        public float Width { get; private set; }
        public float Height { get; private set; }

        public struct RaycastPositions {
            public Vector2 bottomLeft, bottomRight, topLeft, topRight;
            public Vector2 trueBottomLeftEdge;
            public Vector2 trueBottomRightEdge;
        }

        void Start() {
            PlayerCollider = GetComponent<Collider2D>();
            UpdateRaycastPositions();

        }

        /// <summary>
        /// Used to get the bounds for the raycasting controller
        /// </summary>
        public void UpdateRaycastPositions() {
            raycastPositions = new RaycastPositions();
            Bounds bounds = PlayerCollider.bounds;
            
            raycastPositions.trueBottomLeftEdge = new Vector2(bounds.min.x, bounds.min.y);
            raycastPositions.trueBottomRightEdge = new Vector2(bounds.max.x, bounds.min.y);

            bounds.Expand(SkinThickness * -2);
            bounds.center = this.transform.position + (Vector3) PlayerCollider.offset;
            
            raycastPositions.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
            raycastPositions.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
            raycastPositions.topLeft = new Vector2(bounds.min.x, bounds.max.y);
            raycastPositions.topRight = new Vector2(bounds.max.x, bounds.max.y);

            Width = Vector2.Distance(raycastPositions.bottomLeft, raycastPositions.bottomRight);
            Height = Vector2.Distance(raycastPositions.bottomLeft, raycastPositions.topLeft);

            WidthRaycastSpacing = Width / (WidthRaycastsCount - 1);
            HeightRaycastSpacing = Height / (HeightRaycastsCount - 1);
        }
        
        public bool isCollidingTop { get; private set; }
        public bool isCollidingBottom { get; private set; }
        public bool isGrounded { get; private set; }
        public bool isCollidingLeft { get; private set; }
        public bool isCollidingRight { get; private set; }

        private CollisionProperties collisionBottom;
        private CollisionProperties collisionTop;
        private CollisionProperties collisionLeft;
        private CollisionProperties collisionRight;

        private Vector3 LastPosition;

        /// <summary>
        /// Used to update collision booleans
        /// </summary>
        public void UpdateCollisions() {
            LastPosition = this.transform.position;

            UpdateRaycastPositions();

            //Top collision detection and rebounds

            collisionTop = CheckCollision(Vector2.up, raycastPositions.topLeft, raycastPositions.topRight, WidthRaycastsCount, WidthRaycastSpacing, SkinThickness);
            collisionBottom = CheckCollision(Vector2.down, raycastPositions.bottomLeft, raycastPositions.bottomRight, WidthRaycastsCount, WidthRaycastSpacing, SkinThickness);
            collisionLeft = CheckCollision(Vector2.left, raycastPositions.bottomLeft, raycastPositions.topLeft, HeightRaycastsCount, HeightRaycastSpacing, SkinThickness);
            collisionRight = CheckCollision(Vector2.right, raycastPositions.bottomRight, raycastPositions.topRight, HeightRaycastsCount, HeightRaycastSpacing, SkinThickness);
    


            isGrounded = CheckCollision(Vector2.down, raycastPositions.bottomLeft, raycastPositions.bottomRight, WidthRaycastsCount, WidthRaycastSpacing, SkinThickness + 0.075f, false).collided;

            isCollidingTop = collisionTop.collided;
            isCollidingBottom = collisionBottom.collided;
            isCollidingLeft = collisionLeft.collided;
            isCollidingRight = collisionRight.collided;
        }


        public float MinStepDistance = 0.075f;

        /// <summary>
        /// This should be used after moving a large distance to make sure you didn't collide into a wall or anything
        ///
        /// Note: This is highly recommended for low FPS hardware, since at lower fps moving fast enough will cause the player to run through the wall.
        /// </summary>
        public void Move(Vector3 Displacement) {

            UpdateUnstucks();
            if (MinStepDistance <= 0.0001) {
                this.transform.position += Displacement;
                return;
            }

            float magnitude = Displacement.magnitude;
            Vector3 direction = Displacement / magnitude;

            //Step through each displacement checking for a hit and unstucking
            while (magnitude > MinStepDistance) {
                this.transform.position += MinStepDistance * direction;
                magnitude -= MinStepDistance;

                UpdateCollisions();
                UpdateUnstucks();
            }


            
            //Do the remaining distance left and unstuck
            if (magnitude > 0) {
                this.transform.position += magnitude * direction;
                UpdateCollisions();
                UpdateUnstucks();
            }


        }


        /// <summary>
        /// Shoots multiple rays out to check collisions along one line with a direction.
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="amount"></param>
        /// <param name="spacing"></param>
        /// <returns>Data of what happen with collision</returns>
        private CollisionProperties CheckCollision(Vector2 direction, Vector2 a, Vector2 b, int amount, float spacing, float rayLength, bool debug = false) {
            Vector2 spacingDir = (b - a).normalized;
            CollisionProperties properties = new CollisionProperties();
            properties.collided = false;

            for (int i = 0; i < amount; i++) {
                RaycastHit2D hit = Physics2D.Raycast(a + (spacingDir * spacing * i), direction, rayLength, collisionMask);
                bool isOneway = hit.collider.gameObject.tag == "Oneway Platform";
                bool canCollideOneway = (!(isOneway && direction != Vector2.down) && !(isOneway && PassthroughOneways));
                if (hit.collider != null && (canCollideOneway || (!canCollideOneway && !hit.collider.isTrigger))) {
                    
                    if(debug) Debug.DrawRay(a + (spacingDir * spacing * i), direction * rayLength, Color.green);
                    properties.collided = true;
                    properties.hit = hit;
                    return properties;
                } else {
                    if(debug) Debug.DrawRay(a + (spacingDir * spacing * i), direction * rayLength, Color.red);
                }
            }

            return properties;
        }

        /// <summary>
        /// Takes the current ray hit, and if its inside the collision bounds we will move the object to where the ray hit is outside the bounds
        /// </summary>
        private void Unstuck(RaycastHit2D hit2D, Vector3 direction) {
            if (hit2D.distance < SkinThickness) {
                float offset = SkinThickness - hit2D.distance;
                this.transform.position += offset * direction;
            }
        }

        private struct CollisionProperties {
            public bool collided;
            public RaycastHit2D hit;

        }


        void LateUpdate() {
            UpdateCollisions();
            if(AutoUnstuck)UpdateUnstucks();
        }
        
        public void UpdateUnstucks() {
            if (isCollidingTop) Unstuck(collisionTop.hit, new Vector3(0, -1));
            if (isCollidingBottom) Unstuck(collisionBottom.hit, new Vector3(0, 1));
            if (isCollidingLeft) Unstuck(collisionLeft.hit, new Vector3(1, 0));
            if (isCollidingRight) Unstuck(collisionRight.hit, new Vector3(-1, 0));
        }
    }
}
