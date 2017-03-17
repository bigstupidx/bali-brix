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
			(ySpeed > 0) ? (ySpeed / 20) : -(ySpeed / 20) 
		), ForceMode2D.Impulse);
		
		ball3 = Instantiate (currentBall, new Vector3 (x, y, 0f), transform.rotation) as GameObject;
		ball3.GetComponent <Rigidbody2D> ()
			.AddForce (new Vector2 (
			(xSpeed > 0) ? (xSpeed / 10f) : -(xSpeed / 10f), 
			(ySpeed > 0) ? -(0.01f * ySpeed) : (0.01f * ySpeed)
		), ForceMode2D.Impulse);

		ball2.GetComponent <SpriteRenderer> ().color = new Color (255f, 0f, 0f, 255f);
		ball3.GetComponent <SpriteRenderer> ().color = new Color (0f, 255f, 0f, 255f);
	}
}