using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerScore : MonoBehaviour {

	public int levelScore;
	public int totalScore;
	int noDeathBonus;
	int allCoinsBonus;
	public Text scoreText;
	public int bitValue = 100;
	AudioSource source;


	// Use this for initialization
	void Start () {
		levelScore = 0;
		UpdateScore ();
		source = this.GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	public void AddScore(int value)
	{
		levelScore += value;
		UpdateScore ();
		source.Play ();
	}

	public void SubtractScore(int value)
	{
		levelScore -= value;
		if (levelScore < 0)
			levelScore = 0;
		UpdateScore ();
	}

	public void SetScore(int newScore)
	{
		levelScore = newScore;
	}

	public void UpdateScore()
	{
		scoreText.text = "Score: " + levelScore;
	}
}
