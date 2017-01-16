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
	private float timeLeft = 50f;
	private GameObject timer;
	private int totalBricks;

	void Start ()
	{
		background = GameObject.Find ("Background");
		timer = GameObject.Find ("Timer");
		levelComplete = GameObject.Find ("Level Complete");
		levelComplete.GetComponent <CanvasGroup> ().alpha = 0;
		totalBricks = Brick.brickCounts;
	}

	void Update ()
	{
		print ("--total bricks: " + totalBricks);
		print ("--available bricks: " + Brick.brickCounts);
		int delta = totalBricks - Brick.brickCounts;
		print ("--delta: " + delta);
		if (Ball.hasStarted) {
			timeLeft -= Time.deltaTime;
			string minsAndSecs = Mathf.Floor (timeLeft / 60) + " : " + Mathf.Floor (timeLeft % 60);
			timer.GetComponent <Text> ().text = minsAndSecs;
		}
		if (timeLeft <= 0) {
			TotalDamage (totalBricks - Brick.brickCounts);
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

	public void TotalDamage (int destroyedBricks)
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
		int stars = 0;
		if (damage < 0.7) {
			stars = 1;
		} else if (damage >= 0.7 && damage < 1) {
			stars = 2;
		} else {
			stars = 3;
		}
		levelComplete.GetComponent <CanvasGroup> ().alpha = 1;
		print (stars);
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
