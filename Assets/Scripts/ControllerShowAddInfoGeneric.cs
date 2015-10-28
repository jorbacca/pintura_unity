using UnityEngine;
using System.Collections;
using UnityEngine.UI;


/// <summary>
/// IMPORTANT: This script is only for the generic interfaces for the AR mode. Tutorial interfaces have their own ControllerShowAddInfo
/// Controller show add info.
/// This script should be associated to any button in the interface that should show the icons with add info.
/// Note that a reference to the AddInfoDescriptionObject is needed and this is the object in the interface (rectangle) that shows the add info.
/// </summary>
public class ControllerShowAddInfoGeneric : MonoBehaviour {

	//referencia al objeto de la interfaz que muestra la informacion adicional.
	public GameObject AddInfoDescriptionObject;
	
	//referencia al controller que gestiona la interfaz global para determinar cuando se debe ocultar 
	//la informacion adicional:
	public GameObject ControllerInterfaceGeneral;
	
	public string TextToShow;
	
	private AdditionalDescriptionController controller_description;
	//variable para almacenar el componente Script de tipo ControllerBlinkingAddInfo del objeto interfaz general:
	private ControllerBlinkingARGeneric controller_addInfo_general;

	//variable que almacena un id del boton al cual esta asociado este script para poder saber mediante el monitoreo
	//sobre cual boton se ha hecho click:
	public string id_btn_info_adicional;
	
	// Use this for initialization
	void Start () {
		
		TextToShow = "";
		controller_description = AddInfoDescriptionObject.GetComponent<AdditionalDescriptionController> ();
		controller_addInfo_general = ControllerInterfaceGeneral.GetComponent<ControllerBlinkingARGeneric> ();
	}
	
	// Update is called once per frame
	void Update () {
		//preguntando si la informacion adicional esta deplegada y si la informacion del tutorial tambien este desplegada:
		//if (controller_addInfo_general.is_add_info_displayed && controller_description.add_description_info_displayed)
			// entonces se oculta la informacion del tutorial
		//	controller_addInfo_general.HideTutorialInformation ();
		if(!controller_addInfo_general.markerFound && controller_description.add_description_info_displayed)
			controller_description.HideAdditionalInformation ();
		if(controller_addInfo_general.hide_Add_Info_To_ShowTick && controller_description.add_description_info_displayed) //estoy probando este if para ver si todo funciona bien
			controller_description.HideAdditionalInformation ();
	}
	
	public void OnClick_ShowAddInfo(){
		Debug.Log ("Llamado al OnClick en clase ControllerShowAddInfoGeneric para componente: " + this.name);
		controller_description.TextToShowInDescription = TextToShow;
		//si la info addicional esta NO esta desplegada entonces se muestra
		if (!controller_description.add_description_info_displayed)
			controller_description.ShowAdditionalInformation ();
		else
			controller_description.HideAdditionalInformation ();

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		//validando si estamos en el modo informativo o no para registrar los eventos de click en la informacion adicional:
		if(AppManager.manager.in_informative_mode)
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "IA" + this.id_btn_info_adicional, "0", "-1", "consulta");
		else 
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "IA" + this.id_btn_info_adicional, "0", "-1", "guiado");
	}//cierra metodo OnClick_ShowAddInfo
}
