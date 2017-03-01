using UnityEngine;
using System.Collections;
using System;

public class Paddle : MonoBehaviour
{
	public bool autoPlay = false;
	private Vector3 paddlePos;
	private float mousePoseInBlocks;
	private Ball ball;

	// Use this for initialization
	void Start ()
	{
		paddlePos = new Vector3 (0.0f, this.transform.position.y, 0f);
		ball = GameObject.FindObjectOfType<Ball> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!autoPlay) {
			MovePaddle ();
		} else {
			AutoPlay ();
		}
	}

	void MovePaddle ()
	{
		float mousePoseInBlocks = Input.mousePosition.x / Screen.width * 8f;
		paddlePos.x = Mathf.Clamp (mousePoseInBlocks - 4f, -3.0f, 3.0f);
		this.transform.position = paddlePos;
	}

	void AutoPlay ()
	{
		paddlePos.x = ball.transform.position.x;
		this.transform.position = paddlePos;
	}
}
