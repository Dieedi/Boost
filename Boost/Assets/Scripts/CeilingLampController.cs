using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingLampController : MonoBehaviour {

	private Animator animator;

	private void Start()
	{
		animator = GetComponent<Animator>();
	}

	public void EnlightLamp ()
	{
		animator.SetBool("onEnlight", true);
	}
}
