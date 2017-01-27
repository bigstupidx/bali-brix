using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class FallingObjects : MonoBehaviour
{
	public Sprite[] fallingObjects;
	public AudioClip powerUp, powerDown;
	public float bulletSpeed = 10;
	public GameObject bullet;

	private bool active = false;
	private bool gun = false;
	private bool loaded = false;
	private float timeLeft = 7f;
	// one bullet every 0.8 sec
	private float shootingRate = 0.8f;
	private float blinkDuration = 0.3f;
	private GameObject paddle;
	private GameObject ball;

	// Use this for initialization
	void Start ()
	{
		paddle = GameObject.Find ("Paddle");
		ball = GameObject.Find ("Ball");
	}

	// Update is called once per frame
	void Update ()
	{
		this.GetComponent <SpriteRenderer> ().color = 
			new Color (255f, 255f, 255f, Mathf.PingPong (Time.time, blinkDuration) / blinkDuration + 0.5f);	
		if (active) {
			UpdateTimer ();
			shootingRate -= Time.deltaTime;
			if (shootingRate < 0f) {
				shootingRate = 0.8f;
				loaded = true;
				print ("reloaded"); 
			}
			if (Input.GetButton ("Fire1") && loaded) {
				Fire ();
				//InvokeRepeating ("Fire", 0f, 2f);

				//StartCoroutine (Fire ());
			}
		}
	}

	private void Fire ()
	{
		loaded = false;
		Vector3 pos = new Vector3 (0f, 1.0f, 0f);
		GameObject bulletClone = 
			Instantiate (bullet, paddle.transform.position + pos, transform.rotation) as GameObject;
		bulletClone.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 13f);
		Destroy (bulletClone, 1.0f);
	}

	private void UpdateTimer ()
	{
		timeLeft -= Time.deltaTime;
		if (timeLeft < 0) {
			timeLeft = 7f;
			active = false;
		}
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "Paddle") {
			GetGuns ();
			switch (this.GetComponent <SpriteRenderer> ().sprite.name) {
			case "balls_1":
				AudioSource.PlayClipAtPoint (powerUp, this.transform.position);	
				GetGuns ();
				break;
			case "balls_2":
				AudioSource.PlayClipAtPoint (powerUp, this.transform.position);	
				SealFloor ();
				break;
			case "balls_3":
				AudioSource.PlayClipAtPoint (powerDown, this.transform.position);	
				SpeedUp ();
				break;
			case "balls_4":
				AudioSource.PlayClipAtPoint (powerUp, this.transform.position);	
				SlowDown ();
				break;
			case "balls_5":
				AudioSource.PlayClipAtPoint (powerUp, this.transform.position);	
				GrowPaddle ();
				break;
			case "balls_6":
				AudioSource.PlayClipAtPoint (powerDown, this.transform.position);	
				ShrinkPaddle ();
				break;
			case "balls_7":
				AudioSource.PlayClipAtPoint (powerUp, this.transform.position);	
				AddLife ();
				break;
			case "balls_8":
				AudioSource.PlayClipAtPoint (powerUp, this.transform.position);	
				BreakThrough ();
				break;
			case "balls_9":
				AudioSource.PlayClipAtPoint (powerUp, this.transform.position);	
				DoubleBalls ();
				break;
			case "balls_10":
				AudioSource.PlayClipAtPoint (powerUp, this.transform.position);	
				PauseBall ();
				break;
			default:
				print ("something is not right!");
				break;
			}
		}
	}

	public Sprite SetTheBall (int index)
	{
		return fallingObjects [index];
	}

	private void GetGuns ()
	{
		active = true;
		loaded = true;
	}



	private void SealFloor ()
	{

	}

	private void SpeedUp ()
	{

	}

	private void SlowDown ()
	{

	}

	private void GrowPaddle ()
	{

	}

	private void ShrinkPaddle ()
	{

	}

	private void AddLife ()
	{

	}

	private void BreakThrough ()
	{

	}

	private void DoubleBalls ()
	{

	}

	private void PauseBall ()
	{

	}
}
