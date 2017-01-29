using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDown : MonoBehaviour
{
	public float slowDownFactor = -100f;

	private float timeLeft = 7f;
	private bool active = true;
	private GameObject ball;

	// Use this for initialization
	void Start ()
	{
		ball = GameObject.Find ("Ball");
		if (ball.GetComponent <Rigidbody2D> ().velocity.y > 0) {
			ball.GetComponent <Rigidbody2D> ()
				.AddForce (new Vector2 (slowDownFactor / 3, slowDownFactor), ForceMode2D.Impulse);
		} else {
			ball.GetComponent <Rigidbody2D> ()
				.AddForce (new Vector2 (-slowDownFactor / 3, -slowDownFactor), ForceMode2D.Impulse);
		}
	}

	// Update is called once per frame
	void Update ()
	{
		if (active) {
			Countdown ();
		}
	}

	private void Countdown ()
	{
		timeLeft -= Time.deltaTime;
		if (timeLeft < 0) {
			timeLeft = 7f;
			active = false;
			ResetSpeed ();
		}
	}

	private void ResetSpeed ()
	{
		if (ball.GetComponent <Rigidbody2D> ().velocity.y > 0) {
			ball.GetComponent <Rigidbody2D> ()
				.AddForce (new Vector2 (-slowDownFactor / 3, -slowDownFactor), ForceMode2D.Impulse);
		} else {
			ball.GetComponent <Rigidbody2D> ()
				.AddForce (new Vector2 (slowDownFactor / 3, slowDownFactor), ForceMode2D.Impulse);
		}		
	}
}

