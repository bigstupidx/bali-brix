using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Brick : MonoBehaviour
{
	public Sprite[] hitSprites;
	public GameObject particles;

	// we use statis to make sure there is only one variable across all Brick classes
	// that way we can increase/decrease the same variable inside each Brick independently
	public static int brickCounts = 0;
	public AudioClip crack;
	private int timesHit;
	private LevelManager levelManager;
	private GameObject score;

	// Use this for initialization
	void Start ()
	{
		timesHit = 0;
		levelManager = GameObject.FindObjectOfType<LevelManager> ();
		score = GameObject.Find ("Score");
		score.GetComponent <Text> ().text = LevelManager.score.ToString ();
		if (this.tag == "Breakable")
			brickCounts++;
	}

	void OnCollisionEnter2D (Collision2D collision)
	{
		AudioSource.PlayClipAtPoint (crack, this.transform.position);
		if (this.tag == "Breakable") {
			LevelManager.score += 10;
			score.GetComponent <Text> ().text = LevelManager.score.ToString ();
			HandleHits ();		
		}
	}

	void HandleHits ()
	{
		timesHit++;
		if (timesHit >= hitSprites.Length + 1) {
			brickCounts--;
			UpdateView (this.gameObject);
			if (brickCounts <= 0)
				levelManager.LoadNextLevel ();
		} else {
			LoadCrackedBrick ();
		}
	}

	void ShowParticles ()
	{
		GameObject dust = 
			Instantiate (particles, this.gameObject.transform.position, Quaternion.identity) as GameObject;
		dust.GetComponent<ParticleSystem> ().startColor = this.GetComponent<SpriteRenderer> ().color;
	}

	void UpdateView (GameObject brick)
	{
		ShowParticles ();
		Destroy (this.gameObject);
		levelManager.IncreaseBackgroundAlpha ();
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
