using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CanvasBuscarCapoCocheManager : MonoBehaviour {

	public string image_header_buscar_capo;
	public string image_content_capo_carro_marker;
	public string titulo_buscar_capo_carro;
	public string introduction_text_path_1;
	public string introduction_text_path_2;
	public Button regresarMenuActivities;
	public Button buscar;

	//GameObjects para obtener el titulo y el objeto que muestra un texto de introduccion:
	private GameObject imagen_header;
	private GameObject imagen_content_marker;
	private GameObject titulo_object;
	private GameObject introduction_object_one;
	private GameObject introduction_object_two;

	//textos que se deben mostrar:
	private TextAsset introduction_asset_one;
	private TextAsset introduction_asset_two;

	//sprites de las imagenes que se deben cargar:
	private Sprite sprite_image_header_buscar_capo;
	private Sprite sprite_image_content_marker;

	//variable para almacenar la accion del boton regresar:
	public System.Action goBackToMenuActivities;
	public System.Action goToSearchCapoCarro;

	//variable que referencia el texto que va en el boton "Buscar" o "Siguiente" dependiendo de la interfaz que sea creada:
	public Text texto_btn_continuar_obj;

	//variable que contiene el texto que se debe mostrar sobre el boton Continuar:
	public string text_btn_continuar;


	// Use this for initialization
	void Start () {

		Debug.Log ("Llamado al metodo START de CanvasBuscarCapoCocheManager...");

		imagen_header = GameObject.Find ("header_image_buscar_capo");
		if (imagen_header != null) {
			sprite_image_header_buscar_capo = Resources.Load<Sprite> (image_header_buscar_capo);
			imagen_header.GetComponent<Image>().sprite = sprite_image_header_buscar_capo; 
		}
		
		//obteniendo el componente text que se llama titulo en el prefab:
		titulo_object = GameObject.Find ("title_buscar_capo_carro");
		if (titulo_object != null) {
			Debug.LogError("Se va a cambiar el titulo de la interfaz!!");
			titulo_object.GetComponent<Text>().text = titulo_buscar_capo_carro;
		}
		
		//colocando el texto de la introduccion:
		introduction_object_one = GameObject.Find ("introduction_buscar_capo_carro");
		if (introduction_object_one != null) {
			Debug.LogError("Se va a cambiar el texto central!!");
			introduction_asset_one = Resources.Load(introduction_text_path_1) as TextAsset;
			introduction_object_one.GetComponent<Text>().text = introduction_asset_one.text;
		}

		//colocando el texto de la introduccion:
		introduction_object_two = GameObject.Find ("introduction_part_two");
		if (introduction_object_two != null) {
			Debug.LogError("Se va a cambiar el texto central!!");
			introduction_asset_two = Resources.Load(introduction_text_path_2) as TextAsset;
			introduction_object_two.GetComponent<Text>().text = introduction_asset_two.text;
		}

		//cargando imagen del contenido:
		imagen_content_marker = GameObject.Find ("image_marker_capo_carro");
		if (imagen_content_marker != null) {
			sprite_image_content_marker = Resources.Load<Sprite> (image_content_capo_carro_marker);
			imagen_content_marker.GetComponent<Image>().sprite = sprite_image_content_marker;
		}

		if (regresarMenuActivities != null) {
			regresarMenuActivities.onClick.AddListener(()=>{ActionButton_goBackToMenuActivities();});
		}

		if (buscar != null) {
			buscar.onClick.AddListener(()=>{ActionButton_goToBuscarCapoCarro();});
			if(texto_btn_continuar_obj != null){
				texto_btn_continuar_obj.text = text_btn_continuar;
			}
		}


	} //cierra metodo start

	public void ActionButton_goBackToMenuActivities(){
		this.goBackToMenuActivities ();
	}

	public void ActionButton_goToBuscarCapoCarro(){
		this.goToSearchCapoCarro ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
