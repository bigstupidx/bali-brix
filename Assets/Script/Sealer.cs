using UnityEngine;
using System.Collections;

public class Sealer : MonoBehaviour
{
	private float blinkDuration = 0.2f;
	private float timeLeft = 7f;
	private bool active = true;

	// Use this for initialization
	void Start ()
	{
	
	}
	
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
			timeLeft = 7f;
			active = false;
		}
	}

	private void Seal ()
	{
		this.GetComponent <SpriteRenderer> ().color = 
			new Color (255f, 255f, 255f, Mathf.PingPong (Time.time, blinkDuration) / blinkDuration + 0.7f);	
	}
}
