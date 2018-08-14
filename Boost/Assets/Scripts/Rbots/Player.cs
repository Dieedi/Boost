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
	[SerializeField] float velocitySpeedModifier = 12f;
	[SerializeField] float velocityJumpSpeedModifier = 400f;
	[SerializeField] float velocityMax = 6f;
	[SerializeField] float JumpMaxDistance = 3.1f;

	[Header("Base parameters")]
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

	//private LayerMask GroundLayer;
	private float MoveH = 0;
	//private float EndingClipLength = 0f;
	private bool isMoving = false;
	private bool canCast = false;
	private RaycastHit DustHit;
	private Rigidbody rb;
	private AudioSource audioSource;
	private Animator animator;
	private float frontOrientation = 90f;

	private Vector3 newVelocity;
	private Vector3 v_right = Vector3.right;
	private Vector3 v_left = Vector3.left;

	private bool moveRight;
	private bool moveLeft;

	private GameManager gm;

	private void Awake()
	{
		gm = FindObjectOfType<GameManager>();
	}
	// Use this for initialization
	void Start()
	{
		rb = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
		animator = GetComponentInChildren<Animator>();
		//GroundLayer = LayerMask.NameToLayer("Ground");
	}

	// Update is called once per frame
	void Update()
	{
		StateCheck();

		if (gm.dragControls) {
			MoveH = CrossPlatformInputManager.GetAxis("Horizontal"); //Input.GetAxis("Horizontal");
		} else {
			if (CrossPlatformInputManager.GetButtonDown("MoveRight")) moveRight = true;
			if (CrossPlatformInputManager.GetButtonUp("MoveRight")) moveRight = false;
			if (moveRight) MoveRight();

			if (CrossPlatformInputManager.GetButtonDown("MoveLeft")) moveLeft = true;
			if (CrossPlatformInputManager.GetButtonUp("MoveLeft")) moveLeft = false;
			if (moveLeft) MoveLeft();
		}

		if (!OverrideLookAt) {
			// Rotate mesh in direction depending on velocity and orientation given by keyboard
			Vector3 Direction = new Vector3(frontOrientation, rb.velocity.y * rotationSpeedModifier, rb.velocity.z);
			RbotModel.transform.rotation = Quaternion.LookRotation(Direction, Direction);
		} else {
			Vector3 newPosition = new Vector3(ObjectToLookAt.transform.position.x,
				ObjectToLookAt.transform.position.y,
				transform.position.z);
			transform.position = newPosition;
			RbotModel.transform.LookAt(ObjectToLookAt.transform);
		}
	}

	private void StateCheck()
	{
		switch (state) {
			case State.Alive:
				Animate();
				//CastDust();
				CheckActions();
				break;
			default:
				Animate();
				break;
		}

		animator.SetFloat("Velocity", rb.velocity.magnitude);
	}

	private void FixedUpdate()
	{
		if (state == State.Alive) {
				//Thrust();
				Move();
		}
	}

	private void Thrust()
	{
		if (Input.GetButtonDown("Jump")) {
			if (Physics.Raycast(transform.position, Vector3.down, JumpMaxDistance, CheckJumpWith)) {
				rb.velocity += Vector3.up * velocityJumpSpeedModifier * Time.deltaTime;
			}

			audioSource.pitch += .05f;
		} else {
			audioSource.pitch -= .05f;
		}
		audioSource.pitch = Mathf.Clamp(audioSource.pitch, .9f, 1.3f);
	}

	private void Move()
	{
		if (MoveH != 0) {
			if (!audioSource.isPlaying)
				audioSource.Play();
			//Debug.Log(audioSource.isPlaying);
			newVelocity = rb.velocity;

			if (MoveH > 0 && rb.velocity.normalized.x >= 0) {
				MoveRight();
			} else if (MoveH < 0 && rb.velocity.normalized.x <= 0) {
				MoveLeft();
			}

			rb.velocity = newVelocity;
		} else {
			if (audioSource.isPlaying)
				audioSource.Stop();
		}
	}

	public void MoveRight()
	{
		//Debug.Log("Move right");
		newVelocity = rb.velocity;
		newVelocity += v_right * velocitySpeedModifier * Time.deltaTime;
		newVelocity = new Vector3(Mathf.Clamp(newVelocity.x, 0, velocityMax), newVelocity.y, newVelocity.z);
		frontOrientation = 90;
		rb.velocity = newVelocity;
	}

	public void MoveLeft()
	{
		//Debug.Log("Move left");
		newVelocity = rb.velocity;
		newVelocity += v_left * velocitySpeedModifier * Time.deltaTime;
		newVelocity = new Vector3(Mathf.Clamp(newVelocity.x, -velocityMax, 0), newVelocity.y, newVelocity.z);
		frontOrientation = -90;
		rb.velocity = newVelocity;
	}


	private void Animate()
	{
		switch (state) {
			case State.Alive:
				//AnimateMoves();
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
