using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RepairPanelController : MonoBehaviour {

	[Tooltip("time in seconds")]
	[SerializeField] float RepairTime = 3f;
	[SerializeField] UnityEvent e_RepairIsCompleted;
	[SerializeField] UnityEvent e_PlayerInTrigger;
	[SerializeField] UnityEvent e_PlayerExitTrigger;
	[SerializeField] GameObject player;

	private float RepairTimer = 0f;
	private bool RepairStart = false;
	private ParticleSystem[] particleSystems;

	private void Start()
	{
		particleSystems = GetComponentsInChildren<ParticleSystem>();
	}

	private void Update()
	{
		if (RepairStart) {
			if (RepairTimer < RepairTime) {
				RepairTimer += Time.deltaTime;
			} else {
				//Debug.Log("repair is complete, invoke event, launch stop");
				RepairStart = false;
				e_RepairIsCompleted.Invoke();
				RepairIsCompleted();
			}
		}
	}

	private void RepairIsCompleted()
	{
		//Debug.Log("repair is complete, stop particles");
		foreach (ParticleSystem ps in particleSystems) {
			ps.Stop();
		}
	}

	public void RepairProgression ()
	{
		//Debug.Log("repair start");
		RepairStart = true;
	}

	private void OnTriggerStay(Collider other)
	{
		//Debug.Log(other.gameObject.name);
		if (player.name == other.gameObject.name) {
			//Debug.Log("can cast");
			e_PlayerInTrigger.Invoke();
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (player.name == other.gameObject.name) {
			//Debug.Log("can cast");
			e_PlayerExitTrigger.Invoke();
		}
	}
}
