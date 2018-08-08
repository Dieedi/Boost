using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class Switcher : MonoBehaviour {

	[SerializeField] Material OffMaterial;
	[SerializeField] Material OnMaterial;
	[SerializeField] GameObject player;
	[SerializeField] GameObject ObjectToActivate;
	[SerializeField] UnityEvent e_MovePlatform;
	[SerializeField] UnityEvent e_PlayerInTrigger;
	[SerializeField] UnityEvent e_PlayerExitTrigger;

	public float Timer = 3f;

	private Material[] SwitchMaterials;
	private bool PlatformActivated = false;

	// Use this for initialization
	void Start () {
		SwitchMaterials = GetComponentInChildren<MeshRenderer>().materials;
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void PerformAction()
	{
		if (!PlatformActivated) {
			ChangeSwitchLight();
			ActivateObject();
		}
	}

	private void ActivateObject()
	{
		e_MovePlatform.Invoke();
		PlatformActivated = true;
		//MovingPlateform movingPlateform = ObjectToActivate.GetComponent<MovingPlateform>();
		//if (movingPlateform && !movingPlateform.MovementActivated) {
		//	movingPlateform.MovementActivated = true;
		//} else {
		//	movingPlateform.MovementActivated = false;
		//}
	}

	private void ChangeSwitchLight()
	{
		int index = 0;
		foreach (Material m in SwitchMaterials) {
			if (m.name.Contains(OffMaterial.name)) {
				SwitchMaterials[index].CopyPropertiesFromMaterial(OnMaterial);
				SwitchMaterials[index].name = OnMaterial.name;
			} else {
				SwitchMaterials[index].CopyPropertiesFromMaterial(OffMaterial);
				SwitchMaterials[index].name = OffMaterial.name;
			}
			index++;
		}
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
