using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BulletHitController : MonoBehaviour {

	[SerializeField] GameObject ObjectToDestroy;
	[SerializeField] UnityEvent BulletHitEvent;

	private ParticleSystem Bullet;

	// Use this for initialization
	void Start () {
		Bullet = GetComponent<ParticleSystem>();
	}

	private void Update()
	{
		transform.LookAt(ObjectToDestroy.transform);
	}

	private void OnParticleCollision(GameObject other)
	{
		//Debug.Log(other.name);
		if (other.gameObject.name == ObjectToDestroy.name) {
			//Debug.Log("hit : " + ObjectToDestroy.name + ". Launch event");
			BulletHitEvent.Invoke();
		}
	}
}
