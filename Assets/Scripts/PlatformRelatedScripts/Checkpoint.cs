using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

	protected Vector2 myPosition;
	AudioSource source;
	bool hasPassed;
	 public ParticleSystem particles;
	void Awake() {
		myPosition = this.GetComponent<Transform>().position;
		source = GetComponent<AudioSource> ();
		hasPassed = false;
		particles.Stop ();
	}
	// Enters
	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Player") 
		{
			if (hasPassed == false)
			{
				particles.Play ();
				StartCoroutine (Sparks (1f));
				source.Play ();
				col.gameObject.GetComponent<PlayerController> ().SetCheckpoint (myPosition);
				hasPassed = true;
			}
		} 

	}

	IEnumerator Sparks(float delay)
	{
		yield return new WaitForSecondsRealtime (delay);
		particles.Stop ();
	}
}

