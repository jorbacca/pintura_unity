using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuOfStepsPhaseTwoManagerEval : MonoBehaviour, IHasChangedInMenuStepsPhase2 {
	public string image_header_phase1;
	public string titulo;
	public string introduction_text_path;
	public Button btn_one_to_order;
	public Button btn_two_to_order;
	public Button btn_three_to_order;
	public Button btn_four_to_order;
	public Button btn_five_to_order;
	public Button btn_six_to_order;
	public Button btn_seven_to_order;
	public Button btn_eight_to_order;
	public Button regresar;
	
	//Referencia a los componentes Text de los botones:
	public Text text_btn_one_to_order;
	public Text text_btn_two_to_order;
	public Text text_btn_three_to_order;
	public Text text_btn_four_to_order;
	public Text text_btn_five_to_order;
	public Text text_btn_six_to_order;
	public Text text_btn_seven_to_order;
	public Text text_btn_eight_to_order;
	
	//rutas a las imagenes que se deben cargar (numeradas para leerse de izquierda a derecha en la interfaz):
	public string img_one_to_order;
	public string img_two_to_order;
	public string img_three_to_order;
	public string img_four_to_order;
	public string img_five_to_order;
	public string img_six_to_order;
	public string img_seven_to_order;
	public string img_eight_to_order;
	
	//Rutas a las imagenes de las fases para la guia de las fases:
	public string image_phase1_path;
	public string image_phase2_path;
	public string image_phase3_path;
	public string image_phase4_path;
	public string image_phase5_path;
	public string image_phase6_path;
	
	//Textos de los nombres de los pasos:
	public string string_btn_one_text;
	public string string_btn_two_text;
	public string string_btn_three_text;
	public string string_btn_four_text;
	public string string_btn_five_text;
	public string string_btn_six_text;
	public string string_btn_seven_text;
	public string string_btn_eight_text;
	
	//GameObjects para obtener el titulo y el objeto que muestra un texto de introduccion:
	private GameObject imagen_header;
	public GameObject titulo_object;
	public GameObject introduction_object;
	
	//GameObjects para obtener las referencias a los componentes Image que muestran las fases del proceso:
	public GameObject image_phase1_object;
	public GameObject image_phase2_object;
	public GameObject image_phase3_object;
	public GameObject image_phase4_object;
	public GameObject image_phase5_object;
	public GameObject image_phase6_object;
	
	
	//TextAsset para cargar desde archivo el contenido de la introduccion
	private TextAsset introduction_asset;
	
	//Sprites para configurar las imagenes de los botones
	private Sprite sprite_image_header_phase1;

	private Sprite image_button_sprite_uno;
	private Sprite image_button_sprite_dos;
	private Sprite image_button_sprite_tres;
	private Sprite image_button_sprite_cuatro;
	private Sprite image_button_sprite_cinco;
	private Sprite image_button_sprite_seis;
	private Sprite image_button_sprite_siete;
	private Sprite image_button_sprite_ocho;
	
	//Sprites para configurar las referencias a las imagenes:
	private Sprite image_phase1_sprite;
	private Sprite image_phase2_sprite;
	private Sprite image_phase3_sprite;
	private Sprite image_phase4_sprite;
	private Sprite image_phase5_sprite;
	private Sprite image_phase6_sprite;
	
	//valores para asignar los numeros de fase de cada boton:
	public int step_number_btn_one;
	public int step_number_btn_two;
	public int step_number_btn_three;
	public int step_number_btn_four;
	public int step_number_btn_five;
	public int step_number_btn_six;
	public int step_number_btn_seven;
	public int step_number_btn_eight;
	
	//variable para almacenar la accion del boton regresar:
	public System.Action goBackToMenuPhases;
	//variables para las acciones de cada boton:
	public System.Action goToActionBtnOne;
	public System.Action goToActionBtnTwo;
	public System.Action goToActionBtnThree;
	public System.Action goToActionBtnFour;
	public System.Action goToActionBtnFive;
	public System.Action goToActionBtnSix;
	public System.Action goToActionBtnSeven;
	public System.Action goToActionBtnEight;
	
	//Referencia a cada uno de los slots donde se organizan los pasos para controlar la organizacion automatica:
	public GameObject slot_step_one;
	public GameObject slot_step_two;
	public GameObject slot_step_three;
	public GameObject slot_step_four;
	public GameObject slot_step_five;
	public GameObject slot_step_six;
	public GameObject slot_step_seven;
	public GameObject slot_step_eight;
	
	//variables que controlan si los botones de los pasos deben estar habilitados una vez se organizan:
	public bool step_btn_one_enable;
	public bool step_btn_two_enable;
	public bool step_btn_three_enable;
	public bool step_btn_four_enable;
	public bool step_btn_five_enable;
	public bool step_btn_six_enable;
	public bool step_btn_seven_enable;
	public bool step_btn_eight_enable;
	
	private Sprite img_gray_step;
	
	//variable de referencia al componente tipo Imagen que muestra el tick en la interfaz:
	public Image tickCorrectOrder;
	
	//Sprite de la imagen del tick:
	private Sprite img_tick;
	
	//sprite de la imagen del warning que se muestra en cambio del tick:
	private Sprite img_warning;
	
	//metodo delegado para notificar al AppManager cuando ya se han organizado todas las fases:
	public delegate void setEvalStepsPhase1Organized(bool organized);
	
	public setEvalStepsPhase1Organized NotifyStepsOrganized;
	
	//variable para controlar si todos los elementos se han colocado correctamente en los slots correspondientes:
	[SerializeField] Transform[] slots;
	
	//variable para obtener referencia de 
	private bool item_slot;
	
	//variable que controla si todos los objetos estan colocados en la posicion correct:
	public bool items_ubicados_correctamente;
	
	//variable que controla la cuenta de elementos que estan correctamente ubicados en la interfaz:
	private int cont_items_ubicados_correct;
	
	//referencia al script DragBehaviour para asignar el numero de fase/paso que controla ese boton:
	DragBehaviourMenuOfSteps drag_controller;
	
	//variable contador de items que se colocan en los slots (permite controlar si ya se han colocado todos o no)
	private int cont_items_en_slots;
	
	//variable que controla si se han ubicado los pasos automaticamente por peticion del AppManager
	//entonces no se desactivan por algun cambio en la interfaz:
	public bool steps_organized_from_manager;
	
	//vector que almacena las imagenes en gris en el orden aleatorio con el objetivo de colocarlas en el orden
	//correcto una vez el estudiante ha organizado los pasos
	public string[] imgs_gray_random;

	//variable para obtener la lista de identificadores de paso cuando se estan organizando los pasos de la FASE 2:
	private int[] lista_objetos;
	
	//variable para almacenar la id del paso que se esta colocando:
	private int id_paso;
	
	//variable que almacena la secuencia de ids de los pasos que estan en los slots de la fase 2:
	public string secuencia_pasos;
	
	//variable temporal para almacenar cada elemento string de la secuencia:
	private string elemento_secuencia;
	
	
	#region IHasChangedInMenuStepsPhase1 implementation
	public void HasChanged ()
	{	
		cont_items_ubicados_correct = 0;
		cont_items_en_slots = 0;
		lista_objetos = new int[8];//OJO CADA FASE TIENE UN NUMERO DIFERENTE DE PASOS
		int contad = 0;
		Debug.Log ("Se ha notificado de un cambio en CanvasProcessManagerEval organizando pasos");
		foreach (Transform slotTransform in slots) {
			item_slot = slotTransform.GetComponent<SlotsBehaviourMenuStepsPhaseTwo>().slot_con_objeto_correcto;
			id_paso = slotTransform.GetComponent<SlotsBehaviourMenuStepsPhaseTwo>().id_of_step_in_slot;
			lista_objetos[contad] = id_paso;
			contad++;
			//Debug.Log ("Se ha obtenido el componente SlotsBehaviour en CanvasProcessManagerEval organizando pasos");
			if(item_slot){
				Debug.Log ("Los pasos estan correctamente ordenados en CanvasProcessManagerEval organizando pasos");
				cont_items_ubicados_correct++;
			} 
			if(slotTransform.childCount > 0){
				cont_items_en_slots++;
			}
		}
		Debug.Log ("CanvasProcessPhasesManagEval: Cantidad de items correctamente ubicados: " + cont_items_ubicados_correct);
		Debug.Log ("Cantidad de items en slots:" + cont_items_en_slots);
		if (cont_items_ubicados_correct == 8)
			items_ubicados_correctamente = true;
		else
			items_ubicados_correctamente = false;
		
		Debug.Log ("CanvasProcessPhasesManagEval: Se va a definir en el manager organizacion correcta de fases = " + items_ubicados_correctamente);
		if (items_ubicados_correctamente) {
			if (tickCorrectOrder != null){
				tickCorrectOrder.GetComponent<Image>().sprite = img_tick;
				tickCorrectOrder.enabled = true;
			}
			this.NotifyStepsOrganized (true);
			
			if(!steps_organized_from_manager){
				Debug.Log ("Ingresa al metodo para bloquear los pasos por organizacion automatica del appmanager");
				//inactivando los pasos para que el estudiante no pueda acceder a cualquier otro sino al primero
				//inicialmente:
				
				if(imgs_gray_random.Length >= 8){
					Debug.Log ("Ingresa a deshabilitar los botones ");
					Debug.Log ("Numero step_btn_one=" + step_number_btn_one);
					if(step_number_btn_one != 1)
						this.btn_one_to_order.interactable = false;
					img_gray_step = Resources.Load<Sprite>(imgs_gray_random[0]);
					if(img_gray_step != null)
						this.btn_one_to_order.GetComponent<Image>().sprite = img_gray_step;
					
					Debug.Log ("Numero step_btn_two=" + step_number_btn_two);
					if(step_number_btn_two != 1){
						this.btn_two_to_order.interactable = false;
					}
					img_gray_step = Resources.Load<Sprite>(imgs_gray_random[1]);
					if(img_gray_step != null)
						this.btn_two_to_order.GetComponent<Image>().sprite = img_gray_step;
					
					Debug.Log ("Numero step_btn_three=" + step_number_btn_three);
					if(step_number_btn_three != 1){
						this.btn_three_to_order.interactable = false;
					}
					img_gray_step = Resources.Load<Sprite>(imgs_gray_random[2]);
					if(img_gray_step != null)
						this.btn_three_to_order.GetComponent<Image>().sprite = img_gray_step;
					
					Debug.Log ("Numero step_btn_four=" + step_number_btn_four);
					if(step_number_btn_four != 1){
						this.btn_four_to_order.interactable = false;
					}
					img_gray_step = Resources.Load<Sprite>(imgs_gray_random[3]);
					if(img_gray_step != null)
						this.btn_four_to_order.GetComponent<Image>().sprite = img_gray_step;
					
					Debug.Log ("Numero step_btn_five=" + step_number_btn_five);
					if(step_number_btn_five != 1){
						this.btn_five_to_order.interactable = false;
					}
					img_gray_step = Resources.Load<Sprite>(imgs_gray_random[4]);
					if(img_gray_step != null)
						this.btn_five_to_order.GetComponent<Image>().sprite = img_gray_step;
					
					Debug.Log ("Numero step_btn_six=" + step_number_btn_six);
					if(step_number_btn_six != 1){
						this.btn_six_to_order.interactable = false;
					}
					img_gray_step = Resources.Load<Sprite>(imgs_gray_random[5]);
					if(img_gray_step != null)
						this.btn_six_to_order.GetComponent<Image>().sprite = img_gray_step;

					Debug.Log ("Numero step_btn_seven=" + step_number_btn_seven);
					if(step_number_btn_seven != 1){
						this.btn_seven_to_order.interactable = false;
					}
					img_gray_step = Resources.Load<Sprite>(imgs_gray_random[6]);
					if(img_gray_step != null)
						this.btn_seven_to_order.GetComponent<Image>().sprite = img_gray_step;

					Debug.Log ("Numero step_btn_eight=" + step_number_btn_eight);
					if(step_number_btn_eight != 1){
						this.btn_eight_to_order.interactable = false;
					}
					img_gray_step = Resources.Load<Sprite>(imgs_gray_random[7]);
					if(img_gray_step != null)
						this.btn_eight_to_order.GetComponent<Image>().sprite = img_gray_step;


				} else {
					Debug.Log ("MenuOfStepsPhaseOneManager: El vector de imagenes en GRIS es NULO");
				}
			}//cierra if interno que valida si se ha pedido organizar los pasos desde el app-manager
		} else { 
			if (tickCorrectOrder != null)
				tickCorrectOrder.enabled = false;
			if(cont_items_en_slots == 8){
				tickCorrectOrder.GetComponent<Image>().sprite = img_warning;
				tickCorrectOrder.enabled = true;
			}
			this.NotifyStepsOrganized (false);
		}
		
		Debug.Log ("MenuOfStepsPhaseTwoEval: Los items estan correctamente ubicados? = " + items_ubicados_correctamente);
		//se va a obtener la secuencia del orden de pasos 
		secuencia_pasos = "";
		if (lista_objetos.Length >= 8 && cont_items_en_slots >= 8) {
			//recorriendo la secuencia de pasos como esta organizados actualmente:
			Debug.Log ("La capacidad de la lista es: " + lista_objetos.Length );
			for (int i=0; i<lista_objetos.Length; i++) {
				Debug.Log ("Secuencia de pasos en i= "+i+"= " + lista_objetos [i]);
				elemento_secuencia = lista_objetos [i].ToString(); 
				if (i == 0)
					secuencia_pasos = secuencia_pasos + elemento_secuencia;
				else
					secuencia_pasos = secuencia_pasos + "-" + elemento_secuencia;
			}
			
			//intentando guardar en la BD:
			Debug.Log ("Se va a solicitar guardar la secuencia en la BD:");
			var and_unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			var current_act = and_unity.GetStatic<AndroidJavaObject>("currentActivity");
			Debug.Log("Se ha obtenido current activity...");
			// Accessing the class to call a static method on it
			var save_todb_activity = new AndroidJavaClass("edu.udg.bcds.pintura.arapp.SaveDatabaseData");
			Debug.Log ("Se ha obtenido StartActivity...");
			Debug.Log ("Se va a intentar obtener la fecha...");
			string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
			Debug.Log ("La fecha obtenida es: " + fecha);
			
			object[] parameters = new object[3]; 
			parameters [0] = current_act; //pasando el argumento de la actividad actual que se debe reproducir
			parameters [1] = fecha; //pasando el argumento de la fecha y hora actual
			parameters [2] = secuencia_pasos; //enviando 
			// Se llama al metodo estatico de android que almacena la secuencia en la base de datos:
			save_todb_activity.CallStatic("SaveStepsPhaseTwoSeqToDB", parameters);
			
		} else {
			Debug.Log ("La lista de elementos para la secuencia aun NO ESTA COMPLETA items_en_slots =" + cont_items_en_slots );
		}

	}
	#endregion	
	
	// Use this for initialization
	void Start () {
		Debug.Log ("Llamado al metodo START de MenuOfStepsPhase2 - Instancia:" + this.gameObject.GetInstanceID() + " name:" + this.gameObject.name);
		
		//inicializando variable que controla si todos los items estan en los slots:
		items_ubicados_correctamente = false;
		
		//inicializando la variable que indica si se ha pedido organizar automaticamente los pasos
		//por el appmanager:
		//steps_organized_from_manager = false;
		
		//desactivando el tick de orden correcto hasta que se obtenga el orden correcto:
		if (tickCorrectOrder != null)
			tickCorrectOrder.enabled = false;
		
		//cargando la imagen del tick y del boton warning:
		img_tick = Resources.Load<Sprite> ("Sprites/buttons/tick");
		img_warning = Resources.Load<Sprite> ("Sprites/buttons/warning_order");
		
		//obteniendo el componente del encabezado de la imagen:
		imagen_header = GameObject.Find ("header_image_menuphase2_eval");
		if (imagen_header != null) {
			Debug.Log ("La imagen del HEADER es: " + image_header_phase1);
			sprite_image_header_phase1 = Resources.Load<Sprite> (image_header_phase1);
			imagen_header.GetComponent<Image>().sprite = sprite_image_header_phase1; 
		}
		
		//obteniendo el componente text que se llama titulo en el prefab:
		//titulo_object = GameObject.Find ("title_menu_steps_phase2_text_eval");
		if (titulo_object != null) {
			Debug.LogError ("Se va a cambiar el TITULO de la interfaz es: " + titulo);
			titulo_object.GetComponent<Text> ().text = titulo;
		} else {
			Debug.LogError("Error: No se encuentra el objeto TITULO para la interfaz en MenuOfStepsPhaseTwoManagerEval");
		}
		
		//colocando el texto de la introduccion:
		//introduction_object = GameObject.Find ("introduction_steps_of_phase1_interface");
		if (introduction_object != null) {
			Debug.LogError ("Se va a cambiar el texto de la INTRODUCCION");
			introduction_asset = Resources.Load (introduction_text_path) as TextAsset;
			introduction_object.GetComponent<Text> ().text = introduction_asset.text;
		} else {
			Debug.LogError("Error: No se encuentra el objeto INTRODUCCION para la interfaz en MenuOfStepsPhaseTwoManagerEval");
		}
		
		//accion regresar en el boton
		if(regresar != null){
			Debug.LogError("Se agrega la accion al boton REGRESAR");
			regresar.onClick.AddListener(()=>{ActionButton_goBackToMenuPhases();});
		}
		
		//accion ir al Step1 del Phase
		
		
		//colocando las imagenes de las fases despues del encabezado (guia del proceso completo para los estudiantes)
		//image_phase1_object = GameObject.Find ("image_phase1");
		if (image_phase1_object != null) {
			image_phase1_sprite = Resources.Load<Sprite> (image_phase1_path);
			image_phase1_object.GetComponent<Image> ().sprite = image_phase1_sprite;
		} else {
			Debug.LogError("No se ha podido obtener el OBJETO IMAGEN DE FASE EN ENCABEZADO en MenuOfStepsPhaseTwoManagerEval");
		}
		
		//image_phase2_object = GameObject.Find ("image_phase2");
		if (image_phase2_object != null) {
			image_phase2_sprite = Resources.Load<Sprite>(image_phase2_path);
			image_phase2_object.GetComponent<Image>().sprite = image_phase2_sprite;
		}else {
			Debug.LogError("No se ha podido obtener el OBJETO IMAGEN DE FASE EN ENCABEZADO en MenuOfStepsPhaseTwoManagerEval");
		}
		
		//image_phase3_object = GameObject.Find ("image_phase3");
		if (image_phase3_object != null) {
			image_phase3_sprite = Resources.Load<Sprite>(image_phase3_path);
			image_phase3_object.GetComponent<Image>().sprite = image_phase3_sprite;
		}else {
			Debug.LogError("No se ha podido obtener el OBJETO IMAGEN DE FASE EN ENCABEZADO en MenuOfStepsPhaseTwoManagerEval");
		}
		
		//image_phase4_object = GameObject.Find ("image_phase4");
		if (image_phase4_object != null) {
			image_phase4_sprite = Resources.Load<Sprite>(image_phase4_path);
			image_phase4_object.GetComponent<Image>().sprite = image_phase4_sprite;
		}else {
			Debug.LogError("No se ha podido obtener el OBJETO IMAGEN DE FASE EN ENCABEZADO en MenuOfStepsPhaseTwoManagerEval");
		}
		
		//image_phase5_object = GameObject.Find ("image_phase5");
		if (image_phase5_object != null) {
			image_phase5_sprite = Resources.Load<Sprite>(image_phase5_path);
			image_phase5_object.GetComponent<Image>().sprite = image_phase5_sprite;
		}else {
			Debug.LogError("No se ha podido obtener el OBJETO IMAGEN DE FASE EN ENCABEZADO en MenuOfStepsPhaseTwoManagerEval");
		}
		
		//image_phase6_object = GameObject.Find ("image_phase6");
		if (image_phase6_object != null) {
			image_phase6_sprite = Resources.Load<Sprite>(image_phase6_path);
			image_phase6_object.GetComponent<Image>().sprite = image_phase6_sprite;
		}else {
			Debug.LogError("No se ha podido obtener el OBJETO IMAGEN DE FASE EN ENCABEZADO en MenuOfStepsPhaseTwoManagerEval");
		}
		
		
		if (btn_one_to_order != null) {
			image_button_sprite_uno = Resources.Load<Sprite> (img_one_to_order);
			btn_one_to_order.GetComponent<Image>().sprite = image_button_sprite_uno;
			////Agregando la accion para ir al step1 - phase 1 (buscar capo del carro):
			btn_one_to_order.onClick.AddListener(()=>{ActionButton_goToActionBtnOne();});
			drag_controller = btn_one_to_order.GetComponent<DragBehaviourMenuOfSteps>();
			if(drag_controller != null)
				drag_controller.phase_number = step_number_btn_one;
		}
		
		if (btn_two_to_order != null) {
			image_button_sprite_dos = Resources.Load<Sprite> (img_two_to_order);
			btn_two_to_order.GetComponent<Image>().sprite = image_button_sprite_dos;
			//Agregando la accion para ir al conjunto de actividades (2. limpieza):
			btn_two_to_order.onClick.AddListener(()=>{ActionButton_goToActionBtnTwo();});
			drag_controller = btn_two_to_order.GetComponent<DragBehaviourMenuOfSteps>();
			if(drag_controller != null)
				drag_controller.phase_number = step_number_btn_two;
		}
		
		if (btn_three_to_order != null) {
			image_button_sprite_tres = Resources.Load<Sprite> (img_three_to_order);
			btn_three_to_order.GetComponent<Image>().sprite = image_button_sprite_tres;
			//Agregando la accion para ir al conjunto de actividades (3. secado):
			btn_three_to_order.onClick.AddListener(()=>{ActionButton_goToActionBtnThree();});
			drag_controller = btn_three_to_order.GetComponent<DragBehaviourMenuOfSteps>();
			if(drag_controller != null)
				drag_controller.phase_number = step_number_btn_three;
		}
		
		if (btn_four_to_order != null) {
			image_button_sprite_cuatro = Resources.Load<Sprite> (img_four_to_order);
			btn_four_to_order.GetComponent<Image>().sprite = image_button_sprite_cuatro;
			//Agregando la accion para ir al conjunto de actividades (4. localizar irregularidades):
			btn_four_to_order.onClick.AddListener(()=>{ActionButton_goToActionBtnFour();});
			drag_controller = btn_four_to_order.GetComponent<DragBehaviourMenuOfSteps>();
			if(drag_controller != null)
				drag_controller.phase_number = step_number_btn_four;
		}
		
		if (btn_five_to_order != null) {
			image_button_sprite_cinco = Resources.Load<Sprite> (img_five_to_order);
			btn_five_to_order.GetComponent<Image>().sprite = image_button_sprite_cinco;
			//Agregando la accion para ir al conjunto de actividades (5. corregir irregularidades):
			btn_five_to_order.onClick.AddListener(()=>{ActionButton_goToActionBtnFive();});
			drag_controller = btn_five_to_order.GetComponent<DragBehaviourMenuOfSteps>();
			if(drag_controller != null)
				drag_controller.phase_number = step_number_btn_five;
		}
		
		if (btn_six_to_order != null) {
			image_button_sprite_seis = Resources.Load<Sprite> (img_six_to_order);
			btn_six_to_order.GetComponent<Image>().sprite = image_button_sprite_seis;
			//Agregando la accion para ir al conjunto de actividades (5. corregir irregularidades):
			btn_six_to_order.onClick.AddListener(()=>{ActionButton_goToActionBtnSix();});
			drag_controller = btn_six_to_order.GetComponent<DragBehaviourMenuOfSteps>();
			if(drag_controller != null)
				drag_controller.phase_number = step_number_btn_six;
		}

		if (btn_seven_to_order != null) {
			image_button_sprite_siete = Resources.Load<Sprite> (img_seven_to_order);
			btn_seven_to_order.GetComponent<Image>().sprite = image_button_sprite_siete;
			//Agregando la accion para ir al conjunto de actividades (5. corregir irregularidades):
			btn_seven_to_order.onClick.AddListener(()=>{ActionButton_goToActionBtnSeven();});
			drag_controller = btn_seven_to_order.GetComponent<DragBehaviourMenuOfSteps>();
			if(drag_controller != null)
				drag_controller.phase_number = step_number_btn_seven;
		}

		if (btn_eight_to_order != null) {
			image_button_sprite_ocho = Resources.Load<Sprite> (img_eight_to_order);
			btn_eight_to_order.GetComponent<Image>().sprite = image_button_sprite_ocho;
			//Agregando la accion para ir al conjunto de actividades (5. corregir irregularidades):
			btn_eight_to_order.onClick.AddListener(()=>{ActionButton_goToActionBtnEight();});
			drag_controller = btn_eight_to_order.GetComponent<DragBehaviourMenuOfSteps>();
			if(drag_controller != null)
				drag_controller.phase_number = step_number_btn_eight;
		}
		
		//colocando los textos en los botones:
		if(text_btn_one_to_order != null){
			text_btn_one_to_order.text = this.string_btn_one_text;
		}
		
		if(text_btn_two_to_order != null){
			text_btn_two_to_order.text = this.string_btn_two_text;
		}
		
		if(text_btn_three_to_order != null){
			text_btn_three_to_order.text = this.string_btn_three_text;
		}
		
		if(text_btn_four_to_order != null){
			text_btn_four_to_order.text = this.string_btn_four_text;
		}
		
		if(text_btn_five_to_order != null){
			text_btn_five_to_order.text = this.string_btn_five_text;
		}
		
		if(text_btn_six_to_order != null){
			text_btn_six_to_order.text = this.string_btn_six_text;
		}

		if(text_btn_seven_to_order != null){
			text_btn_seven_to_order.text = this.string_btn_seven_text;
		}

		if(text_btn_eight_to_order != null){
			text_btn_eight_to_order.text = this.string_btn_eight_text;
		}
		
		//llamado al metodo hasChanged para verificar si hay algun cambio los slots de pasos:
		HasChanged ();
		
	}//cierra metodo start
	
	
	/// <summary>
	/// Method for going back to the menu of phases
	/// </summary>
	public void ActionButton_goBackToMenuPhases(){
		this.goBackToMenuPhases();
	}
	
	/// <summary>
	/// Method for going to the activities of Phase2-Step1
	/// </summary>
	public void ActionButton_goToActionBtnOne(){
		if (this.goToActionBtnOne != null && items_ubicados_correctamente) {
			Debug.Log ("MenuOfStepsPhaseTwoManagerEval: ACTION del boton ONE");
			this.goToActionBtnOne ();
		}
	}
	
	/// <summary>
	/// Method for going to the activities of Phase2-Step2
	/// </summary>
	public void ActionButton_goToActionBtnTwo(){
		if (this.goToActionBtnTwo != null && items_ubicados_correctamente) {
			Debug.Log ("MenuOfStepsPhaseTwoManagerEval: ACTION del boton TWO");
			this.goToActionBtnTwo ();
		}
	}
	
	/// <summary>
	/// Method for going to the activities of Phase2-Step3
	/// </summary>
	public void ActionButton_goToActionBtnThree(){
		if (this.goToActionBtnThree != null && items_ubicados_correctamente) {
			Debug.Log ("MenuOfStepsPhaseTwoManagerEval: ACTION del boton THREE");
			this.goToActionBtnThree ();
		}
	}
	
	/// <summary>
	/// Method for going to the activities of Phase2-Step4
	/// </summary>
	public void ActionButton_goToActionBtnFour(){
		if (this.goToActionBtnFour != null && items_ubicados_correctamente) {
			Debug.Log ("MenuOfStepsPhaseTwoManagerEval: ACTION del boton FOUR");
			this.goToActionBtnFour ();
		}
	}
	
	/// <summary>
	/// Method for going to the activities of Phase2-Step5
	/// </summary>
	public void ActionButton_goToActionBtnFive(){
		if (this.goToActionBtnFive != null && items_ubicados_correctamente) {
			Debug.Log ("MenuOfStepsPhaseTwoManagerEval: ACTION del boton FIVE");
			this.goToActionBtnFive ();
		}
	}
	
	/// <summary>
	/// Method for going to the activities of Phase2-Step6
	/// </summary>
	public void ActionButton_goToActionBtnSix(){
		if (this.goToActionBtnSix != null && items_ubicados_correctamente) {
			Debug.Log ("MenuOfStepsPhaseTwoManagerEval: ACTION del boton SIX");
			this.goToActionBtnSix ();
		}
	}

	/// <summary>
	/// Method for going to the activities of Phase2-Step7
	/// </summary>
	public void ActionButton_goToActionBtnSeven(){
		if (this.goToActionBtnSeven != null && items_ubicados_correctamente) {
			Debug.Log ("MenuOfStepsPhaseTwoManagerEval: ACTION del boton SEVEN");
			this.goToActionBtnSeven ();
		}
	}

	/// <summary>
	/// Method for going to the activities of Phase2-Step8
	/// </summary>
	public void ActionButton_goToActionBtnEight(){
		if (this.goToActionBtnEight != null && items_ubicados_correctamente) {
			Debug.Log ("MenuOfStepsPhaseTwoManagerEval: ACTION del boton EIGHT");
			this.goToActionBtnEight ();
		}
	}
	
	
	/// <summary>
	/// Metodo que organiza en el orden correcto el conjunto de pasos cuando el estudiante ya los ha organizado
	/// </summary>
	public void OrganizarPasosOrdenCorrecto(){
		
		Debug.Log ("Llamando a la funcion OrganizarPasosOrdenCorrecto en MenuOfStepsPhaseTwoManagerEval");
		this.btn_one_to_order.transform.SetParent (this.slot_step_one.transform);
		this.btn_one_to_order.transform.position = this.slot_step_one.transform.position;
		this.slot_step_one.GetComponent<SlotsBehaviourMenuStepsPhaseTwo> ().slot_con_objeto_correcto = true;
		this.btn_one_to_order.interactable = true;
		
		Debug.Log ("MenuOfStepsPhaseTwoManagerEval: Boton 2 debe habilitarse?=" + step_btn_two_enable);
		this.btn_two_to_order.transform.SetParent (this.slot_step_two.transform);
		this.btn_two_to_order.transform.position = this.slot_step_two.transform.position;
		this.slot_step_two.GetComponent<SlotsBehaviourMenuStepsPhaseTwo> ().slot_con_objeto_correcto = true;
		if (step_btn_two_enable)
			this.btn_two_to_order.interactable = true;
		else
			this.btn_two_to_order.interactable = false;
		
		Debug.Log ("MenuOfStepsPhaseTwoManagerEval: Boton 3 debe habilitarse?=" + step_btn_three_enable);
		this.btn_three_to_order.transform.SetParent (this.slot_step_three.transform);
		this.btn_three_to_order.transform.position = this.slot_step_three.transform.position;
		this.slot_step_three.GetComponent<SlotsBehaviourMenuStepsPhaseTwo> ().slot_con_objeto_correcto = true;
		if (step_btn_three_enable)
			this.btn_three_to_order.interactable = true;
		else
			this.btn_three_to_order.interactable = false;
		
		Debug.Log ("MenuOfStepsPhaseTwoManagerEval: Boton 4 debe habilitarse?=" + step_btn_four_enable);
		this.btn_four_to_order.transform.SetParent (this.slot_step_four.transform);
		this.btn_four_to_order.transform.position = this.slot_step_four.transform.position;
		this.slot_step_four.GetComponent<SlotsBehaviourMenuStepsPhaseTwo> ().slot_con_objeto_correcto = true;
		if (step_btn_four_enable)
			this.btn_four_to_order.interactable = true;
		else
			this.btn_four_to_order.interactable = false;
		
		Debug.Log ("MenuOfStepsPhaseTwoManagerEval: Boton 5 debe habilitarse?=" + step_btn_five_enable);
		this.btn_five_to_order.transform.SetParent (this.slot_step_five.transform);
		this.btn_five_to_order.transform.position = this.slot_step_five.transform.position;
		this.slot_step_five.GetComponent<SlotsBehaviourMenuStepsPhaseTwo> ().slot_con_objeto_correcto = true;
		if (step_btn_five_enable)
			this.btn_five_to_order.interactable = true;
		else this.btn_five_to_order.interactable = false;
		
		Debug.Log ("MenuOfStepsPhaseTwoManagerEval: Boton 6 debe habilitarse?=" + step_btn_six_enable);
		this.btn_six_to_order.transform.SetParent (this.slot_step_six.transform);
		this.btn_six_to_order.transform.position = this.slot_step_six.transform.position;
		this.slot_step_six.GetComponent<SlotsBehaviourMenuStepsPhaseTwo> ().slot_con_objeto_correcto = true;
		if (step_btn_six_enable)
			this.btn_six_to_order.interactable = true;
		else this.btn_six_to_order.interactable = false;

		Debug.Log ("MenuOfStepsPhaseTwoManagerEval: Boton 7 debe habilitarse?=" + step_btn_seven_enable);
		this.btn_seven_to_order.transform.SetParent (this.slot_step_seven.transform);
		this.btn_seven_to_order.transform.position = this.slot_step_seven.transform.position;
		this.slot_step_seven.GetComponent<SlotsBehaviourMenuStepsPhaseTwo> ().slot_con_objeto_correcto = true;
		if (step_btn_seven_enable)
			this.btn_seven_to_order.interactable = true;
		else this.btn_seven_to_order.interactable = false;

		Debug.Log ("MenuOfStepsPhaseTwoManagerEval: Boton 8 debe habilitarse?=" + step_btn_eight_enable);
		this.btn_eight_to_order.transform.SetParent (this.slot_step_eight.transform);
		this.btn_eight_to_order.transform.position = this.slot_step_eight.transform.position;
		this.slot_step_eight.GetComponent<SlotsBehaviourMenuStepsPhaseTwo> ().slot_con_objeto_correcto = true;
		if (step_btn_eight_enable)
			this.btn_eight_to_order.interactable = true;
		else this.btn_eight_to_order.interactable = false;
		
		
		if (tickCorrectOrder != null)
			tickCorrectOrder.enabled = true;
		Debug.Log ("Finalizando funcion OrganizarPasosOrdenCorrecto en MenuOfStepsPhase1ManagerEval");
	}
	
	
	// Update is called once per frame
	void Update () {
		
	}
	
	
}

namespace UnityEngine.EventSystems{
	public interface IHasChangedInMenuStepsPhase2 : IEventSystemHandler{
		void HasChanged();
	}
}

