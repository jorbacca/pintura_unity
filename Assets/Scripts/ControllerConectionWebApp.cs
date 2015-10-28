using UnityEngine;
using System.Collections;

public class ControllerConectionWebApp: MonoBehaviour {


	//variable TextAsset que lee la URL del servidor desde un archivo:
	private TextAsset server_path_asset;
	//variable que almacena la URL del servidor en forma de string:
	private string server_base_path;

	//Variables para la Co-rutina RutineValidateCodeEnabled:
	//variable que almacena el codigo del estudiante que se envia a traves del constructor
	public string codigo_estudiante;
	//variable que controla si ya se ha iniciado sesion:
	private string logged;
	//metodo delegado que permite notificar al AppManager si el codigo es valido o no valido:
	public delegate void NotificarEstadoCodigo(string log);
	
	public NotificarEstadoCodigo notifyCodeState;



	void Start () {
		server_path_asset = Resources.Load<TextAsset> ("Texts/00_server_base_path");
		if (server_path_asset != null)
			server_base_path = server_path_asset.text;
		else
			Debug.Log ("******ERROR ControllerLoginUser: No se ha podido leer la URL del servidor desde el archivo!!!*****");

	}



	/// <summary>
	/// Validates the state of the code.
	/// This method is called in order to validate if the student is authorized to use the application.
	/// Before calling this method you should call the ControllerConectionWebApp(string cod_estudiante) constructor
	/// </summary>
	public void ValidateCodeState(string cod_estudiante){

		codigo_estudiante = cod_estudiante;
		//inicio de la corrutina que valida si el codigo esta habilitado:
		StartCoroutine("RutineValidateCodeEnabled");

	}




	/// <summary>
	/// Rutines for validating the code against the server.
	/// </summary>
	/// <returns>The validate code.</returns>
	IEnumerator RutineValidateCodeEnabled(){
		Debug.Log ("Se inicia co-rutina RutineValidateCodeEnabled en ControllerConnectionWebApp!!");
		// Create a Web Form
		WWWForm form = new WWWForm();
		//definiendo el campo codigo que se envia por metodo post hacia el servidor
		form.AddField("codigo", this.codigo_estudiante );
		
		Debug.Log ("ControllerConnectionWebApp: Se va a enviar el formulario !!");
		// Upload to a cgi script
		WWW ww = new WWW(server_base_path + "validateauth.php", form);
		yield return ww;
		if (!string.IsNullOrEmpty(ww.error)) {
			Debug.Log ("ControllerConnectionWebApp: ERROR EN LA TRANSFERENCIA AL SERVIDOR en RutineValidateCodeEnabled de ControllerConectionWebApp" + ww.error);
			logged = "error";

		}
		else {
			Debug.Log ("Transferencia satisfactoria!!! --> " + ww.text);
			if(ww.text == "0"){
				logged = "false";
			} else if(ww.text == "1")  {
				logged = "true";
			} else {
				Debug.Log ("La respuesta del servicio ha sido diferente de 1 o 0 y no esta notificando de un error en RutineValidateCodeEnabled de ControllerConectionWebApp!!!");
				logged = "error";
			} //cierra else interno

		} //cierra else

		//Notificando mediante metodo delegado al AppManager que ya se ha iniciado session:
		this.notifyCodeState(logged);
		
	} //cierra co-rutina RutineValidateCode

}
