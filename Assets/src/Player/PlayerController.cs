using UnityEngine;


namespace Player
{
    public class PlayerController : MonoBehaviour
    {

        private struct CollisionState
        {
            public bool falling;
            public bool jumping;

            public bool collisionUp;
            public bool collisionDown;
            public bool collisionLeft;
            public bool collisionRight;

            public void Reset()
            {
                falling = false;
                jumping = false;
                collisionDown = false;
                collisionLeft = false;
                collisionRight = false;
                collisionUp = false;
            }
        }

        public BoxCollider hitbox;
        public LayerMask terrian;

        public float jumpHeight;
        public float timeToMaxHeight;
        public float timeFromMaxHeight;

        public float maxWalkSpeed;
        public float timeToMaxWalkSpeed;
        public float timeToStop;

        public Vector2Int collisionResolution;

        public float collisionMargin = 0f;



        private Vector2 hitboxExtents;

        private float upGravity;
        private float downGravity;
        private float jumpVelocity;

        private float walkAcc;
        private float walkDec;

        private CollisionState collisionState;

        public Vector2 velocity;

        // Use this for initialization
        public void Start()
        {
            Debug.Log("init player controller");

            hitboxExtents = new Vector2(hitbox.size.x / 2.0f, hitbox.size.y / 2.0f);

            walkAcc = 2f;
            walkDec = 1;
        }

        // Update is called once per frame
        public void Update()
        {
            collisionState.Reset();
            velocity += new Vector2(0, -0.5f) * Time.deltaTime;
            UpdateWalk();
            Vector2 pos = new Vector2(transform.position.x, transform.position.y);
            HandleCollisionBottom(pos);
            HandleCollisionRight(pos);
            HandleCollisionLeft(pos);
            DoJump();

            Vector2 newPos = pos + velocity ;
            transform.position = new Vector3(newPos.x, newPos.y);
        }

        private void DoJump()
        {
            if (Input.GetButton("Jump") && collisionState.collisionDown)
            {
                Debug.Log("jumping");
                velocity += new Vector2(0, 10) * Time.deltaTime;
            }
        }

        private void UpdateWalk()
        {
            float inputValue = Input.GetAxis("Horizontal");

            
            if (inputValue == 0)
            {
               velocity.x = 0;
            } else
            {
                float acceleration = inputValue * walkAcc * Time.deltaTime;

                velocity.x = velocity.x + acceleration;

                if (velocity.x > 0)
                {
                    velocity.x = Mathf.Min(maxWalkSpeed * Time.deltaTime, velocity.x);
                } else if (velocity.x < 0)
                {
                    velocity.x = Mathf.Max(-maxWalkSpeed * Time.deltaTime, velocity.x);
                }
            }
        }


        private void HandleCollisionBottom(Vector2 position)
        {
            Vector2 startPos = new Vector2(position.x - hitboxExtents.x, position.y - hitboxExtents.y);
            Vector2 endPos = new Vector2(position.x + hitboxExtents.y, position.y - hitboxExtents.y);

            float collisionDistance = CheckCollision(startPos, endPos, Vector2.down, Mathf.Abs(velocity.y), collisionResolution.x);

            if (collisionDistance < Mathf.Abs(velocity.y))
            {
                velocity.y = collisionDistance * Mathf.Sign(velocity.y);
                collisionState.collisionDown = true;
            }
        }

        private void HandleCollisionRight(Vector2 position)
        {
            Vector2 startPos = new Vector2(position.x + hitboxExtents.x, position.y + hitboxExtents.y);
            Vector2 endPos = new Vector2(position.x + hitboxExtents.x, position.y - hitboxExtents.y + 0.001f);

            float collisionDistance = CheckCollision(startPos, endPos, Vector2.right, Mathf.Abs(velocity.x), collisionResolution.x);
            if (collisionDistance < Mathf.Abs(velocity.x) && velocity.x > 0)
            {
                velocity.x = collisionDistance * Mathf.Sign(velocity.x);
            }
        }

        private void HandleCollisionLeft(Vector2 position)
        {
            Vector2 startPos = new Vector2(position.x - hitboxExtents.x, position.y + hitboxExtents.y);
            Vector2 endPos = new Vector2(position.x - hitboxExtents.x, position.y - hitboxExtents.y + 0.001f);

            float collisionDistance = CheckCollision(startPos, endPos, Vector2.right, Mathf.Abs(velocity.x), collisionResolution.x);
            if (collisionDistance < Mathf.Abs(velocity.x) && velocity.x < 0)
            {
                velocity.x = collisionDistance * Mathf.Sign(velocity.x);
            }
        }


        private float CheckCollision(Vector2 startPos, Vector2 endPos, Vector2 dir, float maxDist, int resolution)
        {
            float rayDistance = maxDist + collisionMargin;

            for (int i = 0; i < resolution; i++)
            {
                Vector2 rayOrigin = Vector2.Lerp(startPos, endPos, (float)i / (resolution - 1));

                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, dir, rayDistance, terrian);    
                if (hit && hit.distance < rayDistance)
                {
                    rayDistance = hit.distance;
                }
            }

            return rayDistance;
        }
    }
}

