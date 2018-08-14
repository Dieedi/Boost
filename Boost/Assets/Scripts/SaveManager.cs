using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour {

	public static SaveManager instance;

	[HideInInspector] public IntReference SavedLevel;

	private void Awake()
	{
		if (instance != null) {
			Destroy(gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		Load();
	}

	public void Save()
	{
		PlayerPrefs.SetString("level", Helper.Serialize<IntReference>(SavedLevel));
		PlayerPrefs.Save();
		// Debug.Log("game saved : " + PlayerPrefs.GetString("level"));
	}

	public void Load()
	{
		if (PlayerPrefs.HasKey("level")) {
			SavedLevel = ScriptableObject.CreateInstance<IntReference>();
			// Debug.Log("Load saved level : " + PlayerPrefs.GetString("level"));
			SavedLevel = Helper.Deserialize<IntReference>(PlayerPrefs.GetString("level"));
			// Debug.Log(SavedLevel.value);
		} else {
			SavedLevel = ScriptableObject.CreateInstance<IntReference>();
			Save();
		}
	}
}
