using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Net;
using UnityEngine.Advertisements;

public class LevelManager : MonoBehaviour
{
	public static int ballCounts = 3;
	public static int currentScore = 0;
	public static int playCounts = 0;
	public AudioClip timeoutAlert, popStar, bonusTime, bonusBall;
	public Sprite[] levelCompleteStars, soundIcons;
	public int fallingObjects = 0;
	public int fallingBallIndex = 1;
	public int totalBricks = 0;
	public float timeLeft = 0f;
	public bool secondChance = true;

	private GameObject timer;
	private GameObject background;
	private GameObject levelCompleteCanvas;
	private GameObject iAPCanvas;
	private GameObject starLeft, starMiddle, starRight;
	private GameObject score, ballsNo, levelCompleteScore, level;

	private string minsAndSecs = "0:0";

	private bool alert = true;
	private bool starsPlayed = false;
	private int colorFactor = 20;

	void Start ()
	{
		FindThemAll ();
		SetUILevelName (SceneManager.GetActiveScene ().name);
		if (levelCompleteCanvas) {
			levelCompleteCanvas.GetComponent <CanvasGroup> ().alpha = 0;
			levelCompleteCanvas.GetComponent <CanvasGroup> ().interactable = false;
			TurnOffStars ();
		}
		if (iAPCanvas) {
			iAPCanvas.GetComponent <CanvasGroup> ().alpha = 0;
			iAPCanvas.GetComponent <CanvasGroup> ().interactable = false;
		}
		ballsNo.GetComponent <Text> ().text = ballCounts.ToString ();
		totalBricks = Brick.brickCounts;
		timeLeft = totalBricks * 2.1f;
		playCounts++;
		if (playCounts > 3) {
			playCounts = 0;
			ShowAd ();
		}
	}

	public void ShowAd ()
	{
		if (Advertisement.IsReady ()) {
			Advertisement.Show ();
		}
	}

	private void SetUILevelName (string name)
	{
		level.GetComponent<Text> ().text = name.Substring (0, 5) + " " + name.Substring (5, 2);
	}

	private void FindThemAll ()
	{
		background = GameObject.Find ("Background");
		timer = GameObject.Find ("Timer");
		score = GameObject.Find ("Score");
		//sound = GameObject.Find ("Sound");
		ballsNo = GameObject.Find ("Balls No");
		level = GameObject.Find ("Level");
		levelCompleteCanvas = GameObject.Find ("Canvas - Level Complete");
		iAPCanvas = GameObject.Find ("Canvas - IAP");
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

	private void SecondChance ()
	{
		// 3 sec to make a descision + 30 sec to play
		/*
		if () {
			
		}
		*/
	}

	public void ShowRewardedAd ()
	{
		if (Advertisement.IsReady ("rewardedVideo")) {
			var options = new ShowOptions { resultCallback = HandleShowResult };
			Advertisement.Show ("rewardedVideo", options);
		}
	}

	private void HandleShowResult (ShowResult result)
	{
		Ball.hasStarted = false;
		switch (result) {
		case ShowResult.Finished:
			Debug.Log ("The ad was successfully shown.");
			//
			// YOUR CODE TO REWARD THE GAMER
			// Give coins etc.
			timeLeft = 31f;
			ballCounts++;
			alert = true;
			ballsNo.GetComponent <Text> ().text = ballCounts.ToString ();
			UpdateTimer ();
			break;
		case ShowResult.Skipped:
			Debug.Log ("The ad was skipped before reaching the end.");
			break;
		case ShowResult.Failed:
			Debug.LogError ("The ad failed to be shown.");
			break;
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
		// Need to reset the brick counts otherwise the leftover from last game 
		// will be added to the new game
		Brick.brickCounts = 0;
		Ball.hasStarted = false;
		SceneManager.LoadScene (name);
	}

	public void Reload ()
	{
		Ball.hasStarted = false;
		Brick.brickCounts = 0;
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}

	public void EvalDamage (int destroyedBricks, bool cleared = false)
	{
		Ball.hasStarted = false;
		alert = false;
		if (cleared) {
			ShowLevelComplete (1);
			UnlockNextLevel ();
			Invoke ("LoadNextLevel", timeLeft * 0.1f + 5f);
		} else {
			float damage = (float)destroyedBricks / (float)totalBricks;
			if (damage < 0.6) {
				if (secondChance) {
					ShowRewardedAd ();
					secondChance = false;
				} else {
					LoadLevel ("Loose");	
				}
			} else {
				ShowLevelComplete (damage);
				UnlockNextLevel ();
				Invoke ("LoadNextLevel", 3.5f);
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
		//PlayerPrefs.SetInt ("HighestLevel", sceneIndex);
		SceneManager.LoadScene (sceneIndex);
	}

	private void UnlockNextLevel ()
	{
		// Remember! Level 01 build index is 3.
		int currentLevel = SceneManager.GetActiveScene ().buildIndex - 2;
		if (currentLevel >= LevelSelection.highestLevel) {
			LevelSelection.highestLevel++;
			PlayerPrefs.SetInt ("Highest Level", LevelSelection.highestLevel);	
		}
	}

	public void ShowLevelComplete (float damage)
	{
		int stars = 0;
		levelCompleteCanvas.GetComponent <CanvasGroup> ().alpha = 1;
		levelCompleteCanvas.GetComponent <CanvasGroup> ().interactable = true;
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
		levelCompleteCanvas.GetComponent <CanvasGroup> ().alpha = 
			(levelCompleteCanvas.GetComponent <CanvasGroup> ().alpha == 1) ? 0 : 1;
		levelCompleteCanvas.GetComponent <CanvasGroup> ().interactable = 
			(levelCompleteCanvas.GetComponent <CanvasGroup> ().interactable) ? false : true;
	}

	IEnumerator PlayStarPopSound (int stars)
	{
		yield return new WaitForSeconds (0.4f);
		AudioSource.PlayClipAtPoint (popStar, this.transform.position);	
		if (starLeft) {
			starLeft.GetComponent <Image> ().color += new Color (0, 0, 0, 255);
			yield return new WaitForSeconds (0.4f);
		} 

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
		//sound.GetComponent <Image> ().sprite = (AudioListener.pause) ? soundIcons [1] : soundIcons [0];
	}

	public void ShowIAP ()
	{
		print ("show IAP");
		iAPCanvas.GetComponent <CanvasGroup> ().alpha = 1;
		iAPCanvas.GetComponent <CanvasGroup> ().interactable = true;
	}

	public void Quit ()
	{
		Debug.Log ("quit!");
		Application.Quit ();
	}
}
