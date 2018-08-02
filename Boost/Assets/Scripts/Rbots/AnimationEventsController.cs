using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEventsController : MonoBehaviour {

	[Header("Character Sol")]
	[SerializeField] UnityEvent e_CastArcs;
	[SerializeField] UnityEvent e_StopArcs;

	[Header("Character Lock")]
	[SerializeField] UnityEvent e_UnlockDone;

	[Header("Character Gun")]
	[SerializeField] UnityEvent e_CastGunL;
	[SerializeField] UnityEvent e_CastGunR;
	[SerializeField] UnityEvent e_StopCastGunL;
	[SerializeField] UnityEvent e_StopCastGunR;

	private bool castArcs = false;
	private bool gunShotL = false;
	private bool gunShotR = false;

	public void ToggleArcs()
	{
		castArcs = !castArcs;
		if (castArcs)
			e_CastArcs.Invoke();
		else
			e_StopArcs.Invoke();
	}

	public void UnlockDone()
	{
		e_UnlockDone.Invoke();
	}

	public void ToggleGunL()
	{
		gunShotL = !gunShotL;
		if (gunShotL)
			e_CastGunL.Invoke();
		else
			e_StopCastGunL.Invoke();
	}

	public void ToggleGunR()
	{
		gunShotR = !gunShotR;
		if (gunShotR)
			e_CastGunL.Invoke();
		else
			e_StopCastGunR.Invoke();
	}
}
