using UnityEngine;
using System.Collections;

public class FallingObjects : MonoBehaviour
{
	public Sprite[] fallingObjects;
	public AudioClip powerUp, powerDown;
	public GameObject bullet;

	private float duration = 0.3f;
	private bool active = false;
	private float timeLeft = 10f;

	// Use this for initialization
	void Start ()
	{
	
	}

	// Update is called once per frame
	void Update ()
	{
		this.GetComponent <SpriteRenderer> ().color = 
			new Color (255f, 255f, 255f, Mathf.PingPong (Time.time, duration) / duration + 0.5f);	
		if (active) {
			UpdateTimer ();
		}
	}

	private void UpdateTimer ()
	{
		timeLeft -= Time.deltaTime;
		if (timeLeft < 0) {
			timeLeft = 10f;
			active = false;
		}
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "Paddle") {
			active = true;
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
		while (active) {
			Shoot ();
		}
	}

	private void Shoot ()
	{
		if (Input.GetMouseButton (0) || Input.GetMouseButtonDown (0)) {
			GameObject b = 
				Instantiate (bullet, this.gameObject.transform.position, Quaternion.identity) as GameObject;
			while (b) {
				b.transform.position += new Vector3 (0f, 1f, 0f);
			}
		}
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
