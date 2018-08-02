using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	private Scene CurrentScene;

	// Use this for initialization
	void Start () {
		CurrentScene = SceneManager.GetActiveScene();
		//Debug.Log("Current scene index is : " + CurrentScene.buildIndex);
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void LoadNextScene()
	{
		if (SceneManager.sceneCount >= CurrentScene.buildIndex) {
			SceneManager.LoadScene(CurrentScene.buildIndex + 1);
		} else {
			//Debug.LogWarning("there no " + (CurrentScene.buildIndex + 2) + " index existing in build");
		}
	}
}
