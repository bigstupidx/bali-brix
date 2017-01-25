using UnityEngine;
using System.Collections;

public class FallingObjects : MonoBehaviour
{
	public Sprite[] fallingObjects;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public Sprite SetTheBall (int index)
	{
		return fallingObjects [index];
	}
}
