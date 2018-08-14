using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelEvent", menuName = "SO/Events/LevelEvent")]
public class LevelEvent : ScriptableObject {

	private List<LevelEventListener> listeners = new List<LevelEventListener>();

	public void RegisterListener(LevelEventListener listener)
	{
		listeners.Add(listener);
	}

	public void UnregisterListener(LevelEventListener listener)
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
