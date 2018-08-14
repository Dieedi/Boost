using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	public static LevelManager instance;

	//[SerializeField] IntReference SavedLevel;
	[SerializeField] IntReference StartScene;
	[SerializeField] SaveManager sm;

	[NonSerialized] public Scene CurrentScene;
	//private Scene StartScene;

	public int GetStartScene
	{
		get { return StartScene.value; }
	}

	private void Awake()
	{
		if (instance != null) {
			Destroy(gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad(gameObject);
		}

		StartScene.value = SceneManager.GetActiveScene().buildIndex;
		CurrentScene = SceneManager.GetActiveScene();
	}

	private void OnLevelWasLoaded(int level)
	{
		CurrentScene = SceneManager.GetActiveScene();
		sm.SavedLevel.value = CurrentScene.buildIndex;

		if (level != 0 && !CurrentScene.name.Contains("enlighten")) {
			// SavedLevel.value = level;
			sm.Save();
		}
	}

	public void LoadNextScene()
	{
		if (CurrentScene.buildIndex + 1 < 4) {
			SceneManager.LoadScene(CurrentScene.buildIndex + 1);
		} else {
			SceneManager.LoadScene(StartScene.value);
		}
	}

	public void LoadScene(int index)
	{
		SceneManager.LoadScene(index);
	}

	public void ReloadCurrentScene()
	{
		SceneManager.LoadScene(CurrentScene.buildIndex);
	}
}
