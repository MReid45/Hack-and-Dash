using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

	string sceneName;
	public GameObject pauseMenu;

	void Awake()
	{
		Scene currentScene = SceneManager.GetActiveScene();
		sceneName = currentScene.name;
	}

	// Use this for initialization
	void Start () {
		pauseMenu.gameObject.SetActive (false);
	}
		
		

	void UnPause()
	{
		GameManager.instance.isPaused = false;
		pauseMenu.gameObject.SetActive (false);
		Time.timeScale = 1;
	}

	public void ResumeGame()
	{
		UnPause ();
	}

	public void QuitGame()
	{
		SceneManager.LoadSceneAsync (0);
		Time.timeScale = 1;
	}

	public void RestartLevel()
	{
		SceneManager.LoadScene (sceneName);
		Time.timeScale = 1;
	}
}
