using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CanvasSelectionOfModeManager : MonoBehaviour {


	public string titulo;
	public string introduction_text_path;
	public Button button_guided_mode;
	public Button button_evaluation_mode;
	public Button button_informative_mode;
	public Button config_options_button;

	//variable que controla si el modo evaluacion estara habilitado:
	public bool evaluation_mode_enab;

	private GameObject titulo_object;
	private GameObject introduction_object;
	//private GameObject button_guided_mode;

	private TextAsset introduction_asset;

	public System.Action guidedMode_action;
	public System.Action evaluationMode_action;
	public System.Action informativeMode_action;
	public System.Action configurationOptions_action;

	//variables para cargar los fondos de los botones:
	private Sprite fondo_boton;


	/// <summary>
	/// Actions the button_guided mode.
	/// </summary>
	public void ActionButton_guidedMode(){
		this.guidedMode_action ();
	}

	/// <summary>
	/// Actions the button_evaluation mode.
	/// </summary>
	public void ActionButton_evaluationMode(){
		this.evaluationMode_action ();
	}

	/// <summary>
	/// Actions the button_guided mode.
	/// </summary>
	public void ActionButton_informativeMode(){
		this.informativeMode_action ();
	}

	/// <summary>
	/// Actions the button_config options.
	/// </summary>
	public void ActionButton_configOptions(){
		if (this.configurationOptions_action != null)
			this.configurationOptions_action ();
		else
			Debug.LogError ("No se ha definido la accion del boton configurationOptions_action en CanvasSelectionOfModeManag");
	}

	// Use this for initialization
	void Start () {
		titulo_object = GameObject.Find ("title_selection_mode_interface");
		if (titulo != null) {
			Debug.Log("Cambiando el titulo de la interfaz en SelectionOfMode");
			titulo_object.GetComponent<Text>().text = titulo;
		}
		
		introduction_object = GameObject.Find ("introduction_selection_mode_interface");
		if (introduction_object != null) {
			Debug.Log("Se va a cambiar el texto central en SelectionOfMode");
			introduction_asset = Resources.Load(introduction_text_path) as TextAsset;
			introduction_object.GetComponent<Text>().text = introduction_asset.text;
		}

		//button_guided_mode = GameObject.Find ("guided_mode_button");
		if(button_guided_mode != null){
			Debug.LogError("Se agrega la accion al boton en SelectionOfMode");
			fondo_boton = Resources.Load<Sprite>("Sprites/modo_guiado");
			button_guided_mode.GetComponent<Image>().sprite = fondo_boton;
			button_guided_mode.onClick.AddListener(()=>{ActionButton_guidedMode();}); 
		}

		if (button_evaluation_mode != null) {
			Debug.Log("Se agrega la accion al boton en EvaluationMode");
			if(evaluation_mode_enab){
				fondo_boton = Resources.Load<Sprite>("Sprites/modo_libre");
				button_evaluation_mode.GetComponent<Image>().sprite = fondo_boton;
				button_evaluation_mode.interactable = true;
			}else {
				fondo_boton = Resources.Load<Sprite>("Sprites/modo_libre_gray");
				button_evaluation_mode.GetComponent<Image>().sprite = fondo_boton;
				button_evaluation_mode.interactable = false;
			}
			button_evaluation_mode.onClick.AddListener(()=>{ActionButton_evaluationMode();});
		}

		if (button_informative_mode != null) {
			Debug.Log("Se agrega la accion al boton en InformativeMode");
			fondo_boton = Resources.Load<Sprite>("Sprites/modo_consulta");
			button_informative_mode.GetComponent<Image>().sprite = fondo_boton;
			button_informative_mode.onClick.AddListener(()=>{ActionButton_informativeMode();});

		}

		if (config_options_button != null) {
			Debug.Log("Se agrega la accion al boton en InformativeMode");
			config_options_button.onClick.AddListener(()=>{ActionButton_configOptions();});
		}


	}// cierra metodo start

	/// <summary>
	/// Metodo que le permite al estudiante cerrar la apliacion
	/// </summary>
	public void SalirAplicacion(){
		//se llama al metodo que guarda los datos en la BD local del movil antes de cerrar la aplicacion
		NavigationControllerObject.navigation.GuardarDatosInmediatamente ();
		Application.Quit();
	}

}
