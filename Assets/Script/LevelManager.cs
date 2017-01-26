using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Net;

public class LevelManager : MonoBehaviour
{
	public static int ballCounts = 3;
	public static int currentScore = 0;
	public AudioClip timeoutAlert, popStar, bonus;
	public int fallingObjects = 0;

	private GameObject background;
	private GameObject levelComplete;
	private GameObject starLeft, starMiddle, starRight;
	private GameObject score, levelCompleteScore;

	private float timeLeft = 85f;
	private GameObject timer;
	private int totalBricks;
	private string minsAndSecs = "0:0";
	public Sprite[] levelCompleteStars;
	private bool alert = true;
	private bool starsPlayed = false;
	private int colorFactor = 20;


	void Start ()
	{
		FindThemAll ();

		if (levelComplete) {
			ToggleUI ();
			TurnOffStars ();
		}
		totalBricks = Brick.brickCounts;
		//InvokeRepeating ("Alert", timeLeft - 7f, 1f); // play alert sound 7 sec before times up
	}

	private void FindThemAll ()
	{
		background = GameObject.Find ("Background");
		timer = GameObject.Find ("Timer");
		score = GameObject.Find ("Score");
		levelComplete = GameObject.Find ("Level Complete");
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
		Ball.hasStarted = false;

		//Ball.ballCounts = 3;
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
			LevelComplete (1);
			Invoke ("LoadNextLevel", timeLeft * 0.1f + 5f);
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
		int stars = 0;
		//levelComplete.GetComponent <CanvasGroup> ().alpha = 1;
		//levelComplete.GetComponent <CanvasGroup> ().interactable = true;
		ToggleUI ();
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
		levelComplete.GetComponent <CanvasGroup> ().alpha = 
			(levelComplete.GetComponent <CanvasGroup> ().alpha == 1) ? 0 : 1;
		levelComplete.GetComponent <CanvasGroup> ().interactable = 
			(levelComplete.GetComponent <CanvasGroup> ().interactable) ? false : true;
	}


	IEnumerator PlayStarPopSound (int stars)
	{
		yield return new WaitForSeconds (timeLeft * 0.1f);
		AudioSource.PlayClipAtPoint (popStar, this.transform.position);	
		starLeft.GetComponent <Image> ().color += new Color (0, 0, 0, 255);
		yield return new WaitForSeconds (0.4f);

		if (stars == 2) {
			AudioSource.PlayClipAtPoint (popStar, this.transform.position);
			starMiddle.GetComponent <Image> ().color += new Color (0, 0, 0, 255);
			yield return new WaitForSeconds (0.4f);
		} else {
			AudioSource.PlayClipAtPoint (popStar, this.transform.position);
			starMiddle.GetComponent <Image> ().color += new Color (0, 0, 0, 255);
			yield return new WaitForSeconds (0.4f);
			AudioSource.PlayClipAtPoint (popStar, this.transform.position);
			starRight.GetComponent <Image> ().color += new Color (0, 0, 0, 255);
		}
		starsPlayed = true;
	}


	public void fetchLevelPrize ()
	{
		
	}

	IEnumerator addTimeBonusScore (int time)
	{
		print ("remainder add: " + time);
		for (int i = time; i > 0; i--) {
			AudioSource.PlayClipAtPoint (bonus, this.transform.position);
			minsAndSecs = Mathf.Floor (i / 60) + " : " + Mathf.Floor (i % 60 - 1);
			timer.GetComponent <Text> ().text = minsAndSecs;
			currentScore += 10;
			score.GetComponent <Text> ().text = LevelManager.currentScore.ToString ();
			levelCompleteScore.GetComponent <Text> ().text = LevelManager.currentScore.ToString ();
			yield return new WaitForSeconds (0.1f);
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
