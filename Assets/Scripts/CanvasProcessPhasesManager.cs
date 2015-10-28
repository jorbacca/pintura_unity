using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CanvasProcessPhasesManager : MonoBehaviour {

	public string titulo;
	public string introduction_text_path;
	public string continue_text_path;
	public Button limpiezaButton;
	public Button matizadoButton;
	public Button masilladoButton;
	public Button aparejadoButton;
	public Button pintadoButton;
	public Button barnizadoButton;

	//referencia al boton de regresar:
	public Button regresar;

	public Text textUnoLimpieza;
	public Text textDosMatizado;
	public Text textTresMasillado;
	public Text textCuatroAparejado;
	public Text textCincoPintado;
	public Text textSeisBarnizado;

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

	//variables que controlan si los botones deben aparecer activados o desactivados:
	public bool phase_one_button_enable;
	public bool phase_two_button_enable;
	public bool phase_three_button_enable;
	public bool phase_four_button_enable;
	public bool phase_five_button_enable;
	public bool phase_six_button_enable;

	// Use this for initialization
	void Start () {
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
			if(phase_one_button_enable)
				limpiezaButton.interactable = true;
			else limpiezaButton.interactable = false;

		}

		if (matizadoButton != null) {
			image_button_sprite_dos = Resources.Load<Sprite> (image_dos_matizado);
			matizadoButton.GetComponent<Image>().sprite = image_button_sprite_dos;
			matizadoButton.onClick.AddListener(()=>{ActionButton_goToMenuStepsOfPhase2();});
			if(phase_two_button_enable)
				matizadoButton.interactable = true;
			else matizadoButton.interactable = false;
		}

		if (masilladoButton != null) {
			image_button_sprite_tres = Resources.Load<Sprite> (image_tres_masillado);
			masilladoButton.GetComponent<Image>().sprite = image_button_sprite_tres;
			masilladoButton.onClick.AddListener(()=>{ActionButton_goToMenuStepsOfPhase3();});
			if(phase_three_button_enable)
				masilladoButton.interactable = true;
			else masilladoButton.interactable = false;
		}

		if (aparejadoButton != null) {
			image_button_sprite_cuatro = Resources.Load<Sprite> (image_cuatro_aparejado);
			aparejadoButton.GetComponent<Image>().sprite = image_button_sprite_cuatro;
			aparejadoButton.onClick.AddListener(()=>{ActionButton_goToMenuStepsOfPhase4();});
			if(phase_four_button_enable)
				aparejadoButton.interactable = true;
			else aparejadoButton.interactable = false;
		}

		if (pintadoButton != null) {
			image_button_sprite_cinco = Resources.Load<Sprite> (image_cinco_pintado);
			pintadoButton.GetComponent<Image>().sprite = image_button_sprite_cinco;
			pintadoButton.onClick.AddListener(()=>{ActionButton_goToMenuStepsOfPhase5();});
			if(phase_five_button_enable)
				pintadoButton.interactable = true;
			else pintadoButton.interactable = false;
		}

		if (barnizadoButton != null) {
			image_button_sprite_seis = Resources.Load<Sprite> (image_seis_barnizado);
			barnizadoButton.GetComponent<Image>().sprite = image_button_sprite_seis;
			barnizadoButton.onClick.AddListener(()=>{ActionButton_goToMenuStepsOfPhase6();});
			if(phase_six_button_enable)
				barnizadoButton.interactable = true;
			else barnizadoButton.interactable = false;
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





	}

	/// <summary>
	/// Method for going to the menu of steps for phase 1
	/// </summary>
	public void ActionButton_goToMenuStepsOfPhase1(){
		if (goToMenuStepsOfPhase1_action != null)
			this.goToMenuStepsOfPhase1_action ();
		else
			Debug.LogError ("No se ha definido una accion para goToMenuStepsOfPhase1 en CanvasProcessPhaseManager");
	}

	/// <summary>
	/// Method for going to the menu of steps for phase 2
	/// </summary>
	public void ActionButton_goToMenuStepsOfPhase2(){
		if (goToMenuStepsOfPhase2_action != null)
			this.goToMenuStepsOfPhase2_action ();
		else
			Debug.LogError ("No se ha definido una accion para goToMenuStepsOfPhase2 en CanvasProcessPhaseManager");
	}

	/// <summary>
	/// Method for going to the menu of steps for phase 3
	/// </summary>
	public void ActionButton_goToMenuStepsOfPhase3(){
		if (goToMenuStepsOfPhase3_action != null)
			this.goToMenuStepsOfPhase3_action ();
		else
			Debug.LogError ("No se ha definido una accion para goToMenuStepsOfPhase3 en CanvasProcessPhaseManager");
	}

	/// <summary>
	/// Method for going to the menu of steps for phase 4
	/// </summary>
	public void ActionButton_goToMenuStepsOfPhase4(){
		if (goToMenuStepsOfPhase4_action != null)
			this.goToMenuStepsOfPhase4_action ();
		else
			Debug.LogError ("No se ha definido una accion para goToMenuStepsOfPhase4 en CanvasProcessPhaseManager");
	}

	/// <summary>
	/// Method for going to the menu of steps for phase 5
	/// </summary>
	public void ActionButton_goToMenuStepsOfPhase5(){
		if (goToMenuStepsOfPhase5_action != null)
			this.goToMenuStepsOfPhase5_action ();
		else
			Debug.LogError ("No se ha definido una accion para goToMenuStepsOfPhase5 en CanvasProcessPhaseManager");
	}

	/// <summary>
	/// Method for going to the menu of steps for phase 6
	/// </summary>
	public void ActionButton_goToMenuStepsOfPhase6(){
		if (goToMenuStepsOfPhase6_action != null)
			this.goToMenuStepsOfPhase6_action ();
		else
			Debug.LogError ("No se ha definido una accion para goToMenuStepsOfPhase6 en CanvasProcessPhaseManager");
	}

	public void ActionButton_goToSelectionMode(){
		if (goBackToSelectionOfMode != null)
			this.goBackToSelectionOfMode ();
		else
			Debug.Log ("No se ha definido la accion para el boton regresar en ActionButton_goToSelectionMode de CanvasProcessPhaseManager");
	}



	// Update is called once per frame
	void Update () {
	
	}
}
