using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CanvasActivitiesForEachStepManager : MonoBehaviour {

	//variable que indica la interfaz desde la cual se esta intentando cargar la interfaz Tools and Products
	//esta variable representa la interfaz o el paso desde el cual se esta intentando llamar alguna otra accion:
	public string interfaceCallingGoToTools;
	//variable que indica la interfaz desde la cual se esta intentando llamar el menu de pasos de la fase correspondiente:
	public string interfaceGoingBackFrom;

	//variables para configurar aspectos de la interfaz como titulo imagen de header etc
	public string image_header_step;
	public string titulo_current_step;
	public string introduction_text_path;
	public Button toolsAndProducts;
	public Button videos;
	public Button selfAssessment;
	public Button simulations;
	public Button personalNotes;
	public Button frequenltyAskedQuest;
	public Button askYourTeacher;
	public Button goBackToMenuPhases;

	//rutas a las imagenes que se deben cargar (numeradas para leerse de izquierda a derecha en la interfaz):
	public string image_uno_tools_and_products;
	public string image_dos_videos;
	public string image_tres_self_assessment;
	public string image_cuatro_simulations;
	public string image_cinco_personal_notes;
	public string image_seis_frequently_questions;
	public string image_siete_ask_your_teacher;

	//GameObjects para obtener el titulo y el objeto que muestra un texto de introduccion:
	private GameObject imagen_header;
	private GameObject titulo_object;
	private GameObject introduction_object;

	//TextAsset para cargar desde archivo el contenido de la introduccion
	private TextAsset introduction_asset;
	
	//Sprites para configurar las imagenes de los botones
	private Sprite sprite_image_header_step;
	private Sprite image_button_sprite_uno;
	private Sprite image_button_sprite_dos;
	private Sprite image_button_sprite_tres;
	private Sprite image_button_sprite_cuatro;
	private Sprite image_button_sprite_cinco;
	private Sprite image_button_sprite_seis;
	private Sprite image_button_sprite_siete;

	//variable para almacenar la accion del boton regresar:
	public System.Action goBackToMenuStepsPhase1;
	//public System.Action goToToolsAndProducts;

	//Variable para metodo delegado que permite ir a la interfaz de tools and products correspondiente
	//este metodo delegado recibe como parametro el nombre de la interfaz desde la cual se va a redireccionar
	//hacia la interfaz tools and products
	public delegate void GoToToolsAndProducts(string interface_from);
	//variable para el metodo delegado. Esta variable se asigna desde AppManager
	public GoToToolsAndProducts goToToolsAndProd;


	//Variable para metodo delegado que permite regresar desde la interfaz de actividades hacia la interfaz
	//de menu de pasos correspondiente. El metodo recibe el nombre de la interfaz desde la cual se esta intentando
	//redireccionar:
	public delegate void GoToMenuSteps(string interface_from);
	//variable que se asigna desde el AppManager para regresar hacia donde se debe ir:
	public GoToMenuSteps goToMenuSteps;

	//metodo para llamar a la interfaz para ver videos
	public delegate void GoToVideosForStep(string interface_calling);
	//variable que se asigna desde el AppManager para invocar la actividad android de los videos:
	public GoToVideosForStep goToVideosForStep;

	// Use this for initialization
	void Start () {

		//obteniendo el componente del encabezado de la imagen:
		imagen_header = GameObject.Find ("header_image_activities_foreach_step");
		if (imagen_header != null) {
			sprite_image_header_step = Resources.Load<Sprite> (image_header_step);
			imagen_header.GetComponent<Image>().sprite = sprite_image_header_step; 
		}
		
		//obteniendo el componente text que se llama titulo en el prefab:
		titulo_object = GameObject.Find ("title_activities_foreach_step_text");
		if (titulo_object != null) {
			Debug.LogError("Se va a cambiar el titulo de la interfaz!!");
			titulo_object.GetComponent<Text>().text = titulo_current_step;
		}
		
		//colocando el texto de la introduccion:
		introduction_object = GameObject.Find ("introduction_activities_step_interface");
		if (introduction_object != null) {
			Debug.LogError("Se va a cambiar el texto central!!");
			introduction_asset = Resources.Load(introduction_text_path) as TextAsset;
			introduction_object.GetComponent<Text>().text = introduction_asset.text;
			
		}

		//asignando la accion para regresar al boton regresar:
		if (goBackToMenuPhases != null) {
			goBackToMenuPhases.onClick.AddListener(()=>{ActionButton_goBackToMenuSteps();});
		}

		if (toolsAndProducts != null) {
			image_button_sprite_uno = Resources.Load<Sprite> (image_uno_tools_and_products);
			toolsAndProducts.GetComponent<Image>().sprite = image_button_sprite_uno;
			//asignando la accion:
			toolsAndProducts.onClick.AddListener(()=>{ActionButton_goToToolsAndProducts();});
		}

		if (videos != null) {
			image_button_sprite_dos = Resources.Load<Sprite> (image_dos_videos);
			videos.GetComponent<Image>().sprite = image_button_sprite_dos;
			videos.onClick.AddListener(()=>{ActionButton_goToVideos();});
		}

		if (selfAssessment != null) {
			image_button_sprite_tres = Resources.Load<Sprite> (image_tres_self_assessment);
			selfAssessment.GetComponent<Image>().sprite = image_button_sprite_tres;
		}

		if (simulations != null) {
			image_button_sprite_cuatro = Resources.Load<Sprite> (image_cuatro_simulations);
			simulations.GetComponent<Image>().sprite = image_button_sprite_cuatro;
		}

		if (personalNotes != null) {
			image_button_sprite_cinco = Resources.Load<Sprite> (image_cinco_personal_notes);
			personalNotes.GetComponent<Image>().sprite = image_button_sprite_cinco;
		}

		if (frequenltyAskedQuest != null) {
			image_button_sprite_seis = Resources.Load<Sprite> (image_seis_frequently_questions);
			frequenltyAskedQuest.GetComponent<Image>().sprite = image_button_sprite_seis;
		}

		if (askYourTeacher != null) {
			image_button_sprite_siete = Resources.Load<Sprite> (image_siete_ask_your_teacher);
			askYourTeacher.GetComponent<Image>().sprite = image_button_sprite_siete;
		}

				
	}//cierra metodo start

	/// <summary>
	/// Method for going back to the menu of steps
	/// </summary>
	public void ActionButton_goBackToMenuOfSteps(){
		this.goBackToMenuStepsPhase1();
	}

	/// <summary>
	/// Method that starts the Tools and products interface
	/// </summary>
	public void ActionButton_goToToolsAndProducts(){
		this.goToToolsAndProd(interfaceCallingGoToTools);
	}

	public void ActionButton_goBackToMenuSteps(){
		this.goToMenuSteps (interfaceGoingBackFrom);
	}

	public void ActionButton_goToVideos(){
		//en este caso el argumento que se pasa es "interfaceCallingGoToTools" porque dicha variable indica la interfaz que esta
		//haciendo el llamado al metodo
		this.goToVideosForStep (interfaceCallingGoToTools);
	}


	// Update is called once per frame
	void Update () {
	
	}
}
