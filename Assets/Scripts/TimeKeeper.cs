using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//keeps track of time and time penalty when hit by enemies/hazards
public class TimeKeeper : MonoBehaviour
{
	public Text timeDisplay;
	public int time = 60, penalty;
	
	// Use this for initialization
	void Start ()
	{
		timeDisplay.text = "Time: " + time;
		StartCoroutine(Countdown());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//counts down the seconds
	IEnumerator Countdown()
	{
		Debug.Log( "Coroutine started");
		while (time>0)
		{
			Debug.Log("waiting");
			time--;
			timeDisplay.text = "Time: " + time;
			yield return new WaitForSeconds(1);
		}

		yield return null;
	}

	//deducts time when hit by enemy
	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.CompareTag("Hazard"))
		{
			time -= penalty;
			timeDisplay.text = "Time: " + time;
		}
	}
}
