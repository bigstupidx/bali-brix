using UnityEngine;
using System.Collections;

public class FallingObjects : MonoBehaviour
{
	public Sprite[] fallingObjects;
	public AudioClip powerUp, powerDown;
	private float duration = 0.3f;
	private float alpha = 0f;


	// Use this for initialization
	void Start ()
	{
	
	}

	// Update is called once per frame
	void Update ()
	{
		alpha = Mathf.PingPong (Time.time, duration) / duration;
		this.GetComponent <SpriteRenderer> ().color = new Color (255f, 255f, 255f, alpha + 0.5f);			
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		Sprite sprite = this.GetComponent <SpriteRenderer> ().sprite;
		if (other.tag == "Paddle") {
			switch (sprite) {
			case fallingObjects [1]:
				GetGuns ();
				break;
			case fallingObjects [2]:
				GetGuns ();
				break;
			case fallingObjects [3]:
				GetGuns ();
				break;
			case fallingObjects [4]:
				GetGuns ();
				break;
			case fallingObjects [5]:
				GetGuns ();
				break;
			case fallingObjects [6]:
				GetGuns ();
				break;
			case fallingObjects [7]:
				GetGuns ();
				break;
			case fallingObjects [8]:
				GetGuns ();
				break;
			case fallingObjects [9]:
				GetGuns ();
				break;
			case fallingObjects [10]:
				GetGuns ();
				break;
			default:
				print ("something is not right!");
				break;
			}
			if (sprite == fallingObjects [4] || sprite == fallingObjects [6]) {
				AudioSource.PlayClipAtPoint (powerDown, this.transform.position);	
			} else {
				AudioSource.PlayClipAtPoint (powerUp, this.transform.position);	
			}
			print ("badaaaa!"); 
		}
	}

	public Sprite SetTheBall (int index)
	{
		return fallingObjects [index];
	}

	private void GetGuns ()
	{
		
	}
}
