using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Rocket : MonoBehaviour {

	[SerializeField] private float rcsThrust = 75;
	[SerializeField] private float rcsVThrust_Multiplier = 10; 
	[SerializeField] private float Thruster = 750;
	[SerializeField] private AudioClip ThrustSound;
	[SerializeField] private AudioClip GazSound;
	private float MoveH = 0;
	private float MoveV = 0;
	private bool MoveMod = false;
	private bool SoundToggle = false;
	private Rigidbody rigidbody;
	private AudioSource audioSource;

	// Use this for initialization
	void Start () {
		rigidbody = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		Thrust();
		Rotate();
	}

	private void Thrust()
	{
		if (Input.GetButton("Jump")) {
			float ThrusterSpeed = Thruster * Time.deltaTime;
			rigidbody.AddRelativeForce(Vector3.up * ThrusterSpeed);

			if(!audioSource.isPlaying)
				PlaySound(ThrustSound);
		} else {
			if (audioSource.isPlaying)
				StopSound();
		}
	}

	private void Rotate()
	{
		//rigidbody.freezeRotation = true;

		MoveH = Input.GetAxis("Horizontal");
		MoveV = Input.GetAxis("Vertical");
		MoveMod = Input.GetButton("Straf_modifier");

		float rotationSpeed = rcsThrust * Time.deltaTime;
		
		if (!MoveMod && MoveH != 0) {
			//isMoving = true;
			PlaySound(GazSound);

			if (MoveH > 0) {
				transform.Rotate(-Vector3.forward * rotationSpeed);
			} else if (MoveH < 0) {
				transform.Rotate(Vector3.forward * rotationSpeed);
			}
		} else if (MoveMod && (MoveH != 0 || MoveV != 0)) {

			PlaySound(GazSound);

			if (MoveH > 0) {
				rigidbody.AddRelativeForce(-Vector3.right * rotationSpeed);
			} else if (MoveH < 0) {
				rigidbody.AddRelativeForce(Vector3.right * rotationSpeed);
			}

			if (MoveV > 0) {
				rigidbody.AddRelativeForce(Vector3.up * rotationSpeed * rcsVThrust_Multiplier);
			} else if (MoveV < 0) {
				rigidbody.AddRelativeForce(-Vector3.up * rotationSpeed * rcsVThrust_Multiplier);
			}
		}

		//rigidbody.freezeRotation = false;
	}

	private void StopSound()
	{
		audioSource.Stop();
		audioSource.clip = null;
	}

	private void PlaySound (AudioClip SoundToPlay)
	{
		Debug.Log("should play " + SoundToPlay.name);
		audioSource.clip = SoundToPlay;
		audioSource.Play();
	}

	private void OnCollisionEnter(Collision collision)
	{
		switch (collision.gameObject.tag) {
			case "Friendly":
				Debug.Log("Landed");
				break;
			default:
				Debug.Log("Die");
				break;
		}
	}
}
