using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuOfStepsPhase1Manager : MonoBehaviour {

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
	//En reunion del 27 de Julio (ver acta) el profe Ramon recomienda eliminar la imagen del encabezado
	//private GameObject imagen_header;
	private GameObject titulo_object;
	//En reunion del 27 de julio (ver acta) el profe Ramon recomienda quitar este texto:
	//private GameObject introduction_object;

	//GameObjects para obtener las referencias a los componentes Image que muestran las fases del proceso:
	private GameObject image_phase1_object;
	private GameObject image_phase2_object;
	private GameObject image_phase3_object;
	private GameObject image_phase4_object;
	private GameObject image_phase5_object;
	private GameObject image_phase6_object;


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

	//variable para almacenar la accion del boton regresar:
	public System.Action goBackToMenuPhases;

	public System.Action goToActivitiesPhase1Step1;
	public System.Action goToActivitiesPhase1Step2;
	public System.Action goToActivitiesPhase1Step3;
	public System.Action goToActivitiesPhase1Step4;
	public System.Action goToActivitiesPhase1Step5;
	public System.Action goToActivitiesPhase1Step6;

	//variables para controlar si los pasos se deben dejar activos o no:
	public bool step_one_enabled;
	public bool step_two_enabled;
	public bool step_three_enabled;
	public bool step_four_enabled;
	public bool step_five_enabled;
	public bool step_six_enabled;


	// Use this for initialization
	void Start () {
		Debug.Log ("Llamado al metodo START de MenuOfStepsPhase1 NORMAL Instancia: " + this.gameObject.GetInstanceID() + " name: " + this.name);
		//De acuerdo con la reunion del dia 27 de Julio (ver Acta) el profe Ramon recomienda
		//eliminar la imagen del encabezado en esta interfaz:
		//obteniendo el componente del encabezado de la imagen:
		/*
		imagen_header = GameObject.Find ("header_image_menuphase1");
		if (imagen_header != null) {
			Debug.Log ("La imagen del HEADER es: " + image_header_phase1);
			sprite_image_header_phase1 = Resources.Load<Sprite> (image_header_phase1);
			imagen_header.GetComponent<Image>().sprite = sprite_image_header_phase1; 
		}
		*/
		//obteniendo el componente text que se llama titulo en el prefab:
		titulo_object = GameObject.Find ("title_menu_steps_phase1_text");
		if (titulo_object != null) {
			Debug.LogError("Se va a cambiar el TITULO de la interfaz es: " + titulo );
			titulo_object.GetComponent<Text>().text = titulo;
		}

		//colocando el texto de la introduccion:
		//segun la reunion del 27 de Julio el profe Ramon sugirio eliminar este componente de la interfaz:
		/*
		introduction_object = GameObject.Find ("introduction_steps_of_phase1_interface");
		if (introduction_object != null) {
			Debug.LogError("Se va a cambiar el texto de la INTRODUCCION");
			introduction_asset = Resources.Load(introduction_text_path) as TextAsset;
			introduction_object.GetComponent<Text>().text = introduction_asset.text;
			
		}*/

		//accion regresar en el boton
		if(regresar != null){
			Debug.LogError("Se agrega la accion al boton REGRESAR");
			regresar.onClick.AddListener(()=>{ActionButton_goBackToMenuPhases();});
		}

		//accion ir al Step1 del Phase


		//colocando las imagenes de las fases despues del encabezado (guia del proceso completo para los estudiantes)
		image_phase1_object = GameObject.Find ("image_phase1");
		if (image_phase1_object != null) {
			image_phase1_sprite = Resources.Load<Sprite>(image_phase1_path);
			image_phase1_object.GetComponent<Image>().sprite = image_phase1_sprite;
		}

		image_phase2_object = GameObject.Find ("image_phase2");
		if (image_phase2_object != null) {
			image_phase2_sprite = Resources.Load<Sprite>(image_phase2_path);
			image_phase2_object.GetComponent<Image>().sprite = image_phase2_sprite;
		}

		image_phase3_object = GameObject.Find ("image_phase3");
		if (image_phase3_object != null) {
			image_phase3_sprite = Resources.Load<Sprite>(image_phase3_path);
			image_phase3_object.GetComponent<Image>().sprite = image_phase3_sprite;
		}

		image_phase4_object = GameObject.Find ("image_phase4");
		if (image_phase4_object != null) {
			image_phase4_sprite = Resources.Load<Sprite>(image_phase4_path);
			image_phase4_object.GetComponent<Image>().sprite = image_phase4_sprite;
		}

		image_phase5_object = GameObject.Find ("image_phase5");
		if (image_phase5_object != null) {
			image_phase5_sprite = Resources.Load<Sprite>(image_phase5_path);
			image_phase5_object.GetComponent<Image>().sprite = image_phase5_sprite;
		}

		image_phase6_object = GameObject.Find ("image_phase6");
		if (image_phase6_object != null) {
			image_phase6_sprite = Resources.Load<Sprite>(image_phase6_path);
			image_phase6_object.GetComponent<Image>().sprite = image_phase6_sprite;
		}


		if (capoCarroButton != null) {
			image_button_sprite_uno = Resources.Load<Sprite> (image_uno_capo_carro);
			capoCarroButton.GetComponent<Image>().sprite = image_button_sprite_uno;
			////Agregando la accion para ir al step1 - phase 1 (buscar capo del carro):
			capoCarroButton.onClick.AddListener(()=>{ActionButton_goToActivitiesPhase1Step1();});
			if(step_one_enabled)
				capoCarroButton.interactable = true;
			else capoCarroButton.interactable = false;
		}
		
		if (limpiezaButton != null) {
			image_button_sprite_dos = Resources.Load<Sprite> (image_dos_limpieza);
			limpiezaButton.GetComponent<Image>().sprite = image_button_sprite_dos;
			//Agregando la accion para ir al conjunto de actividades (2. limpieza):
			limpiezaButton.onClick.AddListener(()=>{ActionButton_goToActivitiesPhase1Step2();});
			if(step_two_enabled)
				limpiezaButton.interactable = true;
			else 
				limpiezaButton.interactable = false;
		}
		
		if (secadoButton != null) {
			image_button_sprite_tres = Resources.Load<Sprite> (image_tres_secado);
			secadoButton.GetComponent<Image>().sprite = image_button_sprite_tres;
			//Agregando la accion para ir al conjunto de actividades (3. secado):
			secadoButton.onClick.AddListener(()=>{ActionButton_goToActivitiesPhase1Step3();});
			if(step_three_enabled)
				secadoButton.interactable = true;
			else 
				secadoButton.interactable = false;
		}
		
		if (irregularidadesButton != null) {
			image_button_sprite_cuatro = Resources.Load<Sprite> (image_cuatro_irregularidades);
			irregularidadesButton.GetComponent<Image>().sprite = image_button_sprite_cuatro;
			//Agregando la accion para ir al conjunto de actividades (4. localizar irregularidades):
			irregularidadesButton.onClick.AddListener(()=>{ActionButton_goToActivitiesPhase1Step4();});
			if(step_four_enabled)
				irregularidadesButton.interactable= true;
			else
				irregularidadesButton.interactable = false;
		}
		
		if (corregirButton != null) {
			image_button_sprite_cinco = Resources.Load<Sprite> (image_cinco_corregir);
			corregirButton.GetComponent<Image>().sprite = image_button_sprite_cinco;
			//Agregando la accion para ir al conjunto de actividades (5. corregir irregularidades):
			corregirButton.onClick.AddListener(()=>{ActionButton_goToActivitiesPhase1Step5();});
			if(step_five_enabled)
				corregirButton.interactable = true;
			else
				corregirButton.interactable = false;
		}
		
		if (desengrasadoButton != null) {
			image_button_sprite_seis = Resources.Load<Sprite> (image_seis_desengrasar);
			desengrasadoButton.GetComponent<Image>().sprite = image_button_sprite_seis;
			//Agregando la accion para ir al conjunto de actividades (5. corregir irregularidades):
			desengrasadoButton.onClick.AddListener(()=>{ActionButton_goToActivitiesPhase1Step6();});
			if(step_six_enabled)
				desengrasadoButton.interactable = true;
			else
				desengrasadoButton.interactable = false;
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
		this.goToActivitiesPhase1Step1 ();
	}

	/// <summary>
	/// Method for going to the activities of Phase1-Step2
	/// </summary>
	public void ActionButton_goToActivitiesPhase1Step2(){
		this.goToActivitiesPhase1Step2 ();
	}

	/// <summary>
	/// Method for going to the activities of Phase1-Step3
	/// </summary>
	public void ActionButton_goToActivitiesPhase1Step3(){
		this.goToActivitiesPhase1Step3 ();
	}

	/// <summary>
	/// Method for going to the activities of Phase1-Step4
	/// </summary>
	public void ActionButton_goToActivitiesPhase1Step4(){
		this.goToActivitiesPhase1Step4 ();
	}

	/// <summary>
	/// Method for going to the activities of Phase1-Step5
	/// </summary>
	public void ActionButton_goToActivitiesPhase1Step5(){
		this.goToActivitiesPhase1Step5 ();
	}

	/// <summary>
	/// Method for going to the activities of Phase1-Step6
	/// </summary>
	public void ActionButton_goToActivitiesPhase1Step6(){
		this.goToActivitiesPhase1Step6 ();
	}

	
	// Update is called once per frame
	void Update () {
	
	}
}
