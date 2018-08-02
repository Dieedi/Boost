using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LandingPadController : MonoBehaviour {

	[SerializeField] Player player;
	[SerializeField] UnityEvent PlayerReachEndEvent;

	private void OnTriggerEnter(Collider other)
	{
		//Debug.Log("should recognize player");
		if (other.gameObject.name == player.gameObject.name) {

			//Debug.Log("should invoke end event");
			PlayerReachEndEvent.Invoke();
		}
	}
}
