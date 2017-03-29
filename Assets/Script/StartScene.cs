using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class StartScene : MonoBehaviour
{
	public Canvas helpCanvas;
	public Canvas settingsCanvas;
	public Canvas dailyRewardCanvas;
	public static bool rewarded = false;

	void Start ()
	{
		helpCanvas.enabled = false;
		settingsCanvas.enabled = false;
		dailyRewardCanvas.enabled = false;

		CheckDate ();
	}

	private void CheckDate ()
	{
		//Store the current time when it starts
		DateTime currentDate = System.DateTime.Now;

		//Grab the old time from the player prefs as a long
		long temp = Convert.ToInt64 (PlayerPrefs.GetString ("sysString"));

		//Convert the old time from binary to a DataTime variable
		DateTime oldDate = DateTime.FromBinary (temp);
		print ("oldDate: " + oldDate);

		//Use the Subtract method and store the result as a timespan variable
		TimeSpan difference = currentDate.Subtract (oldDate);
		print ("Difference: " + difference);
	}

	public void DailyRewards ()
	{
		if (!rewarded) {
			dailyRewardCanvas.enabled = true;
		} else {
			SceneManager.LoadScene ("Level Selection");
		}
	}

	public void RewardClaimed ()
	{
		//ToDo: add coins
		rewarded = true;
		SceneManager.LoadScene ("Level Selection");
	}

	public void RateUs ()
	{
		Application.OpenURL ("https://play.google.com/store/apps/details?id=com.soolan.BaliBrix");
	}

	public void Share ()
	{
		// ToDo: Share logic for android and iOS
	}

	public void toggleCanvas (Canvas c)
	{
		c.enabled = (c.enabled == true) ? false : true;
	}

	public void Quit ()
	{
		Debug.Log ("quit!");
		Application.Quit ();
	}
}
