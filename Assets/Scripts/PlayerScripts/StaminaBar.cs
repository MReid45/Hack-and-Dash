using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaBar : MonoBehaviour {

	Vector3 localScale;

	// Use this for initialization
	void Start () {
		localScale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		localScale.x = RopeSystem.playerStamina / 15;
		transform.localScale = localScale;
	}
}
