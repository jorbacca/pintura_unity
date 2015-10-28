using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class TestingDragThree : MonoBehaviour
{
	
	public int normalCollisionCount = 0;
	public float moveLimit = .5f;
	public float collisionMoveFactor = .01f;
	public float addHeightWhenClicked = 0.0f;
	public bool freezeRotationOnDrag = true;
	public Camera cam  ;
	private Rigidbody myRigidbody ;
	private Transform myTransform  ;
	private bool canMove = false;
	private float yPos;
	private bool gravitySetting ;
	private bool freezeRotationSetting ;
	private float sqrMoveLimit ;
	private int collisionCount = 0;
	private Transform camTransform ;
	
	void Start () 
	{
		myRigidbody = GetComponent<Rigidbody>();
		myTransform = transform;
		if (!cam) 
		{
			cam = Camera.main;
		}
		if (!cam) 
		{
			Debug.Log("------->>>>>>>>>>>>ERROR: Can't find camera tagged MainCamera");
			return;
		}
		camTransform = cam.transform;
		sqrMoveLimit = moveLimit * moveLimit;   // Since we're using sqrMagnitude, which is faster than magnitude
	}
	
	void OnMouseDown () 
	{	
		Debug.Log ("Llamado al metodo OnMouseDown");
		canMove = true;
		myTransform.Translate(Vector3.up*addHeightWhenClicked);
		gravitySetting = myRigidbody.useGravity;
		freezeRotationSetting = myRigidbody.freezeRotation;
		myRigidbody.useGravity = false;
		myRigidbody.freezeRotation = freezeRotationOnDrag;
		yPos = myTransform.position.y;
	}
	
	void OnMouseUp () 
	{	
		Debug.Log ("Llamado al metodo OnMouseUp");
		canMove = false;
		myRigidbody.useGravity = gravitySetting;
		myRigidbody.freezeRotation = freezeRotationSetting;
		if (!myRigidbody.useGravity) 
		{
			Vector3 pos = myTransform.position;
			pos.y = yPos-addHeightWhenClicked;
			myTransform.position = pos;
		}
	}
	
	void OnCollisionEnter () 
	{	
		Debug.Log ("Llamado al metodo OnCollisionEnter");
		collisionCount++;
	}
	
	void OnCollisionExit () 
	{	
		Debug.Log ("Llamado al metodo OnCollisionExit");
		collisionCount--;
	}
	
	void FixedUpdate () 
	{
		if (!canMove)
		{
			return;
		}
		
		myRigidbody.velocity = Vector3.zero;
		myRigidbody.angularVelocity = Vector3.zero;
		
		Vector3 pos = myTransform.position;
		pos.y = yPos;
		myTransform.position = pos;
		
		Vector3 mousePos = Input.mousePosition;
		Vector3 move = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, camTransform.position.y - myTransform.position.y)) - myTransform.position;
		move.y = 0.0f;
		if (collisionCount > normalCollisionCount)		
		{
			move = move.normalized*collisionMoveFactor;
		}
		else if (move.sqrMagnitude > sqrMoveLimit) 
		{
			move = move.normalized*moveLimit;
		}
		
		myRigidbody.MovePosition(myRigidbody.position + move);
	}
}
