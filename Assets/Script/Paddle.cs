using UnityEngine;
using System.Collections;
using System;

public class Paddle : MonoBehaviour
{
	private Vector3 paddlePos;
	float mousePoseInBlocks;

	// Use this for initialization
	void Start ()
	{
		paddlePos = new Vector3 (0.0f, this.transform.position.y, 0f);
	}
	
	// Update is called once per frame
	void Update ()
	{
		float mousePoseInBlocks = Input.mousePosition.x / Screen.width * 16;
		paddlePos.x = Mathf.Clamp (mousePoseInBlocks - 8f, -7.5f, 7.5f);

		this.transform.position = paddlePos;
		print ("paddle: " + paddlePos);
	}
}
