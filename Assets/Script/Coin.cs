using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coin : MonoBehaviour
{
	private int value;
	// Use this for initialization
	void Start ()
	{
		value = Random.Range (4, 26);
		LevelManager.coins += value;
	}
}
