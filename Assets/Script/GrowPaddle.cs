using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowPaddle : MonoBehaviour
{
	public float growFactor = 0.6f;

	private float timeLeft = 10f;
	private bool active = true;
	private GameObject paddle;
	// Use this for initialization
	void Start ()
	{
		paddle = GameObject.Find ("Paddle");
		paddle.transform.localScale += new Vector3 (growFactor, -0.22f, 0f);
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
			//timeLeft = 10f;
			active = false;
			ResetPaddle ();
		}
	}

	private void ResetPaddle ()
	{
		paddle.transform.localScale += new Vector3 (-growFactor, 0.22f, 0f);
	}
}