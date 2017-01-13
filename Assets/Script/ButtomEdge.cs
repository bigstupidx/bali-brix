using UnityEngine;
using System.Collections;

public class ButtomEdge : MonoBehaviour
{
	private LevelManager levelManager;

	void Start ()
	{
		// Using FindObjectOfType<> () we can attach game objects programatically and 
		// defining them as public (and drag the object to them in the Unity is no longer needed
		levelManager = GameObject.FindObjectOfType<LevelManager> ();
	}

	// This is where a Collider object triggers
	void OnTriggerEnter2D (Collider2D trigger)  // Type Collider2D
	{
		
	}

	// This is where A Collision object collides
	void OnCollisionEnter2D (Collision2D collision) // Type Collision2D
	{
		if (Ball.ballCounts-- <= 1)
			levelManager.LoadLevel ("Loose");
		else
			levelManager.Reload ();
	}
}
