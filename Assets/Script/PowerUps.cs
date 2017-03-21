using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;
using UnityEngine.UI;

public class PowerUps : MonoBehaviour
{
	public GameObject gun, sealer, slowDown, growPaddle, breakThrough;
	public static List<string> powerUps = new List<string>{ };
	public Sprite[] buttonFrame;
	private CanvasManager cm = new CanvasManager ();
	public Canvas IAP;

	public void processPowerUpSelection (GameObject go)
	{
		string name = go.transform.name;
		int cost = GetPrice (name);
		//go = GameObject.Find (name);
		if (cost < LevelManager.coins) {
			LevelManager.coins -= cost;
			powerUps.Add (name);
			print ("powerup: " + go); 
			go.GetComponent <Image> ().sprite = buttonFrame [1];
			cm.UpdateCoins ();
		} else {
			cm.toggleCanvas (IAP);
		}
	}

	private int GetPrice (string name)
	{
		int price = 0;
		switch (name) {
		case "Sealer":
			price = 100;
			break;
		case "Grow Ball":
			price = 125;
			break;
		case "Triple Ball":
			price = 150;
			break;
		case "Grow Paddle":
			price = 200;
			break;
		case "Gun":
			price = 250;
			break;
		case "Bonus Ball":
			price = 275;
			break;
		case "Break Through":
			price = 300;
			break;
		}
		return price;
	}
}