using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuOfStepsMatizadoSubMenu : MonoBehaviour {

	public string image_header_phase1;
	public string titulo;
	public string introduction_text_path;
	public Button button_one;
	public Button button_two;
	public Button button_three;
	public Button button_four;
	public Button button_five;
	public Button button_six;
	public Button regresar;
	
	//Referencia a los componentes Text de los botones:
	public Text text_btn_one;
	public Text text_btn_two;
	public Text text_btn_three;
	public Text text_btn_four;
	public Text text_btn_five;
	public Text text_btn_six;
	
	//rutas a las imagenes que se deben cargar (numeradas para leerse de izquierda a derecha en la interfaz):
	public string image_one_path;
	public string image_two_path;
	public string image_three_path;
	public string image_four_path;
	public string image_five_path;
	public string image_six_path;
	
	//Rutas a las imagenes de las fases para la guia de las fases:
	public string image_phase1_path;
	public string image_phase2_path;
	public string image_phase3_path;
	public string image_phase4_path;
	public string image_phase5_path;
	public string image_phase6_path;

	//Strings de las rutas a las imagenes de las sub-phases:
	public string image_subphase_one_path;
	public string image_subphase_two_path;
	public string image_subphase_three_path;
	public string image_subphase_four_path;
	public string image_subphase_five_path;
	public string image_subphase_six_path;
	
	//Textos de los nombres de los pasos:
	public string button_one_text_string;
	public string button_two_text_string;
	public string button_three_text_string;
	public string button_four_text_string;
	public string button_five_text_string;
	public string button_six_text_string;
	
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

	//GameObject para obtener las referencias a los componentes Image que muestran los pasos del proceso
	public Image image_subphase_one_obj;
	public Image image_subphase_two_obj;
	public Image image_subphase_three_obj;
	public Image image_subphase_four_obj;
	public Image image_subphase_five_obj;
	public Image image_subphase_six_obj;
	
	
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
	
	//Sprites para configurar las referencias a las imagenes de las fases:
	private Sprite image_phase1_sprite;
	private Sprite image_phase2_sprite;
	private Sprite image_phase3_sprite;
	private Sprite image_phase4_sprite;
	private Sprite image_phase5_sprite;
	private Sprite image_phase6_sprite;

	//Sprites para configurar las imagenes de las subphases:
	private Sprite image_subphase_one_sprite;
	private Sprite image_subphase_two_sprite;
	private Sprite image_subphase_three_sprite;
	private Sprite image_subphase_four_sprite;
	private Sprite image_subphase_five_sprite;
	private Sprite image_subphase_six_sprite;
	
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
	
	//variables que controlan si los botones son visibles o no:
	public bool step_one_btn_visible;
	public bool step_two_btn_visible;
	public bool step_three_btn_visible;
	public bool step_four_btn_visible;
	public bool step_five_btn_visible;
	public bool step_six_btn_visible;

	//variables booleanas que controlan cuales de las imagenes de los encabezados son visibles
	public bool sub_phase_one_visible;
	public bool sub_phase_two_visible;
	public bool sub_phase_three_visible;
	public bool sub_phase_four_visible;
	public bool sub_phase_five_visible;
	public bool sub_phase_six_visible;
	
	
	// Use this for initialization
	void Start () {
		Debug.Log ("Llamado al metodo START de MenuOfStepsMatizadoManager NORMAL Instancia: " + this.gameObject.GetInstanceID() + " name: " + this.name);
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


		//esta seccion carga las imagenes de las subphases o de las imagenes que se muestran debajo de las fases
		//se valida si la imagen se debe mostrar entonces se carga, sino entonces se oculta la imagen.
		if (sub_phase_one_visible) {
			image_subphase_one_sprite = Resources.Load<Sprite> (image_subphase_one_path);
			image_subphase_one_obj.sprite = image_subphase_one_sprite;
		} else {
			image_subphase_one_obj.enabled = false;
		}

		if (sub_phase_two_visible) {
			image_subphase_two_sprite = Resources.Load<Sprite> (image_subphase_two_path);
			image_subphase_two_obj.sprite = image_subphase_two_sprite;
		} else {
			image_subphase_two_obj.enabled = false;
		}

		if (sub_phase_three_visible) {
			image_subphase_three_sprite = Resources.Load<Sprite> (image_subphase_three_path);
			image_subphase_three_obj.sprite = image_subphase_three_sprite;
		} else {
			image_subphase_three_obj.enabled = false;
		}

		if (sub_phase_four_visible) {
			image_subphase_four_sprite = Resources.Load<Sprite> (image_subphase_four_path);
			image_subphase_four_obj.sprite = image_subphase_four_sprite;
		} else {
			image_subphase_four_obj.enabled = false;
		}

		if (sub_phase_five_visible) {
			image_subphase_five_sprite = Resources.Load<Sprite> (image_subphase_five_path);
			image_subphase_five_obj.sprite = image_subphase_five_sprite;
		} else {
			image_subphase_five_obj.enabled = false;
		}

		if (sub_phase_six_visible) {
			image_subphase_six_sprite = Resources.Load<Sprite> (image_subphase_six_path);
			image_subphase_six_obj.sprite = image_subphase_six_sprite;
		} else {
			image_subphase_six_obj.enabled = false;
		}

		//aca termina la carga de imagenes en la parte de las subphases
		
		if (button_one != null) {
			if(step_one_btn_visible){
				Debug.Log ("El boton 1 debe ser visible y se van a cargar los datos");
				image_button_sprite_uno = Resources.Load<Sprite> (image_one_path);
				button_one.GetComponent<Image>().sprite = image_button_sprite_uno;
				////Agregando la accion para ir al step1 - phase 1 (buscar capo del carro):
				button_one.onClick.AddListener(()=>{ActionButton_goToActivitiesPhase1Step1();});
				if(text_btn_one != null){
					text_btn_one.text = this.button_one_text_string;
				}
				if(step_one_enabled){
					button_one.interactable = true;
				} else {
					//el boton no se puede presionar:
					button_one.interactable = false;
				}
			} else {
				//deshabilitando la imagen del btn para desaparecerlo:
				Image componente_imagen = button_one.GetComponent<Image>();
				if(componente_imagen != null)
					componente_imagen.enabled = false;
				//colocando el texto en vacio porque el btn no debe estar habilitado
				text_btn_one.text = "";
			}
		} //cierra button_one != null
		
		if (button_two != null) {
			if(step_two_btn_visible){
				Debug.Log ("El boton 2 debe ser visible y se van a cargar los datos");
				image_button_sprite_dos = Resources.Load<Sprite> (image_two_path);
				button_two.GetComponent<Image>().sprite = image_button_sprite_dos;
				//Agregando la accion para ir al conjunto de actividades (2. limpieza):
				button_two.onClick.AddListener(()=>{ActionButton_goToActivitiesPhase1Step2();});
				if(text_btn_two != null){
					text_btn_two.text = this.button_two_text_string;
				}
				if(step_two_enabled){
					button_two.interactable = true;
					
				} else {
					button_two.interactable = false;
				}
			}else {
				//deshabilitando la imagen del btn para desaparecerlo:
				Image componente_imagen = button_two.GetComponent<Image>();
				if(componente_imagen != null)
					componente_imagen.enabled = false;
				//colocando el texto en vacio porque el btn no debe estar habilitado
				text_btn_two.text = "";
			}
		} //cierra if button_two != null
		
		if (button_three != null) {
			if(step_three_btn_visible){
				Debug.Log ("El boton 3 debe ser visible y se van a cargar los datos");
				image_button_sprite_tres = Resources.Load<Sprite> (image_three_path);
				button_three.GetComponent<Image>().sprite = image_button_sprite_tres;
				//Agregando la accion para ir al conjunto de actividades (3. secado):
				button_three.onClick.AddListener(()=>{ActionButton_goToActivitiesPhase1Step3();});
				if(text_btn_three != null){
					text_btn_three.text = this.button_three_text_string;
				}
				if(step_three_enabled){
					button_three.interactable = true;
					
					
				} else {
					button_three.interactable = false;
				}
			} else {
				//deshabilitando la imagen del btn para desaparecerlo:
				Image componente_imagen = button_three.GetComponent<Image>();
				if(componente_imagen != null)
					componente_imagen.enabled = false;
				//colocando el texto en vacio porque el btn no debe estar habilitado
				text_btn_three.text = "";
			}	
		}//cierra button_three != null
		
		if (button_four != null) {
			if(step_four_btn_visible){
				Debug.Log ("El boton 4 debe ser visible y se van a cargar los datos");
				image_button_sprite_cuatro = Resources.Load<Sprite> (image_four_path);
				button_four.GetComponent<Image>().sprite = image_button_sprite_cuatro;
				//Agregando la accion para ir al conjunto de actividades (4. localizar irregularidades):
				button_four.onClick.AddListener(()=>{ActionButton_goToActivitiesPhase1Step4();});
				if(text_btn_four != null){
					text_btn_four.text = this.button_four_text_string;
				}
				if(step_four_enabled){
					button_four.interactable= true;
					
				} else {
					button_four.interactable = false;
				}
			} else {
				//deshabilitando la imagen del btn para desaparecerlo:
				Image componente_imagen = button_four.GetComponent<Image>();
				if(componente_imagen != null)
					componente_imagen.enabled = false;
				//colocando el texto en vacio porque el btn no debe estar habilitado
				text_btn_four.text = "";
			}
		}// cierra button_four != null
		
		if (button_five != null) {
			if(step_five_btn_visible){
				Debug.Log ("El boton 5 debe ser visible y se van a cargar los datos");
				image_button_sprite_cinco = Resources.Load<Sprite> (image_five_path);
				button_five.GetComponent<Image>().sprite = image_button_sprite_cinco;
				//Agregando la accion para ir al conjunto de actividades (5. corregir irregularidades):
				button_five.onClick.AddListener(()=>{ActionButton_goToActivitiesPhase1Step5();});
				if(text_btn_five != null){
					text_btn_five.text = this.button_five_text_string;
				}
				if(step_five_enabled){
					button_five.interactable = true;
					
				} else {
					button_five.interactable = false;
				}
			} else {
				//deshabilitando la imagen del btn para desaparecerlo:
				Image componente_imagen = button_five.GetComponent<Image>();
				if(componente_imagen != null)
					componente_imagen.enabled = false;
				//colocando el texto en vacio porque el btn no debe estar habilitado
				text_btn_five.text = "";
			}
		} //cierra button_five != null
		
		if (button_six != null) {
			if(step_six_btn_visible){
				Debug.Log ("El boton 6 debe ser visible y se van a cargar los datos");
				image_button_sprite_seis = Resources.Load<Sprite> (image_six_path);
				button_six.GetComponent<Image>().sprite = image_button_sprite_seis;
				//Agregando la accion para ir al conjunto de actividades (5. corregir irregularidades):
				button_six.onClick.AddListener(()=>{ActionButton_goToActivitiesPhase1Step6();});
				if(text_btn_six != null){
					text_btn_six.text = this.button_six_text_string;
				}
				if(step_six_enabled){
					button_six.interactable = true;
					
				} else{ 
					button_six.interactable = false;
				}
			} else {
				//deshabilitando la imagen del btn para desaparecerlo:
				Image componente_imagen = button_six.GetComponent<Image>();
				if(componente_imagen != null)
					componente_imagen.enabled = false;
				//colocando el texto en vacio porque el btn no debe estar habilitado
				text_btn_six.text = "";
			}
		} //cierra button_six != null
		
		//colocando los textos en los botones:
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
