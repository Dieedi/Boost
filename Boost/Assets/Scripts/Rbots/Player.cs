using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour {

	enum State
	{
		Alive,
		Dying,
		Transcending,
		Waiting,
	}

	State state = State.Waiting;

	[Header("Change movement style")]
	[SerializeField] bool USE_FORCE = false;
	[SerializeField] bool USE_TRANSFORM = false;
	[SerializeField] float transformSpeedModifier = 5f;
	[SerializeField] bool USE_VELOCITY = true;
	[SerializeField] float velocitySpeedModifier = 12f;
	[SerializeField] float velocityJumpSpeedModifier = 400f;
	[SerializeField] float velocityMax = 6f;
	[SerializeField] float JumpMaxDistance = 3.1f;

	[Header("Base parameters")]
	[SerializeField] float rcsThrust = 75;
	[SerializeField] float rcsVThrust_Multiplier = 10;
	[SerializeField] float Thruster = 750;
	[SerializeField] float rotationSpeedModifier = 1.5f;
	[SerializeField] AudioClip ThrustSound;
	[SerializeField] AudioClip GazSound;
	[SerializeField] ParticleSystem DustEffect;
	[SerializeField] UnityEvent e_PlayerAction;
	[SerializeField] GameObject RbotModel;
	[SerializeField] LayerMask CheckJumpWith;

	[HideInInspector] public bool canActivate = false;
	[HideInInspector] public bool OverrideLookAt = false;

	[Header("Character Lock Specifics")]
	[SerializeField] GameObject ObjectToLookAt;

	private LayerMask GroundLayer;
	private float rotmax = 0;
	private float MoveH = 0;
	private float MoveV = 0;
	private float EndingClipLength = 0f;
	private bool SoundToggle = false;
	private bool isMoving = false;
	private bool canCast = false;
	private bool castArcs = false;
	private RaycastHit DustHit;
	private Rigidbody rigidbody;
	private AudioSource audioSource;
	private Animator animator;
	private float frontOrientation = 90f;

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
				CheckActions();
				break;
			default:
				Animate();
				break;
		}
	}

	private void FixedUpdate()
	{
		if (!OverrideLookAt) {
			// Rotate mesh in direction depending on velocity and orientation given by keyboard
			Vector3 Direction = new Vector3(frontOrientation, rigidbody.velocity.y * rotationSpeedModifier, rigidbody.velocity.z);
			RbotModel.transform.rotation = Quaternion.LookRotation(Direction, Direction);
		} else {
			Vector3 newPosition = new Vector3(ObjectToLookAt.transform.position.x,
				ObjectToLookAt.transform.position.y,
				transform.position.z);
			transform.position = newPosition;
			RbotModel.transform.LookAt(ObjectToLookAt.transform);
		}

		animator.SetFloat("Velocity", rigidbody.velocity.magnitude);
	}

	private void Thrust()
	{
		if (Input.GetButtonDown("Jump")) {
			if (Physics.Raycast(transform.position, Vector3.down, JumpMaxDistance, CheckJumpWith)) {
				if (USE_FORCE) {
					float ThrusterSpeed = Thruster * Time.deltaTime;
					rigidbody.AddRelativeForce(Vector3.up * ThrusterSpeed);
				} else if (USE_TRANSFORM) {
					Vector3 newPos = new Vector3(transform.position.x, transform.position.y + Vector3.up.y * transformSpeedModifier * Time.deltaTime, transform.position.z);
					//Debug.Log(newPos);
					transform.position = newPos;
				} else if (USE_VELOCITY) {
					rigidbody.velocity += Vector3.up * velocityJumpSpeedModifier * Time.deltaTime;
				}
			}

			audioSource.pitch += .05f;
		} else {
			audioSource.pitch -= .05f;
		}
		audioSource.pitch = Mathf.Clamp(audioSource.pitch, .9f, 1.3f);
	}

	private void Rotate()
	{
		MoveH = CrossPlatformInputManager.GetAxis("Horizontal"); //Input.GetAxis("Horizontal");
		MoveV = CrossPlatformInputManager.GetAxis("Vertical"); //Input.GetAxis("Vertical");

		float rotationSpeed = rcsThrust * Time.deltaTime;

		if (MoveH != 0) {
			if (MoveH > 0 && rigidbody.velocity.normalized.x >= 0) {
				if (USE_FORCE) {
					rigidbody.AddRelativeForce(Vector3.right * rotationSpeed);
				} else if (USE_TRANSFORM) {
					Vector3 newPosition = new Vector3(transform.position.x + Vector3.right.x * transformSpeedModifier * Time.deltaTime, transform.position.y, transform.position.z);
					//Debug.Log(newPosition);
					transform.position = newPosition;
				} else if (USE_VELOCITY) {
					Vector3 newVelocity = rigidbody.velocity;
					newVelocity += Vector3.right * velocitySpeedModifier * Time.deltaTime;
					newVelocity = new Vector3(Mathf.Clamp(newVelocity.x, 0, velocityMax), newVelocity.y, newVelocity.z);
					rigidbody.velocity = newVelocity;
				}
				frontOrientation = 90;
			} else if (MoveH < 0 && rigidbody.velocity.normalized.x <= 0) {
				if (USE_FORCE) {
					rigidbody.AddRelativeForce(-Vector3.right * rotationSpeed);
				} else if (USE_TRANSFORM) {
					Vector3 newPosition = new Vector3(transform.position.x + Vector3.left.x * transformSpeedModifier * Time.deltaTime, transform.position.y, transform.position.z);
					//Debug.Log(newPosition);
					transform.position = newPosition;
				} else if (USE_VELOCITY) {
					Vector3 newVelocity = rigidbody.velocity;
					newVelocity += Vector3.left * velocitySpeedModifier * Time.deltaTime;
					newVelocity = new Vector3(Mathf.Clamp(newVelocity.x, -velocityMax, 0), newVelocity.y, newVelocity.z);
					rigidbody.velocity = newVelocity;
				}
				frontOrientation = -90;
			}
		}

		if (MoveV != 0) {
			if (MoveV > 0) {
				rigidbody.AddRelativeForce(Vector3.up * rotationSpeed);
			} else if (MoveV < 0) {
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

	IEnumerator AnimateTranscending()
	{
		//animator.SetBool("hasFinish", true);
		yield return new WaitForSeconds(.8f);
		//EndingClipLength = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
	}

	private void AnimateMoves()
	{
		//if (Input.GetButton("Jump") || MoveH != 0 || MoveV != 0) {
		//	isMoving = true;
		//} else {
		//	isMoving = false;
		//}
		//animator.SetBool("isMoving", isMoving);
	}

	private void CheckActions()
	{
		if (CrossPlatformInputManager.GetButtonDown("Fire 1") && canCast) {
			//Debug.Log("player cast action");
			canCast = false;
			e_PlayerAction.Invoke();
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		switch (collision.gameObject.tag) {
			case "Friendly":
				//Debug.Log("Landed");
				break;
			case "Finish":
				state = State.Transcending;
				break;
			default:
				state = State.Alive;
				break;
		}
	}

	public void OnTeleportComplete ()
	{
		state = State.Alive;
	}

	public void PlayerInTrigger ()
	{
		canCast = true;
	}

	public void PlayerExitTrigger()
	{
		canCast = false;
	}

	public void UnlockStart ()
	{
		OverrideLookAt = true;
	}

	public void UnlockStop ()
	{
		OverrideLookAt = false;
	}
}
