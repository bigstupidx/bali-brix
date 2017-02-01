using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Net;

public class LevelManager : MonoBehaviour
{
	public static int ballCounts = 3;
	public static int currentScore = 0;
	public AudioClip timeoutAlert, popStar, bonusTime, bonusBall;
	public Sprite[] levelCompleteStars;
	public int fallingObjects = 0;

	private GameObject timer;
	private GameObject background;
	private GameObject levelCompleteCanvas;
	private GameObject starLeft, starMiddle, starRight;
	private GameObject score, ballsNo, levelCompleteScore, level;

	private string minsAndSecs = "0:0";
	private float timeLeft;
	private bool alert = true;
	private bool starsPlayed = false;
	private int colorFactor = 20;
	private int totalBricks;

	void Start ()
	{
		FindThemAll ();
		string levelName = SceneManager.GetActiveScene ().name;
		level.GetComponent <Text> ().text = levelName.Substring (0, 5) + " " + levelName.Substring (5, 2);
		if (levelCompleteCanvas) {
			//ToggleUI ();
			levelCompleteCanvas.GetComponent <CanvasGroup> ().alpha = 0;
			levelCompleteCanvas.GetComponent <CanvasGroup> ().interactable = false;

			TurnOffStars ();
		}
		totalBricks = Brick.brickCounts;
		timeLeft = totalBricks * 2.65f;
	}

	private void FindThemAll ()
	{
		background = GameObject.Find ("Background");
		timer = GameObject.Find ("Timer");
		score = GameObject.Find ("Score");
		ballsNo = GameObject.Find ("Balls No");
		level = GameObject.Find ("Level");
		levelCompleteCanvas = GameObject.Find ("Canvas - Level Complete");
		levelCompleteScore = GameObject.Find ("Level Complete Score");
		starLeft = GameObject.Find ("Star Left");
		starMiddle = GameObject.Find ("Star Middle");
		starRight = GameObject.Find ("Star Right");
	}

	private void TurnOffStars ()
	{
		starLeft.GetComponent <Image> ().color = new Color (255, 255, 255, 0);
		starMiddle.GetComponent <Image> ().color = new Color (255, 255, 255, 0);
		starRight.GetComponent <Image> ().color = new Color (255, 255, 255, 0);
	}

	void Update ()
	{
		if (Ball.hasStarted && timer) {
			UpdateTimer ();
		}
		if (timeLeft <= 0f) {
			EvalDamage (totalBricks - Brick.brickCounts);
			timeLeft = 0.1f; // just to prevent running EvalDamage (the root cause of Flickering)
			alert = false;
		}
		if (timeLeft < 7f) {
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
		if (timer) {
			timer.GetComponent <Text> ().color = (Mathf.Floor (timeLeft % 2) == 0) ?
				Color.Lerp (Color.yellow, Color.red, 1f) :
				Color.Lerp (Color.red, Color.yellow, 1f);
		}
	}

	IEnumerator Alert ()
	{
		colorFactor += 20;
		AudioSource.PlayClipAtPoint (timeoutAlert, this.transform.position);
		yield return new WaitForSeconds (1f);
		alert = (timeLeft > 0.1f) ? true : false;
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
		print ("Load scene " + name + " requested");
		// Need to reset the brick counts otherwise the leftover from last game 
		// will be added to the new game
		Brick.brickCounts = 0;
		Ball.hasStarted = false;
		SceneManager.LoadScene (name);
	}

	public void Reload ()
	{
		print ("realoding the current scene: " + SceneManager.GetActiveScene ().name);
		Ball.hasStarted = false;
		Brick.brickCounts = 0;
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}

	public void EvalDamage (int destroyedBricks, bool cleared = false)
	{
		Ball.hasStarted = false;
		alert = false;
		levelCompleteCanvas.GetComponent <CanvasGroup> ().alpha = 1;
		levelCompleteCanvas.GetComponent <CanvasGroup> ().interactable = true;
		if (cleared) {
			LevelComplete (1);
			UnlockNextLevel ();
			Invoke ("LoadNextLevel", timeLeft * 0.1f + 5f);
		} else {
			float damage = (float)destroyedBricks / (float)totalBricks;
			if (damage < 0.6) {
				LoadLevel ("Loose");
			} else {
				print ("highest level:" + LevelSelection.highestLevel);
				//ToggleUI ();  // show level complete window + its elements
				LevelComplete (damage);
				UnlockNextLevel ();
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

	private void UnlockNextLevel ()
	{
		LevelSelection.highestLevel++;
		PlayerPrefs.SetInt ("Highest Level", LevelSelection.highestLevel);
	}

	public void LevelComplete (float damage)
	{
		int stars = 0;
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
		if (!starsPlayed) {
			starsPlayed = true;
			StartCoroutine (PlayStarPopSound (stars));
		}
	}

	private void ToggleUI ()
	{
		print ("alpha: " + levelCompleteCanvas.GetComponent <CanvasGroup> ().alpha
		+ " interactable: " + levelCompleteCanvas.GetComponent <CanvasGroup> ().interactable);
		//if (levelComplete) {
		levelCompleteCanvas.GetComponent <CanvasGroup> ().alpha = 
			(levelCompleteCanvas.GetComponent <CanvasGroup> ().alpha == 1) ? 0 : 1;
		levelCompleteCanvas.GetComponent <CanvasGroup> ().interactable = 
			(levelCompleteCanvas.GetComponent <CanvasGroup> ().interactable) ? false : true;
		print ("alpha: " + levelCompleteCanvas.GetComponent <CanvasGroup> ().alpha
		+ " interactable: " + levelCompleteCanvas.GetComponent <CanvasGroup> ().interactable);
		//}
	}

	IEnumerator PlayStarPopSound (int stars)
	{
		yield return new WaitForSeconds (timeLeft * 0.1f);
		AudioSource.PlayClipAtPoint (popStar, this.transform.position);	
		if (starLeft)
			starLeft.GetComponent <Image> ().color += new Color (0, 0, 0, 255);
		yield return new WaitForSeconds (0.4f);

		if (stars == 2) {
			AudioSource.PlayClipAtPoint (popStar, this.transform.position);
			if (starMiddle)
				starMiddle.GetComponent <Image> ().color += new Color (0, 0, 0, 255);
			yield return new WaitForSeconds (0.4f);
		} else if (stars == 3) {
			AudioSource.PlayClipAtPoint (popStar, this.transform.position);
			if (starMiddle)
				starMiddle.GetComponent <Image> ().color += new Color (0, 0, 0, 255);
			yield return new WaitForSeconds (0.4f);
			AudioSource.PlayClipAtPoint (popStar, this.transform.position);
			if (starRight)
				starRight.GetComponent <Image> ().color += new Color (0, 0, 0, 255);
		}
		starsPlayed = true;
	}


	public void fetchLevelPrize ()
	{

	}

	IEnumerator addTimeBonusScore (int time)
	{
		for (int i = time; i > 0; i--) {
			AudioSource.PlayClipAtPoint (bonusTime, this.transform.position);
			minsAndSecs = Mathf.Floor (i / 60) + " : " + Mathf.Floor (i % 60 - 1);
			timer.GetComponent <Text> ().text = minsAndSecs;
			currentScore += 10;
			score.GetComponent <Text> ().text = LevelManager.currentScore.ToString ();
			levelCompleteScore.GetComponent <Text> ().text = LevelManager.currentScore.ToString ();
			if ((currentScore / (1000 * Ball.bonusFactor)) >= 1) {
				AddBonusBall (ballsNo);
			}
			yield return new WaitForSeconds (0.1f);
		}
	}

	public void AddBonusBall (GameObject balls)
	{
		Ball.bonusFactor++;
		ballCounts++;
		AudioSource.PlayClipAtPoint (bonusBall, this.transform.position);
		balls.GetComponent <Text> ().text = ballCounts.ToString ();
	}

	public void IncreaseBackgroundAlpha ()
	{
		float alpha = (float)1 / (totalBricks + Brick.brickCounts);
		background.GetComponent <SpriteRenderer> ().color += new Color (0f, 0f, 0f, alpha);
	}

	public void toggleSounds ()
	{
		AudioListener.pause = (AudioListener.pause) ? false : true;
		AudioListener.volume = (AudioListener.pause) ? 0 : 2;
	}

	public void Quit ()
	{
		Debug.Log ("quit!");
		print ("QUIT");
		Application.Quit ();
	}
}
