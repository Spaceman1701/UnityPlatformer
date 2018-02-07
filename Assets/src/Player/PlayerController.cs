using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entity2D;

namespace Player
{
    [RequireComponent(typeof(MoveController2D))]
    public class PlayerController : MonoBehaviour
    {

        private MoveController2D controller;

        public float jumpHeight;
        public float timeToMaxHeight;
        public float timeFromMaxHeight;

        public float maxWalkSpeed;
        public float timeToMaxWalkSpeed;
        public float timeToStop;


        private float walkAcc;
        private float walkDec;

        private float jumpVelocity;
        private float fallingGravity;
        private float jumpingGravity;

        public Vector2 velocity;

        public bool isJumping = false; //either falling or jumping.. useful for choosing gravity

        // Use this for initialization
        void Start()
        {
            velocity = new Vector2(0, 0);
            controller = GetComponent<MoveController2D>();

            walkAcc = 1;
            ComputeGravityAndJumpHeight();
        }


        private void ComputeGravityAndJumpHeight()
        {

            jumpingGravity = (2 * jumpHeight) / (timeToMaxHeight * timeToMaxHeight);
            fallingGravity = (2 * jumpHeight) / (timeFromMaxHeight * timeFromMaxHeight);
            jumpVelocity = jumpingGravity * timeToMaxHeight;
            Debug.Log(fallingGravity + ", " + jumpVelocity) ;
        }

        // Update is called once per frame
        void Update()
        {

            if (controller.State.collisionDown || controller.State.collisionUp)
            {
                velocity.y = 0;
            }

            if (isJumping && velocity.y <= 0)
            {
                isJumping = false;
            }

            UpdateWalk();
            DoJump();

            if (isJumping)
            {
                velocity.y -= jumpingGravity * Time.deltaTime;
            } else
            {
                velocity.y -= fallingGravity * Time.deltaTime;
            }

            controller.Move(velocity * Time.deltaTime);
        }


        private void UpdateWalk()
        {
            float inputValue = Input.GetAxis("Horizontal");


            if (inputValue == 0)
            {
                velocity.x = 0;
            }
            else
            {
                float acceleration = inputValue * walkAcc;

                velocity.x = velocity.x + acceleration;

                velocity.x = Mathf.Clamp(velocity.x, -maxWalkSpeed, maxWalkSpeed);
            }
        }


        private void DoJump()
        {
            if (Input.GetButtonDown("Jump") && controller.State.collisionDown)
            {
                Debug.Log("jumping");
                velocity.y = jumpVelocity;
                isJumping = true;
            }
        }
    }
}

