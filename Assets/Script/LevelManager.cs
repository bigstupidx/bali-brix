using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Net;

public class LevelManager : MonoBehaviour
{
	public static int ballCounts = 3;
	public static int currentScore = 0;
	public AudioClip timeoutAlert;
	public AudioClip popStar;

	private GameObject background;
	private GameObject levelComplete;
	private GameObject starLeft, starMiddle, starRight;
	private GameObject score;

	private float timeLeft = 85f;
	private GameObject timer;
	private int totalBricks;
	private string minsAndSecs = "0:0";
	public Sprite[] levelCompleteStars;
	private bool alert = true;
	private int colorFactor = 20;

	void Start ()
	{
		background = GameObject.Find ("Background");
		timer = GameObject.Find ("Timer");
		score = GameObject.Find ("Score");
		levelComplete = GameObject.Find ("Level Complete");
		if (levelComplete) {
			levelComplete.GetComponent <CanvasGroup> ().alpha = 0;
			starLeft = GameObject.Find ("Star Left");
			starLeft.GetComponent <Image> ().color = new Color (255, 255, 255, 0);
			starMiddle = GameObject.Find ("Star Middle");
			starMiddle.GetComponent <Image> ().color = new Color (255, 255, 255, 0);
			starRight = GameObject.Find ("Star Right");
			starRight.GetComponent <Image> ().color = new Color (255, 255, 255, 0);
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
			Blink ();
			if (alert) {
				timer.GetComponent <Text> ().fontStyle = FontStyle.Bold;
				StartCoroutine (Alert ());
				alert = false;
			}
		}
	}

	private void Blink ()
	{
		timer.GetComponent <Text> ().color = (Mathf.Floor (timeLeft % 2) == 0) ?
			Color.Lerp (Color.yellow, Color.red, 1f) :
			Color.Lerp (Color.red, Color.yellow, 1f);
	}

	IEnumerator Alert ()
	{
		print ("ALERT!!!");
		colorFactor += 20;
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

	public void EvalDamage (int destroyedBricks, bool cleared = false)
	{
		Ball.hasStarted = false;
		alert = false;
		if (cleared) {
			LevelComplete (1);
			Invoke ("LoadNextLevel", 5f);
		} else {
			float damage = (float)destroyedBricks / (float)totalBricks;
			print ("Assess Damage:" + damage); 
			print ("Destroyed:" + (float)destroyedBricks); 
			print ("Total:" + (float)totalBricks); 
			if (damage < 0.6) {
				LoadLevel ("Loose");
			} else {
				LevelComplete (damage);
				Invoke ("LoadNextLevel", 7f);
			}
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
		int stars;
		if (damage < 0.7) {												// 1 star
			stars = 1;
		} else if (damage >= 0.7 && damage < 1) { // 2 stars
			stars = 2;
		} else { 																	// 3 stars
			StartCoroutine (addTimeBonusScore ((int)timeLeft));
			//Invoke ("LoadNextLevel", 5f);
			fetchLevelPrize ();
			stars = 3;
		}
		levelComplete.GetComponent <CanvasGroup> ().alpha = 1;
		ShowStars (stars);
	}

	private void ShowStars (int stars)
	{
		switch (stars) {
		case 3:
			print ("total damage");
			starLeft.GetComponent <Image> ().color += new Color (0, 0, 0, 255);
			starMiddle.GetComponent <Image> ().color += new Color (0, 0, 0, 255);
			starRight.GetComponent <Image> ().color += new Color (0, 0, 0, 255);
			break;

		case 2:
			starLeft.GetComponent <Image> ().color += new Color (0, 0, 0, 255);
			starMiddle.GetComponent <Image> ().color += new Color (0, 0, 0, 255);
			break;

		default:
			starLeft.GetComponent <Image> ().color += new Color (0, 0, 0, 255);
			break;
		}
	}

	private void HandleStar (GameObject star)
	{
		star.GetComponent <Image> ().color += new Color (0, 0, 0, 255);
	}

	public void fetchLevelPrize ()
	{
		
	}

	IEnumerator addTimeBonusScore (int time)
	{
		print ("remainder add: " + time);
		for (int i = time; i > 0; i--) {
			minsAndSecs = Mathf.Floor (i / 60) + " : " + Mathf.Floor (i % 60 - 1);
			timer.GetComponent <Text> ().text = minsAndSecs;
			currentScore += 10;
			score.GetComponent <Text> ().text = LevelManager.currentScore.ToString ();
			yield return new WaitForSeconds (0.01f);
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
