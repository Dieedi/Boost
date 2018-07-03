using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

	enum State
	{
		Alive,
		Dying,
		Transcending
	}
	State state = State.Alive;

	[SerializeField] private float rcsThrust = 75;
	[SerializeField] private float rcsVThrust_Multiplier = 10;
	[SerializeField] private float Thruster = 750;
	[SerializeField] private AudioClip ThrustSound;
	[SerializeField] private AudioClip GazSound;
	[SerializeField] private AudioClip EndingSound;
	[SerializeField] private ParticleSystem DustEffect;
	private LayerMask GroundLayer;
	private float rotmax = 0;
	private float MoveH = 0;
	private float MoveV = 0;
	private float EndingClipLength = 0f;
	private bool SoundToggle = false;
	private bool isMoving = false;
	private RaycastHit DustHit;
	private Rigidbody rigidbody;
	private AudioSource audioSource;
	private Animator animator;

	// Use this for initialization
	void Start()
	{
		rigidbody = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
		animator = GetComponentInChildren<Animator>();
		GroundLayer = LayerMask.NameToLayer("Ground");
	}

	// Update is called once per frame
	void Update()
	{
		StateCheck();
	}

	private void StateCheck()
	{
		switch (state) {
			case State.Alive:
				Thrust();
				Rotate();
				Animate();
				CastDust();
				PlaySounds();
				break;
			default:
				Animate();
				PlaySounds();
				break;
		}
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
		} else {
			audioSource.pitch -= .05f;
		}
		audioSource.pitch = Mathf.Clamp(audioSource.pitch, .9f, 1.3f);
	}

	private void Rotate()
	{
		MoveH = Input.GetAxis("Horizontal");
		MoveV = Input.GetAxis("Vertical");

		float rotationSpeed = rcsThrust * Time.deltaTime;

		if (MoveH != 0) {

			//rotmax += MoveH * rotationSpeed;
			//rotmax = Mathf.Clamp(rotmax, -45.0f, 45.0f);
			if (MoveH > 0) {
				//transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, -rotmax);
				rigidbody.AddRelativeForce(Vector3.right * rotationSpeed);
			} else if (MoveH < 0) {
				//transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, -rotmax);
				rigidbody.AddRelativeForce(-Vector3.right * rotationSpeed);
			}
		}
		if (MoveV != 0) {

			//rotmax += MoveV * rotationSpeed;
			//rotmax = Mathf.Clamp(rotmax, -45.0f, 45.0f);
			if (MoveV > 0) {
				//transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, -rotmax);
				rigidbody.AddRelativeForce(Vector3.up * rotationSpeed);
			} else if (MoveV < 0) {
				//transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, -rotmax);
				rigidbody.AddRelativeForce(-Vector3.up * rotationSpeed);
			}
		}
	}

	private void Animate()
	{
		switch (state) {
			case State.Alive:
				AnimateMoves();
				break;
			case State.Dying:
				// TODO animation
				break;
			case State.Transcending:
				if (!animator.GetBool("hasFinish"))
					StartCoroutine(AnimateTranscending());
				break;
		}
	}

	private void CastDust()
	{
		int layerMask = 1 << 9;
		if (isMoving && Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out DustHit, Mathf.Infinity, layerMask)) {
			if (DustHit.distance <= 4)
				DustEffect.Play();
			else
				DustEffect.Stop();
		} else {
			DustEffect.Stop();
		}
	}

	private void PlaySounds()
	{
		if (state == State.Transcending) {
			audioSource.pitch = 1.0f;
			audioSource.clip = EndingSound;
			if (!audioSource.isPlaying)
				audioSource.Play();
		}
	}

	IEnumerator AnimateTranscending()
	{
		animator.SetBool("hasFinish", true);
		yield return new WaitForSeconds(.8f);
		EndingClipLength = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
		Debug.Log(EndingClipLength);
		Invoke("LoadNextScene", EndingClipLength);
	}

	private void AnimateMoves()
	{
		if (Input.GetButton("Jump") || MoveH != 0 || MoveV != 0) {
			Debug.Log("Moving");
			isMoving = true;
		} else {
			Debug.Log("should stop");
			isMoving = false;
		}
		animator.SetBool("isMoving", isMoving);
	}

	private void OnCollisionEnter(Collision collision)
	{
		switch (collision.gameObject.tag) {
			case "Friendly":
				Debug.Log("Landed");
				break;
			case "Finish":
				state = State.Transcending;
				break;
			default:
				state = State.Alive;
				break;
		}
	}

	public void LoadNextScene()
	{
		Scene currentScene = SceneManager.GetActiveScene();
		SceneManager.LoadScene(currentScene.buildIndex + 1);
	}
}
