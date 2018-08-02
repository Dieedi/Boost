using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour {

	private Dictionary<string, UnityEvent> eventDictionnary;

	private static EventManager eventManager;

	public static EventManager instance
	{
		get
		{
			if (!eventManager) {
				eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

				if (!eventManager) {
					Debug.LogError("There needs to be on active EventManager script on a GameObject in scene.");
				} else {
					eventManager.Init();
				}
			}

			return eventManager;
		}
	}

	void Init()
	{
		if (eventDictionnary == null) {
			eventDictionnary = new Dictionary<string, UnityEvent>();
		}
	}

	public static void StartListening(string eventName, UnityAction listener)
	{
		UnityEvent thisEvent = null;
		if (instance.eventDictionnary.TryGetValue(eventName, out thisEvent)) {
			thisEvent.AddListener(listener);
		} else {
			thisEvent = new UnityEvent();
			thisEvent.AddListener(listener);
			instance.eventDictionnary.Add(eventName, thisEvent);
		}
	}

	public static void StopListening(string eventName, UnityAction listener)
	{
		if (eventManager == null)
			return;

		UnityEvent thisEvent = null;
		if(instance.eventDictionnary.TryGetValue(eventName, out thisEvent)) {
			thisEvent.RemoveListener(listener);
		}
	}

	public static void TriggerEvent(string eventName)
	{
		UnityEvent thisEvent = null;

		if (instance.eventDictionnary.TryGetValue(eventName, out thisEvent)) {
			thisEvent.Invoke();
		}
	}
}
