using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour {

	private static AudioManager instance = null;

	public static AudioManager Instance
	{
		get {
			return instance;
		}
	}

	[SerializeField] private AudioClip EndingSound;

	private AudioSource audioSource;

	private void Awake()
	{
		if (instance != null && instance != this) {
			Destroy(gameObject);
			return;
		} else {
			instance = this;
		}
		DontDestroyOnLoad(gameObject);
	}

	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
	}

	public void PlayEndingSound ()
	{
		////Debug.Log("should play ending sound");
		//audioSource.pitch = 1.0f;
		//audioSource.clip = EndingSound;
		//if (!audioSource.isPlaying)
		//	audioSource.Play();
	}
}
