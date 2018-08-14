using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeControlsController : MonoBehaviour {

	[SerializeField] string[] controlsStrings = new string[] { "Drag Joystick", "Left/Right buttons"};

	private Text controlsText;
	private GameManager gm;

	private void Awake()
	{
		gm = FindObjectOfType<GameManager>();
		controlsText = GetComponentInChildren<Text>();

		ChangeText();
	}

	public void ChangeText()
	{
		if (gm.dragControls) {
			controlsText.text = controlsStrings[0];
		} else {
			controlsText.text = controlsStrings[1];
		}
	}

}
