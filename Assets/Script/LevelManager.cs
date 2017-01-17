using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
	public static int ballCounts = 3;
	public static int score = 0;
	private GameObject background;
	private GameObject levelComplete;
	private GameObject stars;
	private float timeLeft = 95f;
	private GameObject timer;
	private int totalBricks;
	private string minsAndSecs = "0:0";
	public Sprite[] levelCompleteStars;


	void Start ()
	{
		background = GameObject.Find ("Background");
		timer = GameObject.Find ("Timer");
		levelComplete = GameObject.Find ("Level Complete");
		if (levelComplete) {
			levelComplete.GetComponent <CanvasGroup> ().alpha = 0;
			stars = GameObject.Find ("Stars");
		}
		totalBricks = Brick.brickCounts;
	}

	void Update ()
	{
		print ("--total bricks: " + totalBricks);
		print ("--available bricks: " + Brick.brickCounts);
		int delta = totalBricks - Brick.brickCounts;
		print ("--delta: " + delta);
		if (Ball.hasStarted && timer) {
			timeLeft -= Time.deltaTime;
			if (timeLeft < 0)
				timeLeft = 0;
			minsAndSecs = Mathf.Floor (timeLeft / 60) + " : " + Mathf.Floor (timeLeft % 60);
			timer.GetComponent <Text> ().text = minsAndSecs;
		}
		if (timeLeft <= 0) {
			EvalDamage (totalBricks - Brick.brickCounts);
			Ball.hasStarted = false;

			//LoadLevel ("Loose");
		}
	}

	public void LoadLevel (string name)
	{
		Debug.Log ("load level requested:" + name);
		// Need to reset the brick counts otherwise the leftover from last game 
		// will be added to the new game
		Brick.brickCounts = 0;
		//Ball.ballCounts = 3;
		SceneManager.LoadScene (name);
	}

	public void Reload ()
	{
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}

	public void EvalDamage (int destroyedBricks)
	{
		float damage = (float)destroyedBricks / (float)totalBricks;
		if (damage < 0.6) {
			print ("LOST!!!"); 
			LoadLevel ("Loose");
		} else {
			LevelComplete (damage);
			Invoke ("LoadNextLevel", 5f);
		}
	}

	public void LoadNextLevel ()
	{
		// Need to reset the brick counts otherwise the leftover from last game 
		// will be added to the new game
		Brick.brickCounts = 0;
		Ball.hasStarted = false; 
		int sceneIndex = SceneManager.GetActiveScene ().buildIndex + 1;
		PlayerPrefs.SetInt ("HighestLevel", sceneIndex);
		SceneManager.LoadScene (sceneIndex);
	}

	public void LevelComplete (float damage)
	{
		int index;
		if (damage < 0.7) {
			index = 0;
		} else if (damage >= 0.7 && damage < 1) {
			index = 1;
		} else {
			index = 2;
		}
		levelComplete.GetComponent <CanvasGroup> ().alpha = 1;

		if (levelCompleteStars [index] != null)
			stars.GetComponent <Image> ().sprite = levelCompleteStars [index];
		else
			Debug.LogError ("Sprite is missing!");
	}

	public void IncreaseBackgroundAlpha ()
	{
		background.GetComponent <SpriteRenderer> ().color += new Color (0f, 0f, 0f, 0.007f);
	}

	public void Quit ()
	{
		Debug.Log ("quit!");
		Application.Quit ();
	}
}
