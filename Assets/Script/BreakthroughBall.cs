﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Principal;

public class BreakthroughBall : MonoBehaviour
{

	private float timeLeft = 7f;
	private bool active = true;
	private GameObject[] allAvailableBricks;
	private bool trigger = false;

	// Use this for initialization
	void Start ()
	{
		print ("Got your ball!");
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
			print ("Times Up!");
			timeLeft = 7;
			active = false;
			ToggleTrigger ();
		}
	}

	private void ToggleTrigger ()
	{
		trigger = (trigger) ? false : true;
		allAvailableBricks = GameObject.FindGameObjectsWithTag ("Breakable");
		for (int index = 0; index < allAvailableBricks.Length; index++) {
			allAvailableBricks [index].GetComponent <BoxCollider2D> ().isTrigger = trigger;
		}
		print ("length: " + allAvailableBricks.Length + " trigger: " + trigger); 
	}
}