using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : Interactable {

	bool switchedOn = false;


	public override void Interact(GameObject thisObject)
	{

		if (Input.GetKeyDown (KeyCode.F))
		{
			if (!switchedOn)
			{
				Debug.Log ("Switch is on!");
				switchedOn = true;
			}
			else if (switchedOn){
				switchedOn = false;
				Debug.Log ("Switch is off");
			}

		}


		
	}
}
