using UnityEngine;
using System.Collections;

public class ButtomEdge : MonoBehaviour
{
	public LevelManager levelManager;
	// This is where a Collider object triggers
	void OnTriggerEnter2D (Collider2D trigger)  // Type Collider2D
	{
		print ("Triggered");
	}

	// This is where A Collision object collides
	void OnCollisionEnter2D (Collision2D collision) // Type Collision2D
	{
		print ("Collided");
		levelManager.LoadLevel ("Win");
	}
}
