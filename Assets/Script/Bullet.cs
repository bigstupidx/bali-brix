﻿using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnCollisionEnter2D (Collision2D collision)
	{
		Destroy (this.gameObject);
	}
}