using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunCastController : MonoBehaviour {

	[SerializeField] ParticleSystem BulletParticle;
	private ParticleSystem.EmissionModule emission;

	private void Start()
	{
	}

	public void CastParticle()
	{
		//Debug.Log("cast Bullet");
		BulletParticle.Play();
	}

	public void StopParticle()
	{
		BulletParticle.Stop();
	}
}
