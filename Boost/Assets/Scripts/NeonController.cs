using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeonController : MonoBehaviour {

	private Light[] neonLights;

	// Use this for initialization
	void Start () {
		neonLights = GetComponentsInChildren<Light>();
		foreach(Light light in neonLights) {
			light.intensity = 0;
		}
	}
	
	public void Enlight()
	{
		foreach (Light light in neonLights) {
			light.intensity = .5f;
		}
	}
}
