using UnityEngine;
using System.Collections;

public class SynchronizeInformation : MonoBehaviour {

	public bool sincronizando;
	//variable para obtener el asset que tiene la direccion del servidor:
	TextAsset url_server;
	//variable con la url_real:
	string url_conexion; 

	// Use this for initialization
	void Start () {
		url_server = Resources.Load<TextAsset> ("Texts/00_server_base_path");
		if (url_server != null)
			url_conexion = url_server.text;
		Debug.Log ("La url cargada en SynchronizeInformation es: " + url_conexion);
		//inicializando la variable sincronizacion para iniciar la corrutina
		sincronizando = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!sincronizando)
			StartCoroutine (LoadInfoToServer(url_conexion));

	}


	private IEnumerator LoadInfoToServer(string url_serv){
		sincronizando = true;
		Debug.Log ("Se inicia SINCRONIZACION con el servidor... con url=" + url_serv);

		var and_unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		var current_act = and_unity.GetStatic<AndroidJavaObject>("currentActivity");
		Debug.Log("Se ha obtenido current activity para SINCRONIZAR...");
		// Accessing the class to call a static method on it
		var save_server = new AndroidJavaClass("edu.udg.bcds.pintura.arapp.SaveDatabaseData");
		//var jc = new AndroidJavaClass("edu.udg.bcds.pintura.tools.SelfAssessment");
		//var video_activity = new AndroidJavaClass("edu.udg.bcds.pintura.arapp.VideoActivity");
		Debug.Log ("Se ha obtenido StartActivity...");
		
		object[] parameters = new object[4]; 
		parameters [0] = current_act; //pasando el argumento de la actividad actual que se debe reproducir
		parameters [1] = "phases"; //definiendo el tipo de secuencia que se va a guardar
		parameters [2] = url_serv; //definiendo ela URL del servidor:
		parameters [3] = AppManager.manager.codigo_estudiante; //enviando el codigo de estudiante
		Debug.Log ("Se va a llamar el metodo estatico SendSequencesToServer desde Unity");
		// Calling a Call method to which the current activity is passed
		save_server.CallStatic("SendSequencesToServer", parameters);

		//esperando un numero x de segundos antes de hacer la siguiente sincronizacion:
		yield return new WaitForSeconds (60f);

		Debug.Log ("Iniciando la sincronizacion de pasos de la FASE 1 hacia el servidor...");
		parameters = new object[4]; 
		parameters [0] = current_act; //pasando el argumento de la actividad actual que se debe reproducir
		parameters [1] = "phase1"; //definiendo el tipo de secuencia que se va a guardar
		parameters [2] = url_serv; //definiendo ela URL del servidor:
		parameters [3] = AppManager.manager.codigo_estudiante; //enviando el codigo de estudiante
		Debug.Log ("Se va a llamar el metodo estatico SendSequencesToServer desde Unity");
		// Calling a Call method to which the current activity is passed
		save_server.CallStatic("SendSequencesToServer", parameters);

		yield return new WaitForSeconds (60f);

		Debug.Log ("Iniciando la sincronizacion de pasos de la FASE 2 hacia el servidor...");
		parameters = new object[4]; 
		parameters [0] = current_act; //pasando el argumento de la actividad actual que se debe reproducir
		parameters [1] = "phase2"; //definiendo el tipo de secuencia que se va a guardar
		parameters [2] = url_serv; //definiendo ela URL del servidor:
		parameters [3] = AppManager.manager.codigo_estudiante; //enviando el codigo de estudiante
		Debug.Log ("Se va a llamar el metodo estatico SendSequencesToServer desde Unity");
		// Calling a Call method to which the current activity is passed
		save_server.CallStatic("SendSequencesToServer", parameters);

		yield return new WaitForSeconds (60f);

		Debug.Log ("Iniciando la sincronizacion de REGISTROS DE NAVEGACION hacia el servidor...");

		parameters = new object[2]; 
		parameters [0] = current_act; //pasando el argumento de la actividad actual que se debe reproducir
		parameters [1] = url_serv; //definiendo el tipo de secuencia que se va a guardar
		Debug.Log ("Se va a llamar el metodo estatico SendNavigationDataToServer desde Unity");
		// Calling a Call method to which the current activity is passed
		save_server.CallStatic("SendNavigationDataToServer", parameters);

		yield return new WaitForSeconds (30f);

		Debug.Log ("Finaliza la sincronizacion con el servidor...");
		sincronizando = false;

	} //cierra corrutina LoadInfoToServer

	void OnApplicationQuit() {
		Application.CancelQuit ();
			Debug.Log ("SE VA A CERRAR LA APLICACION BYE!!");
		Application.Quit();
	}

	void OnApplicationPause(bool pauseStatus) {
		Debug.Log ("Llamado al metodo OnApplicationPause" + pauseStatus);
	}
}
