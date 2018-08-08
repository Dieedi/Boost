using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftController : MonoBehaviour {

	[Header("Starting position on activation")]
	[Header("All values must be entered !")]
	[SerializeField] Vector3 StartingPosition;
	[SerializeField] GameObject player;

	private MovingPlateform thisPlatform;
	private Vector3 RestPos;
	private Vector3 CurrentPos;
	private Vector3 Direction = Vector3.up;
	private bool movementBegins = false;

	private void Awake()
	{
		thisPlatform = GetComponent<MovingPlateform>();
		RestPos = thisPlatform.transform.localPosition;
		CurrentPos = new Vector3(StartingPosition.x, StartingPosition.y, StartingPosition.z);
		thisPlatform.transform.localPosition = CurrentPos;
		thisPlatform.ComeAndGo = true;
	}

	private void Update()
	{
		if (Vector3.Distance(RestPos, CurrentPos) > .1f) {
			Vector3 newPos = Direction * 1f * Time.deltaTime;
			transform.localPosition = CurrentPos = newPos + CurrentPos;
		}
		
		//if (liftStop == true) {
		//	thisPlatform.MovementActivated = false;
		//}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.name == player.name && !movementBegins) {
			movementBegins = true;
			thisPlatform.platformBlock = false;
			StartCoroutine(LiftUp());
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.name == player.name && !movementBegins) {
			movementBegins = true;
			thisPlatform.platformBlock = false;
			StartCoroutine(LiftUp());
		}
	}

	private void OnCollisionStay(Collision collision)
	{
		if (collision.gameObject.name == player.name) {
			player.transform.parent = transform;
		}
	}

	private void OnCollisionExit(Collision collision)
	{
		if (collision.gameObject.name == player.name) {
			player.transform.parent = null;
		}
	}

	IEnumerator LiftUp()
	{
		yield return new WaitForSeconds(1f);
		thisPlatform.MovementActivated = true;
	}
}
