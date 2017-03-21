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
		TogglePowerUp (cost);

		if (cost <= LevelManager.coins) {
			LevelManager.coins -= cost;
			powerUps.Add (name);
			print ("powerup: " + name); 
			go.GetComponent <Image> ().sprite = buttonFrame [1];
			cm.UpdateCoins ();
		} else {
			cm.toggleCanvas (IAP);
		}
	}

	private void TogglePowerUp (int price)
	{
		
	}

	private int GetPrice (string name)
	{
		int price = 0;
		switch (name) {
		case "Button- sealer":
			price = 100;
			break;
		case "Button- grow ball":
			price = 125;
			break;
		case "Button- clone":
			price = 150;
			break;
		case "Button- grow paddle":
			price = 200;
			break;
		case "Button- gun":
			price = 250;
			break;
		case "Button- life":
			price = 275;
			break;
		case "Button- durian":
			price = 300;
			break;
		}
		return price;
	}
}