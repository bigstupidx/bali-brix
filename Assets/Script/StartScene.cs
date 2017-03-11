using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
	private GameObject dailyRewardCanvas;
	private GameObject helpCanvas;

	public static bool rewarded = false;

	void Start ()
	{
		dailyRewardCanvas = GameObject.Find ("Canvas - Daily Rewards");
		helpCanvas = GameObject.Find ("Canvas - Help");

		if (dailyRewardCanvas) {
			dailyRewardCanvas.SetActive (false);
		}
		helpCanvas.SetActive (false);
	}

	public void DailyRewards ()
	{
		if (dailyRewardCanvas && !rewarded) {
			dailyRewardCanvas.SetActive (true);
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

	// Update is called once per frame
	void Update ()
	{
		
	}

	/*public void Play ()
	{
		Brick.brickCounts = 0;
		Ball.hasStarted = false;

		//Ball.ballCounts = 3;
		SceneManager.LoadScene ("Level Selection");
	}

	public void LoadLevel (string name)
	{
		SceneManager.LoadScene (name);
	}*/

	public void RateUs ()
	{
		Application.OpenURL ("https://play.google.com/store/apps/details?id=com.soolan.BaliBrix");
	}

	public void ShowHelp ()
	{
		helpCanvas.SetActive (true);
	}

	public void HideHelp ()
	{
		helpCanvas.SetActive (false);
	}

	public void Quit ()
	{
		Debug.Log ("quit!");
		Application.Quit ();
	}
}
