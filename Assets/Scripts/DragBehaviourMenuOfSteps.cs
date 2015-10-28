using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DragBehaviourMenuOfSteps : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	public static GameObject itemBeingDragged;
	//esta variable se asigna cuando se asigna la informacion a cada boton:
	public int phase_number;
	
	Vector3 startPosition;
	Transform startParent;
	SlotsBehaviourMenuSteps slot_controller;
	
	#region IBeginDragHandler implementation
	
	public void OnBeginDrag (PointerEventData eventData)
	{
		itemBeingDragged = gameObject;
		startPosition = transform.position;
		startParent = transform.parent;
		GetComponent<CanvasGroup> ().blocksRaycasts = false;
	}
	
	#endregion
	
	#region IDragHandler implementation
	
	public void OnDrag (PointerEventData eventData)
	{
		transform.position = Input.mousePosition;
	}
	
	#endregion
	
	#region IEndDragHandler implementation
	
	public void OnEndDrag (PointerEventData eventData)
	{
		itemBeingDragged = null;
		GetComponent<CanvasGroup> ().blocksRaycasts = true;
		if (transform.parent == startParent)
			transform.position = startPosition;
		else {
			Debug.Log ("DragBehaviour: El objeto se ha cambiado a otro slot: slot_original:" + startParent.gameObject.name);
			slot_controller = startParent.gameObject.GetComponent<SlotsBehaviourMenuSteps>();
			if(slot_controller != null)
				slot_controller.slot_con_objeto_correcto = false;
		}
		
	}
	
	#endregion
}
