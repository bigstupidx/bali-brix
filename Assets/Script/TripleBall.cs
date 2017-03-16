using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleBall : MonoBehaviour
{
	private GameObject currentBall, ball2, ball3;
	private float x, y, xSpeed, ySpeed;
	// Use this for initialization
	void Start ()
	{
		currentBall = GameObject.Find ("Ball");
		x = currentBall.transform.position.x;
		y = currentBall.transform.position.y;
		xSpeed = currentBall.GetComponent <Rigidbody2D> ().velocity.x;
		ySpeed = currentBall.GetComponent <Rigidbody2D> ().velocity.y;
		InitializeClones ();
	}

	private void InitializeClones ()
	{
		ball2 = Instantiate (currentBall, new Vector3 (x, y, 0f), transform.rotation) as GameObject;
		ball2.GetComponent <Rigidbody2D> ()
			.AddForce (new Vector2 (
			(xSpeed > 0) ? -(2 * xSpeed) : (2 * xSpeed), 
			(ySpeed > 0) ? (ySpeed / 10) : -(ySpeed / 10) 
		), ForceMode2D.Impulse);
		ball3 = Instantiate (currentBall, new Vector3 (x, y, 0f), transform.rotation) as GameObject;
		ball3.GetComponent <Rigidbody2D> ()
			.AddForce (new Vector2 (
			(xSpeed > 0) ? (xSpeed / 10) : -(xSpeed / 10), 
			(ySpeed > 0) ? -(2 * ySpeed) : (2 * ySpeed)
		), ForceMode2D.Impulse);
	}
	

}
