using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ElectricalArcController : MonoBehaviour {

	[SerializeField] GameObject RepairPanel;
	[SerializeField] UnityEvent IsRepairingEvent;

	private bool isRepairing = false;

	private void OnParticleCollision(GameObject other)
	{
		//Debug.Log(other.name);
		if (other.name == RepairPanel.name && isRepairing == false) {

			//Debug.Log("repair start, launch repair event");
			isRepairing = true;
			IsRepairingEvent.Invoke();
		}
	}
}
