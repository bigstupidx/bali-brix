﻿using UnityEngine;
using System.Collections;

public class Sealer : MonoBehaviour
{
	private float blinkDuration = 1f;
	private float timeLeft = 10f;
	private bool active = true;

	// Update is called once per frame
	void Update ()
	{
		if (active) {
			Countdown ();
			Seal ();
		}
	}

	private void Countdown ()
	{
		timeLeft -= Time.deltaTime;
		if (timeLeft < 0) {
			active = false;
			Destroy (this.gameObject);
			//timeLeft = 7f;
		}
	}

	private void Seal ()
	{
		this.GetComponent <SpriteRenderer> ().color = 
			new Color (255f, 255f, 255f, Mathf.PingPong (Time.time, blinkDuration) / blinkDuration + 0.55f);	
	}
}
