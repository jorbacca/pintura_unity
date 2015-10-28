using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ThreadedStoreDataLocalDB : ThreadedJob {

	public List<InterfaceInformation> InData;  // arbitary job data
	public string OutData = "Vacio"; // arbitary job data

	public AndroidJavaClass save_server;

	public AndroidJavaObject current_activity;
	
	protected override void ThreadFunction()
	{
		// Do your threaded task. DON'T use the Unity API here
		Debug.Log ("Se llama al metodo ThreadFunction Iniciando...");


		Debug.Log ("Se ha obtenido StartActivity...");
		object[] parameters;

		foreach(InterfaceInformation int_inform in InData){
			Debug.Log ("Interfaz registrada: codigo=" + int_inform.Codigo_interfaz + ",fecha=" + int_inform.Fecha);
			
			parameters = new object[6]; 
			parameters [0] = current_activity; //pasando el argumento de la actividad actual que se debe reproducir
			parameters [1] = int_inform.Id_estudiante; //definiendo el tipo de secuencia que se va a guardar
			parameters [2] = int_inform.Fecha; //definiendo ela URL del servidor:
			parameters [3] = int_inform.Codigo_interfaz; //enviando el codigo de estudiante
			parameters [4] = int_inform.Paso;
			parameters [5] = int_inform.Marcador_error;
			Debug.Log ("Se va a llamar el metodo estatico SaveNavigationMonitoringToDB desde Unity");
			// Calling a Call method to which the current activity is passed
			save_server.CallStatic("SaveNavigationMonitoringToDB", parameters);
		} //cierra for each

	}

	protected override void OnFinished()
	{
		Debug.Log ("Se ha finalizado el trabajo de carga de datos hacia la base de datos");
		OutData = "Finalizado";
	}
}
