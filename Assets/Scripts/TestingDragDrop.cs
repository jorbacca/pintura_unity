using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]

public class TestingDragDrop : MonoBehaviour {

	private Vector3 screenPoint;
	private Vector3 offset;

	// Use this for initialization
	void Start () {
		Debug.Log ("DRAGnDROP: Llamado al metodo START");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown() {
		Debug.Log ("DRAGnDROP: Llamado al metodo onMouseDown");
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
	}

	void OnMouseDrag()
	{
		Debug.Log ("DRAGnDROP: Llamado al metodo OnMouseDrag");
		Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
		transform.position = curPosition;
	}


}