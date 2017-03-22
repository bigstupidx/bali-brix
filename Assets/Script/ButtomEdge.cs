using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtomEdge : MonoBehaviour
{
	public AudioClip missed;
	public GameObject ball;
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



	// This is where A Collision object collides
	void OnCollisionEnter2D (Collision2D collision) // Type Collision2D
	{
		Destroy (collision.gameObject, 0f);
		//DestroyImmediate (collision.gameObject);
		AudioSource.PlayClipAtPoint (missed, this.transform.position);	
		StartCoroutine (CheckActiveBalls ());
	}

	private IEnumerator CheckActiveBalls ()
	{
		yield return new WaitForFixedUpdate ();
		bool active = HasBall ();
		Ball.hasStarted = active;
		print ("Are we active?" + active);
		if (!active) {
			if (LevelManager.ballCounts-- <= 0) {
				levelManager.EvalDamage (levelManager.totalBricks - Brick.brickCounts);
			} else {
				Instantiate (ball);
				print ("We are here and here is the instantiated ball: " + ball);
			}
			balls.GetComponent <Text> ().text = LevelManager.ballCounts.ToString ();
		}
	}

	private bool HasBall ()
	{
		return (GameObject.FindGameObjectWithTag ("Ball") != null) ? true : false;
	}
}
