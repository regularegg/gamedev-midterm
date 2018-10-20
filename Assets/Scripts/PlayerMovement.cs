using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Moves players along X and Y axises(?) 
// Goal of the game is to take pictures at x amount of locations before
// "timer" (phone battery) runs out
public class PlayerMovement : MonoBehaviour {

	public float moveSpeed = 2f;
	public float jump = 50f;
	public float maxSpeed = 1.5f;
	public PhysicMaterial slipperyMaterial, grippyMaterial;
	public GameObject playerModel;
	
	private Rigidbody RB;
	private Vector3 inputVector;


	private bool faceRight = true, onGround = true, keyPressed;

	public bool FaceRight //Changes rotation of player model to face the direction they are moving in
	{
		get { return faceRight; }
		set
		{
			if (value)
			{
				//if the value is not the same as current direction, rotate
				if(!faceRight)
				playerModel.transform.Rotate(Vector3.up,180,Space.Self);
				
				faceRight = value;
			}
			else
			{
				//if the value is not the same as current direction, rotate
				if(faceRight)
				playerModel.transform.Rotate(Vector3.up,180,Space.Self);
				
				faceRight = value;
			}
		}

	}

	
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
		//if ((Input.GetKey(KeyCode.A) || Input.GetKey((KeyCode.D)))&&canMove)
		if ((Mathf.Abs(Input.GetAxis("Horizontal"))>0.5f))
		{
			//change velocity to make guy move
			RB.AddForce(inputVector);
			
			//if velocity.x exceeds maxspeed, change artificially edit velocity.x to maxspeed
			if ((RB.velocity.x > maxSpeed)||(RB.velocity.x<-maxSpeed))
			{
				RB.velocity = new Vector3(maxSpeed * Input.GetAxis("Horizontal"), RB.velocity.y, 0f);
			}
			
			//When player is moving in a direction, performs check to make sure they are moving in the right direction 
			//using Get,Set
			if (Input.GetKey(KeyCode.A))
			{
				FaceRight = false;
			}
			else if (Input.GetKey(KeyCode.D))
			{
				FaceRight = true;
			}

			
		}
		else if ((Mathf.Abs(Input.GetAxis("Horizontal"))<0.5f)&&onGround)
		{
			//Negate X velocity to make guy stop
			//Really weird w/ jumps right now
			Debug.Log("Stop");
			RB.velocity = new Vector3(0,RB.velocity.y,0);
			

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
			RB.AddForce(Vector3.down);
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
		if (other.CompareTag("LowLimit"))
		{
			SceneManager.LoadScene("Dead");
		}
	}


}
