using UnityEngine;
using System.Collections;

public class Brick : MonoBehaviour
{
	public Sprite[] hitSprites;
	public GameObject particles;

	// we use statis to make sure there is only one variable across all Brick classes
	// that way we can increase/decrease the same variable inside each Brick independently
	public static int brickCounts = 0;
	public static Hashtable brickMap;
	public AudioClip crack;
	private int timesHit;
	private LevelManager levelManager;
	private bool isBreakable;

	// Use this for initialization
	void Start ()
	{
		brickMap [this.name] = true;
		isBreakable = (this.tag == "Breakable");
		if (isBreakable && !IsDestroyed (this.name))
			brickCounts++;
		timesHit = 0;
		print ("I am brick:" + this.GetInstanceID () + "and my count is:" + brickCounts);

		levelManager = GameObject.FindObjectOfType<LevelManager> ();
	}
	
	// Update is called once per frame
	bool IsDestroyed (string name)
	{
	
	}

	void OnCollisionEnter2D (Collision2D collision)
	{
		AudioSource.PlayClipAtPoint (crack, this.transform.position);
		if (isBreakable)
			HandleHits ();
	}

	void HandleHits ()
	{
		timesHit++;
		if (timesHit >= hitSprites.Length + 1) {
			brickCounts--;
			showParticles ();
			Destroy (this.gameObject);	
			if (brickCounts <= 0)
				levelManager.LoadNextLevel ();
		} else {
			LoadCrackedBrick ();
		}
	}

	void showParticles ()
	{
		GameObject dust = 
			Instantiate (particles, this.gameObject.transform.position, Quaternion.identity) as GameObject;
		dust.GetComponent<ParticleSystem> ().startColor = this.GetComponent<SpriteRenderer> ().color;
	}

	void LoadCrackedBrick ()
	{
		int index = timesHit - 1;
		if (hitSprites [index] != null)
			this.GetComponent<SpriteRenderer> ().sprite = hitSprites [index];
		else
			Debug.LogError ("Sprite is missing!");
	}
}
