using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TeleportController : MonoBehaviour {

	[SerializeField] Player player;
	[SerializeField] UnityEvent FadeOutEvent;

	private Fader fader;
	private ParticleSystem[] particles;
	
	// Use this for initialization
	void Start () {
		fader = player.GetComponent<Fader>();
		particles = GetComponentsInChildren<ParticleSystem>();
		player.gameObject.SetActive(false);

		foreach (ParticleSystem particle in particles) {
			particle.Play();
		}
		StartCoroutine(PlayerSpawn());
	}

	IEnumerator PlayerSpawn()
	{
		yield return new WaitForSeconds(2f);
		player.gameObject.SetActive(true);
		FadeOutEvent.Invoke();
	}

	public void DisableTeleport ()
	{
		foreach (ParticleSystem particle in particles) {
			var emission = particle.emission;
			emission.enabled = false;
		}
	}
}
