using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Player
{
    public class PlayerController : MonoBehaviour
    {

        public Sprite sprite;
        public float jumpHeight;
        public float timeToMaxHeight;
        public float timeFromMaxHeight;

        private Vector2 downRayStart;
        private Vector2 upRayStart;
        private Vector2 leftRayStart;
        private Vector2 rightRayStart;

        private float upGravity;
        private float downGravity;

        private float jumpVelocity;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

