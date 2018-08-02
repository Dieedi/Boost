using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlateform : MonoBehaviour {

	[SerializeField] Vector3 MoveDirection;
	[Range(0, 1)] [SerializeField] float MoveFactor;
	[Header("Starting position when Activated (lift behavior)")]
	[SerializeField] Vector3 StartingPos;

	public bool MovementActivated = false;
	public bool ComeAndGo = false;
	public bool platformBlock = false;

	private Vector3 CurrentPosition;
	private Vector3 DestinationPos;
	private float Speed = 0;
	private Vector3 newPos;
	private bool moveUp, moveDown = false;

	// Use this for initialization
	void Start () {
		CurrentPosition = StartingPos;
		DestinationPos = new Vector3(CurrentPosition.x + MoveDirection.x,
			CurrentPosition.y + MoveDirection.y,
			CurrentPosition.z + MoveDirection.z);
	}
	
	// Update is called once per frame
	void Update () {
		ChangePositon();
	}

	private void ChangePositon()
	{
		if (MovementActivated && !ComeAndGo) {
			//Debug.Log(Vector3.Distance(DestinationPos, CurrentPosition));
			if (Vector3.Distance(DestinationPos, CurrentPosition) > 0.1) {
				newPos = MoveDirection * MoveFactor * Time.deltaTime;
				transform.localPosition = CurrentPosition = newPos + CurrentPosition;
			}
		}

		if (ComeAndGo && MovementActivated) {
			MovementActivated = false;
			StartCoroutine(LiftUp());
		}
	}

	IEnumerator LiftUp()
	{
		StopCoroutine(LiftDown());
		//while (Vector3.Distance(DestinationPos, CurrentPosition) > 0.1 || !platformBlock) {
		while (!platformBlock) {
			newPos = MoveDirection * MoveFactor * Time.deltaTime;
			transform.localPosition = CurrentPosition = newPos + CurrentPosition;
			yield return null;
		}
		yield return new WaitForSeconds(2f);
		platformBlock = false;
		StartCoroutine(LiftDown());
	}

	IEnumerator LiftDown()
	{
		StopCoroutine(LiftUp());
		//while (Vector3.Distance(StartingPos, CurrentPosition) > 0.1 || !platformBlock) {
		while (!platformBlock) {
			newPos = MoveDirection * -MoveFactor * Time.deltaTime;
			transform.localPosition = CurrentPosition = newPos + CurrentPosition;
			yield return null;
		}
		yield return new WaitForSeconds(2f);
		platformBlock = false;
		StartCoroutine(LiftUp());
	}

	public void ActivateMovement()
	{
		MovementActivated = true;
	}

	public void BlockPlatform()
	{
		platformBlock = !platformBlock;
	}
}
