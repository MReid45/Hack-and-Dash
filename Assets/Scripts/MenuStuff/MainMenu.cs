using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	[SerializeField] private Canvas settingsMenu;
	public GameObject mainPanel;
	public GameObject highScorePanel;
	public GameObject controlsPanel;

	public Text highScoreText;
	bool hasdisplayedText;
	void Start()
	{
		highScorePanel.SetActive (false);
		controlsPanel.SetActive (false);
		mainPanel.SetActive (true);
		hasdisplayedText = false;
	}


	public void StartGame(){
		SceneManager.LoadSceneAsync(1);
		Time.timeScale = 1;
	}


	//Moves to setting menu
	public void Settings(){
	}

	public void HighScores()
	{
		highScorePanel.SetActive (true);
		mainPanel.SetActive (false);
		ListScores ();
	}

	public void ControlsButton(){
        controlsPanel.SetActive(true);
        mainPanel.SetActive(false);
	}

	public void ListScores()
	{
		if (hasdisplayedText == false)
		{
			for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
			{
				//PlayerPrefs.SetInt ("HighScorelevel" + i, 1000);
				highScoreText.text += "Level " + i + " Score: " + PlayerPrefs.GetInt ("HighScorelevel" + i) + "\n\n";

			}
			hasdisplayedText = true;
		}
	}

	public void Back()
	{
		mainPanel.SetActive (true);
		if (highScorePanel.activeSelf == true)
		{
			highScorePanel.SetActive (false);
		}

		if (controlsPanel.activeSelf == true)
		{
			controlsPanel.SetActive (false);
		}


	}

	public void ResetScores()
	{
		PlayerPrefs.DeleteAll ();
		ListScores ();
		SceneManager.LoadSceneAsync (0);
	}

	public void Quit(){
		PlayerPrefs.Save ();
		Application.Quit();
	}
}
