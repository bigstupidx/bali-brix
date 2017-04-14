using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Linq;
using UnityEditor;
using System.Reflection;

public class LevelSelection : MonoBehaviour
{
	public static int highestLevel = 1;
	public static int currentPage = 0;
	public Sprite[] levelStates;
	public Sprite[] levelStars;
	public Canvas c;
	public GameObject levelButton;

	private GameObject[] levels;
	private GameObject[] b;
	private int stars = 0;
	private Vector3[] buttonLocation = new [] { 
		new Vector3 (-170f, 190f, 0f), new Vector3 (0f, 190f, 0f), new Vector3 (170f, 190f, 0f),
		new Vector3 (-170f, 20f, 0f), new Vector3 (0f, 20f, 0f), new Vector3 (170f, 20f, 0f),
		new Vector3 (-170f, -150f, 0f), new Vector3 (0f, -150f, 0f), new Vector3 (170f, -150f, 0f),
		new Vector3 (0f, -320f, 0f)
	};
	// Use this for initialization
	void Start ()
	{
		ReadPlayersPref ();
		AssembleCurrentPageButtons (currentPage);
		// sort them right at the initialization or Unity goes mad
		//levels = GameObject.FindGameObjectsWithTag ("Level Selectors").OrderBy (c => c.name).ToArray ();
		//HandleLocks ();
	}

	private void ReadPlayersPref ()
	{
		// set the highest level
		if (PlayerPrefs.HasKey ("Highest Level")) {
			highestLevel = PlayerPrefs.GetInt ("Highest Level");
		} else {
			PlayerPrefs.SetInt ("Highest Level", highestLevel);
		}

		// set the current page
		if (PlayerPrefs.HasKey ("Current Page")) {
			currentPage = PlayerPrefs.GetInt ("Current Page");
		} else {
			PlayerPrefs.SetInt ("Current Page", currentPage);
		}
	}

	private void AssembleCurrentPageButtons (int page)
	{
		int index = currentPage * 10;
		for (int i = 1; i < 11; i++) {
			GameObject b = Instantiate (levelButton, new Vector3 (0f, 0f, 0f), Quaternion.identity) as GameObject;
			b.transform.SetParent (c.transform, false); 
			b.transform.localPosition = buttonLocation [i - 1];
			b.transform.localScale = new Vector3 (1f, 1f, 1f);
			if (i < highestLevel) {
				ActivateButton (b, i, index);
			} else {
				DeactivateButton (b);
			}
		}
	}

	private void ActivateButton (GameObject b, int i, int index)
	{
		b.GetComponent <Image> ().sprite = levelStates [1]; // green icon
		b.GetComponent <Button> ().interactable = true;
		foreach (Transform t in b.transform) {
			if (t.name == "number") {
				t.GetComponent <Text> ().text = (i + index).ToString (); //48.ToString ();//
			} else if (t.name == "stars") { 
				stars = PlayerPrefs.GetInt ("Level" + (i + index).ToString ());
				t.GetComponent <Image> ().sprite = levelStars [stars];
			}
		}
	}

	private void DeactivateButton (GameObject b)
	{
		b.GetComponent <Image> ().sprite = levelStates [0];
		b.GetComponent <Button> ().interactable = false;
		foreach (Transform t in b.transform) {
			if (t.name == "stars") { 
				t.GetComponent <Image> ().sprite = levelStars [0];
			}
		}
	}

	// We need two sets of buttons: One inactive with the lock icon
	// the other one active with green button,  level number and number of stars
	/*private void HandleLocks ()
	{
		for (int i = 0; i < levels.Length; i++) {
			if (i < highestLevel) {
				//levels [i].GetComponent <Image> ().sprite = levelStates [1]; // green icon
				levels [i].GetComponent <Button> ().interactable = true;
				foreach (Transform t in levels[i].transform) {
					if (t.name == "number") {
						t.GetComponent <Text> ().text = (i + 1).ToString (); //48.ToString ();//
					} else if (t.name == "stars") { 
						stars = PlayerPrefs.GetInt ("Level" + (i + 1).ToString ());
						t.GetComponent <Image> ().sprite = levelStars [stars];
					}
				}
			} else {
				//levels [i].GetComponent <Image> ().sprite = levelStates [0]; // lock icon
				levels [i].GetComponent <Button> ().interactable = false;
				foreach (Transform t in levels[i].transform) {
					if (t.name == "stars") { 
						t.GetComponent <Image> ().sprite = levelStars [0];
					}
				}
			}
		}
	}*/

	private void NextPage ()
	{
		int maxPageLevel = currentPage * 10 + 10;
		if (highestLevel > maxPageLevel) {
			currentPage++;
			AssembleCurrentPageButtons (currentPage);
		}
	}

	private void PrevPage ()
	{
		if (currentPage > 0) {
			currentPage--;
			AssembleCurrentPageButtons (currentPage);
		}
	}

	public void Quit ()
	{
		Application.Quit ();
	}

	public void LoadLevel (string name)
	{
		// Need to reset the brick counts otherwise the leftover from last game 
		// will be added to the new game
		Brick.brickCounts = 0;
		Ball.hasStarted = false;
		SceneManager.LoadScene (name);
	}
}
