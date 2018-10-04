﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Moves players along X and Y axises(?) 
// Goal of the game is to take pictures at x amount of locations before
// "timer" (phone battery) runs out
public class PlayerMovement : MonoBehaviour {

	private Rigidbody RB;
	private Vector3 inputVector;

	private bool faceRight, onGround = true, canMove = true, keyPressed;

	public float moveSpeed = 10f;
	public float jump = 100f;
	public float maxSpeed = 2f;

	public PhysicMaterial slipperyMaterial;
	
	// Use this for initialization
	void Start ()
	{
		RB = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update ()
	{
		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");
		
		//inputVector  = transform.up * vertical *  jump;
		inputVector = transform.right * horizontal *  moveSpeed;
		
		if (Mathf.Abs((RB.velocity.x))<0.01f)
		{
			canMove = true;
		}
		
	}
	
	private void FixedUpdate()
	{
		//can use coded acceleration!!! < figure out later
		//if ((Input.GetKey(KeyCode.A) || Input.GetKey((KeyCode.D)))&&canMove)
		if ((Mathf.Abs(Input.GetAxis("Horizontal"))>0.1f)&&canMove)
		{
			//cha9nge velocity to make guy move
			RB.AddForce(inputVector);
			
			//if velocity.x exceeds maxspeed, change artificially edit velocity.x to maxspeed
			if ((RB.velocity.x > maxSpeed)||(RB.velocity.x<-maxSpeed))
			{
				RB.velocity = new Vector3(maxSpeed*Input.GetAxis("Horizontal"),RB.velocity.y,0f);
			}

			if (Input.GetKey(KeyCode.A))
			{
				faceRight = false;
			}
			else if (Input.GetKey(KeyCode.D))
			{
				faceRight = true;
			}

			
		}
		else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D)&&onGround)
		{
			//Negate X velocity to make guy stop
			//Really weird w/ jumps right now
			//maybe lerp for more realistic stop??
			
			RB.velocity = Vector3.right * Mathf.Lerp(RB.velocity.y,0,0.1f);
			
			//RB.velocity = Vector3.right * -RB.velocity.x;
			//RB.AddForce(-RB.velocity);

		}

		if (Input.GetKeyDown(KeyCode.W)&&onGround)
		{
			//try adding velocity directly for jump
			//when hit ground on side, set x velocity to 0 or deactivate jump controls
			//add force to jump

			RB.GetComponent<CapsuleCollider>().material = slipperyMaterial;
			RB.AddForce(Vector3.up*jump,ForceMode.Impulse);
			onGround = false;
		}

		if (!onGround)
		{
			//makes jumping a little more comfortable
			//accelerates falling 
			RB.AddForce(Vector3.down*0.5f);
		}

	}

	private void OnCollisionEnter(Collision other)
	{
		//should change this to trigger & add trigger boxes on platforms
		if (other.gameObject.CompareTag("Ground"))
		{
			onGround = true;
		}

		
	}

	private void OnTriggerEnter(Collider other)
	{
		//for later when I add the trigger boxes
		if (other.CompareTag("Ground"))
		{
			onGround = true;
			canMove = true;
			RB.GetComponent<CapsuleCollider>().material = null;

		}

		if (other.CompareTag("Side"))
		{
			canMove = false;
		}
		if (other.gameObject.CompareTag("Hazard"))
		{
			float dir;
			if (faceRight)
			{
				dir = 1;
			}
			else
			{
				dir = -1;
			}
			//If you hit hazard, it hits you w a force opposite of your current velocity
			RB.AddForce(new Vector3(transform.position.x-other.GetComponent<Transform>().position.x, RB.transform.position.y - other.GetComponent<Transform>().position.y,0f), ForceMode.Impulse);
			
			canMove = false;
		}
	}

	/*private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("Side"))
		{
			//set velocity to 0??
			//RB.velocity = new Vector3(0,RB.velocity.y,0);
			canMove = false;
			Debug.Log("hit side");
		}
	}*/
}
