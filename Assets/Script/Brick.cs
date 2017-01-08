using UnityEngine;
using System.Collections;

public class Brick : MonoBehaviour
{
	public Sprite[] hitSprites;

	// we use statis to make sure there is only one variable across all Brick classes
	// that way we can increase/decrease the same variable inside each Brick independently
	public static int brickCounts = 0;
	private int timesHit;
	private LevelManager levelManager;
	private bool isBreakable;

	// Use this for initialization
	void Start ()
	{
		isBreakable = (this.tag == "Breakable");
		if (isBreakable)
			brickCounts++;
		timesHit = 0;
		levelManager = GameObject.FindObjectOfType<LevelManager> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnCollisionEnter2D (Collision2D collision)
	{
		if (isBreakable)
			HandleHits ();
	}

	void HandleHits ()
	{
		timesHit++;
		if (timesHit >= hitSprites.Length + 1) {
			brickCounts--;
			Destroy (gameObject);	
			if (brickCounts <= 0)
				levelManager.LoadNextLevel ();
		} else {
			LoadCrackedBrick ();
		}
	}

	void LoadCrackedBrick ()
	{
		int index = timesHit - 1;
		this.GetComponent<SpriteRenderer> ().sprite = hitSprites [index];
	}
}
