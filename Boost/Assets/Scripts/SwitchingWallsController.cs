using System.Collections.Generic;
using UnityEngine;

public class SwitchingWallsController : MonoBehaviour {

	[SerializeField] LayerMask NewLayerToAssign;
	[SerializeField] bool enlight;

	private List<GameObject> SwitchingWalls = new List<GameObject>();
	private int NewLayerInt;

	// Use this for initialization
	void Start () {
		Transform[] walls = GetComponentsInChildren<Transform>();

		foreach (Transform wall in walls) {
			SwitchingWalls.Add(wall.gameObject);
		}

		NewLayerInt = Mathf.RoundToInt(Mathf.Log(NewLayerToAssign.value, 2));
		//Debug.Log(Mathf.RoundToInt(Mathf.Log(NewLayerToAssign.value, 2)));

		// Debug
		if (enlight) {
			EnlightWalls();
		}
	}
	
	public void EnlightWalls()
	{
		foreach(GameObject wall in SwitchingWalls) {
			wall.layer = NewLayerInt;
		}
	}
}
