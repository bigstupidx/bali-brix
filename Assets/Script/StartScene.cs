using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEditor;

public class StartScene : MonoBehaviour
{
	public Canvas helpCanvas;
	public Canvas settingsCanvas;
	public Canvas dailyRewardCanvas;
	public static bool rewarded = false;

	private LevelManager levelManager;

	void Start ()
	{
		helpCanvas.enabled = false;
		settingsCanvas.enabled = false;
		dailyRewardCanvas.enabled = false;

		SetDailyReward (CheckDate ());
	}

	private void SetDailyReward (TimeSpan timeSpan)
	{
		if (timeSpan.Hours < 48 && timeSpan.Hours > 24) {
			//ToDo: Pay the nextt reward
			HandleNextReward ();
		} else if (timeSpan.Hours >= 48) {
			//ToDo: reset the reward to the day 1
			ResetDailyRewards ();
		}
	}

	private void HandleNextReward ()
	{
		
	}

	private void ResetRewards ()
	{
		
	}

	private TimeSpan CheckDate ()
	{
		//Store the current time when it starts
		DateTime currentDate = System.DateTime.Now;

		if (!PlayerPrefs.HasKey ("Last Visit")) {
			// Happy Birthday Sohail
			PlayerPrefs.SetString (
				"Last Visit", 
				Convert.ToDateTime ("3/21/1975 5:00:00 AM").ToBinary ().ToString ()
			);
		}
		//Grab the old time from the player prefs as a long
		long temp = Convert.ToInt64 (PlayerPrefs.GetString ("Last Visit"));

		//Convert the old time from binary to a DataTime variable
		DateTime oldDate = DateTime.FromBinary (temp);
		print ("oldDate: " + oldDate);

		//Use the Subtract method and store the result as a timespan variable
		TimeSpan difference = currentDate.Subtract (oldDate);
		return difference;
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
		levelManager.Quit ();
	}
}
