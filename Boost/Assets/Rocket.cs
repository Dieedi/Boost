using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Rocket : MonoBehaviour {

	private float MoveH = 0;
	private float MoveV = 0;
	private Rigidbody rigidbody;
	private AudioSource audioSource;

	// Use this for initialization
	void Start () {
		rigidbody = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		ProcessInput();
	}

	private void ProcessInput()
	{
		if (Input.GetButton("Jump")) {
			rigidbody.AddRelativeForce(Vector3.up);
			if (!audioSource.isPlaying) {
				audioSource.Play();
			} else {
				audioSource.mute = false;
			}
		} else {
			if (audioSource.isPlaying) {
				audioSource.mute = true;
			}
		}

		MoveH = Input.GetAxis("Horizontal");
		MoveV = Input.GetAxis("Vertical");

		if (MoveH > 0) {
			transform.Rotate(-Vector3.forward);
		} else if (MoveH < 0) {
			transform.Rotate(Vector3.forward);
		}
	}
}
