using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDown : MonoBehaviour
{
	public float slowDownFactor;

	private float timeLeft = 10f;
	private bool active = true;
	private GameObject ball;

	// Use this for initialization
	void Start ()
	{
		ball = GameObject.Find ("Ball");
		ball.GetComponent <Rigidbody2D> ().AddForce (new Vector2 (
			(ball.GetComponent <Rigidbody2D> ().velocity.x > 0) ? (slowDownFactor / 5) : (-slowDownFactor / 5),
			(ball.GetComponent <Rigidbody2D> ().velocity.y > 0) ? (slowDownFactor) : (-slowDownFactor)
		));
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
			active = false;
			ResetSpeed ();
		}
	}

	private void ResetSpeed ()
	{
		ball.GetComponent <Rigidbody2D> ().AddForce (new Vector2 (
			(ball.GetComponent <Rigidbody2D> ().velocity.x > 0) ? (-slowDownFactor / 5) : (slowDownFactor / 5),
			(ball.GetComponent <Rigidbody2D> ().velocity.y > 0) ? (-slowDownFactor) : (slowDownFactor)
		));
	}
}

