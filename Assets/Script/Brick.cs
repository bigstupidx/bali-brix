using UnityEngine;
using System.Collections;

public class Brick : MonoBehaviour
{
	public Sprite[] hitSprites;
	private int timesHit;
	private LevelManager levelManager;

	// Use this for initialization
	void Start ()
	{
		timesHit = 0;
		levelManager = GameObject.FindObjectOfType<LevelManager> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnCollisionEnter2D (Collision2D collision)
	{
		if (this.tag != "Unbreakable")
			HandleHits ();
	}

	void HandleHits ()
	{
		timesHit++;
		if (timesHit >= hitSprites.Length + 1) {
			Destroy (gameObject);	
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
