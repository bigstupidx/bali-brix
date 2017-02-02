using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{

	public Sprite[] soundIcons;
	private GameObject sound;

	void Start ()
	{
		sound = GameObject.Find ("Sound Settings");
	}

	public void LoadLevel (string name)
	{
		SceneManager.LoadScene (name);
	}

	public void Quit ()
	{
		Application.Quit ();
	}

	public void toggleSound ()
	{
		AudioListener.pause = (AudioListener.pause) ? false : true;
		AudioListener.volume = (AudioListener.pause) ? 0 : 2;
		sound.GetComponent <Image> ().sprite = (AudioListener.pause) ? soundIcons [1] : soundIcons [0];
		print ((AudioListener.pause) ? "paused" : "playing...");

	}
}
