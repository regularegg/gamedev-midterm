using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Moves players along X and Y axises(?) 
// Goal of the game is to take pictures at x amount of locations before
// "timer" (phone battery) runs out
public class PlayerMovement : MonoBehaviour {

	private Rigidbody RB;
	private Vector3 inputVector;

	private bool faceRight, onGround = true, canMove = true;

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
		//can use coded acceleration!!! < figure out later
		if ((Input.GetKey(KeyCode.A) || Input.GetKey((KeyCode.D)))&&canMove)
		{
			//cha9nge velocity to make guy move
			//RB.velocity = Vector3.ClampMagnitude(Vector3.right*Input.GetAxis("Horizontal")*moveSpeed,maxSpeed)+(Vector3.up*RB.velocity.y);
			RB.AddForce(inputVector);
			if ((RB.velocity.x > maxSpeed)||(RB.velocity.x<-maxSpeed))
			{
				RB.velocity = new Vector3(maxSpeed*Input.GetAxis("Horizontal"),RB.velocity.y,0f);
			}
			//RB.velocity = Vector3.ClampMagnitude(RB.velocity, maxSpeed);
		}
		else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D)&&onGround)
		{
			//Negate X velocity to make guy stop
			//Really weird w/ jumps right now
			//maybe lerp for more realistic stop??
			
			//RB.velocity = Vector3.right * Mathf.Lerp(RB.velocity.y,0,0.1f);
			
			
		}

		if (Input.GetKeyDown(KeyCode.W)&&onGround)
		{
			//try adding velocity directly for jump
			//when hit ground on side, set x velocity to 0 or deactivate jump controls
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
		}
		if (other.CompareTag("Side"))
		{
			//set velocity to 0??
			//RB.velocity = new Vector3(0,RB.velocity.y,0);
			canMove = false;
			Debug.Log("hit side");
		}
	}

	private void OnTriggerExit(Collider other)
	{
		canMove = true;
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
