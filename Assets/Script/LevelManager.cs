using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
	public static int ballCounts = 3;
	public static int score = 0;
	public AudioClip timeoutAlert;

	private GameObject background;
	private GameObject levelComplete;
	private GameObject stars;
	private float timeLeft = 65f;
	private GameObject timer;
	private int totalBricks;
	private string minsAndSecs = "0:0";
	public Sprite[] levelCompleteStars;
	private bool alert = true;

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
		//InvokeRepeating ("Alert", timeLeft - 7f, 1f); // play alert sound 7 sec before times up
	}

	void Update ()
	{
		if (Ball.hasStarted && timer) {
			UpdateTimer ();
		}
		if (timeLeft <= 0) {
			EvalDamage (totalBricks - Brick.brickCounts);
		}
		if (timeLeft < 7) {
			if (alert) {
				StartCoroutine (Alert ());
				alert = false;
			}
		}
	}

	IEnumerator Alert ()
	{
		print ("ALERT!!!");
		AudioSource.PlayClipAtPoint (timeoutAlert, this.transform.position);	
		yield return new WaitForSeconds (1f);
		alert = true;
	}

	private void UpdateTimer ()
	{
		timeLeft -= Time.deltaTime;
		if (timeLeft < 0)
			timeLeft = 0;
		minsAndSecs = Mathf.Floor (timeLeft / 60) + " : " + Mathf.Floor (timeLeft % 60);
		timer.GetComponent <Text> ().text = minsAndSecs;
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
		Ball.hasStarted = false;
		alert = false;
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
			addTimeBonusScore ((int)timeLeft);
			index = 2;
		}
		levelComplete.GetComponent <CanvasGroup> ().alpha = 1;
		if (levelCompleteStars [index] != null)
			stars.GetComponent <Image> ().sprite = levelCompleteStars [index];
		else
			Debug.LogError ("Sprite is missing!");
	}

	public void addTimeBonusScore (int time)
	{
		print ("remainder add: " + time);
		for (int i = time; i > 0; i--) {
			minsAndSecs = Mathf.Floor (i / 60) + " : " + Mathf.Floor (i % 60);
			timer.GetComponent <Text> ().text = minsAndSecs;
			score += 10;
			//yield return new WaitForSeconds (1f);
		}
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
