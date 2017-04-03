using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Principal;

public class BreakthroughBall : MonoBehaviour
{

	private float timeLeft = 10f;
	private bool active = true;
	private GameObject[] allAvailableBricks;
	private GameObject[] ball;
	public Sprite durian, normal;
	private bool trigger = false;

	// Use this for initialization
	void Start ()
	{
		ToggleTrigger ();
	}

	// Update is called once per frame
	void Update ()
	{
		allAvailableBricks = GameObject.FindGameObjectsWithTag ("Breakable");
		if (active) {
			Countdown ();
		}
	}

	private void Countdown ()
	{
		timeLeft -= Time.deltaTime;
		if (timeLeft < 0) {
			//timeLeft = 7;
			active = false;
			ToggleTrigger ();
		}
	}

	private void ToggleTrigger ()
	{
		trigger = (trigger) ? false : true;
		allAvailableBricks = GameObject.FindGameObjectsWithTag ("Breakable");
		ball = GameObject.FindGameObjectsWithTag ("Ball");
		for (int index = 0; index < allAvailableBricks.Length; index++) {
			allAvailableBricks [index].GetComponent <BoxCollider2D> ().isTrigger = trigger;
		}
		if (ball != null) {
			foreach (GameObject g in ball) {
				if (trigger) {
					g.GetComponent <SpriteRenderer> ().sprite = durian;
				} else {
					g.GetComponent <SpriteRenderer> ().sprite = normal;
				}	
			}
		}
	}
}
