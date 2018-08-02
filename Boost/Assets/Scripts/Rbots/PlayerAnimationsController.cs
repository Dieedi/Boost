using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationsController : MonoBehaviour {

	private Animator animator;
	private bool PowerOn = false;
	private bool BrazeOn = false;
	private bool UnlockOn = false;
	private bool GunShotOn = false;

	// Use this for initialization
	void Start () {
		animator = GetComponentInChildren<Animator>();
	}

	private void FixedUpdate()
	{
		animator.SetBool("Power", PowerOn);
		animator.SetBool("Braze", BrazeOn);
		animator.SetBool("Unlock", UnlockOn);
		animator.SetBool("GunShot", GunShotOn);
	}

	public void Awaken()
	{
		PowerOn = true;
	}

	public void ToggleBraze()
	{
		BrazeOn = !BrazeOn;
	}

	public void ToggleUnlock()
	{
		UnlockOn = !UnlockOn;
	}

	public void ToggleGunShot()
	{
		GunShotOn = !GunShotOn;
	}
}
