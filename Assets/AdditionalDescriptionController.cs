using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Additional description controller.
/// This script should be associated to any object that shows the additional information (Not the buttons, but the rectangles that show the additional info)
/// </summary>
public class AdditionalDescriptionController : MonoBehaviour {

	public GameObject TextObjectToShow;
	public string TextToShowInDescription;
	//variable que controla si la informacion adicional esta siendo mostrada.
	public bool add_description_info_displayed;

	// Use this for initialization
	void Start () {
		this.GetComponent<Image> ().enabled = false;
		TextObjectToShow.GetComponent<Text> ().enabled = false;
		add_description_info_displayed = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/// <summary>
	/// Shows the additional information. This method activates the box that shows the additional information
	/// for a specific button.
	/// Note that the TextObjectToShow should be associated through the editor
	/// </summary>
	public void ShowAdditionalInformation(){
		Debug.Log ("Llamado a la funcion MOSTRAR info adicional de " + this.name);
		TextObjectToShow.GetComponent<Text> ().text = TextToShowInDescription;
		this.GetComponent<Image> ().enabled = true;
		TextObjectToShow.GetComponent<Text> ().enabled = true;
		add_description_info_displayed = true;
	}

	/// <summary>
	/// Hides the additional information. This method hides the additional information already displayed.
	/// Note that the TextObjectToShow should be associated through the editor
	/// </summary>
	public void HideAdditionalInformation(){
		Debug.Log ("Llamado a la funcion para OCULTAR info adicional de " + this.name);
		this.GetComponent<Image> ().enabled = false;
		TextObjectToShow.GetComponent<Text> ().enabled = false;
		add_description_info_displayed = false;

	}

	public void OnClickAction_hide(){
		this.GetComponent<Image> ().enabled = false;
		TextObjectToShow.GetComponent<Text> ().enabled = false;
		add_description_info_displayed = false;
	}
}
