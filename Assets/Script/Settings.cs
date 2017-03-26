using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
	private GameObject coins;

	void Start ()
	{
		coins = GameObject.Find ("count");
		if (coins)
			coins.GetComponent <Text> ().text = LevelManager.coins.ToString ();
	}
}