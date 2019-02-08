//FinishLineScript.cs
// Written by Nick O'Donnell

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Finish line script. Attached to the Finishline gameobject, Triggers when player collides with FinishLine
/// Plays music and displays text when player beats level
/// </summary>



public class FinishLine : MonoBehaviour {

	// AudioClip for finish
	public AudioClip completion;
	private AudioSource source;
	int noDeathBonus;
	int allCoinBonus;
	GameObject[] coinsList; 

	// Main menu scene
	public string mainMenuScene = "MainMenu";
	public Text winText;
	private PlayerScore playerScore;
	// Use this for initialization
	void Start () {
		GameObject playerScoreObject = GameObject.FindGameObjectWithTag ("PlayerScore");
		coinsList = GameObject.FindGameObjectsWithTag("Coin");
		source = GetComponent<AudioSource> ();
		winText.enabled = false;
	}
	
	// OnTriggerEnter
	void OnTriggerEnter2D(Collider2D other)
	{
		// Once player crosses the finish line
		if (other.gameObject.tag == "Player") {
		//	source.PlayOneShot(completion);
			winText.enabled = true;
			if ((SceneManager.GetActiveScene().buildIndex + 1) < SceneManager.sceneCountInBuildSettings) // scales depending on number of levels in build
			{
				Debug.Log ("Player has reached Finish");

				winText.text = "Level Cleared!"; // Maybe have player do idle animation
				StartCoroutine (WinDelay (1f));

			} else {
				winText.text = "Congratulations You Win!"; // Player finishes last level
				StartCoroutine (WinDelay (2f));
			}
		}
	}


	// Moves to the next level in the build, or moves to Mainmenu if all levels are complete
		void NextLevel(){

		if ((SceneManager.GetActiveScene ().buildIndex + 1) < SceneManager.sceneCountInBuildSettings) {
			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1);
		}
		else {
			SceneManager.LoadScene ("Main Menu");
		}

	}
	// Waits for a time before moving to the next level
	IEnumerator WinDelay(float time)
	{
		yield return new WaitForSecondsRealtime (time);
		NextLevel();

	}
}
