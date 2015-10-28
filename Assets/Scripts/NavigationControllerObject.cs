using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NavigationControllerObject : MonoBehaviour {
	//variable estatica para permitir el acceso desde otros scripts:
	public static NavigationControllerObject navigation;

	//variable que almacena el listado de INTERFACES que se van almacenando:
	public List<InterfaceInformation> interfaces;

	//variable para almacenar el codigo del estudiante que es unico para todos los registros:
	public string student_code;

	//variable que controla la tarea asincrona
	ThreadedStoreDataLocalDB saveData;

	// Use this for initialization
	void Start () {
		if (navigation == null) {
			DontDestroyOnLoad (gameObject);
			navigation = this;
		} else if (navigation != this) {
			Destroy(gameObject);
		}

		//inicializando lista de interfaces:
		interfaces = new List<InterfaceInformation> ();

		//inicializado en null 
		saveData = null;

	}//cierra metodo start

	public void ReinicializarListaInterfaces(){
		interfaces = new List<InterfaceInformation> ();
	}

	/// <summary>
	/// Metodo que permite registrar el acceso a una interfaz en el modo guiado.
	/// </summary>
	/// <param name="cod_estud">Cod_estud.</param>
	/// <param name="fecha">Fecha.</param>
	/// <param name="cod_interf">Cod_interf.</param>
	/// <param name="paso">Paso.</param>
	/// <param name="mark_err">Mark_err.</param>
	public void RegistrarInterfazModoGuiado(string cod_estud, string fecha, string cod_interf, string paso, string mark_err){
		Debug.Log ("Se reciben los datos para notificar el acceso a una interfaz: estud=" + cod_estud + ",fecha=" + fecha + ",cod_interf=" + cod_interf + ",paso=" + paso + ",mark_err=" + mark_err);
		Debug.Log ("El codigo del estudiante asignado desde el AppManager es: " + student_code);
		InterfaceInformation interface_reg = new InterfaceInformation (cod_estud, fecha, cod_interf, paso, mark_err, "guiado");
		interfaces.Add (interface_reg);
		//for debugging:
		//imprimir_lista ();
	} //cierra metodo RegistrarInterfazModoGuiado

	/// <summary>
	/// Metodo que permite registrar el acceso a una interfaz en modo guiado con parametro entero para el marcador
	/// </summary>
	/// <param name="cod_estud">Cod_estud.</param>
	/// <param name="fecha">Fecha.</param>
	/// <param name="cod_interf">Cod_interf.</param>
	/// <param name="paso">Paso.</param>
	/// <param name="mark_err">Mark_err.</param>
	public void RegistrarInterfazModoGuiado(string cod_estud, string fecha, string cod_interf, string paso, int mark_err){
		Debug.Log ("Se reciben los datos para notificar el acceso a una interfaz: estud=" + cod_estud + ",fecha=" + fecha + ",cod_interf=" + cod_interf + ",paso=" + paso + ",mark_err=" + mark_err);
		Debug.Log ("El codigo del estudiante asignado desde el AppManager es: " + student_code);
		string marker_error = mark_err.ToString ();
		InterfaceInformation interface_reg = new InterfaceInformation (cod_estud, fecha, cod_interf, paso, marker_error,"guiado");
		interfaces.Add (interface_reg);
		//for debugging:
		//imprimir_lista (); 
	} //cierra metodo RegistrarInterfazModoGuiado


	/// <summary>
	/// Metodo que permite registrar el acceso a una interfaz en el modo guiado.
	/// </summary>
	/// <param name="cod_estud">Cod_estud.</param>
	/// <param name="fecha">Fecha.</param>
	/// <param name="cod_interf">Cod_interf.</param>
	/// <param name="paso">Paso.</param>
	/// <param name="mark_err">Mark_err.</param>
	public void RegistrarInterfazModoEvalConsult(string cod_estud, string fecha, string cod_interf, string paso, string mark_err, string tipo_naveg){
		Debug.Log ("Se reciben los datos para notificar el acceso a una interfaz: estud=" + cod_estud + ",fecha=" + fecha + ",cod_interf=" + cod_interf + ",paso=" + paso + ",mark_err=" + mark_err + ",tipo=" + tipo_naveg);
		Debug.Log ("El codigo del estudiante asignado desde el AppManager es: " + student_code);
		InterfaceInformation interface_reg = new InterfaceInformation (cod_estud, fecha, cod_interf, paso, mark_err, tipo_naveg);
		interfaces.Add (interface_reg);
		//for debugging:
		//imprimir_lista ();
	} //cierra metodo RegistrarInterfazModoGuiado


	/// <summary>
	/// Metodo que permite registrar el acceso a una interfaz en modo guiado con parametro entero para el marcador
	/// </summary>
	/// <param name="cod_estud">Cod_estud.</param>
	/// <param name="fecha">Fecha.</param>
	/// <param name="cod_interf">Cod_interf.</param>
	/// <param name="paso">Paso.</param>
	/// <param name="mark_err">Mark_err.</param>
	public void RegistrarInterfazModoEvalConsult(string cod_estud, string fecha, string cod_interf, string paso, int mark_err, string tipo_naveg){
		Debug.Log ("Se reciben los datos para notificar el acceso a una interfaz: estud=" + cod_estud + ",fecha=" + fecha + ",cod_interf=" + cod_interf + ",paso=" + paso + ",mark_err=" + mark_err + ",tipo=" + tipo_naveg);
		Debug.Log ("El codigo del estudiante asignado desde el AppManager es: " + student_code);
		string marker_error = mark_err.ToString ();
		InterfaceInformation interface_reg = new InterfaceInformation (cod_estud, fecha, cod_interf, paso, marker_error,tipo_naveg);
		interfaces.Add (interface_reg);
		//for debugging:
		//imprimir_lista (); 
	} //cierra metodo RegistrarInterfazModoGuiado

	public void imprimir_lista(){
		foreach(InterfaceInformation int_info in interfaces){
			Debug.Log ("Interfaz registrada: codigo=" + int_info.Codigo_interfaz + ",fecha=" + int_info.Fecha);
		}
	}

	public void RegistrarReinicioDeModo(string cod_estud, string fecha, string cod_interf, string paso, string mark_err, string tipo_modo){
		Debug.Log ("Se reciben los datos para notificar el acceso a una interfaz: estud=" + cod_estud + ",fecha=" + fecha + ",cod_interf=" + cod_interf + ",paso=" + paso + ",mark_err=" + mark_err + ",tipo=" + tipo_modo);
		InterfaceInformation interface_reg = new InterfaceInformation (cod_estud, fecha, cod_interf, paso, mark_err, tipo_modo);
		interfaces.Add (interface_reg);
		//for debugging:
		//imprimir_lista ();
	} //cierra metodo RegistrarInterfazModoGuiado

	public void ImprimirTamañoDeLista(){
		Debug.Log ("El tamaño de la lista de interfaces registradas es: " + interfaces.Count);
	}



	// Update is called once per frame
	void Update () {
		if (interfaces.Count >= 6) {
			List<InterfaceInformation> interf_to_save = interfaces;
			Debug.Log ("El tamaño de la lista COPIA de interfaces registradas es: " + interf_to_save.Count);
			//se reinicializa el listado de interfaces para seguir almacenando las nuevas interfaces:
			ReinicializarListaInterfaces();
			//se crea la tarea asincrona para guardar los datos:
			//saveData = new ThreadedStoreDataLocalDB();
			//se asigna el array de registros:
			//saveData.InData = interf_to_save;
			//se obtiene la instancia para invocar el metodo estatico:
			var and_unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			var current_act = and_unity.GetStatic<AndroidJavaObject>("currentActivity");
			Debug.Log("Se ha obtenido current activity para GUARDAR DATOS EN BD LOCAL...");
			// Accessing the class to call a static method on it
			var saveServer = new AndroidJavaClass("edu.udg.bcds.pintura.arapp.SaveDatabaseData");
			object[] parameters;
			foreach(InterfaceInformation int_inform in interf_to_save){
				Debug.Log ("Interfaz registrada: codigo=" + int_inform.Codigo_interfaz + ",fecha=" + int_inform.Fecha);

				parameters = new object[7]; 
				parameters [0] = current_act; //pasando el argumento de la actividad actual que se debe reproducir
				parameters [1] = int_inform.Id_estudiante; 
				parameters [2] = int_inform.Fecha; 
				parameters [3] = int_inform.Codigo_interfaz; 
				parameters [4] = int_inform.Paso;
				parameters [5] = int_inform.Marcador_error;
				parameters [6] = int_inform.Tipo_modo; 
				Debug.Log ("Se va a llamar el metodo estatico SaveNavigationMonitoringToDB desde Unity");
				// Calling a Call method to which the current activity is passed
				saveServer.CallStatic("SaveNavigationMonitoringToDB", parameters);
			} //cierra for each
			//se asigna la variable a traves de la cual se llama al metodo estatico:
			//saveData.save_server = saveServer;
			//asignando la variable de la actividad actual:
			//saveData.current_activity = current_act;
			//saveData.Start();
		} //cierra if de tamaaño de la lista = 6
		/*
		if (saveData != null) {
			if (saveData.Update())
			{	
				Debug.Log ("El Update del proceso es TRUE...");
				Debug.Log ("El dato de resultado es: " + saveData.OutData);

				// Alternative to the OnFinished callback
				saveData = null;
			}
		} */
	}

	void OnApplicationQuit() {
		Application.CancelQuit ();
		GuardarDatosInmediatamente();
		Debug.Log ("SE VA A CERRAR LA APLICACION BYE!!");
		Application.Quit();
	}
	
	void OnApplicationPause(bool pauseStatus) {
		if (pauseStatus) { //si el pauseStatus es true es decir si se ha puesto en background la aplicacion entonces se finaliza el registro de eventos de navegacion:
			GuardarDatosInmediatamente();
		}
	}

	/// <summary>
	/// Este metodo se utiliza para guardar los datos de navegacion inmediatamente antes de que la aplicacion se cierre.
	/// </summary>
	public void GuardarDatosInmediatamente(){
		//se agrega el evento de finalizacion de la secuencia de navegacion de interfaces:
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		RegistrarInterfazModoEvalConsult(AppManager.manager.codigo_estudiante,fecha,"C1","0","-1","close");
		//se copia el vector para no trabajar sobre el original
		List<InterfaceInformation> interf_to_close = interfaces;
		//se reinicializa el vector original:
		ReinicializarListaInterfaces();
		//se envian los datos a Android DB:
		var and_unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		var current_act = and_unity.GetStatic<AndroidJavaObject>("currentActivity");
		Debug.Log("Se ha obtenido current activity para GUARDAR DATOS EN BD LOCAL...");
		// Accessing the class to call a static method on it
		var saveServer = new AndroidJavaClass("edu.udg.bcds.pintura.arapp.SaveDatabaseData");
		object[] parameters;
		foreach(InterfaceInformation int_inform in interf_to_close){
			Debug.Log ("Interfaz registrada: codigo=" + int_inform.Codigo_interfaz + ",fecha=" + int_inform.Fecha);
			
			parameters = new object[7]; 
			parameters [0] = current_act; //pasando el argumento de la actividad actual que se debe reproducir
			parameters [1] = int_inform.Id_estudiante; 
			parameters [2] = int_inform.Fecha; 
			parameters [3] = int_inform.Codigo_interfaz; 
			parameters [4] = int_inform.Paso;
			parameters [5] = int_inform.Marcador_error;
			parameters [6] = int_inform.Tipo_modo; 
			Debug.Log ("Se va a llamar el metodo estatico SaveNavigationMonitoringToDB desde Unity");
			// Calling a Call method to which the current activity is passed
			saveServer.CallStatic("SaveNavigationMonitoringToDB", parameters);
		} //cierra for each

	}//cierra metodo GuardarDatosInmediatamente

}
