using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	[SerializeField] private float rcsThrust = 75;
	[SerializeField] private float rcsVThrust_Multiplier = 10;
	[SerializeField] private float Thruster = 750;
	[SerializeField] private AudioClip ThrustSound;
	[SerializeField] private AudioClip GazSound;
	float rotmax = 0;
	private float MoveH = 0;
	private float MoveV = 0;
	private bool SoundToggle = false;
	private Rigidbody rigidbody;
	private AudioSource audioSource;

	// Use this for initialization
	void Start()
	{
		rigidbody = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update()
	{
		Thrust();
		Rotate();
	}

	private void FixedUpdate()
	{
		if (rigidbody.rotation.x != 0 || rigidbody.rotation.y != 0) {
			rigidbody.rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z);
		}
	}

	private void Thrust()
	{
		if (Input.GetButton("Jump")) {
			float ThrusterSpeed = Thruster * Time.deltaTime;
			rigidbody.AddRelativeForce(Vector3.up * ThrusterSpeed);

			audioSource.pitch += .05f;
			audioSource.pitch = Mathf.Clamp(audioSource.pitch, .9f, 1.3f);
		} else {
			audioSource.pitch -= .05f;
			audioSource.pitch = Mathf.Clamp(audioSource.pitch, .9f, 1.3f);
		}
	}

	private void Rotate()
	{
		MoveH = Input.GetAxis("Horizontal");
		MoveV = Input.GetAxis("Vertical");

		float rotationSpeed = rcsThrust * Time.deltaTime;

		if (MoveH != 0) {
			//isMoving = true;

			rotmax += MoveH * rotationSpeed;
			rotmax = Mathf.Clamp(rotmax, -45.0f, 45.0f);
			if (MoveH > 0) {
				transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, -rotmax);
				//rigidbody.AddRelativeForce(-Vector3.right * rotationSpeed);
			} else if (MoveH < 0) {
				transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, -rotmax);
				//rigidbody.AddRelativeForce(Vector3.right * rotationSpeed);
			}
		}
	}

	private void StopSound()
	{
		//audioSource.Stop();
		//audioSource.clip = null;
		//SoundToggle = false;
	}

	private void PlaySound(AudioClip SoundToPlay)
	{
		//SoundToggle = true;
		//audioSource.clip = SoundToPlay;
		//audioSource.Play();
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
