using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
	public void LoadLevel (string name)
	{
		Debug.Log ("load level requested:" + name);
		SceneManager.LoadScene (name);
	}

	public void LoadNextLevel ()
	{
		//int currentScene = SceneManager.sceneLoaded;
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1);
	}

	public void checkBrickCounts ()
	{
		// Remember that brickCounts we defined in the Brick class?
		// It is cool that we don't need to construct that class here since the 
		// variable is static, it means it is available to all classes in the game.
		if (Brick.brickCounts <= 0)
			LoadNextLevel ();
	}

	public void Quit ()
	{
		Debug.Log ("quit!");
		Application.Quit ();
	}
}
