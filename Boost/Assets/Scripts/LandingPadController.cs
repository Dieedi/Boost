using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LandingPadController : MonoBehaviour {

	[SerializeField] UnityEvent PlayerReachEndEvent;

	private Player player;

	private void Awake()
	{
		player = FindObjectOfType<Player>();
	}

	private void OnTriggerEnter(Collider other)
	{
		//Debug.Log("should recognize player");
		if (other.gameObject.name == player.gameObject.name) {
			//Debug.Log("should invoke end event");
			PlayerReachEndEvent.Invoke();
		}
	}
}
