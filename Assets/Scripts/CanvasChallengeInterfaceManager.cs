using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CanvasChallengeInterfaceManager : MonoBehaviour {


	public string titulo;
	public string introduction_text_path;
	public string continue_text_path;
	public string image_header_path;
	public Button continueButton;

	private TextAsset introduction_asset;
	private TextAsset continue_asset;
	private Sprite image_sprite;

	private GameObject titulo_object;
	private GameObject introduction_object;
	private GameObject continue_text_object;
	private GameObject image_header_object;

	public System.Action continue_button_action;

	// Use this for initialization
	void Start () {

		//obteniendo el componente text que se llama titulo en el prefab:
		titulo_object = GameObject.Find ("title");
		if (titulo_object != null) {
			//Debug.Log("Se va a cambiar el titulo de la interfaz en ChallengeInterface!!");
			titulo_object.GetComponent<Text>().text = titulo;
		}

		introduction_object = GameObject.Find ("introduction");
		if (introduction_object != null) {
			//Debug.Log("Se va a cambiar el texto central en ChallengeInterface!!");
			introduction_asset = Resources.Load(introduction_text_path) as TextAsset;
			if(introduction_asset != null)
				introduction_object.GetComponent<Text>().text = introduction_asset.text;
		}

		continue_text_object = GameObject.Find ("continue_text");
		if (continue_text_object != null) {
			//Debug.Log("Se va a cambiar el texto de continuar");
			continue_asset = Resources.Load(continue_text_path) as TextAsset;
			if(continue_asset != null)
				continue_text_object.GetComponent<Text>().text = continue_asset.text;
		}

		image_header_object = GameObject.Find ("image_content_introduction");
		if (image_header_object != null) {

			image_sprite = Resources.Load<Sprite> (image_header_path);
			//Debug.Log ("El Sprite que se cargo es: " + image_sprite.name + " en ChallengeInterface");
			image_header_object.GetComponent<Image> ().sprite = image_sprite;
		}

		if (continueButton != null) {
			//Debug.Log("Se va a cambiar la accion del btn en ChallengeInterface");
			continueButton.onClick.AddListener (() => {ContinueButtonAction ();});
		}

	}
	//Action que llama al metodo del AppManager para ejecutar el metodo.
	public void ContinueButtonAction(){
		Debug.Log ("ACCION del BTN Continuar en ChallengeInterface");
		this.continue_button_action ();
	}
	
	// Update is called once per frame
	void Update () {

	}
	
}
