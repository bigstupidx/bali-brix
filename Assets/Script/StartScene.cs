using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEditor;
using UnityEngine.UI;

public class StartScene : MonoBehaviour
{
	public Canvas helpCanvas;
	public Canvas settingsCanvas;
	public Canvas dailyRewardCanvas;
	public Sprite[] acceptRejectIcons;
	public static bool nonStopPlay = false;

	private LevelManager levelManager;
	private GameObject today, tomorrow;
	private int currentRewardDay = 1;
	private int currentRewardAmount = 100;

	void Start ()
	{		 
		InitGameObjects ();
		SetPlayerPrefs ();
		//HandleDailyReward (CheckDate ());
	}

	private void InitGameObjects ()
	{
		helpCanvas.enabled = false;
		settingsCanvas.enabled = false;
		dailyRewardCanvas.enabled = false;
		today = GameObject.Find ("Text - Day");
		tomorrow = GameObject.Find ("Text - Tomorrow");
	}

	private void SetPlayerPrefs ()
	{
		if (!PlayerPrefs.HasKey ("Last Visit")) {
			// Happy Birthday Sohail
			PlayerPrefs.SetString (
				"Last Visit", 
				Convert.ToDateTime ("3/21/1975 5:00:00 AM").ToBinary ().ToString ()
			);
		}
		if (!PlayerPrefs.HasKey ("CurrentRewardDay")) {
			PlayerPrefs.SetInt ("CurrentRewardDay", 1);
		}
		if (!PlayerPrefs.HasKey ("Rewarded")) {
			PlayerPrefs.SetInt ("Rewarded", 0);
		}
	}

	public void HandleDailyReward ()
	{
		TimeSpan timeSpan = CheckDate ();
		if (timeSpan.Hours <= 24 && PlayerPrefs.GetInt ("Rewarded") == 0) {
			SetReward ();
			PlayerPrefs.SetInt ("Rewarded", 1);
		} else if (timeSpan.Hours > 24 && timeSpan.Hours < 48) {
			SetReward ();
		} else if (timeSpan.Hours >= 48) {
			ResetRewards ();
		} else {
			SceneManager.LoadScene ("Level Selection");
		}
	}

	private void SetReward ()
	{
		currentRewardDay = PlayerPrefs.GetInt ("CurrentRewardDay");
		currentRewardAmount += currentRewardDay * 10;
		today.GetComponent <Text> ().text = 
				"Day " + currentRewardDay.ToString () + "\n +" + currentRewardAmount.ToString ();
		tomorrow.GetComponent <Text> ().text = 
				"tomorrow\n +" + (currentRewardAmount + 10).ToString () + "\n coins";
		dailyRewardCanvas.enabled = true;
	}

	private void ResetRewards ()
	{
		PlayerPrefs.SetInt ("CurrentRewardDay", 1);
		SetReward ();
	}

	public TimeSpan CheckDate ()
	{
		//Store the current time when it starts
		DateTime currentDate = System.DateTime.Now;

		//Grab the old time from the player prefs as a long
		long temp = Convert.ToInt64 (PlayerPrefs.GetString ("Last Visit"));

		//Convert the old time from binary to a DataTime variable
		DateTime oldDate = DateTime.FromBinary (temp);
		print ("oldDate: " + oldDate);

		//Use the Subtract method and store the result as a timespan variable
		TimeSpan difference = currentDate.Subtract (oldDate);
		return difference;
	}

	public void RewardClaimed ()
	{
		LevelManager.coins += currentRewardAmount;
		PlayerPrefs.SetInt ("CurrentRewardDay", currentRewardDay + 1);
		PlayerPrefs.SetInt ("Rewarded", 1);
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

	public void ToggleNonStopPlay ()
	{
		nonStopPlay = (nonStopPlay) ? false : true;
		//sound = GameObject.Find ("Sound");
		//sound.GetComponent <Image> ().sprite = (AudioListener.pause) ? soundIcons [1] : soundIcons [0];
	}

	public void Quit ()
	{
		levelManager.Quit ();
	}
}
