﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entity2D;
using Camera;

namespace Player
{
    [RequireComponent(typeof(MoveController2D))]
    public class PlayerController : CameraFollowTarget
    {

        private MoveController2D controller;

        public float jumpHeight;
        public float jumpDistance;

        public float fallSpeedMultiplier;
        public float partialJumpMultiplier;

        public float maxWalkSpeed;
        public float timeToMaxWalkSpeed;
        public float timeToStop;
        public float timeToMaxAirSpeed;
        public float timeToAirStop;

        public bool fastReverseInAir = true;


        private float walkAcc;
        private float walkDec;

        private float airWalkAcc;
        private float airWalkDec;

        private float jumpVelocity;
        private float fallingGravity;
        private float jumpingGravity;
        private float partialJumpGravity;

        public Vector2 velocity;

        public bool isJumping = false; //either falling or jumping.. useful for choosing gravity
        public bool releasedJumpEarly = false; //if player is doing a partial jump

        // Use this for initialization
        void Start()
        {
            velocity = new Vector2(0, 0);
            controller = GetComponent<MoveController2D>();

            ComputeGravityAndJumpHeight();
            ComputeWalkAcceleration();
        }


        private void ComputeGravityAndJumpHeight()
        {
            jumpingGravity = (2 * jumpHeight * maxWalkSpeed * maxWalkSpeed) / (jumpDistance * jumpDistance);
            fallingGravity = fallSpeedMultiplier * jumpingGravity;
            partialJumpGravity = partialJumpMultiplier * jumpingGravity;
            jumpVelocity = (2 * jumpHeight * maxWalkSpeed) / jumpDistance;


            Debug.Log(fallingGravity + ", " + jumpVelocity) ;
        }

        private void ComputeWalkAcceleration()
        {
            walkAcc = maxWalkSpeed / timeToMaxWalkSpeed;
            walkDec = maxWalkSpeed / timeToStop;

            airWalkAcc = maxWalkSpeed / timeToMaxAirSpeed;
            airWalkDec = maxWalkSpeed / timeToAirStop;
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
                releasedJumpEarly = false;
            }

            UpdateWalk();
            DoJump();

            UpdateGravity();

            controller.Move(velocity * Time.deltaTime);
        }

        private void UpdateGravity()
        {
            if (isJumping && !releasedJumpEarly)
            {
                velocity.y -= jumpingGravity * Time.deltaTime;
            }
            else if (releasedJumpEarly)
            {
                velocity.y -= partialJumpGravity * Time.deltaTime;
            }
            else
            {
                velocity.y -= fallingGravity * Time.deltaTime;
            }
        }


        private void UpdateWalk()
        {
            float inputValue = Input.GetAxis("Horizontal");
            float acceleration;
            float decceleration;

            bool canDoFastReverse = controller.State.collisionDown || fastReverseInAir;

            if (controller.State.collisionDown) //grounded
            {
                acceleration = inputValue * walkAcc * Time.deltaTime;
                decceleration = -1 * Mathf.Sign(velocity.x) * walkDec * Time.deltaTime;
            } else //air
            {
                acceleration = inputValue * airWalkAcc * Time.deltaTime;
                decceleration = -1 * Mathf.Sign(velocity.x) * airWalkDec * Time.deltaTime;
            }
            

            if (inputValue == 0)
            {
               

                if (Mathf.Abs(decceleration) >= Mathf.Abs(velocity.x))
                {
                    velocity.x = 0;
                } else
                {
                    velocity.x += decceleration;
                }
            }
            else if (Mathf.Sign(inputValue) == Mathf.Sign(velocity.x) || velocity.x == 0 || !canDoFastReverse)
            {
                

                velocity.x = velocity.x + acceleration;

                velocity.x = Mathf.Clamp(velocity.x, -maxWalkSpeed, maxWalkSpeed);
            } else if (canDoFastReverse)
            {
                velocity.x = -velocity.x; //fast reversal
            }
        }


        private void DoJump()
        {
            bool isHoldingJump = Input.GetButton("Jump");
            if (Input.GetButtonDown("Jump") && controller.State.collisionDown)
            {
                Debug.Log("jumping");
                velocity.y = jumpVelocity;
                isJumping = true;
            } else if (!isHoldingJump && isJumping) //released jump early
            {
                releasedJumpEarly = true;
            }
        }

        public override Vector2 Position
        {
            get
            {
                return new Vector2(transform.position.x, transform.position.y);
            }
        }

        public override Vector2 Velocity
        {
            get
            {
                return velocity;
            }
        }

        public override Vector2 Acceleration
        {
            get
            {
                return new Vector2();
            }
        }

        public override bool IsLanded
        {
            get
            {
                return controller.State.collisionDown;
            }
        }
    }
}

