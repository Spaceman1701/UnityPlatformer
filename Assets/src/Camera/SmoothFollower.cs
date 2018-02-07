using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Camera
{
    public class SmoothFollower : MonoBehaviour
    {



        public float horizontalConvergeRate;
        public float verticalConvergeRate;

        public CameraFollowTarget target;

        // Use this for initialization
        void Start()
        {

        }

        private void LateUpdate()
        {


            float newX = transform.position.x;
            float newY = transform.position.y;

            newX += (target.Position.x - newX) * horizontalConvergeRate * Time.deltaTime;

            if (target.IsLanded || (target.Velocity.y < 0 && transform.position.y > target.Position.y))
            {
                newY += (target.Position.y - newY) * verticalConvergeRate * Time.deltaTime;
            }

            transform.position = new Vector3(newX, newY, transform.position.z);
        }
    }
}

