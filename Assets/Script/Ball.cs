using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
	public AudioClip hit;
	public static bool hasStarted = false;
	public static bool bonusPaid = false;
	public static int bonusFactor = 1;
	public Vector2 defaultSpeed = new Vector2 (2f, 6.8f);
	public GameObject tripleBall;

	private Paddle paddle;
	private Vector3 paddleToBallVector;

	// Use this for initialization
	void Start ()
	{
		paddle = GameObject.FindObjectOfType<Paddle> ();
		if (!hasStarted)
			this.transform.position = new Vector3 (paddle.transform.position.x, -4.5f, 0);
		paddleToBallVector = this.transform.position - paddle.transform.position;
	}
	
	// Update is called once per frame
	void Update ()
	{
		float direction = 0f;
		if (!hasStarted) {
			this.transform.position = paddle.transform.position + paddleToBallVector;
			if (!CanvasManager.canvasActive && Input.GetButtonDown ("Fire1")) {
				// set the ball direction at the beginning
				direction = (this.transform.position.x > 0) ? 1f : -1f;
				this.GetComponent <Rigidbody2D> ().velocity = 
					new Vector2 (defaultSpeed.x * direction, defaultSpeed.y);
				// Check if TrippleBall PowerUp is added
				if (LevelManager.tripleBall == true) {
					GameObject tripleBallClone =
						Instantiate (tripleBall, new Vector3 (0f, -5.8f, 0f), transform.rotation) as GameObject;
					LevelManager.tripleBall = false;
				}
				hasStarted = true;
			}		
		}		
	}

	void OnCollisionEnter2D (Collision2D collision)
	{
		float ySpeed = Random.Range (-0.001f, 0.02f);
		float xSpeed = Random.Range (-0.001f, 0.005f);

		//Vector2 speedUp = new Vector2 (Random.Range (-0.4f, 0.4f), Random.Range (-0.1f, 0.4f));

		if (hasStarted) {
			AudioSource.PlayClipAtPoint (hit, this.transform.position);	
			this.GetComponent <Rigidbody2D> ()
					.AddForce (new Vector2 (
				(this.GetComponent <Rigidbody2D> ().velocity.x > 0) ? xSpeed : -xSpeed, 
				(this.GetComponent <Rigidbody2D> ().velocity.y > 0) ? ySpeed : -ySpeed 
			), ForceMode2D.Impulse);
	
			SetSpeedLimits (this.GetComponent <Rigidbody2D> ().velocity);

			/*print ("current speed (x,y): ( " +
			this.GetComponent <Rigidbody2D> ().velocity.x + " , " +
			this.GetComponent <Rigidbody2D> ().velocity.y + " )");*/
			
			//this.GetComponent <Rigidbody2D> ().velocity += speedUp;
			if (Brick.brickCounts <= 0) // Make sure everything stops after last brick was hit
				Destroy (this.gameObject);
		}		
	}

	private void SetSpeedLimits (Vector2 speed)
	{
		// fix the straight horizontal/vertical movements
		if (speed.x > -0.3f && speed.x < 0.3f)
			speed.x = (speed.x < 0) ? -1f : 1f;
		if (speed.y > -5f && speed.y < 5f)
			speed.y = (speed.y < 0) ? -7f : 7f;
		Vector2 newSpeed = new Vector2 (
			                   Mathf.Clamp (speed.x, -7f, 7f),
			                   Mathf.Clamp (speed.y, -15f, 15f)
		                   );
		this.GetComponent <Rigidbody2D> ().velocity = newSpeed;
	}
}