using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
	public GameObject bullet;
	public float bulletSpeed = 10;

	// one bullet every 0.8 sec
	private float shootingRate = 1f;
	private float timeLeft = 7f;
	private bool gun = false;
	private bool loaded = true;
	private bool active = true;
	private GameObject paddle;

	// Use this for initialization
	void Start ()
	{
		paddle = GameObject.Find ("Paddle");
	}
	
	// Update is called once per frame
	void Update ()
	{
		shootingRate -= Time.deltaTime;
		if (active) {
			Countdown ();
		
			if (shootingRate < 0f) {
				shootingRate = 1f;
				loaded = true;
				print ("reloaded"); 
			}
			if (Input.GetButton ("Fire1") && loaded) {
				print ("calling fire");
				Fire ();
			}
		}
	}

	private void Countdown ()
	{
		timeLeft -= Time.deltaTime;
		if (timeLeft < 0) {
			timeLeft = 7f;
			active = false;
		}
	}

	private void Fire ()
	{
		loaded = false;
		Vector3 pos = new Vector3 (0f, 1.0f, 0f);
		GameObject bulletClone = 
			Instantiate (bullet, paddle.transform.position + pos, transform.rotation) as GameObject;
		bulletClone.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 14f);
		print ("fired");
		Destroy (bulletClone, 0.8f);
	}
}
