using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Moves players along X and Y axises(?) 
// Goal of the game is to take pictures at x amount of locations before
// "timer" (phone battery) runs out
public class PlayerMovement : MonoBehaviour {

	private Rigidbody RB;
	private Vector3 inputVector;

	private bool faceRight, onGround = true, canTakePic;

	public float moveSpeed = 10f;
	public float jump = 1000f;
	public float maxSpeed = 2f;
	
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
		
		
	}
	
	private void FixedUpdate()
	{
		if (Input.GetKey(KeyCode.A) || Input.GetKey((KeyCode.D)))
		{
			//cha9nge velocity to make guy move
			RB.velocity = Vector3.ClampMagnitude(Vector3.right*Input.GetAxis("Horizontal")*moveSpeed,maxSpeed)+(Vector3.up*RB.velocity.y);
		}
		else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D)&&onGround)
		{
			//Negate X velocity to make guy stop
			//Really weird w/ jumps right now
			//maybe lerp for more realistic stop??
			RB.velocity = Vector3.right * Mathf.Lerp(RB.velocity.y,0,0.1f);
		}

		if (Input.GetKeyDown(KeyCode.W)&&onGround)
		{
			//add force to jump
			RB.AddForce(Vector3.up*jump,ForceMode.Impulse);
			onGround = false;
		}

		if (!onGround)
		{
			//makes jumping a little more comfortable
			//accelerates falling 
			RB.AddForce(Vector3.down*0.5f);
		}

		if (Input.GetKey(KeyCode.Space)&&canTakePic)
		{
			//take pic code
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
		//lets player "take pic" at scenic spot
		if (other.CompareTag("CamPoint"))
		{
			canTakePic = true;
		}
		//for later when I add the trigger boxes
		if (other.CompareTag("Ground"))
		{
			onGround = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		//stops player from taking pic if they exit the scenic spot
		if (other.CompareTag("CamPoint"))
		{
			canTakePic = false;
		}	
	}
}
