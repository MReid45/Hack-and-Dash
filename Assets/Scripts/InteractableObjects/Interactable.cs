using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour, InteractableInterface {

	bool isInteractable = false;


	public bool getInteractable ()
	{
		return isInteractable;
	}

	public void OnTriggerStay2D (Collider2D col)
	{
		//Debug.Log ("Colliding");

		if (col.gameObject.CompareTag("Player"))
		{
			isInteractable = true;

			Interact (this.gameObject);
		}
	}

	public void OnTriggerExit2D (Collider2D col)
	{
		isInteractable = false;
	}

	public virtual void Interact(GameObject thisObject)
	{
		// inherited methods will override this
	}

}
