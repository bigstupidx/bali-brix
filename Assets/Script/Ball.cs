using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{
	public AudioClip hit;
	public static bool hasStarted = false;
	public static bool bonusPaid = false;
	public static int bonusFactor = 1;
	public Vector2 defaultSpeed = new Vector2 (2.25f, 9.2f);

	private Paddle paddle;
	private Vector3 paddleToBallVector;

	// Use this for initialization
	void Start ()
	{
		paddle = GameObject.FindObjectOfType<Paddle> ();
		paddleToBallVector = this.transform.position - paddle.transform.position;
	}
	
	// Update is called once per frame
	void Update ()
	{
		float direction = 0f;
		if (!hasStarted) {
			this.transform.position = paddle.transform.position + paddleToBallVector;
			if (Input.GetButtonDown ("Fire1")) {
				// set the ball direction at the beginning
				direction = (this.transform.position.x > 0) ? 1f : -1f;
				this.GetComponent <Rigidbody2D> ().velocity = 
					new Vector2 (defaultSpeed.x * direction, defaultSpeed.y);
				hasStarted = true;
			}		
		}
	}

	void OnCollisionEnter2D (Collision2D collision)
	{
		float ySpeed = Random.Range (-0.002f, 0.011f);
		float xSpeed = Random.Range (-0.002f, 0.011f);

		//Vector2 speedUp = new Vector2 (Random.Range (-0.4f, 0.4f), Random.Range (-0.1f, 0.4f));

		if (hasStarted) {
			AudioSource.PlayClipAtPoint (hit, this.transform.position);	
			this.GetComponent <Rigidbody2D> ()
					.AddForce (new Vector2 (
				(this.GetComponent <Rigidbody2D> ().velocity.x > 0) ? xSpeed : -xSpeed, 
				(this.GetComponent <Rigidbody2D> ().velocity.y > 0) ? ySpeed : -ySpeed 
			), ForceMode2D.Impulse);
	
			SetSpeedLimits (this.GetComponent <Rigidbody2D> ().velocity);

			//this.GetComponent <Rigidbody2D> ().velocity += speedUp;
			if (Brick.brickCounts <= 0) // Make sure everything stops after last brick was hit
				Destroy (this.gameObject);
		}		
	}

	private void SetSpeedLimits (Vector2 speed)
	{
		print ("current speed: " + speed);
		// fix the straight horizontal/vertical movements
		if (speed.x > -0.07f && speed.x < 0.07f)
			speed.x = (speed.x < 0) ? -1f : 1f;
		if (speed.y > -7f && speed.y < 7f)
			speed.y = (speed.y < 0) ? -8f : 8f;
		Vector2 newSpeed = new Vector2 (
			                   Mathf.Clamp (speed.x, -9f, 9f),
			                   Mathf.Clamp (speed.y, -15f, 15f)
		                   );
		this.GetComponent <Rigidbody2D> ().velocity = newSpeed;

		print ("fixed speed: " + newSpeed);
	}
}