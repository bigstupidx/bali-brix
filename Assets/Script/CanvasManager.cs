using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
	public static bool canvasActive = false;
	public static bool powerUpOffered = false;

	public Canvas pause;
	public Canvas iAP;
	public Canvas powerUps;
	public Canvas levelComplete;
	public Canvas lost, lostContinue;
	public AudioClip popStar;
	public bool starsPlayed = false;

	private GameObject starLeft, starMiddle, starRight;
	private GameObject coinsNo;

	// Use this for initialization
	void Start ()
	{
		initCanvases ();
		if (!powerUpOffered) {
			toggleCanvas (powerUps);
			powerUpOffered = true;
		}
	}

	private void initCanvases ()
	{
		pause.enabled = false;
		iAP.enabled = false;
		powerUps.enabled = false;
		levelComplete.enabled = false;
		lost.enabled = false;
		lostContinue.enabled = false;
		canvasActive = false;
		starLeft = GameObject.Find ("Star Left");
		starMiddle = GameObject.Find ("Star Middle");
		starRight = GameObject.Find ("Star Right");
		TurnOffStars ();
		UpdateCoins ();
	}

	public void UpdateCoins ()
	{
		coinsNo = GameObject.Find ("Coins No");
		coinsNo.GetComponent <Text> ().text = LevelManager.coins.ToString ();
	}

	private void TurnOffStars ()
	{
		starLeft.GetComponent <Image> ().color = new Color (255, 255, 255, 0);
		starMiddle.GetComponent <Image> ().color = new Color (255, 255, 255, 0);
		starRight.GetComponent <Image> ().color = new Color (255, 255, 255, 0);
	}

	/*public void showCanvas (Canvas c)
	{
		c.enabled = true;
		canvasActive = true;
	}

	public void hideCanvas (Canvas c)
	{
		c.enabled = false;
		canvasActive = false;
	}
*/
	public void toggleCanvas (Canvas c)
	{
		c.enabled = (c.enabled == true) ? false : true;
		canvasActive = (c.transform.name == "Canvas - IAP") ? true : c.enabled;
		UpdateCoins ();
	}

	public void ShowPause ()
	{
		toggleCanvas (pause);
		Time.timeScale = 0;
	}

	public void Continue ()
	{
		toggleCanvas (pause);
		Time.timeScale = 1;
	}

	public IEnumerator PlayStarPopSound (int stars)
	{
		print ("are we playing");
		AudioSource.PlayClipAtPoint (popStar, this.transform.position);	
		yield return new WaitForSeconds (0.4f);
		if (starLeft) {
			starLeft.GetComponent <Image> ().color += new Color (0, 0, 0, 255);
			yield return new WaitForSeconds (0.4f);
		} 

		if (stars == 2) {
			AudioSource.PlayClipAtPoint (popStar, this.transform.position);
			if (starMiddle)
				starMiddle.GetComponent <Image> ().color += new Color (0, 0, 0, 255);
			yield return new WaitForSeconds (0.8f);
		} else if (stars == 3) {
			AudioSource.PlayClipAtPoint (popStar, this.transform.position);
			if (starMiddle)
				starMiddle.GetComponent <Image> ().color += new Color (0, 0, 0, 55);
			yield return new WaitForSeconds (0.4f);
			AudioSource.PlayClipAtPoint (popStar, this.transform.position);
			if (starRight)
				starRight.GetComponent <Image> ().color += new Color (0, 0, 0, 155);
		}
		starsPlayed = true;
	}
}
