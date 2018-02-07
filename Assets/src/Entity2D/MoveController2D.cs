using UnityEngine;


namespace Entity2D
{
    public class MoveController2D : MonoBehaviour
    {

        private const float HITBOX_MARGIN = 0.0001f;

        public struct CollisionState
        {
            public bool collisionUp;
            public bool collisionDown;
            public bool collisionLeft;
            public bool collisionRight;

            public void Reset()
            {
                collisionDown = false;
                collisionLeft = false;
                collisionRight = false;
                collisionUp = false;
            }
        }

        public BoxCollider hitbox;
        public LayerMask terrian;



        public Vector2Int collisionResolution;

        public float collisionMargin = 0f;


        private Vector2 hitboxExtents;

        private CollisionState collisionState;

        // Use this for initialization
        public void Start()
        {
            Debug.Log("init player controller");

            hitboxExtents = new Vector2(hitbox.size.x / 2.0f, hitbox.size.y / 2.0f);

        }


        // Update is called once per frame
        public void Move(Vector2 velocity)
        {
            collisionState.Reset();
            HandleVerticalCollisions(ref velocity);
            HandleHorizontalCollisions(ref velocity);

            transform.Translate(new Vector3(velocity.x, velocity.y));
        }



        private void HandleVerticalCollisions(ref Vector2 velocity)
        {
            Vector2 position = new Vector2(transform.position.x, transform.position.y);

            float maxDist = hitboxExtents.y + Mathf.Abs(velocity.y);
            Vector2 rayDir = new Vector2(0, Mathf.Sign(velocity.y));

            Vector2 startPos = position - new Vector2(hitboxExtents.x - HITBOX_MARGIN, 0);
            Vector2 endPos = position + new Vector2(hitboxExtents.x - HITBOX_MARGIN, 0);

            float collisionDist = CheckCollision(startPos, endPos, rayDir, maxDist, collisionResolution.x);

            if (collisionDist < maxDist)
            {
                collisionState.collisionDown = velocity.y < 0;
                collisionState.collisionUp = velocity.y > 0;

                velocity.y = Mathf.Sign(velocity.y) * (collisionDist - hitboxExtents.y);

            }
        }

        private void HandleHorizontalCollisions(ref Vector2 velocity)
        {
            Vector2 position = new Vector2(transform.position.x, transform.position.y);

            float maxDist = hitboxExtents.x + Mathf.Abs(velocity.x);
            Vector2 rayDir = new Vector2(Mathf.Sign(velocity.x), 0);

            Vector2 startPos = position - new Vector2(0, hitboxExtents.y - HITBOX_MARGIN);
            Vector2 endPos = position + new Vector2(0, hitboxExtents.y - HITBOX_MARGIN);

            float collisionDist = CheckCollision(startPos, endPos, rayDir, maxDist, collisionResolution.y);

            if (collisionDist < maxDist)
            {

                collisionState.collisionLeft = velocity.x < 0;
                collisionState.collisionRight = velocity.x > 0;

                velocity.x = Mathf.Sign(velocity.x) * (collisionDist - hitboxExtents.x);
            }
        }


        private float CheckCollision(Vector2 startPos, Vector2 endPos, Vector2 dir, float maxDist, int resolution)
        {
            float rayDistance = maxDist + collisionMargin;

            for (int i = 0; i < resolution; i++)
            {
                Vector2 rayOrigin = Vector2.Lerp(startPos, endPos, (float)i / (resolution - 1));
                Debug.DrawRay(rayOrigin, dir * rayDistance);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, dir, rayDistance, terrian);    
                if (hit && hit.distance < rayDistance)
                {
                    rayDistance = hit.distance;
                }
            }

            return rayDistance;
        }

        public CollisionState State
        {
            get
            {
                return collisionState;
            }
        }
    }
}

