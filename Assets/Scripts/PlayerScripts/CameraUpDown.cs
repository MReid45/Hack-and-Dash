using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraUpDown : MonoBehaviour {

	public Camera thisCam;
	Transform thisTransform;
	private float moveVertical;

	public float upperCamThresh = 3;
	public float lowerCamThresh = -3;

	private float yPos;
	private float defaultYPos;


	// Use this for initialization
	void Start () {
		
		defaultYPos = thisCam.transform.localPosition.y;
		upperCamThresh = defaultYPos + 3;
		lowerCamThresh = defaultYPos - 3;
	}
	
	// Update is called once per frame
	void Update () {
		yPos = thisCam.transform.localPosition.y;
		//Need to sue vectors
		moveVertical = Input.GetAxis ("Vertical");
		//Player presses "W" to move camera up
		if (moveVertical > 0)
		{
			if (yPos <= upperCamThresh)
			{
				yPos += 4 * Time.deltaTime;
				thisCam.transform.localPosition = new Vector3 (thisCam.transform.localPosition.x, yPos,thisCam.transform.localPosition.z);

			}
			 
		}

		//Reset camera if player isnt touching either
		if (moveVertical == 0)
		{
			if (yPos >= defaultYPos)
			{
				yPos -= 6 * Time.deltaTime;

			}

			if (yPos <= defaultYPos)
			{
				yPos += 6 * Time.deltaTime;
			}
			thisCam.transform.localPosition = new Vector3 (thisCam.transform.localPosition.x, yPos, thisCam.transform.localPosition.z);
		}

		if (moveVertical < 0)
		{
			if (yPos >= lowerCamThresh)
			{
				yPos -= 4 * Time.deltaTime;
				thisCam.transform.localPosition = new Vector3 (thisCam.transform.localPosition.x, yPos,thisCam.transform.localPosition.z);
			}

		}
	}
}
