using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkPaddle : MonoBehaviour
{

	public float shrinkFactor = -0.2f;

	private float timeLeft = 7f;
	private bool active = true;
	private GameObject paddle;
	// Use this for initialization
	void Start ()
	{
		paddle = GameObject.Find ("Paddle");
		paddle.transform.localScale += new Vector3 (shrinkFactor, 0.22f, 0f);
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
			ResetPaddle ();
		}
	}

	private void ResetPaddle ()
	{
		paddle.transform.localScale += new Vector3 (-shrinkFactor, -0.22f, 0f);
	}
}
