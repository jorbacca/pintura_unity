using UnityEngine;
using System.Collections;

public class TestingDragTwo : MonoBehaviour {
	public float speed = 0.1F;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) {
			// Get movement of the finger since last frame
			Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
			
			// Move object across XY plane
			//transform.Translate(-touchDeltaPosition.x * speed, -touchDeltaPosition.y * speed, 0);
			transform.Translate(touchDeltaPosition.x, touchDeltaPosition.y, 0);
		}
	}
}
