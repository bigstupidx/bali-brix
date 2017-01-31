using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{

	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void Play ()
	{
		Brick.brickCounts = 0;
		Ball.hasStarted = false;

		//Ball.ballCounts = 3;
		SceneManager.LoadScene ("Level Selection");
	}

	public void Quit ()
	{
		Debug.Log ("quit!");
		Application.Quit ();
	}
}
