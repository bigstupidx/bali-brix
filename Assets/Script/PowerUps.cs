using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;

public class PowerUps : MonoBehaviour
{
	public GameObject gun, sealer, slowDown, growPaddle, breakThrough;
	private CanvasManager cm;
	public static List<string> powerUps = new List<string>{ };

	public void processPowerUpSelection (GameObject go)
	{
		int cost = GetPrice (go.transform.name);
		if (cost < LevelManager.coins) {
			LevelManager.coins -= cost;
			powerUps.Add (go.transform.name);
			print ("current powers: " + powerUps.ToArray () + " - " + go.transform.name);
			print ("coins: " + LevelManager.coins);
			cm.UpdateCoins ();
		}
	}

	private int GetPrice (string name)
	{
		int price = 0;
		switch (name) {
		case "Sealer":
			price = 100;
			break;
		
		case "Gun":
			price = 250;
			break;
		}
		return price;
	}
}
