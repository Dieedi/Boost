using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	[SerializeField] GameObject levelManager;
	[SerializeField] GameObject saveManager;
	[SerializeField] GameObject menu;

	private LevelManager lm;
	private SaveManager sm;
	private GameMenuController gmc;

#if UNITY_EDITOR
	[SerializeField] bool resetPrefs;
#endif

	public bool dragControls = true;

	private void Awake()
	{
		if (instance != null) {
			Destroy(gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad(gameObject);
		}

#if UNITY_EDITOR
		if (resetPrefs) {
			PlayerPrefs.DeleteAll();
		}
#endif
		Instantiate(saveManager);
		Instantiate(levelManager);
		Instantiate(menu);

		lm = FindObjectOfType<LevelManager>();
		sm = FindObjectOfType<SaveManager>();
		gmc = FindObjectOfType<GameMenuController>();
	}

	private void Start()
	{
		if (PlayerPrefs.HasKey("Controls")) {
			dragControls = Convert.ToBoolean(PlayerPrefs.GetString("Controls"));
		} else {
			PlayerPrefs.SetString("Controls", dragControls.ToString());
		}
		gmc.OnControlModeJoystick(dragControls);
	}

	public void OnEventControlsChanged()
	{
		dragControls = !dragControls;
		gmc.OnControlModeJoystick(dragControls);
		PlayerPrefs.SetString("Controls", dragControls.ToString());
		PlayerPrefs.Save();
	}

	public void OnEventPlayerReachEnd()
	{
		lm.LoadNextScene();
	}
}
