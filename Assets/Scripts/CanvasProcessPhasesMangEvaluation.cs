using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CanvasProcessPhasesMangEvaluation : MonoBehaviour, IHasChanged {

	//variable seteada desde el manager que almacena el titulo de la interfaz:
	public string titulo;
	//texto de introduccion
	public string introduction_text_path;

	public string continue_text_path;
	public Button limpiezaButton;
	public Button matizadoButton;
	public Button masilladoButton;
	public Button aparejadoButton;
	public Button pintadoButton;
	public Button barnizadoButton;

	//referencia al boton regresar de la interfaz:
	public Button regresar;
	
	public Text textUnoLimpieza;
	public Text textDosMatizado;
	public Text textTresMasillado;
	public Text textCuatroAparejado;
	public Text textCincoPintado;
	public Text textSeisBarnizado;
	//variable de referencia al componente que muestra la imagen del tick o del warning:
	public Image tickCorrectOrder;

	//Sprite de la imagen que se muestra en cambio del tick (warning):
	private Sprite img_warning;
	//Sprite de la imagen del tick
	private Sprite img_tick;

	//Referencia a los componentes text que van dentro de los slots donde se colocan los pasos:
	/*
	public Text slot1_Ordenado;
	public Text slot2_Ordenado;
	public Text slot3_Ordenado;
	public Text slot4_Ordenado;
	public Text slot5_Ordenado;
	public Text slot6_Ordenado;
	*/

	//Strings que van como textos dentro de los slots que se colocan dentro de los pasos:
	/*
	public string texto_slot1_ordenar;
	public string texto_slot2_ordenar;
	public string texto_slot3_ordenar;
	public string texto_slot4_ordenar;
	public string texto_slot5_ordenar;
	public string texto_slot6_ordenar;
	*/

	//rutas a las imagenes que se deben cargar (numeradas para leerse de izquierda a derecha en la interfaz):
	public string image_uno_limpieza;
	public string image_dos_matizado;
	public string image_tres_masillado;
	public string image_cuatro_aparejado;
	public string image_cinco_pintado;
	public string image_seis_barnizado;
	
	//Textos de los nombres de los pasos:
	public string button_uno_text_limpieza;
	public string button_dos_text_matizado;
	public string button_tres_text_masillado;
	public string button_cuatro_text_aparejado;
	public string button_cinco_text_pintado;
	public string button_seis_text_barnizado;

	//numeros correspondientes a las fases que se asignan desde el AppManage de forma aleatoria:
	public int phase_number_button_one;
	public int phase_number_button_two;
	public int phase_number_button_three;
	public int phase_number_button_four;
	public int phase_number_button_five;
	public int phase_number_button_six;

	//referencia al script DragBehaviour para asignar el numero de fase que controla ese boton:
	DragBehaviour drag_controller;
	
	private TextAsset introduction_asset;
	//Objetos para la referencia a componentes de la interfaz:
	private GameObject titulo_object;
	private GameObject introduction_object;
	
	//Sprites para configurar las imagenes de los botones
	private Sprite image_button_sprite_uno;
	private Sprite image_button_sprite_dos;
	private Sprite image_button_sprite_tres;
	private Sprite image_button_sprite_cuatro;
	private Sprite image_button_sprite_cinco;
	private Sprite image_button_sprite_seis;
	
	//variables para controlar las acciones de los botones:
	public System.Action goToMenuStepsOfPhase1_action;
	public System.Action goToMenuStepsOfPhase2_action;
	public System.Action goToMenuStepsOfPhase3_action;
	public System.Action goToMenuStepsOfPhase4_action;
	public System.Action goToMenuStepsOfPhase5_action;
	public System.Action goToMenuStepsOfPhase6_action;

	//variable para controlar el boton hacia atras:
	public System.Action goBackToSelectionOfMode;

	//metodo delegado para notificar al AppManager cuando ya se han organizado todas las fases:
	public delegate void setEvalModePhasesOrganized(bool organized);

	public setEvalModePhasesOrganized NotifyPhasesOrganized;

	//Referencia a cada uno de los slots donde se organizan los pasos para controlar la organizacion automatica:
	public GameObject slot_phase_one;
	public GameObject slot_phase_two;
	public GameObject slot_phase_three;
	public GameObject slot_phase_four;
	public GameObject slot_phase_five;
	public GameObject slot_phase_six;

	//variables que controlan si los botones de los pasos deben estar habilitados una vez se organizan:
	public bool step_btn_one_enable;
	public bool step_btn_two_enable;
	public bool step_btn_three_enable;
	public bool step_btn_four_enable;
	public bool step_btn_five_enable;
	public bool step_btn_six_enable;


	//variable para controlar si todos los elementos se han colocado correctamente en los slots correspondientes:
	[SerializeField] Transform[] slots;

	//variable para obtener referencia de 
	private bool item_slot;

	//variable que controla si todos los objetos estan colocados en la posicion correct:
	public bool items_ubicados_correctamente;

	//variable que controla la cuenta de elementos que estan correctamente ubicados en la interfaz:
	private int cont_items_ubicados_correct;

	//variable contador de items que se colocan en los slots (permite controlar si ya se han colocado todos o no)
	private int cont_items_en_slots;

	//variable que controla si se han ubicado las fases automaticamente por peticion del AppManager
	//entonces no se desactivan por algun cambio en la interfaz:
	public bool phases_organized_from_manager;
	
	//vector que almacena las imagenes en gris en el orden aleatorio con el objetivo de colocarlas en el orden
	//correcto una vez el estudiante ha organizado los pasos
	public string[] imgs_gray_random_phases;

	//variable temportal que almacena la imagen en gris que se debe cargar:
	private Sprite img_gray_step;

	//variable para obtener la lista de identificadores de fase cuando se estan organizando las fases del proceso:
	private int[] lista_objetos;
	
	//variable para almacenar la id de la fase que se esta colocando:
	private int id_paso;
	
	//variable que almacena la secuencia de fases que estan en los slots:
	public string secuencia_pasos;
	
	//variable temporal para almacenar cada elemento string de la secuencia:
	private string elemento_secuencia;

	#region IHasChanged implementation
	public void HasChanged ()
	{	
		cont_items_ubicados_correct = 0;
		cont_items_en_slots = 0;
		lista_objetos = new int[6];
		int contad = 0;
		Debug.Log ("Se ha notificado de un cambio en CanvasProcessManagerEval organizando pasos");
		foreach (Transform slotTransform in slots) {
			item_slot = slotTransform.GetComponent<SlotsBehaviour>().slot_con_objeto_correcto; 
			//Debug.Log ("Se ha obtenido el componente SlotsBehaviour en CanvasProcessManagerEval organizando pasos");
			id_paso = slotTransform.GetComponent<SlotsBehaviour>().id_of_phase_in_slot;
			lista_objetos[contad] = id_paso;
			contad++;
			if(item_slot){
				Debug.Log ("Las fases estan correctamente ordenadas en CanvasProcessManagerEval organizando pasos");
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
			this.NotifyPhasesOrganized (true);

			if(!phases_organized_from_manager){
				Debug.Log ("Ingresa al metodo para bloquear los pasos por organizacion automatica del appmanager");
				//inactivando los pasos para que el estudiante no pueda acceder a cualquier otro sino al primero
				//inicialmente:
				
				if(imgs_gray_random_phases.Length >= 6){
					Debug.Log ("Ingresa a deshabilitar los botones ");
					Debug.Log ("Numero step_btn_one=" + phase_number_button_one);
					if(phase_number_button_one != 1)
						this.limpiezaButton.interactable = false;
					img_gray_step = Resources.Load<Sprite>(imgs_gray_random_phases[0]);
					if(img_gray_step != null)
						this.limpiezaButton.GetComponent<Image>().sprite = img_gray_step;
					
					Debug.Log ("Numero step_btn_two=" + phase_number_button_two);
					if(phase_number_button_two != 1){
						this.matizadoButton.interactable = false;
					}
					img_gray_step = Resources.Load<Sprite>(imgs_gray_random_phases[1]);
					if(img_gray_step != null)
						this.matizadoButton.GetComponent<Image>().sprite = img_gray_step;
					
					Debug.Log ("Numero step_btn_three=" + phase_number_button_three);
					if(phase_number_button_three != 1){
						this.masilladoButton.interactable = false;
					}
					img_gray_step = Resources.Load<Sprite>(imgs_gray_random_phases[2]);
					if(img_gray_step != null)
						this.masilladoButton.GetComponent<Image>().sprite = img_gray_step;
					
					Debug.Log ("Numero step_btn_four=" + phase_number_button_four);
					if(phase_number_button_four != 1){
						this.aparejadoButton.interactable = false;
					}
					img_gray_step = Resources.Load<Sprite>(imgs_gray_random_phases[3]);
					if(img_gray_step != null)
						this.aparejadoButton.GetComponent<Image>().sprite = img_gray_step;
					
					Debug.Log ("Numero step_btn_five=" + phase_number_button_five);
					if(phase_number_button_five != 1){
						this.pintadoButton.interactable = false;
					}
					img_gray_step = Resources.Load<Sprite>(imgs_gray_random_phases[4]);
					if(img_gray_step != null)
						this.pintadoButton.GetComponent<Image>().sprite = img_gray_step;
					
					Debug.Log ("Numero step_btn_six=" + phase_number_button_six);
					if(phase_number_button_six != 1){
						this.barnizadoButton.interactable = false;
					}
					img_gray_step = Resources.Load<Sprite>(imgs_gray_random_phases[5]);
					if(img_gray_step != null)
						this.barnizadoButton.GetComponent<Image>().sprite = img_gray_step;
					
				} else {
					Debug.Log ("MenuOfStepsPhaseOneManager: El vector de imagenes en GRIS es NULO");
				}
			}//cierra if interno que valida si se ha pedido organizar los pasos desde el app-manager

		} else { 
			if (tickCorrectOrder != null){
				tickCorrectOrder.enabled = false;
			}
			if(cont_items_en_slots == 6){
				tickCorrectOrder.GetComponent<Image>().sprite = img_warning;
				tickCorrectOrder.enabled = true;
			}

			this.NotifyPhasesOrganized (false);
		}

		Debug.Log ("CanvasProcessPhasesManagEval: Los items estan correctamente ubicados? = " + items_ubicados_correctamente);
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
			save_todb_activity.CallStatic("SavePhasesSequenceToLocalDB", parameters);
			
		} else {
			Debug.Log ("La lista de elementos para la secuencia aun NO ESTA COMPLETA items_en_slots =" + cont_items_en_slots );
		}
	}
	#endregion	
	
	
	// Use this for initialization
	void Start () {

		//inicializando variable que controla si todos los items estan en los slots:
		items_ubicados_correctamente = false;

		//desactivando el tick de orden correcto hasta que se obtenga el orden correcto:
		if (tickCorrectOrder != null)
			tickCorrectOrder.enabled = false;

		//cargando la imagen del warning que se muestra en cambio del tick cuando el orden no es correcto:
		img_warning = Resources.Load<Sprite> ("Sprites/buttons/warning_order");

		//cargando la imagen del tick:
		img_tick = Resources.Load<Sprite> ("Sprites/buttons/tick");

		//lamado al metodo que verifica si los items estan ubicados correctamente o no:

		//obteniendo el componente text que se llama titulo en el prefab:
		titulo_object = GameObject.Find ("title_process_phases_interface");
		if (titulo_object != null) {
			Debug.LogError("Se va a cambiar el titulo de la interfaz!!");
			titulo_object.GetComponent<Text>().text = titulo;
		}
		
		introduction_object = GameObject.Find ("introduction_process_phases_interface");
		if (introduction_object != null) {
			Debug.LogError("Se va a cambiar el texto central!!");
			introduction_asset = Resources.Load(introduction_text_path) as TextAsset;
			introduction_object.GetComponent<Text>().text = introduction_asset.text;
			
		}
		
		if (limpiezaButton != null) {
			image_button_sprite_uno = Resources.Load<Sprite> (image_uno_limpieza);
			limpiezaButton.GetComponent<Image>().sprite = image_button_sprite_uno;
			//Agregando la accion del boton: esta accion apunta al Action definido desde AppManager
			limpiezaButton.onClick.AddListener(()=>{ActionButton_goToMenuStepsOfPhase1();});
			drag_controller = limpiezaButton.GetComponent<DragBehaviour>();
			if(drag_controller != null)
				drag_controller.phase_number = phase_number_button_one;
			Debug.Log("CanvasProcessManagerEval: Se asigna el numero de fase: " + phase_number_button_one + " al btn LIMPIEZA");
		}
		
		if (matizadoButton != null) {
			image_button_sprite_dos = Resources.Load<Sprite> (image_dos_matizado);
			matizadoButton.GetComponent<Image>().sprite = image_button_sprite_dos;
			matizadoButton.onClick.AddListener(()=>{ActionButton_goToMenuStepsOfPhase2();});
			drag_controller = matizadoButton.GetComponent<DragBehaviour>();
			if(drag_controller != null)
				drag_controller.phase_number = phase_number_button_two;
		}
		
		if (masilladoButton != null) {
			image_button_sprite_tres = Resources.Load<Sprite> (image_tres_masillado);
			masilladoButton.GetComponent<Image>().sprite = image_button_sprite_tres;
			masilladoButton.onClick.AddListener(()=>{ActionButton_goToMenuStepsOfPhase3();});
			drag_controller = masilladoButton.GetComponent<DragBehaviour>();
			if(drag_controller != null)
				drag_controller.phase_number = phase_number_button_three;
		}
		
		if (aparejadoButton != null) {
			image_button_sprite_cuatro = Resources.Load<Sprite> (image_cuatro_aparejado);
			aparejadoButton.GetComponent<Image>().sprite = image_button_sprite_cuatro;
			aparejadoButton.onClick.AddListener(()=>{ActionButton_goToMenuStepsOfPhase4();});
			drag_controller = aparejadoButton.GetComponent<DragBehaviour>();
			if(drag_controller != null)
				drag_controller.phase_number = phase_number_button_four;
		}
		
		if (pintadoButton != null) {
			image_button_sprite_cinco = Resources.Load<Sprite> (image_cinco_pintado);
			pintadoButton.GetComponent<Image>().sprite = image_button_sprite_cinco;
			pintadoButton.onClick.AddListener(()=>{ActionButton_goToMenuStepsOfPhase5();});
			drag_controller = pintadoButton.GetComponent<DragBehaviour>();
			if(drag_controller != null)
				drag_controller.phase_number = phase_number_button_five;
		}
		
		if (barnizadoButton != null) {
			image_button_sprite_seis = Resources.Load<Sprite> (image_seis_barnizado);
			barnizadoButton.GetComponent<Image>().sprite = image_button_sprite_seis;
			barnizadoButton.onClick.AddListener(()=>{ActionButton_goToMenuStepsOfPhase6();});
			drag_controller = barnizadoButton.GetComponent<DragBehaviour>();
			if(drag_controller != null)
				drag_controller.phase_number = phase_number_button_six;
		}

		if (regresar != null) {
			regresar.onClick.AddListener(()=>{ActionButton_goToSelectionMode();});
		}
		
		//colocando los textos en los botones:
		if(textUnoLimpieza != null){
			textUnoLimpieza.text = this.button_uno_text_limpieza;
		}
		
		if(textDosMatizado != null){
			textDosMatizado.text = this.button_dos_text_matizado;
		}
		
		if(textTresMasillado != null){
			textTresMasillado.text = this.button_tres_text_masillado;
		}
		
		if(textCuatroAparejado != null){
			textCuatroAparejado.text = this.button_cuatro_text_aparejado;
		}
		
		if(textCincoPintado != null){
			textCincoPintado.text = this.button_cinco_text_pintado;
		}
		
		if(textSeisBarnizado != null){
			textSeisBarnizado.text = this.button_seis_text_barnizado;
		}
		/*
		//Agrrgando los textos que van dentro de los slots donde se organizan:
		if (slot1_Ordenado != null)
			this.slot1_Ordenado.text = texto_slot1_ordenar;

		if (slot2_Ordenado != null)
			this.slot2_Ordenado.text = texto_slot2_ordenar;

		if (slot3_Ordenado != null)
			this.slot3_Ordenado.text = texto_slot3_ordenar;

		if (slot4_Ordenado != null)
			this.slot4_Ordenado.text = texto_slot4_ordenar;

		if (slot5_Ordenado != null)
			this.slot5_Ordenado.text = texto_slot5_ordenar;

		if (slot6_Ordenado != null)
			this.slot6_Ordenado.text = texto_slot6_ordenar;
		*/

		HasChanged ();
	}
	
	/// <summary>
	/// Method for going to the menu of steps for phase 1
	/// </summary>
	public void ActionButton_goToMenuStepsOfPhase1(){
		if (goToMenuStepsOfPhase1_action != null && items_ubicados_correctamente)
			this.goToMenuStepsOfPhase1_action ();
		else
			Debug.LogError ("No se ha definido una accion para goToMenuStepsOfPhase1 o los items no estan organizados CanvasProcessPhaseManager");
	}
	
	/// <summary>
	/// Method for going to the menu of steps for phase 2
	/// </summary>
	public void ActionButton_goToMenuStepsOfPhase2(){
		if (goToMenuStepsOfPhase2_action != null && items_ubicados_correctamente)
			this.goToMenuStepsOfPhase2_action ();
		else
			Debug.LogError ("No se ha definido una accion para goToMenuStepsOfPhase2 o los items no estan organizados CanvasProcessPhaseManager");
	}
	
	/// <summary>
	/// Method for going to the menu of steps for phase 3
	/// </summary>
	public void ActionButton_goToMenuStepsOfPhase3(){
		if (goToMenuStepsOfPhase3_action != null && items_ubicados_correctamente)
			this.goToMenuStepsOfPhase3_action ();
		else
			Debug.LogError ("No se ha definido una accion para goToMenuStepsOfPhase3 o los items no estan organizados CanvasProcessPhaseManager");
	}
	
	/// <summary>
	/// Method for going to the menu of steps for phase 4
	/// </summary>
	public void ActionButton_goToMenuStepsOfPhase4(){
		if (goToMenuStepsOfPhase4_action != null && items_ubicados_correctamente)
			this.goToMenuStepsOfPhase4_action ();
		else
			Debug.LogError ("No se ha definido una accion para goToMenuStepsOfPhase4 o los items no estan organizados CanvasProcessPhaseManager");
	}
	
	/// <summary>
	/// Method for going to the menu of steps for phase 5
	/// </summary>
	public void ActionButton_goToMenuStepsOfPhase5(){
		if (goToMenuStepsOfPhase5_action != null && items_ubicados_correctamente)
			this.goToMenuStepsOfPhase5_action ();
		else
			Debug.LogError ("No se ha definido una accion para goToMenuStepsOfPhase5 o los items NO estan organizados CanvasProcessPhaseManager");
	}
	
	/// <summary>
	/// Method for going to the menu of steps for phase 6
	/// </summary>
	public void ActionButton_goToMenuStepsOfPhase6(){
		if (goToMenuStepsOfPhase6_action != null && items_ubicados_correctamente)
			this.goToMenuStepsOfPhase6_action ();
		else
			Debug.LogError ("No se ha definido una accion para goToMenuStepsOfPhase6 o los items no estan organizados CanvasProcessPhaseManager");
	}

	/// <summary>
	/// Actions the button_go to selection mode.
	/// </summary>
	public void ActionButton_goToSelectionMode(){
		if (goBackToSelectionOfMode != null)
			this.goBackToSelectionOfMode ();
		else
			Debug.Log ("No se ha definido la accion para el boton regresar en ActionButton_goToSelectionMode de CanvasProcessPhaseManager");
	}
	
	/// <summary>
	/// Metodo que organiza en el orden correcto el conjunto de pasos cuando el estudiante ya los ha organizado
	/// </summary>
	public void OrganizarPasosOrdenCorrecto(){
		Debug.Log ("Llamando a la funcion OrganizarPasosOrdenCorrecto en CanvasProcessPhasesManagerEval");
		this.limpiezaButton.transform.SetParent (this.slot_phase_one.transform);
		this.limpiezaButton.transform.position = this.slot_phase_one.transform.position;
		this.slot_phase_one.GetComponent<SlotsBehaviour> ().slot_con_objeto_correcto = true;
			this.limpiezaButton.interactable = true;

		this.matizadoButton.transform.SetParent (this.slot_phase_two.transform);
		this.matizadoButton.transform.position = this.slot_phase_two.transform.position;
		this.slot_phase_two.GetComponent<SlotsBehaviour> ().slot_con_objeto_correcto = true;
		if (step_btn_two_enable)
			this.matizadoButton.interactable = true;
		else
			this.matizadoButton.interactable = false;

		this.masilladoButton.transform.SetParent (this.slot_phase_three.transform);
		this.masilladoButton.transform.position = this.slot_phase_three.transform.position;
		this.slot_phase_three.GetComponent<SlotsBehaviour> ().slot_con_objeto_correcto = true;
			if (step_btn_three_enable)
				this.masilladoButton.interactable = true;
			else
				this.masilladoButton.interactable = false;

		this.aparejadoButton.transform.SetParent (this.slot_phase_four.transform);
		this.aparejadoButton.transform.position = this.slot_phase_four.transform.position;
		this.slot_phase_four.GetComponent<SlotsBehaviour> ().slot_con_objeto_correcto = true;
			if (step_btn_four_enable)
				this.aparejadoButton.interactable = true;
			else
				this.aparejadoButton.interactable = false;

		this.pintadoButton.transform.SetParent (this.slot_phase_five.transform);
		this.pintadoButton.transform.position = this.slot_phase_five.transform.position;
		this.slot_phase_five.GetComponent<SlotsBehaviour> ().slot_con_objeto_correcto = true;
			if (step_btn_five_enable)
				this.pintadoButton.interactable = true;
			else
				this.pintadoButton.interactable = false;

		this.barnizadoButton.transform.SetParent (this.slot_phase_six.transform);
		this.barnizadoButton.transform.position = this.slot_phase_six.transform.position;
		this.slot_phase_six.GetComponent<SlotsBehaviour> ().slot_con_objeto_correcto = true;
			if (step_btn_six_enable)
				this.barnizadoButton.interactable = true;
			else
				this.barnizadoButton.interactable = false;

		if (tickCorrectOrder != null)
			tickCorrectOrder.enabled = true;
		Debug.Log ("Finalizando funcion OrganizarPasosOrdenCorrecto en CanvasProcessPhasesManagerEval");
	}




	// Update is called once per frame
	void Update () {
		
	}
}

namespace UnityEngine.EventSystems{

	public interface IHasChanged : IEventSystemHandler{
		void HasChanged();
	}
}

