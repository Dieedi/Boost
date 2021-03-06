using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Eventest : MonoBehaviour {

	private UnityAction someListener;

	private void Awake()
	{
		someListener = new UnityAction(SomeFunction);
	}

	private void OnEnable()
	{
		EventManager.StartListening("test", someListener);
	}

	private void OnDisable()
	{
		EventManager.StopListening("test", someListener);
	}

	private void SomeFunction()
	{
		//Debug.Log("function called");
	}
}
