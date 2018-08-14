using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuController : MonoBehaviour {

	public static GameMenuController instance;

	//[SerializeField] IntReference SavedLevel;
	[SerializeField] Button bt_continue;
	[SerializeField] GameObject p_startMenu;
	[SerializeField] GameObject p_optionsMenu;
	[SerializeField] GameObject p_PlayMenu;
	[SerializeField] GameObject b_joystick;
	[SerializeField] GameObject b_LeftRight;

	private int startLevelIndex;
	private bool playMenuExist = false;
	private LevelManager lm;
	private SaveManager sm;

	private void Awake()
	{
		if (instance != null) {
			Destroy(gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad(gameObject);
		}

		lm = FindObjectOfType<LevelManager>();
		sm = FindObjectOfType<SaveManager>();
	}

	private void Start()
	{
		HideContinue();
	}

	private void HideContinue()
	{
		if (sm.SavedLevel.value > startLevelIndex) {
			bt_continue.interactable = true;
		} else {
			bt_continue.interactable = false;
		}
	}

	public void OnEventLevelLoadEnd()
	{
		//Debug.Log("GMC : levelloadendevent, startlevelindex : " + startLevelIndex + ", current scene index : " + lm.CurrentScene.buildIndex);
		startLevelIndex = lm.GetStartSceneIndex;

		if (startLevelIndex == lm.CurrentScene.buildIndex) {
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

	public void OnClickOptions()
	{
		p_startMenu.SetActive(false);
		p_optionsMenu.SetActive(true);
	}

	public void OnClickBack()
	{
		p_startMenu.SetActive(true);
		p_optionsMenu.SetActive(false);
	}

	public void OnControlModeJoystick(bool joystickOn)
	{
		b_joystick.SetActive(joystickOn);
		b_LeftRight.SetActive(!joystickOn);
	}

	public void OnClickStart()
	{
		lm.LoadNextScene();
	}

	public void OnClickContinue()
	{
		lm.LoadScene(sm.SavedLevel.value);
	}

	public void OnClickReload()
	{
		lm.ReloadCurrentScene();
	}

	public void OnClickMenu()
	{
		//Debug.Log("OnClickMenu, start level is : " + startLevelIndex);
		lm.LoadScene(startLevelIndex);
	}
}
