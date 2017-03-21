using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtomEdge : MonoBehaviour
{
	public AudioClip missed;
	private LevelManager levelManager;
	private GameObject balls;

	void Start ()
	{
		// Using FindObjectOfType<> () we can attach game objects programatically and 
		// defining them as public (and drag the object to them in the Unity is no longer needed
		levelManager = GameObject.FindObjectOfType<LevelManager> ();
		balls = GameObject.Find ("Balls No");
		balls.GetComponent <Text> ().text = LevelManager.ballCounts.ToString ();
	}

	// This is where a Collider object triggers
	void OnTriggerEnter2D (Collider2D trigger)  // Type Collider2D
	{
		
	}

	// This is where A Collision object collides
	void OnCollisionEnter2D (Collision2D collision) // Type Collision2D
	{
		AudioSource.PlayClipAtPoint (missed, this.transform.position);	
		// ToDo: Find out if there are other balls in the game

		Ball.hasStarted = HasBalls ();
		Destroy (collision.gameObject);
		if (LevelManager.ballCounts-- <= 1) {
			print ("total: " + levelManager.totalBricks + " available: " + Brick.brickCounts);
			print (" Damage: " + (float)(levelManager.totalBricks - Brick.brickCounts) / (float)levelManager.totalBricks);
			levelManager.EvalDamage (levelManager.totalBricks - Brick.brickCounts);
		}
		balls.GetComponent <Text> ().text = LevelManager.ballCounts.ToString ();
	}

	private bool HasBalls ()
	{
		return true;
	}
}
