using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ControllerFeedbackScript : MonoBehaviour {

	public GameObject TextObjectToShow;
	public string TextToShowInFeedback;
	public bool feedback_info_displayed;
	
	// Use this for initialization
	void Start () {
		this.GetComponent<Image> ().enabled = false;
		TextObjectToShow.GetComponent<Text> ().enabled = false;
		feedback_info_displayed = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	/// <summary>
	/// Shows the additional information. This method activates the box that shows the additional information
	/// for a specific button.
	/// Note that the TextObjectToShow should be associated through the editor
	/// </summary>
	public void ShowFeedback(){
		Debug.Log ("Llamado a la funcion MOSTRAR Feedback " + this.name);
		TextObjectToShow.GetComponent<Text> ().text = TextToShowInFeedback;
		this.GetComponent<Image> ().enabled = true;
		TextObjectToShow.GetComponent<Text> ().enabled = true;
		feedback_info_displayed = true;
	}
	
	/// <summary>
	/// Hides the additional information. This method hides the additional information already displayed.
	/// Note that the TextObjectToShow should be associated through the editor
	/// </summary>
	public void HideFeedback(){
		Debug.Log ("Llamado a la funcion para OCULTAR feedback de " + this.name);
		this.GetComponent<Image> ().enabled = false;
		TextObjectToShow.GetComponent<Text> ().enabled = false;
		feedback_info_displayed = false;
		
	}

	/// <summary>
	/// Raises the click action_hide feedback event. Action that is called from an OnClick event on the feedback objetct in the interface
	/// </summary>
	public void OnClickAction_hideFeedback(){
		this.GetComponent<Image> ().enabled = false;
		TextObjectToShow.GetComponent<Text> ().enabled = false;
		feedback_info_displayed = false;
	}
}
