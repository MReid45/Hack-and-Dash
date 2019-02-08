using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour {


	public PlayerController player;
	AudioSource source;
	void Start()
	{
		source = GetComponent<AudioSource> ();
		GameObject playerControllerObject = GameObject.FindGameObjectWithTag ("Player");
		player = playerControllerObject.GetComponent<PlayerController> ();
	}

	// Enters hazard collision
	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Player") 
		{
			source.Play ();
			col.gameObject.GetComponent<PlayerController>().Die();

		} 

	}
}
