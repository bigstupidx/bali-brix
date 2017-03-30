using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseBall : MonoBehaviour
{
	private bool active = true;
	private int activeBalls;

	// Use this for initialization
	void Start ()
	{
		activeBalls = GameObject.FindGameObjectsWithTag ("Ball").Length;
		if (activeBalls > 0) {
			ActivateCatcher ();
		}
	}

	// Update is called once per frame
	void Update ()
	{
		if (active) {
			
		}
	}

	private void ActivateCatcher ()
	{
		GameObject.FindObjectOfType<Paddle> ().GetComponent <PolygonCollider2D> ().isTrigger = true;
		//Destroy (this.gameObject);
	}
		
}
