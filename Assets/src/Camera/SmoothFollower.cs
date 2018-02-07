using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollower : MonoBehaviour {



    public float horizontalConvergeRate = 0.0001f;
    public float verticalConvergeRate = 0.00025f;

    public Transform target;

	// Use this for initialization
	void Start () {
		
	}

    private void LateUpdate()
    {

        float newX = (1 - horizontalConvergeRate) * transform.position.x + horizontalConvergeRate * target.position.x; 
        float newY = (1 - verticalConvergeRate) * transform.position.y + verticalConvergeRate * target.position.y;

        transform.position = new Vector3(newX, newY, transform.position.z);
    }
}
