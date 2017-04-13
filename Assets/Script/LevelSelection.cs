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

	// Use this for initialization
	void Start ()
	{
		ReadPlayersPref ();
		AssembleCurrentPageButtons (currentPage);
		// sort them right at the initialization or Unity goes mad
		levels = GameObject.FindGameObjectsWithTag ("Level Selectors").OrderBy (c => c.name).ToArray ();
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
		// ToDo:
		Vector3[] buttonLocation = new [] { 
			new Vector3 (-170f, 190f, 0f), new Vector3 (0f, 190f, 0f), new Vector3 (170f, 190f, 0f),
			new Vector3 (90f, 290f, 0f), new Vector3 (180f, 290f, 0f), new Vector3 (270f, 290f, 0f), 
			new Vector3 (90f, 190f, 0f), new Vector3 (180f, 190f, 0f), new Vector3 (270f, 190f, 0f),
		};
		int index = currentPage * 10;
		for (int i = 0; i < 3; i++) {
			GameObject b = Instantiate (levelButton, new Vector3 (0f, 0f, 0f), Quaternion.identity) as GameObject;

			b.transform.SetParent (c.transform, false); 
			b.transform.localPosition = buttonLocation [i];
			b.transform.localScale = new Vector3 (1f, 1f, 1f);
			if (i < highestLevel) {
				b.GetComponent <Image> ().sprite = levelStates [1]; // green icon
				b.GetComponent <Button> ().interactable = true;
				foreach (Transform t in b.transform) {
					if (t.name == "number") {
						t.GetComponent <Text> ().text = (i).ToString (); //48.ToString ();//
					} else if (t.name == "stars") { 
						stars = PlayerPrefs.GetInt ("Level" + (i + 1).ToString ());
						t.GetComponent <Image> ().sprite = levelStars [stars];
					}
				}
			} else {
				b.GetComponent <Image> ().sprite = levelStates [0];
				b.GetComponent <Button> ().interactable = false;
				foreach (Transform t in levels[i].transform) {
					if (t.name == "stars") { 
						t.GetComponent <Image> ().sprite = levelStars [0];
					}
				}
			}
		}
		// create a child game object for canvas + set is location
		// attach button component to it
		// attach script for loading level tto it
		// create child game object for level number + set is location
		// create child game object for level stars + set is location
	}

	// We need two sets of buttons: One inactive with the lock icon
	// the other one active with green button,  level number and number of stars
	private void HandleLocks ()
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
	}

	// Update is called once per frame
	void Update ()
	{
		// get the text property 
		// set them to numbers or lock ison based on the value of HighestLevel
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
