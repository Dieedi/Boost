using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CoreEventListener : MonoBehaviour {

	public CoreEvent CoreEvent;
	public UnityEvent Response;

	private void OnEnable()
	{
		CoreEvent.RegisterListener(this);
	}

	private void OnDisable()
	{
		CoreEvent.UnregisterListener(this);
	}

	public void RaiseEvent()
	{
		Response.Invoke();
	}
}
