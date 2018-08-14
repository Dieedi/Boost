using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	[SerializeField] IntReference StartingLevel;
	// [SerializeField] IntReference CurrentLevel;
	[SerializeField] LevelManager lm;
	[SerializeField] SaveManager sm;

	private void Awake()
	{
		if (instance != null) {
			Destroy(gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}

	private void Start()
	{
		StartingLevel.value = lm.GetStartScene;
	}

	public void OnClickStart ()
	{
		lm.LoadNextScene();
	}

	public void OnClickContinue()
	{
		lm.LoadScene(sm.SavedLevel.value);
	}
}
