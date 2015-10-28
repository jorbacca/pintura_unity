using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ControllerConfigurationOptions : MonoBehaviour {

	//Referencia al objeto Text de la interfaz de usuario donde se coloca el codigo del estudiante:
	public Text codigoEstudianteObject;

	//referencia al objeto de la interfaz donde se debe colocar el texto de la introduccion:
	public Text intro_text_int_object;

	//variable que almacena la ruta al archivo donde se encuentra el texto de la introduccion:
	public string intro_text_path_config_opts;

	//variable que almacena el texto cargado desde el archivo:
	private TextAsset texto_asset;

	//Metodo delegado que ejecuta el metodo de reinicio del modo guiado en el AppManager:
	public System.Action ActionRestartGuidedMode;

	//Metodo delegado que ejecuta el metodo de reinicio del modo evaluacion en el AppManager:
	public System.Action ActionRestartEvaluationMode;

	//metodo delegador que permite regresar a la interfaz de selection of mode:
	public System.Action ActionGoBackSelectionMode;



	// Use this for initialization
	void Start () {

		//inicializando la introduccion con el texto correspondiente:
		if (intro_text_int_object != null) {
			texto_asset = Resources.Load<TextAsset>(intro_text_path_config_opts);
			intro_text_int_object.text = texto_asset.text;
		}

		if (codigoEstudianteObject != null) {
			codigoEstudianteObject.text = codigoEstudianteObject.text + AppManager.manager.codigo_estudiante;
		} else {
			codigoEstudianteObject.text = "No se ha podido obtener el Codigo de Estudiante";
		}

	} //cierra metodo start()

	/// <summary>
	/// Raises the click restart guided mode event.
	/// Este metodo reinicia el modo guiado para que los estudiantes comiencen desde 0 todo del proceso
	/// </summary>
	public void OnClickRestartGuidedMode(){
		if (this.ActionRestartGuidedMode != null) {
			this.ActionRestartGuidedMode ();
		}
		else 
			Debug.LogError ("No se ha definido el metodo ActionRestartGuidedMode en ControllerConfigurationOptions y en AppManager");

	}

	/// <summary>
	/// Raises the click restart evaluation mode event.
	/// Este metodo reinicia el modo evaluacion para que los estudiantes comiencen desde 0 todo el proceso:
	/// </summary>
	public void OnClickRestartEvaluationMode(){
		if (this.ActionRestartEvaluationMode != null) {
			this.ActionRestartEvaluationMode ();

		} else
			Debug.LogError ("No se ha definido el metodo ActionRestartEvaluationMode en ControllerConfigurationOptions y en AppManager");
	}

	/// <summary>
	/// Gos the back to selection of mode.
	/// Este metodo es invocado al presionar el boton atras de la interfaz de configuracion
	/// </summary>
	public void GoBackToSelectionOfMode(){
		if (ActionGoBackSelectionMode != null)
			this.ActionGoBackSelectionMode ();
		else
			Debug.LogError ("No se ha definido el metodo ActionGoBackSelectionMode en ControllerConfigOptions y en AppManager");
	}
	

}
