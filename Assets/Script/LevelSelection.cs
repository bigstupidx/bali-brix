using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Linq;

public class LevelSelection : MonoBehaviour
{
	public static int highestLevel = 1;
	public Sprite[] levelStates;
	public Sprite[] levelStars;

	private GameObject[] levels;
	private int stars = 0;

	// Use this for initialization
	void Start ()
	{
		
		//PlayerPrefs.SetInt ("Level1", 0);
		//PlayerPrefs.SetInt ("Highest Level", 1);
		int index = 0;
		// sort them right at the initialization or Unity goes mad
		levels = GameObject.FindGameObjectsWithTag ("Level Selectors").OrderBy (c => c.name).ToArray ();
		if (PlayerPrefs.HasKey ("Highest Level")) {
			highestLevel = PlayerPrefs.GetInt ("Highest Level");
		} else {
			PlayerPrefs.SetInt ("Highest Level", highestLevel);
		}
		HandleLocks ();
	}
		
	/*
levelKey = string.Concat("Level", levelIndex.ToString())
layerPrefs.SetInt(levelKey, levelStars)

for each level
if (levelstars>0)
{
	activate button
	show image related to number of stars
} else {
	deactivate button
	show lock image
}
	*/

	// We need two sets of buttons: One inactive with the lock icon
	// the other one active with green button,  level number and number of stars
	private void HandleLocks ()
	{
		for (int i = 0; i < levels.Length; i++) {
			if (i < highestLevel) {
				levels [i].GetComponent <Image> ().sprite = levelStates [1]; // green icon
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
				levels [i].GetComponent <Image> ().sprite = levelStates [0]; // lock icon
				levels [i].GetComponent <Button> ().interactable = false;
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
