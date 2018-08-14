using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGameEvent", menuName = "SO/Events/CoreEvent")]
public class CoreEvent : ScriptableObject {

	private List<CoreEventListener> listeners = new List<CoreEventListener>();

	public void RegisterListener(CoreEventListener listener)
	{
		listeners.Add(listener);
	}

	public void UnregisterListener(CoreEventListener listener)
	{
		listeners.Remove(listener);
	}

	public void Raise()
	{
		for (int i = listeners.Count - 1; i >= 0; --i) {
			listeners[i].RaiseEvent();
		}
	}
}
