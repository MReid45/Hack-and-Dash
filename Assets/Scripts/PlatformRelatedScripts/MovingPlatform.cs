/// <summary>
/// Worked on by: Mark Porowicz
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>

public class MovingPlatform : MonoBehaviour {

    public GameObject platform;
    public float moveSpeed;
    public Transform currentPoint;
    public Transform[] points;
    public int pointSelection;

	// Use this for initialization
	void Start () {
        currentPoint = points[pointSelection];
	}
	
	// Update is called once per frame
	void Update () {

        platform.transform.position = Vector3.MoveTowards(platform.transform.position, currentPoint.position, Time.deltaTime * moveSpeed);

        // Sets the node to move back and forth from the two points
        if(platform.transform.position == currentPoint.position)
        {
            pointSelection++;

            if(pointSelection == points.Length)
            {
                pointSelection = 0;
            }

            currentPoint = points[pointSelection];

        }
	}




	
	// Detects collision with the moving platform
	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			other.collider.transform.SetParent (transform);
		}

	}

	// Detects continous collision with the moving platform
	void OnCollisionStay2D(Collision2D hit) {
		if (hit.gameObject.tag == "Player") {
			hit.collider.transform.SetParent (transform);
		}
	}

	// Detects collision exit with the moving platform
	void OnCollisionExit2D(Collision2D exit) {

		if (exit.gameObject.tag == "Player")
		{
			exit.collider.transform.SetParent(null);
		}
	}
	
}