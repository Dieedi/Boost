using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlatformBlocker : MonoBehaviour {

	[SerializeField] UnityEvent PlatformBlock;
	[SerializeField] GameObject Parent;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.name == Parent.name) {
			PlatformBlock.Invoke();
		}
	}
}
