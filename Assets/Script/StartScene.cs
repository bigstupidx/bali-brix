using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
