using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TeleportController : MonoBehaviour {

	[SerializeField] Player player;

	private Fader fader;
	private ParticleSystem[] particles;

	private void Awake()
	{
	}

	// Use this for initialization
	void Start () {
		fader = player.GetComponent<Fader>();
		particles = GetComponentsInChildren<ParticleSystem>();
		player.gameObject.SetActive(false);

		foreach (ParticleSystem particle in particles) {
			particle.Play();
		}
	}
	
	// Update is called once per frame
	void Update () {
		StartCoroutine(PlayerSpawn());
	}

	IEnumerator PlayerSpawn()
	{
		yield return new WaitForSeconds(1f);
		player.gameObject.SetActive(true);
	}

	public void DisableTeleport ()
	{
		if (fader.alphaValue >= 1) {
			foreach (ParticleSystem particle in particles) {
				var emission = particle.emission;
				emission.enabled = false;
			}
		}
	}
}
