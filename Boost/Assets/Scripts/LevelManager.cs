using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	public static LevelManager instance;
	
	[SerializeField] IntReference StartScene;
	[SerializeField] UnityEvent e_levelLoadEnd;
	[NonSerialized] public Scene CurrentScene;
	
	private SaveManager sm;

	public int GetStartSceneIndex
	{
		get { return StartScene.value; }
	}

	private void OnEnable()
	{
		SceneManager.sceneLoaded += OnLevelLoadDone;
	}

	private void OnLevelLoadDone(Scene scene, LoadSceneMode mode)
	{
		//Debug.Log("should cast event");
		CurrentScene = SceneManager.GetActiveScene();
		//Debug.Log("current scene : " + SceneManager.GetActiveScene());
		if (scene.buildIndex != 0 && !CurrentScene.name.Contains("enlighten")) {
			sm.SavedLevel.value = CurrentScene.buildIndex;
			sm.Save();
		}

		e_levelLoadEnd.Invoke();
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
		sm = FindObjectOfType<SaveManager>();
	}

	//private void OnLevelWasLoaded(int level)
	//{
	//	CurrentScene = SceneManager.GetActiveScene();

	//	if (level != 0 && !CurrentScene.name.Contains("enlighten")) {
	//		sm.SavedLevel.value = CurrentScene.buildIndex;
	//		sm.Save();
	//	}
	//}

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
		//Debug.Log("LoadScene index : " + index);
		SceneManager.LoadScene(index);
	}

	public void ReloadCurrentScene()
	{
		SceneManager.LoadScene(CurrentScene.buildIndex);
	}
}
