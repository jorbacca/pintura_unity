using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Vuforia;

public class ControllerAddInfoInMarker : MonoBehaviour {

	//Referencia al controlador principal de la app:
	public AppManager manager;

	//variables especificas que se utilizan en el tutorial 1:
	public string image_marker_path;
	public string image_marker_real_path;
	public string text_to_show_blinking;
	public string text_to_show_blinking_touch;
	public string image_hand_touch_path;

	//variable que permite definir el vector de ordenes de pasos que controla la interfaz:
	public int[] ordenes;

	//variables que contienen las rutas a las imagenes que se deben mostrar en los diferentes botones:
	public string image_for_button_select;
	public string image_for_button_one;
	public string image_for_button_two;
	public string image_for_button_three;
	public string image_for_button_four;
	public string image_for_button_five;
	public string image_for_button_six;
	public string image_for_button_seven;
	public string image_for_button_eight;

	//variables que controlan el texto que se muestra en la info adicional para cada boton:
	public string text_add_info_btn_one;
	public string text_add_info_btn_two;
	public string text_add_info_btn_three;
	public string text_add_info_btn_four;
	public string text_add_info_btn_five;
	public string text_add_info_btn_six;
	public string text_add_info_btn_seven;
	public string text_add_info_btn_eight;

	//variables que controlan si los botones de informacion adicional se deben desplegar
	public bool info_add_select_button_enable;
	public bool info_add_button_one_enable;
	public bool info_add_button_two_enable;
	public bool info_add_button_three_enable;
	public bool info_add_button_four_enable;
	public bool info_add_button_five_enable;
	public bool info_add_button_six_enable;
	public bool info_add_button_seven_enable;
	public bool info_add_button_eight_enable;

	//Variables especificas que se utilizan en el Modo RA del Modo Evaluacion
	public string image_question_mark_path;
	public string image_real_help_path;

	//variables para controlar el texto del Feedback:
	public string feedback_info_text_path;

	//variables para controlar referencias a scripts:
	private ControllerBlinkingMarker controller_tutorial_one;
	private ControllerBlinkingAddInfo controller_tutorial_two;
	private ControllerBlinkingARGeneric controller_generic_ar;
	private ControllerBlinkARModeEvaluation controller_evaluation_mode;

	//variables que almacenan las acciones:
	public System.Action onClickSelectButton_tut1;
	public System.Action onClickSelectButton_generic;
	public System.Action onClickSelectButton_evaluationMode;

	/*
	//METODO PARA EL BOTON SELECCIONAR:
	//Funcion delegada que permite asignar la accion que se debe ejecutar en el AppManager cuando se
	//hace click sobre el boton Select:
	public delegate void onClickSelectGeneric_action(string interface_from);
	//variable del metodo delegado que se asigna desde el AppManager con el metodo que se debe ejecutar para ir con select
	public onClickSelectGeneric_action onClickSelect_act;
*/
	//METODO PARA EL BOTON SELECCIONAR:
	//Funcion delegada que permite asignar la accion que se debe ejecutar en el AppManager cuando se
	//hace click sobre el boton Select:
	//public delegate void onClickSelectButtonContinue (string interface_from);
	//variable del metodo delegado que se asigna desde el AppManager con el metodo que se debe ejecutar para ir con select
	//public onClickSelectButtonContinue onClickSelectBtn;


	//variable que controla la interfaz desde la cual se retorna para controlarlo en el AppManager:
	public string interface_coming_from;

	//Variable que referencia el Script de markerBehaviour para recuperar el ID del marcador:
	private MarkerBehaviour mMarkerBehav;




	// Use this for initialization
	void Start () {
		Debug.LogError ("Llamado al metodo START de ControllerAddInfoInMarker ************-->");
		/*
		info_add_select_button_enable = false;
		info_add_button_one_enable = false;
		info_add_button_two_enable = false;
		info_add_button_three_enable = false;
		info_add_button_four_enable = false;
		info_add_button_five_enable = false;
		info_add_button_six_enable = false;
		info_add_button_seven_enable = false;
		info_add_button_eight_enable = false;

		image_for_button_select = "";
		image_for_button_one = "";
		image_for_button_two = "";
		image_for_button_three = "";
		image_for_button_four = "";
		image_for_button_five = "";
		image_for_button_six = "";
		image_for_button_seven = "";
		image_for_button_eight = "";

		text_add_info_btn_one = "";
		text_add_info_btn_two = "";
		text_add_info_btn_three = "";
		text_add_info_btn_four = "";
		text_add_info_btn_five = "";
		text_add_info_btn_six = "";
		text_add_info_btn_seven = "";
		text_add_info_btn_eight = "";
		
		feedback_info_text_path = "Texts/errors/1_feedback_no_text";
		*/
		mMarkerBehav = this.GetComponent<MarkerBehaviour> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/// <summary>
	/// Loads the information to interface.
	/// </summary>
	/// <param name="tutorial_one">If set to <c>true</c> tutorial_one.</param>
	/// <param name="tutorial_two">If set to <c>true</c> tutorial_two.</param>
	/// <param name="other_step">If set to <c>true</c> other_step.</param>
	public void LoadInformationToInterface(bool tutorial_one, bool tutorial_two, bool other_step){
		/*
		if (mMarkerBehav != null) {
			Debug.Log ("Marker: " + mMarkerBehav.Marker.Name);
			Debug.Log ("ControllerAddInfoInMarker.LoadInformationToInterface - Cargando info desde el marcador: " + mMarkerBehav.Marker.MarkerID);

		}
		else
			Debug.LogError ("Error en ControllerAddInfoInMarker: El mMarkerBehav es NULL");
		*/
		if (manager.interfaceInstanceActive != null) {
			if(tutorial_one){
				//obteniendo referencia al controlador del tutorial 1 (referencia al script)
				controller_tutorial_one = manager.interfaceInstanceActive.GetComponent<ControllerBlinkingMarker>();
				//activando los botones que correspondan:
				controller_tutorial_one.info_add_select_button_enable = this.info_add_select_button_enable;
				controller_tutorial_one.info_add_button_one_enable = this.info_add_button_one_enable;
				controller_tutorial_one.info_add_button_two_enable = this.info_add_button_two_enable;
				controller_tutorial_one.info_add_button_three_enable = this.info_add_button_three_enable;
				controller_tutorial_one.info_add_button_four_enable = this.info_add_button_four_enable;
				controller_tutorial_one.info_add_button_five_enable = this.info_add_button_five_enable;
				controller_tutorial_one.info_add_button_six_enable = this.info_add_button_six_enable;
				controller_tutorial_one.info_add_button_seven_enable = this.info_add_button_seven_enable;
				controller_tutorial_one.info_add_button_eight_enable = this.info_add_button_eight_enable;
				//asignando las imagenes que se deben cargar:
				controller_tutorial_one.image_for_button_select = this.image_for_button_select;
				controller_tutorial_one.image_for_button_one = this.image_for_button_one;
				controller_tutorial_one.image_for_button_two = this.image_for_button_two;
				controller_tutorial_one.image_for_button_three = this.image_for_button_three;
				controller_tutorial_one.image_for_button_four = this.image_for_button_four;
				controller_tutorial_one.image_for_button_five = this.image_for_button_five;
				controller_tutorial_one.image_for_button_six = this.image_for_button_six;
				controller_tutorial_one.image_for_button_seven = this.image_for_button_seven;
				controller_tutorial_one.image_for_button_eight = this.image_for_button_eight;

				//asignando el texto de la informacion adicional que se muestra al hacer click sobre cada icono:
				controller_tutorial_one.text_add_info_btn_one = this.text_add_info_btn_one;
				controller_tutorial_one.text_add_info_btn_two = this.text_add_info_btn_two;
				controller_tutorial_one.text_add_info_btn_three = this.text_add_info_btn_three;
				controller_tutorial_one.text_add_info_btn_four = this.text_add_info_btn_four;
				controller_tutorial_one.text_add_info_btn_five = this.text_add_info_btn_five;
				controller_tutorial_one.text_add_info_btn_six = this.text_add_info_btn_six;
				controller_tutorial_one.text_add_info_btn_seven = this.text_add_info_btn_seven;
				controller_tutorial_one.text_add_info_btn_eight = this.text_add_info_btn_eight;

				//asignando variables particulares para el tutorial 1:
				controller_tutorial_one.image_marker_path = this.image_marker_path;
				controller_tutorial_one.image_marker_real_path = this.image_marker_real_path;
				controller_tutorial_one.text_to_show_blinking = this.text_to_show_blinking;
				controller_tutorial_one.text_to_show_blinking_touch = this.text_to_show_blinking_touch;
				controller_tutorial_one.image_hand_touch_path = this.image_hand_touch_path;
				controller_tutorial_one.onClickSelectButton = onClickSelectButton_tut1;

				Debug.Log("LoadInfoToInterface - select_image: " + controller_tutorial_one.image_for_button_select);
				Debug.Log("LoadInfoToInterface - Estado SELECT: " + controller_tutorial_one.info_add_button_one_enable);
				//inicializando variable de control de carga de informacion en el AppManager
				manager.informationLoadedFromMarker = true;

			} else if (tutorial_two){
				//obteniendo referencia al controlador del tutorial 2:
				controller_tutorial_two = manager.interfaceInstanceActive.GetComponent<ControllerBlinkingAddInfo>();
				controller_tutorial_two.info_add_select_button_enable = this.info_add_select_button_enable;
				controller_tutorial_two.info_add_button_one_enable = this.info_add_button_one_enable;
				controller_tutorial_two.info_add_button_two_enable = this.info_add_button_two_enable;
				controller_tutorial_two.info_add_button_three_enable = this.info_add_button_three_enable;
				controller_tutorial_two.info_add_button_four_enable = this.info_add_button_four_enable;
				controller_tutorial_two.info_add_button_five_enable = this.info_add_button_five_enable;
				controller_tutorial_two.info_add_button_six_enable = this.info_add_button_six_enable;
				controller_tutorial_two.info_add_button_seven_enable = this.info_add_button_seven_enable;
				controller_tutorial_two.info_add_button_eight_enable = this.info_add_button_eight_enable;
				//asignando las imagenes que se deben cargar:
				controller_tutorial_two.image_for_button_select = this.image_for_button_select;
				controller_tutorial_two.image_for_button_one = this.image_for_button_one;
				controller_tutorial_two.image_for_button_two = this.image_for_button_two;
				controller_tutorial_two.image_for_button_three = this.image_for_button_three;
				controller_tutorial_two.image_for_button_four = this.image_for_button_four;
				controller_tutorial_two.image_for_button_five = this.image_for_button_five;
				controller_tutorial_two.image_for_button_six = this.image_for_button_six;
				controller_tutorial_two.image_for_button_seven = this.image_for_button_seven;
				controller_tutorial_two.image_for_button_eight = this.image_for_button_eight;
				//asignando el texto de la informacion adicional que se muestra al hacer click sobre cada icono:
				controller_tutorial_two.text_add_info_btn_one = this.text_add_info_btn_one;
				controller_tutorial_two.text_add_info_btn_two = this.text_add_info_btn_two;
				controller_tutorial_two.text_add_info_btn_three = this.text_add_info_btn_three;
				controller_tutorial_two.text_add_info_btn_four = this.text_add_info_btn_four;
				controller_tutorial_two.text_add_info_btn_five = this.text_add_info_btn_five;
				controller_tutorial_two.text_add_info_btn_six = this.text_add_info_btn_six;
				controller_tutorial_two.text_add_info_btn_seven = this.text_add_info_btn_seven;
				controller_tutorial_two.text_add_info_btn_eight = this.text_add_info_btn_eight;

				//variables especificas del tutorial 2:
				controller_tutorial_two.image_marker_path = this.image_marker_path;
				controller_tutorial_two.image_marker_real_path = this.image_marker_real_path;

				//lo siguiente se asigna directamente a la interfaz desde AppManager
				//controller_tutorial_two.feedback_info_text_path = this.feedback_info_text_path;

				Debug.Log("LoadInfoToInterface - select_image: " + controller_tutorial_two.image_for_button_select);
				Debug.Log("LoadInfoToInterface - Estado SELECT: " + controller_tutorial_two.info_add_button_one_enable);
				Debug.Log("LoadInfoToInterface - Estado BTN_ONE: " + controller_tutorial_two.info_add_button_one_enable);
				Debug.Log("LoadInfoToInterface - BTN_ONE_image: " + controller_tutorial_two.image_for_button_one);

				//Notificando al AppManager que la informacion ya se ha cargado desde el marcador
				manager.informationLoadedFromMarker = true;

			} else if(other_step){

				controller_generic_ar = manager.interfaceInstanceActive.GetComponent<ControllerBlinkingARGeneric>();
				controller_generic_ar.info_add_select_button_enable = this.info_add_select_button_enable;
				controller_generic_ar.info_add_button_one_enable = this.info_add_button_one_enable;
				controller_generic_ar.info_add_button_two_enable = this.info_add_button_two_enable;
				controller_generic_ar.info_add_button_three_enable = this.info_add_button_three_enable;
				controller_generic_ar.info_add_button_four_enable = this.info_add_button_four_enable;
				controller_generic_ar.info_add_button_five_enable = this.info_add_button_five_enable;
				controller_generic_ar.info_add_button_six_enable = this.info_add_button_six_enable;
				controller_generic_ar.info_add_button_seven_enable = this.info_add_button_seven_enable;
				controller_generic_ar.info_add_button_eight_enable = this.info_add_button_eight_enable;
				//asignando las imagenes que se deben cargar:
				controller_generic_ar.image_for_button_select = this.image_for_button_select;
				controller_generic_ar.image_for_button_one = this.image_for_button_one;
				controller_generic_ar.image_for_button_two = this.image_for_button_two;
				controller_generic_ar.image_for_button_three = this.image_for_button_three;
				controller_generic_ar.image_for_button_four = this.image_for_button_four;
				controller_generic_ar.image_for_button_five = this.image_for_button_five;
				controller_generic_ar.image_for_button_six = this.image_for_button_six;
				controller_generic_ar.image_for_button_seven = this.image_for_button_seven;
				controller_generic_ar.image_for_button_eight = this.image_for_button_eight;
				//asignando el texto de la informacion adicional que se muestra al hacer click sobre cada icono:
				controller_generic_ar.text_add_info_btn_one = this.text_add_info_btn_one;
				controller_generic_ar.text_add_info_btn_two = this.text_add_info_btn_two;
				controller_generic_ar.text_add_info_btn_three = this.text_add_info_btn_three;
				controller_generic_ar.text_add_info_btn_four = this.text_add_info_btn_four;
				controller_generic_ar.text_add_info_btn_five = this.text_add_info_btn_five;
				controller_generic_ar.text_add_info_btn_six = this.text_add_info_btn_six;
				controller_generic_ar.text_add_info_btn_seven = this.text_add_info_btn_seven;
				controller_generic_ar.text_add_info_btn_eight = this.text_add_info_btn_eight;

				controller_generic_ar.image_marker_path = this.image_marker_path;
				controller_generic_ar.image_marker_real_path = this.image_marker_real_path;

				manager.informationLoadedFromMarker = true;

				Debug.Log("LoadInfoToInterface-Other - select_image: " + controller_generic_ar.image_for_button_select);
				Debug.Log("LoadInfoToInterface-Other - Estado SELECT: " + controller_generic_ar.info_additional_button_select);
				Debug.Log("LoadInfoToInterface-Other - Estado BTN_ONE: " + controller_generic_ar.info_add_button_one_enable);
				Debug.Log("LoadInfoToInterface-Other - BTN_ONE_image: " + controller_generic_ar.image_for_button_one);

			}
		}
	}// cierra metodo: LoadInformationToInterface


	/// <summary>
	/// Loads the information for evaluation mode.
	/// This method is called from AppManager in each function that activates the AR Mode for the Evaluation mode
	/// see for example the method: GoToSearchCapoCocheEvalMode
	/// </summary>
	public void LoadInformationForEvaluationMode(){
		Debug.Log ("Ingresa al metodo LoadInformationForEvaluationMode con image_real_help_path=" + image_real_help_path);
		controller_evaluation_mode = manager.interfaceInstanceActive.GetComponent<ControllerBlinkARModeEvaluation> ();
		controller_evaluation_mode.image_help_real_path = image_real_help_path;
		controller_evaluation_mode.question_mark_icon_path = image_question_mark_path;
		controller_evaluation_mode.image_for_button_select = this.image_for_button_select;
		controller_evaluation_mode.info_add_select_button_enable = true;
		//la informacion ya ha sido cargada sobre el marcador:
		manager.informationLoadedFromMarker = true;

		Debug.Log ("Termina metodo LoadInformationForEvaluationMode con image_question_mark_path=" + image_question_mark_path);

	}

}
