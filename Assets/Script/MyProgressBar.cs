using UnityEngine;
using System.Collections;
using ProgressBar;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MyProgressBar : MonoBehaviour
{
	private LevelManager levelManager;
	ProgressBarBehaviour BarBehaviour;
	[SerializeField] float UpdateDelay = 2f;

	IEnumerator Start ()
	{
		levelManager = GameObject.FindObjectOfType<LevelManager> ();
		Debug.Log ("START Load Async");
		BarBehaviour = GetComponent<ProgressBarBehaviour> ();
		while (BarBehaviour.Value < 100) {
			yield return new WaitForSeconds (UpdateDelay);
			BarBehaviour.Value += Random.value * 100;
			//result.allowSceneActivation = true;
		}
		Debug.Log ("YEAH Loaded Async");

		yield return new WaitForSeconds (0.5f);

		SceneManager.LoadScene ("Level Selection");
	}
}