using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Crumbling_wall : MonoBehaviour {

	[SerializeField] UnityEvent e_WallIsDestroyed;
	[SerializeField] UnityEvent e_PlayerInTrigger;
	[SerializeField] UnityEvent e_PlayerExitTrigger;
	[SerializeField] Player player;

	private Animator animator;
	private float HitPoint = 3;

	private void Start()
	{
		animator = GetComponent<Animator>();
	}

	public void ObjectIsHit()
	{
		HitPoint -= 1;
		if (HitPoint <= 0) {
			LaunchCrumbling();
		}
	}

	private void LaunchCrumbling()
	{
		animator.SetBool("isHit", true);
	}

	public void CastEvent()
	{
		//Debug.Log("Invoke is destroyed event");
		e_WallIsDestroyed.Invoke();
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.name == player.gameObject.name) {
			e_PlayerInTrigger.Invoke();
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.name == player.gameObject.name) {
			e_PlayerExitTrigger.Invoke();
		}
	}
}
