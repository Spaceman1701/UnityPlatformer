using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Camera
{
    public abstract class CameraFollowTarget : MonoBehaviour
    {

        public abstract Vector2 Position
        {
            get;
        }

        public abstract Vector2 Velocity
        {
            get;
        }

        public abstract Vector2 Acceleration
        {
            get;
        }

        public abstract bool IsLanded
        {
            get;
        }
    }
}

