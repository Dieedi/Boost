using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuckController : MonoBehaviour {

	public static StuckController instance;

	[SerializeField] Vector3 StartingPos;

	private void Awake()
	{
		if (instance != null) {
			Destroy(gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}

	public void ResetPos()
	{
		GameObject player = FindObjectOfType<Player>().gameObject;
		player.transform.position = StartingPos;
		player.transform.rotation = Quaternion.identity;
	}
}
