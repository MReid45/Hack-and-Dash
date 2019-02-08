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



public class NewFinishLine : MonoBehaviour {

	// AudioClip for finish
	public AudioClip completion;
	private AudioSource source;
	public int noDeathBonus = 1000;
	public int allCoinBonus = 2000;
	int baseScore;
	int numCoins;
	GameObject[] coinsList; 
	public GameObject endLevelCan;
	public Text baseScoreText;
	public Text noDeathText;
	public Text allCoinsText;
	public Text totalScoreText;
	public Text winText;
	public Text nextLevelText;
	// Main menu scene
	public string mainMenuScene = "MainMenu";
	//public Text winText;
	private PlayerScore playerScore;
	private PlayerController player;
	// Use this for initialization
	int totalScore;
	bool hasPassed;

	void Start () {
		GameObject playerScoreObject = GameObject.Find("PlayerScore");
		GameObject playerControllerObject = GameObject.FindGameObjectWithTag ("Player");
		playerScore = playerScoreObject.GetComponent<PlayerScore>();
		player = playerControllerObject.GetComponent<PlayerController> ();
		coinsList = GameObject.FindGameObjectsWithTag("Coin");
		//Debug.Log (coinsList.Length);
		source = GetComponent<AudioSource> ();
		endLevelCan.SetActive (false);
		hasPassed = false;
	}

	// OnTriggerEnter
	void OnTriggerEnter2D(Collider2D other)
	{
		baseScore = playerScore.levelScore;
		coinsList = GameObject.FindGameObjectsWithTag("Coin");
		numCoins = coinsList.Length;
		// Once player crosses the finish line
		if (other.gameObject.tag == "Player") {
			if (hasPassed == false)
			{
				source.PlayOneShot (completion);
				hasPassed = true;
			}
			endLevelCan.SetActive (true);
			if ((SceneManager.GetActiveScene().buildIndex + 1) < SceneManager.sceneCountInBuildSettings) // scales depending on number of levels in build
			{

				winText.text = "Level Cleared!"; // Maybe have player do idle animation
			
			} else {
				winText.text = "Congratulations You Win!"; // Player finishes last level, can change button text the same way
				nextLevelText.text = "Main Menu";

			}
			StartCoroutine (PauseDelay (1f));
		}

		//End of level score calcualations
		baseScoreText.text = "Base Score:  " + baseScore;

		//get player deaths number
		if (player.deathCount == 0)
		{
			noDeathBonus = 1000;
		} else
			noDeathBonus = 0;
		noDeathText.text = "No Death Bonus: " + noDeathBonus;

		// get total coins in scene
		if (numCoins == 0)
		{
			allCoinBonus = 2000;
		} else
			allCoinBonus = 0;
		allCoinsText.text = "All Coins Bonus: " + allCoinBonus;

		totalScore = baseScore + noDeathBonus + allCoinBonus;
		totalScoreText.text = "Total Level Score: \n" + totalScore;

		if (totalScore > PlayerPrefs.GetInt("HighScorelevel" + SceneManager.GetActiveScene().buildIndex))
		PlayerPrefs.SetInt ("HighScorelevel" + SceneManager.GetActiveScene().buildIndex, totalScore);
		//if (SceneManager

	}
		
	// Moves to the next level in the build, or moves to Mainmenu if all levels are complete
	public void NextLevel()
	{

		if ((SceneManager.GetActiveScene ().buildIndex + 1) < SceneManager.sceneCountInBuildSettings) {
			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1);
			Time.timeScale = 1;
		}
		else {
			SceneManager.LoadScene ("Main Menu");
		}

	}
	// Waits for a time before moving to the next level
	IEnumerator PauseDelay(float time)
	{
		yield return new WaitForSecondsRealtime (time);
		Time.timeScale = 0;

	}
}
