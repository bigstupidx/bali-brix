using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Net;
using UnityEngine.Advertisements;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
	public static int ballCounts = 3;
	public static int currentScore = 0;
	public static int playCounts = 0;
	public static int coins = 550;
	public AudioClip timeoutAlert, bonusTime, bonusBall;
	public Sprite[] soundIcons;
	public int fallingObjects = 0;
	public int fallingObjectIndex = 1;
	public int totalBricks = 0;
	public float timeLeft = 0f;
	public bool secondChance = true;
	public CanvasManager canvasManager;

	private GameObject timer;
	private GameObject background;
	private GameObject score, ballsNo, levelCompleteScore, level, sound;
	private GameObject touchArea;

	private string minsAndSecs = "0:0";
	private bool alert = true;
	private int colorFactor = 20;
	private int currentLevel;

	void Start ()
	{
		currentLevel = SceneManager.GetActiveScene ().buildIndex - 2;
		FindThemAll ();
		ballsNo.GetComponent <Text> ().text = ballCounts.ToString ();
		totalBricks = Brick.brickCounts;
		timeLeft = totalBricks * 2.1f;
		playCounts++;
		if (playCounts > 4) {
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

	private void FindThemAll ()
	{
		background = GameObject.Find ("Background");
		timer = GameObject.Find ("Timer");
		score = GameObject.Find ("Score");
		ballsNo = GameObject.Find ("Balls No");
		level = GameObject.Find ("Level");
		levelCompleteScore = GameObject.Find ("Level Complete Score");
		touchArea = GameObject.Find ("Touch Area");
	}

	void Update ()
	{
		if (Ball.hasStarted && timer) {
			touchArea.GetComponent <Text> ().text = "Touch Area";
			UpdateTimer ();
		} 
		if (timeLeft <= 0f) {
			EvalDamage (totalBricks - Brick.brickCounts);
			timeLeft = 0.1f; // just to prevent running EvalDamage (the root cause of Flickering)
			alert = false;
		}
		if (timeLeft < 7f) {
			Blink (timer);
			if (alert) {
				timer.GetComponent <Text> ().fontStyle = FontStyle.Bold;
				StartCoroutine (Alert ());
				alert = false;
			}
		}
	}

	public void CheckPowerUps ()
	{
		print (PowerUps.powerUps.Count);
		if (PowerUps.powerUps.Count > 0)
			foreach (string s in PowerUps.powerUps) {
				print (s + "\n");
				InitiatePowerUp (s);
			}
		PowerUps.powerUps = new List<string>{ };
		print (PowerUps.powerUps.Count);
	}

	private void InitiatePowerUp (string name)
	{
		print ("initiating the powerup!");
	}


	private void SecondChance ()
	{
	}

	public void NoThanks ()
	{
		canvasManager.toggleCanvas (canvasManager.lostContinue);
		canvasManager.toggleCanvas (canvasManager.lost);
	}

	public void Pay300Coins ()
	{
		timeLeft = 61f;
		ballCounts = 2;
		// ToDo:deduct 200 coins
		alert = true;
		ballsNo.GetComponent <Text> ().text = ballCounts.ToString ();
		UpdateTimer ();	
		canvasManager.toggleCanvas (canvasManager.lostContinue);
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
			//
			// YOUR CODE TO REWARD THE GAMER
			// Give coins etc.
			timeLeft = 31f;
			ballCounts = 1;
			alert = true;
			ballsNo.GetComponent <Text> ().text = ballCounts.ToString ();
			UpdateTimer ();
			canvasManager.toggleCanvas (canvasManager.lostContinue);
			break;
		case ShowResult.Skipped:
			break;
		case ShowResult.Failed:
			break;
		}
	}

	private void Blink (GameObject go)
	{
		if (go) {
			go.GetComponent <Text> ().color = (Mathf.Floor (timeLeft % 2) == 0) ?
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
					canvasManager.toggleCanvas (canvasManager.lostContinue);
					//ShowRewardedAd ();
					secondChance = false;
				} else {
					//LoadLevel ("Loose");	
					canvasManager.toggleCanvas (canvasManager.lost);

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
		if (currentLevel >= LevelSelection.highestLevel) {
			LevelSelection.highestLevel++;
			PlayerPrefs.SetInt ("Highest Level", LevelSelection.highestLevel);	
		}
	}

	public void ShowLevelComplete (float damage)
	{
		int stars = 0;
		CanvasManager.canvasActive = true;
		canvasManager.toggleCanvas (canvasManager.levelComplete);
		DeleteAllMovingObjects ();
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
		PlayerPrefs.SetInt ("Level" + currentLevel, stars);

		if (!canvasManager.starsPlayed) {
			canvasManager.starsPlayed = true;
			StartCoroutine (canvasManager.PlayStarPopSound (stars));
		}
	}

	private void DeleteAllMovingObjects ()
	{
		//ToDo: clear the scene from falling objects and cloned balls
		GameObject[] fallingObjects = GameObject.FindGameObjectsWithTag ("Falling Objects");
		GameObject[] balls = GameObject.FindGameObjectsWithTag ("Ball");
		if (fallingObjects != null) {
			foreach (GameObject g in fallingObjects)
				Destroy (g);
		}
		if (balls != null) {
			foreach (GameObject g in balls)
				Destroy (g);
		}
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
		sound = GameObject.Find ("Sound");
		sound.GetComponent <Image> ().sprite = (AudioListener.pause) ? soundIcons [1] : soundIcons [0];
	}

	public void Quit ()
	{
		Application.Quit ();
	}
}
