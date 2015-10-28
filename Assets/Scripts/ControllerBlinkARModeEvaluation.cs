using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Vuforia;

public class ControllerBlinkARModeEvaluation : MonoBehaviour {
	//variable for externally controlling if this texture should be start blinking
	public bool should_be_blinking;
	//variable for controlling marker blinking:
	private bool blink;

	//variable para cargar la imagen del icono de pregunta:
	private Sprite question_mark_icon;
	//variable para cargar la imagen real que deben ver los estudiantes (es decir el icono que representa la herramienta o producto que deben usar):
	private Sprite real_image_help_icon;
	//variable para cargar la segunda imagen de la mano haciendo touch para el tutorial:
	
	public bool markerFound;
	//variable para almacenar la referencia a la imagen que se debe cargar:
	public string question_mark_icon_path;
	//variable de referencia a la imagen real que se debe mostrar:
	public string image_help_real_path;
	
	//variable para controlar el tick de feedback positivo:
	public UnityEngine.UI.Image ImageFeedbackTick;
	//variable que controla si la informacion adicional se debe ocultar porque se va a mostrar el tick en la interfaz:
	public bool hide_Add_Info_To_ShowTick;
	
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

	//referencia al objeto que muestra el texto de la guia del numero de producto o herramienta que se debe buscar
	public Text guia_producto_herramienta;

	//texto que se debe mostrar en la guia del producto o herramienta:
	public string texto_guia_producto;

	//variables para controlar los objetos que deben estar en blinking:
	public GameObject object_image_marker;
	//public GameObject object_text;
	
	//variables para referenciar los botones de informacion adicional:
	public Button info_additional_button_select;
	/*
	public Button info_additional_button_one;
	public Button info_additional_button_two;
	public Button info_additional_button_three;
	public Button info_additional_button_four;
	public Button info_additional_button_five;
	public Button info_additional_button_six;
	public Button info_additional_button_seven;
	public Button info_additional_button_eight;
	*/
	
	//variables que controlan si los botones de informacion adicional se deben desplegar
	public bool info_add_select_button_enable;
	/*
	public bool info_add_button_one_enable;
	public bool info_add_button_two_enable;
	public bool info_add_button_three_enable;
	public bool info_add_button_four_enable;
	public bool info_add_button_five_enable;
	public bool info_add_button_six_enable;
	public bool info_add_button_seven_enable;
	public bool info_add_button_eight_enable;
	*/
	
	//variables que contienen las rutas a las imagenes que se deben mostrar en los diferentes botones:
	public string image_for_button_select;
	/*
	public string image_for_button_one;
	public string image_for_button_two;
	public string image_for_button_three;
	public string image_for_button_four;
	public string image_for_button_five;
	public string image_for_button_six;
	public string image_for_button_seven;
	public string image_for_button_eight;
	*/
	
	//variables que almacenan los sprites para mostrar las imagenes:
	private Sprite image_button_select_sprite;
	/*
	private Sprite image_button_one_sprite;
	private Sprite image_button_two_sprite;
	private Sprite image_button_three_sprite;
	private Sprite image_button_four_sprite;
	private Sprite image_button_five_sprite;
	private Sprite image_button_six_sprite;
	private Sprite image_button_seven_sprite;
	private Sprite image_button_eight_sprite;
	*/
	
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
	
	//variable para cargar los textos desde los archivos:
	private TextAsset texto_asset;
	
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

	//variable que controla si se debe hacer validacion de una secuencia de 2 marcadores:
	public bool validate_sequence_of_markers = false;

	//variable que almacena el ID del primer marcador de la secuencia:
	public int previous_marker_id; //esta variable se asigna desde el AppManager cuando se instancia la interfaz
	//variable que almacena el ID del segundo marcador de la secuencia:
	public int next_marker_id; //esta variable se asigna desde el AppManager cuando se instancia la interfaz

	//variable temporal para cargar el texto que se muestra en el feedback:
	private TextAsset text_feedback_loaded;
	
		
	// Use this for initialization
	void Start () {
		Debug.LogError ("llamado al start de ControllerBlinking");
		should_be_blinking = true;
		blink = false;
		
		if (object_image_marker != null) {
			question_mark_icon = Resources.Load<Sprite>(question_mark_icon_path);
			object_image_marker.GetComponent<UnityEngine.UI.Image>().enabled = true;
			object_image_marker.GetComponent<UnityEngine.UI.Image>().sprite = question_mark_icon;
		}

		if (guia_producto_herramienta != null) {
			guia_producto_herramienta.text = texto_guia_producto;
		}
		
		//cargando el sprite de la imagen real
		real_image_help_icon = Resources.Load<Sprite>(image_help_real_path);
		
		//inicializando la variable touch sobre la textura:
		touch_over_texture = false;
		
		//inicializando variable que indica que el marcador ha sido encontrado:
		markerFound = false;
		
		//inicializando la variable que indica que el orden si es correcto:
		correct_order = false;
		
		//inicializando la referencia al controlador del feedback:
		controller_feedback = FeedbackObjectReference.GetComponent<ControllerFeedbackScript> ();
		
		//inicializando la imagen que se muestra como feedback positivo (tick en la interfaz):
		if (ImageFeedbackTick != null)
			ImageFeedbackTick.enabled = false;
		
		//inicializando la variable que indica si la info adicional se debe ocultar 
		hide_Add_Info_To_ShowTick = false;
	}
	
	// Update is called once per frame
	void Update () {

		//validando si touch_over_texture es false lo que indica que la corrutina ya se ejecuto para detenerla:
		if(!touch_over_texture)
			StopCoroutine(ChangeRealImageForMarkerImage());

		if(markerFound){
			//object_image_marker.GetComponent<Image>().sprite = handImagetouch;
			//object_text.GetComponent<Text>().text = text_to_show_blinking_touch;
			showSelectButton = true;
			if(is_add_info_displayed)
				should_be_blinking = false;	
		}else { //me parece que este else sobra porque aqui la imagen no debe cambiar
			//object_image_marker.GetComponent<Image>().sprite = question_mark_icon;
			//object_text.GetComponent<Text>().text = text_to_show_blinking;
			showSelectButton = false;
		}
		
		if (should_be_blinking) {
			
			//validando si no se ha inciado el proceso de blinking se inicia la corrutina
			/*
			if(!blink){
				Debug.LogError("Llama a corrutina MarkerBlinkingCameraView");
				blink=true;
				StartCoroutine("MarkerBlinkingCameraView");
			}*/
			
		}else {
			//Debug.LogError("Detiene corrutina MarkerBlinking");
			//StopCoroutine("MarkerBlinking");
			//if(blink)
			//	StopCoroutine("MarkerBlinkingCameraView");
			blink=false;
			//object_image_marker.GetComponent<UnityEngine.UI.Image>().enabled = false;
			
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

		object_image_marker.GetComponent<UnityEngine.UI.Image> ().sprite = real_image_help_icon;
		//object_text.GetComponent<Text> ().enabled = object_image_marker.GetComponent<Image> ().enabled;
		yield return new WaitForSeconds (3.0f);
		Debug.LogError ("Se cambia por real image: " + question_mark_icon);
		object_image_marker.GetComponent<UnityEngine.UI.Image> ().sprite = question_mark_icon;
		//object_text.GetComponent<Text> ().enabled = false;
		touch_over_texture = false;
		
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
		HideAllAdditionalInformation ();
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
		Debug.LogError ("Prepara carga de iconos en ControllerBlinkingAModeEvaluation");
		
		if (info_add_select_button_enable) {
			Debug.Log("Ingresa a la carga de datos el boton SELECT en ControllerBlinkingArGeneric.PrepareAdditionalIcons");
			image_button_select_sprite = Resources.Load<Sprite> (image_for_button_select);
			info_additional_button_select.GetComponent<UnityEngine.UI.Image>().sprite = image_button_select_sprite;
			Debug.Log("Finaliza la carga de datos el boton select en ControllerBlinkingArGeneric.PrepareAdditionalIcons");
		}
		/*
		if (info_add_button_one_enable) {
			Debug.Log("Ingresa a la carga de datos el boton ONE en ControllerBlinkingArGeneric.PrepareAdditionalIcons");
			info_additional_button_one.GetComponent<UnityEngine.UI.Image> ().sprite = Resources.Load<Sprite> (image_for_button_one);
			texto_asset = Resources.Load<TextAsset>(text_add_info_btn_one);
			if(texto_asset != null){
				info_additional_button_one.GetComponent<ControllerShowAddInfoGeneric>().TextToShow = texto_asset.text;
				Debug.Log("Se ha cargado el texto para el boton TWO...");
			} else info_additional_button_one.GetComponent<ControllerShowAddInfoGeneric>().TextToShow = "";
			Debug.Log("Finaliza la carga de datos el boton ONE en ControllerBlinkingArGeneric.PrepareAdditionalIcons");
		}
		
		if (info_add_button_two_enable) {
			Debug.Log("Ingresa a la carga de datos el boton TWO en ControllerBlinkingArGeneric.PrepareAdditionalIcons: " + image_for_button_two + " , " + text_add_info_btn_two);
			info_additional_button_two.GetComponent<UnityEngine.UI.Image> ().sprite = Resources.Load<Sprite> (image_for_button_two);
			Debug.Log ("Imagen cargada para boton TWO...");
			texto_asset = Resources.Load<TextAsset> (text_add_info_btn_two);
			if (texto_asset != null){
				info_additional_button_two.GetComponent<ControllerShowAddInfoGeneric> ().TextToShow = texto_asset.text;
				Debug.Log ("El texto_asset se ha cargado para boton TWO...");
			} else info_additional_button_two.GetComponent<ControllerShowAddInfoGeneric> ().TextToShow = "";
			Debug.Log("Finaliza la carga de datos el boton TWO en ControllerBlinkingArGeneric.PrepareAdditionalIcons");
		}
		
		if (info_add_button_three_enable) {
			Debug.Log("Ingresa a la carga de datos el boton THREE en ControllerBlinkingArGeneric.PrepareAdditionalIcons");
			info_additional_button_three.GetComponent<UnityEngine.UI.Image> ().sprite = Resources.Load<Sprite> (image_for_button_three);
			texto_asset = Resources.Load<TextAsset> (text_add_info_btn_three);
			if(texto_asset != null){
				info_additional_button_three.GetComponent<ControllerShowAddInfoGeneric> ().TextToShow = texto_asset.text;
			} else info_additional_button_three.GetComponent<ControllerShowAddInfoGeneric> ().TextToShow = "";
			Debug.Log("Finaliza la carga de datos el boton THREE en ControllerBlinkingArGeneric.PrepareAdditionalIcons");
		}
		
		
		if (info_add_button_four_enable) {
			Debug.Log("Ingresa a la carga de datos el boton FOUR en ControllerBlinkingArGeneric.PrepareAdditionalIcons");
			info_additional_button_four.GetComponent<UnityEngine.UI.Image> ().sprite = Resources.Load<Sprite> (image_for_button_four);
			texto_asset = Resources.Load<TextAsset> (text_add_info_btn_four);
			if(texto_asset != null){
				info_additional_button_four.GetComponent<ControllerShowAddInfoGeneric> ().TextToShow = texto_asset.text;
			} else info_additional_button_four.GetComponent<ControllerShowAddInfoGeneric> ().TextToShow = "";
			Debug.Log("Finaliza la carga de datos el boton FOUR en ControllerBlinkingArGeneric.PrepareAdditionalIcons");
		}
		
		if (info_add_button_five_enable) {
			Debug.Log("Ingresa a la carga de datos el boton FIVE en ControllerBlinkingArGeneric.PrepareAdditionalIcons");
			info_additional_button_five.GetComponent<UnityEngine.UI.Image> ().sprite = Resources.Load<Sprite> (image_for_button_five);
			texto_asset = Resources.Load<TextAsset> (text_add_info_btn_five);
			if(texto_asset != null){
				info_additional_button_five.GetComponent<ControllerShowAddInfoGeneric> ().TextToShow = texto_asset.text;
			} else info_additional_button_five.GetComponent<ControllerShowAddInfoGeneric> ().TextToShow = "";
			Debug.Log("Finaliza la carga de datos el boton FIVE en ControllerBlinkingArGeneric.PrepareAdditionalIcons");
		}
		
		if (info_add_button_six_enable) {
			Debug.Log("Finaliza la carga de datos el boton SIX en ControllerBlinkingArGeneric.PrepareAdditionalIcons");
			info_additional_button_six.GetComponent<UnityEngine.UI.Image> ().sprite = Resources.Load<Sprite> (image_for_button_six);
			texto_asset = Resources.Load<TextAsset> (text_add_info_btn_six);
			if(texto_asset!= null){
				info_additional_button_six.GetComponent<ControllerShowAddInfoGeneric> ().TextToShow = texto_asset.text;
			} else info_additional_button_six.GetComponent<ControllerShowAddInfoGeneric> ().TextToShow = "";
			Debug.Log("Finaliza la carga de datos el boton SIX en ControllerBlinkingArGeneric.PrepareAdditionalIcons");
		}
		
		if (info_add_button_seven_enable) {
			info_additional_button_seven.GetComponent<UnityEngine.UI.Image> ().sprite = Resources.Load<Sprite> (image_for_button_seven);
			info_additional_button_seven.GetComponent<ControllerShowAddInfoGeneric> ().TextToShow = Resources.Load<TextAsset> (text_add_info_btn_seven).text;
		}
		
		if (info_add_button_eight_enable) {
			info_additional_button_eight.GetComponent<UnityEngine.UI.Image> ().sprite = Resources.Load<Sprite> (image_for_button_eight);
			info_additional_button_eight.GetComponent<ControllerShowAddInfoGeneric> ().TextToShow = Resources.Load<TextAsset> (text_add_info_btn_eight).text;
		}
		*/

		//preparando el texto que se mostrara en el feedback:
		//cargando el texto del feedback:
		Debug.Log (" ---> Se va a cargar el Feedback en ControllerBlinkARModeEvaluation....");
		text_feedback_loaded = Resources.Load<TextAsset> (feedback_info_text_path);
		if (text_feedback_loaded != null)
			controller_feedback.TextToShowInFeedback = text_feedback_loaded.text;
		else 
			controller_feedback.TextToShowInFeedback = "No";
		Debug.Log (" YA Se ha  CARGADO EL FEEDBACK en ControllerBlinkARModeEvaluation....");
	
	}// cierra metodo prepareAdditionalInfo
	
	/// <summary>
	/// Shows the additional incons. This method is called from the AppManager Interface in the method OnSingleTapped
	/// And is used to show the additional information when the user touches the interface
	/// 
	/// </summary>
	public void ShowAdditionalIncons(){
		Debug.LogError ("Metodo de muestra info adicional en ControllerBlinkingARModeEvaluation.ShowAdditionalIncons");
		
		Debug.Log ("El ID del marcador desde el que se carga la info es: " + marker_id_loading_from);
		
		
		//los botones se activan siempre y cuando el marcador este siendo detectado:
		if (markerFound) {
			Debug.Log("El marker esta siendo detectado y se va a cargar la info adicional");
			
			if (info_add_select_button_enable) {
				info_additional_button_select.GetComponent<UnityEngine.UI.Image>().enabled = true;
				Debug.LogError("Se esta activando el boton SELECT");
				info_additional_button_select.onClick.AddListener(()=>{ActionButton_SelectElement();}); 
			}
			/*
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
			*/
			
			//setting the varible in true when the information has been displayed:
			is_add_info_displayed = true;
			
			Debug.Log("Finaliza la carga de la info adicional en ControllerBlinkingARModeEvaluation");
		}
		
	} //cierra ShowAdditionalIncons
	
	public void HideAllAdditionalInformation(){
		
		Debug.Log ("Ingresa a HideAllAdditionalInformation de ControllerBlinkingARModeEvaluation");
		
		if(controller_feedback.feedback_info_displayed)
			controller_feedback.HideFeedback();
		
		Debug.Log ("Despues de ocultar el feedback en ControllerBlinkingARModeEvaluation");
		
		if (info_add_select_button_enable) {
			info_additional_button_select.GetComponent<UnityEngine.UI.Image>().enabled = false;
			Debug.LogError("Se esta desactivando el boton SELECT en ControllerBlinkingARModeEvaluation");
		}
		/*
		if (info_add_button_one_enable) {
			info_additional_button_one.GetComponent<UnityEngine.UI.Image>().enabled = false;
		}
		
		if (info_add_button_two_enable)
			info_additional_button_two.GetComponent<UnityEngine.UI.Image> ().enabled = false;
		
		if (info_add_button_three_enable)
			info_additional_button_three.GetComponent<UnityEngine.UI.Image> ().enabled = false;
		
		if (info_add_button_four_enable)
			info_additional_button_four.GetComponent<UnityEngine.UI.Image> ().enabled = false;
		
		if (info_add_button_five_enable)
			info_additional_button_five.GetComponent<UnityEngine.UI.Image> ().enabled = false;
		
		if (info_add_button_six_enable)
			info_additional_button_six.GetComponent<UnityEngine.UI.Image> ().enabled = false;
		
		if (info_add_button_seven_enable)
			info_additional_button_seven.GetComponent<UnityEngine.UI.Image> ().enabled = false;
		
		if (info_add_button_eight_enable)
			info_additional_button_eight.GetComponent<UnityEngine.UI.Image> ().enabled = false;
		*/
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

			//Edicion 23_09_2015:
			//Voy a agregar una variable adicional que se modifique en el AppManager para controlar
			//el marcador que se acaba de seleccionar y determinar si el siguiente marcador es valido o no
			//esto solamente se utiliza en la fase de matizado cuando se quiere validar si el orden de los
			//discos es correcto. Por ejemplo comenzar por P80 y seguir por P180 no es correcto:
			if (validate_sequence_of_markers) {
				Debug.Log ("ControllerBlinkARModeEvaluation: Ingresa a validar secuencia de marcadores con previous=" + AppManager.manager.last_marker_id_evalmode + ",actual=" + marker_id_loading_from);
				//preguntanto si el ultimo marcador escaneado es igual al (previous_marker_id) que valida esta interfaz:
				if (AppManager.manager.last_marker_id_evalmode == this.previous_marker_id) {
					Debug.Log ("ControllerBlinkARModeEvaluation: El marcador anterior corresponde con el que valida esta secuencia!");
					//se pregunta si el marcador que se escanea ahora es el esperado en la secuencia como next_marker
					if (marker_id_loading_from == this.next_marker_id) {
						Debug.Log ("ControllerBlinkARModeEvaluation: La secuencia de marcadores es correcta!!!");
						string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
						Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
						NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "SE20", "12",marker_id_loading_from,"eval");
						StartCoroutine ("ShowTickOnInterface");
					} else {
						string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
						Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
						NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "EE21", "12",marker_id_loading_from,"eval");
						controller_feedback.ShowFeedback ();
					}
				}
			} else {
				Debug.Log ("ControllerBlinkARModeEvaluation: Guardando el ID del marcador en el AppManager: marker_id=" + marker_id_loading_from);
				//esta variable almacena en el AppManager el ID del marcador que se acaba de escanear para 
				//que en un proximo paso se pueda saber cual fue el que se escaneo antes:
				//pero aqui se pregunta que si se ha escaneado el marcador 30 o el 32 entonces se debe guardar alguno de los dos IDs de lo contrario no:
				if (marker_id_loading_from == 30 || marker_id_loading_from == 32)
					AppManager.manager.last_marker_id_evalmode = marker_id_loading_from;
				
				//Se inicia esta corrutina para poder mostrar el tick en la interfaz
				//Despues de que termina la corrutina se llama al evento click del boton que redirecciona a la proxima interfaz:
				//es decir el llamado a la funcion se encuentra dentro de la siguiente corrutina:
				StartCoroutine ("ShowTickOnInterface");
			}

		} else {
			string fecha = System.DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss");
			if(ordenes[0] == 1){
				//registrando la navegacion de la interfaz:
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion EE1");
				NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "EE1", "0", marker_id_loading_from,"eval");
			} else if (ordenes[0] == 2){
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion EE2");
				NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "EE2", "1", marker_id_loading_from,"eval");
			} else if (ordenes[0] == 3){
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion EE3");
				NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "EE3", "1", marker_id_loading_from,"eval");
			} else if (ordenes[0] == 4){
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion EE4");
				NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "EE4", "1", marker_id_loading_from,"eval");
			} else if (ordenes[0] == 5){
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion EE5");
				NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "EE5", "2", marker_id_loading_from,"eval");
			} else if (ordenes[0] == 6){
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion EE6");
				NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "EE6", "2", marker_id_loading_from,"eval");
			} else if (ordenes[0] == 7){
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion EE7");
				NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "EE7", "3", marker_id_loading_from,"eval");
			}else if (ordenes[0] == 8){
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion EE8");
				NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "EE8", "4", marker_id_loading_from,"eval");
			}else if (ordenes[0] == 9){
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion EE9");
				NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "EE9", "5", marker_id_loading_from,"eval");
			}else if (ordenes[0] == 10){
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion EE10");
				NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "EE10", "5", marker_id_loading_from,"eval");
			}else if (ordenes[0] == 11){
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion EE11");
				NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "EE11", "7", marker_id_loading_from,"eval");
			}else if (ordenes[0] == 12){
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion EE12");
				NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "EE12", "7", marker_id_loading_from,"eval");
			}else if (ordenes[0] == 13){
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion EE13");
				NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "EE13", "9", marker_id_loading_from,"eval");
			}else if (ordenes[0] == 14){
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion EE14");
				NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "EE14", "9", marker_id_loading_from,"eval");
			} else if (ordenes[0] == 15){
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion EE15");
				NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "EE15", "10", marker_id_loading_from,"eval");
			}else if (ordenes[0] == 16){
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion EE16");
				NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "EE16", "10", marker_id_loading_from,"eval");
			} else if (ordenes[0] == 17){
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion EE17");
				NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "EE17", "11", marker_id_loading_from,"eval");
			}else if (ordenes[0] == 18){
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion EE18");
				NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "EE18", "11", marker_id_loading_from,"eval");
			}else if (ordenes[0] == 19){
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion EE19");
				NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "EE19", "11", marker_id_loading_from,"eval");
			}else if (ordenes[0] == 20){
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion EE20");
				NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "EE20", "12", marker_id_loading_from,"eval");
			}else if (ordenes[0] == 21){
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion EE22");
				NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "EE22", "12", marker_id_loading_from,"eval");
			}else if (ordenes[0] == 22){
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion EE23");
				NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "EE23", "13", marker_id_loading_from,"eval");
			}else if (ordenes[0] == 23){
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion EE24");
				NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "EE24", "13", marker_id_loading_from,"eval");
			}else if (ordenes[0] == 24){
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion EE25");
				NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "EE25", "14", marker_id_loading_from,"eval");
			}else if (ordenes[0] == 25){
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion EE26");
				NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "EE26", "14", marker_id_loading_from,"eval");
			}

			controller_feedback.ShowFeedback ();
		}//cierra else
	}
	
	public void ImageClickAction(){
		Debug.LogError ("SE HA LLAMADO AL EVENTO CLICK DE SOLICITAR AYUDA");

		string fecha = System.DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss");
		if(ordenes[0] == 1){
			//registrando la navegacion de la interfaz:
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion HE1");
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "HE1", "0", marker_id_loading_from,"eval");
		} else if (ordenes[0] == 2){
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion HE2");
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "HE2", "1", marker_id_loading_from,"eval");
		} else if (ordenes[0] == 3){
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion HE3");
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "HE3", "1", marker_id_loading_from,"eval");
		} else if (ordenes[0] == 4){
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion HE4");
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "HE4", "1", marker_id_loading_from,"eval");
		} else if (ordenes[0] == 5){
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion HE5");
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "HE5", "2", marker_id_loading_from,"eval");
		} else if (ordenes[0] == 6){
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion HE6");
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "HE6", "2", marker_id_loading_from,"eval");
		} else if (ordenes[0] == 7){
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion HE7");
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "HE7", "3", marker_id_loading_from,"eval");
		}else if (ordenes[0] == 8){
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion HE8");
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "HE8", "4", marker_id_loading_from,"eval");
		}else if (ordenes[0] == 9){
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion HE9");
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "HE9", "5", marker_id_loading_from,"eval");
		}else if (ordenes[0] == 10){
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion HE10");
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "HE10", "5", marker_id_loading_from,"eval");
		}else if (ordenes[0] == 11){
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion HE11");
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "HE11", "7", marker_id_loading_from,"eval");
		}else if (ordenes[0] == 12){
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion HE12");
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "HE12", "7", marker_id_loading_from,"eval");
		}else if (ordenes[0] == 13){
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion HE13");
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "HE13", "9", marker_id_loading_from,"eval");
		}else if (ordenes[0] == 14){
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion HE14");
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "HE14", "9", marker_id_loading_from,"eval");
		} else if (ordenes[0] == 15){
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion HE15");
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "HE15", "10", marker_id_loading_from,"eval");
		}else if (ordenes[0] == 16){
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion HE16");
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "HE16", "10", marker_id_loading_from,"eval");
		} else if (ordenes[0] == 17){
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion HE17");
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "HE17", "11", marker_id_loading_from,"eval");
		}else if (ordenes[0] == 18){
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion HE18");
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "HE18", "11", marker_id_loading_from,"eval");
		}else if (ordenes[0] == 19){
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion HE19");
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "HE19", "11", marker_id_loading_from,"eval");
		}else if (ordenes[0] == 20){
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion HE20");
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "HE20", "12", marker_id_loading_from,"eval");
		}else if (ordenes[0] == 21){
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion HE21");
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "HE21", "12", marker_id_loading_from,"eval");
		}else if (ordenes[0] == 22){
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion HE22");
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "HE22", "13", marker_id_loading_from,"eval");
		}else if (ordenes[0] == 23){
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion HE23");
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "HE23", "13", marker_id_loading_from,"eval");
		}else if (ordenes[0] == 24){
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion HE24");
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "HE24", "14", marker_id_loading_from,"eval");
		}else if (ordenes[0] == 25){
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion HE25");
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "HE25", "14", marker_id_loading_from,"eval");
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
