using UnityEngine;
using System.Collections;

public class LevelSelection : MonoBehaviour
{
	public static int highestLevel;
	// Use this for initialization
	void Start ()
	{
		highestLevel = PlayerPrefs.GetInt ("HighestLevel", 1);
	}
	
	// Update is called once per frame
	void Update ()
	{
		// get the text property 
		// set them to numbers or lock ison based on the value of HighestLevel
	}
}
