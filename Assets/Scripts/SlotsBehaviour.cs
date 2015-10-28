using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;


/// <summary>
/// Esta clase controla cada slot en la parte de organizacion de las fases
/// </summary>
public class SlotsBehaviour : MonoBehaviour, IDropHandler {
	//variable que indica la fase correcta del slot
	public int phase_for_slot;

	//variable boolean que indica si el objeto colocado es el que debe ir en este slot
	public bool slot_con_objeto_correcto;

	//variable que indica la fase del boton que se ha colocado sobre este slot:
	public int id_of_phase_in_slot;

	public GameObject item{
		get {
			if (transform.childCount > 0)
				 return transform.GetChild (0).gameObject;
			return null;
		}
	}

	private DragBehaviour drag_controller;
	private int phase_from_button_being_dragged;

	#region IDropHandler implementation

	public void OnDrop (PointerEventData eventData)
	{
		if (!item) {
			Debug.Log ("Ingresa a item diferente de NULL");
			DragBehaviour.itemBeingDragged.transform.SetParent (transform);
			drag_controller = item.GetComponent<DragBehaviour> ();
			if (drag_controller != null) {
				phase_from_button_being_dragged = drag_controller.phase_number;
				id_of_phase_in_slot = drag_controller.phase_number;
				Debug.Log ("Fase del boton seleccionado: " + phase_from_button_being_dragged + " en slot: " + phase_for_slot);
				if (phase_from_button_being_dragged == phase_for_slot) {
					slot_con_objeto_correcto = true;
					Debug.Log ("El item se ha colocado en el lugar correcto y se va a NOTIFICAR AL SCRIPT");
				} else {
					slot_con_objeto_correcto = false;
					Debug.Log ("El item NO se ha colocado correctamente - NO SE NOTIFICA AL SCRIPT");
				}
				//invocando al metodo HasChanged del script CanvasProcessPhasesManagerEval para notificar que un objeto se ha ubicado en el slot:
				ExecuteEvents.ExecuteHierarchy<IHasChanged> (gameObject, null, (x,y) => x.HasChanged ());
			} else
				Debug.Log ("SlotsBehaviour: NO se ha podido obtener el componente DragBehaviour");
		} else {
			slot_con_objeto_correcto = false;
			Debug.Log ("En slot: " + this.gameObject.name + " tiene slot_con_objeto_correcto= " + slot_con_objeto_correcto);
		}
	}

	#endregion
}
