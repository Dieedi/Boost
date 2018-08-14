using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuController : MonoBehaviour {

	public static GameMenuController instance;

	//[SerializeField] IntReference SavedLevel;
	[SerializeField] Button bt_continue;
	[SerializeField] GameObject p_startMenu;
	[SerializeField] GameObject p_PlayMenu;
	[SerializeField] LevelManager lm;
	[SerializeField] SaveManager sm;

	private int startLevel;
	private bool playMenuExist = false;

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
		HideContinue();
	}

	private void HideContinue()
	{
		if (sm.SavedLevel.value > startLevel) {
			bt_continue.interactable = true;
		}
	}

	private void OnLevelWasLoaded(int level)
	{
		startLevel = lm.GetStartScene;

		if (startLevel == lm.CurrentScene.buildIndex) {
			p_PlayMenu.SetActive(false);
			p_startMenu.SetActive(true);
		} else {
			p_PlayMenu.SetActive(true);
			p_startMenu.SetActive(false);
		}

		HideContinue();
	}

	public void OnClickRestart()
	{
		lm.ReloadCurrentScene();
	}
}
