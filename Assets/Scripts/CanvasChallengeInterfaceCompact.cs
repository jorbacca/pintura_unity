using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CanvasChallengeInterfaceCompact : MonoBehaviour {

	public string titulo;
	public string introduction_text_path;
	public string image_header_path;
	public Button continueButton;

	private TextAsset introduction_asset;
	private Sprite image_sprite;

	
	private GameObject titulo_object;
	private GameObject introduction_object;
	private GameObject image_header_object;

	public System.Action continue_button_action;

	
	// Use this for initialization
	void Start () {
		//obteniendo el componente text que se llama titulo en el prefab:
		titulo_object = GameObject.Find ("titulo_compact_interface");
		if (titulo != null) {
			//Debug.LogError("Se va a cambiar el titulo de la interfaz!!");
			titulo_object.GetComponent<Text>().text = titulo;
		}
		
		introduction_object = GameObject.Find ("introduction_compact_interface");
		if (introduction_object != null) {
			//Debug.LogError ("Se va a cambiar el texto del contenido!!");
			introduction_asset = Resources.Load (introduction_text_path) as TextAsset;
			introduction_object.GetComponent<Text> ().text = introduction_asset.text;

		} else {
			Debug.LogError ("Introduction es NULL!!!!");
		}

		image_header_object = GameObject.Find ("image_header_compact_interface");
		if (image_header_object != null) {
			//Debug.LogError ("Se va a cambiar la imagen del header!!");
			image_sprite = Resources.Load<Sprite> (image_header_path);
			//Debug.LogError ("Se va a cambiar la imagen del header!!");
			image_header_object.GetComponent<Image> ().sprite = image_sprite;
		}

		if (continueButton != null) {
			continueButton.onClick.AddListener (() => {ContinueButtonAction ();});
		}
			
		
	}

	//Action que llama al metodo del AppManager para ejecutar el metodo.
	public void ContinueButtonAction (){
		Debug.Log ("Se acciono el boton continuar en ChallengeInterface!!");
		this.continue_button_action ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
