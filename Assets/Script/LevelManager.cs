using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
	public static int ballCounts = 3;
	public static int score = 0;
	private GameObject background;
	private float timeLeft = 300f;
	private GameObject timer;

	void Start ()
	{
		background = GameObject.Find ("Background");
		timer = GameObject.Find ("Timer");
	}

	void Update ()
	{
		if (Ball.hasStarted) {
			timeLeft -= Time.deltaTime;
			string minsAndSecs = Mathf.Round (timeLeft / 60) + " : " + Mathf.Round (timeLeft % 60);
			timer.GetComponent <Text> ().text = minsAndSecs;
		}
		if (timeLeft <= 0) {
			Ball.hasStarted = false;
			LoadLevel ("Loose");
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

	public void LoadNextLevel ()
	{
		// Need to reset the brick counts otherwise the leftover from last game 
		// will be added to the new game
		Brick.brickCounts = 0;
		Ball.hasStarted = false;
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1);
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
