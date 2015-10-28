using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CanvasTakePictureInterfaceManager : MonoBehaviour {

	public string image_header_path;
	public string title_header_text_path;
	public string title_intro_content_text_path;
	public string tool_one_text_path;
	//public string footer_search_text_path;
	
	public string ruta_img_product_one_seguridad;
	public string ruta_img_product_one_tecnica;
	public string ruta_img_product_two_seguridad;
	public string ruta_img_product_two_tecnica;
	public string ruta_img_product_three_seguridad;
	public string ruta_img_product_three_tecnica;
	
	
	public Image image_header_object;
	public Text title_header_object;
	public Text title_content_intro_object;
	public Text tool_one_object;
	//public Text tool_two_object;
	//public Text footer_search_text_object;
	
	public Button product_one_tecnica; 
	public Button product_one_seguridad;
	public Button product_two_tecnica;
	public Button product_two_seguridad;
	public Button product_three_tecnica;
	public Button product_three_seguridad;
	
	
	//public Button btn_search_tools_products;
	public Button btn_goBackToMenu;
	
	private Sprite img_header_sprite;
	private Sprite img_product_one_sprite;
	private Sprite img_product_two_sprite;
	private Sprite img_product_three_sprite;
	private Sprite img_product_four_sprite;
	private Sprite img_product_five_sprite;
	private Sprite img_product_six_sprite;

	public bool btn_product_one_tecnica_enable;
	public bool btn_product_one_seguridad_enable;
	public bool btn_product_two_tecnica_enable;
	public bool btn_product_two_seguridad_enable;
	public bool btn_product_three_tecnica_enable;
	public bool btn_product_three_seguridad_enable;
	
	//public System.Action accion;
	public delegate void accionDos(string value);
	public accionDos accdos;

	public delegate void TomarFotoProductOne(string type);
	public TomarFotoProductOne goToPictureProdOne;

	public delegate void TomarFotoProductTwo(string type);
	public TomarFotoProductTwo goToPictureProdTwo;

	public delegate void TomarFotoProductThree(string type);
	public TomarFotoProductThree goToPictureProdThree;


	public System.Action goBackButtonAction;
	

	
	//variable que indica la interfaz desde la cual se esta intentando regresar:
	public string interfaceGoingBackFrom;
	
	
	// Use this for initialization
	void Start () {
		
		if (image_header_object != null) {
			Debug.Log ("CanvasTooslAndProducts: Se va a cargar el header...");
			img_header_sprite = Resources.Load<Sprite> (image_header_path);
			image_header_object.sprite = img_header_sprite;
			
		}
		
		if (btn_goBackToMenu != null) {
			btn_goBackToMenu.onClick.AddListener (()=>{ActionButton_goBack();});
		}
		
		if (title_header_object != null) {
			Debug.Log ("CanvasTooslAndProducts: Se va a cargar el titulo...");
			title_header_object.text = Resources.Load<TextAsset>(title_header_text_path).text;
		}
		
		if (title_content_intro_object != null) {
			Debug.Log ("CanvasTooslAndProducts: Se va a cargar el contenido de la intro");
			title_content_intro_object.text = Resources.Load<TextAsset>(title_intro_content_text_path).text;
		}
		
		if (tool_one_object != null) {
			Debug.Log ("CanvasTooslAndProducts: Se va a cargar el texto de herramienta 1.");
			if(tool_one_text_path != "")
				tool_one_object.text = Resources.Load<TextAsset>(tool_one_text_path).text;
			else tool_one_object.text = "";
			
		}

		if (btn_product_one_tecnica_enable) {
			if (product_one_tecnica != null) {
				//habilitando el componente imagen:
				product_one_tecnica.GetComponent<Image>().enabled = true;

				Debug.Log ("CanvasTooslAndProducts: Se va a cargar IMAGEN de herramienta 1: " + ruta_img_product_one_tecnica);
				img_product_one_sprite = Resources.Load<Sprite> (ruta_img_product_one_tecnica);
				product_one_tecnica.GetComponent<Image> ().sprite = img_product_one_sprite;
				product_one_tecnica.onClick.AddListener (() => {
					ActionButton_productOne ("tecnica");});
			}
		} else {
			product_one_tecnica.enabled = false;
		}

		if (btn_product_one_seguridad_enable) {
			if (product_one_seguridad != null) {
				//habilitando el componente imagen:
				product_one_seguridad.GetComponent<Image>().enabled = true;

				Debug.Log ("CanvasTooslAndProducts: Se va a cargar IMAGEN de herramienta 2.");
				img_product_two_sprite = Resources.Load<Sprite> (ruta_img_product_one_seguridad);
				product_one_seguridad.GetComponent<Image> ().sprite = img_product_two_sprite;
				product_one_seguridad.onClick.AddListener (() => {
					ActionButton_productOne ("seguridad");});
			}
		} else {
			product_one_seguridad.enabled = false;
		}

		if (btn_product_two_tecnica_enable) {
			if (product_two_tecnica != null) {
				//habilitando el componente imagen:
				product_two_tecnica.GetComponent<Image>().enabled = true;
				Debug.Log ("CanvasTooslAndProducts: Se va a cargar IMAGEN de herramienta 3.");
				img_product_three_sprite = Resources.Load<Sprite> (ruta_img_product_two_tecnica);
				product_two_tecnica.GetComponent<Image> ().sprite = img_product_three_sprite;
				product_two_tecnica.onClick.AddListener (() => {
					ActionButton_productTwo ("tecnica");});
			}
		} else {
			product_two_tecnica.enabled = false;
		}

		if (btn_product_two_seguridad_enable) {
			if (product_two_seguridad != null) {
				//habilitando el componente imagen:
				product_two_seguridad.GetComponent<Image>().enabled = true;
				Debug.Log ("CanvasTooslAndProducts: Se va a cargar IMAGEN de herramienta 4.");
				img_product_four_sprite = Resources.Load<Sprite> (ruta_img_product_two_seguridad);
				product_two_seguridad.GetComponent<Image> ().sprite = img_product_four_sprite;
				product_two_seguridad.onClick.AddListener (() => {
					ActionButton_productTwo ("seguridad");});
			}
		} else {
			product_two_seguridad.enabled = false;
		}

		if (btn_product_three_tecnica_enable) {
			if (product_three_tecnica != null) {
				//habilitando el componente imagen:
				product_three_tecnica.GetComponent<Image>().enabled = true;
				Debug.Log ("CanvasTooslAndProducts: Se va a cargar IMAGEN de herramienta 5.");
				img_product_five_sprite = Resources.Load<Sprite> (ruta_img_product_three_tecnica);
				product_three_tecnica.GetComponent<Image> ().sprite = img_product_five_sprite;
				product_three_tecnica.onClick.AddListener (() => {
					ActionButton_productThree ("tecnica");});
			}
		} else {
			product_three_tecnica.enabled = false;
		}

		if (btn_product_three_seguridad_enable) {
			if (product_three_seguridad != null) {
				//habilitando el componente imagen:
				product_three_seguridad.GetComponent<Image>().enabled = true;
				Debug.Log ("CanvasTooslAndProducts: Se va a cargar IMAGEN de herramienta 6.");
				img_product_six_sprite = Resources.Load<Sprite> (ruta_img_product_three_seguridad);
				product_three_seguridad.GetComponent<Image> ().sprite = img_product_six_sprite;
				product_three_seguridad.onClick.AddListener (() => {
					ActionButton_productThree ("seguridad");});
			}
		} else {
			product_three_seguridad.enabled = false;
		}
				
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ActionButton_productOne(string tipo){
		this.goToPictureProdOne (tipo);
	}

	public void ActionButton_productTwo(string tipo){
		this.goToPictureProdTwo (tipo);
	}

	public void ActionButton_productThree(string tipo){
		this.goToPictureProdThree (tipo);
	}





	public void ActionButton_accion(){
		//this.accion("");
		this.accdos ("resultado");
	}
	

	
	public void ActionButton_goBack(){
		this.goBackButtonAction ();
	}
}
