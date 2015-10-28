using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour {

	public Image image_header;
	public Text title;
	public Button btn1;
	public Button self_assessment_button;

	//variables para definir las acciones que deben ejecutar los botones:
	public System.Action btn1_action;
	public System.Action self_asessment_action;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ChangeImage(string path){

		image_header.sprite = Resources.Load<Sprite> (path);
	}

	public void ChangeStepTitle(string name){
		title.text = name;
	}

	public void ActionButton1(){
		Debug.LogError ("Llamado en el CanvasManager");
		this.btn1_action ();
	}

	public void SelfAssessmentButtonAction(){
		Debug.LogError ("Llamado al action del SelfAssessment");
		this.self_asessment_action ();
	}

	public void ChangeButtonAction(){
		//btn1.onClick = act;
		btn1.onClick.AddListener (()=>{ActionButton1();});
	}

	public void SetAssessmentButtonAction(){
		self_assessment_button.onClick.AddListener(()=>{SelfAssessmentButtonAction();});
	}

}
