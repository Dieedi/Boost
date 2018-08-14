using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelEventListener : MonoBehaviour {

	public static LevelEventListener instance;

	[SerializeField] LevelEvent LevelEvent;
	[SerializeField] UnityEvent Response;

	private void Awake()
	{
		if (instance != null) {
			Destroy(gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}

	private void OnEnable()
	{
		LevelEvent.RegisterListener(this);
	}

	private void OnDisable()
	{
		LevelEvent.UnregisterListener(this);
	}

	public void RaiseEvent()
	{
		Response.Invoke();
	}
}
