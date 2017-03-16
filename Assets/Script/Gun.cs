using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
	public GameObject bullet;
	public float bulletSpeed = 18f;

	// one bullet every 0.5 sec
	private float shootingRate = 0.5f;
	private float timeLeft = 10f;
	private bool active = true;
	private bool gun = false;
	private bool loaded = true;
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
				shootingRate = 0.5f;
				loaded = true; 
			}
			if (Input.GetButton ("Fire1") && loaded) {
				Fire ();
			}
		}
	}

	private void Countdown ()
	{
		timeLeft -= Time.deltaTime;
		if (timeLeft < 0) {
			//timeLeft = 7f;
			active = false;
		}
	}

	private void Fire ()
	{
		loaded = false;
		Vector3 pos = new Vector3 (0f, 1.0f, 0f);
		GameObject bulletClone = 
			Instantiate (bullet, paddle.transform.position + pos, transform.rotation) as GameObject;
		bulletClone.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, bulletSpeed);
		Destroy (bulletClone, 1.1f);
	}
}
