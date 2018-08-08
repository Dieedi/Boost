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
	void Awake () {
		animator = GetComponentInChildren<Animator>();
	}

	public void Awaken()
	{
		PowerOn = true;
		animator.SetBool("Power", PowerOn);
	}

	public void ToggleBraze()
	{
		BrazeOn = !BrazeOn;
		animator.SetBool("Braze", BrazeOn);
	}

	public void ToggleUnlock()
	{
		UnlockOn = !UnlockOn;
		animator.SetBool("Unlock", UnlockOn);
	}

	public void ToggleGunShot()
	{
		GunShotOn = !GunShotOn;
		animator.SetBool("GunShot", GunShotOn);
	}
}
