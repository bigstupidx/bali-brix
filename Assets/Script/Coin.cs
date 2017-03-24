using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coin : MonoBehaviour
{
	public GameObject coinValue;
	private int value;
	// Use this for initialization
	void Start ()
	{
		value = Random.Range (4, 26);
		LevelManager.coins += value;
		ShowValue (value);
	}
	
	// Update is called once per frame
	public void ShowValue (int value)
	{
		GameObject g = 
			Instantiate (coinValue, new Vector3 (0f, 0f, 0f), transform.rotation) as GameObject;
		g.GetComponent <TextMesh> ().text = "Bada";
		print ("coin value: " + value);
		print ("coin value loc: " + g.transform.position);

	}
}
