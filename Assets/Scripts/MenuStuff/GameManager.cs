using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {


	public GameObject pauseMenu;
	private static GameManager gm;
	string sceneName;
	public static GameManager instance = null;  

	public bool isPaused = false; 
	void Awake()
	{

		//Check if instance already exists
		if (instance == null)

			//if not, set instance to this
			instance = this;

		//If instance already exists and it's not this:
		else if (instance != this)

			//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
			Destroy(gameObject);   



		Scene currentScene = SceneManager.GetActiveScene();
		sceneName = currentScene.name;
	}

	// Use this for initialization
	void Start () {
		pauseMenu.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		//Player hits escape to pause game nd display menu
		// If the escape key is pressed, toggle the pause state.
		if (Input.GetKeyDown (KeyCode.P))
			TogglePause ();
		}


	public bool getPaused()
	{
		return isPaused;
	}
	void TogglePause()
	{
		if (!isPaused)
			Pause ();
		else
			Unpause ();
	}
	public void Pause()
	{
		pauseMenu.gameObject.SetActive (true);
		Time.timeScale = 0;
		isPaused = true;
	}

	public void Unpause()
	{
		isPaused = false;
		Time.timeScale = 1;
		pauseMenu.gameObject.SetActive (false);
	}

	public void ResumeGame()
	{
		Unpause ();
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
		pauseMenu.gameObject.SetActive (false);
	}

}
