using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//using System;

public class Brick : MonoBehaviour
{
	// we use statis to make sure there is only one variable across all Brick classes
	// that way we can increase/decrease the same variable inside each Brick independently
	public static int brickCounts = 0;
	public AudioClip crack;
	public Sprite[] hitSprites;
	public GameObject particles;
	public GameObject fallingObject;

	private bool hasBall = false;
	private int fallingBallIndex;
	private int timesHit;
	private LevelManager levelManager;
	private GameObject score, levelCompleteScore;
	private GameObject balls, dust;

	// Use this for initialization
	void Start ()
	{
		timesHit = 0;
		levelManager = GameObject.FindObjectOfType<LevelManager> ();
		score = GameObject.Find ("Score");
		score.GetComponent <Text> ().text = LevelManager.currentScore.ToString ();
		levelCompleteScore = GameObject.Find ("Level Complete Score");
		if (levelManager.fallingObjects > 0 && Random.Range (0f, 1f) > 0.5f) {
			hasBall = true;
			levelManager.fallingObjects--;
			SetBall ();
		}

		balls = GameObject.Find ("Balls No");
		balls.GetComponent <Text> ().text = LevelManager.ballCounts.ToString ();

		if (this.tag == "Breakable")
			brickCounts++;
	}

	private void SetBall ()
	{
		int max = (int)Mathf.Clamp (SceneManager.GetActiveScene ().buildIndex, 3, 5);
		fallingBallIndex = 5;//(int)Random.Range (1, max);  
	}


	void OnCollisionEnter2D (Collision2D collision)
	{
		AudioSource.PlayClipAtPoint (crack, this.transform.position);
		if (this.tag == "Breakable") {
			HandleScores ();
			HandleHits ();		
		}
	}

	void HandleScores ()
	{
		LevelManager.currentScore += 15;
		score.GetComponent <Text> ().text = LevelManager.currentScore.ToString ();
		levelCompleteScore.GetComponent <Text> ().text = LevelManager.currentScore.ToString ();
		if ((LevelManager.currentScore / (1000 * Ball.bonusFactor)) >= 1) {
			levelManager.AddBonusBall (balls);
		}
	}

	void HandleHits ()
	{
		timesHit++;
		if (timesHit >= hitSprites.Length + 1) {
			brickCounts--;
			UpdateView (this.gameObject);
			if (brickCounts <= 0) {
				levelManager.EvalDamage (0, true);
			}
		} else {
			LoadCrackedBrick ();
		}
	}

	void ShowParticles ()
	{
		dust = 
			Instantiate (particles, this.gameObject.transform.position, Quaternion.identity) as GameObject;
		dust.GetComponent<ParticleSystem> ().startColor = this.GetComponent<SpriteRenderer> ().color;
	}

	void UpdateView (GameObject brick)
	{
		ShowParticles ();
		Destroy (this.gameObject);
		if (hasBall) {
			DropBall (fallingBallIndex);
		}
		levelManager.IncreaseBackgroundAlpha ();
	}

	void DropBall (int index)
	{
		GameObject ball = 
			Instantiate (fallingObject, this.gameObject.transform.position, Quaternion.identity) as GameObject;
		ball.GetComponent <SpriteRenderer> ().sprite = ball.GetComponent <FallingObjects> ().SetTheBall (index);
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
