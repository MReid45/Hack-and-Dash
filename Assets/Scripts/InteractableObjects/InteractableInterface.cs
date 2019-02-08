using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface InteractableInterface {


	bool getInteractable ();

	void OnTriggerStay2D (Collider2D col);

	void OnTriggerExit2D (Collider2D col);

	void Interact(GameObject thisObject);



}
