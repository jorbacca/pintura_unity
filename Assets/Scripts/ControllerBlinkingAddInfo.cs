﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Vuforia;

public class ControllerBlinkingAddInfo : MonoBehaviour {

	//variable for externally controlling if this texture should be start blinking
	public bool should_be_blinking;
	//variable for controlling marker blinking:
	private bool blink;
	//Pixel Inset para modificar el tamaño de la textura:
	private Rect pixel_inset;
	//variable para cargar la imagen del marcador que se mostrara en cambio del objeto real cuando el estudiante
	//hace tap sobre la textura
	private Sprite marker_image;
	//variable para cargar la imagen real que deben ver los estudiantes (es decir el icono que representa la herramienta o producto que deben usar):
	private Sprite real_image;
	//variable para cargar la segunda imagen de la mano haciendo touch para el tutorial:
	private Sprite handImagetouch;
	
	public bool markerFound;
	//variable para almacenar la referencia a la imagen que se debe cargar:
	public string image_marker_path;
	//variable de referencia a la imagen real que se debe mostrar:
	public string image_marker_real_path;
	//variable con la ruta de la imagen al icono de la mano haciendo touch para el tutorial:
	public string image_hand_touch_path;

	//variable para controlar el tick de feedback positivo:
	public UnityEngine.UI.Image ImageFeedbackTick;
	//variable que controla si la informacion adicional se debe ocultar porque se va a mostrar el tick en la interfaz:
	public bool hide_Add_Info_To_ShowTick;

	//variable para controlar el titulo que se debe mostrar:
	public string text_to_show_blinking;
	//variable para controlar el titulo que se muestra en blinking con el hand touch image:
	public string text_to_show_blinking_touch;
	//variable para controlar si el estudiante hizo touch sobre la textura para cambiarla temporalmente:
	private bool touch_over_texture;
	//variable for controlling the scaling according to the device screen size:
	private static float DeviceDependentScale
	{
		get { if ( Screen.width > Screen.height)
			return Screen.height / 480f;
			else 
				return Screen.width / 480f; 
		}
	}
	
	//variables para controlar los objetos que deben estar en blinking:
	public GameObject object_image_marker;
	//public GameObject object_text;
	
	//variables para referenciar los botones de informacion adicional:
	public Button info_additional_button_select;
	public Button info_additional_button_one;
	public Button info_additional_button_two;
	public Button info_additional_button_three;
	public Button info_additional_button_four;
	public Button info_additional_button_five;
	public Button info_additional_button_six;
	public Button info_additional_button_seven;
	public Button info_additional_button_eight;
	
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
	
	//variables que almacenan los sprites para mostrar las imagenes:
	private Sprite image_button_select_sprite;
	private Sprite image_button_one_sprite;
	private Sprite image_button_two_sprite;
	private Sprite image_button_three_sprite;
	private Sprite image_button_four_sprite;
	private Sprite image_button_five_sprite;
	private Sprite image_button_six_sprite;
	private Sprite image_button_seven_sprite;
	private Sprite image_button_eight_sprite;

	//variables que controlan el texto que se muestra en la info adicional para cada boton:
	public string text_add_info_btn_one;
	public string text_add_info_btn_two;
	public string text_add_info_btn_three;
	public string text_add_info_btn_four;
	public string text_add_info_btn_five;
	public string text_add_info_btn_six;
	public string text_add_info_btn_seven;
	public string text_add_info_btn_eight;
	
	//variables para controlar la activacion de los elementos de la fase del tutorial
	private GameObject texto_btn_seleccionar_tutorial;
	private GameObject img_btn_seleccionar_tutorial;
	
	//variable para controlar cuando se debe mostrar el btn seleccionar durante el tutorial
	private bool showSelectButton = false;
	public bool is_add_info_displayed = false;
	
	//variables para agregar las acciones a los botones:
	public System.Action onClickSelectButton;

	//variable que almacena la interfaz desde la cual se retorna para controlar en el AppManager:
	public string interface_going_from;

	//variables para configurar el metodo cuando se hace click sobre el boton select este metodo tiene  un metodo asociado en el AppManager
	public delegate void onClickSelectButtonContinue (string interface_from);
	public onClickSelectButtonContinue onClickSelectBtn;


	//variable que controla el vector de ordenes de los pasos que controla esta interfaz:
	public int[] ordenes;

	//variables que controlan los objetos marcadores y DefaultTrackableEventHandler para verificar los ordenes:
	private GameObject[] markers;
	private DefaultTrackableEventHandler markerHandler;
	//variable para contar la cantidad de ordenes tanto locales como del marcador detectado:
	private int cont_ordenes;
	private int cont_ordenes_marker;

	//vector de ordenes del marcador detectado:
	private int[] ordenes_marker;

	//variable que controla si el marcador detectado corresponde con alguno de los ordenes que se han definido para esta interfaz:
	public bool correct_order;

	//referencia al objeto que muestra el feedback:
	public GameObject FeedbackObjectReference;

	//referencia al script del objeto feedback:
	private ControllerFeedbackScript controller_feedback;

	//variable que contiene la ruta al texto que muestra el feedback:
	public string feedback_info_text_path;

	//variable que controla el marcador desde el cual se esta intentando cargar la informacion adicional:
	public int marker_id_loading_from;

	
	// Use this for initialization
	void Start () {
		Debug.LogError ("llamado al start de ControllerBlinking");
		should_be_blinking = true;
		blink = false;
		
		if (object_image_marker != null) {
			marker_image = Resources.Load<Sprite>(image_marker_real_path);
			object_image_marker.GetComponent<UnityEngine.UI.Image>().enabled = false;
			object_image_marker.GetComponent<UnityEngine.UI.Image>().sprite = marker_image;
			
			//cargando la segunda imagen que se debe mostrar en el tutorial (hand haciendo touch):
			handImagetouch = Resources.Load<Sprite>(image_hand_touch_path);
		}
		/*
		if (object_text != null) {
			object_text.GetComponent<Text>().text = text_to_show_blinking;
			object_text.GetComponent<Text>().enabled = false;
		}*/
		
		//cargando el sprite de la imagen real
		real_image = Resources.Load<Sprite>(image_marker_path);
				
		//inicializando la variable touch sobre la textura:
		touch_over_texture = false;
		
		//inicializando variable que indica que el marcador ha sido encontrado:
		markerFound = false;

		//inicializando el vector ordenes:
		//ordenes = new int[1];

		//inicializando la variable que indica que el orden si es correcto:
		correct_order = false;

		//inicializando la referencia al controlador del feedback:
		controller_feedback = FeedbackObjectReference.GetComponent<ControllerFeedbackScript> ();

		//inicializando la imagen que se muestra como feedback positivo (tick en la interfaz):
		if (ImageFeedbackTick != null)
			ImageFeedbackTick.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (should_be_blinking) {
			
			//validando si touch_over_texture es false lo que indica que la corrutina ya se ejecuto para detenerla:
			if(!touch_over_texture)
				StopCoroutine(ChangeRealImageForMarkerImage());
			
			//validando si no se ha inciado el proceso de blinking se inicia la corrutina
			if(!blink){
				Debug.LogError("Llama a corrutina MarkerBlinkingCameraView");
				blink=true;
				StartCoroutine("MarkerBlinkingCameraView");
			}
			
			if(markerFound){
				//object_image_marker.GetComponent<Image>().sprite = handImagetouch;
				//object_text.GetComponent<Text>().text = text_to_show_blinking_touch;
				showSelectButton = true;
				if(is_add_info_displayed)
					should_be_blinking = false;	
			}else { //me parece que este else sobra porque aqui la imagen no debe cambiar
				//object_image_marker.GetComponent<Image>().sprite = marker_image;
				//object_text.GetComponent<Text>().text = text_to_show_blinking;
				showSelectButton = false;
			}
			
		}else {
			//Debug.LogError("Detiene corrutina MarkerBlinking");
			//StopCoroutine("MarkerBlinking");
			if(blink)
				StopCoroutine("MarkerBlinkingCameraView");
			blink=false;
			object_image_marker.GetComponent<UnityEngine.UI.Image>().enabled = false;
			//object_text.GetComponent<Text>().enabled = false;
			//this.GetComponent<GUITexture>().enabled=false;
			//	this.GetComponent<GUIText>().enabled = false;
		}
	}
	
	/// <summary>
	/// Co-rutine for the blinking effect of the marker when the CameraView is enabled.
	/// This co-rutine is controlled from the Update function
	/// </summary>
	/// <returns>Wait for seconds 1s</returns>
	private IEnumerator MarkerBlinkingCameraView(){
		while (blink) {
			object_image_marker.GetComponent<UnityEngine.UI.Image>().enabled = !object_image_marker.GetComponent<UnityEngine.UI.Image>().enabled;
			//object_text.GetComponent<Text>().enabled = !object_text.GetComponent<Text>().enabled;
			//this.GetComponent<GUITexture>().enabled = !this.GetComponent<GUITexture>().enabled;
			/*
			if(touch_over_texture)
				object_text.GetComponent<Text>().enabled = !object_text.GetComponent<Text>().enabled;
				//this.GetComponent<GUIText>().enabled = !this.GetComponent<GUIText>().enabled;
			*/	
			yield return new WaitForSeconds (1.0f);
		}
	}
	
	
	/// <summary>
	/// Co-rutine for changing the current blinking image for the marker image
	/// This co-rutine is controlled by the touch section in the Update function
	/// </summary>
	/// <returns>Wait for seconds 3s</returns>
	private IEnumerator ChangeRealImageForMarkerImage(){
		
		object_image_marker.GetComponent<UnityEngine.UI.Image> ().sprite = real_image;
		//object_text.GetComponent<Text> ().enabled = object_image_marker.GetComponent<Image> ().enabled;
		yield return new WaitForSeconds (4.0f);
		Debug.LogError ("Se cambia por real image: " + real_image);
		object_image_marker.GetComponent<UnityEngine.UI.Image> ().sprite = marker_image;
		//object_text.GetComponent<Text> ().enabled = false;
		touch_over_texture = false;
		/*
			this.GetComponent<GUITexture>().texture = marker_image;
			this.GetComponent<GUIText>().enabled = this.GetComponent<GUITexture>().enabled;
			yield return new WaitForSeconds (4.0f);
			this.GetComponent<GUITexture>().texture = real_image;
			this.GetComponent<GUIText>().enabled = false;
			touch_over_texture = false;
			*/
	} //cierra rutina ChangeRealImageForMarkerImage


	/// <summary>
	/// Co-rutine for showing the tick in the interface when the object selected is correct
	/// This co-rutine is controlled by the touch section in the Update function
	/// </summary>
	/// <returns>Wait for seconds 3s</returns>
	private IEnumerator ShowTickOnInterface(){
		
		//primero se desactiva toda la informacion adicional:
		hide_Add_Info_To_ShowTick = true; //esta variable es leida por el script ControllerShowAddInfoGeneric para controlar si la info se debe ocultar
		//se oculta la informacion adicional:
		HideAllAdditionalInformation (false, true);
		//se desactiva el blinking del marcador:
		should_be_blinking = false;
		//luego se activa el tick en la interfaz:
		ImageFeedbackTick.enabled = true;
		yield return new WaitForSeconds (2.0f);
		Debug.Log ("Se va a llamar el metodo de continuar despues de la rutina");
		//se desactiva el tick y se llama al metodo para redireccionar a la interfaz:
		ImageFeedbackTick.enabled = false;
		hide_Add_Info_To_ShowTick = false; //esta variable es leida por el script ControllerShowAddInfoGeneric
		this.onClickSelectBtn (interface_going_from);

	} //cierra rutina ShowTickOnInterface


	
	/// <summary>
	/// Method for loading images for showing the additional information
	/// This method is called from the AppManager from method onSingleTapped() when the user clicks on the interface
	/// The method loads the images that should be shown for each button. 
	/// Remember that the paths to images are loaded from each marker when it is tracked
	/// </summary>
	public void PrepareAdditionalIcons(){
		Debug.LogError ("Prepara carga de iconos en ControllerBlinkingAddInfo - TUTORIAL 2******");
		
		if (info_add_select_button_enable) {
			image_button_select_sprite = Resources.Load<Sprite> (image_for_button_select);
			info_additional_button_select.GetComponent<UnityEngine.UI.Image>().sprite = image_button_select_sprite;
		}
		
		if (info_add_button_one_enable) {
			info_additional_button_one.GetComponent<UnityEngine.UI.Image> ().sprite = Resources.Load<Sprite> (image_for_button_one);
			//agregando el texto que se debe mostrar en la informacion adicional para este elemento:
			info_additional_button_one.GetComponent<ControllerShowAddInfo>().TextToShow = Resources.Load<TextAsset>(text_add_info_btn_one).text;
		}
		
		if (info_add_button_two_enable) {
			info_additional_button_two.GetComponent<UnityEngine.UI.Image> ().sprite = Resources.Load<Sprite> (image_for_button_two);
			info_additional_button_two.GetComponent<ControllerShowAddInfo> ().TextToShow = Resources.Load<TextAsset> (text_add_info_btn_two).text;
		}
		
		if (info_add_button_three_enable) {
			info_additional_button_three.GetComponent<UnityEngine.UI.Image> ().sprite = Resources.Load<Sprite> (image_for_button_three);
			info_additional_button_three.GetComponent<ControllerShowAddInfo> ().TextToShow = Resources.Load<TextAsset> (text_add_info_btn_three).text;
		}

		
		if (info_add_button_four_enable) {
			info_additional_button_four.GetComponent<UnityEngine.UI.Image> ().sprite = Resources.Load<Sprite> (image_for_button_four);
			info_additional_button_four.GetComponent<ControllerShowAddInfo> ().TextToShow = Resources.Load<TextAsset> (text_add_info_btn_four).text;
		}
		
		if (info_add_button_five_enable) {
			info_additional_button_five.GetComponent<UnityEngine.UI.Image> ().sprite = Resources.Load<Sprite> (image_for_button_five);
			info_additional_button_five.GetComponent<ControllerShowAddInfo> ().TextToShow = Resources.Load<TextAsset> (text_add_info_btn_five).text;
		}
		
		if (info_add_button_six_enable) {
			info_additional_button_six.GetComponent<UnityEngine.UI.Image> ().sprite = Resources.Load<Sprite> (image_for_button_six);
			info_additional_button_six.GetComponent<ControllerShowAddInfo> ().TextToShow = Resources.Load<TextAsset> (text_add_info_btn_six).text;
		}
		
		if (info_add_button_seven_enable) {
			info_additional_button_seven.GetComponent<UnityEngine.UI.Image> ().sprite = Resources.Load<Sprite> (image_for_button_seven);
			info_additional_button_seven.GetComponent<ControllerShowAddInfo> ().TextToShow = Resources.Load<TextAsset> (text_add_info_btn_seven).text;
		}
		
		if (info_add_button_eight_enable) {
			info_additional_button_eight.GetComponent<UnityEngine.UI.Image> ().sprite = Resources.Load<Sprite> (image_for_button_eight);
			info_additional_button_eight.GetComponent<ControllerShowAddInfo> ().TextToShow = Resources.Load<TextAsset> (text_add_info_btn_eight).text;
		}

		//preparando el texto que se mostrara en el feedback:
		//cargando el texto del feedback:
		controller_feedback.TextToShowInFeedback = Resources.Load<TextAsset> (feedback_info_text_path).text;
		
	}// cierra metodo prepareAdditionalInfo

	/// <summary>
	/// Shows the additional incons. This method is called from the AppManager Interface in the method OnSingleTapped
	/// And is used to show the additional information when the user touches the interface
	/// 
	/// </summary>
	/// <param name="tutorial_phase1">If set to <c>true</c> tutorial_phase1.</param>
	/// <param name="tutorial_phase2">If set to <c>true</c> tutorial_phase2.</param>
	public void ShowAdditionalIncons(bool tutorial_phase1, bool tutorial_phase2){
		Debug.LogError ("Metodo de muestra info adicional tutorial1=" + tutorial_phase1 + " TUTORIAL2 =" + tutorial_phase2);
		Debug.Log ("El ID del marcador desde el que se carga la info es: " + marker_id_loading_from);

		if (tutorial_phase1 && markerFound && marker_id_loading_from == 1) {
			texto_btn_seleccionar_tutorial = GameObject.Find ("text_tutorial_select_button");
			texto_btn_seleccionar_tutorial.GetComponent<Text> ().enabled = true;
			
			img_btn_seleccionar_tutorial = GameObject.Find ("arrow_left");
			img_btn_seleccionar_tutorial.GetComponent<UnityEngine.UI.Image> ().enabled = true;
			is_add_info_displayed = true;
		} else if (tutorial_phase2 && markerFound && marker_id_loading_from == 16) {
			texto_btn_seleccionar_tutorial = GameObject.Find ("text_tutorial_addInfo_button");
			texto_btn_seleccionar_tutorial.GetComponent<Text> ().enabled = true;
			
			img_btn_seleccionar_tutorial = GameObject.Find ("arrow_left_addInfo");
			img_btn_seleccionar_tutorial.GetComponent<UnityEngine.UI.Image> ().enabled = true;
			is_add_info_displayed = true;
		}
		
		if (info_add_select_button_enable && markerFound) {
			info_additional_button_select.GetComponent<UnityEngine.UI.Image>().enabled = true;
			Debug.LogError("Se esta activando el boton SELECT");
			info_additional_button_select.onClick.AddListener(()=>{ActionButton_SelectElement();}); 
		}

		//los botones se activan siempre y cuando el marcador este siendo detectado:
		if (markerFound) {
			if (info_add_button_one_enable)
				info_additional_button_one.GetComponent<UnityEngine.UI.Image> ().enabled = true;
		
			if (info_add_button_two_enable)
				info_additional_button_two.GetComponent<UnityEngine.UI.Image> ().enabled = true;
		
			if (info_add_button_three_enable)
				info_additional_button_three.GetComponent<UnityEngine.UI.Image> ().enabled = true;
		
			if (info_add_button_four_enable)
				info_additional_button_four.GetComponent<UnityEngine.UI.Image> ().enabled = true;
		
			if (info_add_button_five_enable)
				info_additional_button_five.GetComponent<UnityEngine.UI.Image> ().enabled = true;
		
			if (info_add_button_six_enable)
				info_additional_button_six.GetComponent<UnityEngine.UI.Image> ().enabled = true;
		
			if (info_add_button_seven_enable)
				info_additional_button_seven.GetComponent<UnityEngine.UI.Image> ().enabled = true;
		
			if (info_add_button_eight_enable)
				info_additional_button_eight.GetComponent<UnityEngine.UI.Image> ().enabled = true;

			//setting the varible in true when the information has been displayed:
			is_add_info_displayed = true;
		
		}
			
	} //cierra ShowAdditionalIncons
	
	public void HideAllAdditionalInformation(bool tutorial_phase1, bool tutorial_phase2){
		
		if (tutorial_phase1 && !markerFound) {
			texto_btn_seleccionar_tutorial = GameObject.Find ("text_tutorial_select_button");
			texto_btn_seleccionar_tutorial.GetComponent<Text> ().enabled = false;
			
			img_btn_seleccionar_tutorial = GameObject.Find ("arrow_left");
			img_btn_seleccionar_tutorial.GetComponent<UnityEngine.UI.Image> ().enabled = false;
			//se vuelve false la variable que indica que la informacion no se esta mostrando
			is_add_info_displayed = false;
			
		} else if (tutorial_phase2 && !markerFound) {
			texto_btn_seleccionar_tutorial = GameObject.Find ("text_tutorial_addInfo_button");
			texto_btn_seleccionar_tutorial.GetComponent<Text> ().enabled = false;
			
			img_btn_seleccionar_tutorial = GameObject.Find ("arrow_left_addInfo");
			img_btn_seleccionar_tutorial.GetComponent<UnityEngine.UI.Image> ().enabled = false;
			is_add_info_displayed = false;

			if(controller_feedback.feedback_info_displayed)
				controller_feedback.HideFeedback();
		}
		
		
		if (info_add_select_button_enable) {
			info_additional_button_select.GetComponent<UnityEngine.UI.Image>().enabled = false;
			Debug.LogError("Se esta desactivando el boton SELECT");
		}

		if (info_add_button_one_enable) {
			info_additional_button_one.GetComponent<UnityEngine.UI.Image>().enabled = false;
		}
		//setting the variable that indicates that the add info is not displayed:
		is_add_info_displayed = false;

	}
	
	public void ActionButton_SelectElement(){

		//Evaluando si el marcador que se esta detectando corresponde al marcador que se debe escanear en esta
		//fase del proceso:
		Debug.Log ("ordenes que controla esta interfaz: " + ordenes[0]);
		correct_order = false;
		if (markerFound) {
			markers = GameObject.FindGameObjectsWithTag ("MarkerObjectScene");
			foreach (GameObject mark in markers) {
				markerHandler = mark.GetComponent<DefaultTrackableEventHandler> ();
				if(markerHandler.tracked){
					ordenes_marker = markerHandler.order;
					Debug.Log("El primer orden del MarkerDetectado es: " + ordenes_marker[0]);
					for(cont_ordenes = 0; cont_ordenes < ordenes.Length; cont_ordenes++){
						for(cont_ordenes_marker=0; cont_ordenes_marker < ordenes_marker.Length; cont_ordenes_marker++){
							if(ordenes[cont_ordenes]==ordenes_marker[cont_ordenes_marker]){
								Debug.Log("ORDEN CORRECTO: " + ordenes[cont_ordenes] +" Y "+ ordenes_marker[cont_ordenes_marker]);
								correct_order = true;
								break;
							}
						}
					}
				}
			}
		}

		if (correct_order) {
			//Se inicia esta corrutina para poder mostrar el tick en la interfaz
			//Despues de que termina la corrutina se llama al evento click del boton que redirecciona a la proxima interfaz:
			//es decir el llamado a la funcion se encuentra dentro de la siguiente corrutina:
			StartCoroutine ("ShowTickOnInterface");
			//this.onClickSelectBtn(interface_going_from);
		} else {
			//modificacion del 29/09/2015 para registrar la interfaz:
			string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
			if(ordenes[0] == 2){
				//registrando la navegacion de la interfaz:
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion E2:");
				NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (AppManager.manager.codigo_estudiante, fecha, "E2", "1", marker_id_loading_from);
			}else if(ordenes[0] == 3){
				//registrando la navegacion de la interfaz:
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion E3:");
				NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (AppManager.manager.codigo_estudiante, fecha, "E3", "1", marker_id_loading_from);
			} else if (ordenes[0] == 4){
				//registrando la navegacion de la interfaz:
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion E4:");
				NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (AppManager.manager.codigo_estudiante, fecha, "E4", "1", marker_id_loading_from);
			}
			controller_feedback.ShowFeedback ();
		}
	}
	
	public void ImageClickAction(){
		Debug.LogError ("SE HA LLAMADO AL EVENTO CLICK PEDIR AYUDA!!!");

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		if(ordenes[0] == 2){
			//registrando la navegacion de la interfaz:
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion H2:");
			NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (AppManager.manager.codigo_estudiante, fecha, "H2", "1", marker_id_loading_from);
		}else if(ordenes[0] == 3){
			//registrando la navegacion de la interfaz:
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion H3:");
			NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (AppManager.manager.codigo_estudiante, fecha, "H3", "1", marker_id_loading_from);
		} else if (ordenes[0] == 4){
			//registrando la navegacion de la interfaz:
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion H4:");
			NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (AppManager.manager.codigo_estudiante, fecha, "H4", "1", marker_id_loading_from);
		}
		touch_over_texture = true;
		StartCoroutine(ChangeRealImageForMarkerImage());
	}

	/// <summary>
	/// Hides the tutorial information. This method hides the information of the tutorial when one of the 
	/// buttons of additional information is clicked.
	/// </summary>
	public void HideTutorialInformation(){

		texto_btn_seleccionar_tutorial = GameObject.Find ("text_tutorial_addInfo_button");
		texto_btn_seleccionar_tutorial.GetComponent<Text> ().enabled = false;
		
		img_btn_seleccionar_tutorial = GameObject.Find ("arrow_left_addInfo");
		img_btn_seleccionar_tutorial.GetComponent<UnityEngine.UI.Image> ().enabled = false;
		is_add_info_displayed = false;
	}
	
	/*
	public void onClickCanvasAreaInterface(){
		Debug.LogError ("CANVAS: Se llama a Mostrar Informacion Adicional");
		if (showAdditionalInformation && markerFound) {
			PrepareAdditionalIcons ();
			ShowAdditionalIncons(true,false);

		}


	}*/
	


}
