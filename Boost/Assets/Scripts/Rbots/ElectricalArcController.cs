using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ElectricalArcController : MonoBehaviour {

	[SerializeField] GameObject RepairPanel;
	[SerializeField] UnityEvent IsRepairingEvent;

	private ParticleSystem ArcParticles;
	private bool isRepairing = false;

	// Use this for initialization
	void Start () {
		ArcParticles = GetComponent<ParticleSystem>();
	}

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
