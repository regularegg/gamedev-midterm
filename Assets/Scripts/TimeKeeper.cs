using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//keeps track of time and time penalty when hit by enemies/hazards
public class TimeKeeper : MonoBehaviour
{
	public Text timeDisplay;
	public int penalty;

	private int Time;
	public int time
	{
		get { return Time; }
		set
		{
			Time = value;
			if (value <= 0)
			{
				SceneManager.LoadScene("GameOver");
			}
		}
	}
	
	// Use this for initialization
	void Start ()
	{
		time = 60;
		timeDisplay.text = "Time: " + time;
		StartCoroutine(Countdown());
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
