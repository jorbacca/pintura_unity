using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CanvasToolsAndProductsManager : MonoBehaviour {

	public string image_header_path;
	public string title_header_text_path;
	public string title_intro_content_text_path;
	public string tool_one_text_path;
	public string tool_two_text_path;
	public string footer_search_text_path;

	public string ruta_img_one_tool_path;
	public string ruta_img_two_tool_path;
	public string ruta_img_three_tool_path;
	public string ruta_img_four_tool_path;
	public string ruta_img_five_tool_path;
	public string ruta_img_six_tool_path;

	 
	public Image image_header_object;
	public Text title_header_object;
	public Text title_content_intro_object;
	public Text tool_one_object;
	public Text tool_two_object;
	public Text footer_search_text_object;

	public Image tool_one_image_obj;
	public Image tool_two_image_obj;
	public Image tool_three_image_obj;
	public Image tool_four_image_obj;
	public Image tool_five_image_obj;
	public Image tool_six_image_obj;


	public Button btn_search_tools_products;
	public Button btn_goBackToMenu;

	private Sprite img_header_sprite;
	private Sprite img_tool_one_sprite;
	private Sprite img_tool_two_sprite;
	private Sprite img_tool_three_sprite;
	private Sprite img_tool_four_sprite;
	private Sprite img_tool_five_sprite;
	private Sprite img_tool_six_sprite;

	//public System.Action accion;
	public delegate void accionDos(string value);

	public accionDos accdos;

	public System.Action goBackButtonAction;

	public System.Action goToSearchProductsTools;

	//variable que indica la interfaz desde la cual se esta intentando regresar:
	public string interfaceGoingBackFrom;

	//variable temporal para cargar los textos:
	private TextAsset text_asset_load;


	// Use this for initialization
	void Start () {

		if (image_header_object != null) {
			Debug.Log ("CanvasTooslAndProducts: Se va a cargar el header...");
			img_header_sprite = Resources.Load<Sprite> (image_header_path);
			image_header_object.sprite = img_header_sprite;

		}

		if (btn_search_tools_products != null)
			btn_search_tools_products.onClick.AddListener (()=>{ActionButton_goToSearch();});

		if (btn_goBackToMenu != null) {
			btn_goBackToMenu.onClick.AddListener (()=>{ActionButton_goBack();});
		}

		if (title_header_object != null) {
			Debug.Log ("CanvasTooslAndProducts: Se va a cargar el titulo...");
			text_asset_load = Resources.Load<TextAsset>(title_header_text_path);
			if(text_asset_load != null)
				title_header_object.text = text_asset_load.text;
			else title_header_object.text = "";
		}

		if (title_content_intro_object != null) {
			Debug.Log ("CanvasTooslAndProducts: Se va a cargar el contenido de la intro");
			text_asset_load = Resources.Load<TextAsset>(title_intro_content_text_path);
			if(text_asset_load != null)
				title_content_intro_object.text = text_asset_load.text;
			else title_content_intro_object.text = "";
		}

		if (tool_one_object != null) {
			Debug.Log ("CanvasTooslAndProducts: Se va a cargar el texto de herramienta 1.");
			if(tool_one_text_path != ""){
				text_asset_load = Resources.Load<TextAsset>(tool_one_text_path);
				if(text_asset_load != null)
					tool_one_object.text = text_asset_load.text;
				else tool_one_object.text = "";
			} else tool_one_object.text = "";
				
		}

		if (tool_two_object != null) {
			Debug.Log ("CanvasTooslAndProducts: Se va a cargar el texto de herramienta 2.");
			if(tool_two_text_path != ""){
				text_asset_load = Resources.Load<TextAsset>(tool_two_text_path);
				if(text_asset_load != null)
					tool_two_object.text = text_asset_load.text;
				else tool_two_object.text = "";
			} else tool_two_object.text = "";
		}


		if (tool_one_image_obj != null) {
			Debug.Log ("CanvasTooslAndProducts: Se va a cargar IMAGEN de herramienta 1: " + ruta_img_one_tool_path);
			img_tool_one_sprite = Resources.Load<Sprite>(ruta_img_one_tool_path);
			tool_one_image_obj.sprite = img_tool_one_sprite;
		}

		if (tool_two_image_obj != null) {
			Debug.Log ("CanvasTooslAndProducts: Se va a cargar IMAGEN de herramienta 2.");
			img_tool_two_sprite = Resources.Load<Sprite>(ruta_img_two_tool_path);
			tool_two_image_obj.sprite = img_tool_two_sprite;
		}

		if (tool_three_image_obj != null) {
			Debug.Log ("CanvasTooslAndProducts: Se va a cargar IMAGEN de herramienta 3.");
			img_tool_three_sprite = Resources.Load<Sprite>(ruta_img_three_tool_path);
			tool_three_image_obj.sprite = img_tool_three_sprite;
		}

		if (tool_four_image_obj != null) {
			Debug.Log ("CanvasTooslAndProducts: Se va a cargar IMAGEN de herramienta 4.");
			img_tool_four_sprite = Resources.Load<Sprite>(ruta_img_four_tool_path);
			tool_four_image_obj.sprite = img_tool_four_sprite;
		}

		if (tool_five_image_obj != null) {
			Debug.Log ("CanvasTooslAndProducts: Se va a cargar IMAGEN de herramienta 5.");
			img_tool_five_sprite = Resources.Load<Sprite>(ruta_img_five_tool_path);
			tool_five_image_obj.sprite = img_tool_five_sprite;
		}

		if (tool_six_image_obj != null) {
			Debug.Log ("CanvasTooslAndProducts: Se va a cargar IMAGEN de herramienta 6.");
			img_tool_six_sprite = Resources.Load<Sprite>(ruta_img_six_tool_path);
			tool_six_image_obj.sprite = img_tool_six_sprite;
		}

		if (footer_search_text_object != null) {
			Debug.Log ("CanvasTooslAndProducts: Se va a cargar pie de pagina.");
			footer_search_text_object.text = Resources.Load<TextAsset>(footer_search_text_path).text;
		}

	


	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ActionButton_accion(){
		//this.accion("");
		this.accdos ("resultado");
	}

	public void ActionButton_goToSearch(){
		this.goToSearchProductsTools ();
	}

	public void ActionButton_goBack(){
		this.goBackButtonAction ();
	}
}
