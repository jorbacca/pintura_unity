using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Vuforia;

public class ControllerBlinkingARGeneric : MonoBehaviour {

	//variable for externally controlling if this texture should be start blinking
	public bool should_be_blinking;
	//variable for controlling marker blinking:
	private bool blink;
	//variable para cargar la imagen del marcador que se mostrara en cambio del objeto real cuando el estudiante
	//hace tap sobre la textura
	private Sprite marker_image;
	//variable para cargar la imagen real que deben ver los estudiantes (es decir el icono que representa la herramienta o producto que deben usar):
	private Sprite real_image;
	//variable para cargar la segunda imagen de la mano haciendo touch para el tutorial:
		
	public bool markerFound;
	//variable para almacenar la referencia a la imagen que se debe cargar:
	public string image_marker_path;
	//variable de referencia a la imagen real que se debe mostrar:
	public string image_marker_real_path;

	//variable para controlar el tick de feedback positivo:
	public UnityEngine.UI.Image ImageFeedbackTick;
	//variable que controla si la informacion adicional se debe ocultar porque se va a mostrar el tick en la interfaz:
	public bool hide_Add_Info_To_ShowTick;

	//variable temporal para cargar el texto que se muestra en el feedback:
	private TextAsset text_feedback_loaded;

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


	// Use this for initialization
	void Start () {
		Debug.LogError ("llamado al start de ControllerBlinking");
		should_be_blinking = true;
		blink = false;
		
		if (object_image_marker != null) {
			marker_image = Resources.Load<Sprite>(image_marker_real_path);
			object_image_marker.GetComponent<UnityEngine.UI.Image>().enabled = false;
			object_image_marker.GetComponent<UnityEngine.UI.Image>().sprite = marker_image;
		}
				
		//cargando el sprite de la imagen real
		real_image = Resources.Load<Sprite>(image_marker_path);
		
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
		Debug.LogError ("Prepara carga de iconos en ControllerBlinkingARGeneric");
		
		if (info_add_select_button_enable) {
			Debug.Log("Ingresa a la carga de datos el boton SELECT en ControllerBlinkingArGeneric.PrepareAdditionalIcons");
			image_button_select_sprite = Resources.Load<Sprite> (image_for_button_select);
			info_additional_button_select.GetComponent<UnityEngine.UI.Image>().sprite = image_button_select_sprite;
			Debug.Log("Finaliza la carga de datos el boton select en ControllerBlinkingArGeneric.PrepareAdditionalIcons");
		}
		
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
		
		//preparando el texto que se mostrara en el feedback:
		//cargando el texto del feedback:
		Debug.Log (" ---> Se va a cargar el Feedback en ControllerBlinkingARGeneric....");
		text_feedback_loaded = Resources.Load<TextAsset> (feedback_info_text_path);
		if (text_feedback_loaded != null)
			controller_feedback.TextToShowInFeedback = text_feedback_loaded.text;
		else 
			controller_feedback.TextToShowInFeedback = "No";
		Debug.Log (" YA Se ha  CARGADO EL FEEDBACK en ControllerBlinkingARGeneric....");
		
	}// cierra metodo prepareAdditionalInfo
	
	/// <summary>
	/// Shows the additional incons. This method is called from the AppManager Interface in the method OnSingleTapped
	/// And is used to show the additional information when the user touches the interface
	/// 
	/// </summary>
	public void ShowAdditionalIncons(){
		Debug.LogError ("Metodo de muestra info adicional en ControllerBlinkingARGeneric.ShowAdditionalIncons");

		Debug.Log ("El ID del marcador desde el que se carga la info es: " + marker_id_loading_from);
		
			
		//los botones se activan siempre y cuando el marcador este siendo detectado:
		if (markerFound) {
			Debug.Log("El marker esta siendo detectado y se va a cargar la info adicional");

			if (info_add_select_button_enable) {
				info_additional_button_select.GetComponent<UnityEngine.UI.Image>().enabled = true;
				Debug.LogError("Se esta activando el boton SELECT");
				info_additional_button_select.onClick.AddListener(()=>{ActionButton_SelectElement();}); 
			}

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

			Debug.Log("Finaliza la carga de la info adicional");
		}
		
	} //cierra ShowAdditionalIncons
	
	public void HideAllAdditionalInformation(){
		
		Debug.Log ("Ingresa a HideAllAdditionalInformation de ControllerBlinkingARGeneric");

		if(controller_feedback.feedback_info_displayed)
				controller_feedback.HideFeedback();

		Debug.Log ("Despues de ocultar el feedback en ControllerBlinkingARGeneric");
				
		if (info_add_select_button_enable) {
			info_additional_button_select.GetComponent<UnityEngine.UI.Image>().enabled = false;
			Debug.LogError("Se esta desactivando el boton SELECT");
		}
		
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
			//Edicion 16_09_2015:
			//Voy a agregar una variable adicional que se modifique en el AppManager para controlar
			//el marcador que se acaba de seleccionar y determinar si el siguiente marcador es valido o no
			//esto solamente se utiliza en la fase de matizado cuando se quiere validar si el orden de los
			//discos es correcto. Por ejemplo comenzar por P80 y seguir por P180 no es correcto:
			if (validate_sequence_of_markers) {
				Debug.Log ("ControllerBlinkingARGeneric: Ingresa a validar secuencia de marcadores con previous=" + AppManager.manager.last_markerid_scanned + ",actual=" + marker_id_loading_from);
				//preguntanto si el ultimo marcador escaneado es igual al (previous_marker_id) que valida esta interfaz:
				if (AppManager.manager.last_markerid_scanned == this.previous_marker_id) {
					Debug.Log ("ControllerBlinkingARGeneric: El marcador anterior corresponde con el que valida esta secuencia!");
					//se pregunta si el marcador que se escanea ahora es el esperado en la secuencia como next_marker
					if (marker_id_loading_from == this.next_marker_id) {
						Debug.Log ("ControllerBlinkingARGeneric: La secuencia de marcadores es correcta!!!");
						string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
						NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (AppManager.manager.codigo_estudiante, fecha, "S20", "12", marker_id_loading_from);
						StartCoroutine ("ShowTickOnInterface");
					} else {
						string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
						NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (AppManager.manager.codigo_estudiante, fecha, "E20", "12", marker_id_loading_from);
						controller_feedback.ShowFeedback ();
					}
				}
			} else {
				Debug.Log ("ControllerBlinkingARGeneric: Guardando el ID del marcador en el AppManager: marker_id=" + marker_id_loading_from);
				//esta variable almacena en el AppManager el ID del marcador que se acaba de escanear para 
				//que en un proximo paso se pueda saber cual fue el que se escaneo antes:
				//pero aqui se pregunta que si se ha escaneado el marcador 30 o el 32 entonces se debe guardar alguno de los dos IDs de lo contrario no:
				if (marker_id_loading_from == 30 || marker_id_loading_from == 32)
					AppManager.manager.last_markerid_scanned = marker_id_loading_from;
				
				//Se inicia esta corrutina para poder mostrar el tick en la interfaz
				//Despues de que termina la corrutina se llama al evento click del boton que redirecciona a la proxima interfaz:
				//es decir el llamado a la funcion se encuentra dentro de la siguiente corrutina:
				StartCoroutine ("ShowTickOnInterface");
			}


		} else {
			string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
			if(ordenes[0] == 5){
				//registrando la navegacion de la interfaz:
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion E5");
				NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (AppManager.manager.codigo_estudiante, fecha, "E5", "2", marker_id_loading_from);
			}else if (ordenes[0] == 6){
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion E6");
				NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (AppManager.manager.codigo_estudiante, fecha, "E6", "2", marker_id_loading_from);
			}else if (ordenes[0] == 7){
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion E7");
				NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (AppManager.manager.codigo_estudiante, fecha, "E7", "3", marker_id_loading_from);
			}else if (ordenes[0] == 8){
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion E8");
				NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (AppManager.manager.codigo_estudiante, fecha, "E4", "5", marker_id_loading_from);
			}else if (ordenes[0] == 9){
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion E9");
				NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (AppManager.manager.codigo_estudiante, fecha, "E9", "5", marker_id_loading_from);
			}else if (ordenes[0] == 10){
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion E10");
				NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (AppManager.manager.codigo_estudiante, fecha, "E10", "5", marker_id_loading_from);
			}else if (ordenes[0] == 11){
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion E11");
				NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (AppManager.manager.codigo_estudiante, fecha, "E11", "7", marker_id_loading_from);
			}else if (ordenes[0] == 12){
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion E12");
				NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (AppManager.manager.codigo_estudiante, fecha, "E12", "7", marker_id_loading_from);
			}else if (ordenes[0] == 13){
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion E13");
				NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (AppManager.manager.codigo_estudiante, fecha, "E13", "9", marker_id_loading_from);
			}else if (ordenes[0] == 14){
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion E14");
				NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (AppManager.manager.codigo_estudiante, fecha, "E14", "9", marker_id_loading_from);
			}else if (ordenes[0] == 15){
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion E15");
				NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (AppManager.manager.codigo_estudiante, fecha, "E15", "10", marker_id_loading_from);
			}else if (ordenes[0] == 16){
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion E16");
				NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (AppManager.manager.codigo_estudiante, fecha, "E16", "10", marker_id_loading_from);
			}else if (ordenes[0] == 17){
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion E17");
				NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (AppManager.manager.codigo_estudiante, fecha, "E17", "11", marker_id_loading_from);
			}else if (ordenes[0] == 18){
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion E18");
				NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (AppManager.manager.codigo_estudiante, fecha, "E18", "11", marker_id_loading_from);
			}else if (ordenes[0] == 19){
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion E19");
				NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (AppManager.manager.codigo_estudiante, fecha, "E19", "11", marker_id_loading_from);
			}else if (ordenes[0] == 20){
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion E21"); //Aca salta en un numero adicional el valor del id: E21 porque mas arriba ya se usa el E20
				NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (AppManager.manager.codigo_estudiante, fecha, "E21", "12", marker_id_loading_from);
			}else if (ordenes[0] == 21){
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion E22"); //Aca salta en un numero adicional el valor del id: E21 porque mas arriba ya se usa el E20
				NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (AppManager.manager.codigo_estudiante, fecha, "E22", "12", marker_id_loading_from);
			}else if (ordenes[0] == 22){
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion E23"); //Aca salta en un numero adicional el valor del id: E21 porque mas arriba ya se usa el E20
				NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (AppManager.manager.codigo_estudiante, fecha, "E23", "13", marker_id_loading_from);
			}else if (ordenes[0] == 23){
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion E24"); //Aca salta en un numero adicional el valor del id: E21 porque mas arriba ya se usa el E20
				NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (AppManager.manager.codigo_estudiante, fecha, "E24", "13", marker_id_loading_from);
			}else if (ordenes[0] == 24){
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion E25"); //Aca salta en un numero adicional el valor del id: E21 porque mas arriba ya se usa el E20
				NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (AppManager.manager.codigo_estudiante, fecha, "E25", "14", marker_id_loading_from);
			}else if (ordenes[0] == 25){
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion E26"); //Aca salta en un numero adicional el valor del id: E21 porque mas arriba ya se usa el E20
				NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (AppManager.manager.codigo_estudiante, fecha, "E26", "14", marker_id_loading_from);
			}


			//se activa el feedback para mostrar que nos e ha escaneado el marcador correcto
			controller_feedback.ShowFeedback ();

		}
	}
	
	public void ImageClickAction(){
		Debug.LogError ("SE HA LLAMADO AL EVENTO CLICK PEDIR AYUDA!!!");

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		if(ordenes[0] == 5){
			//registrando la navegacion de la interfaz:
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion H5");
			NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (AppManager.manager.codigo_estudiante, fecha, "H5", "2", marker_id_loading_from);
		}else if (ordenes[0] == 6){
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion H6");
			NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (AppManager.manager.codigo_estudiante, fecha, "H6", "2", marker_id_loading_from);
		}else if (ordenes[0] == 7){
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion H7");
			NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (AppManager.manager.codigo_estudiante, fecha, "H7", "3", marker_id_loading_from);
		}else if (ordenes[0] == 8){
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion H8");
			NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (AppManager.manager.codigo_estudiante, fecha, "H4", "5", marker_id_loading_from);
		}else if (ordenes[0] == 9){
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion H9");
			NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (AppManager.manager.codigo_estudiante, fecha, "H9", "5", marker_id_loading_from);
		}else if (ordenes[0] == 10){
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion H10");
			NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (AppManager.manager.codigo_estudiante, fecha, "H10", "5", marker_id_loading_from);
		}else if (ordenes[0] == 11){
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion H11");
			NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (AppManager.manager.codigo_estudiante, fecha, "H11", "7", marker_id_loading_from);
		}else if (ordenes[0] == 12){
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion H12");
			NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (AppManager.manager.codigo_estudiante, fecha, "H12", "7", marker_id_loading_from);
		}else if (ordenes[0] == 13){
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion H13");
			NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (AppManager.manager.codigo_estudiante, fecha, "H13", "9", marker_id_loading_from);
		}else if (ordenes[0] == 14){
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion H14");
			NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (AppManager.manager.codigo_estudiante, fecha, "H14", "9", marker_id_loading_from);
		}else if (ordenes[0] == 15){
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion H15");
			NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (AppManager.manager.codigo_estudiante, fecha, "H15", "10", marker_id_loading_from);
		}else if (ordenes[0] == 16){
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion H16");
			NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (AppManager.manager.codigo_estudiante, fecha, "H16", "10", marker_id_loading_from);
		}else if (ordenes[0] == 17){
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion H17");
			NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (AppManager.manager.codigo_estudiante, fecha, "H17", "11", marker_id_loading_from);
		}else if (ordenes[0] == 18){
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion H18");
			NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (AppManager.manager.codigo_estudiante, fecha, "H18", "11", marker_id_loading_from);
		}else if (ordenes[0] == 19){
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion H19");
			NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (AppManager.manager.codigo_estudiante, fecha, "H19", "11", marker_id_loading_from);
		}else if (ordenes[0] == 20){
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion H20"); 
			NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (AppManager.manager.codigo_estudiante, fecha, "H20", "12", marker_id_loading_from);
		}else if (ordenes[0] == 21){
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion H21"); 
			NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (AppManager.manager.codigo_estudiante, fecha, "H21", "12", marker_id_loading_from);
		}else if (ordenes[0] == 22){
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion H22"); 
			NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (AppManager.manager.codigo_estudiante, fecha, "H22", "13", marker_id_loading_from);
		}else if (ordenes[0] == 23){
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion H23"); 
			NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (AppManager.manager.codigo_estudiante, fecha, "H23", "13", marker_id_loading_from);
		}else if (ordenes[0] == 24){
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion H24"); 
			NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (AppManager.manager.codigo_estudiante, fecha, "H24", "14", marker_id_loading_from);
		}else if (ordenes[0] == 25){
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion H25"); 
			NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (AppManager.manager.codigo_estudiante, fecha, "H25", "14", marker_id_loading_from);
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
