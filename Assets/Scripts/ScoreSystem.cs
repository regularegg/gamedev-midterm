using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

//scoring system
public class ScoreSystem : MonoBehaviour
{
	//destinations keeps track of which sites were visited to distribute pictures at end of game
	//public bool[] destinations;
	public Text ScoreDisplay;
	
	private int currentDestination, score;
	private bool canTakePic;
	private GameObject otherObject;

	// Use this for initialization
	void Start()
	{
		score = 0;
		ScoreDisplay.text = "Score: " + score;
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space) && canTakePic)
		{
			//take pic code
			//should make code to "deactivate" scenic point
			ScoreKeeper.Destinations[currentDestination] = true;
			score++;
			ScoreDisplay.text = "Score: " + score;
			ScoreKeeper.Destinations[currentDestination] = true;
			Debug.Log("Took pic!!!");
			otherObject.GetComponent<MeshRenderer>().enabled = false;
			otherObject.GetComponent<Collider>().enabled = false;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		//lets player "take pic" at scenic spot
		if (other.CompareTag("CamPoint"))
		{
			canTakePic = true;
			currentDestination = int.Parse(other.name);
			Debug.Log("Campoint is ok! can take pic!");
			otherObject = other.gameObject;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		//stops player from taking pic if they exit the scenic spot
		if (other.CompareTag("CamPoint"))
		{
			canTakePic = false;
			currentDestination = -1;
			Debug.Log("left campoint :c");
			otherObject = null;
		}
	}
}
