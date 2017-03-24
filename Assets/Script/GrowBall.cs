using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowBall : MonoBehaviour
{
	private float timeLeft = 10f;
	private bool active = true;
	private GameObject ball;

	// Use this for initialization
	void Start ()
	{
		ball = GameObject.Find ("Ball");
		print (ball.transform.localScale.x);
		ball.transform.localScale += new Vector3 (0.3f, 0.3f, 0f);
		print (ball.transform.localScale.x);
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
			ResetSize ();
		}
	}

	private void ResetSize ()
	{
		if (ball)
			ball.transform.localScale = new Vector3 (1f, 1f, 1f);
	}
}
