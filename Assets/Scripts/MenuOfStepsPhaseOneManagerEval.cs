using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class MenuOfStepsPhaseOneManagerEval : MonoBehaviour, IHasChangedInMenuStepsPhase1 {

	public string image_header_phase1;
	public string titulo;
	public string introduction_text_path;
	public Button capoCarroButton;
	public Button limpiezaButton;
	public Button secadoButton;
	public Button irregularidadesButton;
	public Button corregirButton;
	public Button desengrasadoButton;
	public Button regresar;
	
	//Referencia a los componentes Text de los botones:
	public Text textUnocapoCarro;
	public Text textDosLimpieza;
	public Text textTresSecado;
	public Text textCuatroIrregularidades;
	public Text textCincoCorregir;
	public Text textSeisDesengrasar;
	
	//rutas a las imagenes que se deben cargar (numeradas para leerse de izquierda a derecha en la interfaz):
	public string image_uno_capo_carro;
	public string image_dos_limpieza;
	public string image_tres_secado;
	public string image_cuatro_irregularidades;
	public string image_cinco_corregir;
	public string image_seis_desengrasar;
	
	//Rutas a las imagenes de las fases para la guia de las fases:
	public string image_phase1_path;
	public string image_phase2_path;
	public string image_phase3_path;
	public string image_phase4_path;
	public string image_phase5_path;
	public string image_phase6_path;
	
	//Textos de los nombres de los pasos:
	public string button_uno_text_capo_carro;
	public string button_dos_text_limpieza;
	public string button_tres_text_secado;
	public string button_cuatro_text_irregularidades;
	public string button_cinco_text_corregir;
	public string button_seis_text_desengrasar;
	
	//GameObjects para obtener el titulo y el objeto que muestra un texto de introduccion:
	private GameObject imagen_header;
	public GameObject titulo_object;
	private GameObject introduction_object;
	
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
	
	//Sprites para configurar las referencias a las imagenes:
	private Sprite image_phase1_sprite;
	private Sprite image_phase2_sprite;
	private Sprite image_phase3_sprite;
	private Sprite image_phase4_sprite;
	private Sprite image_phase5_sprite;
	private Sprite image_phase6_sprite;

	//valores para asignar los numeros de fase de cada boton:
	public int phase_number_button_one;
	public int phase_number_button_two;
	public int phase_number_button_three;
	public int phase_number_button_four;
	public int phase_number_button_five;
	public int phase_number_button_six;
	
	//variable para almacenar la accion del boton regresar:
	public System.Action goBackToMenuPhases;
	
	public System.Action goToActivitiesPhase1Step1;
	public System.Action goToActivitiesPhase1Step2;
	public System.Action goToActivitiesPhase1Step3;
	public System.Action goToActivitiesPhase1Step4;
	public System.Action goToActivitiesPhase1Step5;
	public System.Action goToActivitiesPhase1Step6;

	//Referencia a cada uno de los slots donde se organizan los pasos para controlar la organizacion automatica:
	public GameObject slot_step_one;
	public GameObject slot_step_two;
	public GameObject slot_step_three;
	public GameObject slot_step_four;
	public GameObject slot_step_five;
	public GameObject slot_step_six;

	//variables que controlan si los botones de los pasos deben estar habilitados una vez se organizan:
	public bool step_btn_one_enable;
	public bool step_btn_two_enable;
	public bool step_btn_three_enable;
	public bool step_btn_four_enable;
	public bool step_btn_five_enable;
	public bool step_btn_six_enable;

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

	//variable para obtener la lista de identificadores de paso cuando se estan organizando los pasos de la FASE 1:
	private int[] lista_objetos;

	//variable para almacenar la id del paso que se esta colocando:
	private int id_paso;

	//variable que almacena la secuencia de ids de los pasos que estan en los slots:
	public string secuencia_pasos;

	//variable temporal para almacenar cada elemento string de la secuencia:
	private string elemento_secuencia;

	#region IHasChangedInMenuStepsPhase1 implementation
	public void HasChanged ()
	{	
		cont_items_ubicados_correct = 0;
		cont_items_en_slots = 0;
		lista_objetos = new int[6];
		int contad = 0;
		Debug.Log ("Se ha notificado de un cambio en CanvasProcessManagerEval organizando pasos");
		foreach (Transform slotTransform in slots) {
			item_slot = slotTransform.GetComponent<SlotsBehaviourMenuSteps>().slot_con_objeto_correcto;
			id_paso = slotTransform.GetComponent<SlotsBehaviourMenuSteps>().id_of_step_in_slot;
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
		if (cont_items_ubicados_correct == 6)
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

				if(imgs_gray_random.Length >= 6){
					Debug.Log ("Ingresa a deshabilitar los botones ");
					Debug.Log ("Numero step_btn_one=" + phase_number_button_one);
					if(phase_number_button_one != 1)
						this.capoCarroButton.interactable = false;
					img_gray_step = Resources.Load<Sprite>(imgs_gray_random[0]);
					if(img_gray_step != null)
						this.capoCarroButton.GetComponent<Image>().sprite = img_gray_step;

					Debug.Log ("Numero step_btn_two=" + phase_number_button_two);
					if(phase_number_button_two != 1){
						this.limpiezaButton.interactable = false;
					}
					img_gray_step = Resources.Load<Sprite>(imgs_gray_random[1]);
					if(img_gray_step != null)
						this.limpiezaButton.GetComponent<Image>().sprite = img_gray_step;

					Debug.Log ("Numero step_btn_three=" + phase_number_button_three);
					if(phase_number_button_three != 1){
						this.secadoButton.interactable = false;
					}
					img_gray_step = Resources.Load<Sprite>(imgs_gray_random[2]);
					if(img_gray_step != null)
						this.secadoButton.GetComponent<Image>().sprite = img_gray_step;

					Debug.Log ("Numero step_btn_four=" + phase_number_button_four);
					if(phase_number_button_four != 1){
						this.irregularidadesButton.interactable = false;
					}
					img_gray_step = Resources.Load<Sprite>(imgs_gray_random[3]);
					if(img_gray_step != null)
						this.irregularidadesButton.GetComponent<Image>().sprite = img_gray_step;

					Debug.Log ("Numero step_btn_five=" + phase_number_button_five);
					if(phase_number_button_five != 1){
						this.corregirButton.interactable = false;
					}
					img_gray_step = Resources.Load<Sprite>(imgs_gray_random[4]);
					if(img_gray_step != null)
						this.corregirButton.GetComponent<Image>().sprite = img_gray_step;

					Debug.Log ("Numero step_btn_six=" + phase_number_button_six);
					if(phase_number_button_six != 1){
						this.desengrasadoButton.interactable = false;
					}
					img_gray_step = Resources.Load<Sprite>(imgs_gray_random[5]);
					if(img_gray_step != null)
						this.desengrasadoButton.GetComponent<Image>().sprite = img_gray_step;

				} else {
					Debug.Log ("MenuOfStepsPhaseOneManager: El vector de imagenes en GRIS es NULO");
				}
			}//cierra if interno que valida si se ha pedido organizar los pasos desde el app-manager
		} else { 
			if (tickCorrectOrder != null)
				tickCorrectOrder.enabled = false;
			if(cont_items_en_slots == 6){
				tickCorrectOrder.GetComponent<Image>().sprite = img_warning;
				tickCorrectOrder.enabled = true;
			}
			this.NotifyStepsOrganized (false);
		}
		
		Debug.Log ("CanvasProcessPhasesManagEval: Los items estan correctamente ubicados? = " + items_ubicados_correctamente);
		//se va a obtener la secuencia del orden de pasos 
		secuencia_pasos = "";
		if (lista_objetos.Length >= 6 && cont_items_en_slots >= 6) {
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
			save_todb_activity.CallStatic("SaveStepsPhaseOneSequenceToDB", parameters);

		} else {
			Debug.Log ("La lista de elementos para la secuencia aun NO ESTA COMPLETA items_en_slots =" + cont_items_en_slots );
		}

		
		
	}
	#endregion	
	
	// Use this for initialization
	void Start () {
		Debug.Log ("Llamado al metodo START de MenuOfStepsPhase1 - Instancia:" + this.gameObject.GetInstanceID() + " name:" + this.gameObject.name);

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
		imagen_header = GameObject.Find ("header_image_menuphase1");
		if (imagen_header != null) {
			Debug.Log ("La imagen del HEADER es: " + image_header_phase1);
			sprite_image_header_phase1 = Resources.Load<Sprite> (image_header_phase1);
			imagen_header.GetComponent<Image>().sprite = sprite_image_header_phase1; 
		}
		
		//obteniendo el componente text que se llama titulo en el prefab:
		//titulo_object = GameObject.Find ("title_menu_steps_phase1_text");
		if (titulo_object != null) {
			Debug.LogError ("Se va a cambiar el TITULO de la interfaz es: " + titulo);
			titulo_object.GetComponent<Text> ().text = titulo;
		} else {
			Debug.LogError("ERROR: El objeto que debe mostrar el titulo de la interfaz es NULL");
		}
		
		//colocando el texto de la introduccion:
		introduction_object = GameObject.Find ("introduction_steps_of_phase1_interface");
		if (introduction_object != null) {
			Debug.LogError("Se va a cambiar el texto de la INTRODUCCION con path=" + introduction_text_path);
			introduction_asset = Resources.Load(introduction_text_path) as TextAsset;
			introduction_object.GetComponent<Text>().text = introduction_asset.text;
			Debug.LogError("Despues de cambiar el texto de la intro: " + introduction_asset.text);
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
			Debug.LogError ("---> Error: El objeto que debe mostrar la imagen de FASE en el encabezado es NULL");
		}
		
		//image_phase2_object = GameObject.Find ("image_phase2");
		if (image_phase2_object != null) {
			image_phase2_sprite = Resources.Load<Sprite>(image_phase2_path);
			image_phase2_object.GetComponent<Image>().sprite = image_phase2_sprite;
		}else {
			Debug.LogError ("---> Error: El objeto que debe mostrar la imagen de FASE en el encabezado es NULL");
		}
		
		//image_phase3_object = GameObject.Find ("image_phase3");
		if (image_phase3_object != null) {
			image_phase3_sprite = Resources.Load<Sprite>(image_phase3_path);
			image_phase3_object.GetComponent<Image>().sprite = image_phase3_sprite;
		}else {
			Debug.LogError ("---> Error: El objeto que debe mostrar la imagen de FASE en el encabezado es NULL");
		}
		
		//image_phase4_object = GameObject.Find ("image_phase4");
		if (image_phase4_object != null) {
			image_phase4_sprite = Resources.Load<Sprite>(image_phase4_path);
			image_phase4_object.GetComponent<Image>().sprite = image_phase4_sprite;
		}else {
			Debug.LogError ("---> Error: El objeto que debe mostrar la imagen de FASE en el encabezado es NULL");
		}
		
		//image_phase5_object = GameObject.Find ("image_phase5");
		if (image_phase5_object != null) {
			image_phase5_sprite = Resources.Load<Sprite>(image_phase5_path);
			image_phase5_object.GetComponent<Image>().sprite = image_phase5_sprite;
		}else { 
			Debug.LogError ("---> Error: El objeto que debe mostrar la imagen de FASE en el encabezado es NULL");
		}
		
		//image_phase6_object = GameObject.Find ("image_phase6");
		if (image_phase6_object != null) {
			image_phase6_sprite = Resources.Load<Sprite>(image_phase6_path);
			image_phase6_object.GetComponent<Image>().sprite = image_phase6_sprite;
		}
		
		
		if (capoCarroButton != null) {
			image_button_sprite_uno = Resources.Load<Sprite> (image_uno_capo_carro);
			capoCarroButton.GetComponent<Image>().sprite = image_button_sprite_uno;
			////Agregando la accion para ir al step1 - phase 1 (buscar capo del carro):
			capoCarroButton.onClick.AddListener(()=>{ActionButton_goToActivitiesPhase1Step1();});
			drag_controller = capoCarroButton.GetComponent<DragBehaviourMenuOfSteps>();
			if(drag_controller != null)
				drag_controller.phase_number = phase_number_button_one;
		}
		
		if (limpiezaButton != null) {
			image_button_sprite_dos = Resources.Load<Sprite> (image_dos_limpieza);
			limpiezaButton.GetComponent<Image>().sprite = image_button_sprite_dos;
			//Agregando la accion para ir al conjunto de actividades (2. limpieza):
			limpiezaButton.onClick.AddListener(()=>{ActionButton_goToActivitiesPhase1Step2();});
			drag_controller = limpiezaButton.GetComponent<DragBehaviourMenuOfSteps>();
			if(drag_controller != null)
				drag_controller.phase_number = phase_number_button_two;
		}
		
		if (secadoButton != null) {
			image_button_sprite_tres = Resources.Load<Sprite> (image_tres_secado);
			secadoButton.GetComponent<Image>().sprite = image_button_sprite_tres;
			//Agregando la accion para ir al conjunto de actividades (3. secado):
			secadoButton.onClick.AddListener(()=>{ActionButton_goToActivitiesPhase1Step3();});
			drag_controller = secadoButton.GetComponent<DragBehaviourMenuOfSteps>();
			if(drag_controller != null)
				drag_controller.phase_number = phase_number_button_three;
		}
		
		if (irregularidadesButton != null) {
			image_button_sprite_cuatro = Resources.Load<Sprite> (image_cuatro_irregularidades);
			irregularidadesButton.GetComponent<Image>().sprite = image_button_sprite_cuatro;
			//Agregando la accion para ir al conjunto de actividades (4. localizar irregularidades):
			irregularidadesButton.onClick.AddListener(()=>{ActionButton_goToActivitiesPhase1Step4();});
			drag_controller = irregularidadesButton.GetComponent<DragBehaviourMenuOfSteps>();
			if(drag_controller != null)
				drag_controller.phase_number = phase_number_button_four;
		}
		
		if (corregirButton != null) {
			image_button_sprite_cinco = Resources.Load<Sprite> (image_cinco_corregir);
			corregirButton.GetComponent<Image>().sprite = image_button_sprite_cinco;
			//Agregando la accion para ir al conjunto de actividades (5. corregir irregularidades):
			corregirButton.onClick.AddListener(()=>{ActionButton_goToActivitiesPhase1Step5();});
			drag_controller = corregirButton.GetComponent<DragBehaviourMenuOfSteps>();
			if(drag_controller != null)
				drag_controller.phase_number = phase_number_button_five;
		}
		
		if (desengrasadoButton != null) {
			image_button_sprite_seis = Resources.Load<Sprite> (image_seis_desengrasar);
			desengrasadoButton.GetComponent<Image>().sprite = image_button_sprite_seis;
			//Agregando la accion para ir al conjunto de actividades (5. corregir irregularidades):
			desengrasadoButton.onClick.AddListener(()=>{ActionButton_goToActivitiesPhase1Step6();});
			drag_controller = desengrasadoButton.GetComponent<DragBehaviourMenuOfSteps>();
			if(drag_controller != null)
				drag_controller.phase_number = phase_number_button_six;
		}
		
		//colocando los textos en los botones:
		if(textUnocapoCarro != null){
			textUnocapoCarro.text = this.button_uno_text_capo_carro;
		}
		
		if(textDosLimpieza != null){
			textDosLimpieza.text = this.button_dos_text_limpieza;
		}
		
		if(textTresSecado != null){
			textTresSecado.text = this.button_tres_text_secado;
		}
		
		if(textCuatroIrregularidades != null){
			textCuatroIrregularidades.text = this.button_cuatro_text_irregularidades;
		}
		
		if(textCincoCorregir != null){
			textCincoCorregir.text = this.button_cinco_text_corregir;
		}
		
		if(textSeisDesengrasar != null){
			textSeisDesengrasar.text = this.button_seis_text_desengrasar;
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
	/// Method for going to the activities of Phase1-Step1
	/// </summary>
	public void ActionButton_goToActivitiesPhase1Step1(){
		if(this.goToActivitiesPhase1Step1 != null && items_ubicados_correctamente)
			this.goToActivitiesPhase1Step1 ();
	}
	
	/// <summary>
	/// Method for going to the activities of Phase1-Step2
	/// </summary>
	public void ActionButton_goToActivitiesPhase1Step2(){
		if(this.goToActivitiesPhase1Step2 != null && items_ubicados_correctamente)
			this.goToActivitiesPhase1Step2 ();
	}
	
	/// <summary>
	/// Method for going to the activities of Phase1-Step3
	/// </summary>
	public void ActionButton_goToActivitiesPhase1Step3(){
		if(this.goToActivitiesPhase1Step3 != null && items_ubicados_correctamente)
			this.goToActivitiesPhase1Step3 ();
	}
	
	/// <summary>
	/// Method for going to the activities of Phase1-Step4
	/// </summary>
	public void ActionButton_goToActivitiesPhase1Step4(){
		if(this.goToActivitiesPhase1Step4 != null && items_ubicados_correctamente)
			this.goToActivitiesPhase1Step4 ();
	}
	
	/// <summary>
	/// Method for going to the activities of Phase1-Step5
	/// </summary>
	public void ActionButton_goToActivitiesPhase1Step5(){
		if(this.goToActivitiesPhase1Step5 != null && items_ubicados_correctamente)
			this.goToActivitiesPhase1Step5 ();
	}
	
	/// <summary>
	/// Method for going to the activities of Phase1-Step6
	/// </summary>
	public void ActionButton_goToActivitiesPhase1Step6(){
		if(this.goToActivitiesPhase1Step6 != null && items_ubicados_correctamente)
			this.goToActivitiesPhase1Step6 ();
	}
	

	/// <summary>
	/// Metodo que organiza en el orden correcto el conjunto de pasos cuando el estudiante ya los ha organizado
	/// </summary>
	public void OrganizarPasosOrdenCorrecto(){



		Debug.Log ("Llamando a la funcion OrganizarPasosOrdenCorrecto en CanvasProcessPhasesManagerEval");
		this.capoCarroButton.transform.SetParent (this.slot_step_one.transform);
		this.capoCarroButton.transform.position = this.slot_step_one.transform.position;
		this.slot_step_one.GetComponent<SlotsBehaviourMenuSteps> ().slot_con_objeto_correcto = true;
		this.capoCarroButton.interactable = true;

		Debug.Log ("MenuOFStepsPhaseOneManager: Boton 2 debe habilitarse?=" + step_btn_two_enable);
		this.limpiezaButton.transform.SetParent (this.slot_step_two.transform);
		this.limpiezaButton.transform.position = this.slot_step_two.transform.position;
		this.slot_step_two.GetComponent<SlotsBehaviourMenuSteps> ().slot_con_objeto_correcto = true;
		if (step_btn_two_enable)
			this.limpiezaButton.interactable = true;
		else
			this.limpiezaButton.interactable = false;

		Debug.Log ("MenuOFStepsPhaseOneManager: Boton 3 debe habilitarse?=" + step_btn_three_enable);
		this.secadoButton.transform.SetParent (this.slot_step_three.transform);
		this.secadoButton.transform.position = this.slot_step_three.transform.position;
		this.slot_step_three.GetComponent<SlotsBehaviourMenuSteps> ().slot_con_objeto_correcto = true;
		if (step_btn_three_enable)
			this.secadoButton.interactable = true;
		else
			this.secadoButton.interactable = false;

		Debug.Log ("MenuOFStepsPhaseOneManager: Boton 4 debe habilitarse?=" + step_btn_four_enable);
		this.irregularidadesButton.transform.SetParent (this.slot_step_four.transform);
		this.irregularidadesButton.transform.position = this.slot_step_four.transform.position;
		this.slot_step_four.GetComponent<SlotsBehaviourMenuSteps> ().slot_con_objeto_correcto = true;
		if (step_btn_four_enable)
			this.irregularidadesButton.interactable = true;
		else
			this.irregularidadesButton.interactable = false;

		Debug.Log ("MenuOFStepsPhaseOneManager: Boton 5 debe habilitarse?=" + step_btn_five_enable);
		this.corregirButton.transform.SetParent (this.slot_step_five.transform);
		this.corregirButton.transform.position = this.slot_step_five.transform.position;
		this.slot_step_five.GetComponent<SlotsBehaviourMenuSteps> ().slot_con_objeto_correcto = true;
		if (step_btn_five_enable)
			this.corregirButton.interactable = true;
		else this.corregirButton.interactable = false;

		Debug.Log ("MenuOFStepsPhaseOneManager: Boton 6 debe habilitarse?=" + step_btn_six_enable);
		this.desengrasadoButton.transform.SetParent (this.slot_step_six.transform);
		this.desengrasadoButton.transform.position = this.slot_step_six.transform.position;
		this.slot_step_six.GetComponent<SlotsBehaviourMenuSteps> ().slot_con_objeto_correcto = true;
		if (step_btn_six_enable)
			this.desengrasadoButton.interactable = true;
		else this.desengrasadoButton.interactable = false;

				
		if (tickCorrectOrder != null)
			tickCorrectOrder.enabled = true;
		Debug.Log ("Finalizando funcion OrganizarPasosOrdenCorrecto en MenuOfStepsPhase1ManagerEval");
	}


	// Update is called once per frame
	void Update () {
		
	}


}

namespace UnityEngine.EventSystems{
	public interface IHasChangedInMenuStepsPhase1 : IEventSystemHandler{
		void HasChanged();
	}
}
