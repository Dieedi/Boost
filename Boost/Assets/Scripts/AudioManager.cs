using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour {

	[SerializeField] private AudioClip EndingSound;

	private AudioSource audioSource;

	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
	}

	public void PlayEndingSound ()
	{
		//Debug.Log("should play ending sound");
		audioSource.pitch = 1.0f;
		audioSource.clip = EndingSound;
		if (!audioSource.isPlaying)
			audioSource.Play();
	}
}
