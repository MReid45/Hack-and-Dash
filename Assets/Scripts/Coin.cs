using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

	private PlayerScore playerScore;
	public int coinValue = 100;



	//Coin float stuff
	public float amplitude;          //Set in Inspector 
	public float speed;                  //Set in Inspector 
	private float tempVal;
	private Vector3 tempPos;

	// Use this for initialization
	void Start () {
		GameObject playerScoreObject = GameObject.FindGameObjectWithTag ("PlayerScore");
		playerScore = playerScoreObject.GetComponent<PlayerScore> ();
		tempVal = transform.position.y;

	}

	void Update()
	{
		tempPos.y = tempVal + amplitude * Mathf.Sin(speed * Time.time);
		transform.position = new Vector3(transform.position.x,tempPos.y,transform.position.z);
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.CompareTag ("Player"))
		{
			playerScore.AddScore (coinValue);
			Destroy (this.gameObject);

		}
			
	}
}
