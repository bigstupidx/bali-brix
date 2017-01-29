using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDown : MonoBehaviour
{
	public float slowDownFactor = -1f;

	private float timeLeft = 7f;
	private bool active = true;
	private GameObject ball;
	private float xSpeed, ySpeed;
	private Vector2 defaultSpeed;

	// Use this for initialization
	void Start ()
	{
		ball = GameObject.Find ("Ball");
		defaultSpeed = ball.GetComponent <Ball> ().defaultSpeed;
		xSpeed = ball.GetComponent <Rigidbody2D> ().velocity.x;
		ySpeed = (ball.GetComponent <Rigidbody2D> ().velocity.y > 0) ?
				ball.GetComponent <Rigidbody2D> ().velocity.y + slowDownFactor :
				ball.GetComponent <Rigidbody2D> ().velocity.y - slowDownFactor;

		ball.GetComponent <Rigidbody2D> ().velocity = new Vector2 (xSpeed, ySpeed);
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
			print (ball.GetComponent <Rigidbody2D> ().velocity);
			ResetSpeed ();
		}
	}

	private void ResetSpeed ()
	{
		ySpeed = (ball.GetComponent <Rigidbody2D> ().velocity.y > 0) ?
				ball.GetComponent <Rigidbody2D> ().velocity.y - slowDownFactor / 2 :
				ball.GetComponent <Rigidbody2D> ().velocity.y + slowDownFactor / 2;
		if (Mathf.Abs (ySpeed) < defaultSpeed.y) {
			ySpeed = (ySpeed > 0) ? defaultSpeed.y : -defaultSpeed.y;
		}
		ball.GetComponent <Rigidbody2D> ().velocity = 
				new Vector2 (ball.GetComponent <Rigidbody2D> ().velocity.x, ySpeed);
	}
}

