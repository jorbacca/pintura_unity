using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ControllerLoginUser : MonoBehaviour {

	//variable que referencia el componente de texto que muestra un error en la interfaz:
	public Text TextErrorMessageObject;

	//Referencia al objeto input en el que el estudiante escribe el codigo:
	public Text codigo_estud_interface_obj;
	//variable que almacena el codigo del estud que se ingresa por la interfaz
	private string codigo_ingresado;
	//variable TextAsset que lee la URL del servidor desde un archivo:
	private TextAsset server_path_asset;
	//variable que almacena la URL del servidor en forma de string:
	private string server_base_path;

	//variable que controla si ya se ha iniciado sesion:
	private bool logged;

	//metodo delegado que permite notificar al AppManager de que ya se ha inicado sesion:
	public delegate void NotificarInicioSesion(bool logged);

	public NotificarInicioSesion notifyUserLogin;

	// Use this for initialization
	void Start () {
		if (TextErrorMessageObject != null)
			TextErrorMessageObject.enabled = false;

		server_path_asset = Resources.Load<TextAsset> ("Texts/00_server_base_path");
		if (server_path_asset != null)
			server_base_path = server_path_asset.text;
		else
			Debug.Log ("******ERROR ControllerLoginUser: No se ha podido leer la URL del servidor desde el archivo!!!*****");

		logged = false;
	}

	/// <summary>
	/// Raises the click ingresar button event.
	/// Cuando se hace click sobre el boton Ingresar de la interfaz
	/// </summary>
	public void OnClickIngresarButton(){
		Debug.Log ("ControllerLoginUser: Se hace click sobre el boton ingresar.");
		if (codigo_estud_interface_obj != null) {
			Debug.Log ("ControllerLoginUser: El codigo que se ha ingresado es: " + codigo_estud_interface_obj.text);
			codigo_ingresado = codigo_estud_interface_obj.text;
			if(!string.IsNullOrEmpty(codigo_ingresado))
				StartCoroutine("RutineValidateCode");
			else Debug.LogError("No se ha ingresado ningun codigo");

		}

	}


	public void OnClickEndEditIdNumber(){
		Debug.Log ("Se ha finalizado la edicion!!!");
	}

	public void OnClickCancelButton(){
		Application.Quit ();
	}

	public void OnClickInVirtualKeyboard(string num){
		if (TextErrorMessageObject != null)
			TextErrorMessageObject.enabled = false;

		Debug.Log ("Click sobre el boton: " + num);
		if (codigo_estud_interface_obj != null) {
			codigo_estud_interface_obj.text = codigo_estud_interface_obj.text + num;
		} else {
			Debug.LogError("Error: El teclado virtual no funciona!.");
			if (TextErrorMessageObject != null){
				TextErrorMessageObject.text = "El teclado virtual NO esta funcionando.";
				TextErrorMessageObject.enabled = true;
			}
		}
	}

	public void EraseCodeVirtualKeyboard(){
		if (codigo_estud_interface_obj.text.Length > 0) {
			string codigo = codigo_estud_interface_obj.text;
			codigo = codigo.Substring(0, codigo.Length - 1);
			codigo_estud_interface_obj.text = codigo;
		}

	}


	/// <summary>
	/// Rutines for validating the code against the server.
	/// </summary>
	/// <returns>The validate code.</returns>
	IEnumerator RutineValidateCode(){
		Debug.Log ("Se inicia co-rutina RutineValidateCode!!");
		// Create a Web Form
		WWWForm form = new WWWForm();
		//definiendo el campo codigo que se envia por metodo post hacia el servidor
		form.AddField("codigo", codigo_ingresado );

		Debug.Log ("Se va a enviar el formulario !!");
		// Upload to a cgi script
		WWW ww = new WWW(server_base_path + "validateuser.php", form);
		yield return ww;
		if (!string.IsNullOrEmpty(ww.error)) {
			Debug.Log ("ControllerLoginUser: ERROR EN LA TRANSFERENCIA AL SERVIDOR" + ww.error);
			logged = false;
			if (TextErrorMessageObject != null){
				TextErrorMessageObject.text = "Error 1: Verifica que estas conectado a Internet.";
				TextErrorMessageObject.enabled = true;

			}
		}
		else {
			Debug.Log ("Transferencia satisfactoria!!! --> " + ww.text);
			if(ww.text == "0"){
				//se va a notificar el error:
				if (TextErrorMessageObject != null){
					TextErrorMessageObject.text = "El codigo NO es valido o el estudiante NO esta activo.";
					TextErrorMessageObject.enabled = true;
				}
				logged = false;
			} else if(ww.text == "1")  {
				logged = true;
				AppManager.manager.codigo_estudiante = codigo_ingresado;
			} else {
				Debug.Log ("La respuesta del servicio ha sido diferente de 1 o 0 y no esta notificando de un error!!!");
				logged = false;
				if (TextErrorMessageObject != null){
					TextErrorMessageObject.text = "Error 2: Verifica que tienes conexion a internet.";
					TextErrorMessageObject.enabled = true;
				}
			}

			//Notificando mediante metodo delegado al AppManager que ya se ha iniciado session:
			this.notifyUserLogin(logged);
		}
		
	} //cierra co-rutina RutineValidateCode

	
}
