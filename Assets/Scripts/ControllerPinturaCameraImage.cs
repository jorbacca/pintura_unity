using UnityEngine;
using System.Collections;
using Vuforia;

public class ControllerPinturaCameraImage : MonoBehaviour {

	private Vuforia.Image.PIXEL_FORMAT m_PixelFormat = Image.PIXEL_FORMAT.RGB888;
	private bool m_RegisteredFormat = false; 
	private bool m_LogInfo = true;
	public Vuforia.Image image;

	public string url_servidor;
	public string tipo_img;
	public string codigo_estudiante;
	public string interface_coming_from;

	public GameObject feedback_obj; //esta variable apunta al recuadro imagen que coloca el fondo del feedback
	public GameObject texto_feedback; //esta variable apunta al texto que esta dentro del recuadro del feedback

	public GameObject panel_guia_foto;

	public delegate void ResultadoTomarFoto(string paso,string tipo_ficha);

	public ResultadoTomarFoto ResultPhotoActivity;


	//public static ControllerPinturaCameraImage controlCamera;

	// Use this for initialization
	void Start () {
		/*
		if (controlCamera == null) {
			DontDestroyOnLoad (gameObject);
			controlCamera = this;
		} else if (controlCamera != this) {
			Destroy(gameObject);
		}
		*/

		//He comentareado temporalmente 22 de agosto este metodo para descartar problemas en el movil pequeño:
		/*
		QCARBehaviour qcarBehaviour = (QCARBehaviour) FindObjectOfType(typeof(QCARBehaviour));
		if (qcarBehaviour)
		{
			qcarBehaviour.RegisterTrackablesUpdatedCallback(OnTrackablesUpdated);
		}
		Debug.Log ("INICIALIZANDO LA CAMARA Y EL CONTROL QCAR!!!");
		*/

		if (feedback_obj != null && texto_feedback != null) {
		
			feedback_obj.GetComponent<UnityEngine.UI.Image> ().enabled = false;
			texto_feedback.GetComponent<UnityEngine.UI.Text> ().enabled = false;
		} else {
			Debug.LogError("ERROR: No se han definido correctamente las variables feedback_obj o texto_feedback en ControllerPinturaCAmeraImage");
		}

	}

	/*
	public void OnTrackablesUpdated()
	{	
		//Debug.Log ("Llamado al OnTrackablesUpdated");
		if (!m_RegisteredFormat)
		{
			CameraDevice.Instance.SetFrameFormat(m_PixelFormat, true);
			m_RegisteredFormat = true;
		}

		if (m_LogInfo)
		{
			CameraDevice cam = CameraDevice.Instance;
			//Debug.Log("Antes de definir la configuracion...");
			//Vuforia.QCARRenderer.VideoBGCfgData config = Vuforia.QCARRenderer.Instance.GetVideoBackgroundConfig();
			//config.reflection = Vuforia.QCARRenderer.VideoBackgroundReflection.ON;
			//Vuforia.QCARRenderer.Instance.SetVideoBackgroundConfig(config);
			//Debug.Log("Despues de definir la configuracion...");

			image = cam.GetCameraImage(m_PixelFormat);

			if (image == null)
			{
				Debug.Log(m_PixelFormat + " image is not available yet");
			}
			else
			{
				string s = m_PixelFormat + " image: \n";
				s += "  size: " + image.Width + "x" + image.Height + "\n";
				s += "  bufferSize: " + image.BufferWidth + "x" + image.BufferHeight + "\n";
				s += "  stride: " + image.Stride;
				Debug.Log(s);
				m_LogInfo = false;
			}
		}
	} //cierra OnTrackablesUpdated
	*/

	//public bool image_requested;

	public void GetImageFromCamera(){
		m_LogInfo = true;

		//invalidando la posibilidad de que el estudiante se devuelva cuando se ha iniciado este proceso:
		AppManager.manager.can_return_from_take_photo = false;

		StartCoroutine ("rutinaObtenerImagen");
		Debug.Log ("Despues de la corrutina!!!");



	}

	IEnumerator rutinaObtenerImagen(){

		//Debug.Log ("La corrutina inicia: " + image);
		Debug.Log ("La corrutina inicia... ");

		//yield return image;
		yield return new WaitForEndOfFrame ();

		//Debug.Log ("Imagen obtenida!!!!" + image);
		//Texture2D tex = new Texture2D ( image.Width, image.Height);
		//image.CopyToTexture (tex);

		//tex.Apply (); //de acuerdo con la documentacion de Unity esta puede ser una operacion de procescamiento intensiva
		//Debug.Log ("Ya se ha copiado la textura!!!");
		/*
		//iniciando pruebas:
		Sprite test = new Sprite ();
		test = Sprite.Create (tex, new Rect (0, 0, tex.width, tex.height), new Vector2 (0f, 0f));
		textura_prueba.GetComponent<UnityEngine.UI.Image> ().sprite = test;

		Debug.Log ("Antes de obtener el renderer!!!");
		Material mater = GetComponent<Renderer> ().material;
		Texture2D textur = mater.mainTexture as Texture2D;
		Debug.Log ("Despues de obtener el renderer");
		//terminando pruebas
		*/

		//Texture2D txt_girada = FlipTexture (tex);
		//Texture2D txt_horiz = FlipTextureHoriz (txt_girada);


		int	ancho = Screen.width;
		int	alto = Screen.height;


		Texture2D txt_girada = new Texture2D (ancho, alto, TextureFormat.RGB24, false);

		txt_girada.ReadPixels (new Rect (0, 0, ancho, alto), 0, 0);
		txt_girada.Apply ();

		byte[] bytes = txt_girada.EncodeToPNG();
		//byte[] bytes = tex.EncodeToPNG();
		Destroy( txt_girada );

		//Mostrando el cuadro de feedback en la interfaz para que el estudiante no regrese hacia atras
		//mientras se esta enviando la imagen:
		if (feedback_obj != null && texto_feedback != null) {
			
			feedback_obj.GetComponent<UnityEngine.UI.Image> ().enabled = true;
			texto_feedback.GetComponent<UnityEngine.UI.Text> ().enabled = true;
		}

		//definiendo el nombre de la imagen con base en el codigo de estudiante y en el tipo de img:
		string nombre_imagen = this.codigo_estudiante + "_" + tipo_img + ".png";
			
		// Create a Web Form
		WWWForm form = new WWWForm();
		form.AddField("frameCount", Time.frameCount.ToString());
		form.AddField("cod_estud", this.codigo_estudiante);
		form.AddField("tipo_img", this.tipo_img);
		//adjuntando los datos binarios para enviar la imagen (el nombre se ha definido anteriormente)
		form.AddBinaryData("fileimage", bytes, nombre_imagen, "image/png");
		Debug.Log ("Se va a enviar el formulario !!");
		// Upload to a cgi script
		WWW w = new WWW(this.url_servidor + "uploadimage.php", form);
		yield return w;
		if (!string.IsNullOrEmpty(w.error)) {
			//ocultando el cuadro del feedback porque ya se ha terminado la transferencia de la imagen:
			if (feedback_obj != null && texto_feedback != null) {
				
				feedback_obj.GetComponent<UnityEngine.UI.Image> ().enabled = false;
				texto_feedback.GetComponent<UnityEngine.UI.Text> ().enabled = false;
			}
			AppManager.manager.can_return_from_take_photo = true;
			//mostrando el feedback de ERROR en el envio del archivo:
			MobileNativeMessage mensaje_confirm = new MobileNativeMessage("Error","No se ha podido enviar la foto. Porfavor verifica que estas conectado a internet y que la conexion es buena. Informa al profesor de este problema.","Aceptar");
			Debug.Log ("ERROR EN LA TRANSFERENCIA AL SERVIDOR" + w.error);
		}
		else {
			//ocultando el cuadro del feedback:
			if (feedback_obj != null && texto_feedback != null) {
				
				feedback_obj.GetComponent<UnityEngine.UI.Image> ().enabled = false;
				texto_feedback.GetComponent<UnityEngine.UI.Text> ().enabled = false;
			}
			AppManager.manager.can_return_from_take_photo = true;
			//mostrando el feedback de EXITO en el envio del archivo:
			MobileNativeMessage mensaje_confirm = new MobileNativeMessage("Datos enviados","La foto se ha enviado correctamente.","Aceptar");
			Debug.Log ("Transferencia satisfactoria!!! --> " + w.text);
			
		} //cierra else

		//aqui se notifica al AppManager que ya se ha completado la actividad de tomar la foto
		//esto se hace invocando al metodo delegado ResultPhotoActivity enviando como parametros
		//la interfaz que solicito la accion y el tipo de imagen que se estaba tomando:
		this.ResultPhotoActivity (interface_coming_from, tipo_img);

	} //cierra rutina Obtener imagen


	Texture2D FlipTexture(Texture2D original){
		Texture2D flipped = new Texture2D(original.width,original.height);
		
		int xN = original.width;
		int yN = original.height;
		
		
		for(int i=0;i<xN;i++){
			for(int j=0;j<yN;j++){
				flipped.SetPixel(xN-i-1, j, original.GetPixel(i,j));
			}
		}
		flipped.Apply();
		
		return flipped;
	}

	Texture2D FlipTextureHoriz(Texture2D original){
		Texture2D flipped = new Texture2D(original.width,original.height);
		
		int xN = original.width;
		int yN = original.height;
		
		
		for(int i=0;i<xN;i++){
			for(int j=0;j<yN;j++){
				flipped.SetPixel(i,yN-j-1, original.GetPixel(i,j));
			}
		}
		flipped.Apply();
		
		return flipped;
	}



}
