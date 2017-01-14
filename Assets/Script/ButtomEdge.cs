using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtomEdge : MonoBehaviour
{
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
		Ball.hasStarted = false;
		if (LevelManager.ballCounts-- <= 1) {
			LevelManager.ballCounts = 3;
			levelManager.LoadLevel ("Loose");	
		}
		balls.GetComponent <Text> ().text = LevelManager.ballCounts.ToString ();
	}
}
