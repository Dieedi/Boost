using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuckController : MonoBehaviour {

	[SerializeField] Vector3 StartingPos;

	public void ResetPos()
	{
		transform.position = StartingPos;
		transform.rotation = Quaternion.identity;
	}
}
