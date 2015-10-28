/*============================================================================== 
 * Copyright (c) 2012-2014 Qualcomm Connected Experiences, Inc. All Rights Reserved. 
 * ==============================================================================*/
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Vuforia;
using UnityEngine.UI;

/// <summary>
/// This class manages different views in the scene like AboutPage, SplashPage and ARCameraView.
/// All of its Init, Update and Draw calls take place via SceneManager's Monobehaviour calls to ensure proper sync across all updates
/// </summary>
public class AppManager : MonoBehaviour
{

    #region PUBLIC_MEMBER_VARIABLES

	//Referencia static a la variable global para configurar Singleton para acceder desde otros scripts:
	public static AppManager manager;

    public string TitleForAboutPage = "About";
    public ISampleAppUIEventHandler m_UIEventHandler;

	//List of interfaces available to be instanced (this interfaces are prefabs):
	public GameObject LoginUserInterface;
	public GameObject challenge;
	public GameObject challenge_compact;
	public GameObject selectionOfMode;
	public GameObject ConfigurationOptions;
	public GameObject interfaz;
	public GameObject ActivitiesForEachStep;
	public GameObject ActivitiesForEachStepEvalMode;
	public GameObject menuProcessPhases;
	public GameObject menuStepsPhase1;
	public GameObject menuStepsPhase2;
	public GameObject menuSubStepsPhase2;
	public GameObject BuscarCapoCarro;
	public GameObject TutorialSearchCapoCarroInterface;
	public GameObject TutorialSearchProductsPhase2;
	public GameObject AR_Mode_interface;
	public GameObject ToolsAndProductsInterface;
	public GameObject TestingInterfacePictures;
	public GameObject InterfaceTakePicturesInfo;
	//Objetos de interfaz para el modo de evaluacion:
	public GameObject menuProcessPhasesEval;
	public GameObject menuStepsPhase1Eval;
	public GameObject menuStepsPhase2Eval;
	//Objetos de interfaz para el modo RA en evaluacion:
	public GameObject BuscarCapoCarroEvaluationMode;
	public GameObject ARModeEvaluation;

	//Objeto para controlar el modo informativo:
	public InformativeMode informative_mode;

	//Objecto GameObject para controlar las conexiones hacia el servidor:
	public GameObject ConectionController;


	public bool inTutorialPhase1;
	//variable para controlar si la informacion adicional por objeto esta desplegada:
	public bool inTutorialPhase2;
	//variable para controlar si estamos en modo paso a paso  y si estamos en modo RA:
	public bool in_RA_mode;

	//variable para controlar si estamos en el modo evaluacion:
	public bool in_Evaluation_mode;

	//variable que controla si estamos en el modo informativo:
	public bool in_informative_mode;

	public bool info_additional_displayed;

	//variables que controlan el proceso en el modo guiado incluyendo si se habilita el modo evaluacion:
	public bool evaluation_mode_enabled;

	//variables que controlan la activacion de las fases:
	public bool phase_one_enable;
	public bool phase_two_enable;
	public bool phase_three_enable;
	public bool phase_four_enable;
	public bool phase_five_enable;
	public bool phase_six_enable;

	//variable que controla si el usuario ya esta logueado o si despues del proceso de login ya quedo logueado:
	public bool user_logged;
	//variable que almacena el codigo del estudiante que esta usando la aplicacion una vez se ha logueado:
	public string codigo_estudiante;

	//variable para almacenar el ID del ultimo marcador escaneado. Este dato se fija desde el ControllerBlinkingARGeneric:
	public int last_markerid_scanned;

	//variaable que almacena el ID del ultimo marcador escaneado para el modo evaluacion. Este dato se fija desde el ControllerBlinkARModeEvaluation
	public int last_marker_id_evalmode;

	//-------------------------------------------------------------------------------------------------
	//-------------------------------------------------------------------------------------------------
	//------------------- Modo evaluacion: Variables que se almacenan en el dispositivo ---------------

	//variable para controlar si ya se han organizado las fases correctamente y se deben mostrar ya organizados para evitar que el estudiante
	//los vuelva a organizar en la fase de evaluacion (Esta variable se almacena en el dispositivo):
	public bool eval_mode_phases_organized;
	
	//variable para controlar si ya se han organizado los pasos de la fase 1 correctamente y se deben mostrar ya organizados para evitar que el estudiante
	//los tenga que volver a organizar (Esta variable se almacena en el dispositivo):
	public bool eval_mode_phase1_steps_organized;
	//varisble que controla si ya se han organizado los pasos de la FASE 2:
	public bool eval_mode_phase2_steps_organized;


	//variables que permiten controlar los pasos de cada fase:
	private StepOfProcess[] steps_phase_one_completed;
	private StepOfProcess[] steps_phase_two_completed;

	//variables que permiten controlar los pasos de cada fase en el MODO EVALUACION:
	private StepOfProcessEvalMode[] steps_p_one_eval_completed;
	private StepOfProcessEvalMode[] steps_p_two_eval_completed;

	//variables que controlan la activacion de las fases en el modo evaluacion:
	public bool phase_one_enable_eval_mode;
	public bool phase_two_enable_eval_mode;
	public bool phase_three_enable_eval_mode;
	public bool phase_four_enable_eval_mode;
	public bool phase_five_enable_eval_mode;
	public bool phase_six_enable_eval_mode;

	#endregion PUBLIC_MEMBER_VARIABLES

    #region PROTECTED_MEMBER_VARIABLES
    public static ViewType mActiveViewType;
	public static CurrentInterface current_interface;
     
	public enum ViewType { SPLASHVIEW, ABOUTVIEW, UIVIEW, ARCAMERAVIEW, TESTING };

	public enum CurrentInterface 
	{
		CHALLENGE, 
		SELECTION_OF_MODE,
		CONFIG_OPTIONS,
		MENU_PHASES,
		MENU_PHASES_EV,
		MENU_STEPS_PHASE1,
		MENU_STEPS_PHASE2,
		MENU_SUB_STEPS_PHASE2,
		MENU_SUB_STEPS_INTERIORES_PHASE2,
		MENU_STEPS_PHASE1_EV,
		MENU_STEPS_PHASE2_EV,
		ACTIVITIES_PHASE1_STEP1,
		ACTIVITIES_PHASE1_STEP1_EV,
		ACTIVITIES_PHASE1_STEP2,
		ACTIVITIES_PHASE1_STEP2_EV,
		ACTIVITIES_PHASE1_STEP3,
		ACTIVITIES_PHASE1_STEP3_EV,
		ACTIVITIES_PHASE1_STEP4,
		ACTIVITIES_PHASE1_STEP4_EV,
		ACTIVITIES_PHASE1_STEP5,
		ACTIVITIES_PHASE1_STEP5_EV,
		ACTIVITIES_PHASE1_STEP6,
		ACTIVITIES_PHASE1_STEP6_EV,
		ACTIVITIES_PHASE2_STEP1,
		ACTIVITIES_PHASE2_STEP1_EV,
		ACTIVITIES_PHASE2_STEP2,
		ACTIVITIES_PHASE2_STEP2_EV,
		ACTIVITIES_PHASE2_STEP3,
		ACTIVITIES_PHASE2_STEP3_EV,
		ACTIVITIES_PHASE2_STEP4,
		ACTIVITIES_PHASE2_STEP4_EV,
		ACTIVITIES_PHASE2_STEP5,
		ACTIVITIES_PHASE2_STEP5_EV,
		ACTIVITIES_PHASE2_STEP6,
		ACTIVITIES_PHASE2_STEP6_EV,
		ACTIVITIES_PHASE2_STEP7,
		ACTIVITIES_PHASE2_STEP7_EV,
		ACTIVITIES_PHASE2_STEP8,
		ACTIVITIES_PHASE2_STEP8_EV,
		AR_SEARCH_CAR_HOOD,
		AR_SEARCH_CAR_HOOD_EVAL,
		TOOLS_AND_PRODUCTS,
		TOOLS_AND_PRODUCTS_EVAL,
		TAKE_PICTURES,
		TAKE_AND_SEND_THE_PICTURE,
		AR_SEARCH_PRODUCTS_TUTORIAL,
		AR_SEARCH_AGUA_JABON_PRODUCT_TUTORIAL,
		AR_SEARCH_BAYETA_PRODUCT_TUTORIAL,
		AR_SEARCH_PHASE1_STEP3,
		AR_SEARCH_PHASE1_STEP4,
		AR_SEARCH_PHASE1_STEP5,
		AR_SEARCH_PHASE1_STEP6,
		AR_SEARCH_PHASE2_STEP2,
		AR_SEARCH_PHASE2_STEP3,
		AR_SEARCH_PHASE2_STEP4,
		AR_SEARCH_PHASE2_STEP5,
		AR_SEARCH_PHASE2_STEP6,
		AR_SEARCH_PHASE2_STEP7,
		AR_SEARCH_PHASE2_STEP8,
		AR_SEARCH_PHASE1_STEP2_EV,
		AR_SEARCH_PHASE1_STEP3_EV,
		AR_SEARCH_PHASE1_STEP4_EV,
		AR_SEARCH_PHASE1_STEP5_EV,
		AR_SEARCH_PHASE1_STEP6_EV,
		AR_SEARCH_PHASE2_STEP2_EV,
		AR_SEARCH_PHASE2_STEP3_EV,
		AR_SEARCH_PHASE2_STEP4_EV,
		AR_SEARCH_PHASE2_STEP5_EV,
		AR_SEARCH_PHASE2_STEP6_EV,
		AR_SEARCH_PHASE2_STEP7_EV,
		AR_SEARCH_PHASE2_STEP8_EV,
		INFORMATIVE_MODE


	};
    
    #endregion PROTECTED_MEMBER_VARIABLES

    #region PRIVATE_MEMBER_VARIABLES

	private int processOrder; //esta variable controla el orden de los pasos del proceso.

    private SplashScreenView mSplashView;
    private AboutScreenView mAboutView;
    private float mSecondsVisible = 4.0f;
	//list of instanced objects for each interface:
	private GameObject login_user_interface_instance;
	private GameObject instance_interface;
	private GameObject challenge_interface;
	private GameObject challenge_interface_compact;
	//Este objeto es publico para poderlo destruir desde InformativeMode
	public GameObject selectionOfMode_interface_instance;
	private GameObject configuration_opts_interface_instance;
	private GameObject menuProcessPhases_interface_instance;
	private GameObject menuStepsPhase1_interface_instance;
	private GameObject menuStepsPhaseTwo_interface_instance;
	private GameObject menuSubStepsPhaseTwo_interface_instance;
	private GameObject menuSubStepsPhaseTwoInterior_interf_instance;
	private GameObject ActivitiesForPhase1Step1_interface_instance;
	private GameObject ActivitiesForPhase1Step2_interface_instance;
	private GameObject ActivitiesForPhase1Step3_interface_instance;
	private GameObject ActivitiesForPhase1Step4_interface_instance;
	private GameObject ActivitiesForPhase1Step5_interface_instance;
	private GameObject ActivitiesForPhase1Step6_interface_instance;
	private GameObject ActivitiesForPhase2Step1_interface_instance;
	private GameObject ActivitiesForPhase2Step2_interface_instance;
	private GameObject ActivitiesForPhase2Step3_interface_instance;
	private GameObject ActivitiesForPhase2Step4_interface_instance;
	private GameObject ActivitiesForPhase2Step5_interface_instance;
	private GameObject ActivitiesForPhase2Step6_interface_instance;
	private GameObject ActivitiesForPhase2Step7_interface_instance;
	private GameObject ActivitiesForPhase2Step8_interface_instance;
	private GameObject TurorialSearchCapoCarro_interface_instance;
	//Tools and products phase 1:
	private GameObject ToolsAndProductsPhase1Step2_interface_instance;
	private GameObject ToolsAndProductsPhase1Step3_interface_instance;
	private GameObject ToolsAndProductsPhase1Step4_interface_instance;
	private GameObject ToolsAndProductsPhase1Step5_interface_instance;
	private GameObject ToolsAndProductsPhase1Step6_interface_instance;
	//Tools and products phase 2:
	private GameObject ToolsAndProductsPhase2Step2_interface_instance;
	private GameObject ToolsAndProductsPhase2Step3_interface_instance;
	private GameObject ToolsAndProductsPhase2Step4_interface_instance;
	private GameObject ToolsAndProductsPhase2Step5_interface_instance;
	private GameObject ToolsAndProductsPhase2Step6_interface_instance;
	private GameObject ToolsAndProductsPhase2Step7_interface_instance;
	private GameObject ToolsAndProductsPhase2Step8_interface_instance;
	private GameObject TakePicturesPhase1Step6_interface_instance;
	private GameObject TutorialPhaseTwoSearchProd_interface_instance;
	private GameObject TutorialTwoSearchBayeta_interface_instance;
	private GameObject AR_Mode_Search_interface_instance;
	private GameObject TakePictureCamera_interface_instance;


	//variables de interfaz para el modo evaluacion:
	private GameObject menuProcessPhases_int_eval_instance;
	//variables que definen las interfaces para organizar pasos del modo evaluacion:
	private GameObject menuStepsPhase1_int_eval_instance;
	private GameObject menuStepsPhase2_int_eval_instance;
	//variables que definen las interfaces de Actividades de cada paso en el modo evaluacion FASE 1:
	private GameObject ActivitiesForPhase1Step1_int_eval_instance;
	private GameObject ActivitiesForPhase1Step2_int_eval_instance;
	private GameObject ActivitiesForPhase1Step3_int_eval_instance;
	private GameObject ActivitiesForPhase1Step4_int_eval_instance;
	private GameObject ActivitiesForPhase1Step5_int_eval_instance;
	private GameObject ActivitiesForPhase1Step6_int_eval_instance;
	//variables que definen las interfaces de Actividades de cada paso en el modo evaluacion FASE 1:
	private GameObject ActivitiesForPhase2Step1_int_eval_instance;
	private GameObject ActivitiesForPhase2Step2_int_eval_instance;
	private GameObject ActivitiesForPhase2Step3_int_eval_instance;
	private GameObject ActivitiesForPhase2Step4_int_eval_instance;
	private GameObject ActivitiesForPhase2Step5_int_eval_instance;
	private GameObject ActivitiesForPhase2Step6_int_eval_instance;
	private GameObject ActivitiesForPhase2Step7_int_eval_instance;
	private GameObject ActivitiesForPhase2Step8_int_eval_instance;

	//Variables para el modo RA:
	private GameObject SearchCapoCarroEvaluationMode_instance;
	private GameObject ARSearch_eval_mode_instance;

	//variables para el ToolsAndProducts del modo evaluacion (FASE1):
	private GameObject ToolsAndProductsPhase1Step2_int_eval_instance;
	private GameObject ToolsAndProductsPhase1Step3_int_eval_instance;
	private GameObject ToolsAndProductsPhase1Step4_int_eval_instance;
	private GameObject ToolsAndProductsPhase1Step5_int_eval_instance;
	private GameObject ToolsAndProductsPhase1Step6_int_eval_instance;

	//variables para el ToolsAndProducts del modo evaluacion (FASE2):
	private GameObject ToolsAndProductsPhase2Step2_int_eval_instance;
	private GameObject ToolsAndProductsPhase2Step3_int_eval_instance;
	private GameObject ToolsAndProductsPhase2Step4_int_eval_instance;
	private GameObject ToolsAndProductsPhase2Step5_int_eval_instance;
	private GameObject ToolsAndProductsPhase2Step6_int_eval_instance;
	private GameObject ToolsAndProductsPhase2Step7_int_eval_instance;
	private GameObject ToolsAndProductsPhase2Step8_int_eval_instance;

	//variable que determina si la app se esta ejecutando en un movil con pantalla muy pequeña:
	private bool compact_mode;
	private bool goBackFromOtherInterface;
	private string interfaceComingBackFrom;

	//Variable para almacenar el conjunto de marcadores para controlar el blinking:
	private GameObject[] markers;
	private DefaultTrackableEventHandler markerHandler;
	private int[] order_in_markerhandler;
	public GameObject interfaceInstanceActive;

	//variable para almacenar el orden de los pasos que va a controlar cada interfaz del proceso:
	public int[] order_in_process;

	//variable generica que almacena la referencia a un marcador en general:
	private GameObject markerInScene;
	//variable que almacena la referencia al controlador para carga de informacion de cada marcador:
	ControllerAddInfoInMarker controller_info_marker;
	ControllerBlinkingARGeneric controller_info_generic_ar;

	//variable que permite controlar si el estudiante se puede devolver de la interfaz de tomar fotos o no
	//hasta que se haya enviado completamente la foto hacia el servidor:
	public bool can_return_from_take_photo;

	//variable para controlar que la informacion ya se haya cargado desde el marcador antes de invocar el metodo PrepareAdditionalInfo
	public bool informationLoadedFromMarker;

	//variables que definen las URL a los videos que se van a reproducir:
	//private string video_phase1_step2 = "http://www.androidbegin.com/tutorial/AndroidCommercial.3gp";
	private string video_phase1_step2 = "http://piranya.udg.edu/pintuRA/videos/phase1_step2.mp4";
	private string video_phase1_step3 = "http://piranya.udg.edu/pintuRA/videos/phase1_step3.mp4";
	private string video_phase1_step4 = "http://piranya.udg.edu/pintuRA/videos/phase1_step4.mp4";
	private string video_phase1_step5 = "http://piranya.udg.edu/pintuRA/videos/1_limpieza_convert.mp4";
	private string video_phase1_step6 = "http://piranya.udg.edu/pintuRA/videos/phase1_step6.mp4";
	private string video_matizado_phase = "http://piranya.udg.edu/pintuRA/videos/phase2.mp4";

	//Variables que definen los textos de las interfaces:
	//Interfaz de el reto:
	private string challenge_interface_title="El Reto...";
	private string challenge_interface_introduction_text_path = "Texts/0_reto";
	private string challenge_interface_image_header_path = "Sprites/ImagenPresentacion";
	private string challenge_interface_text_continue_path="Texts/0_reto_continue";
	//la funcion que controla el boton continuar se llama GoToSelectionOfMode

	//Interfaz seleccion de modo:
	private string selectionMode_interface_title = "Seleccionar Modo";
	private string selectionMode_interface_introduction_text_path = "Texts/1_seleccionar_modo";
	//la funcion que controla el boton 'modo guiado' se llama: 

	//interfaz de menu de fases (6 fases limpieza, matizado, masillado, aparejado, pintado, barnizado):
	private string menuPhases_interface_title= "Fases del Proceso";
	private string menuPhases_interface_introduction_text_path = "Texts/2_menuPhases";
	private string menuPhases_interface_button_uno_text = "1. Limpieza";
	private string menuPhases_interface_button_dos_text = "2. Matizado";
	private string menuPhases_interface_button_tres_text = "3. Masillado";
	private string menuPhases_interface_button_cuatro_text = "4. Aparejado";
	private string menuPhases_interface_button_cinco_text = "5. Pintado";
	private string menuPhases_interface_button_seis_text = "6. Barnizado";
	private string menuPhases_interface_button_uno_image = "Sprites/1_Limpieza_FaseProceso";
	private string menuPhases_interface_button_dos_image = "Sprites/2_matizado_FaseProceso";
	private string menuPhases_interface_button_tres_image = "Sprites/3_masillado_FaseProceso";
	private string menuPhases_interface_button_cuatro_image = "Sprites/4_aparejado_FaseProceso";
	private string menuPhases_interface_button_cinco_image = "Sprites/5_pintado_FaseProceso";
	private string menuPhases_interface_button_seis_image = "Sprites/6_barnizado_FaseProceso";

	//listado de variables que definen las rutas a los botones con fondo gris:
	private string menuPhases_int_btn_uno_image_gray = "Sprites/1_Limpieza_FaseProceso_gray";
	private string menuPhases_int_btn_dos_image_gray = "Sprites/2_matizado_FaseProceso_gray";
	private string menuPhases_int_btn_tres_image_gray = "Sprites/3_masillado_FaseProceso_gray";
	private string menuPhases_int_btn_cuatro_image_gray = "Sprites/4_aparejado_FaseProceso_gray";
	private string menuPhases_int_btn_cinco_image_gray = "Sprites/5_pintado_FaseProceso_gray";
	private string menuPhases_int_btn_seis_image_gray = "Sprites/6_barnizado_FaseProceso_gray";

	//Interfaz del menu de pasos de la fase 1:
	private string menuStepsPhase1_image_header = "Sprites/images_phases_text/1_Limpieza_FaseProceso";
	private string menuStepsPhase1_interface_title = "Pasos de la fase de limpieza";
	private string menuStepsPhase1_introduction_text_path = "Texts/3_menuStep1_text";
	private string menuStepsPhase1_button_uno_text = "1. Buscar Capo del Coche";
	private string menuStepsPhase1_button_dos_text = "2. Limpieza";
	private string menuStepsPhase1_button_tres_text = "3. Aclarado y Secado";
	private string menuStepsPhase1_button_cuatro_text = "4. Localizar Irregularidades";
	private string menuStepsPhase1_button_cinco_text = "5. Corregir Irregularidades";
	private string menuStepsPhase1_button_seis_text = "6. Desengrasado";
	private string menuStepsPhase1_interface_button_uno_image = "Sprites/phase1/Step1_Phase1";
	private string menuStepsPhase1_interface_button_dos_image = "Sprites/phase1/Step2_Phase1";
	private string menuStepsPhase1_interface_button_tres_image = "Sprites/phase1/Step3_Phase1";
	private string menuStepsPhase1_interface_button_cuatro_image = "Sprites/phase1/Step4_Phase1";
	private string menuStepsPhase1_interface_button_cinco_image = "Sprites/phase1/Step5_Phase1";
	private string menuStepsPhase1_interface_button_seis_image = "Sprites/phase1/Step6_Phase1";

	//Rutas a las imagenes de cada uno de los pasos de la fase 1 sin texto y con escala de grises:
	private string menuStepsPhase1_int_btn_dos_image_gray = "Sprites/phase1/Step2_Phase1_gray";
	private string menuStepsPhase1_int_btn_tres_image_gray = "Sprites/phase1/Step3_Phase1_gray";
	private string menuStepsPhase1_int_btn_cuatro_image_gray = "Sprites/phase1/Step4_Phase1_gray";
	private string menuStepsPhase1_int_btn_cinco_image_gray = "Sprites/phase1/Step5_Phase1_gray";
	private string menuStepsPhase1_int_btn_seis_image_gray = "Sprites/phase1/Step6_Phase1_gray";


	//Rutas a las imagenes de cada una de las fases (estas imagenes tienen el texto embebido):
	private string phase1_with_text_image_path = "Sprites/images_phases_text/1_Limpieza_FaseProceso";
	private string phase2_with_text_image_path = "Sprites/images_phases_text/2_matizado_FaseProceso";
	private string phase3_with_text_image_path = "Sprites/images_phases_text/3_masillado_FaseProceso";
	private string phase4_with_text_image_path = "Sprites/images_phases_text/4_aparejado_FaseProceso";
	private string phase5_with_text_image_path = "Sprites/images_phases_text/5_pintado_FaseProceso";
	private string phase6_with_text_image_path = "Sprites/images_phases_text/6_barnizado_FaseProceso";

	//Rutas a las imagenes de cada una de las fases (estas imagenes tienen el texto embebido y estan en escala de gris):
	private string phase1_with_text_image_gray_path = "Sprites/images_phases_text/1_Limpieza_FaseProceso_gris";
	private string phase2_with_text_image_gray_path = "Sprites/images_phases_text/2_matizado_FaseProceso_gris";
	private string phase3_with_text_image_gray_path = "Sprites/images_phases_text/3_masillado_FaseProceso_gris";
	private string phase4_with_text_image_gray_path = "Sprites/images_phases_text/4_aparejado_FaseProceso_gris";
	private string phase5_with_text_image_gray_path = "Sprites/images_phases_text/5_pintado_FaseProceso_gris";
	private string phase6_with_text_image_gray_path = "Sprites/images_phases_text/6_barnizado_FaseProceso_gris";

	//Rutas a las imagenes de cada uno de los pasos de la FASE 1 - Limpieza (imagenes con texto emebebido y en GRIS):
	private string image_phase1_step1_text_gray = "Sprites/phase1/Step1_Phase1_text_gray";
	private string image_phase1_step2_text_gray = "Sprites/phase1/Step2_Phase1_text_gray";
	private string image_phase1_step3_text_gray = "Sprites/phase1/Step3_Phase1_text_gray";
	private string image_phase1_step4_text_gray = "Sprites/phase1/Step4_Phase1_text_gray";
	private string image_phase1_step5_text_gray = "Sprites/phase1/Step5_Phase1_text_gray";
	private string image_phase1_step6_text_gray = "Sprites/phase1/Step6_Phase1_text_gray";


	//Variables generales para configurar las imagenes de cada una de las actividades disponibles para cada paso (A Color):
	private string image_uno_tools_and_products="Sprites/images_activities/1_herramientasAR";
	private string image_dos_videos="Sprites/images_activities/2_videos_procedimiento_icon";
	private string image_tres_self_assessment="Sprites/images_activities/3_autoevaluacion_icon";
	private string image_cuatro_simulations="Sprites/images_activities/4_simulation_icon";
	private string image_cuatro_fotos = "Sprites/images_activities/4_take_pictures";
	private string image_cinco_personal_notes="Sprites/images_activities/5_apuntes_estudiante_icon";
	private string image_seis_frequently_questions="Sprites/images_activities/6_faq_icon";
	private string image_siete_ask_your_teacher="Sprites/images_activities/7_preguntarProfesor";
	//Variables generales para configurar las imagenes de cada una de las actividades disponibles para cada paso en GRIS:
	private string image_uno_tools_produc_gray = "Sprites/images_activities/1_herramientasAR_gray";
	private string image_dos_videos_gray="Sprites/images_activities/2_videos_procedimiento_icon_gray";
	private string image_tres_self_assessment_gray="Sprites/images_activities/3_autoevaluacion_icon_gray";
	private string image_cuatro_simulations_gray="Sprites/images_activities/4_simulation_icon_gray";
	private string image_cuatro_fotos_gray = "Sprites/images_activities/4_take_pictures_gray";
	private string image_cinco_personal_notes_gray="Sprites/images_activities/5_apuntes_estudiante_icon_gray";
	private string image_seis_frequently_questions_gray="Sprites/images_activities/6_faq_icon_gray";
	private string image_siete_ask_your_teacher_gray="Sprites/images_activities/7_preguntarProfesor_gray";

	//variables para configurar la interfaz del Fase1 - Paso 1 (buscar capo carro):
	private string title_phase1_step1 = "1. Buscar el capo del coche";
	private string image_buscar_capo_path = "Sprites/phase1/Step1_Phase1_text";
	private string image_content_marker = "Sprites/markers/frameMarker_001";
	private string introduction_text_phase1Step1_path_one = "Texts/4_Phase1Step1_text";
	private string introduction_text_phase1Step1_path_two = "Texts/4_Phase1Step1_text_dos";
	//variables para configurar la parte del tutorial 1 para busqueda del capo del carro:
	private string image_marker_p = "Sprites/markers/frameMarker_001";
	private string image_marker_real_p = "Sprites/markers/frameMarker_001";
	private string text_to_show_blink = "Busca el codigo";
	private string text_to_show_blink_touch = "Pulsa la pantalla para ver opciones";
	private string image_hand_touch_p = "Sprites/buttons/single_tap";
	private string image_btn_select = "Sprites/buttons/btn_seleccionar";
	private string feedback_text_path = "Texts/5_feedback_capo_carro";


	//Variables para configurar la interfaz del listado de actividades Fase 1 - Paso 2 (limpieza):
	private string title_phase1_step2 = "2. Limpieza y desengrasado";
	private string image_header_phase1Step2 = "Sprites/phase1/Step2_Phase1_text";
	private string introduction_text_phase1Step2_path = "Texts/Phase1Step2/0_Phase1Step2_text";

	//variables para configurar la parte del tutorial 2 para la busqueda de productos y herramientas (agua a pressio):
	private string image_marker_tutorial2_p = "Sprites/markers/frameMarker16_maquina_agua";
	private string image_marker_tutorial2_real_p = "Sprites/phase1step2/FrameMarker16_maquina_agua";
	private string img_eval_mode_hint_marker16 = "Sprites/phase1step2_eval/FrameMarker16_maquina_agua";
	private string image_btn_select_path = "Sprites/buttons/btn_seleccionar";
	private string image_btn_one_path = "Sprites/buttons/guantes_info";
	private string image_btn_two_path = "Sprites/buttons/";
	private string text_add_info_btn_one = "Texts/Phase1Step2/6_add_info_btn_one_text";
	private string text_feedback = "Texts/Phase1Step2/7_feedback_text";

	//variables para configurar la parte del tutorial 2 para la busqueda de productos y herramientas (agua y jabon):
	private string image_marker_tut2_p = "Sprites/markers/frameMarker19_agua_jabon";
	private string image_marker_tut2_real_p = "Sprites/phase1step2/FrameMarker19_agua_jabon";
	private string img_eval_mode_hint_marker19 = "Sprites/phase1step2_eval/FrameMarker19_agua_jabon";
	private string text_feedback_jabon = "Texts/Phase1Step2/7_feedback_text_jabon";

	//variables para configurar la parte de buscar la bayeta como parte final del tutorial 2: (Bayeta)
	private string image_marker3_tutorial2_p = "Sprites/markers/frameMarker21_bayeta_limpiar";
	private string image_marker3_tutorial2_real_p = "Sprites/phase1step2/FrameMarker21_baieta_neteja";
	private string img_eval_mode_hint_marker21 = "Sprites/phase1step2_eval/FrameMarker21_baieta_neteja";
	private string text_feedback_bayeta = "Texts/Phase1Step2/7_feedback_text_bayeta";

	//variable que configura el feedback del agua a presion en la Fase 1 - Paso 3 (secado):
	private string feedback_phase1step3_agua = "Texts/Phase1Step3/7_feedback_text_agua";

	//Variables para configurar la interfaz del listado de actividades Fase 1 - Paso 3 (secado):
	private string title_phase1_step3 = "3. Secado";
	private string image_header_phase1Step3 = "Sprites/phase1/Step3_Phase1_text";
	private string introduction_text_phase1Step3_path = "Texts/Phase1Step3/0_Phase1Step3_text";
	//variables que configuran la interfaz del modo RA para la fase 1 paso 3 (secado) - Objeto - Papel absorbente (DC3430):
	private string image_marker25_p = "Sprites/markers/frameMarker25_absorbente_dc3430";
	private string image_marker25_real_p = "Sprites/phase1step3/FrameMarker25_papel_dc3430";
	private string img_eval_mode_hint_marker25 = "Sprites/phase1step3_eval/FrameMarker25_papel_dc3430";
	private string text_feedback_marker25_dc3430 = "Texts/Phase1Step3/8_feedback_text_papel_absorb";
	private string marker25_text_add_info_btn_one = "Texts/Phase1Step3/6_add_info_btn_one_text";

	//variables que configuran la interfaz del modo RA para la fase 1 paso 3 (secado) - Objeto - paper neteja:
	private string image_marker24_p = "Sprites/markers/frameMarker24_papel_limpieza";
	private string image_marker24_real_p = "Sprites/phase1step3/FrameMarker24_paper_neteja";
	private string img_eval_mode_hint_marker24 = "Sprites/phase1step3_eval/FrameMarker24_paper_neteja";
	private string text_feedback_marker24_paper = "Texts/Phase1Step3/8_feedback_text_paper_neteja";
	private string marker24_text_add_info_btn_one = "Texts/Phase1Step3/6_add_info_btn_one_text";
	/*
	//variables que configuran la interfaz del modo RA par ala fase 1 del paso 3 (secado) - Objeto - Paper de Neteja:
	private string image_marker4_p = "Sprites/markers/frameMarker_004";
	private string image_marker4_real_p = "Sprites/phase1step3/papel_absorbente";
	private string text_feedback_marker4_papel = "Texts/Phase1Step3/8_feedback_text_papel_absorb";
	private string marker4_text_add_info_btn_one = "Texts/Phase1Step3/6_add_info_btn_one_text";
	*/
	//variables para configurar la interfaz del listado de actividades Fase1 - Paso 4 (Localizar irregularidades):
	private string title_phase1_step4 = "4. Localizar Irregularidades";
	private string image_header_phase1Step4 = "Sprites/phase1/Step4_Phase1_text";
	private string introduction_text_phase1Step4_path = "Texts/Phase1Step4/0_Phase1Step4_text";
	//variables que configuran la interfaz del modo RA para la fase 1 paso 4 (Localizar Irregularidades) - Objeto: Lija
	private string image_marker45_p = "Sprites/markers/frameMarker45_esponja_p320";
	private string image_marker45_real_p = "Sprites/phase1step4/FrameMarker45_esponja_p320";
	private string img_eval_mode_hint_marker45 = "Sprites/phase1step4_eval/FrameMarker45_esponja_p320";
	private string text_feedback_marker45_lija = "Texts/Phase1Step4/8_feedback_text_lija";
	private string marker5_text_add_info_btn_one = "Texts/Phase1Step4/6_add_info_btn_one_text";
	private string marker5_text_add_info_btn_two = "Texts/Phase1Step4/7_add_info_btn_two_text";
	private string image_button_two_mascara_polv = "Sprites/buttons/mascara_info";
	//variables que permiten configurar la imagen del marcador 46:
	private string image_marker46_p = "Sprites/markers/frameMarker46_esponja_p400";
	private string image_marker46_real_p = "Sprites/phase1step4/FrameMarker46_esponja_p400";
	private string text_feedback_marker46_lija = "Texts/Phase1Step4/8_feedback_text_lija";
	private string img_eval_mode_hint_marker46 = "Sprites/phase1step4_eval/FrameMarker46_esponja_p400";


	//variables para configurar la interfaz del listado de actividades de la Fase1-Paso5 (Corregir irregularidades)
	private string title_phase1_step5 = "5. Corregir Irregularidades";
	private string image_header_phase1Step5 = "Sprites/phase1/Step5_Phase1_text";
	private string introduction_text_phase1Step5_path = "Texts/Phase1Step5/0_Phase1Step5_text";
	//variables que configuran la interfaz del modo RA para la fase 1 paso 5 (corregir Irregularidades) - Objeto: martillo repasar
	private string image_marker100_p = "Sprites/markers/frameMarker100_martillo";
	private string image_marker100_real_p = "Sprites/phase1step5/FrameMarker100_martillo_repasar";
	private string img_eval_mode_hint_marker100 = "Sprites/phase1step5_eval/FrameMarker100_martillo_repasar";
	private string text_feedback_marker6_martillo = "Texts/Phase1Step5/4_feedback_text_martillo_repasar";
	private string marker6_text_add_info_btn_one = "Texts/Phase1Step5/5_add_info_btn_one_text";

	//variables para configurar la interfaz del listado de actividades de la Fase1-Paso6 (Desengrasado):
	private string title_phase1_step6 = "6. Desengrasado";
	private string image_header_phase1Step6 = "Sprites/phase1/Step6_Phase1_text";
	private string introduction_text_phase1Step6_path = "Texts/Phase1Step6/0_Phase1Step6_text";
	//variables que configuran la interfaz del modo RA para la fase 1 paso 6 (desengrasado) - Objeto: desengrasante
	private string image_marker26_p = "Sprites/markers/frameMarker26_desengrasante_da93";
	private string image_marker26_real_p = "Sprites/phase1step6/FrameMarker26_desengrasante";
	private string text_feedback_marker26_desengras = "Texts/Phase1Step6/8_feedback_text_desengrasante_bayeta";
	private string img_eval_mode_hint_marker26 = "Sprites/phase1step6_eval/FrameMarker26_desengrasante";
	private string marker26_text_add_info_btn_one = "Texts/Phase1Step6/6_add_info_btn_one_text";
	private string marker26_text_add_info_btn_two = "Texts/Phase1Step6/7_add_info_btn_two_text";
	//variables que configuran la interfaz del modo RA para la fase 1 paso 6 segunda parte : - Objeto: bayeta
	private string image_button_two_mascara_gas = "Sprites/buttons/mascara_gas";


	//*******************************************************************************************************************
	//CONFIGURANDO LA FASE 2 - MATIZADO:
	//VARIABLES de las imagenes a color con el texto de cada uno de los pasos del proceso:
	private string image_phase2step1_with_text = "Sprites/phase2/Step1_Phase2_text";
	private string image_phase2step2_with_text = "Sprites/phase2/Step2_Phase2_text";
	private string image_phase2step3_with_text = "Sprites/phase2/Step3_Phase2_text";
	private string image_phase2step4_with_text = "Sprites/phase2/Step4_Phase2_text";
	private string image_phase2step5_with_text = "";
	private string image_phase2step6_with_text = "";

	//Variables de las imagenes en gris con texto de cada uno de los pasos del proceso:
	private string image_phase2step1_with_text_gray = "Sprites/phase2/Step1_Phase2_text_gray";
	private string image_phase2step2_with_text_gray = "Sprites/phase2/Step2_Phase2_text_gray";
	private string image_phase2step3_with_text_gray = "Sprites/phase2/Step3_Phase2_text_gray";
	private string image_phase2step4_with_text_gray = "Sprites/phase2/Step4_Phase2_text_gray";
	private string image_phase2step5_with_text_gray = "";
	private string image_phase2step6_with_text_gray = "";

	//VARIABLES de la interfaz de pasos generales del proceso:
	private string menuStepsPhaseTwo_interface_title = "Pasos de la fase de Matizado";
	private string menuStepsPhaseTwo_introduction_text_path = "Texts/Phase2/0_introduction_text";
	private string menuStepsPhaseTwo_image_header = "Sprites/images_phases_text/2_matizado_FaseProceso";
	private string menuStepsPhaseTwo_button_uno_text = "1. Introduccion";
	private string menuStepsPhaseTwo_button_dos_text = "2. Proteccion Superficies";
	private string menuStepsPhaseTwo_button_tres_text = "3. Lijado Cantos";
	private string menuStepsPhaseTwo_button_cuatro_text = "4. Lijado Interiores";
	private string menuStepsPhaseTwo_button_cinco_text = "";
	private string menuStepsPhaseTwo_button_seis_text = "";
	//NOTA: Las siguientes imagenes no tienen el texto embebido y estan en color:
	private string menuStepsPhaseTwo_interface_button_uno_image = "Sprites/phase2/Step1_Phase2";
	private string menuStepsPhaseTwo_interface_button_dos_image = "Sprites/phase2/Step2_Phase2";
	private string menuStepsPhaseTwo_interface_button_tres_image = "Sprites/phase2/Step3_Phase2";
	private string menuStepsPhaseTwo_interface_button_cuatro_image = "Sprites/phase2/Step4_Phase2";
	private string menuStepsPhaseTwo_interface_button_cinco_image = "";
	private string menuStepsPhaseTwo_interface_button_seis_image = "";

	//Variables para las imagenes en gris para mostrar que los pasos estan deshabilitados:
	private string menuStepsPhaseTwo_int_btn_uno_image_gray = "Sprites/phase2/Step1_Phase2_text_gray";
	private string menuStepsPhaseTwo_int_btn_dos_image_gray = "Sprites/phase2/Step2_Phase2_text_gray";
	private string menuStepsPhaseTwo_int_btn_tres_image_gray = "Sprites/phase2/Step3_Phase2_text_gray";
	private string menuStepsPhaseTwo_int_btn_cuatro_image_gray = "Sprites/phase2/Step4_Phase2_text_gray";
	private string menuStepsPhasTwo1_int_btn_cinco_image_gray = "Sprites/phase2/Step4_Phase2_text_gray";
	private string menuStepsPhaseTwo_int_btn_seis_image_gray = "Sprites/phase2/Step4_Phase2_text_gray";

	//Variables para la simagenes en gris para cada uno de los pasos pero sin el texto embebido:
	private string stepsPhaseTwo_img_one_gray_notxt = "Sprites/phase2/Step2_Phase2_gray_notxt";
	private string stepsPhaseTwo_img_two_gray_notxt = "Sprites/phase2/Step2_Phase2_gray_notxt";
	private string stepsPhaseTwo_img_three_gray_notxt = "Sprites/phase2/Step3_1_Phase2_gray_notxt";
	private string stepsPhaseTwo_img_four_gray_notxt = "Sprites/phase2/Step3_2_Phase2_gray_notxt";
	private string stepsPhaseTwo_img_five_gray_notxt = "Sprites/phase2/Step4_1_Phase2_gray_notxt";
	private string stepsPhaseTwo_img_six_gray_notxt = "Sprites/phase2/Step4_2_Phase2_gray_notxt";
	private string stepsPhaseTwo_img_seven_gray_notxt = "Sprites/phase2/Step4_3_Phase2_gray_notxt";
	private string stepsPhaseTwo_img_eight_gray_notxt = "Sprites/phase2/Step4_4_Phase2_gray_notxt";

	//variables que configuran el sub-menu de pasos del paso de LIJADO DE CANTOS:
	private string menuSubStepsPhaseTwo_interface_title = "Sub-pasos del Lijado de Cantos";
	//debido a que solo son 2 pasos solo se definen dos imagenes de pasos:
	private string menuSubStepsPhaseTwo_int_button_uno_image = "Sprites/phase2/Step3_1_Phase2";
	private string menuSubStepsPhaseTwo_int_button_dos_image = "Sprites/phase2/Step3_2_Phase2";
	//definiendo imags en escala de grises:
	private string menuSubStepsPhaseTwo_int_button_uno_image_gray = "Sprites/phase2/Step3_1_Phase2_text_gray";
	private string menuSubStepsPhaseTwo_int_button_dos_image_gray = "Sprites/phase2/Step3_2_Phase2_text_gray";
	//definiendo textos de los dos pasos del submenu (Lijado de Cantos primera y segunda pasada):
	private string menuSubStepsPhaseTwo_button_uno_text = "3.1. Primera Pasada(Cantos)";
	private string menuSubStepsPhaseTwo_button_dos_text = "3.2. Segunda Pasada(Cantos)";
	private string menuSubStepsPhaseTwo_int_btn_tres_imag = "Sprites/phase2/Step3_Phase2_text";

	//variables que configuran el sub-menu de pasos del paso LIJADO DE INTERIORES:
	private string menuSubStepsPhaseTwo_interior_int_title = "Sub-pasos del Lijado de Interiores";
	//Definiendo los textos que se colocan como nombres de los pasos:
	private string menuSubStepsInteriores_btn_uno_text = "4.1. Primera Pasada(Interior)";
	private string menuSubStepsInteriores_btn_dos_text = "4.2. Segunda Pasada(Interior)";
	private string menuSubStepsInteriores_btn_tres_text = "4.3. Tercera Pasada(Interior)";
	private string menuSubStepsInteriores_btn_cuatro_text = "4.4. Pasada Final(Interior)";
	private string menuStepsPhaseTwo_int_btn_cuatro_with_text = "Sprites/phase2/Step4_Phase2_text_gray";
	private string menuStepsPhaseTwo_int_btn_cuatro_color = "Sprites/phase2/Step4_Phase2_text";
	//Se cargan las imagenes de los pasos en color pero sin el texto embebido:
	private string menuSubStepsP2_int_btn_uno_image = "Sprites/phase2/Step4_1_Phase2";
	private string menuSubStepsP2_int_btn_dos_image = "Sprites/phase2/Step4_2_Phase2";
	private string menuSubStepsP2_int_btn_tres_image = "Sprites/phase2/Step4_3_Phase2";
	private string menuSubStepsP2_int_btn_cuatro_image = "Sprites/phase2/Step4_4_Phase2";
	//se cargan las imagenes de los pasos en gris y con el texto embebido:
	private string menuSubStepsP2_int_btn_uno_image_gray = "Sprites/phase2/Step4_1_Phase2_text_gray";
	private string menuSubStepsP2_int_btn_dos_image_gray = "Sprites/phase2/Step4_2_Phase2_text_gray";
	private string menuSubStepsP2_int_btn_tres_image_gray = "Sprites/phase2/Step4_3_Phase2_text_gray";
	private string menuSubStepsP2_int_btn_cuatro_image_gray = "Sprites/phase2/Step4_4_Phase2_text_gray";

	//Variables para Configurar las actividades de FASE 2 - PASO1 (INTRODUCCION AL MATIZADO)
	private string image_intro_matizado_header_path = "Sprites/phase2/Step1_Phase2";
	private string title_phase2_step1 = "1. Introduccion al matizado";
	private string introduction_text_phase2Step1_path_one = "Texts/Phase2Step1/0_Phase1Step1_text";
	private string introduction_text_phase2Step1_path_two = "Texts/Phase2Step1/1_Phase1Step1_second_part";

	//Variables para configurar el paso 2 de la FASE 2 - PASO2 (Proteccion de la Superficie)
	private string title_phase2_step2 = "2. Proteccion de la Superficie";
	private string image_header_phase2Step2 = "Sprites/phase2/Step2_Phase2_text";
	private string introduction_text_phase2Step2_path = "Texts/Phase2Step1/2_introduction_to_step";
	private string text_feedback_marker65_cinta = "Texts/Phase2Step2/5_feedback_text_cinta_papel";
	//marcador de la cinta de enmascarar:
	private string image_marker65_p = "Sprites/markers/frameMarker65_cinta_enmascarar";
	private string image_marker65_real_p = "Sprites/phase2step2/FrameMarker65_cinta_enmascarar";
	private string marker65_text_add_info_btn_one = "Texts/Phase2Step2/6_add_info_btn_one_text";
	private string img_eval_mode_hint_marker65 = "Sprites/phase2step2_eval/FrameMarker65_cinta_enmascarar";
	//marcador del papel de enmascarar:
	private string image_marker69_p = "Sprites/markers/frameMarker69_papel_enmascarar";
	private string image_marker69_real_p = "Sprites/phase2step2/FrameMarker69_papel_enmascarar";
	private string marker69_text_add_info_btn_one = "Texts/Phase2Step2/7_add_info_btn_one_marker2";
	private string img_eval_mode_hint_marker69 = "Sprites/phase2step2_eval/FrameMarker69_papel_enmascarar";

	//Variables paa configurar las actividades de la FASE2 - PASO3 (Lijado de Cantos - Primera pasada):
	private string title_phase2_step3 = "3.1 Primera pasada del lijado de cantos";
	private string image_header_phase2Step3 = "Sprites/phase2/Step3_1_Phase2_text";
	private string introduction_text_phase2Step3_path = "Texts/Phase2Step3/0_introduction_text";
	private string text_feedback_phase2step3 = "Texts/Phase2Step3/6_feedback_text_phase2step3";

	//Variables para configurar las actividades de la FASE2 - PASO4 (Lijado de cantos - Pasada Final):
	private string title_phase2_step4 = "3.2 Pasada Final del lijado de cantos";
	private string image_header_phase2Step4 = "Sprites/phase2/Step3_2_Phase2_text";
	private string introduction_text_phase2Step4_path = "Texts/Phase2Step4/0_introduction_text";
	private string text_feedback_phase2step4 = "Texts/Phase2Step4/6_feedback_text_phase2step4";

	//variables para configurar las actividades de la FASE2 - Paso5 (Lijado de interiores - Primera pasada):
	private string title_phase2_step5 = "4.1. Primera Pasada del lijado de interiores";
	private string image_header_phase2Step5 = "Sprites/phase2/Step4_1_Phase2_text";
	private string introduction_text_phase2Step5_path = "Texts/Phase2Step5/0_introduction_text";
	private string text_feedback_phase2step5 = "Texts/Phase2Step5/6_feedback_text_phase2step4";
	//variables para configurar el marcador 99 (roto orbital):
	private string image_marker99_p = "Sprites/markers/frameMarker99_roto_orbital";
	private string image_marker99_real_p = "Sprites/phase2step5/FrameMarker99_roto_orbital";

	//Variables para configurar el marcador 30 (Disco abrasivo P80):
	private string image_marker30_p = "Sprites/markers/frameMarker30_disco_p80";
	private string image_marker30_real_p = "Sprites/phase2step5/FrameMarker30_disco_p80";
	//variables para configurar el marcador 32 (Disco abrasivo P120):
	private string image_marker32_p = "Sprites/markers/frameMarker32_disco_p120";
	private string image_marker32_real_p = "Sprites/phase2step5/FrameMarker32_disco_p120";

	//Variables para configurar las actividades de la FASE2 - Paso 6 (Lijado de interiores - Segunda Pasada):
	private string title_phase2_step6 = "4.2. Segunda Pasada del lijado de interiores";
	private string image_header_phase2Step6 = "Sprites/phase2/Step4_2_Phase2_text";
	private string introduction_text_phase2Step6_path = "Texts/Phase2Step6/0_introduction_text";
	//esto defin el feedback cuando se debe buscar el disco P150
	private string text_feedback_phase2step6 = "Texts/Phase2Step6/6_feedback_text_phase2step6";
	//esto define el feeback cuando se debe buscar el disco P180:
	private string text_feedback_phase2step6_p180 = "Texts/Phase2Step6/6_feedback_text_phase2step6_p180";

	//variables para configurar el marcador 33 (Disco P150):
	private string image_marker33_p = "Sprites/markers/frameMarker33_disco_p150";
	private string image_marker33_real_p = "Sprites/phase2step6/FrameMarker33_disco_p150";
	private string marker33_text_info_btn_two_polv = "Texts/Phase2Step6/7_add_info_btn_text_polvo";
	//variables que definen el marcador 34 (Disco P180):
	private string image_marker34_p = "Sprites/markers/frameMarker34_disco_p180";
	private string image_marker34_real_p = "Sprites/phase2step6/FrameMarker34_disco_p180";

	//variables para configurar las actividades de la FASE2 - Paso 7 (Lijado de interiores - Tercera pasada)
	private string title_phase2_step7 = "4.3. Tercera Pasada del lijado de interiores";
	private string introduction_text_phase2Step7_path = "Texts/Phase2Step7/0_introduction_text";

	//variables para configurar el marcador 36(disco p240):
	private string image_marker36_p = "Sprites/markers/frameMarker36_disco_p240";
	private string image_marker36_real_p = "Sprites/phase2step7/FrameMarker36_disco_p240";
	private string text_feedback_phase2step7 = "Texts/Phase2Step7/6_feedback_text_phase2step7";

	//Variables para configurar las actividades de la FASE2 - Paso 8 (Lijado de Interiores - Pasada Final):
	private string title_phase2_step8 = "4.4. Pasada Final del lijado de interiores";
	private string introduction_text_phase2Step8_path = "Texts/Phase2Step8/0_introduction_text";

	//variables apra configurar el marcador 38 (disco P320):
	private string image_marker38_p = "Sprites/markers/frameMarker38_disco_p320";
	private string image_marker38_real_p = "Sprites/phase2step8/FrameMarker38_disco_p320";
	private string text_feedback_phase2step8 = "Texts/Phase2Step8/6_feedback_text_phase2step8";

	//**********************************************************************************************************************************
	//**********************************************************************************************************************************
	//**********************************************************************************************************************************
	//Variables para el modo EVALUACION:

	//Variables que controlan los nombres de los pasos en el menu de fases (los pasos no tienen numero):
	private string menuPhases_int_eval_button_uno_text = "Limpieza";
	private string menuPhases_int_eval_button_dos_text = "Matizado";
	private string menuPhases_int_eval_button_tres_text = "Masillado";
	private string menuPhases_int_eval_button_cuatro_text = "Aparejado";
	private string menuPhases_int_eval_button_cinco_text = "Pintado";
	private string menuPhases_int_eval_button_seis_text = "Barnizado";

	private string title_ordering_phases = "Organizando las fases del proceso";
	private string menuPhases_eval_introduction_text_path = "Texts/EvalMode/OrganizePhases/2_menuPhases";

	//Variables que controlan los textos para el menu de pasos de la fase 1 en modo de evaluacion:
	private string menuStepsPhase1_int_eval_title = "Pasos de la fase de limpieza";
	private string menuStepsPhase1_introduction_text_path_eval = "Texts/EvalMode/Phase1Steps/1_menuSteps_introduction";
	private string menuStepsPhase1_button_uno_text_eval = "Buscar Capo del Coche";
	private string menuStepsPhase1_button_dos_text_eval = "Limpieza";
	private string menuStepsPhase1_button_tres_text_eval = "Aclarado y Secado";
	private string menuStepsPhase1_button_cuatro_text_eval = "Localizar Irregularidades";
	private string menuStepsPhase1_button_cinco_text_eval = "Corregir Irregularidades";
	private string menuStepsPhase1_button_seis_text_eval = "Desengrasado";

	//variables que controlan los textos para el menu de pasos de la fase 2 en modo de evaluacion:
	private string menuStepsPhase2_int_eval_title = "Pasos de la fase de matizado";
	private string menuStepsPhase2_image_header = "Sprites/images_phases_text/2_matizado_FaseProceso";
	private string menuStepsPhase2_introduction_text_path_eval = "Texts/EvalMode/Phase2Steps/1_menuSteps_introduction";
	private string menuStepsPhase2_button_uno_text_eval = "Introduccion";
	private string menuStepsPhase2_button_dos_text_eval = "Proteccion Superficie";
	private string menuStepsPhase2_button_tres_text_eval = "Cantos Pasada 1";
	private string menuStepsPhase2_button_cuatro_text_eval = "Cantos Pasada 2";
	private string menuStepsPhase2_button_cinco_text_eval = "Interiores Pasada 1";
	private string menuStepsPhase2_button_seis_text_eval = "Interiores Pasada 2";
	private string menuStepsPhase2_button_siete_text_eval = "Interiores Pasada 3";
	private string menuStepsPhase2_button_ocho_text_eval = "Interiores Pasada 4";
	//variables a las imagenes de cada paso de la fase 2 en color pero sin texto embebido:
	private string phaseTwo_img_step_one_no_text = "Sprites/phase2/Step1_Phase2";
	private string phaseTwo_img_step_two_no_text = "Sprites/phase2/Step2_Phase2";
	private string phaseTwo_img_step_three_no_text = "Sprites/phase2/Step3_1_Phase2";
	private string phaseTwo_img_step_four_no_text = "Sprites/phase2/Step3_2_Phase2";
	private string phaseTwo_img_step_five_no_text = "Sprites/phase2/Step4_1_Phase2";
	private string phaseTwo_img_step_six_no_text = "Sprites/phase2/Step4_2_Phase2";
	private string phaseTwo_img_step_seven_no_text = "Sprites/phase2/Step4_3_Phase2";
	private string phaseTwo_img_step_eight_no_text = "Sprites/phase2/Step4_4_Phase2";


	//Variables para configurar el paso 1 de la fase 1 (buscar capo del carro):
	private string question_mark_path = "Sprites/phase1step1_eval/question_mark";
	private string real_image_help_path = "Sprites/phase1step1_eval/capo_coche";
	private string intro_text_phase1Step1_eval_path_one = "Texts/EvalMode/phase1step1/1_Phase1Step1_text";
	private string intro_text_phase1Step1_eval_path_two = "Texts/EvalMode/phase1step1/2_Phase1Step1_text_dos";
	private string feedback_capo_text_path = "Texts/EvalMode/phase1step1/3_feedback_capo_carro";

	//Vairblaes para configurar la interfaz de actividades de FASE1-Paso2 (Limpieza):
	private string title_phase1_step2_eval_mode = "2. Limpieza";
	private string intro_text_phase1Step2_path_eval = "Texts/EvalMode/phase1step2/1_introduction_text";
	private string feedback_marker16_eval_mode = "Texts/EvalMode/phase1step2/7_feedback_marker16";
	private string feedback_marker19_eval_mode = "Texts/EvalMode/phase1step2/8_feedback_marker19";
	private string feedback_marker21_eval_mode = "Texts/EvalMode/phase1step2/9_feedback_marker21";

	//Variables para cnfigurar la interfaz de actividades de FASE1 -STEP3 (Aclarado y Secado):
	private string title_phase1_step3_eval_mode = "3. Aclarado y Secado";
	private string intro_text_phase1Step3_path_eval = "Texts/EvalMode/phase1step3/1_introduction_text";
	private string feedback_marker16_eval_mode_step3 = "Texts/EvalMode/phase1step3/7_feedback_marker16_step3";
	private string img_help_marker24_25_eval_step3 = "Sprites/phase1step3_eval/FrameMarker25_de3430_limpieza";
	private string feedback_marker24_25_eval_mode_step3 = "Texts/EvalMode/phase1step3/8_feedback_marker24_25";

	//Variables para configrar la interfaz de actividadees de FASE1-STEP4 (Localizar Irregularidades):
	private string title_phase1_step4_eval_mode = "4. Localizar Irregularidades";
	private string intro_text_phase1Step4_path_eval = "Texts/EvalMode/phase1step4/1_introduction_text";
	private string feedback_marker45_46_eval_mode_step4 = "Texts/EvalMode/phase1step4/8_feedback_marker46";

	//Variables para configurar la interfaz de actividades de FASE1-STEP5 (Corregir Irregularidades):
	private string title_phase1_step5_eval_mode = "5. Corregir Irregularidades";
	private string intro_text_phase1Step5_path_eval = "Texts/EvalMode/phase1step5/1_introduction_text";
	private string feedback_marker100_eval_mode_step5 = "Texts/EvalMode/phase1step5/8_feedback_marker100";

	//Variables para configurar la interfaz de actividades de FASE1-STEP6 (Desengrasado):
	private string title_phase1_step6_eval_mode = "6. Desengrasado";
	private string intro_text_phase1Step6_path_eval = "Texts/EvalMode/phase1step6/1_introduction_text";
	private string feedback_marker26_eval_mode_step6 = "Texts/EvalMode/phase1step6/8_feedback_marker26";
	private string feedback_marker25_eval_mode_step6 = "Texts/EvalMode/phase1step6/9_feedback_marker25";

	//Variables para configurar el MODO EVALUACION de la FASE 2:
	private string intro_text_phase2Step1_eval_path_one = "Texts/EvalMode/phase2step1/0_Phase1Step1_text";
	private string intro_text_phase2Step1_eval_path_two = "Texts/EvalMode/phase2step1/1_Phase1Step1_second_part";

	//variables para configurar las actividades modo evaluacion de FASE2-STEP2 (Proteccion de superficies)
	private string title_phase2_step2_eval_mode = "2. Proteccion de Superficies";
	private string intro_text_phase2Step2_path_eval = "Texts/EvalMode/phase2step2/1_introduction_text";
	private string feedback_marker65_eval_mode_step2 = "Texts/EvalMode/phase2step2/8_feedback_marker65";
	private string feedback_marker69_eval_mode_step2 = "Texts/EvalMode/phase2step2/9_feedback_marker69";

	//variables para configurar las actividades modo evaluacion FASE2_STEP3 (Lijado de Cantos - Primera pasada)
	private string title_phase2_step3_eval_mode = "3.1 Lijado de Cantos - Primera pasada";
	private string intro_text_phase2Step3_path_eval = "Texts/EvalMode/phase2step3/1_introduction_text";
	private string feedback_marker45_eval_mode_step3 = "Texts/EvalMode/phase2step3/8_feedback_marker45";
	private string feedback_marker25_24_23_eval_mode_step3 = "Texts/EvalMode/phase2step3/9_feedback_marker23_24_25";
	private string img_eval_mode_hint_marker25_24_23 = "Sprites/phase2step3_eval/FrameMarker25_24_23_limpieza";

	//Variables para configurar las actividades del modo evaluacion FASE2-STEP4 (Lijado de cantos - Pasada Final):
	private string title_phase2_step4_eval_mode = "3.2 Lijado de Cantos - Pasada Final";
	private string intro_text_phase2Step4_path_eval = "Texts/EvalMode/phase2step4/1_introduction_text";
	private string feedback_marker46_eval_mode_step4 = "Texts/EvalMode/phase2step4/8_feedback_marker45";
	private string feedback_marker25_24_23_eval_mode_step4 = "Texts/EvalMode/phase2step4/9_feedback_marker23_24_25";
	//variables para configurar las actividades del modo evaluacion FASE2-STEP5 (Lijado Interiores - Primera pasada):
	private string title_phase2_step5_eval_mode = "4.1 Lijado de interiores - Primera pasada";
	private string intro_text_phase2Step5_path_eval = "Texts/EvalMode/phase2step5/1_introduction_text";
	private string img_eval_mode_hint_marker99 = "Sprites/phase2step5_eval/FrameMarker99_roto_orbital_hint";
	private string feedback_marker99_eval_mode_step5 = "Texts/EvalMode/phase2step5/8_feedback_marker99";
	private string feedback_marker30_eval_mode_step5 = "Texts/EvalMode/phase2step5/9_feedback_marker30";
	private string img_eval_mode_hint_marker30 = "Sprites/phase2step5_eval/FrameMarker30_disco_p80_hint";
	private string feedback_marker25_24_23_eval_mode_step6 = "Texts/EvalMode/phase2step5/10_feedback_marker23_24_25";

	//Varaiables que controlan las actividades del modo evaluacion FASE2-STEP6(lIJADO DE INTERIORES - sEGUNDA PASADA):
	private string title_phase2_step6_eval_mode = "4.2 Lijado de interiores - Segunda pasada";
	private string intro_text_phase2Step6_path_eval = "Texts/EvalMode/phase2step6/1_introduction_text";
	private string marker33_32_hint_image = "Sprites/phase2step6_eval/FrameMarker33_disco_p150_hint";
	private string feedback_marker33_34eval_mode_step6 = "Texts/EvalMode/phase2step6/8_feedback_marker32_33";
	private string text_feedback_phase2step6_eval_p180 = "Texts/EvalMode/phase2step6/6_feedback_text_phase2step6_p180";
	private string text_feedback_phase2step6_eval = "Texts/EvalMode/phase2step6/6_feedback_text_phase2step6";

	//variables para configurar las actividades del modo ecVluacion FASE2-STEP7 (LIJADO DE INTERIORES - TERCERA PASADA):
	private string title_phase2_step7_eval_mode = "4.3 Lijado de interiores - Tercera pasada";
	private string intro_text_phase2Step7_path_eval = "Texts/EvalMode/phase2step7/1_introduction_text";
	private string img_eval_mode_hint_marker36 = "Sprites/phase2step7_eval/FrameMarker36_disco_p240_hint";
	private string feedback_marker36_eval_mode_step7 = "Texts/EvalMode/phase2step7/8_feedback_marker36";
	private string feedback_marker24_25_eval_mode_step7 = "Texts/EvalMode/phase2step7/9_feedback_marker23_24_25";

	//Variables para configurar las actividades del modo evaluacion FASE2-STEP8 (LIJADO DE INTERIORES - PASADA FINAL):
	private string title_phase2_step8_eval_mode = "4.4 Lijado de interiores - Pasada Final";
	private string intro_text_phase2Step8_path_eval = "Texts/EvalMode/phase2step8/1_introduction_text";
	private string feedback_marker38_eval_mode_step8 = "Texts/EvalMode/phase2step8/8_feedback_marker38";
	private string feedback_marker25_24_23_EvalMode_step8 = "Texts/EvalMode/phase2step8/9_feedback_marker23_24_25";
	private string img_eval_mode_hint_marker38 = "Sprites/phase2step8_eval/FrameMarker38_disco_p320_hint";

	#endregion PRIVATE_MEMBER_VARIABLES

    //This gets called from SceneManager's Start() 
    public virtual void InitManager()
    {
		//configurando el singleton para referenciar el manager
		if (manager == null) {
			DontDestroyOnLoad (gameObject);
			manager = this;
		} else if (manager != this) {
			Destroy(gameObject);
		}

		Debug.Log ("Width: " + Screen.width);
		Debug.Log ("Height: " + Screen.height);
		Debug.Log ("dpi: " + Screen.dpi);

        mSplashView = new SplashScreenView();
        mAboutView = new AboutScreenView();
        mAboutView.SetTitle(TitleForAboutPage);
        mAboutView.OnStartButtonTapped += OnAboutStartButtonTapped;
        m_UIEventHandler.CloseView += OnTappedOnCloseButton;
        m_UIEventHandler.GoToAboutPage += OnTappedOnGoToAboutPage;
        InputController.SingleTapped += OnSingleTapped;
        InputController.DoubleTapped += OnDoubleTapped;
        InputController.BackButtonTapped += OnBackButtonTapped;

        mSplashView.LoadView();
        StartCoroutine(LoadAboutPageForFirstTime());
        mActiveViewType = ViewType.SPLASHVIEW;

		if (Screen.height <= 480)
			compact_mode = true;
		else
			compact_mode = false;

		//inicializando las variables que controlan el flujo hacia atras de la app:
		goBackFromOtherInterface = false;
		interfaceComingBackFrom = "";
		//inicializando en false la variable que controla el tutorial 1:
		inTutorialPhase1 = false;

		//inicializando variable que controla el tutorial fase 2:
		inTutorialPhase2 = false;

		//inicializando variable para determinar si esta en modo AR:
		in_RA_mode = false;

		//inicializando la variable de modo de evaluacion:
		in_Evaluation_mode = false;

		//inicializando la variable del modo informativo:
		in_informative_mode = false;

		//inicializando la variable que controla los pasos del proceso:
		processOrder = 0;

		//inicializando informacion adicional desplegada en false:
		info_additional_displayed = false;

		//inicializando variable que controla la carga de info desde el marcador:
		informationLoadedFromMarker = false;

		//cargando los datos de informacion adicional que debe mostrar cada marcador:
		//Para el marcador 1 (Capo del carro):
		markerInScene = GameObject.Find ("FrameMarker1");
		controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		controller_info_marker.image_marker_path = image_marker_p;
		controller_info_marker.image_marker_real_path = image_marker_real_p;
		controller_info_marker.text_to_show_blinking = text_to_show_blink;
		controller_info_marker.image_hand_touch_path = image_hand_touch_p;
		controller_info_marker.text_to_show_blinking_touch = text_to_show_blink_touch;
		controller_info_marker.info_add_select_button_enable = true;
		Debug.Log ("******Estado del boton select: " + controller_info_marker.info_add_select_button_enable);
		controller_info_marker.image_for_button_select = image_btn_select;
		Debug.Log ("******Asignando la imagen para el boton select: " + image_btn_select_path);
		controller_info_marker.onClickSelectButton_tut1 += onClickSelectCapoCarroSearch;
		//La siguiente es informacion adicional que se carga para la fase de evaluacion:
		controller_info_marker.image_question_mark_path = question_mark_path;
		controller_info_marker.image_real_help_path = real_image_help_path;
		
		//para el marcador 16 (maquina aigua a pressio):
		markerInScene = GameObject.Find ("FrameMarker16");
		controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		controller_info_marker.image_marker_path = image_marker_tutorial2_p;
		controller_info_marker.image_marker_real_path = image_marker_tutorial2_real_p;
		controller_info_marker.info_add_select_button_enable = true;
		controller_info_marker.info_add_button_one_enable = true;
		//informacion adicional que se debe mostrar para el icono que se ha activado
		controller_info_marker.text_add_info_btn_one = text_add_info_btn_one;
		controller_info_marker.image_for_button_select = image_btn_select_path;
		controller_info_marker.image_for_button_one = image_btn_one_path;
		controller_info_marker.image_question_mark_path = question_mark_path;
		controller_info_marker.image_real_help_path = img_eval_mode_hint_marker16;
		//El metodo que se debe ejecutar aqui en el AppManager se define directamente  sobre la interfaz en
		//el metodo: GoToSearchObjectsTutorialPhase2 porque se requiere asignar directamente sobre la 
		//interfaz por el uso del metodo delegado


		//variables para el marcador 19 (agua y jabon (galleda d'aigua))
		markerInScene = GameObject.Find ("FrameMarker19");
		controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		controller_info_marker.image_marker_path = image_marker_tut2_p;
		controller_info_marker.image_marker_real_path = image_marker_tut2_real_p;
		controller_info_marker.info_add_select_button_enable = true;
		controller_info_marker.info_add_button_one_enable = true;
		//informacion adicional que se debe mostrar para el icono que se ha activado
		controller_info_marker.text_add_info_btn_one = text_add_info_btn_one;
		controller_info_marker.image_for_button_select = image_btn_select_path;
		controller_info_marker.image_for_button_one = image_btn_one_path;
		controller_info_marker.image_question_mark_path = question_mark_path;
		controller_info_marker.image_real_help_path = img_eval_mode_hint_marker19;

		//variables para el marcador 21 (bayeta - baieta rentar):
		markerInScene = GameObject.Find ("FrameMarker21");
		controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		controller_info_marker.image_marker_path = image_marker3_tutorial2_p;
		controller_info_marker.image_marker_real_path = image_marker3_tutorial2_real_p;
		controller_info_marker.info_add_select_button_enable = true;
		controller_info_marker.info_add_button_one_enable = true;
		controller_info_marker.text_add_info_btn_one = text_add_info_btn_one;
		controller_info_marker.image_for_button_select = image_btn_select_path;
		controller_info_marker.image_for_button_one = image_btn_one_path;
		controller_info_marker.image_question_mark_path = question_mark_path;
		controller_info_marker.image_real_help_path = img_eval_mode_hint_marker21;

		markerInScene = GameObject.Find ("FrameMarker24");
		controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		controller_info_marker.image_marker_path = image_marker24_p;
		controller_info_marker.image_marker_real_path = image_marker24_real_p;
		controller_info_marker.info_add_select_button_enable = true;
		controller_info_marker.info_add_button_one_enable = true;
		controller_info_marker.text_add_info_btn_one = marker24_text_add_info_btn_one;
		controller_info_marker.image_for_button_select = image_btn_select_path;
		controller_info_marker.image_for_button_one = image_btn_one_path;
		controller_info_marker.image_question_mark_path = question_mark_path;
		controller_info_marker.image_real_help_path = img_eval_mode_hint_marker24;

		//variables para el marcador 25 (Papel Absorbente Roberlo DC3430):
		markerInScene = GameObject.Find ("FrameMarker25");
		controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		controller_info_marker.image_marker_path = image_marker25_p;
		controller_info_marker.image_marker_real_path = image_marker25_real_p;
		controller_info_marker.info_add_select_button_enable = true;
		controller_info_marker.info_add_button_one_enable = true;
		controller_info_marker.text_add_info_btn_one = marker25_text_add_info_btn_one;
		controller_info_marker.image_for_button_select = image_btn_select_path;
		controller_info_marker.image_for_button_one = image_btn_one_path;
		controller_info_marker.image_question_mark_path = question_mark_path;
		controller_info_marker.image_real_help_path = img_eval_mode_hint_marker25;

		//variables para el marcador 26 (desengrasante DA93) (Fase 1 - Paso 6):
		markerInScene = GameObject.Find ("FrameMarker26");
		controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		controller_info_marker.image_marker_path = image_marker26_p;
		controller_info_marker.image_marker_real_path = image_marker26_real_p;
		controller_info_marker.info_add_select_button_enable = true;
		controller_info_marker.info_add_button_one_enable = true;
		controller_info_marker.info_add_button_two_enable = true;
		controller_info_marker.text_add_info_btn_one = marker26_text_add_info_btn_one;
		controller_info_marker.text_add_info_btn_two = marker26_text_add_info_btn_two;
		controller_info_marker.image_for_button_select = image_btn_select_path;
		controller_info_marker.image_for_button_one = image_btn_one_path;
		controller_info_marker.image_for_button_two = image_button_two_mascara_gas;
		controller_info_marker.image_question_mark_path = question_mark_path;
		controller_info_marker.image_real_help_path = img_eval_mode_hint_marker26;

		//variables para el marcador 30 (Disco abrasivo P80):
		markerInScene = GameObject.Find ("FrameMarker30");
		controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		controller_info_marker.image_marker_path = image_marker30_p;
		controller_info_marker.image_marker_real_path = image_marker30_real_p;
		controller_info_marker.info_add_select_button_enable = true;
		controller_info_marker.info_add_button_one_enable = true;
		controller_info_marker.info_add_button_two_enable = true;
		controller_info_marker.text_add_info_btn_one = marker26_text_add_info_btn_one;
		controller_info_marker.text_add_info_btn_two = marker33_text_info_btn_two_polv;
		controller_info_marker.image_for_button_select = image_btn_select_path;
		controller_info_marker.image_for_button_one = image_btn_one_path;
		controller_info_marker.image_for_button_two = image_button_two_mascara_polv;
		controller_info_marker.image_question_mark_path = question_mark_path; 
		controller_info_marker.image_real_help_path = img_eval_mode_hint_marker30;

		//variables para el marcador 32 (Disco abrasivo P120):
		markerInScene = GameObject.Find ("FrameMarker32");
		controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		controller_info_marker.image_marker_path = image_marker32_p;
		controller_info_marker.image_marker_real_path = image_marker32_real_p;
		controller_info_marker.info_add_select_button_enable = true;
		controller_info_marker.info_add_button_one_enable = true;
		controller_info_marker.info_add_button_two_enable = true;
		controller_info_marker.text_add_info_btn_one = marker26_text_add_info_btn_one;
		controller_info_marker.text_add_info_btn_two = marker33_text_info_btn_two_polv;
		controller_info_marker.image_for_button_select = image_btn_select_path;
		controller_info_marker.image_for_button_one = image_btn_one_path;
		controller_info_marker.image_for_button_two = image_button_two_mascara_polv;
		controller_info_marker.image_question_mark_path = question_mark_path;
		//controller_info_marker.image_real_help_path = hint;

		//variables para el marcador 33 (Disco abrasivo P150):
		markerInScene = GameObject.Find ("FrameMarker33");
		controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		controller_info_marker.image_marker_path = image_marker33_p;
		controller_info_marker.image_marker_real_path = image_marker33_real_p;
		controller_info_marker.info_add_select_button_enable = true;
		controller_info_marker.info_add_button_one_enable = true;
		controller_info_marker.info_add_button_two_enable = true;
		controller_info_marker.text_add_info_btn_one = marker26_text_add_info_btn_one;
		controller_info_marker.text_add_info_btn_two = marker33_text_info_btn_two_polv;
		controller_info_marker.image_for_button_select = image_btn_select_path;
		controller_info_marker.image_for_button_one = image_btn_one_path;
		controller_info_marker.image_for_button_two = image_button_two_mascara_polv;
		controller_info_marker.image_question_mark_path = question_mark_path;

		//variables para el marcador 34 (Disco abrasivo P180):
		markerInScene = GameObject.Find ("FrameMarker34");
		controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		controller_info_marker.image_marker_path = image_marker34_p;
		controller_info_marker.image_marker_real_path = image_marker34_real_p;
		controller_info_marker.info_add_select_button_enable = true;
		controller_info_marker.info_add_button_one_enable = true;
		controller_info_marker.info_add_button_two_enable = true;
		controller_info_marker.text_add_info_btn_one = marker26_text_add_info_btn_one;
		controller_info_marker.text_add_info_btn_two = marker33_text_info_btn_two_polv;
		controller_info_marker.image_for_button_select = image_btn_select_path;
		controller_info_marker.image_for_button_one = image_btn_one_path;
		controller_info_marker.image_for_button_two = image_button_two_mascara_polv;
		controller_info_marker.image_question_mark_path = question_mark_path;


		//variables para el marcador 36 (Disco abrasivo P240):
		markerInScene = GameObject.Find ("FrameMarker36");
		controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		controller_info_marker.image_marker_path = image_marker36_p;
		controller_info_marker.image_marker_real_path = image_marker36_real_p;
		controller_info_marker.info_add_select_button_enable = true;
		controller_info_marker.info_add_button_one_enable = true;
		controller_info_marker.info_add_button_two_enable = true;
		controller_info_marker.text_add_info_btn_one = marker26_text_add_info_btn_one;
		controller_info_marker.text_add_info_btn_two = marker33_text_info_btn_two_polv;
		controller_info_marker.image_for_button_select = image_btn_select_path;
		controller_info_marker.image_for_button_one = image_btn_one_path;
		controller_info_marker.image_for_button_two = image_button_two_mascara_polv;
		controller_info_marker.image_question_mark_path = question_mark_path;
		controller_info_marker.image_real_help_path = img_eval_mode_hint_marker36;

		//variables para el marcador 38 (Disco abrasivo P320):
		markerInScene = GameObject.Find ("FrameMarker38");
		controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		controller_info_marker.image_marker_path = image_marker38_p;
		controller_info_marker.image_marker_real_path = image_marker38_real_p;
		controller_info_marker.info_add_select_button_enable = true;
		controller_info_marker.info_add_button_one_enable = true;
		controller_info_marker.info_add_button_two_enable = true;
		controller_info_marker.text_add_info_btn_one = marker26_text_add_info_btn_one;
		controller_info_marker.text_add_info_btn_two = marker33_text_info_btn_two_polv;
		controller_info_marker.image_for_button_select = image_btn_select_path;
		controller_info_marker.image_for_button_one = image_btn_one_path;
		controller_info_marker.image_for_button_two = image_button_two_mascara_polv;
		controller_info_marker.image_question_mark_path = question_mark_path;
		controller_info_marker.image_real_help_path = img_eval_mode_hint_marker38;

		//variables para el marcador 45 (esponja paper P320):
		markerInScene = GameObject.Find ("FrameMarker45");
		controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		controller_info_marker.image_marker_path = image_marker45_p;
		controller_info_marker.image_marker_real_path = image_marker45_real_p;
		controller_info_marker.info_add_select_button_enable = true;
		controller_info_marker.info_add_button_one_enable = true;
		controller_info_marker.info_add_button_two_enable = true;
		controller_info_marker.text_add_info_btn_one = marker5_text_add_info_btn_one;
		controller_info_marker.text_add_info_btn_two = marker33_text_info_btn_two_polv;
		controller_info_marker.image_for_button_select = image_btn_select_path;
		controller_info_marker.image_for_button_one = image_btn_one_path;
		controller_info_marker.image_for_button_two = image_button_two_mascara_polv;
		controller_info_marker.image_question_mark_path = question_mark_path;
		controller_info_marker.image_real_help_path = img_eval_mode_hint_marker45;

		//variables para el marcador 45 (esponja paper P400):
		markerInScene = GameObject.Find ("FrameMarker46");
		controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		controller_info_marker.image_marker_path = image_marker46_p;
		controller_info_marker.image_marker_real_path = image_marker46_real_p;
		controller_info_marker.info_add_select_button_enable = true;
		controller_info_marker.info_add_button_one_enable = true;
		controller_info_marker.info_add_button_two_enable = true;
		controller_info_marker.text_add_info_btn_one = marker5_text_add_info_btn_one;
		controller_info_marker.text_add_info_btn_two = marker33_text_info_btn_two_polv;
		controller_info_marker.image_for_button_select = image_btn_select_path;
		controller_info_marker.image_for_button_one = image_btn_one_path;
		controller_info_marker.image_for_button_two = image_button_two_mascara_polv;
		controller_info_marker.image_question_mark_path = question_mark_path;
		controller_info_marker.image_real_help_path = img_eval_mode_hint_marker46;

		//variables para el marcador 65 (Cinta de Enmascarar):
		markerInScene = GameObject.Find ("FrameMarker65");
		controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		controller_info_marker.image_marker_path = image_marker65_p;
		controller_info_marker.image_marker_real_path = image_marker65_real_p;
		controller_info_marker.info_add_select_button_enable = true;
		controller_info_marker.info_add_button_one_enable = true;
		controller_info_marker.text_add_info_btn_one = marker65_text_add_info_btn_one;
		controller_info_marker.image_for_button_select = image_btn_select_path;
		controller_info_marker.image_for_button_one = image_btn_one_path;
		controller_info_marker.image_question_mark_path = question_mark_path;
		controller_info_marker.image_real_help_path = img_eval_mode_hint_marker65;

		//variables para el marcador 69 (Cinta de Enmascarar):
		markerInScene = GameObject.Find ("FrameMarker69");
		controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		controller_info_marker.image_marker_path = image_marker69_p;
		controller_info_marker.image_marker_real_path = image_marker69_real_p;
		controller_info_marker.info_add_select_button_enable = true;
		controller_info_marker.info_add_button_one_enable = true;
		controller_info_marker.text_add_info_btn_one = marker69_text_add_info_btn_one;
		controller_info_marker.image_for_button_select = image_btn_select_path;
		controller_info_marker.image_for_button_one = image_btn_one_path;
		controller_info_marker.image_question_mark_path = question_mark_path;
		controller_info_marker.image_real_help_path = img_eval_mode_hint_marker69;

		markerInScene = GameObject.Find ("FrameMarker99");
		controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		controller_info_marker.image_marker_path = image_marker99_p;
		controller_info_marker.image_marker_real_path = image_marker99_real_p;
		controller_info_marker.info_add_select_button_enable = true;
		controller_info_marker.info_add_button_one_enable = true;
		controller_info_marker.info_add_button_two_enable = true;
		controller_info_marker.text_add_info_btn_one = marker5_text_add_info_btn_one;
		controller_info_marker.text_add_info_btn_two = marker5_text_add_info_btn_two;
		controller_info_marker.image_for_button_select = image_btn_select_path;
		controller_info_marker.image_for_button_one = image_btn_one_path;
		controller_info_marker.image_for_button_two = image_button_two_mascara_polv;
		controller_info_marker.image_question_mark_path = question_mark_path;
		controller_info_marker.image_real_help_path = img_eval_mode_hint_marker99;

		//variables para el marcador 6 (martillo):
		markerInScene = GameObject.Find ("FrameMarker100");
		controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		controller_info_marker.image_marker_path = image_marker100_p;
		controller_info_marker.image_marker_real_path = image_marker100_real_p;
		controller_info_marker.info_add_select_button_enable = true;
		controller_info_marker.info_add_button_one_enable = true;
		controller_info_marker.text_add_info_btn_one = marker6_text_add_info_btn_one;
		controller_info_marker.image_for_button_select = image_btn_select_path;
		controller_info_marker.image_for_button_one = image_btn_one_path;
		controller_info_marker.image_question_mark_path = question_mark_path;
		controller_info_marker.image_real_help_path = img_eval_mode_hint_marker100;

		/*
		markerInScene = GameObject.Find ("FrameMarker6");
		controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		controller_info_marker.image_marker_path = image_marker6_p;
		controller_info_marker.image_marker_real_path = image_marker6_real_p;
		controller_info_marker.info_add_select_button_enable = true;
		controller_info_marker.info_add_button_one_enable = true;
		controller_info_marker.text_add_info_btn_one = marker6_text_add_info_btn_one;
		controller_info_marker.image_for_button_select = image_btn_select_path;
		controller_info_marker.image_for_button_one = image_btn_one_path;
		*/

		/*
		markerInScene = GameObject.Find ("FrameMarker7");
		controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		controller_info_marker.image_marker_path = image_marker7_p;
		controller_info_marker.image_marker_real_path = image_marker7_real_p;
		controller_info_marker.info_add_select_button_enable = true;
		controller_info_marker.info_add_button_one_enable = true;
		controller_info_marker.info_add_button_two_enable = true;
		controller_info_marker.text_add_info_btn_one = marker7_text_add_info_btn_one;
		controller_info_marker.text_add_info_btn_two = marker7_text_add_info_btn_two;
		controller_info_marker.image_for_button_select = image_btn_select_path;
		controller_info_marker.image_for_button_one = image_btn_one_path;
		controller_info_marker.image_for_button_two = image_button_two_mascara_gas;
		*/


		//inicializando la variable qque indica si las fases ya se han organizado en el modo de evaluacion:
		eval_mode_phases_organized = false;

		//inicializando la variabe que indica si los pasos de la FASE 1 ya se han organizado en el modo evaluacion:
		eval_mode_phase1_steps_organized = false;
		//inicializando variable que indica si los pasos de la FASE 2 ya se han organizado en el modo evaluacion
		eval_mode_phase2_steps_organized = false;

		//inicializando las variables que controlan el modo guiado y la disponibilidad del modo evaluacion:
		evaluation_mode_enabled = true;
		
		phase_one_enable = true; //la fase uno esta habilitada por defecto debido a que es por donde se comienza
		phase_two_enable = false;
		phase_three_enable = false;
		phase_four_enable = false;
		phase_five_enable = false;
		phase_six_enable = false;

		//inicializando las variables que controlan la activacion de las fases en el modo evaluacion:
		phase_one_enable_eval_mode = true;
		phase_two_enable_eval_mode = false;
		phase_three_enable_eval_mode = false;
		phase_four_enable_eval_mode = false;
		phase_five_enable_eval_mode = false;
		phase_six_enable_eval_mode = false;
		
		//inicializando el vector que controla si los pasos de la fase 1 ya se han completado:
		Debug.Log ("AppManager: Se va a crear el vector de StepsOfProcess FASE 1 e inicializando...");
		steps_phase_one_completed = new StepOfProcess[6];
		for (int i = 0; i<steps_phase_one_completed.Length; i++) {
			steps_phase_one_completed[i] = new StepOfProcess(false,false,false,true,true); //se inicializa un vector con todas las actividades en false menos la ultima: tomar fotos (porque hay algunos pasos que no tienen esta actividad)
		}

		//debido a que hay una actividad de tomar fotos en la fase 1 en el paso 6 (desengrasado) entonces se inicializa en false la actividad take_photo:
		steps_phase_one_completed [5].take_photo_ficha_tecnica = false;
		steps_phase_one_completed [5].take_photo_ficha_seguridad = false;


		//inicializando el vector que controla si los pasos de la fase ya se han completado:
		Debug.Log ("AppManager: Se va a crear el vector de StepsOfProcess FASE 2 e inicializando...");
		steps_phase_two_completed = new StepOfProcess[8];
		for (int j = 0; j<steps_phase_two_completed.Length; j++) {
			steps_phase_two_completed[j] = new StepOfProcess(false,false,false,true,true); //se inicializa un vector con todas las actividades en false menos la ultima: tomar fotos (porque hay algunos pasos que no tienen esta actividad)
		}

		//inicializando el vector que controla si los pasos de la FASE 1 ya se han completado
		//por defecto todos se inicializan en false:
		steps_p_one_eval_completed = new StepOfProcessEvalMode[6];
		for (int i = 0; i<steps_p_one_eval_completed.Length; i++) {
			steps_p_one_eval_completed[i] = new StepOfProcessEvalMode(false,false); //se inicializa un vector con todas las actividades en false menos la ultima: tomar fotos (porque hay algunos pasos que no tienen esta actividad)
		}

		//inicializando el vector que controla si los pasos de la FASE 2 ya se han completado
		//por defecto todos se inicializan en false:
		steps_p_two_eval_completed = new StepOfProcessEvalMode[8];
		for (int i = 0; i<steps_p_two_eval_completed.Length; i++) {
			steps_p_two_eval_completed[i] = new StepOfProcessEvalMode(false,false); //se inicializa un vector con todas las actividades en false menos la ultima: tomar fotos (porque hay algunos pasos que no tienen esta actividad)
		}


		//se inicializa la variable que le permite al estudiante regresar de la interfaz de tomar fotos:
		//esta variable es modificda desde el script ControllerPinturaCameraImage
		can_return_from_take_photo = true;

		//Intentando cargar los datos del estudiante desde el archivo de datos local:
		

	} //cierra el InitManager


    public virtual void DeInitManager()
    {
        mSplashView.UnLoadView();
        mAboutView.UnLoadView();
        m_UIEventHandler.CloseView -= OnAboutStartButtonTapped;
        m_UIEventHandler.GoToAboutPage -= OnTappedOnGoToAboutPage;
        InputController.SingleTapped -= OnSingleTapped;
        InputController.DoubleTapped -= OnDoubleTapped;
        InputController.BackButtonTapped -= OnBackButtonTapped;

        m_UIEventHandler.UnBind();
    }

	/// <summary>
	/// Loads the data for student.
	/// </summary>
	public void LoadDataForStudent(){
		Debug.Log ("Llamado al metodo LoadDataForStudent");
		if (File.Exists (Application.persistentDataPath + "/studentData.dat")) {

			Debug.Log ("El archivo de datos del estudiante si existe y se va a cargar");
			Debug.Log ("DATA Path--> " + Application.persistentDataPath);

			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/studentData.dat", FileMode.Open);
			StudentData student_data = (StudentData)bf.Deserialize (file);
			file.Close ();

			//asignando recuperando el modo evaluacion de los datos guardados del estudiante:
			evaluation_mode_enabled = student_data.evaluat_mode_enabled;

			//recuperando los datos de la organizacion de las fases del proceso:
			eval_mode_phases_organized = student_data.evaluat_mode_phases_organized;
			//recuperando el estado de organizacion de los pasos de la FASE 1:
			eval_mode_phase1_steps_organized = student_data.evaluat_mode_phase1_steps_organized;
			//recuperando estado de organizacion de los pasos de la FASE2:
			eval_mode_phase2_steps_organized = student_data.evaluat_mode_phase2_steps_organized;
			//cargando el estado de login:
			this.user_logged = student_data.student_logged;
			//cargando el codigo de estudiante:
			this.codigo_estudiante = student_data.cod_estudiante;

			//cargando el ultimo marcador escaneado por el estudiante en el paso Fase2 - Step4:
			//esto se utiliza para determinar cual es el proximo marcador que debe escanear el estudiante
			//en el paso 5 de la fase 2
			this.last_markerid_scanned = student_data.last_marker_step4;
			//cargando el ultimo marcador escaneado por el estudiante en FASE EVALUACION fase2-step4:
			this.last_marker_id_evalmode = student_data.last_marker_step4_eval_mode;

			//cargando los estados de todas las fases:
			this.phase_one_enable = student_data.phaseOneEnable;
			this.phase_two_enable = student_data.phaseTwoEnable;
			this.phase_three_enable = student_data.phaseThreeEnable;
			this.phase_four_enable = student_data.phaseFourEnable;
			this.phase_five_enable = student_data.phaseFiveEnable; 
			this.phase_six_enable = student_data.phaseSixEnable;

			this.phase_one_enable_eval_mode = student_data.phaseOneEnableEval;
			this.phase_two_enable_eval_mode = student_data.phaseTwoEnableEval;
			this.phase_three_enable_eval_mode = student_data.phaseThreeEnableEval;
			this.phase_four_enable_eval_mode = student_data.phaseFourEnableEval;
			this.phase_five_enable_eval_mode = student_data.phaseFiveEnableEval;
			this.phase_six_enable_eval_mode = student_data.phaseSixEnableEval;


			//cargando los estados de los pasos de la fase 1:
			if(student_data.step_one_phase_one)
				steps_phase_one_completed[0].NotifyLoadingStepCompleted();
			if(student_data.step_two_phase_one)
				steps_phase_one_completed[1].NotifyLoadingStepCompleted();
			if(student_data.step_three_phase_one)
				steps_phase_one_completed[2].NotifyLoadingStepCompleted();
			if(student_data.step_four_phase_one)
				steps_phase_one_completed[3].NotifyLoadingStepCompleted();
			if(student_data.step_five_phase_one)
				steps_phase_one_completed[4].NotifyLoadingStepCompleted();
			if(student_data.step_six_phase_one)
				steps_phase_one_completed[5].NotifyLoadingStepCompleted();

			Debug.Log("Dato Cargado: EvaluationMode=" + evaluation_mode_enabled);
			Debug.Log("Dato Cargado: PhaseOne=" + this.phase_one_enable);
			Debug.Log("Dato Cargado: PhaseTwo=" + this.phase_two_enable);
			Debug.Log("Dato Cargado: PhaseThree=" + this.phase_three_enable);
			Debug.Log("Dato Cargado: PhaseFour=" + this.phase_four_enable);
			Debug.Log("Dato Cargado: PhaseFive=" + this.phase_five_enable);
			Debug.Log("Dato Cargado: PhaseSix=" + this.phase_six_enable);

			Debug.Log("Dato Cargado: StepOne-Phase1=" + steps_phase_one_completed[0].step_completed);
			Debug.Log("Dato Cargado: StepTwo-Phase1=" + steps_phase_one_completed[1].step_completed);
			Debug.Log("Dato Cargado: StepThree-Phase1=" + steps_phase_one_completed[2].step_completed);
			Debug.Log("Dato Cargado: StepFour-Phase1=" + steps_phase_one_completed[3].step_completed);
			Debug.Log("Dato Cargado: StepFive-Phase1=" + steps_phase_one_completed[4].step_completed);
			Debug.Log("Dato Cargado: StepSix-Phase1=" + steps_phase_one_completed[5].step_completed);

			Debug.Log ("CodigoEstudianteCargado: " + this.codigo_estudiante);
			Debug.Log ("Estado de Login Cargado: " + this.user_logged);

			Debug.Log("Dato cargado: Last marker ID (phase2step4): " + this.last_markerid_scanned);

			//Cargando los datos de la FASE 2 MODO GUIADO:
			if(student_data.step_one_phase_two)
				steps_phase_two_completed[0].NotifyLoadingStepCompleted();
			if(student_data.step_two_phase_two)
				steps_phase_two_completed[1].NotifyLoadingStepCompleted();
			if(student_data.step_three_phase_two)
				steps_phase_two_completed[2].NotifyLoadingStepCompleted();
			if(student_data.step_four_phase_two)
				steps_phase_two_completed[3].NotifyLoadingStepCompleted();
			if(student_data.step_five_phase_two)
				steps_phase_two_completed[4].NotifyLoadingStepCompleted();
			if(student_data.step_six_phase_two)
				steps_phase_two_completed[5].NotifyLoadingStepCompleted();
			if(student_data.step_seven_phase_two)
				steps_phase_two_completed[6].NotifyLoadingStepCompleted();
			if(student_data.step_eight_phase_two)
				steps_phase_two_completed[7].NotifyLoadingStepCompleted();

			//Cargando datos del MODO EVALUACION para la fase 1:
			if(student_data.step_one_p1_eval_mode)
				steps_p_one_eval_completed[0].NotifyLoadingStepCompleted();
			if(student_data.step_two_p1_eval_mode)
				steps_p_one_eval_completed[1].NotifyLoadingStepCompleted();
			if(student_data.step_three_p1_eval_mode)
				steps_p_one_eval_completed[2].NotifyLoadingStepCompleted();
			if(student_data.step_four_p1_eval_mode)
				steps_p_one_eval_completed[3].NotifyLoadingStepCompleted();
			if(student_data.step_five_p1_eval_mode)
				steps_p_one_eval_completed[4].NotifyLoadingStepCompleted();
			if(student_data.step_six_p1_eval_mode)
				steps_p_one_eval_completed[5].NotifyLoadingStepCompleted();

			//cargando datos del MODO EVALUACION para la fase 2:
			if(student_data.step_one_p2_eval_mode)
				steps_p_two_eval_completed[0].NotifyLoadingStepCompleted();
			if(student_data.step_two_p2_eval_mode)
				steps_p_two_eval_completed[1].NotifyLoadingStepCompleted();
			if(student_data.step_three_p2_eval_mode)
				steps_p_two_eval_completed[2].NotifyLoadingStepCompleted();
			if(student_data.step_four_p2_eval_mode)
				steps_p_two_eval_completed[3].NotifyLoadingStepCompleted();
			if(student_data.step_five_p2_eval_mode)
				steps_p_two_eval_completed[4].NotifyLoadingStepCompleted();
			if(student_data.step_six_p2_eval_mode)
				steps_p_two_eval_completed[5].NotifyLoadingStepCompleted();
			if(student_data.step_seven_p2_eval_mode)
				steps_p_two_eval_completed[6].NotifyLoadingStepCompleted();
			if(student_data.step_eight_p2_eval_mode)
				steps_p_two_eval_completed[7].NotifyLoadingStepCompleted();


			/*
			ControllerConectionWebApp control_conection = new ControllerConectionWebApp(this.codigo_estudiante);
			control_conection.notifyCodeState += NotifyCodeValidation;
			control_conection.ValidateCodeState(this.codigo_estudiante);
			*/

			//************************************************************************************
			//************************************************************************************
			//************************************************************************************
			//*****ATTENTION:::ONLY FOR TESTING**************************************************
			/*
			this.phase_one_enable = true;
			this.phase_two_enable = true;

			this.phase_two_enable_eval_mode = true;
			//completando para pruebas las actividades de la fase 1 MODO GUIADO
			steps_phase_one_completed[0].NotifyLoadingStepCompleted();
			steps_phase_one_completed[1].NotifyLoadingStepCompleted();
			steps_phase_one_completed[2].NotifyLoadingStepCompleted();
			steps_phase_one_completed[3].NotifyLoadingStepCompleted();
			steps_phase_one_completed[4].NotifyLoadingStepCompleted();
			steps_phase_one_completed[5].NotifyLoadingStepCompleted();
			//completando para pruebas las actividades de la fase 2 MODO GUIADO
			steps_phase_two_completed[0].NotifyLoadingStepCompleted();
			steps_phase_two_completed[1].NotifyLoadingStepCompleted();
			steps_phase_two_completed[2].NotifyLoadingStepCompleted();
			steps_phase_two_completed[3].NotifyLoadingStepCompleted();
			steps_phase_two_completed[4].NotifyLoadingStepCompleted();
			steps_phase_two_completed[5].NotifyLoadingStepCompleted();
			steps_phase_two_completed[6].NotifyLoadingStepCompleted();
			steps_phase_two_completed[7].NotifyLoadingStepCompleted();
			//completando para pruebas las actividades de la fase 1 MODO EVALUACION
			steps_p_one_eval_completed[0].NotifyLoadingStepCompleted();
			steps_p_one_eval_completed[1].NotifyLoadingStepCompleted();
			steps_p_one_eval_completed[2].NotifyLoadingStepCompleted();
			steps_p_one_eval_completed[3].NotifyLoadingStepCompleted();
			steps_p_one_eval_completed[4].NotifyLoadingStepCompleted();
			steps_p_one_eval_completed[5].NotifyLoadingStepCompleted();
			//completando para pruebas las actividades de la fase 2 MODO EVALUACION:
			steps_p_two_eval_completed[0].NotifyLoadingStepCompleted();
			steps_p_two_eval_completed[1].NotifyLoadingStepCompleted();
			steps_p_two_eval_completed[2].NotifyLoadingStepCompleted();
			steps_p_two_eval_completed[3].NotifyLoadingStepCompleted();
			steps_p_two_eval_completed[4].NotifyLoadingStepCompleted();
			steps_p_two_eval_completed[5].NotifyLoadingStepCompleted();
			*/			
			//************************************************************************************
			//*************************************************************************************
			//*************************************************************************************

			ControllerConectionWebApp control_conection = this.ConectionController.GetComponent<ControllerConectionWebApp>();
			control_conection.notifyCodeState += NotifyCodeValidation;
			control_conection.ValidateCodeState(this.codigo_estudiante);


		} else {
			//si el archivo de datos no existe entonces se inicia la interfaz de inicio de sesion para
			//validar el codigo del estudiante:
			LoadLoginUserInterface();
		}
	} //cierra metodo LoadDataForStudent

	/// <summary>
	/// Notifies the code validation.
	/// Metodo delegado que se llama desde el script ControllerConectionWebApp
	/// cuando se valida si el codigo todavia esta activo es decir si al estudiante todavia se le permite el uso de la app
	/// Este metodo controla en caso de que no haya conexion a internet se utiliza el valor almacenado por defecto en el 
	/// archivo de datos de la aplicacion
	/// </summary>
	/// <param name="valid">If set to <c>true</c> valid.</param>
	public void NotifyCodeValidation(string resp){

		if (resp == "false") {
			Debug.Log ("AppManager: Usuario no logueado actualmente y NO autorizado por WEB");
			this.user_logged = false;
			SaveDataForStudent (); //guardando el estado de login del estudiante en este caso FALSE
			LoadLoginUserInterface ();
		} else if (resp == "error") {
			Debug.Log ("AppManager: No hay conexion al servidor");
			if(!this.user_logged){ //si el usuario no esta logueado y no hay conex a internet entonces no se le permite usar la app
				Debug.Log ("AppManager: No hay conexion al servidor y el usuario NO esta logeado");
				LoadLoginUserInterface();
			}
		} else if (resp == "true") {
			Debug.Log ("AppManager: El usuario SI esta autorizado por WEB");
			if(!this.user_logged){
				Debug.Log ("AppManager: El usuario SI esta autorizado por WEB pero NO estaba logueado");
				//this.user_logged = true;
				//SaveDataForStudent (); //guardando el estado de login del estudiante
				LoadLoginUserInterface();
			}
		}

	}//cierra metodo NotifyCodeValidation

	/// <summary>
	/// Saves the data for student.
	/// </summary>
	public void SaveDataForStudent(){
		Debug.Log ("Llamado al metodo SaveDataForStudent!!!");
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create(Application.persistentDataPath + "/studentData.dat");

		//preparando los datos para almacenarlos:

		StudentData s_data = new StudentData ();
		//modo evaluation estado:
		s_data.evaluat_mode_enabled = evaluation_mode_enabled;
		//estado de organizacion de las fases del proceso:
		s_data.evaluat_mode_phases_organized = this.eval_mode_phases_organized;
		//estado de organizacion de los pasos de la FASE 1:
		s_data.evaluat_mode_phase1_steps_organized = this.eval_mode_phase1_steps_organized;
		//guardando estado de organizacion de pasos de FASE 2:
		s_data.evaluat_mode_phase2_steps_organized = this.eval_mode_phase2_steps_organized;
		//guardando el estado de login del estudiante:
		s_data.student_logged = this.user_logged;
		//guardando el codigo del estudiante:
		s_data.cod_estudiante = this.codigo_estudiante;
		//guardando el ultimo marcador que ha escaneado el estudiante en la fase2 - step4:
		s_data.last_marker_step4 = this.last_markerid_scanned;
		//guardando el ultimo marcador que ha escaneado el estudiante en la fase 2- step4:
		s_data.last_marker_step4_eval_mode = this.last_marker_id_evalmode;

		//fases activas MODO GUIADO estado:
		s_data.phaseOneEnable = this.phase_one_enable;
		s_data.phaseTwoEnable = this.phase_two_enable;
		s_data.phaseThreeEnable = this.phase_three_enable;
		s_data.phaseFourEnable = this.phase_four_enable;
		s_data.phaseFiveEnable = this.phase_five_enable;
		s_data.phaseSixEnable = this.phase_six_enable;

		//guardando fases activas MODO EVALUACION:
		s_data.phaseOneEnableEval = this.phase_one_enable_eval_mode;
		s_data.phaseTwoEnableEval = this.phase_two_enable_eval_mode;
		s_data.phaseThreeEnableEval = this.phase_three_enable_eval_mode;
		s_data.phaseFourEnableEval = this.phase_four_enable_eval_mode;
		s_data.phaseFiveEnableEval = this.phase_five_enable_eval_mode;
		s_data.phaseSixEnableEval = this.phase_six_enable_eval_mode;

		//pasos activos de la FASE 1:
		s_data.step_one_phase_one = this.steps_phase_one_completed [0].step_completed;
		s_data.step_two_phase_one = this.steps_phase_one_completed [1].step_completed;
		s_data.step_three_phase_one = this.steps_phase_one_completed [2].step_completed;
		s_data.step_four_phase_one = this.steps_phase_one_completed [3].step_completed;
		s_data.step_five_phase_one = this.steps_phase_one_completed [4].step_completed;
		s_data.step_six_phase_one = this.steps_phase_one_completed [5].step_completed;

		//pasos activos de la FASE 2:
		s_data.step_one_phase_two = this.steps_phase_two_completed [0].step_completed;
		s_data.step_two_phase_two = this.steps_phase_two_completed [1].step_completed;
		s_data.step_three_phase_two = this.steps_phase_two_completed [2].step_completed;
		s_data.step_four_phase_two = this.steps_phase_two_completed [3].step_completed;
		s_data.step_five_phase_two = this.steps_phase_two_completed [4].step_completed;
		s_data.step_six_phase_two = this.steps_phase_two_completed [5].step_completed;
		s_data.step_seven_phase_two = this.steps_phase_two_completed [6].step_completed;
		s_data.step_eight_phase_two = this.steps_phase_two_completed [7].step_completed;

		//guardando los pasos del MODO EVALUACION para la FASE 1:
		s_data.step_one_p1_eval_mode = this.steps_p_one_eval_completed [0].step_completed;
		s_data.step_two_p1_eval_mode = this.steps_p_one_eval_completed [1].step_completed;
		s_data.step_three_p1_eval_mode = this.steps_p_one_eval_completed [2].step_completed;
		s_data.step_four_p1_eval_mode = this.steps_p_one_eval_completed [3].step_completed;
		s_data.step_five_p1_eval_mode = this.steps_p_one_eval_completed [4].step_completed;
		s_data.step_six_p1_eval_mode = this.steps_p_one_eval_completed [5].step_completed;

		//guardando los pasos del MODO EVALUACION para la FASE 2:
		s_data.step_one_p2_eval_mode = this.steps_p_two_eval_completed [0].step_completed;
		s_data.step_two_p2_eval_mode = this.steps_p_two_eval_completed [1].step_completed;
		s_data.step_three_p2_eval_mode = this.steps_p_two_eval_completed [2].step_completed;
		s_data.step_four_p2_eval_mode = this.steps_p_two_eval_completed [3].step_completed;
		s_data.step_five_p2_eval_mode = this.steps_p_two_eval_completed [4].step_completed;
		s_data.step_six_p2_eval_mode = this.steps_p_two_eval_completed [5].step_completed;
		s_data.step_seven_p2_eval_mode = this.steps_p_two_eval_completed [6].step_completed;
		s_data.step_eight_p2_eval_mode = this.steps_p_two_eval_completed [7].step_completed;

		bf.Serialize (file, s_data);
		file.Close ();
		Debug.Log ("Ya se han guardado los datos del estudiante!!");
		Debug.Log ("Se ha guardado el last_marker_id= " + this.last_markerid_scanned);

	} //cierra metodo SaveDataForStudent

	/// <summary>
	/// Loads the login user interface.
	/// This occurs when the user is not logged in the app
	/// </summary>
	public void LoadLoginUserInterface(){
		//Esto lo implemente el 22 de agosto como solucion provisional porque
		//no se podia escribir sobre el inpuText del codigo de usuario
		//debido a que al parecer la interfaz que esta detras obstaculiza:
		if (compact_mode) {
			DestroyImmediate(challenge_interface_compact);
		}
		//fin de la solucion del 22 de agosto

		login_user_interface_instance = Instantiate (LoginUserInterface);
		ControllerLoginUser controller_login = login_user_interface_instance.GetComponent<ControllerLoginUser> ();
		controller_login.notifyUserLogin += NotifyLoginSucceed;

	}// cierra metodo LoadLoginUserInterface

	/// <summary>
	/// Notifies the login succeed.
	/// This method is called through a delegate method from the ControllerLoginUser
	/// </summary>
	/// <param name="logged">If set to <c>true</c> logged.</param>
	public void NotifyLoginSucceed(bool logged){
		if (logged) {

			//Se inicia el proceso de inicializacion del archivo de datos que se almacena en la tablet:

			Debug.Log ("El archivo de datos del estudiante NO existe y se van a definir los datos por defecto:");

			user_logged = true;

			DestroyImmediate (login_user_interface_instance);

			Debug.Log("El codigo de estudiante que se va a registrar es: " + codigo_estudiante);
			
			//asignando valores por defecto porque el archivo no existe:
			//evaluation_mode_enabled = false;
			
			//cambiando valor para pruebas de desarrollo
			evaluation_mode_enabled = true;	 //este valor originalmente debe ser false
			//Asignando valor por defecto para el estado de organizacion de las fases en el modo evaluacion:
			eval_mode_phases_organized = false;
			//Asignando valor por defecto para el estado de organizacion de los pasos de la FASE 1:
			eval_mode_phase1_steps_organized = false;
			//Asignando valor por defecto para el estado de organizacion de los pasos de la FASE 2:
			eval_mode_phase2_steps_organized = false;

			
			//el usuario ahora esta logueado:
			user_logged = true;
			
			//NOTA: El codigo del estudiante se asigna directamente desde ControllerLoginUser!!
			
			//inicializando los valores para las fases MODO GUIADO:
			phase_one_enable = true;
			phase_two_enable = false;
			phase_three_enable = false;
			phase_four_enable = false;
			phase_five_enable = false;
			phase_six_enable = false;

			//inicializando los valores para las fases MODO EVALUACION:
			phase_one_enable_eval_mode = true;
			phase_two_enable_eval_mode = false;
			phase_three_enable_eval_mode = false;
			phase_four_enable_eval_mode = false;
			phase_five_enable_eval_mode = false;
			phase_six_enable_eval_mode = false;
			
			//NOTA IMPORTANTE: Si el archivo de datos no existe, entonces se dejan por defecto los vectores 
			//steps_phase_one_completed[] con la inicializacion en 0 que se hace desde el InitManager()
			
			
			//creando el archivo y guardando los datos por defecto:
			SaveDataForStudent ();
			//metodo que muestra el mensaje de inicio de sesion exitoso
			MobileNativeMessage mensaje_confirm = new MobileNativeMessage("Login Correcto","Has iniciado sesion correctamente","Aceptar");

			//en modo compacto desde aca se redirecciona a la interfaz del challenge porque ha sido previamente eliminada
			//cuando se carga la interfaz del modo login:
			if(compact_mode){
				GoToChallengeInterface ();
			}


		} else
			user_logged = false;
	} //cierra NotifyLoginSucceed


	public virtual void UpdateManager()
    {
        //Does nothing but anyone extending AppManager can run their update calls here
    }

    public virtual void Draw()
    {
        m_UIEventHandler.UpdateView(false);
        switch (mActiveViewType)
        {
            case ViewType.SPLASHVIEW:
                mSplashView.UpdateUI(true);
                break;

            case ViewType.ABOUTVIEW:
                mAboutView.UpdateUI(true);
				
                break;

            case ViewType.UIVIEW:
                m_UIEventHandler.UpdateView(true);
                break;

            case ViewType.ARCAMERAVIEW:
                break;

			case ViewType.TESTING:
				break;

        }
    }

    #region UNITY_MONOBEHAVIOUR_METHODS

    #endregion UNITY_MONOBEHAVIOUR_METHODS

	public void GoToChallengeInterface(){

		if (current_interface == CurrentInterface.SELECTION_OF_MODE)
			Destroy (selectionOfMode_interface_instance);

		if (compact_mode) {
			challenge_interface_compact = Instantiate(challenge_compact);
			CanvasChallengeInterfaceCompact cChallengeCompact = challenge_interface_compact.GetComponent<CanvasChallengeInterfaceCompact>();
			cChallengeCompact.titulo= this.challenge_interface_title;
			cChallengeCompact.introduction_text_path = this.challenge_interface_introduction_text_path;
			cChallengeCompact.image_header_path = this.challenge_interface_image_header_path;
			cChallengeCompact.continue_button_action += GoToSelectionOfMode;
		} else {
			challenge_interface = Instantiate (challenge);
			CanvasChallengeInterfaceManager cChallengeInterface = challenge_interface.GetComponent<CanvasChallengeInterfaceManager> ();
			cChallengeInterface.titulo = this.challenge_interface_title;
			cChallengeInterface.introduction_text_path = this.challenge_interface_introduction_text_path;
			cChallengeInterface.continue_text_path = this.challenge_interface_text_continue_path;
			cChallengeInterface.image_header_path = this.challenge_interface_image_header_path;
			cChallengeInterface.continue_button_action += GoToSelectionOfMode;
			
		}
		
		//asignando la interfaz actual:
		current_interface = CurrentInterface.CHALLENGE;

				
		LoadDataForStudent ();
		//Debido a que en este punto ya se han cargado los datos del estudiante entonces se puede
		//asignar el codigo de estudiante a la interfaz de control de navegacion
		//NavigationControllerObject.navigation.student_code = this.codigo_estudiante;
		NavigationControllerObject.navigation.student_code = this.codigo_estudiante;
		//obteniendo la fecha en la que se carga la interfaz:
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: I1");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "I1", "0", "-1");

	}//cierra GoToChallengeInterface

	public void GoToSelectionOfMode(){
		Debug.LogError ("AppManager: Llamado al metodo go to selection of mode");

		if (current_interface == CurrentInterface.CHALLENGE) {
			if (compact_mode)
				Destroy (challenge_interface_compact);
			else
				Destroy (challenge_interface);
		} else if (current_interface == CurrentInterface.MENU_PHASES)
			Destroy (menuProcessPhases_interface_instance);
		else if (current_interface == CurrentInterface.MENU_PHASES_EV)
			Destroy (menuProcessPhases_int_eval_instance);
		else if (current_interface == CurrentInterface.CONFIG_OPTIONS)
			Destroy (configuration_opts_interface_instance);

		DestroyInstancesWithTag ("MenuPhasesEvalMode");

		current_interface = CurrentInterface.SELECTION_OF_MODE;

		selectionOfMode_interface_instance = Instantiate (selectionOfMode);
		CanvasSelectionOfModeManager cSelectionModeManager = selectionOfMode_interface_instance.GetComponent<CanvasSelectionOfModeManager>();
		cSelectionModeManager.titulo = selectionMode_interface_title;
		cSelectionModeManager.introduction_text_path = selectionMode_interface_introduction_text_path;
		cSelectionModeManager.guidedMode_action += GoToMenuPhases;
		cSelectionModeManager.evaluationMode_action += GoToMenuPhasesEvaluationMode;
		cSelectionModeManager.informativeMode_action += GoToInformativeMode;
		cSelectionModeManager.configurationOptions_action += LoadConfigurationOptions;
		//definiendo si el boton de evaluacion debe aparecer habilitado o no:
		cSelectionModeManager.evaluation_mode_enab = this.evaluation_mode_enabled;

		//for debugging:
		/*
		TextAsset url_server = Resources.Load<TextAsset> ("Texts/00_server_base_path");
		string url_conexion = "";
		if (url_server != null)
			url_conexion = url_server.text;
		
		Debug.Log ("Click en el Metodo SelfAssessment!!!"); 
		var and_unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		var current_act = and_unity.GetStatic<AndroidJavaObject>("currentActivity");
		Debug.Log("Se ha obtenido current activity...");
		// Accessing the class to call a static method on it
		var save_server = new AndroidJavaClass("edu.udg.bcds.pintura.arapp.SaveDatabaseData");
		//var jc = new AndroidJavaClass("edu.udg.bcds.pintura.tools.SelfAssessment");
		//var video_activity = new AndroidJavaClass("edu.udg.bcds.pintura.arapp.VideoActivity");
		Debug.Log ("Se ha obtenido StartActivity...");
		
		object[] parameters = new object[4]; 
		parameters [0] = current_act; //pasando el argumento de la actividad actual que se debe reproducir
		parameters [1] = "phases"; //definiendo el tipo de secuencia que se va a guardar
		parameters [2] = url_conexion; //definiendo ela URL del servidor:
		parameters [3] = this.codigo_estudiante; //enviando el codigo de estudiante
		Debug.Log ("Se va a llamar el metodo estatico SendSequencesToServer desde Unity");
		// Calling a Call method to which the current activity is passed
		save_server.CallStatic("SendSequencesToServer", parameters);
		*/

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: I2");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "I2", "0", "-1");


	} //cierra GoToSelectionOfMode

	/// <summary>
	/// Loads the config options interface. This method is called from the SelectionOfModeInterface
	/// </summary>
	public void LoadConfigurationOptions(){

		if(current_interface == CurrentInterface.SELECTION_OF_MODE)
			Destroy (selectionOfMode_interface_instance);

		current_interface = CurrentInterface.CONFIG_OPTIONS;

		configuration_opts_interface_instance = Instantiate (ConfigurationOptions);
		ControllerConfigurationOptions controller_config_opts = configuration_opts_interface_instance.GetComponent<ControllerConfigurationOptions> ();
		controller_config_opts.intro_text_path_config_opts = "Texts/6_configuration_text_intro";
		controller_config_opts.ActionGoBackSelectionMode += GoToSelectionOfMode;
		controller_config_opts.ActionRestartGuidedMode += RestartGuidedMode;
		controller_config_opts.ActionRestartEvaluationMode += RestartEvaluationMode;


	}// cierra LoadConfigOptions

	/// <summary>
	/// Restarts the guided mode.
	/// This method is called mainly from the ConfigurationInterface from the SelectionOfMode interface
	/// in order to restart the guided mode
	/// </summary>
	public void RestartGuidedMode(){
		
		this.phase_one_enable = true;
		this.phase_two_enable = false;
		this.phase_three_enable = false;
		this.phase_four_enable = false;
		this.phase_five_enable = false;
		this.phase_six_enable = false;
		
		//reiniciando el vector que controla los pasos de la FASE 1:
		steps_phase_one_completed = new StepOfProcess[6];
		for (int i = 0; i<steps_phase_one_completed.Length; i++) {
			steps_phase_one_completed [i] = new StepOfProcess (false, false,false,true,true);
		}
		//OJO Aqui hay que inicializar los pasos especificos donde SI hay actividad de tomar fotos:
		//debido a que en la fase 1 paso 6 hay una actividad de tomar fotos, entonces se reinicializa esa actividad en false:
		steps_phase_one_completed [5].take_photo_ficha_tecnica = false;
		steps_phase_one_completed [5].take_photo_ficha_seguridad = false;


		//reiniciando el vector que controla los pasos de la FASE 2:
		Debug.Log ("AppManager: Re-inicializando StepsOfProcess FASE 2 e inicializando...");
		steps_phase_two_completed = new StepOfProcess[8];
		for (int j = 0; j<steps_phase_two_completed.Length; j++) {
			steps_phase_two_completed[j] = new StepOfProcess(false,false,false,true,true); //se inicializa un vector con todas las actividades en false menos la ultima: tomar fotos (porque hay algunos pasos que no tienen esta actividad)
		}

		//se procede a guardar los datos en el dispositivo:
		SaveDataForStudent ();

		//Se notifica como registro de navegacion que el estudiante ha reiniciado el modo guiado:
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		NavigationControllerObject.navigation.RegistrarReinicioDeModo (this.codigo_estudiante, fecha, "RRMG", "0", "-1", "guiado");

		//Notificando mendiante mensaje que se ha reiniciado el modo guiado:
		MobileNativeMessage mensaje_confirm = new MobileNativeMessage("Modo Guiado reiniciado","El Modo Guiado se ha reiniciado correctamente","Aceptar");

				
	} // cierra metodo RestartGuidedMode


	/// <summary>
	/// Restarts the evaluation mode.
	/// This method is maintly called from the ConfigurationInterface from the SelectionOfMode interface
	/// </summary>
	public void RestartEvaluationMode(){
		
		this.evaluation_mode_enabled = true;
		//estado de organizacion de las fases del proceso:
		this.eval_mode_phases_organized = false;
		//estado de organizacion de los pasos de la FASE 1:
		this.eval_mode_phase1_steps_organized = false;
		//estado de organizacion de los pasos de la FASE 2:
		this.eval_mode_phase2_steps_organized = false;

		//reiniciando los pasos del MODO EVALUACION de la FASE1:
		steps_p_one_eval_completed = new StepOfProcessEvalMode[6];
		for (int i = 0; i<steps_p_one_eval_completed.Length; i++) {
			steps_p_one_eval_completed [i] = new StepOfProcessEvalMode (false, false);
		}

		//reiniciando los pasos del MODO EVALUACION de la FASE2:
		steps_p_two_eval_completed = new StepOfProcessEvalMode[8];
		for (int i = 0; i<steps_p_two_eval_completed.Length; i++) {
			steps_p_two_eval_completed [i] = new StepOfProcessEvalMode (false, false);
		}
		
		//se procede a guardar los datos en el dispositivo:
		SaveDataForStudent ();

		//Se notifica como registro de navegacion que el estudiante ha reiniciado el modo guiado:
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		NavigationControllerObject.navigation.RegistrarReinicioDeModo (this.codigo_estudiante, fecha, "RRME", "0", "-1", "eval");

		//Notificando mendiante mensaje que se ha reiniciado el modo evaluativo:
		MobileNativeMessage mensaje_confirm = new MobileNativeMessage("Modo Evaluativo reiniciado","El Modo Evaluativo se ha reiniciado correctamente","Aceptar");
						
	} // cierra metodo RestartEvaluationMode


	/// <summary>
	/// Goes to informative mode.
	/// This method starts de informative mode
	/// </summary>
	public void GoToInformativeMode(){
		Debug.Log ("Se va a iniciar el MODO INFORMATIVO... en AppManager.GoToInformativeMode");
		in_informative_mode = true;
		current_interface = CurrentInterface.INFORMATIVE_MODE;
		informative_mode = new InformativeMode ();
		informative_mode.GoToMenuPhasesInformativeMode ();

	} // cierra GoToInformativeMode

	public void GoToMenuPhases(){
		Debug.LogError ("Llamado al metodo go to Menu Phases en AppManager");
		//validando que no vamos a iniciar el modo informativo:
		in_informative_mode = false;

		if (current_interface == CurrentInterface.SELECTION_OF_MODE)
			Destroy (selectionOfMode_interface_instance);
		else if (current_interface == CurrentInterface.MENU_STEPS_PHASE1)
			Destroy (menuStepsPhase1_interface_instance);
		else if (current_interface == CurrentInterface.MENU_STEPS_PHASE2)
			Destroy (menuStepsPhaseTwo_interface_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_PRODUCTS_TUTORIAL)
			Destroy (TutorialPhaseTwoSearchProd_interface_instance);

		current_interface = CurrentInterface.MENU_PHASES;

		menuProcessPhases_interface_instance = Instantiate (menuProcessPhases);
		CanvasProcessPhasesManager cProcessPhaseManager = menuProcessPhases_interface_instance.GetComponent<CanvasProcessPhasesManager> ();
		cProcessPhaseManager.titulo = this.menuPhases_interface_title;
		//asignando imagenes a los botones de la interfaz
		cProcessPhaseManager.introduction_text_path = this.menuPhases_interface_introduction_text_path;
		//verificando si se debe mostrar la imagen normal del boton o la imagen en gris dependiendo de si el boton
		//estaria activo o no:
		if (phase_one_enable)
			cProcessPhaseManager.image_uno_limpieza = this.menuPhases_interface_button_uno_image;
		else
			cProcessPhaseManager.image_uno_limpieza = this.menuPhases_int_btn_uno_image_gray;

		if (phase_two_enable)
			cProcessPhaseManager.image_dos_matizado = this.menuPhases_interface_button_dos_image;
		else
			cProcessPhaseManager.image_dos_matizado = this.menuPhases_int_btn_dos_image_gray;

		if (phase_three_enable)
			cProcessPhaseManager.image_tres_masillado = this.menuPhases_interface_button_tres_image;
		else 
			cProcessPhaseManager.image_tres_masillado = this.menuPhases_int_btn_tres_image_gray;

		if (phase_four_enable)
			cProcessPhaseManager.image_cuatro_aparejado = this.menuPhases_interface_button_cuatro_image;
		else
			cProcessPhaseManager.image_cuatro_aparejado = this.menuPhases_int_btn_cuatro_image_gray;

		if (phase_five_enable)
			cProcessPhaseManager.image_cinco_pintado = this.menuPhases_interface_button_cinco_image;
		else
			cProcessPhaseManager.image_cinco_pintado = this.menuPhases_int_btn_cinco_image_gray;

		if (phase_six_enable)
			cProcessPhaseManager.image_seis_barnizado = this.menuPhases_interface_button_seis_image;
		else
			cProcessPhaseManager.image_seis_barnizado = this.menuPhases_int_btn_seis_image_gray;

		//Asignando los valores booleanos que indican si el boton se debe habilitar o no:
		cProcessPhaseManager.phase_one_button_enable = this.phase_one_enable;
		cProcessPhaseManager.phase_two_button_enable = this.phase_two_enable;
		cProcessPhaseManager.phase_three_button_enable = this.phase_three_enable;
		cProcessPhaseManager.phase_four_button_enable = this.phase_four_enable;
		cProcessPhaseManager.phase_five_button_enable = this.phase_five_enable;
		cProcessPhaseManager.phase_six_button_enable = this.phase_six_enable;

		//asignando textos a los botones de la interfaz:
		cProcessPhaseManager.button_uno_text_limpieza = this.menuPhases_interface_button_uno_text;
		cProcessPhaseManager.button_dos_text_matizado = this.menuPhases_interface_button_dos_text;
		cProcessPhaseManager.button_tres_text_masillado = this.menuPhases_interface_button_tres_text;
		cProcessPhaseManager.button_cuatro_text_aparejado = this.menuPhases_interface_button_cuatro_text;
		cProcessPhaseManager.button_cinco_text_pintado = this.menuPhases_interface_button_cinco_text;
		cProcessPhaseManager.button_seis_text_barnizado = this.menuPhases_interface_button_seis_text;

		cProcessPhaseManager.goToMenuStepsOfPhase1_action += GoToMenuStepsPhase1;
		cProcessPhaseManager.goToMenuStepsOfPhase2_action += GoToMenuStepsPhase2;

		cProcessPhaseManager.goBackToSelectionOfMode += GoToSelectionOfMode;

		interfaceComingBackFrom = "MenuPhases";
		goBackFromOtherInterface = true;

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: I3");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "I3", "0", "-1");
		//NavigationControllerObject.navigation.imprimir_lista ();

	} // cierra GoToMenuPhases

	public void GoToMenuStepsPhase1(){
		Debug.LogError ("Llamado al metodo go to Menu steps phase 1");

		if (current_interface == CurrentInterface.MENU_PHASES)
			Destroy (menuProcessPhases_interface_instance);
		else if (current_interface == CurrentInterface.ACTIVITIES_PHASE1_STEP1)
			Destroy (ActivitiesForPhase1Step1_interface_instance);
		else if (current_interface == CurrentInterface.ACTIVITIES_PHASE1_STEP2)
			Destroy (ActivitiesForPhase1Step2_interface_instance);
		else if (current_interface == CurrentInterface.ACTIVITIES_PHASE1_STEP3)
			Destroy (ActivitiesForPhase1Step3_interface_instance);
		else if (current_interface == CurrentInterface.ACTIVITIES_PHASE1_STEP4)
			Destroy (ActivitiesForPhase1Step4_interface_instance);
		else if(current_interface == CurrentInterface.ACTIVITIES_PHASE1_STEP5)
			Destroy (ActivitiesForPhase1Step5_interface_instance);
		else if(current_interface == CurrentInterface.ACTIVITIES_PHASE1_STEP6)
			Destroy (ActivitiesForPhase1Step6_interface_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_CAR_HOOD)
			Destroy (TurorialSearchCapoCarro_interface_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_PRODUCTS_TUTORIAL)
			Destroy (TutorialPhaseTwoSearchProd_interface_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_BAYETA_PRODUCT_TUTORIAL)
			Destroy (TutorialTwoSearchBayeta_interface_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP3)
			Destroy (AR_Mode_Search_interface_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP4)
			Destroy (AR_Mode_Search_interface_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP5)
			Destroy (AR_Mode_Search_interface_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP6)
			Destroy (AR_Mode_Search_interface_instance);



		processOrder = 0;

		current_interface = CurrentInterface.MENU_STEPS_PHASE1;

		//Llamado al metodo para destruir instancias existentes de esta interfaz para evitar duplicidad:
		//esto se hace antes de instanciar la nueva interfaz mas adelante
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhase1");
		DestroyInstancesWithTag ("BlinkingTutorialPhaseOne");

		menuStepsPhase1_interface_instance = Instantiate (menuStepsPhase1);
		//Es importante asignarle un nombre a la interfaz para despues poder destruir todas las instancias 
		//de esa interfaz:
		menuStepsPhase1_interface_instance.name = "InterfaceMenuOfStepsPhase1";
		//Obteniendo referencia al script
		MenuOfStepsPhase1Manager cMenuStepsPhase1Manager = menuStepsPhase1_interface_instance.GetComponent<MenuOfStepsPhase1Manager> ();
		Debug.Log ("El titulo de la interfaz desde el AppManager es: " + menuStepsPhase1_interface_title);
		cMenuStepsPhase1Manager.titulo = menuStepsPhase1_interface_title;
		cMenuStepsPhase1Manager.introduction_text_path = menuStepsPhase1_introduction_text_path;
		cMenuStepsPhase1Manager.image_header_phase1 = menuStepsPhase1_image_header;

		//definiendo cuales botones de los pasos se deben habilitar dependiendo de si el estudiante ya ha completado
		//las actividades anteriores:
		//por defecto el paso 1 se habilita inicialmente y con la imagen normal:
		cMenuStepsPhase1Manager.step_one_enabled = true;
		cMenuStepsPhase1Manager.image_uno_capo_carro = this.menuStepsPhase1_interface_button_uno_image;

		Debug.Log ("Las actividades del paso 2: completado=" + steps_phase_one_completed [1].step_completed);
		Debug.Log ("Las actividades del paso 2: herramientas=" + steps_phase_one_completed [1].activity_tools_and_products);
		Debug.Log ("Las actividades del paso 2: videos=" + steps_phase_one_completed [1].videos_about_process);
		Debug.Log ("Las actividades del paso 2: assessment=" + steps_phase_one_completed [1].selfevaluation);

		Debug.Log ("Las actividades del paso 3: completado=" + steps_phase_one_completed [2].step_completed);
		Debug.Log ("Las actividades del paso 3: herramientas=" + steps_phase_one_completed [2].activity_tools_and_products);
		Debug.Log ("Las actividades del paso 3: videos=" + steps_phase_one_completed [2].videos_about_process);
		Debug.Log ("Las actividades del paso 3: assessment=" + steps_phase_one_completed [2].selfevaluation);

		Debug.Log ("Las actividades del paso 4: completado=" + steps_phase_one_completed [3].step_completed);
		Debug.Log ("Las actividades del paso 4: herramientas=" + steps_phase_one_completed [3].activity_tools_and_products);
		Debug.Log ("Las actividades del paso 4: videos=" + steps_phase_one_completed [3].videos_about_process);
		Debug.Log ("Las actividades del paso 4: assessment=" + steps_phase_one_completed [3].selfevaluation);

		Debug.Log ("Las actividades del paso 5: completado=" + steps_phase_one_completed [4].step_completed);
		Debug.Log ("Las actividades del paso 5: herramientas=" + steps_phase_one_completed [4].activity_tools_and_products);
		Debug.Log ("Las actividades del paso 5: videos=" + steps_phase_one_completed [4].videos_about_process);
		Debug.Log ("Las actividades del paso 5: assessment=" + steps_phase_one_completed [4].selfevaluation);

		Debug.Log ("Las actividades del paso 6: completado=" + steps_phase_one_completed [5].step_completed);
		Debug.Log ("Las actividades del paso 6: herramientas=" + steps_phase_one_completed [5].activity_tools_and_products);
		Debug.Log ("Las actividades del paso 6: videos=" + steps_phase_one_completed [5].videos_about_process);
		Debug.Log ("Las actividades del paso 6: assessment=" + steps_phase_one_completed [5].selfevaluation);
		Debug.Log ("Las actividades del paso 6: FichaTecnica:" + steps_phase_one_completed [5].take_photo_ficha_tecnica);
		Debug.Log ("Las actividades del paso 6: FichaSeguridad:" + steps_phase_one_completed [5].take_photo_ficha_seguridad);


		//habilitando el paso 2:
		if (steps_phase_one_completed [0].step_completed) {
			cMenuStepsPhase1Manager.step_two_enabled = true;
			cMenuStepsPhase1Manager.image_dos_limpieza = this.menuStepsPhase1_interface_button_dos_image;
		} else {
			cMenuStepsPhase1Manager.step_two_enabled = false;
			cMenuStepsPhase1Manager.image_dos_limpieza = this.menuStepsPhase1_int_btn_dos_image_gray;
		}

		//habilitando el paso 3:
		if (steps_phase_one_completed [1].step_completed) {
			cMenuStepsPhase1Manager.step_three_enabled = true;
			cMenuStepsPhase1Manager.image_tres_secado = this.menuStepsPhase1_interface_button_tres_image;

		} else {
			cMenuStepsPhase1Manager.step_three_enabled = false;
			cMenuStepsPhase1Manager.image_tres_secado = this.menuStepsPhase1_int_btn_tres_image_gray;
		}
		//habilitando el paso 4:
		if (steps_phase_one_completed [2].step_completed) {
			cMenuStepsPhase1Manager.step_four_enabled = true;
			cMenuStepsPhase1Manager.image_cuatro_irregularidades = this.menuStepsPhase1_interface_button_cuatro_image;
		} else {
			cMenuStepsPhase1Manager.step_four_enabled = false;
			cMenuStepsPhase1Manager.image_cuatro_irregularidades = this.menuStepsPhase1_int_btn_cuatro_image_gray;
		}
		//habilitando el paso 5:
		if (steps_phase_one_completed [3].step_completed) {
			cMenuStepsPhase1Manager.step_five_enabled = true;
			cMenuStepsPhase1Manager.image_cinco_corregir = this.menuStepsPhase1_interface_button_cinco_image;
		} else {
			cMenuStepsPhase1Manager.step_five_enabled = false;
			cMenuStepsPhase1Manager.image_cinco_corregir = this.menuStepsPhase1_int_btn_cinco_image_gray;
		}
		//habilitando el paso 6:
		if (steps_phase_one_completed [4].step_completed) {
			cMenuStepsPhase1Manager.step_six_enabled = true;
			cMenuStepsPhase1Manager.image_seis_desengrasar = this.menuStepsPhase1_interface_button_seis_image;
		} else {
			cMenuStepsPhase1Manager.step_six_enabled = false;
			cMenuStepsPhase1Manager.image_seis_desengrasar = this.menuStepsPhase1_int_btn_seis_image_gray;
		}
		//Habilitnado siguiente FASE 2:
		//si el paso 6 (ultimo paso de esta fase) ya se ha completado entonces se notifica para habilitar la siguiente fase del proceso:
		if (steps_phase_one_completed [5].step_completed)
			phase_two_enable = true;

		//Asignando textos
		cMenuStepsPhase1Manager.button_uno_text_capo_carro = this.menuStepsPhase1_button_uno_text;
		cMenuStepsPhase1Manager.button_dos_text_limpieza = this.menuStepsPhase1_button_dos_text;
		cMenuStepsPhase1Manager.button_tres_text_secado = this.menuStepsPhase1_button_tres_text;
		cMenuStepsPhase1Manager.button_cuatro_text_irregularidades = this.menuStepsPhase1_button_cuatro_text;
		cMenuStepsPhase1Manager.button_cinco_text_corregir = this.menuStepsPhase1_button_cinco_text;
		cMenuStepsPhase1Manager.button_seis_text_desengrasar = this.menuStepsPhase1_button_seis_text;
		//Asignando rutas a la imagenes de las fases para mostrarlos como guia para el estudiante en el encabezado de la interfaz:
		cMenuStepsPhase1Manager.image_phase1_path = phase1_with_text_image_path;
		cMenuStepsPhase1Manager.image_phase2_path = phase2_with_text_image_gray_path;
		cMenuStepsPhase1Manager.image_phase3_path = phase3_with_text_image_gray_path;
		cMenuStepsPhase1Manager.image_phase4_path = phase4_with_text_image_gray_path;
		cMenuStepsPhase1Manager.image_phase5_path = phase5_with_text_image_gray_path;
		cMenuStepsPhase1Manager.image_phase6_path = phase6_with_text_image_gray_path;

		cMenuStepsPhase1Manager.goBackToMenuPhases += GoToMenuPhases;
		cMenuStepsPhase1Manager.goToActivitiesPhase1Step1 += GoToActivitiesPhase1Step1;
		cMenuStepsPhase1Manager.goToActivitiesPhase1Step2 += GoToActivitiesPhase1Step2;
		cMenuStepsPhase1Manager.goToActivitiesPhase1Step3 += GoToActivitiesPhase1Step3;
		cMenuStepsPhase1Manager.goToActivitiesPhase1Step4 += GoToActivitiesPhase1Step4;
		cMenuStepsPhase1Manager.goToActivitiesPhase1Step5 += GoToActivitiesPhase1Step5;
		cMenuStepsPhase1Manager.goToActivitiesPhase1Step6 += GoToActivitiesPhase1Step6;

		//cMenuStepsPhase1Manager.LoadInformationIntoInterface ();

		goBackFromOtherInterface = true;
		interfaceComingBackFrom = "Phase1";

		//registrando la navegacion de la interfaz:
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: I6");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "I6", "0", "-1");

		//Guardando los datos porque cuando se llama a esta interfaz en la mayoria de los casos es porque ya se ha completado
		//un paso del proceso
		SaveDataForStudent ();
	}

	//Metodo que instancia la interfaz del Phase1 - Step 1 (buscar capo del carro)
	public void GoToActivitiesPhase1Step1(){

		if (current_interface == CurrentInterface.MENU_STEPS_PHASE1)
			Destroy (menuStepsPhase1_interface_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_CAR_HOOD)
			Destroy (TurorialSearchCapoCarro_interface_instance);

		current_interface = CurrentInterface.ACTIVITIES_PHASE1_STEP1;
		//Ojo con lo siguiente estoy destruyendo todas las instancias que hayan de la interfaz de actividades por cada paso
		//DestroyInstancesWithTag ("ActivitiesForEachStep");

		if (BuscarCapoCarro == null)
			Debug.LogError ("BuscarCapoCarro no esta definido en el AppManager!!");

		//incrementando el numero del proceso a 1 poque comenzamos el paso 1:
		processOrder = 1;

		ActivitiesForPhase1Step1_interface_instance = Instantiate (BuscarCapoCarro);
		CanvasBuscarCapoCocheManager cBuscarCapoCoche = ActivitiesForPhase1Step1_interface_instance.GetComponent<CanvasBuscarCapoCocheManager> ();
		cBuscarCapoCoche.image_header_buscar_capo = image_buscar_capo_path;
		cBuscarCapoCoche.image_content_capo_carro_marker = image_content_marker;
		cBuscarCapoCoche.titulo_buscar_capo_carro = title_phase1_step1;
		cBuscarCapoCoche.introduction_text_path_1 = introduction_text_phase1Step1_path_one;
		cBuscarCapoCoche.introduction_text_path_2 = introduction_text_phase1Step1_path_two;
		cBuscarCapoCoche.text_btn_continuar = "Buscar"; //Aca se define el texto del boton continuar que aparece en la parte inferior de la interfaz
		cBuscarCapoCoche.goBackToMenuActivities += GoToMenuStepsPhase1;
		cBuscarCapoCoche.goToSearchCapoCarro += GoToSearchCapoCoche;

		//registrando la navegacion de la interfaz:
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: I8");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "I8", "0", "-1");
	}

	/// <summary>
	/// Goes to activities phase1 step2.
	/// </summary>
	public void GoToActivitiesPhase1Step2(){

		if (current_interface == CurrentInterface.MENU_STEPS_PHASE1)
			Destroy (menuStepsPhase1_interface_instance);
		else if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS)
			Destroy (ToolsAndProductsPhase1Step2_interface_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_BAYETA_PRODUCT_TUTORIAL)
			Destroy (TutorialTwoSearchBayeta_interface_instance);

		current_interface = CurrentInterface.ACTIVITIES_PHASE1_STEP2;

		//Ojo con lo siguiente estoy destruyendo todas las instancias que hayan de la interfaz de actividades por cada paso
		DestroyInstancesWithTag ("ActivitiesForEachStep");
		DestroyInstancesWithTag ("TutorialPhaseTwo");

		ActivitiesForPhase1Step2_interface_instance = Instantiate (ActivitiesForEachStep);
		CanvasActivitiesForEachStepHeaders cActivitiesForStep = ActivitiesForPhase1Step2_interface_instance.GetComponent<CanvasActivitiesForEachStepHeaders> ();
		cActivitiesForStep.titulo_current_step = title_phase1_step2;
		cActivitiesForStep.introduction_text_path = introduction_text_phase1Step2_path;

		//agregando imagenes que se cargan en el header para el conjunto de fases del proceso:
		cActivitiesForStep.image_phase_one_header = phase1_with_text_image_path;
		cActivitiesForStep.image_phase_two_header = phase2_with_text_image_gray_path;
		cActivitiesForStep.image_phase_three_header = phase3_with_text_image_gray_path;
		cActivitiesForStep.image_phase_four_header = phase4_with_text_image_gray_path;
		cActivitiesForStep.image_phase_five_header = phase5_with_text_image_gray_path;
		cActivitiesForStep.image_phase_six_header = phase6_with_text_image_gray_path;

		//agregando imagenes que se cargan en el header para el conjunto de pasos de la fase correspondiente:
		cActivitiesForStep.image_step_one_header = image_phase1_step1_text_gray;
		cActivitiesForStep.image_step_two_header = image_header_phase1Step2; //esta imagen se muestra activa NO en gris porque es el paso correspondiente
		cActivitiesForStep.image_step_three_header = image_phase1_step3_text_gray;
		cActivitiesForStep.image_step_four_header = image_phase1_step4_text_gray;
		cActivitiesForStep.image_step_five_header = image_phase1_step5_text_gray;
		cActivitiesForStep.image_step_six_header = image_phase1_step6_text_gray;

		//asignando imagenes de los botones del contenido que redireccionan a cada actividad disponible:
		cActivitiesForStep.image_uno_tools_and_products = image_uno_tools_and_products;
		cActivitiesForStep.image_dos_videos = image_dos_videos;
		cActivitiesForStep.image_tres_self_assessment = image_tres_self_assessment;
		cActivitiesForStep.image_cuatro_simulations = image_cuatro_fotos_gray;
		cActivitiesForStep.image_cinco_personal_notes = image_cinco_personal_notes_gray;
		cActivitiesForStep.image_seis_frequently_questions = image_seis_frequently_questions_gray;
		cActivitiesForStep.image_siete_ask_your_teacher = image_siete_ask_your_teacher_gray;
		cActivitiesForStep.goToToolsAndProd += GoToToolsAndProducts;
		cActivitiesForStep.interfaceCallingGoToTools = "Phase1Step2";
		cActivitiesForStep.goToMenuSteps += GoBackToMenuOfStepsFromActivities;
		cActivitiesForStep.interfaceGoingBackFrom = "Phase1Step2";
		cActivitiesForStep.goToVideosForStep += GoToVideoOfEachStep;
		cActivitiesForStep.goToSelfAssessm += GoToSelfAssessment;

		//registrando la navegacion de la interfaz:
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: I9");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "I9", "1", "-1");

	}

	/// <summary>
	/// Goes to activities phase1 step3.
	/// </summary>
	public void GoToActivitiesPhase1Step3(){
		
		if (current_interface == CurrentInterface.MENU_STEPS_PHASE1)
			Destroy (menuStepsPhase1_interface_instance);
		else if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS)
			Destroy (ToolsAndProductsPhase1Step3_interface_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP3)
			Destroy (AR_Mode_Search_interface_instance);

		
		current_interface = CurrentInterface.ACTIVITIES_PHASE1_STEP3;

		//Ojo con lo siguiente estoy destruyendo todas las instancias que hayan de la interfaz de actividades por cada paso
		DestroyInstancesWithTag ("ActivitiesForEachStep");
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");

		
		ActivitiesForPhase1Step3_interface_instance = Instantiate (ActivitiesForEachStep);
		CanvasActivitiesForEachStepHeaders cActivitiesForStep = ActivitiesForPhase1Step3_interface_instance.GetComponent<CanvasActivitiesForEachStepHeaders> ();
		cActivitiesForStep.titulo_current_step = title_phase1_step3;
		cActivitiesForStep.introduction_text_path = introduction_text_phase1Step3_path;

		//agregando imagenes que se cargan en el header para el conjunto de fases del proceso:
		cActivitiesForStep.image_phase_one_header = phase1_with_text_image_path;
		cActivitiesForStep.image_phase_two_header = phase2_with_text_image_gray_path;
		cActivitiesForStep.image_phase_three_header = phase3_with_text_image_gray_path;
		cActivitiesForStep.image_phase_four_header = phase4_with_text_image_gray_path;
		cActivitiesForStep.image_phase_five_header = phase5_with_text_image_gray_path;
		cActivitiesForStep.image_phase_six_header = phase6_with_text_image_gray_path;
		
		//agregando imagenes que se cargan en el header para el conjunto de pasos de la fase correspondiente:
		cActivitiesForStep.image_step_one_header = image_phase1_step1_text_gray;
		cActivitiesForStep.image_step_two_header = image_phase1_step2_text_gray; //esta imagen se muestra activa NO en gris porque es el paso correspondiente
		cActivitiesForStep.image_step_three_header = image_header_phase1Step3;
		cActivitiesForStep.image_step_four_header = image_phase1_step4_text_gray;
		cActivitiesForStep.image_step_five_header = image_phase1_step5_text_gray;
		cActivitiesForStep.image_step_six_header = image_phase1_step6_text_gray;

		//Agregando las imagenes a los botones por cada actividad:		
		cActivitiesForStep.image_uno_tools_and_products = image_uno_tools_and_products;
		cActivitiesForStep.image_dos_videos = image_dos_videos;
		cActivitiesForStep.image_tres_self_assessment = image_tres_self_assessment;
		cActivitiesForStep.image_cuatro_simulations = image_cuatro_fotos_gray;
		cActivitiesForStep.image_cinco_personal_notes = image_cinco_personal_notes_gray;
		cActivitiesForStep.image_seis_frequently_questions = image_seis_frequently_questions_gray;
		cActivitiesForStep.image_siete_ask_your_teacher = image_siete_ask_your_teacher_gray;
		cActivitiesForStep.goToToolsAndProd += GoToToolsAndProducts;
		cActivitiesForStep.interfaceCallingGoToTools = "Phase1Step3";
		cActivitiesForStep.goToMenuSteps += GoBackToMenuOfStepsFromActivities;
		cActivitiesForStep.interfaceGoingBackFrom = "Phase1Step3";
		cActivitiesForStep.goToVideosForStep += GoToVideoOfEachStep;
		cActivitiesForStep.goToSelfAssessm += GoToSelfAssessment;

		//registrando la navegacion de la interfaz:
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: I10");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "I10", "2", "-1");
	} //cierra menu of activities phase1step3

	/// <summary>
	/// Goes to activities phase1 step4.
	/// </summary>
	public void GoToActivitiesPhase1Step4(){
		
		if (current_interface == CurrentInterface.MENU_STEPS_PHASE1)
			Destroy (menuStepsPhase1_interface_instance);
		else if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS)
			Destroy (ToolsAndProductsPhase1Step4_interface_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP4)
			Destroy (AR_Mode_Search_interface_instance);
		
		current_interface = CurrentInterface.ACTIVITIES_PHASE1_STEP4;
		
		//Ojo con lo siguiente estoy destruyendo todas las instancias que hayan de la interfaz de actividades por cada paso
		DestroyInstancesWithTag ("ActivitiesForEachStep");
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");
				
		ActivitiesForPhase1Step4_interface_instance = Instantiate (ActivitiesForEachStep);
		CanvasActivitiesForEachStepHeaders cActivitiesForStep = ActivitiesForPhase1Step4_interface_instance.GetComponent<CanvasActivitiesForEachStepHeaders> ();
		cActivitiesForStep.titulo_current_step = title_phase1_step4;
		cActivitiesForStep.introduction_text_path = introduction_text_phase1Step4_path;

		//agregando imagenes que se cargan en el header para el conjunto de fases del proceso:
		cActivitiesForStep.image_phase_one_header = phase1_with_text_image_path;
		cActivitiesForStep.image_phase_two_header = phase2_with_text_image_gray_path;
		cActivitiesForStep.image_phase_three_header = phase3_with_text_image_gray_path;
		cActivitiesForStep.image_phase_four_header = phase4_with_text_image_gray_path;
		cActivitiesForStep.image_phase_five_header = phase5_with_text_image_gray_path;
		cActivitiesForStep.image_phase_six_header = phase6_with_text_image_gray_path;
		
		//agregando imagenes que se cargan en el header para el conjunto de pasos de la fase correspondiente:
		cActivitiesForStep.image_step_one_header = image_phase1_step1_text_gray;
		cActivitiesForStep.image_step_two_header = image_phase1_step2_text_gray; //esta imagen se muestra activa NO en gris porque es el paso correspondiente
		cActivitiesForStep.image_step_three_header = image_phase1_step3_text_gray;
		cActivitiesForStep.image_step_four_header = image_header_phase1Step4;
		cActivitiesForStep.image_step_five_header = image_phase1_step5_text_gray;
		cActivitiesForStep.image_step_six_header = image_phase1_step6_text_gray;

		//Agregando los botones para cada actividad disponible:
		cActivitiesForStep.image_uno_tools_and_products = image_uno_tools_and_products;
		cActivitiesForStep.image_dos_videos = image_dos_videos;
		cActivitiesForStep.image_tres_self_assessment = image_tres_self_assessment;
		cActivitiesForStep.image_cuatro_simulations = image_cuatro_fotos_gray;
		cActivitiesForStep.image_cinco_personal_notes = image_cinco_personal_notes_gray;
		cActivitiesForStep.image_seis_frequently_questions = image_seis_frequently_questions_gray;
		cActivitiesForStep.image_siete_ask_your_teacher = image_siete_ask_your_teacher_gray;
		cActivitiesForStep.goToToolsAndProd += GoToToolsAndProducts;
		cActivitiesForStep.interfaceCallingGoToTools = "Phase1Step4";
		cActivitiesForStep.goToMenuSteps += GoBackToMenuOfStepsFromActivities;
		cActivitiesForStep.interfaceGoingBackFrom = "Phase1Step4";
		cActivitiesForStep.goToVideosForStep += GoToVideoOfEachStep;
		cActivitiesForStep.goToSelfAssessm += GoToSelfAssessment;

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: I11");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "I11", "3", "-1");
	}//cierra GoToActivitiesPhase1Step4


	/// <summary>
	/// Goes to activities phase1 step5.
	/// </summary>
	public void GoToActivitiesPhase1Step5(){
		
		if (current_interface == CurrentInterface.MENU_STEPS_PHASE1)
			Destroy (menuStepsPhase1_interface_instance);
		else if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS)
			Destroy (ToolsAndProductsPhase1Step5_interface_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP5)
			Destroy (AR_Mode_Search_interface_instance);

		
		current_interface = CurrentInterface.ACTIVITIES_PHASE1_STEP5;
		
		//Ojo con lo siguiente estoy destruyendo todas las instancias que hayan de la interfaz de actividades por cada paso
		DestroyInstancesWithTag ("ActivitiesForEachStep");
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");
		
		ActivitiesForPhase1Step5_interface_instance = Instantiate (ActivitiesForEachStep);
		CanvasActivitiesForEachStepHeaders cActivitiesForStep = ActivitiesForPhase1Step5_interface_instance.GetComponent<CanvasActivitiesForEachStepHeaders> ();
		cActivitiesForStep.titulo_current_step = title_phase1_step5;
		cActivitiesForStep.introduction_text_path = introduction_text_phase1Step5_path;

		//agregando imagenes que se cargan en el header para el conjunto de fases del proceso:
		cActivitiesForStep.image_phase_one_header = phase1_with_text_image_path;
		cActivitiesForStep.image_phase_two_header = phase2_with_text_image_gray_path;
		cActivitiesForStep.image_phase_three_header = phase3_with_text_image_gray_path;
		cActivitiesForStep.image_phase_four_header = phase4_with_text_image_gray_path;
		cActivitiesForStep.image_phase_five_header = phase5_with_text_image_gray_path;
		cActivitiesForStep.image_phase_six_header = phase6_with_text_image_gray_path;
		
		//agregando imagenes que se cargan en el header para el conjunto de pasos de la fase correspondiente:
		cActivitiesForStep.image_step_one_header = image_phase1_step1_text_gray;
		cActivitiesForStep.image_step_two_header = image_phase1_step2_text_gray; //esta imagen se muestra activa NO en gris porque es el paso correspondiente
		cActivitiesForStep.image_step_three_header = image_phase1_step3_text_gray;
		cActivitiesForStep.image_step_four_header = image_phase1_step4_text_gray;
		cActivitiesForStep.image_step_five_header = image_header_phase1Step5;
		cActivitiesForStep.image_step_six_header = image_phase1_step6_text_gray;

		//Agregando las imagenes de los botones de cada actividad disponible:
		cActivitiesForStep.image_uno_tools_and_products = image_uno_tools_and_products;
		cActivitiesForStep.image_dos_videos = image_dos_videos;
		cActivitiesForStep.image_tres_self_assessment = image_tres_self_assessment;
		cActivitiesForStep.image_cuatro_simulations = image_cuatro_fotos_gray;
		cActivitiesForStep.image_cinco_personal_notes = image_cinco_personal_notes_gray;
		cActivitiesForStep.image_seis_frequently_questions = image_seis_frequently_questions_gray;
		cActivitiesForStep.image_siete_ask_your_teacher = image_siete_ask_your_teacher_gray;
		cActivitiesForStep.goToToolsAndProd += GoToToolsAndProducts;
		cActivitiesForStep.interfaceCallingGoToTools = "Phase1Step5";
		cActivitiesForStep.goToMenuSteps += GoBackToMenuOfStepsFromActivities;
		cActivitiesForStep.interfaceGoingBackFrom = "Phase1Step5";
		cActivitiesForStep.goToVideosForStep += GoToVideoOfEachStep;
		cActivitiesForStep.goToSelfAssessm += GoToSelfAssessment;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: I12");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "I12", "4", "-1");
	}//cierra GoToActivitiesPhase1Step5


	/// <summary>
	/// Goes to activities phase1 step6.
	/// </summary>
	public void GoToActivitiesPhase1Step6(){
		
		if (current_interface == CurrentInterface.MENU_STEPS_PHASE1)
			Destroy (menuStepsPhase1_interface_instance);
		else if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS)
			Destroy (ToolsAndProductsPhase1Step6_interface_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP6)
			Destroy (AR_Mode_Search_interface_instance);
		else if (current_interface == CurrentInterface.TAKE_PICTURES)
			Destroy (TakePicturesPhase1Step6_interface_instance);

				
		current_interface = CurrentInterface.ACTIVITIES_PHASE1_STEP6;
		
		//Ojo con lo siguiente estoy destruyendo todas las instancias que hayan de la interfaz de actividades por cada paso
		DestroyInstancesWithTag ("ActivitiesForEachStep");
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");
		
		ActivitiesForPhase1Step6_interface_instance = Instantiate (ActivitiesForEachStep);
		CanvasActivitiesForEachStepHeaders cActivitiesForStep = ActivitiesForPhase1Step6_interface_instance.GetComponent<CanvasActivitiesForEachStepHeaders> ();
		cActivitiesForStep.titulo_current_step = title_phase1_step6;
		cActivitiesForStep.introduction_text_path = introduction_text_phase1Step6_path;

		//agregando imagenes que se cargan en el header para el conjunto de fases del proceso:
		cActivitiesForStep.image_phase_one_header = phase1_with_text_image_path;
		cActivitiesForStep.image_phase_two_header = phase2_with_text_image_gray_path;
		cActivitiesForStep.image_phase_three_header = phase3_with_text_image_gray_path;
		cActivitiesForStep.image_phase_four_header = phase4_with_text_image_gray_path;
		cActivitiesForStep.image_phase_five_header = phase5_with_text_image_gray_path;
		cActivitiesForStep.image_phase_six_header = phase6_with_text_image_gray_path;
		
		//agregando imagenes que se cargan en el header para el conjunto de pasos de la fase correspondiente:
		cActivitiesForStep.image_step_one_header = image_phase1_step1_text_gray;
		cActivitiesForStep.image_step_two_header = image_phase1_step2_text_gray; //esta imagen se muestra activa NO en gris porque es el paso correspondiente
		cActivitiesForStep.image_step_three_header = image_phase1_step3_text_gray;
		cActivitiesForStep.image_step_four_header = image_phase1_step4_text_gray;
		cActivitiesForStep.image_step_five_header = image_phase1_step5_text_gray;
		cActivitiesForStep.image_step_six_header = image_header_phase1Step6;

		//agregando las imagenes a los botones 
		cActivitiesForStep.image_uno_tools_and_products = image_uno_tools_and_products;
		cActivitiesForStep.image_dos_videos = image_dos_videos;
		cActivitiesForStep.image_tres_self_assessment = image_tres_self_assessment;
		cActivitiesForStep.image_cuatro_simulations = image_cuatro_fotos;
		cActivitiesForStep.image_cinco_personal_notes = image_cinco_personal_notes_gray;
		cActivitiesForStep.image_seis_frequently_questions = image_seis_frequently_questions_gray;
		cActivitiesForStep.image_siete_ask_your_teacher = image_siete_ask_your_teacher_gray;
		cActivitiesForStep.goToToolsAndProd += GoToToolsAndProducts;
		cActivitiesForStep.interfaceCallingGoToTools = "Phase1Step6";
		cActivitiesForStep.goToMenuSteps += GoBackToMenuOfStepsFromActivities;
		cActivitiesForStep.interfaceGoingBackFrom = "Phase1Step6";
		cActivitiesForStep.goToVideosForStep += GoToVideoOfEachStep;
		cActivitiesForStep.goToSelfAssessm += GoToSelfAssessment;
		cActivitiesForStep.goToTakePictures += GoToTakePictures;
		//registrando navegacion de interfaz
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: I13");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "I13", "5", "-1");
	}//cierra GoToActivitiesPhase1Step6

	/// <summary>
	/// Gos to menu steps phase1.
	/// Method that is called for loading the interface of steps for phase 2 (MATIZADO):
	/// </summary>
	public void GoToMenuStepsPhase2(){
		Debug.LogError ("Llamado al metodo go to Menu steps phase 2");
		
		if (current_interface == CurrentInterface.MENU_PHASES)
			Destroy (menuProcessPhases_interface_instance);
		else if (current_interface == CurrentInterface.MENU_SUB_STEPS_PHASE2)
			Destroy (menuSubStepsPhaseTwo_interface_instance);
		else if (current_interface == CurrentInterface.MENU_SUB_STEPS_INTERIORES_PHASE2)
			Destroy (menuSubStepsPhaseTwoInterior_interf_instance);
		else if (current_interface == CurrentInterface.ACTIVITIES_PHASE2_STEP1)
			Destroy (ActivitiesForPhase2Step1_interface_instance);
		else if (current_interface == CurrentInterface.ACTIVITIES_PHASE2_STEP2)
			Destroy (ActivitiesForPhase2Step2_interface_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP2)
			Destroy (AR_Mode_Search_interface_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP3)
			Destroy (AR_Mode_Search_interface_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP4)
			Destroy (AR_Mode_Search_interface_instance);
		/*
		else if(current_interface == CurrentInterface.ACTIVITIES_PHASE1_STEP5)
			Destroy (ActivitiesForPhase1Step5_interface_instance);
		else if(current_interface == CurrentInterface.ACTIVITIES_PHASE1_STEP6)
			Destroy (ActivitiesForPhase1Step6_interface_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_CAR_HOOD)
			Destroy (TurorialSearchCapoCarro_interface_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_PRODUCTS_TUTORIAL)
			Destroy (TutorialPhaseTwoSearchProd_interface_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_BAYETA_PRODUCT_TUTORIAL)
			Destroy (TutorialTwoSearchBayeta_interface_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP3)
			Destroy (AR_Mode_Search_interface_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP4)
			Destroy (AR_Mode_Search_interface_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP5)
			Destroy (AR_Mode_Search_interface_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP6)
			Destroy (AR_Mode_Search_interface_instance);
		*/
		
		
		processOrder = 0;
		
		current_interface = CurrentInterface.MENU_STEPS_PHASE2;
		
		//Llamado al metodo para destruir instancias existentes de esta interfaz para evitar duplicidad:
		//esto se hace antes de instanciar la nueva interfaz mas adelante
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhase2");
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");
		
		menuStepsPhaseTwo_interface_instance = Instantiate (menuStepsPhase2);
		//Es importante asignarle un nombre a la interfaz para despues poder destruir todas las instancias 
		//de esa interfaz:
		menuStepsPhaseTwo_interface_instance.name = "InterfaceMenuOfStepsPhase2";
		//Obteniendo referencia al script
		MenuOfStepsMatizadoManager cMenuStepsMatizadoManager = menuStepsPhaseTwo_interface_instance.GetComponent<MenuOfStepsMatizadoManager> ();
		Debug.Log ("El titulo de la interfaz desde el AppManager es: " + menuStepsPhaseTwo_interface_title);
		cMenuStepsMatizadoManager.titulo = menuStepsPhaseTwo_interface_title;
		cMenuStepsMatizadoManager.introduction_text_path = menuStepsPhaseTwo_introduction_text_path;
		cMenuStepsMatizadoManager.image_header_phase1 = menuStepsPhaseTwo_image_header;
		
		//definiendo cuales botones de los pasos se deben habilitar dependiendo de si el estudiante ya ha completado
		//las actividades anteriores:
		//por defecto el paso 1 se habilita inicialmente y con la imagen normal:
		cMenuStepsMatizadoManager.step_one_enabled = true;
		cMenuStepsMatizadoManager.image_one_path = this.menuStepsPhaseTwo_interface_button_uno_image;
		
		Debug.Log ("Las actividades del paso 2: completado=" + steps_phase_two_completed [1].step_completed);
		Debug.Log ("Las actividades del paso 2: herramientas=" + steps_phase_two_completed [1].activity_tools_and_products);
		Debug.Log ("Las actividades del paso 2: videos=" + steps_phase_two_completed [1].videos_about_process);
		Debug.Log ("Las actividades del paso 2: assessment=" + steps_phase_two_completed [1].selfevaluation);
		
		Debug.Log ("Las actividades del paso 3: completado=" + steps_phase_two_completed [2].step_completed);
		Debug.Log ("Las actividades del paso 3: herramientas=" + steps_phase_two_completed [2].activity_tools_and_products);
		Debug.Log ("Las actividades del paso 3: videos=" + steps_phase_two_completed [2].videos_about_process);
		Debug.Log ("Las actividades del paso 3: assessment=" + steps_phase_two_completed [2].selfevaluation);
		
		Debug.Log ("Las actividades del paso 4: completado=" + steps_phase_two_completed [3].step_completed);
		Debug.Log ("Las actividades del paso 4: herramientas=" + steps_phase_two_completed [3].activity_tools_and_products);
		Debug.Log ("Las actividades del paso 4: videos=" + steps_phase_two_completed [3].videos_about_process);
		Debug.Log ("Las actividades del paso 4: assessment=" + steps_phase_two_completed [3].selfevaluation);
		
		Debug.Log ("Las actividades del paso 5: completado=" + steps_phase_two_completed [4].step_completed);
		Debug.Log ("Las actividades del paso 5: herramientas=" + steps_phase_two_completed [4].activity_tools_and_products);
		Debug.Log ("Las actividades del paso 5: videos=" + steps_phase_two_completed [4].videos_about_process);
		Debug.Log ("Las actividades del paso 5: assessment=" + steps_phase_two_completed [4].selfevaluation);
		
		Debug.Log ("Las actividades del paso 6: completado=" + steps_phase_two_completed [5].step_completed);
		Debug.Log ("Las actividades del paso 6: herramientas=" + steps_phase_two_completed [5].activity_tools_and_products);
		Debug.Log ("Las actividades del paso 6: videos=" + steps_phase_two_completed [5].videos_about_process);
		Debug.Log ("Las actividades del paso 6: assessment=" + steps_phase_two_completed [5].selfevaluation);
		Debug.Log ("Las actividades del paso 6: FichaTecnica:" + steps_phase_two_completed [5].take_photo_ficha_tecnica);
		Debug.Log ("Las actividades del paso 6: FichaSeguridad:" + steps_phase_two_completed [5].take_photo_ficha_seguridad);

		Debug.Log ("Las actividades del paso 7: completado=" + steps_phase_two_completed [6].step_completed);
		Debug.Log ("Las actividades del paso 7: herramientas=" + steps_phase_two_completed [6].activity_tools_and_products);
		Debug.Log ("Las actividades del paso 7: videos=" + steps_phase_two_completed [6].videos_about_process);
		Debug.Log ("Las actividades del paso 7: assessment=" + steps_phase_two_completed [6].selfevaluation);
		Debug.Log ("Las actividades del paso 7: FichaTecnica:" + steps_phase_two_completed [6].take_photo_ficha_tecnica);
		Debug.Log ("Las actividades del paso 7: FichaSeguridad:" + steps_phase_two_completed [6].take_photo_ficha_seguridad);

		Debug.Log ("Las actividades del paso 8: completado=" + steps_phase_two_completed [7].step_completed);
		Debug.Log ("Las actividades del paso 8: herramientas=" + steps_phase_two_completed [7].activity_tools_and_products);
		Debug.Log ("Las actividades del paso 8: videos=" + steps_phase_two_completed [7].videos_about_process);
		Debug.Log ("Las actividades del paso 8: assessment=" + steps_phase_two_completed [7].selfevaluation);
		Debug.Log ("Las actividades del paso 8: FichaTecnica:" + steps_phase_two_completed [7].take_photo_ficha_tecnica);
		Debug.Log ("Las actividades del paso 8: FichaSeguridad:" + steps_phase_two_completed [7].take_photo_ficha_seguridad);
		

		//Aqui se indica a la interfaz cuales botones deben ser visibles:
		cMenuStepsMatizadoManager.step_one_btn_visible = true;
		cMenuStepsMatizadoManager.step_two_btn_visible = true;
		cMenuStepsMatizadoManager.step_three_btn_visible = true;
		cMenuStepsMatizadoManager.step_four_btn_visible = true;
		cMenuStepsMatizadoManager.step_five_btn_visible = false;
		cMenuStepsMatizadoManager.step_six_btn_visible = false;


		//habilitando el paso 2:
		if (steps_phase_two_completed [0].step_completed) {
			cMenuStepsMatizadoManager.step_two_enabled = true;
			cMenuStepsMatizadoManager.image_two_path = this.menuStepsPhaseTwo_interface_button_dos_image;
		} else {
			cMenuStepsMatizadoManager.step_two_enabled = false;
			cMenuStepsMatizadoManager.image_two_path = this.menuStepsPhaseTwo_int_btn_dos_image_gray;
		}
		
		//habilitando el paso 3: Voy a dejar el paso 3 siempre habilitado porque realmente dentro hay dos subpasos mas
		//if (steps_phase_one_completed [1].step_completed) {
			cMenuStepsMatizadoManager.step_three_enabled = true;
			cMenuStepsMatizadoManager.image_three_path = this.menuStepsPhaseTwo_interface_button_tres_image;
		//} else {
		//	cMenuStepsMatizadoManager.step_three_enabled = false;
		//	cMenuStepsMatizadoManager.image_three_path = this.menuStepsPhaseTwo_int_btn_tres_image_gray;
		//}


		//habilitando el paso 4:
		//if (steps_phase_one_completed [2].step_completed) {
			cMenuStepsMatizadoManager.step_four_enabled = true;
			cMenuStepsMatizadoManager.image_four_path = this.menuStepsPhaseTwo_interface_button_cuatro_image;
		//} else {
		//	cMenuStepsMatizadoManager.step_four_enabled = false;
		//	cMenuStepsMatizadoManager.image_four_path = this.menuStepsPhaseTwo_int_btn_cuatro_image_gray;
		//}

		/*
		//habilitando el paso 5:
		if (steps_phase_one_completed [3].step_completed) {
			cMenuStepsMatizadoManager.step_five_enabled = true;
			cMenuStepsMatizadoManager.image_cinco_corregir = this.menuStepsPhase1_interface_button_cinco_image;
		} else {
			cMenuStepsMatizadoManager.step_five_enabled = false;
			cMenuStepsMatizadoManager.image_cinco_corregir = this.menuStepsPhase1_int_btn_cinco_image_gray;
		}
		//habilitando el paso 6:
		if (steps_phase_one_completed [4].step_completed) {
			cMenuStepsMatizadoManager.step_six_enabled = true;
			cMenuStepsMatizadoManager.image_seis_desengrasar = this.menuStepsPhase1_interface_button_seis_image;
		} else {
			cMenuStepsMatizadoManager.step_six_enabled = false;
			cMenuStepsMatizadoManager.image_seis_desengrasar = this.menuStepsPhase1_int_btn_seis_image_gray;
		}

		//Habilitnado siguiente FASE 2:
		//si el paso  (ultimo paso de esta fase) ya se ha completado entonces se notifica para habilitar la siguiente fase del proceso:
		if (steps_phase_one_completed [5].step_completed)
			phase_two_enable = true;
		*/
		
		//Asignando textos
		cMenuStepsMatizadoManager.button_one_text_string = this.menuStepsPhaseTwo_button_uno_text;
		cMenuStepsMatizadoManager.button_two_text_string = this.menuStepsPhaseTwo_button_dos_text;
		cMenuStepsMatizadoManager.button_three_text_string = this.menuStepsPhaseTwo_button_tres_text;
		cMenuStepsMatizadoManager.button_four_text_string = this.menuStepsPhaseTwo_button_cuatro_text;
		//cMenuStepsMatizadoManager.button_one_text_string = this.menuStepsPhase1_button_cinco_text;
		//cMenuStepsMatizadoManager.button_one_text_string = this.menuStepsPhase1_button_seis_text;
		//Asignando rutas a la imagenes de las fases para mostrarlos como guia para el estudiante en el encabezado de la interfaz:
		cMenuStepsMatizadoManager.image_phase1_path = phase1_with_text_image_gray_path;
		cMenuStepsMatizadoManager.image_phase2_path = phase2_with_text_image_path;
		cMenuStepsMatizadoManager.image_phase3_path = phase3_with_text_image_gray_path;
		cMenuStepsMatizadoManager.image_phase4_path = phase4_with_text_image_gray_path;
		cMenuStepsMatizadoManager.image_phase5_path = phase5_with_text_image_gray_path;
		cMenuStepsMatizadoManager.image_phase6_path = phase6_with_text_image_gray_path;
		
		cMenuStepsMatizadoManager.goBackToMenuPhases += GoToMenuPhases;
		cMenuStepsMatizadoManager.goToActionButtoOne += GoToActivitiesPhase2Step1;
		cMenuStepsMatizadoManager.goToActionButtoTwo += GoToActivitiesPhase2Step2;
		cMenuStepsMatizadoManager.goToActionButtoThree += GoToSubMenuStepsLijadoCantos;
		cMenuStepsMatizadoManager.goToActionButtoFour += GoToSubMenuStepsLijadoInteriores;

		
		//cMenuStepsPhase1Manager.LoadInformationIntoInterface ();
		
		goBackFromOtherInterface = true;
		interfaceComingBackFrom = "Phase2";

		//registrando la navegacion de la interfaz:
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: I7");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "I7", "0", "-1");
		
		//Guardando los datos porque cuando se llama a esta interfaz en la mayoria de los casos es porque ya se ha completado
		//un paso del proceso
		SaveDataForStudent ();

	} //cierra GoToMenuStepsPhase2 (Matizado)

	/// <summary>
	/// Gos to sub menu steps lijado cantos. Metodo que configura la interfaz del sub-menu del lijado de cantos:
	/// </summary>
	public void GoToSubMenuStepsLijadoCantos(){
		Debug.LogError ("Llamado al metodo go to SubMenu LijadoCantos phase 2");
		
		if (current_interface == CurrentInterface.MENU_STEPS_PHASE2)
			Destroy (menuStepsPhaseTwo_interface_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP3)
			Destroy (AR_Mode_Search_interface_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP4)
			Destroy (AR_Mode_Search_interface_instance);
		else if (current_interface == CurrentInterface.ACTIVITIES_PHASE2_STEP3)
			Destroy (ActivitiesForPhase2Step3_interface_instance);
		else if (current_interface == CurrentInterface.ACTIVITIES_PHASE2_STEP4)
			Destroy (ActivitiesForPhase2Step4_interface_instance);

		current_interface = CurrentInterface.MENU_SUB_STEPS_PHASE2;
		
		//Llamado al metodo para destruir instancias existentes de esta interfaz para evitar duplicidad:
		//esto se hace antes de instanciar la nueva interfaz mas adelante
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhase2");
		//DestroyInstancesWithTag ("BlinkingTutorialPhaseOne");
		
		menuSubStepsPhaseTwo_interface_instance = Instantiate (menuSubStepsPhase2);
		//Es importante asignarle un nombre a la interfaz para despues poder destruir todas las instancias 
		//de esa interfaz:
		menuSubStepsPhaseTwo_interface_instance.name = "InterfaceMenuOfSubForStepsPhase2";
		//Obteniendo referencia al script
		MenuOfStepsMatizadoSubMenu cMenuSubStepsMatizado = menuSubStepsPhaseTwo_interface_instance.GetComponent<MenuOfStepsMatizadoSubMenu> ();
		Debug.Log ("El titulo de la interfaz desde el AppManager es: " + menuStepsPhaseTwo_interface_title);
		cMenuSubStepsMatizado.titulo = menuSubStepsPhaseTwo_interface_title;
		//cMenuSubStepsMatizado.introduction_text_path = menuStepsPhaseTwo_introduction_text_path;
		//cMenuSubStepsMatizado.image_header_phase1 = menuStepsPhaseTwo_image_header;
		
		//definiendo cuales botones de los pasos se deben habilitar dependiendo de si el estudiante ya ha completado
		//las actividades anteriores:
		//por defecto el paso 1 se habilita inicialmente y con la imagen normal (este paso realmente corresponde al paso 9):
		//cMenuSubStepsMatizado.step_one_enabled = true; //esto habilita directamente 
		//cMenuSubStepsMatizado.image_one_path = this.menuSubStepsPhaseTwo_int_button_uno_image;

		//Lo siguinte configura el header de la interfaz:

		//Aqui se indica a la interfaz cuales botones del menu deben ser visibles:
		cMenuSubStepsMatizado.step_one_btn_visible = true; //Lijado de cantos - Primera pasada
		cMenuSubStepsMatizado.step_two_btn_visible = true; //Lijado de cantos - Segunda pasada
		cMenuSubStepsMatizado.step_three_btn_visible = false;
		cMenuSubStepsMatizado.step_four_btn_visible = false;
		cMenuSubStepsMatizado.step_five_btn_visible = false;
		cMenuSubStepsMatizado.step_six_btn_visible = false;

		//ahora se habilitan las imagenes de los pasos que se deben mostrar en el header debajo de las imgs de las fases:
		cMenuSubStepsMatizado.sub_phase_one_visible = true;
		cMenuSubStepsMatizado.sub_phase_two_visible = true;
		cMenuSubStepsMatizado.sub_phase_three_visible = true;
		cMenuSubStepsMatizado.sub_phase_four_visible = true;
		cMenuSubStepsMatizado.sub_phase_five_visible = false;
		cMenuSubStepsMatizado.sub_phase_six_visible = false;

		//ahora se indican las rutas a las imagenes que se deben mostrar:
		cMenuSubStepsMatizado.image_subphase_one_path = menuStepsPhaseTwo_int_btn_uno_image_gray; //imagen en gris con texto embebido
		cMenuSubStepsMatizado.image_subphase_two_path = menuStepsPhaseTwo_int_btn_dos_image_gray;
		cMenuSubStepsMatizado.image_subphase_three_path = menuSubStepsPhaseTwo_int_btn_tres_imag; //esta imagen si tiene el texto embebido
		cMenuSubStepsMatizado.image_subphase_four_path = menuStepsPhaseTwo_int_btn_cuatro_image_gray;

		//Ahora se inicia el proceso para verificar si los pasos deben estar activos o no:
		//Aca en este caso debido a que es un submenu se activan dos botones de la interfaz:
		if (steps_phase_two_completed [1].step_completed) { //aca se pregunta si ya se ha completado el paso de proteccion de superficies (paso2)
			cMenuSubStepsMatizado.step_one_enabled = true; //ojo aca SI es el btn one porque es el primer boton del submenu
			cMenuSubStepsMatizado.image_one_path = this.menuSubStepsPhaseTwo_int_button_uno_image;
		} else {
			cMenuSubStepsMatizado.step_one_enabled = false;
			cMenuSubStepsMatizado.image_one_path = this.menuSubStepsPhaseTwo_int_button_uno_image_gray;
		}

		if (steps_phase_two_completed [2].step_completed) { //aca se pregunta si ya se ha completado el paso de primera pasada del lijado de cantos (paso 3)
			cMenuSubStepsMatizado.step_two_enabled = true; //ojo aca SI es el btn one porque es el primer boton del submenu
			cMenuSubStepsMatizado.image_two_path = this.menuSubStepsPhaseTwo_int_button_dos_image;
		} else {
			cMenuSubStepsMatizado.step_two_enabled = false;
			cMenuSubStepsMatizado.image_two_path = this.menuSubStepsPhaseTwo_int_button_dos_image_gray;
		}

		//aca se asignan los textos:
		cMenuSubStepsMatizado.button_one_text_string = this.menuSubStepsPhaseTwo_button_uno_text;
		cMenuSubStepsMatizado.button_two_text_string = this.menuSubStepsPhaseTwo_button_dos_text;

		//asignando las imagenes de las fases que se muestran en el header de la interfaz:
		cMenuSubStepsMatizado.image_phase1_path = phase1_with_text_image_gray_path;
		cMenuSubStepsMatizado.image_phase2_path = phase2_with_text_image_path;
		cMenuSubStepsMatizado.image_phase3_path = phase3_with_text_image_gray_path;
		cMenuSubStepsMatizado.image_phase4_path = phase4_with_text_image_gray_path;
		cMenuSubStepsMatizado.image_phase5_path = phase5_with_text_image_gray_path;
		cMenuSubStepsMatizado.image_phase6_path = phase6_with_text_image_gray_path;

		//Ahora se agregan las acciones de los botones:
		cMenuSubStepsMatizado.goBackToMenuPhases += GoToMenuStepsPhase2;
		cMenuSubStepsMatizado.goToActivitiesPhase1Step1 += GoToActivitiesPhase2Step3;
		cMenuSubStepsMatizado.goToActivitiesPhase1Step2 += GoToActivitiesPhase2Step4;
	

	} //cierra GoToSubMenuStepsLijadoCantos

	/// <summary>
	/// Gos to sub menu steps lijado cantos. Metodo que configura el sub-menu de pasos del lijado de interiores
	/// </summary>
	public void GoToSubMenuStepsLijadoInteriores(){
		Debug.LogError ("Llamado al metodo go to SubMenu LijadoInteriores phase 2");
		
		if (current_interface == CurrentInterface.MENU_STEPS_PHASE2)
			Destroy (menuStepsPhaseTwo_interface_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP5)
			Destroy (AR_Mode_Search_interface_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP6)
			Destroy (AR_Mode_Search_interface_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP7)
			Destroy (AR_Mode_Search_interface_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP8)
			Destroy (AR_Mode_Search_interface_instance);
		else if (current_interface == CurrentInterface.ACTIVITIES_PHASE2_STEP5)
			Destroy (ActivitiesForPhase2Step5_interface_instance);
		else if (current_interface == CurrentInterface.ACTIVITIES_PHASE2_STEP6)
			Destroy (ActivitiesForPhase2Step6_interface_instance);
		else if (current_interface == CurrentInterface.ACTIVITIES_PHASE2_STEP7)
			Destroy (ActivitiesForPhase2Step7_interface_instance);
		else if (current_interface == CurrentInterface.ACTIVITIES_PHASE2_STEP8)
			Destroy (ActivitiesForPhase2Step8_interface_instance);
		
		current_interface = CurrentInterface.MENU_SUB_STEPS_INTERIORES_PHASE2;
		
		//Llamado al metodo para destruir instancias existentes de esta interfaz para evitar duplicidad:
		//esto se hace antes de instanciar la nueva interfaz mas adelante
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhase2");
		//DestroyInstancesWithTag ("BlinkingTutorialPhaseOne");
		
		menuSubStepsPhaseTwoInterior_interf_instance = Instantiate (menuSubStepsPhase2);
		//Es importante asignarle un nombre a la interfaz para despues poder destruir todas las instancias 
		//de esa interfaz:
		menuSubStepsPhaseTwoInterior_interf_instance.name = "InterfaceMenuOfSubForStepsPhase2_Interiores";
		//Obteniendo referencia al script
		MenuOfStepsMatizadoSubMenu cMenuSubStepsMatizado = menuSubStepsPhaseTwoInterior_interf_instance.GetComponent<MenuOfStepsMatizadoSubMenu> ();
		Debug.Log ("El titulo de la interfaz desde el AppManager es: " + menuSubStepsPhaseTwo_interior_int_title);
		cMenuSubStepsMatizado.titulo = menuSubStepsPhaseTwo_interior_int_title;

		//Lo siguinte configura el header de la interfaz:
		
		//Aqui se indica a la interfaz cuales botones del menu deben ser visibles:
		cMenuSubStepsMatizado.step_one_btn_visible = true; //Lijado de cantos - Primera pasada
		cMenuSubStepsMatizado.step_two_btn_visible = true; //Lijado de cantos - Segunda pasada
		cMenuSubStepsMatizado.step_three_btn_visible = true;
		cMenuSubStepsMatizado.step_four_btn_visible = true;
		cMenuSubStepsMatizado.step_five_btn_visible = false;
		cMenuSubStepsMatizado.step_six_btn_visible = false;
		
		//ahora se habilitan las imagenes de los pasos que se deben mostrar en el header debajo de las imgs de las fases:
		cMenuSubStepsMatizado.sub_phase_one_visible = true;
		cMenuSubStepsMatizado.sub_phase_two_visible = true;
		cMenuSubStepsMatizado.sub_phase_three_visible = true;
		cMenuSubStepsMatizado.sub_phase_four_visible = true;
		cMenuSubStepsMatizado.sub_phase_five_visible = false;
		cMenuSubStepsMatizado.sub_phase_six_visible = false;
		
		//ahora se indican las rutas a las imagenes que se deben mostrar:
		cMenuSubStepsMatizado.image_subphase_one_path = menuStepsPhaseTwo_int_btn_uno_image_gray; //imagen en gris con texto embebido
		cMenuSubStepsMatizado.image_subphase_two_path = menuStepsPhaseTwo_int_btn_dos_image_gray;
		cMenuSubStepsMatizado.image_subphase_three_path = menuStepsPhaseTwo_int_btn_tres_image_gray; //esta imagen si tiene el texto embebido
		cMenuSubStepsMatizado.image_subphase_four_path = menuStepsPhaseTwo_int_btn_cuatro_color;
		
		//Ahora se inicia el proceso para verificar si los pasos deben estar activos o no:
		//Aca en este caso debido a que es un submenu se activan dos botones de la interfaz:
		if (steps_phase_two_completed [3].step_completed) { //aca se pregunta si ya se ha completado el paso de proteccion de superficies (paso2)
			cMenuSubStepsMatizado.step_one_enabled = true; //ojo aca SI es el btn one porque es el primer boton del submenu
			cMenuSubStepsMatizado.image_one_path = this.menuSubStepsP2_int_btn_uno_image;
		} else {
			cMenuSubStepsMatizado.step_one_enabled = false;
			cMenuSubStepsMatizado.image_one_path = this.menuSubStepsP2_int_btn_uno_image_gray;
		}
		
		if (steps_phase_two_completed [4].step_completed) { //aca se pregunta si ya se ha completado el paso de primera pasada del lijado de cantos (paso 3)
			cMenuSubStepsMatizado.step_two_enabled = true; //ojo aca SI es el btn one porque es el primer boton del submenu
			cMenuSubStepsMatizado.image_two_path = this.menuSubStepsP2_int_btn_dos_image;
		} else {
			cMenuSubStepsMatizado.step_two_enabled = false;
			cMenuSubStepsMatizado.image_two_path = this.menuSubStepsP2_int_btn_dos_image_gray;
		}

		if (steps_phase_two_completed [5].step_completed) { //aca se pregunta si ya se ha completado el paso de primera pasada del lijado de cantos (paso 3)
			cMenuSubStepsMatizado.step_three_enabled = true; //ojo aca SI es el btn one porque es el primer boton del submenu
			cMenuSubStepsMatizado.image_three_path = this.menuSubStepsP2_int_btn_tres_image;
		} else {
			cMenuSubStepsMatizado.step_three_enabled = false;
			cMenuSubStepsMatizado.image_three_path = this.menuSubStepsP2_int_btn_tres_image_gray;
		}

		if (steps_phase_two_completed [6].step_completed) { //aca se pregunta si ya se ha completado el paso de primera pasada del lijado de cantos (paso 3)
			cMenuSubStepsMatizado.step_four_enabled = true; //ojo aca SI es el btn one porque es el primer boton del submenu
			cMenuSubStepsMatizado.image_four_path = this.menuSubStepsP2_int_btn_cuatro_image;
		} else {
			cMenuSubStepsMatizado.step_four_enabled = false;
			cMenuSubStepsMatizado.image_four_path = this.menuSubStepsP2_int_btn_cuatro_image_gray;
		}
		
		//aca se asignan los textos:
		cMenuSubStepsMatizado.button_one_text_string = this.menuSubStepsInteriores_btn_uno_text;
		cMenuSubStepsMatizado.button_two_text_string = this.menuSubStepsInteriores_btn_dos_text;
		cMenuSubStepsMatizado.button_three_text_string = this.menuSubStepsInteriores_btn_tres_text;
		cMenuSubStepsMatizado.button_four_text_string = this.menuSubStepsInteriores_btn_cuatro_text;
		
		//asignando las imagenes de las fases que se muestran en el header de la interfaz:
		cMenuSubStepsMatizado.image_phase1_path = phase1_with_text_image_gray_path;
		cMenuSubStepsMatizado.image_phase2_path = phase2_with_text_image_path;
		cMenuSubStepsMatizado.image_phase3_path = phase3_with_text_image_gray_path;
		cMenuSubStepsMatizado.image_phase4_path = phase4_with_text_image_gray_path;
		cMenuSubStepsMatizado.image_phase5_path = phase5_with_text_image_gray_path;
		cMenuSubStepsMatizado.image_phase6_path = phase6_with_text_image_gray_path;
		
		//Ahora se agregan las acciones de los botones:
		cMenuSubStepsMatizado.goBackToMenuPhases += GoToMenuStepsPhase2;
		cMenuSubStepsMatizado.goToActivitiesPhase1Step1 += GoToActivitiesPhase2Step5;
		cMenuSubStepsMatizado.goToActivitiesPhase1Step2 += GoToActivitiesPhase2Step6;
		cMenuSubStepsMatizado.goToActivitiesPhase1Step3 += GoToActivitiesPhase2Step7;
		cMenuSubStepsMatizado.goToActivitiesPhase1Step4 += GoToActivitiesPhase2Step8;
				
	} //cierra GoToSubMenuStepsLijadoInteriores


	//Metodo que instancia la interfaz del Phase2 - Step 1 (Introduccion al Matizado)
	public void GoToActivitiesPhase2Step1(){
		
		if (current_interface == CurrentInterface.MENU_STEPS_PHASE2)
			Destroy (menuStepsPhaseTwo_interface_instance);

		
		current_interface = CurrentInterface.ACTIVITIES_PHASE2_STEP1;
		//Ojo con lo siguiente estoy destruyendo todas las instancias que hayan de la interfaz de actividades por cada paso
		//DestroyInstancesWithTag ("ActivitiesForEachStep");
		
		if (BuscarCapoCarro == null)
			Debug.LogError ("GoToActivitiesPhase2Step1: BuscarCapoCarro no esta definido para ser usado como interfaz de introduccion al matizado en el AppManager!!");
		
		//incrementando el numero del proceso a 1 poque comenzamos el paso 1:
		processOrder = 1;
		
		ActivitiesForPhase2Step1_interface_instance = Instantiate (BuscarCapoCarro);
		CanvasBuscarCapoCocheManager cBuscarCapoCoche = ActivitiesForPhase2Step1_interface_instance.GetComponent<CanvasBuscarCapoCocheManager> ();
		cBuscarCapoCoche.image_header_buscar_capo = image_intro_matizado_header_path;
		cBuscarCapoCoche.image_content_capo_carro_marker = image_intro_matizado_header_path;
		cBuscarCapoCoche.titulo_buscar_capo_carro = title_phase2_step1;
		cBuscarCapoCoche.introduction_text_path_1 = introduction_text_phase2Step1_path_one;
		cBuscarCapoCoche.introduction_text_path_2 = introduction_text_phase2Step1_path_two;
		cBuscarCapoCoche.text_btn_continuar = "Continuar"; //texto que se mustra en el btn de la parte inferior de la interfaz
		cBuscarCapoCoche.goBackToMenuActivities += GoToMenuStepsPhase2;
		cBuscarCapoCoche.goToSearchCapoCarro += GoToMenuStepsPhase2; //Aunque dice goToSearchCapoCarro realmente este metodo se ejecuta cuando se hace click sobre el boton

		steps_phase_two_completed [0].activity_tools_and_products = true;
		steps_phase_two_completed [0].videos_about_process = true;
		steps_phase_two_completed [0].selfevaluation = true;
		steps_phase_two_completed [0].CheckStepCompletion ();

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: I29");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "I29", "0", "-1");
	} //cierra metodo GoToActivitiesPhase2Step1


	/// <summary>
	/// Goes to activities phase2 step2. (Actividades de Proteccion de la Superficie (Fase Matizado))
	/// </summary>
	public void GoToActivitiesPhase2Step2(){
		
		if (current_interface == CurrentInterface.MENU_STEPS_PHASE2)
			Destroy (menuStepsPhaseTwo_interface_instance);
		else if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS)
			Destroy (ToolsAndProductsPhase2Step2_interface_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP2)
			Destroy (AR_Mode_Search_interface_instance);

		
		current_interface = CurrentInterface.ACTIVITIES_PHASE2_STEP2;
		
		//Ojo con lo siguiente estoy destruyendo todas las instancias que hayan de la interfaz de actividades por cada paso
		DestroyInstancesWithTag ("ActivitiesForEachStep");
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");
		
		ActivitiesForPhase2Step2_interface_instance = Instantiate (ActivitiesForEachStep);
		CanvasActivitiesForEachStepHeaders cActivitiesForStep = ActivitiesForPhase2Step2_interface_instance.GetComponent<CanvasActivitiesForEachStepHeaders> ();
		cActivitiesForStep.titulo_current_step = title_phase2_step2;
		cActivitiesForStep.introduction_text_path = introduction_text_phase2Step2_path;

		//agregando imagenes que se cargan en el header para el conjunto de fases del proceso:
		cActivitiesForStep.image_phase_one_header = phase1_with_text_image_gray_path;
		cActivitiesForStep.image_phase_two_header = phase2_with_text_image_path; //imagen activa que no va en gris
		cActivitiesForStep.image_phase_three_header = phase3_with_text_image_gray_path;
		cActivitiesForStep.image_phase_four_header = phase4_with_text_image_gray_path;
		cActivitiesForStep.image_phase_five_header = phase5_with_text_image_gray_path;
		cActivitiesForStep.image_phase_six_header = phase6_with_text_image_gray_path;
		
		//agregando imagenes que se cargan en el header para el conjunto de pasos de la fase correspondiente:
		cActivitiesForStep.image_step_one_header = image_phase2step1_with_text_gray;
		cActivitiesForStep.image_step_two_header = image_phase2step2_with_text; //esta imagen se muestra activa NO en gris porque es el paso correspondiente
		cActivitiesForStep.image_step_three_header = image_phase2step3_with_text_gray;
		cActivitiesForStep.image_step_four_header = image_phase2step4_with_text_gray;
		cActivitiesForStep.image_step_five_header = image_phase2step5_with_text_gray;
		cActivitiesForStep.image_step_six_header = image_phase2step6_with_text_gray;
		
		//agregando las imagenes a los botones 
		cActivitiesForStep.image_uno_tools_and_products = image_uno_tools_and_products;
		cActivitiesForStep.image_dos_videos = image_dos_videos;
		cActivitiesForStep.image_tres_self_assessment = image_tres_self_assessment;
		cActivitiesForStep.image_cuatro_simulations = image_cuatro_fotos_gray;
		cActivitiesForStep.image_cinco_personal_notes = image_cinco_personal_notes_gray;
		cActivitiesForStep.image_seis_frequently_questions = image_seis_frequently_questions_gray;
		cActivitiesForStep.image_siete_ask_your_teacher = image_siete_ask_your_teacher_gray;
		cActivitiesForStep.goToToolsAndProd += GoToToolsAndProducts;
		cActivitiesForStep.interfaceCallingGoToTools = "Phase2Step2";
		cActivitiesForStep.goToMenuSteps += GoBackToMenuOfStepsFromActivities;
		cActivitiesForStep.interfaceGoingBackFrom = "Phase2Step2";
		cActivitiesForStep.goToVideosForStep += GoToVideoOfEachStep;
		cActivitiesForStep.goToSelfAssessm += GoToSelfAssessment;

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: I30");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "I30", "7", "-1");

	}//cierra GoToActivitiesPhase2Step2


	/// <summary>
	/// Goes to activities phase2 step3. (Actividades de Matizado de Cantos Primera pasada(Fase Matizado))
	/// </summary>
	public void GoToActivitiesPhase2Step3(){
		
		if (current_interface == CurrentInterface.MENU_SUB_STEPS_PHASE2)
			Destroy (menuSubStepsPhaseTwo_interface_instance);
		else if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS)
			Destroy (ToolsAndProductsPhase2Step3_interface_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP3)
			Destroy (AR_Mode_Search_interface_instance);
		
		
		current_interface = CurrentInterface.ACTIVITIES_PHASE2_STEP3;
		
		//Ojo con lo siguiente estoy destruyendo todas las instancias que hayan de la interfaz de actividades por cada paso
		DestroyInstancesWithTag ("ActivitiesForEachStep");
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");
		
		ActivitiesForPhase2Step3_interface_instance = Instantiate (ActivitiesForEachStep);
		CanvasActivitiesForEachStepHeaders cActivitiesForStep = ActivitiesForPhase2Step3_interface_instance.GetComponent<CanvasActivitiesForEachStepHeaders> ();
		cActivitiesForStep.titulo_current_step = title_phase2_step3;
		cActivitiesForStep.introduction_text_path = introduction_text_phase2Step3_path;
		
		//agregando imagenes que se cargan en el header para el conjunto de fases del proceso:
		cActivitiesForStep.image_phase_one_header = phase1_with_text_image_gray_path;
		cActivitiesForStep.image_phase_two_header = phase2_with_text_image_path; //imagen activa que no va en gris
		cActivitiesForStep.image_phase_three_header = phase3_with_text_image_gray_path;
		cActivitiesForStep.image_phase_four_header = phase4_with_text_image_gray_path;
		cActivitiesForStep.image_phase_five_header = phase5_with_text_image_gray_path;
		cActivitiesForStep.image_phase_six_header = phase6_with_text_image_gray_path;
		
		//agregando imagenes que se cargan en el header para el conjunto de pasos de la fase correspondiente:
		cActivitiesForStep.image_step_one_header = image_phase2step1_with_text_gray;
		cActivitiesForStep.image_step_two_header = image_phase2step2_with_text_gray; //esta imagen se muestra activa NO en gris porque es el paso correspondiente
		cActivitiesForStep.image_step_three_header = image_phase2step3_with_text;
		cActivitiesForStep.image_step_four_header = image_phase2step4_with_text_gray;
		cActivitiesForStep.image_step_five_header = image_phase2step5_with_text_gray;
		cActivitiesForStep.image_step_six_header = image_phase2step6_with_text_gray;
		
		//agregando las imagenes a los botones 
		cActivitiesForStep.image_uno_tools_and_products = image_uno_tools_and_products;
		cActivitiesForStep.image_dos_videos = image_dos_videos;
		cActivitiesForStep.image_tres_self_assessment = image_tres_self_assessment;
		cActivitiesForStep.image_cuatro_simulations = image_cuatro_fotos_gray;
		cActivitiesForStep.image_cinco_personal_notes = image_cinco_personal_notes_gray;
		cActivitiesForStep.image_seis_frequently_questions = image_seis_frequently_questions_gray;
		cActivitiesForStep.image_siete_ask_your_teacher = image_siete_ask_your_teacher_gray;
		cActivitiesForStep.goToToolsAndProd += GoToToolsAndProducts;
		cActivitiesForStep.interfaceCallingGoToTools = "Phase2Step3";
		cActivitiesForStep.goToMenuSteps += GoBackToMenuOfStepsFromActivities;
		cActivitiesForStep.interfaceGoingBackFrom = "Phase2Step3";
		cActivitiesForStep.goToVideosForStep += GoToVideoOfEachStep;
		cActivitiesForStep.goToSelfAssessm += GoToSelfAssessment;

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: I31");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "I31", "9", "-1");
		
	}//cierra GoToActivitiesPhase2Step3


	/// <summary>
	/// Goes to activities phase2 step4. (Actividades de Matizado de Cantos Pasada Final(Fase Matizado))
	/// </summary>
	public void GoToActivitiesPhase2Step4(){
		
		if (current_interface == CurrentInterface.MENU_SUB_STEPS_PHASE2)
			Destroy (menuSubStepsPhaseTwo_interface_instance);
		else if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS)
			Destroy (ToolsAndProductsPhase2Step4_interface_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP4)
			Destroy (AR_Mode_Search_interface_instance);
		
		
		current_interface = CurrentInterface.ACTIVITIES_PHASE2_STEP4;
		
		//Ojo con lo siguiente estoy destruyendo todas las instancias que hayan de la interfaz de actividades por cada paso
		DestroyInstancesWithTag ("ActivitiesForEachStep");
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");
		
		ActivitiesForPhase2Step4_interface_instance = Instantiate (ActivitiesForEachStep);
		CanvasActivitiesForEachStepHeaders cActivitiesForStep = ActivitiesForPhase2Step4_interface_instance.GetComponent<CanvasActivitiesForEachStepHeaders> ();
		cActivitiesForStep.titulo_current_step = title_phase2_step4;
		cActivitiesForStep.introduction_text_path = introduction_text_phase2Step4_path;
		
		//agregando imagenes que se cargan en el header para el conjunto de fases del proceso:
		cActivitiesForStep.image_phase_one_header = phase1_with_text_image_gray_path;
		cActivitiesForStep.image_phase_two_header = phase2_with_text_image_path; //imagen activa que no va en gris
		cActivitiesForStep.image_phase_three_header = phase3_with_text_image_gray_path;
		cActivitiesForStep.image_phase_four_header = phase4_with_text_image_gray_path;
		cActivitiesForStep.image_phase_five_header = phase5_with_text_image_gray_path;
		cActivitiesForStep.image_phase_six_header = phase6_with_text_image_gray_path;
		
		//agregando imagenes que se cargan en el header para el conjunto de pasos de la fase correspondiente:
		cActivitiesForStep.image_step_one_header = image_phase2step1_with_text_gray;
		cActivitiesForStep.image_step_two_header = image_phase2step2_with_text_gray; //esta imagen se muestra activa NO en gris porque es el paso correspondiente
		cActivitiesForStep.image_step_three_header = image_phase2step3_with_text_gray;
		cActivitiesForStep.image_step_four_header = image_phase2step4_with_text;
		cActivitiesForStep.image_step_five_header = image_phase2step5_with_text_gray;
		cActivitiesForStep.image_step_six_header = image_phase2step6_with_text_gray;
		
		//agregando las imagenes a los botones 
		cActivitiesForStep.image_uno_tools_and_products = image_uno_tools_and_products;
		cActivitiesForStep.image_dos_videos = image_dos_videos;
		cActivitiesForStep.image_tres_self_assessment = image_tres_self_assessment;
		cActivitiesForStep.image_cuatro_simulations = image_cuatro_fotos_gray;
		cActivitiesForStep.image_cinco_personal_notes = image_cinco_personal_notes_gray;
		cActivitiesForStep.image_seis_frequently_questions = image_seis_frequently_questions_gray;
		cActivitiesForStep.image_siete_ask_your_teacher = image_siete_ask_your_teacher_gray;
		cActivitiesForStep.goToToolsAndProd += GoToToolsAndProducts;
		cActivitiesForStep.interfaceCallingGoToTools = "Phase2Step4";
		cActivitiesForStep.goToMenuSteps += GoBackToMenuOfStepsFromActivities;
		cActivitiesForStep.interfaceGoingBackFrom = "Phase2Step4";
		cActivitiesForStep.goToVideosForStep += GoToVideoOfEachStep;
		cActivitiesForStep.goToSelfAssessm += GoToSelfAssessment;

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: I32");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "I32", "10", "-1");
	}//cierra GoToActivitiesPhase2Step4


	/// <summary>
	/// Goes to activities phase2 step5. (Actividades de Matizado de interiores Primera pasada(Fase Matizado))
	/// </summary>
	public void GoToActivitiesPhase2Step5(){
		
		if (current_interface == CurrentInterface.MENU_SUB_STEPS_INTERIORES_PHASE2)
			Destroy (menuSubStepsPhaseTwoInterior_interf_instance);
		else if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS)
			Destroy (ToolsAndProductsPhase2Step5_interface_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP5)
			Destroy (AR_Mode_Search_interface_instance);
		
		
		current_interface = CurrentInterface.ACTIVITIES_PHASE2_STEP5;
		
		//Ojo con lo siguiente estoy destruyendo todas las instancias que hayan de la interfaz de actividades por cada paso
		DestroyInstancesWithTag ("ActivitiesForEachStep");
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");
		
		ActivitiesForPhase2Step5_interface_instance = Instantiate (ActivitiesForEachStep);
		CanvasActivitiesForEachStepHeaders cActivitiesForStep = ActivitiesForPhase2Step5_interface_instance.GetComponent<CanvasActivitiesForEachStepHeaders> ();
		cActivitiesForStep.titulo_current_step = title_phase2_step5;
		cActivitiesForStep.introduction_text_path = introduction_text_phase2Step5_path;
		
		//agregando imagenes que se cargan en el header para el conjunto de fases del proceso:
		cActivitiesForStep.image_phase_one_header = phase1_with_text_image_gray_path;
		cActivitiesForStep.image_phase_two_header = phase2_with_text_image_path; //imagen activa que no va en gris
		cActivitiesForStep.image_phase_three_header = phase3_with_text_image_gray_path;
		cActivitiesForStep.image_phase_four_header = phase4_with_text_image_gray_path;
		cActivitiesForStep.image_phase_five_header = phase5_with_text_image_gray_path;
		cActivitiesForStep.image_phase_six_header = phase6_with_text_image_gray_path;
		
		//agregando imagenes que se cargan en el header para el conjunto de pasos de la fase correspondiente:
		cActivitiesForStep.image_step_one_header = image_phase2step1_with_text_gray;
		cActivitiesForStep.image_step_two_header = image_phase2step2_with_text_gray; //esta imagen se muestra activa NO en gris porque es el paso correspondiente
		cActivitiesForStep.image_step_three_header = image_phase2step3_with_text_gray;
		cActivitiesForStep.image_step_four_header = image_phase2step4_with_text;
		cActivitiesForStep.image_step_five_header = image_phase2step5_with_text_gray;
		cActivitiesForStep.image_step_six_header = image_phase2step6_with_text_gray;
		
		//agregando las imagenes a los botones 
		cActivitiesForStep.image_uno_tools_and_products = image_uno_tools_and_products;
		cActivitiesForStep.image_dos_videos = image_dos_videos;
		cActivitiesForStep.image_tres_self_assessment = image_tres_self_assessment;
		cActivitiesForStep.image_cuatro_simulations = image_cuatro_fotos_gray;
		cActivitiesForStep.image_cinco_personal_notes = image_cinco_personal_notes_gray;
		cActivitiesForStep.image_seis_frequently_questions = image_seis_frequently_questions_gray;
		cActivitiesForStep.image_siete_ask_your_teacher = image_siete_ask_your_teacher_gray;
		cActivitiesForStep.goToToolsAndProd += GoToToolsAndProducts;
		cActivitiesForStep.interfaceCallingGoToTools = "Phase2Step5";
		cActivitiesForStep.goToMenuSteps += GoBackToMenuOfStepsFromActivities;
		cActivitiesForStep.interfaceGoingBackFrom = "Phase2Step5";
		cActivitiesForStep.goToVideosForStep += GoToVideoOfEachStep;
		cActivitiesForStep.goToSelfAssessm += GoToSelfAssessment;

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: I33");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "I33", "11", "-1");
		
	}//cierra GoToActivitiesPhase2Step5


	/// <summary>
	/// Goes to activities phase2 step6. (Actividades de Matizado de interiores Segunda pasada(Fase Matizado))
	/// </summary>
	public void GoToActivitiesPhase2Step6(){
		
		if (current_interface == CurrentInterface.MENU_SUB_STEPS_INTERIORES_PHASE2)
			Destroy (menuSubStepsPhaseTwoInterior_interf_instance);
		else if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS)
			Destroy (ToolsAndProductsPhase2Step6_interface_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP6)
			Destroy (AR_Mode_Search_interface_instance);
		
		
		current_interface = CurrentInterface.ACTIVITIES_PHASE2_STEP6;
		
		//Ojo con lo siguiente estoy destruyendo todas las instancias que hayan de la interfaz de actividades por cada paso
		DestroyInstancesWithTag ("ActivitiesForEachStep");
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");
		
		ActivitiesForPhase2Step6_interface_instance = Instantiate (ActivitiesForEachStep);
		CanvasActivitiesForEachStepHeaders cActivitiesForStep = ActivitiesForPhase2Step6_interface_instance.GetComponent<CanvasActivitiesForEachStepHeaders> ();
		cActivitiesForStep.titulo_current_step = title_phase2_step6;
		cActivitiesForStep.introduction_text_path = introduction_text_phase2Step6_path;
		
		//agregando imagenes que se cargan en el header para el conjunto de fases del proceso:
		cActivitiesForStep.image_phase_one_header = phase1_with_text_image_gray_path;
		cActivitiesForStep.image_phase_two_header = phase2_with_text_image_path; //imagen activa que no va en gris
		cActivitiesForStep.image_phase_three_header = phase3_with_text_image_gray_path;
		cActivitiesForStep.image_phase_four_header = phase4_with_text_image_gray_path;
		cActivitiesForStep.image_phase_five_header = phase5_with_text_image_gray_path;
		cActivitiesForStep.image_phase_six_header = phase6_with_text_image_gray_path;
		
		//agregando imagenes que se cargan en el header para el conjunto de pasos de la fase correspondiente:
		cActivitiesForStep.image_step_one_header = image_phase2step1_with_text_gray;
		cActivitiesForStep.image_step_two_header = image_phase2step2_with_text_gray; //esta imagen se muestra activa NO en gris porque es el paso correspondiente
		cActivitiesForStep.image_step_three_header = image_phase2step3_with_text_gray;
		cActivitiesForStep.image_step_four_header = image_phase2step4_with_text;
		cActivitiesForStep.image_step_five_header = image_phase2step5_with_text_gray;
		cActivitiesForStep.image_step_six_header = image_phase2step6_with_text_gray;
		
		//agregando las imagenes a los botones 
		cActivitiesForStep.image_uno_tools_and_products = image_uno_tools_and_products;
		cActivitiesForStep.image_dos_videos = image_dos_videos;
		cActivitiesForStep.image_tres_self_assessment = image_tres_self_assessment;
		cActivitiesForStep.image_cuatro_simulations = image_cuatro_fotos_gray;
		cActivitiesForStep.image_cinco_personal_notes = image_cinco_personal_notes_gray;
		cActivitiesForStep.image_seis_frequently_questions = image_seis_frequently_questions_gray;
		cActivitiesForStep.image_siete_ask_your_teacher = image_siete_ask_your_teacher_gray;
		cActivitiesForStep.goToToolsAndProd += GoToToolsAndProducts;
		cActivitiesForStep.interfaceCallingGoToTools = "Phase2Step6";
		cActivitiesForStep.goToMenuSteps += GoBackToMenuOfStepsFromActivities;
		cActivitiesForStep.interfaceGoingBackFrom = "Phase2Step6";
		cActivitiesForStep.goToVideosForStep += GoToVideoOfEachStep;
		cActivitiesForStep.goToSelfAssessm += GoToSelfAssessment;

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: I34");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "I34", "12", "-1");
		
	}//cierra GoToActivitiesPhase2Step6

	/// <summary>
	/// Goes to activities phase2 step7. (Actividades de Matizado de interiores Tercera pasada(Fase Matizado))
	/// </summary>
	public void GoToActivitiesPhase2Step7(){
		
		if (current_interface == CurrentInterface.MENU_SUB_STEPS_INTERIORES_PHASE2)
			Destroy (menuSubStepsPhaseTwoInterior_interf_instance);
		else if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS)
			Destroy (ToolsAndProductsPhase2Step7_interface_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP7)
			Destroy (AR_Mode_Search_interface_instance);
		
		
		current_interface = CurrentInterface.ACTIVITIES_PHASE2_STEP7;
		
		//Ojo con lo siguiente estoy destruyendo todas las instancias que hayan de la interfaz de actividades por cada paso
		DestroyInstancesWithTag ("ActivitiesForEachStep");
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");
		
		ActivitiesForPhase2Step7_interface_instance = Instantiate (ActivitiesForEachStep);
		CanvasActivitiesForEachStepHeaders cActivitiesForStep = ActivitiesForPhase2Step7_interface_instance.GetComponent<CanvasActivitiesForEachStepHeaders> ();
		cActivitiesForStep.titulo_current_step = title_phase2_step7;
		cActivitiesForStep.introduction_text_path = introduction_text_phase2Step7_path;
		
		//agregando imagenes que se cargan en el header para el conjunto de fases del proceso:
		cActivitiesForStep.image_phase_one_header = phase1_with_text_image_gray_path;
		cActivitiesForStep.image_phase_two_header = phase2_with_text_image_path; //imagen activa que no va en gris
		cActivitiesForStep.image_phase_three_header = phase3_with_text_image_gray_path;
		cActivitiesForStep.image_phase_four_header = phase4_with_text_image_gray_path;
		cActivitiesForStep.image_phase_five_header = phase5_with_text_image_gray_path;
		cActivitiesForStep.image_phase_six_header = phase6_with_text_image_gray_path;
		
		//agregando imagenes que se cargan en el header para el conjunto de pasos de la fase correspondiente:
		cActivitiesForStep.image_step_one_header = image_phase2step1_with_text_gray;
		cActivitiesForStep.image_step_two_header = image_phase2step2_with_text_gray; //esta imagen se muestra activa NO en gris porque es el paso correspondiente
		cActivitiesForStep.image_step_three_header = image_phase2step3_with_text_gray;
		cActivitiesForStep.image_step_four_header = image_phase2step4_with_text;
		cActivitiesForStep.image_step_five_header = image_phase2step5_with_text_gray;
		cActivitiesForStep.image_step_six_header = image_phase2step6_with_text_gray;
		
		//agregando las imagenes a los botones 
		cActivitiesForStep.image_uno_tools_and_products = image_uno_tools_and_products;
		cActivitiesForStep.image_dos_videos = image_dos_videos;
		cActivitiesForStep.image_tres_self_assessment = image_tres_self_assessment;
		cActivitiesForStep.image_cuatro_simulations = image_cuatro_fotos_gray;
		cActivitiesForStep.image_cinco_personal_notes = image_cinco_personal_notes_gray;
		cActivitiesForStep.image_seis_frequently_questions = image_seis_frequently_questions_gray;
		cActivitiesForStep.image_siete_ask_your_teacher = image_siete_ask_your_teacher_gray;
		cActivitiesForStep.goToToolsAndProd += GoToToolsAndProducts;
		cActivitiesForStep.interfaceCallingGoToTools = "Phase2Step7";
		cActivitiesForStep.goToMenuSteps += GoBackToMenuOfStepsFromActivities;
		cActivitiesForStep.interfaceGoingBackFrom = "Phase2Step7";
		cActivitiesForStep.goToVideosForStep += GoToVideoOfEachStep;
		cActivitiesForStep.goToSelfAssessm += GoToSelfAssessment;

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: I35");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "I35", "13", "-1");
		
	}//cierra GoToActivitiesPhase2Step7


	// <summary>
	/// Goes to activities phase2 step8. (Actividades de Matizado de interiores Pasada Final(Fase Matizado))
	/// </summary>
	public void GoToActivitiesPhase2Step8(){
		
		if (current_interface == CurrentInterface.MENU_SUB_STEPS_INTERIORES_PHASE2)
			Destroy (menuSubStepsPhaseTwoInterior_interf_instance);
		else if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS)
			Destroy (ToolsAndProductsPhase2Step8_interface_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP8)
			Destroy (AR_Mode_Search_interface_instance);
		
		
		current_interface = CurrentInterface.ACTIVITIES_PHASE2_STEP8;
		
		//Ojo con lo siguiente estoy destruyendo todas las instancias que hayan de la interfaz de actividades por cada paso
		DestroyInstancesWithTag ("ActivitiesForEachStep");
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");
		
		ActivitiesForPhase2Step8_interface_instance = Instantiate (ActivitiesForEachStep);
		CanvasActivitiesForEachStepHeaders cActivitiesForStep = ActivitiesForPhase2Step8_interface_instance.GetComponent<CanvasActivitiesForEachStepHeaders> ();
		cActivitiesForStep.titulo_current_step = title_phase2_step8;
		cActivitiesForStep.introduction_text_path = introduction_text_phase2Step8_path;
		
		//agregando imagenes que se cargan en el header para el conjunto de fases del proceso:
		cActivitiesForStep.image_phase_one_header = phase1_with_text_image_gray_path;
		cActivitiesForStep.image_phase_two_header = phase2_with_text_image_path; //imagen activa que no va en gris
		cActivitiesForStep.image_phase_three_header = phase3_with_text_image_gray_path;
		cActivitiesForStep.image_phase_four_header = phase4_with_text_image_gray_path;
		cActivitiesForStep.image_phase_five_header = phase5_with_text_image_gray_path;
		cActivitiesForStep.image_phase_six_header = phase6_with_text_image_gray_path;
		
		//agregando imagenes que se cargan en el header para el conjunto de pasos de la fase correspondiente:
		cActivitiesForStep.image_step_one_header = image_phase2step1_with_text_gray;
		cActivitiesForStep.image_step_two_header = image_phase2step2_with_text_gray; //esta imagen se muestra activa NO en gris porque es el paso correspondiente
		cActivitiesForStep.image_step_three_header = image_phase2step3_with_text_gray;
		cActivitiesForStep.image_step_four_header = image_phase2step4_with_text;
		cActivitiesForStep.image_step_five_header = image_phase2step5_with_text_gray;
		cActivitiesForStep.image_step_six_header = image_phase2step6_with_text_gray;
		
		//agregando las imagenes a los botones 
		cActivitiesForStep.image_uno_tools_and_products = image_uno_tools_and_products;
		cActivitiesForStep.image_dos_videos = image_dos_videos;
		cActivitiesForStep.image_tres_self_assessment = image_tres_self_assessment;
		cActivitiesForStep.image_cuatro_simulations = image_cuatro_fotos_gray;
		cActivitiesForStep.image_cinco_personal_notes = image_cinco_personal_notes_gray;
		cActivitiesForStep.image_seis_frequently_questions = image_seis_frequently_questions_gray;
		cActivitiesForStep.image_siete_ask_your_teacher = image_siete_ask_your_teacher_gray;
		cActivitiesForStep.goToToolsAndProd += GoToToolsAndProducts;
		cActivitiesForStep.interfaceCallingGoToTools = "Phase2Step8";
		cActivitiesForStep.goToMenuSteps += GoBackToMenuOfStepsFromActivities;
		cActivitiesForStep.interfaceGoingBackFrom = "Phase2Step8";
		cActivitiesForStep.goToVideosForStep += GoToVideoOfEachStep;
		cActivitiesForStep.goToSelfAssessm += GoToSelfAssessment;

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: I36");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "I36", "14", "-1");
		
	}//cierra GoToActivitiesPhase2Step8



	public void GoToToolsAndProducts(string interface_from){
		string fecha = "";
		Debug.LogError ("Llamado al metodo go to tools and products!!");

		switch (interface_from) {
				case "Phase1Step2":
					Debug.Log("Ingresa al case Phase1Step2... Cargando Interfaz en GoToToolsAndProducts");
					if (current_interface == CurrentInterface.ACTIVITIES_PHASE1_STEP2)
						Destroy (ActivitiesForPhase1Step2_interface_instance);
					else if(current_interface == CurrentInterface.AR_SEARCH_PRODUCTS_TUTORIAL)
						Destroy(TutorialPhaseTwoSearchProd_interface_instance);
					else if (current_interface == CurrentInterface.AR_SEARCH_BAYETA_PRODUCT_TUTORIAL)
						//Destroy(TutorialPhaseTwoSearchProd_interface_instance);
						Destroy (TutorialTwoSearchBayeta_interface_instance);
					
					current_interface = CurrentInterface.TOOLS_AND_PRODUCTS;
					
					ToolsAndProductsPhase1Step2_interface_instance = Instantiate (ToolsAndProductsInterface);
					CanvasToolsAndProductsManager cToolsAndProductsManager = ToolsAndProductsPhase1Step2_interface_instance.GetComponent<CanvasToolsAndProductsManager> ();
					cToolsAndProductsManager.image_header_path = "Sprites/tools_and_products/tools";
					cToolsAndProductsManager.title_header_text_path = "Texts/Phase1Step2/1_title_header_text";
					cToolsAndProductsManager.title_intro_content_text_path = "Texts/Phase1Step2/2_introduction_text";
					cToolsAndProductsManager.tool_one_text_path = "Texts/Phase1Step2/3_tool_one_text";
					cToolsAndProductsManager.tool_two_text_path = "Texts/Phase1Step2/4_tool_two_text";
					cToolsAndProductsManager.ruta_img_one_tool_path = "Sprites/phase1step2/FrameMarker16_maquina_agua_icon";
					cToolsAndProductsManager.ruta_img_two_tool_path = "Sprites/phase1step2/FrameMarker19_agua_jabon_icon";
					cToolsAndProductsManager.ruta_img_four_tool_path = "Sprites/phase1step2/FrameMarker21_baieta_neteja_icon";
					cToolsAndProductsManager.footer_search_text_path = "Texts/Phase1Step2/5_ending_search_text";
					cToolsAndProductsManager.goBackButtonAction += GoToActivitiesPhase1Step2;
					cToolsAndProductsManager.goToSearchProductsTools += GoToSearchObjectsTutorialPhase2;
					cToolsAndProductsManager.interfaceGoingBackFrom = interface_from;

					//asignando la interfaz activa para controlar el regreso:
					this.interfaceInstanceActive = ToolsAndProductsPhase1Step2_interface_instance;
					
					//registrando la navegacion de la interfaz:
					fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
					Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: I15");
					NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "I15", "1", "-1");
				
					break;
				case "Phase1Step3":

					Debug.Log("Ingresa al case Phase1Step3... Cargando Interfaz en GoToToolsAndProducts");
					if (current_interface == CurrentInterface.ACTIVITIES_PHASE1_STEP3)
							Destroy (ActivitiesForPhase1Step3_interface_instance);
					else if (current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP3)
							Destroy(AR_Mode_Search_interface_instance);

					current_interface = CurrentInterface.TOOLS_AND_PRODUCTS;

					ToolsAndProductsPhase1Step3_interface_instance = Instantiate (ToolsAndProductsInterface);
					CanvasToolsAndProductsManager cToolsAndProductsManagerP1S3 = ToolsAndProductsPhase1Step3_interface_instance.GetComponent<CanvasToolsAndProductsManager> ();
					cToolsAndProductsManagerP1S3.image_header_path = "Sprites/tools_and_products/tools";
					cToolsAndProductsManagerP1S3.title_header_text_path = "Texts/Phase1Step3/1_title_header_text";
					cToolsAndProductsManagerP1S3.title_intro_content_text_path = "Texts/Phase1Step3/2_introduction_text";
					cToolsAndProductsManagerP1S3.tool_one_text_path = "Texts/Phase1Step3/3_tool_one_text";
					cToolsAndProductsManagerP1S3.tool_two_text_path = "Texts/Phase1Step3/4_tool_two_text";
					cToolsAndProductsManagerP1S3.ruta_img_one_tool_path = "Sprites/phase1step3/FrameMarker16_maquina_agua_icon";
					cToolsAndProductsManagerP1S3.ruta_img_four_tool_path = "Sprites/phase1step3/FrameMarker25_papel_dc3430_icon";
					cToolsAndProductsManagerP1S3.footer_search_text_path = "Texts/Phase1Step3/5_ending_search_text";
					cToolsAndProductsManagerP1S3.goBackButtonAction += GoToActivitiesPhase1Step3;
					cToolsAndProductsManagerP1S3.goToSearchProductsTools += GoToSearchAguaPresionPhase1Step3;
					//Atencion: Es muy importante definir esta variable para poder ir hacia atras:
					cToolsAndProductsManagerP1S3.interfaceGoingBackFrom = interface_from;

					//asignando la interfaz activa para controlar el regreso:
					this.interfaceInstanceActive = ToolsAndProductsPhase1Step3_interface_instance;
					//registrando la navegacion de la interfaz:
					fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
					Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: I16");
					NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "I16", "2", "-1");
							
				break;
				case "Phase1Step4":
			
					Debug.Log("Ingresa al case Phase1Step4... Cargando Interfaz en GoToToolsAndProducts");
					if (current_interface == CurrentInterface.ACTIVITIES_PHASE1_STEP4)
						Destroy (ActivitiesForPhase1Step4_interface_instance);
					else if (current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP4)
						Destroy(AR_Mode_Search_interface_instance);
					
					current_interface = CurrentInterface.TOOLS_AND_PRODUCTS;
					
					ToolsAndProductsPhase1Step4_interface_instance = Instantiate (ToolsAndProductsInterface);
					CanvasToolsAndProductsManager cToolsAndProductsManagerP1S4 = ToolsAndProductsPhase1Step4_interface_instance.GetComponent<CanvasToolsAndProductsManager> ();
					cToolsAndProductsManagerP1S4.image_header_path = "Sprites/tools_and_products/tools";
					cToolsAndProductsManagerP1S4.title_header_text_path = "Texts/Phase1Step4/1_title_header_text";
					cToolsAndProductsManagerP1S4.title_intro_content_text_path = "Texts/Phase1Step4/2_introduction_text";
					cToolsAndProductsManagerP1S4.tool_one_text_path = "Texts/Phase1Step4/3_tool_one_text";
					//cToolsAndProductsManagerP1S4.tool_two_text_path = "Texts/Phase1Step4/4_tool_two_text";
					cToolsAndProductsManagerP1S4.ruta_img_one_tool_path = "Sprites/phase1step4/FrameMarker45_esponja_p320_icon";
					cToolsAndProductsManagerP1S4.ruta_img_four_tool_path = "Sprites/phase1step4/FrameMarker46_esponja_p400_icon";
					cToolsAndProductsManagerP1S4.footer_search_text_path = "Texts/Phase1Step4/5_ending_search_text";
					cToolsAndProductsManagerP1S4.goBackButtonAction += GoToActivitiesPhase1Step4;
					cToolsAndProductsManagerP1S4.goToSearchProductsTools += GoToSearchLijaFinaPhase1Step4;
					//Atencion: Es muy importante definir esta variable para poder ir hacia atras:
					cToolsAndProductsManagerP1S4.interfaceGoingBackFrom = interface_from;
					
					//asignando la interfaz activa para controlar el regreso:
					this.interfaceInstanceActive = ToolsAndProductsPhase1Step4_interface_instance;
					//registrando la navegacion de la interfaz:
					fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
					Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: I17");
					NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "I17", "3", "-1");

					break;
				case "Phase1Step5":
					Debug.Log("Ingresa al case Phase1Step5... Cargando Interfaz en GoToToolsAndProducts");
					if (current_interface == CurrentInterface.ACTIVITIES_PHASE1_STEP5)
						Destroy (ActivitiesForPhase1Step5_interface_instance);
					else if (current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP5)
						Destroy(AR_Mode_Search_interface_instance);
					
					current_interface = CurrentInterface.TOOLS_AND_PRODUCTS;
					
					ToolsAndProductsPhase1Step5_interface_instance = Instantiate (ToolsAndProductsInterface);
					CanvasToolsAndProductsManager cToolsAndProductsManagerP1S5 = ToolsAndProductsPhase1Step5_interface_instance.GetComponent<CanvasToolsAndProductsManager> ();
					cToolsAndProductsManagerP1S5.image_header_path = "Sprites/tools_and_products/tools";
					cToolsAndProductsManagerP1S5.title_header_text_path = "Texts/Phase1Step5/1_title_header_text";
					cToolsAndProductsManagerP1S5.title_intro_content_text_path = "Texts/Phase1Step5/2_introduction_text";
					cToolsAndProductsManagerP1S5.tool_one_text_path = "Texts/Phase1Step5/3_tool_one_text";
					//cToolsAndProductsManagerP1S4.tool_two_text_path = "Texts/Phase1Step4/4_tool_two_text";
					cToolsAndProductsManagerP1S5.ruta_img_one_tool_path = "Sprites/phase1step5/FrameMarker100_martillo_repasar_icon";
					//cToolsAndProductsManagerP1S4.ruta_img_four_tool_path = "Sprites/phase1step3/papel_absorbente";
					cToolsAndProductsManagerP1S5.footer_search_text_path = "Texts/Phase1Step5/6_ending_search_text";
					cToolsAndProductsManagerP1S5.goBackButtonAction += GoToActivitiesPhase1Step5;
					cToolsAndProductsManagerP1S5.goToSearchProductsTools += GoToSearchMartilloPhase1Step5;
					//Atencion: Es muy importante definir esta variable para poder ir hacia atras:
					cToolsAndProductsManagerP1S5.interfaceGoingBackFrom = interface_from;
					
					//asignando la interfaz activa para controlar el regreso:
					this.interfaceInstanceActive = ToolsAndProductsPhase1Step5_interface_instance;
					fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
					Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: I18");
					NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "I18", "4", "-1");
					break;

				case "Phase1Step6":
					Debug.Log("Ingresa al case Phase1Step6... Cargando Interfaz en GoToToolsAndProducts");
					if (current_interface == CurrentInterface.ACTIVITIES_PHASE1_STEP6)
						Destroy (ActivitiesForPhase1Step6_interface_instance);
					else if (current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP6)
						Destroy(AR_Mode_Search_interface_instance);
					
					current_interface = CurrentInterface.TOOLS_AND_PRODUCTS;
					
					ToolsAndProductsPhase1Step6_interface_instance = Instantiate (ToolsAndProductsInterface);
					CanvasToolsAndProductsManager cToolsAndProductsManagerP1S6 = ToolsAndProductsPhase1Step6_interface_instance.GetComponent<CanvasToolsAndProductsManager> ();
					cToolsAndProductsManagerP1S6.image_header_path = "Sprites/tools_and_products/tools";
					cToolsAndProductsManagerP1S6.title_header_text_path = "Texts/Phase1Step6/1_title_header_text";
					cToolsAndProductsManagerP1S6.title_intro_content_text_path = "Texts/Phase1Step6/2_introduction_text";
					cToolsAndProductsManagerP1S6.tool_one_text_path = "Texts/Phase1Step6/3_tool_one_text";
					cToolsAndProductsManagerP1S6.tool_two_text_path = "Texts/Phase1Step6/4_tool_two_text";
					cToolsAndProductsManagerP1S6.ruta_img_one_tool_path = "Sprites/phase1step6/FrameMarker26_desengrasante_icon";
					cToolsAndProductsManagerP1S6.ruta_img_four_tool_path = "Sprites/phase1step6/FrameMarker25_papel_dc3430_icon";
					cToolsAndProductsManagerP1S6.footer_search_text_path = "Texts/Phase1Step6/5_ending_search_text";
					cToolsAndProductsManagerP1S6.goBackButtonAction += GoToActivitiesPhase1Step6;
					cToolsAndProductsManagerP1S6.goToSearchProductsTools += GoToSearchDesengrasantePhase1Step6;
					//Atencion: Es muy importante definir esta variable para poder ir hacia atras:
					cToolsAndProductsManagerP1S6.interfaceGoingBackFrom = interface_from;
					
					//asignando la interfaz activa para controlar el regreso:
					this.interfaceInstanceActive = ToolsAndProductsPhase1Step6_interface_instance;
					fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
					Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: I19");
					NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "I19", "5", "-1");
				break;
				case "Phase2Step2":
					Debug.Log("Ingresa al case Phase2Step2... Cargando Interfaz en GoToToolsAndProducts");
					if (current_interface == CurrentInterface.ACTIVITIES_PHASE2_STEP2)
						Destroy (ActivitiesForPhase2Step2_interface_instance);
					else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP2)
						Destroy(AR_Mode_Search_interface_instance);
					
					current_interface = CurrentInterface.TOOLS_AND_PRODUCTS;
					
					ToolsAndProductsPhase2Step2_interface_instance = Instantiate (ToolsAndProductsInterface);
					CanvasToolsAndProductsManager cToolsAndProductsManagerP2S2 = ToolsAndProductsPhase2Step2_interface_instance.GetComponent<CanvasToolsAndProductsManager> ();
					cToolsAndProductsManagerP2S2.image_header_path = "Sprites/tools_and_products/tools";
					cToolsAndProductsManagerP2S2.title_header_text_path = "Texts/Phase2Step2/1_title_header_text";
					cToolsAndProductsManagerP2S2.title_intro_content_text_path = "Texts/Phase2Step2/0_introduction_text";
					cToolsAndProductsManagerP2S2.tool_one_text_path = "Texts/Phase2Step2/2_tool_one_text";
					cToolsAndProductsManagerP2S2.tool_two_text_path = "Texts/Phase2Step2/3_tool_two_text";
					cToolsAndProductsManagerP2S2.ruta_img_one_tool_path = "Sprites/phase2step2/FrameMarker65_cinta_enmascarar_icon";
					cToolsAndProductsManagerP2S2.ruta_img_four_tool_path = "Sprites/phase2step2/FrameMarker69_papel_enmascarar_icon";
					cToolsAndProductsManagerP2S2.footer_search_text_path = "Texts/Phase2Step2/4_ending_search_text";
					cToolsAndProductsManagerP2S2.goBackButtonAction += GoToActivitiesPhase2Step2;
					cToolsAndProductsManagerP2S2.goToSearchProductsTools += GoToSearchCintaEnmascPhase2Step2;
					//Atencion: Es muy importante definir esta variable para poder ir hacia atras:
					cToolsAndProductsManagerP2S2.interfaceGoingBackFrom = interface_from;
					
					//asignando la interfaz activa para controlar el regreso:
					this.interfaceInstanceActive = ToolsAndProductsPhase2Step2_interface_instance;
					fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
					Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: I37");
					NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "I37", "7", "-1");
				break;
				case "Phase2Step3":
					Debug.Log("Ingresa al case Phase2Step3... Cargando Interfaz en GoToToolsAndProducts");
					if (current_interface == CurrentInterface.ACTIVITIES_PHASE2_STEP3)
						Destroy (ActivitiesForPhase2Step3_interface_instance);
					else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP3)
						Destroy(AR_Mode_Search_interface_instance);
					
					current_interface = CurrentInterface.TOOLS_AND_PRODUCTS;
					
					ToolsAndProductsPhase2Step3_interface_instance = Instantiate (ToolsAndProductsInterface);
					CanvasToolsAndProductsManager cToolsAndProductsManagerP2S3 = ToolsAndProductsPhase2Step3_interface_instance.GetComponent<CanvasToolsAndProductsManager> ();
					cToolsAndProductsManagerP2S3.image_header_path = "Sprites/tools_and_products/tools";
					cToolsAndProductsManagerP2S3.title_header_text_path = "Texts/Phase2Step3/1_title_header_text";
					cToolsAndProductsManagerP2S3.title_intro_content_text_path = "Texts/Phase2Step3/2_introduction_text";
					cToolsAndProductsManagerP2S3.tool_one_text_path = "Texts/Phase2Step3/3_tool_one_text";
					cToolsAndProductsManagerP2S3.tool_two_text_path = "Texts/Phase2Step3/4_tool_two_text";
					cToolsAndProductsManagerP2S3.ruta_img_one_tool_path = "Sprites/phase2step3/FrameMarker45_esponja_p320_icon";
					cToolsAndProductsManagerP2S3.ruta_img_four_tool_path = "Sprites/phase2step3/FrameMarker25_papel_dc3430_icon";
					cToolsAndProductsManagerP2S3.ruta_img_five_tool_path = "Sprites/phase2step3/FrameMarker24_paper_neteja_icon";
					cToolsAndProductsManagerP2S3.ruta_img_six_tool_path = "Sprites/phase2step3/FrameMarker23_pistola_aire_icon";
					cToolsAndProductsManagerP2S3.footer_search_text_path = "Texts/Phase2Step3/5_ending_search_text";
					cToolsAndProductsManagerP2S3.goBackButtonAction += GoToActivitiesPhase2Step3;
					cToolsAndProductsManagerP2S3.goToSearchProductsTools += GoToSearchEsponjaP320Phase2Step3;
					//Atencion: Es muy importante definir esta variable para poder ir hacia atras:
					cToolsAndProductsManagerP2S3.interfaceGoingBackFrom = interface_from;
							
					//asignando la interfaz activa para controlar el regreso:
					this.interfaceInstanceActive = ToolsAndProductsPhase2Step3_interface_instance;
					fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
					Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: I38");
					NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "I38", "9", "-1");
				break;
				case "Phase2Step4":
					Debug.Log("Ingresa al case Phase2Step4... Cargando Interfaz en GoToToolsAndProducts");
					if (current_interface == CurrentInterface.ACTIVITIES_PHASE2_STEP4)
						Destroy (ActivitiesForPhase2Step4_interface_instance);
					else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP4)
						Destroy(AR_Mode_Search_interface_instance);
					
					current_interface = CurrentInterface.TOOLS_AND_PRODUCTS;
					
					ToolsAndProductsPhase2Step4_interface_instance = Instantiate (ToolsAndProductsInterface);
					CanvasToolsAndProductsManager cToolsAndProductsManagerP2S4 = ToolsAndProductsPhase2Step4_interface_instance.GetComponent<CanvasToolsAndProductsManager> ();
					cToolsAndProductsManagerP2S4.image_header_path = "Sprites/tools_and_products/tools";
					cToolsAndProductsManagerP2S4.title_header_text_path = "Texts/Phase2Step4/1_title_header_text";
					cToolsAndProductsManagerP2S4.title_intro_content_text_path = "Texts/Phase2Step4/2_introduction_text";
					cToolsAndProductsManagerP2S4.tool_one_text_path = "Texts/Phase2Step4/3_tool_one_text";
					cToolsAndProductsManagerP2S4.tool_two_text_path = "Texts/Phase2Step4/4_tool_two_text";
					cToolsAndProductsManagerP2S4.ruta_img_one_tool_path = "Sprites/phase2step4/FrameMarker46_esponja_p400_icon";
					cToolsAndProductsManagerP2S4.ruta_img_four_tool_path = "Sprites/phase2step4/FrameMarker25_papel_dc3430_icon";
					cToolsAndProductsManagerP2S4.ruta_img_five_tool_path = "Sprites/phase2step4/FrameMarker24_paper_neteja_icon";
					cToolsAndProductsManagerP2S4.ruta_img_six_tool_path = "Sprites/phase2step4/FrameMarker23_pistola_aire_icon";
					cToolsAndProductsManagerP2S4.footer_search_text_path = "Texts/Phase2Step4/5_ending_search_text";
					cToolsAndProductsManagerP2S4.goBackButtonAction += GoToActivitiesPhase2Step4;
					cToolsAndProductsManagerP2S4.goToSearchProductsTools += GoToSearchEsponjaP400Phase2Step4;
					//Atencion: Es muy importante definir esta variable para poder ir hacia atras:
					cToolsAndProductsManagerP2S4.interfaceGoingBackFrom = interface_from;
					
					//asignando la interfaz activa para controlar el regreso:
					this.interfaceInstanceActive = ToolsAndProductsPhase2Step4_interface_instance;
					fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
					Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: I39");
					NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "I39", "10", "-1");
				break;
				case "Phase2Step5":
					Debug.Log("Ingresa al case Phase2Step5... Cargando Interfaz en GoToToolsAndProducts");
					if (current_interface == CurrentInterface.ACTIVITIES_PHASE2_STEP5)
						Destroy (ActivitiesForPhase2Step5_interface_instance);
					else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP5)
						Destroy(AR_Mode_Search_interface_instance);
					
					current_interface = CurrentInterface.TOOLS_AND_PRODUCTS;
					
					ToolsAndProductsPhase2Step5_interface_instance = Instantiate (ToolsAndProductsInterface);
					CanvasToolsAndProductsManager cToolsAndProductsManagerP2S5 = ToolsAndProductsPhase2Step5_interface_instance.GetComponent<CanvasToolsAndProductsManager> ();
					cToolsAndProductsManagerP2S5.image_header_path = "Sprites/tools_and_products/tools";
					cToolsAndProductsManagerP2S5.title_header_text_path = "Texts/Phase2Step5/1_title_header_text";
					cToolsAndProductsManagerP2S5.title_intro_content_text_path = "Texts/Phase2Step5/2_introduction_text_tools";
					cToolsAndProductsManagerP2S5.tool_one_text_path = "Texts/Phase2Step5/3_tool_one_text";
					cToolsAndProductsManagerP2S5.tool_two_text_path = "Texts/Phase2Step5/4_tool_two_text";
					cToolsAndProductsManagerP2S5.ruta_img_one_tool_path = "Sprites/phase2step5/FrameMarker99_roto_orbital_icon";
					cToolsAndProductsManagerP2S5.ruta_img_two_tool_path = "Sprites/phase2step5/FrameMarker30_disco_p80_icon";
					cToolsAndProductsManagerP2S5.ruta_img_three_tool_path = "Sprites/phase2step5/FrameMarker32_disco_p120_icon";
					cToolsAndProductsManagerP2S5.ruta_img_four_tool_path = "Sprites/phase2step5/FrameMarker23_pistola_aire_icon";
					cToolsAndProductsManagerP2S5.ruta_img_five_tool_path = "Sprites/phase2step5/FrameMarker24_paper_neteja_icon";
					cToolsAndProductsManagerP2S5.ruta_img_six_tool_path = "Sprites/phase2step5/FrameMarker25_papel_dc3430_icon";
					cToolsAndProductsManagerP2S5.footer_search_text_path = "Texts/Phase2Step5/5_ending_search_text";
					cToolsAndProductsManagerP2S5.goBackButtonAction += GoToActivitiesPhase2Step5;
					cToolsAndProductsManagerP2S5.goToSearchProductsTools += GoToSearchRotoOrbitalPhase2Step5;
							//Atencion: Es muy importante definir esta variable para poder ir hacia atras:
					cToolsAndProductsManagerP2S5.interfaceGoingBackFrom = interface_from;
							
							//asignando la interfaz activa para controlar el regreso:
					this.interfaceInstanceActive = ToolsAndProductsPhase2Step5_interface_instance;
					fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
					Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: I40");
					NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "I40", "11", "-1");
			break;
			case "Phase2Step6":
				Debug.Log("Ingresa al case Phase2Step6... Cargando Interfaz en GoToToolsAndProducts");
				if (current_interface == CurrentInterface.ACTIVITIES_PHASE2_STEP6)
					Destroy (ActivitiesForPhase2Step6_interface_instance);
				else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP6)
					Destroy(AR_Mode_Search_interface_instance);
				
				current_interface = CurrentInterface.TOOLS_AND_PRODUCTS;
				
				ToolsAndProductsPhase2Step6_interface_instance = Instantiate (ToolsAndProductsInterface);
				CanvasToolsAndProductsManager cToolsAndProductsManagerP2S6 = ToolsAndProductsPhase2Step6_interface_instance.GetComponent<CanvasToolsAndProductsManager> ();
				cToolsAndProductsManagerP2S6.image_header_path = "Sprites/tools_and_products/tools";
				cToolsAndProductsManagerP2S6.title_header_text_path = "Texts/Phase2Step6/1_title_header_text";
				cToolsAndProductsManagerP2S6.title_intro_content_text_path = "Texts/Phase2Step6/2_introduction_text_tools";
				cToolsAndProductsManagerP2S6.tool_one_text_path = "Texts/Phase2Step6/3_tool_one_text";
				cToolsAndProductsManagerP2S6.tool_two_text_path = "Texts/Phase2Step6/4_tool_two_text";
				if(last_markerid_scanned == 30)
					cToolsAndProductsManagerP2S6.ruta_img_one_tool_path = "Sprites/phase2step6/FrameMarker33_disco_p150";
				else 
					cToolsAndProductsManagerP2S6.ruta_img_one_tool_path = "Sprites/phase2step6/FrameMarker34_disco_p180";
				cToolsAndProductsManagerP2S6.ruta_img_four_tool_path = "Sprites/phase2step6/FrameMarker23_pistola_aire_icon";
				cToolsAndProductsManagerP2S6.ruta_img_five_tool_path = "Sprites/phase2step6/FrameMarker24_paper_neteja_icon";
				cToolsAndProductsManagerP2S6.ruta_img_six_tool_path = "Sprites/phase2step6/FrameMarker25_papel_dc3430_icon";
				cToolsAndProductsManagerP2S6.footer_search_text_path = "Texts/Phase2Step6/5_ending_search_text";
				cToolsAndProductsManagerP2S6.goBackButtonAction += GoToActivitiesPhase2Step6;
				cToolsAndProductsManagerP2S6.goToSearchProductsTools += GoToSearchDiscoP150Phase2Step6;
				//Atencion: Es muy importante definir esta variable para poder ir hacia atras:
				cToolsAndProductsManagerP2S6.interfaceGoingBackFrom = interface_from;
				
				//asignando la interfaz activa para controlar el regreso:
			this.interfaceInstanceActive = ToolsAndProductsPhase2Step6_interface_instance;
			fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: I41");
			NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "I41", "12", "-1");
				break;
		case "Phase2Step7":
			Debug.Log("Ingresa al case Phase2Step7... Cargando Interfaz en GoToToolsAndProducts");
			if (current_interface == CurrentInterface.ACTIVITIES_PHASE2_STEP7)
				Destroy (ActivitiesForPhase2Step7_interface_instance);
			else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP7)
				Destroy(AR_Mode_Search_interface_instance);
			
			current_interface = CurrentInterface.TOOLS_AND_PRODUCTS;
			
			ToolsAndProductsPhase2Step7_interface_instance = Instantiate (ToolsAndProductsInterface);
			CanvasToolsAndProductsManager cToolsAndProductsManagerP2S7 = ToolsAndProductsPhase2Step7_interface_instance.GetComponent<CanvasToolsAndProductsManager> ();
			cToolsAndProductsManagerP2S7.image_header_path = "Sprites/tools_and_products/tools";
			cToolsAndProductsManagerP2S7.title_header_text_path = "Texts/Phase2Step7/1_title_header_text";
			cToolsAndProductsManagerP2S7.title_intro_content_text_path = "Texts/Phase2Step7/2_introduction_text_tools";
			cToolsAndProductsManagerP2S7.tool_one_text_path = "Texts/Phase2Step7/3_tool_one_text";
			cToolsAndProductsManagerP2S7.tool_two_text_path = "Texts/Phase2Step7/4_tool_two_text";
			cToolsAndProductsManagerP2S7.ruta_img_one_tool_path = "Sprites/phase2step7/FrameMarker36_disco_p240_icon";
			cToolsAndProductsManagerP2S7.ruta_img_four_tool_path = "Sprites/phase2step7/FrameMarker23_pistola_aire_icon";
			cToolsAndProductsManagerP2S7.ruta_img_five_tool_path = "Sprites/phase2step7/FrameMarker24_paper_neteja_icon";
			cToolsAndProductsManagerP2S7.ruta_img_six_tool_path = "Sprites/phase2step7/FrameMarker25_papel_dc3430_icon";
			cToolsAndProductsManagerP2S7.footer_search_text_path = "Texts/Phase2Step7/5_ending_search_text";
			cToolsAndProductsManagerP2S7.goBackButtonAction += GoToActivitiesPhase2Step7;
			cToolsAndProductsManagerP2S7.goToSearchProductsTools += GoToSearchDiscoP240Phase2Step7;
			//Atencion: Es muy importante definir esta variable para poder ir hacia atras:
			cToolsAndProductsManagerP2S7.interfaceGoingBackFrom = interface_from;
			
			//asignando la interfaz activa para controlar el regreso:
			this.interfaceInstanceActive = ToolsAndProductsPhase2Step7_interface_instance;
			fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: I42");
			NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "I42", "13", "-1");
			break;
		case "Phase2Step8":
			Debug.Log("Ingresa al case Phase2Step8... Cargando Interfaz en GoToToolsAndProducts");
			if (current_interface == CurrentInterface.ACTIVITIES_PHASE2_STEP8)
				Destroy (ActivitiesForPhase2Step8_interface_instance);
			else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP8)
				Destroy(AR_Mode_Search_interface_instance);
			
			current_interface = CurrentInterface.TOOLS_AND_PRODUCTS;
			
			ToolsAndProductsPhase2Step8_interface_instance = Instantiate (ToolsAndProductsInterface);
			CanvasToolsAndProductsManager cToolsAndProductsManagerP2S8 = ToolsAndProductsPhase2Step8_interface_instance.GetComponent<CanvasToolsAndProductsManager> ();
			cToolsAndProductsManagerP2S8.image_header_path = "Sprites/tools_and_products/tools";
			cToolsAndProductsManagerP2S8.title_header_text_path = "Texts/Phase2Step8/1_title_header_text";
			cToolsAndProductsManagerP2S8.title_intro_content_text_path = "Texts/Phase2Step8/2_introduction_text_tools";
			cToolsAndProductsManagerP2S8.tool_one_text_path = "Texts/Phase2Step8/3_tool_one_text";
			cToolsAndProductsManagerP2S8.tool_two_text_path = "Texts/Phase2Step8/4_tool_two_text";
			cToolsAndProductsManagerP2S8.ruta_img_one_tool_path = "Sprites/phase2step8/FrameMarker38_disco_p320";
			cToolsAndProductsManagerP2S8.ruta_img_four_tool_path = "Sprites/phase2step8/FrameMarker23_pistola_aire_icon";
			cToolsAndProductsManagerP2S8.ruta_img_five_tool_path = "Sprites/phase2step8/FrameMarker24_paper_neteja_icon";
			cToolsAndProductsManagerP2S8.ruta_img_six_tool_path = "Sprites/phase2step8/FrameMarker25_papel_dc3430_icon";
			cToolsAndProductsManagerP2S8.footer_search_text_path = "Texts/Phase2Step8/5_ending_search_text";
			cToolsAndProductsManagerP2S8.goBackButtonAction += GoToActivitiesPhase2Step8;
			cToolsAndProductsManagerP2S8.goToSearchProductsTools += GoToSearchDiscoP320Phase2Step8;
			//Atencion: Es muy importante definir esta variable para poder ir hacia atras:
			cToolsAndProductsManagerP2S8.interfaceGoingBackFrom = interface_from;
			
			//asignando la interfaz activa para controlar el regreso:
			this.interfaceInstanceActive = ToolsAndProductsPhase2Step8_interface_instance;
			fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: I43");
			NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "I43", "14", "-1");
			break;
			break;
			
		case "":
			Debug.Log("-->ERROR: La interfaz de la cual se pretende retornar viene vacia");
				break;
		}


		/*
		if (current_interface == CurrentInterface.MENU_STEPS_PHASE1)
			Destroy (instance_interface);
		else if (current_interface == CurrentInterface.MENU_PHASES) {
			//other interface
		}
		*/

	} //cierra GoToToolsAndProducts


	/// <summary>
	/// Goes the back to menu of activities from tools products.
	/// </summary>
	/// <param name="interface_coming_from">Interface_coming_from.</param>
	public void GoBackToMenuActivitiesFromToolsProducts(string interface_coming_from){

		Debug.LogError ("Llamado al metodo go to tools and products!!");
		
		switch (interface_coming_from) {
			case "Phase1Step2":
				GoToActivitiesPhase1Step2();
				break;
			case "Phase1Step3":
				GoToActivitiesPhase1Step3();
				break;
			case "Phase1Step4":
				GoToActivitiesPhase1Step4();
				break;
			case "Phase1Step5":
				GoToActivitiesPhase1Step5();
				break;
			case "Phase1Step6":
				GoToActivitiesPhase1Step6();
				break;
			case "Phase2Step2":
				GoToActivitiesPhase2Step2();
				break;
			case "Phase2Step3":
				GoToActivitiesPhase2Step3();
				break;
			case "Phase2Step4":
				GoToActivitiesPhase2Step4();
				break;
			case "Phase2Step5":
				GoToActivitiesPhase2Step5();
				break;
			case "Phase2Step6":
				GoToActivitiesPhase2Step6();
				break;
			case "Phase2Step7":
				GoToActivitiesPhase2Step7();
				break;
			case "Phase2Step8":
				GoToActivitiesPhase2Step8();
				break;
		case "":
				Debug.Log("ERROR: No hay interface_coming_from definido en el metodo GoBackFromToolsAndProducts");
				break;
		}
	} //cierra GoBackToMenuActivitiesFromToolsProducts


	/// <summary>
	/// Goes the back to menu of activities from take pictures info interface.
	/// </summary>
	/// <param name="interface_coming_from">Interface_coming_from.</param>
	public void GoBackToMenuActivitiesFromTakePictures(string interface_coming_from){
		
		Debug.LogError ("Llamado al metodo go to tools and products!!");
		
		switch (interface_coming_from) {
		case "Phase1Step2":
			GoToActivitiesPhase1Step2();
			break;
		case "Phase1Step3":
			GoToActivitiesPhase1Step3();
			break;
		case "Phase1Step4":
			GoToActivitiesPhase1Step4();
			break;
		case "Phase1Step5":
			GoToActivitiesPhase1Step5();
			break;
		case "Phase1Step6":
			GoToActivitiesPhase1Step6();
			break;
		case "":
			Debug.Log("ERROR: No hay interface_coming_from definido en el metodo GoBackFromToolsAndProducts");
			break;
		}
	} //cierra GoBackToMenuActivitiesFromToolsProducts
	
	
	/// <summary>
	/// Este metodo es invocado desde el action de un boton en la interfaz. Normalmente desde el boton regresar
	/// en el header de la interfaz de actividades de un paso del proceso. 
	/// Previamente antes de llamar a este metodo se debe haber fijado el parametro: interface_coming_from
	/// </summary>
	/// <param name="interface_coming_from">Parametro que indica la interfaz desde la cual se esta regresando</param>
	public void GoBackToMenuOfStepsFromActivities(string interface_coming_from){
		
		Debug.LogError ("Llamado al metodo go back to menu of Steps!!");
		
		switch (interface_coming_from) {
		case "Phase1Step2":
			Debug.Log("Ingresa a Phase1Step2 en GoBackToMenuOfStepsFromActivities con interface_coming_from= " + interface_coming_from);
			//ir al menu de pasos de la fase 1:
			//en este caso no es necesario comparar desde que interfaz viene porque ya se hace en el siguiente metodo:
			GoToMenuStepsPhase1();
			break;
		case "Phase1Step3":
			Debug.Log("Ingresa a Phase1Step3 en GoBackToMenuOfStepsFromActivities con interface_coming_from= " + interface_coming_from);
			GoToMenuStepsPhase1();
			break;
		case "Phase1Step4":
			Debug.Log("Ingresa a Phase1Step4 en GoBackToMenuOfStepsFromActivities con interface_coming_from= " + interface_coming_from);
			GoToMenuStepsPhase1();
			break;
		case "Phase1Step5":
			Debug.Log("Ingresa a Phase1Step5 en GoBackToMenuOfStepsFromActivities con interface_coming_from= " + interface_coming_from);
			GoToMenuStepsPhase1();
			break;
		case "Phase1Step6":
			Debug.Log("Ingresa a Phase1Step6 en GoBackToMenuOfStepsFromActivities con interface_coming_from= " + interface_coming_from);
			GoToMenuStepsPhase1();
			break;
		case "Phase2Step2":
			Debug.Log("Ingresa a Phase2Step2 en GoBackToMenuOfStepsFromActivities con interface_coming_from= " + interface_coming_from);
			GoToMenuStepsPhase2();
			break;
		case "Phase2Step3":
			Debug.Log("Ingresa a Phase2Step3 en GoBackToMenuOfStepsFromActivities con interface_coming_from= " + interface_coming_from);
			GoToSubMenuStepsLijadoCantos();
			break;
		case "Phase2Step4":
			Debug.Log("Ingresa a Phase2Step4 en GoBackToMenuOfStepsFromActivities con interface_coming_from= " + interface_coming_from);
			GoToSubMenuStepsLijadoCantos();
			break;
		case "Phase2Step5":
			Debug.Log("Ingresa a Phase2Step5 en GoBackToMenuOfStepsFromActivities con interface_coming_from= " + interface_coming_from);
			GoToSubMenuStepsLijadoInteriores();
			break;
		case "Phase2Step6":
			Debug.Log("Ingresa a Phase2Step6 en GoBackToMenuOfStepsFromActivities con interface_coming_from= " + interface_coming_from);
			GoToSubMenuStepsLijadoInteriores();
			break;
		case "Phase2Step7":
			Debug.Log("Ingresa a Phase2Step7 en GoBackToMenuOfStepsFromActivities con interface_coming_from= " + interface_coming_from);
			GoToSubMenuStepsLijadoInteriores();
			break;
		case "Phase2Step8":
			Debug.Log("Ingresa a Phase2Step8 en GoBackToMenuOfStepsFromActivities con interface_coming_from= " + interface_coming_from);
			GoToSubMenuStepsLijadoInteriores();
			break;
		}

	} //cierra GoBackToMenuOfStepsFromActivities


	public void accion_test(string tipo){
		Debug.LogError ("El string recibido con la accion es: " + tipo);
	
	}

	public void GoToSelfAssessment(string interface_from){
		string numero_paso = "";
		string numero_de_pregs = "5"; //este parametro ahora se fija desde la interfaz web de la App Pintura. Por lo tanto este es un valor por defecto.
		string tipo_test = ""; //variable que define el tipo de test 1 = modo guiado, 2 = modo evaluacion.
		//obteniendo la fecha para el registro de navegacion de interfaces:
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		switch (interface_from) {
			case "Phase1Step2":
				Debug.Log ("Llamando al metodo GoToSelfAssessment con parametro Phase1Step2");
				numero_paso = "1";
				tipo_test = "1";
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: A1");
				NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "A1", "1", "-1");
			break;
			case "Phase1Step3":
				Debug.Log ("Llamando al metodo GoToSelfAssessment con parametro Phase1Step3");
				numero_paso = "2";
				tipo_test = "1";
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:A2");
				NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "A2", "2", "-1");
			break;
			case "Phase1Step4":
				Debug.Log ("Llamando al metodo GoToSelfAssessment con parametro Phase1Step4");
				numero_paso = "3";
				tipo_test = "1";
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: A3");
				NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "A3", "3", "-1");
			break;
			case "Phase1Step5":
				Debug.Log ("Llamando al metodo GoToSelfAssessment con parametro Phase1Step5");
				numero_paso = "4";
				tipo_test = "1";
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: A4");
				NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "A4", "4", "-1");
			break;
			case "Phase1Step6":
				Debug.Log ("Llamando al metodo GoToSelfAssessment con parametro Phase1Step6");
				numero_paso = "5";
				tipo_test = "1";
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: A5");
				NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "A5", "5", "-1");
			break;
			case "Phase2Step2":
				Debug.Log ("Llamando al metodo GoToSelfAssessment con parametro Phase2Step2");
				numero_paso = "7"; //proteccion de superficies
				tipo_test = "1";
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: A6");
				NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "A6", "7", "-1");
			break;
			case "Phase2Step3":
				Debug.Log ("Llamando al metodo GoToSelfAssessment con parametro Phase2Step3");
				numero_paso = "9"; //lijado de cantos pasada inicial
				tipo_test = "1";
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: A7");
				NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "A7", "9", "-1");
			break;
			case "Phase2Step4":
				Debug.Log ("Llamando al metodo GoToSelfAssessment con parametro Phase2Step4");
				numero_paso = "10"; //lijado de cantos pasada final
				tipo_test = "1";
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: A8");
				NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "A8", "10", "-1");
			break;
			case "Phase2Step5":
				Debug.Log ("Llamando al metodo GoToSelfAssessment con parametro Phase2Step5");
				numero_paso = "11"; //lijado de interiores primera pasada
				tipo_test = "1";
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: A9");
				NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "A9", "11", "-1");
			break;
			case "Phase2Step6":
				Debug.Log ("Llamando al metodo GoToSelfAssessment con parametro Phase2Step6");
				numero_paso = "12"; //lijado de interiores segunda pasada
				tipo_test = "1";
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: A10");
				NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "A10", "12", "-1");
			break;
			case "Phase2Step7":
				Debug.Log ("Llamando al metodo GoToSelfAssessment con parametro Phase2Step7");
				numero_paso = "13"; //lijado de interiores segunda pasada
				tipo_test = "1";
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: A11");
				NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "A11", "13", "-1");
			break;
			case "Phase2Step8":
				Debug.Log ("Llamando al metodo GoToSelfAssessment con parametro Phase2Step8");
				numero_paso = "14"; //lijado de interiores segunda pasada
				tipo_test = "1";
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: A12");
				NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "A12", "14", "-1");
			break;
		}

		//Cargando la URL desde un archivo de texto para que se pueda parametrizar facilmente:
		TextAsset url_server = Resources.Load<TextAsset> ("Texts/00_server_base_path");
		string url_conexion = "";
		if (url_server != null)
			url_conexion = url_server.text;

		Debug.Log ("Click en el Metodo SelfAssessment!!!"); 
		var and_unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		var current_act = and_unity.GetStatic<AndroidJavaObject>("currentActivity");
		Debug.Log("Se ha obtenido current activity...");
		// Accessing the class to call a static method on it
		var assessment_activity = new AndroidJavaClass("edu.udg.bcds.pintura.arapp.SelfAssessmentMain");
		//var jc = new AndroidJavaClass("edu.udg.bcds.pintura.tools.SelfAssessment");
		//var video_activity = new AndroidJavaClass("edu.udg.bcds.pintura.arapp.VideoActivity");
		Debug.Log ("Se ha obtenido StartActivity...");

		object[] parameters = new object[6]; 
		parameters [0] = current_act; //pasando el argumento de la actividad actual que se debe reproducir
		parameters [1] = numero_de_pregs; //definiendo el numero de preguntas que se quiere que aparezcan
		parameters [2] = numero_paso; //definiendo el paso para el cual se quieren cargar las preguntas
		parameters [3] = url_conexion;
		parameters [4] = this.codigo_estudiante;
		parameters [5] = tipo_test;
		// Calling a Call method to which the current activity is passed
		assessment_activity.CallStatic("StartModule", parameters);

	} // cierra metodo GoToSelfAssessment


	public void ResultSelfAssessment(string msg){
		Debug.Log ("**********Se ha invocado el metodo ResultSelfAssessment msg=" + msg);
		//obteniendo la fecha para el registro de navegacion de interfaces:
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		switch (msg) {
			case "1":
				Debug.Log ("AppManager: Se ha completado el test del paso 1");
				steps_phase_one_completed[1].selfevaluation = true;
				steps_phase_one_completed[1].CheckStepCompletion();
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: A5");
				NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "SA1", "1", "-1");
			break;
			case "2":
				Debug.Log ("AppManager: Se ha completado el test del paso 2");
				steps_phase_one_completed[2].selfevaluation = true;
				steps_phase_one_completed[2].CheckStepCompletion();
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: A5");
				NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "SA2", "2", "-1");
			break;
			case "3":
				Debug.Log ("AppManager: Se ha completado el test del paso 3");
				steps_phase_one_completed[3].selfevaluation = true;
				steps_phase_one_completed[3].CheckStepCompletion();
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: A5");
				NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "SA3", "3", "-1");
			break;
			case "4":
				Debug.Log ("AppManager: Se ha completado el test del paso 4");
				steps_phase_one_completed[4].selfevaluation = true;
				steps_phase_one_completed[4].CheckStepCompletion();
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: A5");
				NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "SA4", "4", "-1");
			break;
			case "5":
				Debug.Log ("AppManager: Se ha completado el test del paso 5");
				steps_phase_one_completed[5].selfevaluation = true;
				steps_phase_one_completed[5].CheckStepCompletion();
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: A5");
				NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "SA5", "5", "-1");
			break;
			case "7": //proteccion de superficies FASE MATIZADO
				Debug.Log ("AppManager: Se ha completado el test del paso 7");
				steps_phase_two_completed[1].selfevaluation = true;
				steps_phase_two_completed[1].CheckStepCompletion();
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: A5");
				NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "SA6", "7", "-1");
			break;
			case "9": //lijado de cantos primera pasada FASE MATIZADO
				Debug.Log ("AppManager: Se ha completado el test del paso 9");
				steps_phase_two_completed[2].selfevaluation = true;
				steps_phase_two_completed[2].CheckStepCompletion();
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: A5");
				NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "SA7", "9", "-1");
			break;
			case "10": //lijado de cantos segunda pasada FASE MATIZADO
				Debug.Log ("AppManager: Se ha completado el test del paso 10");
				steps_phase_two_completed[3].selfevaluation = true;
				steps_phase_two_completed[3].CheckStepCompletion();
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: A5");
				NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "SA8", "10", "-1");
			break;
			case "11": //lijado de interiores Primera pasada FASE MATIZADO
				Debug.Log ("AppManager: Se ha completado el test del paso 11");
				steps_phase_two_completed[4].selfevaluation = true;
				steps_phase_two_completed[4].CheckStepCompletion();
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: A5");
				NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "SA9", "11", "-1");
			break;
			case "12": //lijado de interiores Primera pasada FASE MATIZADO
				Debug.Log ("AppManager: Se ha completado el test del paso 12");
				steps_phase_two_completed[5].selfevaluation = true;
				steps_phase_two_completed[5].CheckStepCompletion();
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: A5");
				NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "SA10", "12", "-1");
			break;
			case "13": //lijado de interiores Primera pasada FASE MATIZADO
				Debug.Log ("AppManager: Se ha completado el test del paso 13");
				steps_phase_two_completed[6].selfevaluation = true;
				steps_phase_two_completed[6].CheckStepCompletion();
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: A5");
				NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "SA11", "13", "-1");
			break;
			case "14": //lijado de interiores Primera pasada FASE MATIZADO
				Debug.Log ("AppManager: Se ha completado el test del paso 14");
				steps_phase_two_completed[7].selfevaluation = true;
				steps_phase_two_completed[7].CheckStepCompletion();
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: A5");
				NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "SA12", "14", "-1");
			break;
			
		} //cierra switch
		//guardando el estado de la aplicacion en este momento:
		SaveDataForStudent ();
	} //cierra ResultForSelfAssessment

	/// <summary>
	/// Results the take photo activity. Method that is called from ControllerPinturaCameraImage to notify if the activity was completed
	/// </summary>
	/// <param name="paso">Paso.</param>
	public void ResultTakePhotoActivity(string paso, string tipo_ficha){
		Debug.Log ("**********Se ha invocado el metodo ResultTakePhoto con parametros paso=" + paso + ",tipo=" + tipo_ficha);
		
		switch (paso) {
			//en el paso 5 de la fase 1 es el unico paso donde hay actividad de tomar fotos:
			case "Phase1Step6":
				if(tipo_ficha == "seguridad"){
					steps_phase_one_completed[5].take_photo_ficha_seguridad = true;
					steps_phase_one_completed[5].CheckStepCompletion();
					if(steps_phase_one_completed[5].step_completed)//si el paso esta completo entonces ya se puede habilitar la FASE 2
						phase_two_enable = true;
				} else { 
					steps_phase_one_completed[5].take_photo_ficha_tecnica = true;
					steps_phase_one_completed[5].CheckStepCompletion();
					if(steps_phase_one_completed[5].step_completed)//si el paso esta completo entonces ya se puede habilitar la FASE 2
						phase_two_enable = true;
				}
			break;
		} //cierra switch

		//guardando el estado de la aplicacion en este momento:
		SaveDataForStudent ();
	}


	public void GoToVideoOfEachStep(string interface_from){

		AndroidJavaClass androidJC;
		AndroidJavaObject jo;
		AndroidJavaClass jc;
		string video_url;
		//obteniendo la fecha para el registro de navegacion de interfaces:
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		//hay que tener en cuenta que este metodo depende de la variable "interfaceCallingGoToTools" que se define cuando
		//se define el llamado a este metodo por ejemplo en el metodo "GoToActivitiesPhase1Step2"
		switch (interface_from) {
			case "Phase1Step2":
				steps_phase_one_completed[1].videos_about_process = true;
				steps_phase_one_completed[1].CheckStepCompletion();
				//asignando la URL dependiendo del paso:
				video_url = video_phase1_step2;
				//registrando la navegacion hacia esta interfaz:
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: V1");
				NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "V1", "1", "-1");
				break;
			case "Phase1Step3":
				steps_phase_one_completed[2].videos_about_process = true;
				steps_phase_one_completed[2].CheckStepCompletion();
				//asignando la URL dependiendo del paso:
				video_url = video_phase1_step3;
				//registrando la navegacion hacia esta interfaz:
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: V2");
				NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "V2", "1", "-1");
				break;
			case "Phase1Step4":
				steps_phase_one_completed[3].videos_about_process = true;
				steps_phase_one_completed[3].CheckStepCompletion();
				//asignando la URL dependiendo del paso:
				video_url = video_phase1_step4;
				//registrando la navegacion hacia esta interfaz:
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: V3");
				NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "V3", "1", "-1");
				break;
			case "Phase1Step5":
				steps_phase_one_completed[4].videos_about_process = true;
				steps_phase_one_completed[4].CheckStepCompletion();
				//asignando la URL dependiendo del paso:
				video_url = video_phase1_step5;
				//registrando la navegacion hacia esta interfaz:
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: V4");
				NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "V4", "1", "-1");
				break;
			case "Phase1Step6":
				steps_phase_one_completed[5].videos_about_process = true;
				steps_phase_one_completed[5].CheckStepCompletion();
				//asignando la URL dependiendo del paso:
				video_url = video_phase1_step6;
				//registrando la navegacion hacia esta interfaz:
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: V5");
				NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "V5", "1", "-1");
				break;
			case "Phase2Step2":
				steps_phase_two_completed[1].videos_about_process = true;
				steps_phase_two_completed[1].CheckStepCompletion();
				//asignando URL dependiendo del paso:
				video_url = this.video_matizado_phase;
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: V6");
				NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "V6", "7", "-1");
				break;
			case "Phase2Step3":
				steps_phase_two_completed[2].videos_about_process = true;
				steps_phase_two_completed[2].CheckStepCompletion();
				//asignando URL dependiendo del paso:
				video_url = this.video_matizado_phase;
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: V7");
				NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "V7", "9", "-1");
				break;
			case "Phase2Step4":
				steps_phase_two_completed[3].videos_about_process = true;
				steps_phase_two_completed[3].CheckStepCompletion();
				//asignando URL dependiendo del paso:
				video_url = this.video_matizado_phase;
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: V8");
				NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "V8", "10", "-1");
				break;
			case "Phase2Step5":
				steps_phase_two_completed[4].videos_about_process = true;
				steps_phase_two_completed[4].CheckStepCompletion();
				//asignando URL dependiendo del paso:
				video_url = this.video_matizado_phase;
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: V9");
				NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "V9", "11", "-1");
				break;
			case "Phase2Step6":
				steps_phase_two_completed[5].videos_about_process = true;
				steps_phase_two_completed[5].CheckStepCompletion();
				//asignando URL dependiendo del paso:
				video_url = this.video_matizado_phase;
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: V10");
				NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "V10", "12", "-1");
				break;
			case "Phase2Step7":
				steps_phase_two_completed[6].videos_about_process = true;
				steps_phase_two_completed[6].CheckStepCompletion();
				//asignando URL dependiendo del paso:
				video_url = this.video_matizado_phase;
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: V11");
				NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "V11", "13", "-1");
				break;
			case "Phase2Step8":
				steps_phase_two_completed[7].videos_about_process = true;
				steps_phase_two_completed[7].CheckStepCompletion();
				//asignando URL dependiendo del paso:
				video_url = this.video_matizado_phase;
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: V12");
				NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "V12", "14", "-1");
				break;
		default:
				video_url = video_phase1_step2;
				break;
		}

		//guardando el estado en el que se encuentra la aplicacion en este momento:
		SaveDataForStudent ();

		Debug.LogError ("Click en el boton Videos of the step Phase1Step2!!!"); 
		androidJC = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		Debug.LogError("Se ha obtenido el AndroidJavaClass");
		jo = androidJC.GetStatic<AndroidJavaObject>("currentActivity");
		Debug.LogError("Se ha obtenido current activity!!");
		// Accessing the class to call a static method on it
		jc = new AndroidJavaClass("edu.udg.bcds.pintura.arapp.VideoActivity");
		Debug.LogError ("Se ha obtenido StartActivity");
		// Calling a Call method to which the current activity is passed
		
		object[] objs = new object[2];
		objs [0] = jo; //pasando el argumento de la actividad actual que se debe reproducir
		objs [1] = video_url; //definiendo la URL al video que se debe reproducir
		
		jc.CallStatic("Call",objs);

	}//cierra GoToVideoOfEachStep

	/// <summary>
	/// Starts the interface with the activity for taking pictures
	/// </summary>
	/// <param name="interface_from">Interface_from.</param>
	public void GoToTakePictures(string interface_from){
		Debug.LogError ("Llamado al metodo GoToTakePictures!!");
		
		switch (interface_from) {
			case "Phase1Step6":
				Debug.Log("Ingresa al case Phase1Step6... Cargando Interfaz en GoToToolsAndProducts");
				if (current_interface == CurrentInterface.ACTIVITIES_PHASE1_STEP6)
					Destroy (ActivitiesForPhase1Step6_interface_instance);
				else if (current_interface == CurrentInterface.TAKE_AND_SEND_THE_PICTURE)
					Destroy(TakePictureCamera_interface_instance);
				
				current_interface = CurrentInterface.TAKE_PICTURES;
				
				TakePicturesPhase1Step6_interface_instance = Instantiate (InterfaceTakePicturesInfo);
				CanvasTakePictureInterfaceManager cTakePicturesManagerP1S6 = TakePicturesPhase1Step6_interface_instance.GetComponent<CanvasTakePictureInterfaceManager> ();
				cTakePicturesManagerP1S6.image_header_path = "Sprites/tools_and_products/take_pictures";
				cTakePicturesManagerP1S6.title_header_text_path = "Texts/Phase1Step6/9_take_pict_title_header";
				cTakePicturesManagerP1S6.title_intro_content_text_path = "Texts/Phase1Step6/10_take_pict_intro_text";
				cTakePicturesManagerP1S6.tool_one_text_path = "Texts/Phase1Step6/11_take_pict_title_buttons";
				cTakePicturesManagerP1S6.btn_product_one_seguridad_enable = true;
				cTakePicturesManagerP1S6.ruta_img_product_one_seguridad = "Sprites/phase1step6/FichaDatosSeguridad";
				cTakePicturesManagerP1S6.btn_product_one_tecnica_enable = true;
				cTakePicturesManagerP1S6.ruta_img_product_one_tecnica = "Sprites/phase1step6/FichaDatosTecnicos";
				cTakePicturesManagerP1S6.goBackButtonAction += GoToActivitiesPhase1Step6;
				cTakePicturesManagerP1S6.goToPictureProdOne += TakePicturePhase1Step6Desengras;
				//cTakePicturesManagerP1S6.goToSearchProductsTools += GoToSearchDesengrasantePhase1Step6;
				//Atencion: Es muy importante definir esta variable para poder ir hacia atras:
				cTakePicturesManagerP1S6.interfaceGoingBackFrom = interface_from;
				
				//asignando la interfaz activa para controlar el regreso:
				this.interfaceInstanceActive = TakePicturesPhase1Step6_interface_instance;
				string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: PFT1");
				NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "PFT1", "5", "-1");
			break;
		}
	}

	/// <summary>
	/// Takes the picture phase1 step6 desengrasante depending on the type
	/// </summary>
	/// <param name="tipo">Tipo.</param>
	public void TakePicturePhase1Step6Desengras(string tipo){
		Debug.Log("Llamado al metodo para TOMAR FOTO para el producto 1 con tipo=" + tipo);

		if (current_interface == CurrentInterface.TAKE_PICTURES) {
			Destroy(TakePicturesPhase1Step6_interface_instance);
		}

		current_interface = CurrentInterface.TAKE_AND_SEND_THE_PICTURE;

		TakePictureCamera_interface_instance = Instantiate (TestingInterfacePictures);
		ControllerPinturaCameraImage controller_camera = TakePictureCamera_interface_instance.GetComponent<ControllerPinturaCameraImage> ();
		if (controller_camera != null) {
			//asignando el codigo del estudiante:
			controller_camera.codigo_estudiante = this.codigo_estudiante;
			//obteniendo la URL del servidor:
			TextAsset url_server = Resources.Load<TextAsset> ("Texts/00_server_base_path");
			if (url_server != null)
				controller_camera.url_servidor = url_server.text; 

			//asignando el tipo de foto que se va a tomar:
			controller_camera.tipo_img = tipo;

			//asignando la interfaz desde donde se esta llamando:
			controller_camera.interface_coming_from = "Phase1Step6";

			//asignando el metodo delegado que registra si ya se ha completado la actividad:
			controller_camera.ResultPhotoActivity += ResultTakePhotoActivity;

			//asignando la interfaz actual (Esto se utiliza en el OnBackButtonTapped):
			this.interfaceInstanceActive = TakePictureCamera_interface_instance;
		}

	}


	/// <summary>
	/// Goes to search capo coche. Metodo que inicia el modo RA para buscar el capo del carro y que incluye la fase 1 del tutorial
	/// </summary>
	public void GoToSearchCapoCoche(){
		Debug.Log ("Entra al metodo GoToSearchCapoCoche... interfaz: "  + current_interface);
		if (current_interface == CurrentInterface.ACTIVITIES_PHASE1_STEP1) {
			Destroy(ActivitiesForPhase1Step1_interface_instance);
		}

		current_interface = CurrentInterface.AR_SEARCH_CAR_HOOD;

		TurorialSearchCapoCarro_interface_instance = Instantiate (TutorialSearchCapoCarroInterface);
		ControllerBlinkingMarker blinkingMarker = TurorialSearchCapoCarro_interface_instance.GetComponent<ControllerBlinkingMarker> ();

		//Es iportante asignar esta variable para poder controlar los marcadores
		interfaceInstanceActive = TurorialSearchCapoCarro_interface_instance;

		//definiendo que estamos en tutorial fase 1:
		inTutorialPhase1 = true;

		//localizando el marcador que se debe buscar en esta fase del proceso y cargando datos respectivos:
		markerInScene = GameObject.Find ("FrameMarker1");
		ControllerAddInfoInMarker controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		controller_info_marker.onClickSelectButton_tut1 += onClickSelectCapoCarroSearch;
		//asignando el texto que se debe mostrar al momento del feedback:
		blinkingMarker.feedback_info_text_path = feedback_text_path;

		//definiendo los ordenes que controlara esta interfaz:
		order_in_process = new int[1];
		order_in_process [0] = 1; //buscar el capo del carro
		blinkingMarker.ordenes = order_in_process;

		//NOTA: antes de hacer esto es importante que la variable interfaceInstanceActive este asignada:
		//tambien es importante haber asignado la variable inTutorialPhase1
		controller_info_marker.LoadInformationToInterface (true, false, false);

		Debug.LogError ("NOW: Start Blinking");
		//iniciando el proceso blinking:
		blinkingMarker.should_be_blinking = true;

		//colocando en false la informacion adicional por si se le habia dado atras en algun momento en la interfaz:
		info_additional_displayed = false;

		//registrando la navegacion de la interfaz:
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: i14");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "I14", "1", "-1");

	}

	/// <summary>
	/// Goes to search objects tutorial phase2.
	/// Method that is called in order to start tutorial phase 2 - Searching Agua y Jabon (parte 1)
	/// </summary>
	public void GoToSearchObjectsTutorialPhase2(){
		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS) {
			Destroy(ToolsAndProductsPhase1Step2_interface_instance);
		}
		
		current_interface = CurrentInterface.AR_SEARCH_PRODUCTS_TUTORIAL;

		//Destruyendo las demas interfaces que pueda haber en memoria en caso de que las haya para
		//evitar problemas con interfaces que se solapan:
		DestroyInstancesWithTag ("TutorialPhaseTwo");

		//indica que entramos en la fase 2 del tutorial:
		inTutorialPhase2 = true;

		//ahora aqui debo instanciar la interfaz del tutorial 2 y asignarle todas las propiedades:
		TutorialPhaseTwoSearchProd_interface_instance = Instantiate (TutorialSearchProductsPhase2);
		ControllerBlinkingAddInfo blinking_search_phase_two = TutorialPhaseTwoSearchProd_interface_instance.GetComponent<ControllerBlinkingAddInfo> ();

		//hay que asignar la interfaz activa tambien:
		interfaceInstanceActive = TutorialPhaseTwoSearchProd_interface_instance;

		//localizando el marcador que se debe buscar en esta fase del proceso y cargando datos respectivos:
		markerInScene = GameObject.Find ("FrameMarker16");
		ControllerAddInfoInMarker controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();


		//NOTA: antes de hacer esto es importante que la variable interfaceInstanceActive este asignada:
		//tambien es importante haber asignado la variable inTutorialPhase1
		controller_info_marker.LoadInformationToInterface (false,true,false);
		
		Debug.LogError ("NOW: Start Blinking");
		//asignando el texto para el feedback directamente a la interfaz:
		blinking_search_phase_two.feedback_info_text_path = text_feedback;

		//definiendo los ordenes que controlara esta interfaz:
		order_in_process = new int[1];
		order_in_process [0] = 2; //Buscar agua a presion
		blinking_search_phase_two.ordenes = order_in_process;

		//Asignando el metodo que se debe ejecutar cuando se hace click sobre el btn Select de la interfaz
		blinking_search_phase_two.interface_going_from = "Phase1Step2";  //la variable Phase1Step2 permite obtener la interfaz para obtenerla en OnBackButtonTapped
		blinking_search_phase_two.onClickSelectBtn += OnClickSelectAguaPressioSearch;

		//iniciando el proceso blinking:
		blinking_search_phase_two.should_be_blinking = true;

		//registrando la navegacion de la interfaz:
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:I20");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "I20", "1", "-1");

	} //cierra GoToSearchObjectsTutorialPhase2


	/// <summary>
	/// Go to search Agua Jabon
	/// Method that is called in order to start tutorial phase 2 - Searching Agua Jabon (parte 2)
	/// </summary>
	public void GoToSearchAguaJabonTutPhase2(){

		if (current_interface == CurrentInterface.AR_SEARCH_PRODUCTS_TUTORIAL) {
			Destroy(TutorialPhaseTwoSearchProd_interface_instance);
		}

		DestroyInstancesWithTag ("TutorialPhaseTwo");
		
		current_interface = CurrentInterface.AR_SEARCH_AGUA_JABON_PRODUCT_TUTORIAL;
		
		//indica que entramos en la fase 2 del tutorial:
		inTutorialPhase2 = true;
		
		//ahora aqui debo instanciar la interfaz del tutorial 2 y asignarle todas las propiedades:
		TutorialTwoSearchBayeta_interface_instance = Instantiate (TutorialSearchProductsPhase2);
		ControllerBlinkingAddInfo blinking_search_phase_two = TutorialTwoSearchBayeta_interface_instance.GetComponent<ControllerBlinkingAddInfo> ();

		Debug.Log ("---> Nueva Interfaz Instanciada en GoToSearchAguaJabonTutPhase2");

		//hay que asignar la interfaz activa tambien:
		interfaceInstanceActive = TutorialTwoSearchBayeta_interface_instance;
		
		//localizando el marcador que se debe buscar en esta fase del proceso y cargando datos respectivos:
		markerInScene = GameObject.Find ("FrameMarker19");
		ControllerAddInfoInMarker controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		
		//NOTA: antes de hacer esto es importante que la variable interfaceInstanceActive este asignada:
		//tambien es importante haber asignado la variable inTutorialPhase1
		controller_info_marker.LoadInformationToInterface (false,true,false);
		
		Debug.LogError ("NOW: Start Blinking en GoToSearchAguaJabonTutPhase2");
		//asignando el texto para el feedback directamente a la interfaz:
		blinking_search_phase_two.feedback_info_text_path = text_feedback_jabon;
		
		//definiendo los ordenes que controlara esta interfaz:
		order_in_process = new int[1];
		order_in_process [0] = 3; //buscando agua y jabon
		blinking_search_phase_two.ordenes = order_in_process;
		
		//Asignando el metodo que se debe ejecutar cuando se hace click sobre el btn Select de la interfaz
		blinking_search_phase_two.interface_going_from = "Phase1Step2";  //la variable Phase1Step2 permite obtener la interfaz para obtenerla en OnBackButtonTapped
		blinking_search_phase_two.onClickSelectBtn += OnClickSelectAguaJabTutorial;
		
		//iniciando el proceso blinking:
		blinking_search_phase_two.should_be_blinking = true;

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: I21 ");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "I21", "1", "-1");
	} //cierra GoToSearchObjectsTutorialPhase2

	public void GoToSearchBayetaPhase1Step2() {
			
		if (current_interface == CurrentInterface.AR_SEARCH_AGUA_JABON_PRODUCT_TUTORIAL) {
			Destroy(TutorialPhaseTwoSearchProd_interface_instance);
		}
		
		DestroyInstancesWithTag ("TutorialPhaseTwo");
		
		current_interface = CurrentInterface.AR_SEARCH_BAYETA_PRODUCT_TUTORIAL;
		
		//indica que entramos en la fase 2 del tutorial:
		inTutorialPhase2 = true;
		
		//ahora aqui debo instanciar la interfaz del tutorial 2 y asignarle todas las propiedades:
		TutorialTwoSearchBayeta_interface_instance = Instantiate (TutorialSearchProductsPhase2);
		ControllerBlinkingAddInfo blinking_search_phase_two = TutorialTwoSearchBayeta_interface_instance.GetComponent<ControllerBlinkingAddInfo> ();
		
		Debug.Log ("Nueva Interfaz Instanciada!!");
		
		//hay que asignar la interfaz activa tambien:
		interfaceInstanceActive = TutorialTwoSearchBayeta_interface_instance;
		
		//localizando el marcador que se debe buscar en esta fase del proceso y cargando datos respectivos:
		markerInScene = GameObject.Find ("FrameMarker21");
		ControllerAddInfoInMarker controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		
		//NOTA: antes de hacer esto es importante que la variable interfaceInstanceActive este asignada:
		//tambien es importante haber asignado la variable inTutorialPhase1
		controller_info_marker.LoadInformationToInterface (false,true,false);
		
		Debug.LogError ("NOW: Start Blinking en GoToSearchBayetaPhase1Step2");
		//asignando el texto para el feedback directamente a la interfaz:
		blinking_search_phase_two.feedback_info_text_path = text_feedback_bayeta;
		
		//definiendo los ordenes que controlara esta interfaz:
		order_in_process = new int[1];
		order_in_process [0] = 4; //Buscando la bayeta
		blinking_search_phase_two.ordenes = order_in_process;
		
		//Asignando el metodo que se debe ejecutar cuando se hace click sobre el btn Select de la interfaz
		blinking_search_phase_two.interface_going_from = "Phase1Step2";  //la variable Phase1Step2 permite obtener la interfaz para obtenerla en OnBackButtonTapped
		blinking_search_phase_two.onClickSelectBtn += OnClickSelectBayetaSearch;
		
		//iniciando el proceso blinking:
		blinking_search_phase_two.should_be_blinking = true;

		//registrando la navegacion de la interfaz:
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: I22");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "I22", "1", "-1");
	}

	/// <summary>
	/// Method that is called in order to start the AR mode for searching the tools of Phase 1 - Step3 (agua a presion)
	/// </summary>
	public void GoToSearchAguaPresionPhase1Step3(){
		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS) {
			Destroy(ToolsAndProductsPhase1Step3_interface_instance);
		}
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE1_STEP3;
		
		//indica que entramos en la fase 2 del tutorial:
		inTutorialPhase2 = false;
		Debug.Log ("--> Iniciando modo RA en GoToSearchAguaPresionPhase1Step3");
		//indica que se ingresa al modo RA fuera de los tutoriales:
		in_RA_mode = true;
		
		//ahora aqui debo instanciar la interfaz del tutorial 2 y asignarle todas las propiedades:
		AR_Mode_Search_interface_instance = Instantiate (AR_Mode_interface);
		ControllerBlinkingARGeneric blinking_search_phase1step3 = AR_Mode_Search_interface_instance.GetComponent<ControllerBlinkingARGeneric> ();
		
		Debug.Log ("Nueva Interfaz Instanciada en GoToSearchToolsPhase1Step3!!");
		
		//hay que asignar la interfaz activa tambien:
		interfaceInstanceActive = AR_Mode_Search_interface_instance;
		
		//localizando el marcador que se debe buscar en esta fase del proceso y cargando datos respectivos:
		markerInScene = GameObject.Find ("FrameMarker16");
		ControllerAddInfoInMarker controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		
		//NOTA: antes de hacer esto es importante que la variable interfaceInstanceActive este asignada:
		//tambien es importante haber asignado la variable inTutorialPhase1
		controller_info_marker.LoadInformationToInterface (false,false,true);
		
		Debug.LogError ("NOW: Start Blinking en GoToSearchToolsPhase1Step3");
		//asignando el texto para el feedback directamente a la interfaz:
		blinking_search_phase1step3.feedback_info_text_path = feedback_phase1step3_agua;
		
		//definiendo los ordenes que controlara esta interfaz:
		order_in_process = new int[1];
		order_in_process [0] = 5; //buscando agua a presion paso 3
		blinking_search_phase1step3.ordenes = order_in_process;
		
		//Asignando el metodo que se debe ejecutar cuando se hace click sobre el btn Select de la interfaz
		blinking_search_phase1step3.interface_going_from = "Phase1Step3";  //la variable Phase1Step3 permite obtener la interfaz para obtenerla en OnBackButtonTapped
		blinking_search_phase1step3.onClickSelectBtn += OnClickSelectAguaPresion;
		
		//iniciando el proceso blinking:
		blinking_search_phase1step3.should_be_blinking = true;

		//registrando la navegacion de la interfaz:
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion I23");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "I23", "2", "-1");

	} //cierra metodo de agua a presion


	/// <summary>
	/// Method that is called in order to start the AR mode for searching the tools of Phase 1 - Step3 (papel absorbente)
	/// </summary>
	public void GoToSearchPapelAbsorbentePhase1Step3(){

		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS) {
			Destroy (ToolsAndProductsPhase1Step3_interface_instance);
		} else if (current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP3)
			Destroy (AR_Mode_Search_interface_instance);
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE1_STEP3;
		
		//indica que entramos en la fase 2 del tutorial:
		inTutorialPhase2 = false;

		Debug.Log ("--> Iniciando modo RA en GoToSearchPapelAbsorbentePhase1Step3");
		//indica que se ingresa al modo RA fuera de los tutoriales:
		in_RA_mode = true;
		
		//ahora aqui debo instanciar la interfaz del tutorial 2 y asignarle todas las propiedades:
		AR_Mode_Search_interface_instance = Instantiate (AR_Mode_interface);
		ControllerBlinkingARGeneric blinking_search_phase1step3 = AR_Mode_Search_interface_instance.GetComponent<ControllerBlinkingARGeneric> ();
		
		Debug.Log ("Nueva Interfaz Instanciada en GoToSearchPapelAbsorbentePhase1Step3!!");
		
		//hay que asignar la interfaz activa tambien:
		interfaceInstanceActive = AR_Mode_Search_interface_instance;
		
		//localizando el marcador que se debe buscar en esta fase del proceso y cargando datos respectivos:
		markerInScene = GameObject.Find ("FrameMarker25");
		ControllerAddInfoInMarker controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		
		//NOTA: antes de hacer esto es importante que la variable interfaceInstanceActive este asignada:
		//tambien es importante haber asignado la variable inTutorialPhase1
		controller_info_marker.LoadInformationToInterface (false,false,true);
		
		Debug.LogError ("NOW: Start Blinking en GoToSearchPapelAbsorbentePhase1Step3");
		//asignando el texto para el feedback directamente a la interfaz:
		blinking_search_phase1step3.feedback_info_text_path = text_feedback_marker25_dc3430;
		
		//definiendo los ordenes que controlara esta interfaz:
		order_in_process = new int[1];
		order_in_process [0] = 6; //buscando papel absorbente paso 3
		blinking_search_phase1step3.ordenes = order_in_process;
		
		//Asignando el metodo que se debe ejecutar cuando se hace click sobre el btn Select de la interfaz
		blinking_search_phase1step3.interface_going_from = "Phase1Step3";  //la variable Phase1Step2 permite obtener la interfaz para obtenerla en OnBackButtonTapped
		blinking_search_phase1step3.onClickSelectBtn += OnClickSelectPapelAbsorbente;
		
		//iniciando el proceso blinking:
		blinking_search_phase1step3.should_be_blinking = true;

		//registrando la navegacion de la interfaz:
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: I24");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "I24", "2", "-1");
		
	}//cierra GoToSearchPapelAbsorbente


	/// <summary>
	/// Method that is called in order to start the AR mode for searching the tools of Phase 1 - Step4 (lija fina)
	/// </summary>
	public void GoToSearchLijaFinaPhase1Step4(){
		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS) {
			Destroy(ToolsAndProductsPhase1Step4_interface_instance);
		}
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE1_STEP4;
		
		//indica que entramos en la fase 2 del tutorial:
		inTutorialPhase2 = false;
		Debug.Log ("--> Iniciando modo RA en GoToSearchAguaPresionPhase1Step4");
		//indica que se ingresa al modo RA fuera de los tutoriales:
		in_RA_mode = true;
		
		//ahora aqui debo instanciar la interfaz del tutorial 2 y asignarle todas las propiedades:
		AR_Mode_Search_interface_instance = Instantiate (AR_Mode_interface);
		ControllerBlinkingARGeneric blinking_search_phase1step4 = AR_Mode_Search_interface_instance.GetComponent<ControllerBlinkingARGeneric> ();
		
		Debug.Log ("Nueva Interfaz Instanciada en GoToSearchToolsPhase1Step4!!");
		
		//hay que asignar la interfaz activa tambien:
		interfaceInstanceActive = AR_Mode_Search_interface_instance;
		
		//localizando el marcador que se debe buscar en esta fase del proceso y cargando datos respectivos:
		markerInScene = GameObject.Find ("FrameMarker45");
		ControllerAddInfoInMarker controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		controller_info_marker.image_marker_path = "Sprites/markers/frameMarker45_46_TwoOptions";
		controller_info_marker.image_marker_real_path = "Sprites/phase1step4/FrameMarker45_46_TwoOptionsImage";
		
		//NOTA: antes de hacer esto es importante que la variable interfaceInstanceActive este asignada:
		//tambien es importante haber asignado la variable inTutorialPhase1
		controller_info_marker.LoadInformationToInterface (false,false,true);
		
		Debug.LogError ("NOW: Start Blinking en GoToSearchToolsPhase1Step4");
		//asignando el texto para el feedback directamente a la interfaz:
		blinking_search_phase1step4.feedback_info_text_path = text_feedback_marker45_lija;
		
		//definiendo los ordenes que controlara esta interfaz:
		order_in_process = new int[1];
		order_in_process [0] = 7; //Buscando la lija fina - abrasivo de lijado
		blinking_search_phase1step4.ordenes = order_in_process;
		
		//Asignando el metodo que se debe ejecutar cuando se hace click sobre el btn Select de la interfaz
		blinking_search_phase1step4.interface_going_from = "Phase1Step4";  //la variable Phase1Step3 permite obtener la interfaz para obtenerla en OnBackButtonTapped
		blinking_search_phase1step4.onClickSelectBtn += OnClickSelectLijaFina;
		
		//iniciando el proceso blinking:
		blinking_search_phase1step4.should_be_blinking = true;

		//registrando la navegacion de la interfaz:
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: I25");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "I25", "3", "-1");
		
	} //cierra metodo GoToSearchLijaFinaPhase1Step4


	/// <summary>
	/// Method that is called in order to start the AR mode for searching the tools of Phase 1 - Step5 (martillo)
	/// </summary>
	public void GoToSearchMartilloPhase1Step5(){

		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS) {
			Destroy(ToolsAndProductsPhase1Step5_interface_instance);
		}
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE1_STEP5;
		
		//indica que entramos en la fase 2 del tutorial:
		inTutorialPhase2 = false;
		Debug.Log ("--> Iniciando modo RA en GoToSearchMartilloPhase1Step5");
		//indica que se ingresa al modo RA fuera de los tutoriales:
		in_RA_mode = true;
		
		//ahora aqui debo instanciar la interfaz del tutorial 2 y asignarle todas las propiedades:
		AR_Mode_Search_interface_instance = Instantiate (AR_Mode_interface);
		ControllerBlinkingARGeneric blinking_search_phase1step5 = AR_Mode_Search_interface_instance.GetComponent<ControllerBlinkingARGeneric> ();
		
		Debug.Log ("Nueva Interfaz Instanciada en GoToSearchToolsPhase1Step4!!");
		
		//hay que asignar la interfaz activa tambien:
		interfaceInstanceActive = AR_Mode_Search_interface_instance;
		
		//localizando el marcador que se debe buscar en esta fase del proceso y cargando datos respectivos:
		markerInScene = GameObject.Find ("FrameMarker100");
		ControllerAddInfoInMarker controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		
		//NOTA: antes de hacer esto es importante que la variable interfaceInstanceActive este asignada:
		//tambien es importante haber asignado la variable inTutorialPhase1
		controller_info_marker.LoadInformationToInterface (false,false,true);
		
		Debug.LogError ("NOW: Start Blinking en GoToSearchMartilloPhase1Step5");
		//asignando el texto para el feedback directamente a la interfaz:
		blinking_search_phase1step5.feedback_info_text_path = text_feedback_marker6_martillo;
		
		//definiendo los ordenes que controlara esta interfaz:
		order_in_process = new int[1];
		order_in_process [0] = 8; //buscando el martillo
		blinking_search_phase1step5.ordenes = order_in_process;
		
		//Asignando el metodo que se debe ejecutar cuando se hace click sobre el btn Select de la interfaz
		blinking_search_phase1step5.interface_going_from = "Phase1Step5";  //la variable Phase1Step3 permite obtener la interfaz para obtenerla en OnBackButtonTapped
		blinking_search_phase1step5.onClickSelectBtn += OnClickSelectMartillo;
		
		//iniciando el proceso blinking:
		blinking_search_phase1step5.should_be_blinking = true;

		//registrando la navegacion de la interfaz:
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: I26");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "I26", "4", "-1");
		
	} //cierra metodo de busqueda del martillo


	/// <summary>
	/// Method that is called in order to start the AR mode for searching the tools of Phase 1 - Step6 (desengrasante)
	/// </summary>
	public void GoToSearchDesengrasantePhase1Step6(){
		
		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS) {
			Destroy(ToolsAndProductsPhase1Step6_interface_instance);
		}
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE1_STEP6;
		
		//indica que entramos en la fase 2 del tutorial:
		inTutorialPhase2 = false;
		Debug.Log ("--> Iniciando modo RA en GoToSearchDesengrasantePhase1Step6");
		//indica que se ingresa al modo RA fuera de los tutoriales:
		in_RA_mode = true;
		
		//ahora aqui debo instanciar la interfaz del tutorial 2 y asignarle todas las propiedades:
		AR_Mode_Search_interface_instance = Instantiate (AR_Mode_interface);
		ControllerBlinkingARGeneric blinking_search_phase1step6 = AR_Mode_Search_interface_instance.GetComponent<ControllerBlinkingARGeneric> ();
		
		Debug.Log ("Nueva Interfaz Instanciada en GoToSearchDesengrasantePhase1Step6!!");
		
		//hay que asignar la interfaz activa tambien:
		interfaceInstanceActive = AR_Mode_Search_interface_instance;
		
		//localizando el marcador que se debe buscar en esta fase del proceso y cargando datos respectivos:
		markerInScene = GameObject.Find ("FrameMarker26");
		ControllerAddInfoInMarker controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		
		//NOTA: antes de hacer esto es importante que la variable interfaceInstanceActive este asignada:
		//tambien es importante haber asignado la variable inTutorialPhase1
		controller_info_marker.LoadInformationToInterface (false,false,true);
		
		Debug.LogError ("NOW: Start Blinking en GoToSearchDesengrasantePhase1Step6");
		//asignando el texto para el feedback directamente a la interfaz:
		blinking_search_phase1step6.feedback_info_text_path = text_feedback_marker26_desengras;
		
		//definiendo los ordenes que controlara esta interfaz:
		order_in_process = new int[2];
		order_in_process [0] = 9; //buscando el DA93
		blinking_search_phase1step6.ordenes = order_in_process;
		
		//Asignando el metodo que se debe ejecutar cuando se hace click sobre el btn Select de la interfaz
		blinking_search_phase1step6.interface_going_from = "Phase1Step6";  //la variable Phase1Step3 permite obtener la interfaz para obtenerla en OnBackButtonTapped
		blinking_search_phase1step6.onClickSelectBtn += OnClickSelectDesengrasante;
		
		//iniciando el proceso blinking:
		blinking_search_phase1step6.should_be_blinking = true;

		//registrando la navegacion de la interfaz:
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: I27");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "I27", "5", "-1");
		
	} //cierra metodo de busqueda del desengrasante


	/// <summary>
	/// Method that is called in order to start the AR mode for searching the tools of Phase 1 - Step6 (desengrasante)
	/// </summary>
	public void GoToSearchBayetaPhase1Step6(){
		
		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS) {
			Destroy(ToolsAndProductsPhase1Step6_interface_instance);
		} else if (current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP6)
			Destroy (AR_Mode_Search_interface_instance);
	
		current_interface = CurrentInterface.AR_SEARCH_PHASE1_STEP6;
		
		//indica que entramos en la fase 2 del tutorial:
		inTutorialPhase2 = false;
		Debug.Log ("--> Iniciando modo RA en GoToSearchBayetaPhase1Step6");
		//indica que se ingresa al modo RA fuera de los tutoriales:
		in_RA_mode = true;
		
		//ahora aqui debo instanciar la interfaz del tutorial 2 y asignarle todas las propiedades:
		AR_Mode_Search_interface_instance = Instantiate (AR_Mode_interface);
		ControllerBlinkingARGeneric blinking_search_phase1step6 = AR_Mode_Search_interface_instance.GetComponent<ControllerBlinkingARGeneric> ();
		
		Debug.Log ("Nueva Interfaz Instanciada en GoToSearchBayetaPhase1Step6!!");
		
		//hay que asignar la interfaz activa tambien:
		interfaceInstanceActive = AR_Mode_Search_interface_instance;
		
		//localizando el marcador que se debe buscar en esta fase del proceso y cargando datos respectivos:
		markerInScene = GameObject.Find ("FrameMarker25");
		ControllerAddInfoInMarker controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		
		//NOTA: antes de hacer esto es importante que la variable interfaceInstanceActive este asignada:
		//tambien es importante haber asignado la variable inTutorialPhase1
		controller_info_marker.LoadInformationToInterface (false,false,true);
		
		Debug.LogError ("NOW: Start Blinking en GoToSearchBayetaPhase1Step6");
		//asignando el texto para el feedback directamente a la interfaz:
		blinking_search_phase1step6.feedback_info_text_path = text_feedback_marker26_desengras;
		
		//definiendo los ordenes que controlara esta interfaz:
		order_in_process = new int[1];
		order_in_process [0] = 10; //Pide ayuda buscando la bayeta
		blinking_search_phase1step6.ordenes = order_in_process;
		
		//Asignando el metodo que se debe ejecutar cuando se hace click sobre el btn Select de la interfaz
		blinking_search_phase1step6.interface_going_from = "Phase1Step6";  //la variable Phase1Step3 permite obtener la interfaz para obtenerla en OnBackButtonTapped
		blinking_search_phase1step6.onClickSelectBtn += OnClickSelectBayetaStep6;
		
		//iniciando el proceso blinking:
		blinking_search_phase1step6.should_be_blinking = true;

		//registrando la navegacion de la interfaz:
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: I28");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "I28", "5", "-1");
		
	} //cierra metodo de busqueda de la bayeta segunda parte (paso 6)


	/// <summary>
	/// Method that is called in order to start the AR mode for searching the tools of Phase 2 - Step2 (MATIZADO - Proteccion de la superficie) Parte 1 - Cinta de enmascarar
	/// </summary>
	public void GoToSearchCintaEnmascPhase2Step2(){
		
		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS) {
			Destroy(ToolsAndProductsPhase2Step2_interface_instance);
		}
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE2_STEP2;
		
		//indica que entramos en la fase 2 del tutorial:
		inTutorialPhase2 = false;
		Debug.Log ("--> Iniciando modo RA en GoToSearchCintaEnmascPhase2Step2");
		//indica que se ingresa al modo RA fuera de los tutoriales:
		in_RA_mode = true;
		
		//ahora aqui debo instanciar la interfaz del tutorial 2 y asignarle todas las propiedades:
		AR_Mode_Search_interface_instance = Instantiate (AR_Mode_interface);
		ControllerBlinkingARGeneric blinking_search_phase2step2 = AR_Mode_Search_interface_instance.GetComponent<ControllerBlinkingARGeneric> ();
		
		Debug.Log ("Nueva Interfaz Instanciada en GoToSearchCintaEnmascPhase2Step2!!");
		
		//hay que asignar la interfaz activa tambien:
		interfaceInstanceActive = AR_Mode_Search_interface_instance;
		
		//localizando el marcador que se debe buscar en esta fase del proceso y cargando datos respectivos:
		markerInScene = GameObject.Find ("FrameMarker65");
		ControllerAddInfoInMarker controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		
		//NOTA: antes de hacer esto es importante que la variable interfaceInstanceActive este asignada:
		//tambien es importante haber asignado la variable inTutorialPhase1
		controller_info_marker.LoadInformationToInterface (false,false,true);
		
		Debug.LogError ("NOW: Start Blinking en GoToSearchCintaEnmascPhase2Step2");
		//asignando el texto para el feedback directamente a la interfaz:
		blinking_search_phase2step2.feedback_info_text_path = text_feedback_marker65_cinta;
		
		//definiendo los ordenes que controlara esta interfaz:
		order_in_process = new int[1];
		order_in_process [0] = 11; //buscando la cinta de enmascarar
		blinking_search_phase2step2.ordenes = order_in_process;
		
		//Asignando el metodo que se debe ejecutar cuando se hace click sobre el btn Select de la interfaz
		blinking_search_phase2step2.interface_going_from = "Phase2Step2";  //la variable Phase1Step3 permite obtener la interfaz para obtenerla en OnBackButtonTapped
		blinking_search_phase2step2.onClickSelectBtn += OnClickSelectCintaEmascarar;
		
		//iniciando el proceso blinking:
		blinking_search_phase2step2.should_be_blinking = true;

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: I44");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "I44", "13", "-1");

		
	} //cierra metodo GoToSearchCintaEnmascPhase2Step2

	/// <summary>
	/// Method that is called in order to start the AR mode for searching the tools of Phase 2 - Step2 (MATIZADO - Proteccion de la superficie) Parte 2: papel de enmascarar
	/// </summary>
	public void GoToSearchPapelEnmascPhase2Step2(){

		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS) {
			Destroy(ToolsAndProductsPhase2Step2_interface_instance);
		} else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP2)
			Destroy (AR_Mode_Search_interface_instance);
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE2_STEP2;
		
		//indica que entramos en la fase 2 del tutorial:
		inTutorialPhase2 = false;
		Debug.Log ("--> Iniciando modo RA en GoToSearchPapelEnmascPhase2Step2");
		//indica que se ingresa al modo RA fuera de los tutoriales:
		in_RA_mode = true;
		
		//ahora aqui debo instanciar la interfaz del tutorial 2 y asignarle todas las propiedades:
		AR_Mode_Search_interface_instance = Instantiate (AR_Mode_interface);
		ControllerBlinkingARGeneric blinking_search_phase2step2_second = AR_Mode_Search_interface_instance.GetComponent<ControllerBlinkingARGeneric> ();
		
		Debug.Log ("Nueva Interfaz Instanciada en GoToSearchPapelEnmascPhase2Step2!!");
		
		//hay que asignar la interfaz activa tambien:
		interfaceInstanceActive = AR_Mode_Search_interface_instance;
		
		//localizando el marcador que se debe buscar en esta fase del proceso y cargando datos respectivos:
		markerInScene = GameObject.Find ("FrameMarker69");
		ControllerAddInfoInMarker controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		
		//NOTA: antes de hacer esto es importante que la variable interfaceInstanceActive este asignada:
		//tambien es importante haber asignado la variable inTutorialPhase1
		controller_info_marker.LoadInformationToInterface (false,false,true);
		
		Debug.LogError ("NOW: Start Blinking en GoToSearchBayetaPhase1Step6");
		//asignando el texto para el feedback directamente a la interfaz:
		blinking_search_phase2step2_second.feedback_info_text_path = text_feedback_marker65_cinta;
		
		//definiendo los ordenes que controlara esta interfaz:
		order_in_process = new int[1];
		order_in_process [0] = 12; //buscando el papel de enmascarar
		blinking_search_phase2step2_second.ordenes = order_in_process;
		
		//Asignando el metodo que se debe ejecutar cuando se hace click sobre el btn Select de la interfaz
		blinking_search_phase2step2_second.interface_going_from = "Phase2Step2";  //la variable Phase1Step3 permite obtener la interfaz para obtenerla en OnBackButtonTapped
		blinking_search_phase2step2_second.onClickSelectBtn += OnClickSelectPapelEnmascPhase2Step2;
		
		//iniciando el proceso blinking:
		blinking_search_phase2step2_second.should_be_blinking = true;

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: I45");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "I45", "7", "-1");

	} //cierra GoToSearchPapelEnmascPhase2Step2

	public void GoToSearchEsponjaP320Phase2Step3(){

		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS) {
			Destroy(ToolsAndProductsPhase2Step3_interface_instance);
		} else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP3)
			Destroy (AR_Mode_Search_interface_instance);
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE2_STEP3;
		
		//indica que entramos en la fase 2 del tutorial:
		inTutorialPhase2 = false;
		Debug.Log ("--> Iniciando modo RA en GoToSearchEsponjaP320Phase2Step3");
		//indica que se ingresa al modo RA fuera de los tutoriales:
		in_RA_mode = true;
		
		//ahora aqui debo instanciar la interfaz del tutorial 2 y asignarle todas las propiedades:
		AR_Mode_Search_interface_instance = Instantiate (AR_Mode_interface);
		ControllerBlinkingARGeneric blinking_search_phase2step3 = AR_Mode_Search_interface_instance.GetComponent<ControllerBlinkingARGeneric> ();
		
		Debug.Log ("Nueva Interfaz Instanciada en GoToSearchPapelEnmascPhase2Step2!!");
		
		//hay que asignar la interfaz activa tambien:
		interfaceInstanceActive = AR_Mode_Search_interface_instance;
		
		//localizando el marcador que se debe buscar en esta fase del proceso y cargando datos respectivos:
		markerInScene = GameObject.Find ("FrameMarker45");
		ControllerAddInfoInMarker controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		controller_info_marker.image_marker_path = "Sprites/markers/frameMarker45_esponja_p320";
		controller_info_marker.image_marker_real_path = "Sprites/phase2step3/FrameMarker45_esponja_p320_icon";
		
		//NOTA: antes de hacer esto es importante que la variable interfaceInstanceActive este asignada:
		//tambien es importante haber asignado la variable inTutorialPhase1
		controller_info_marker.LoadInformationToInterface (false,false,true);
		
		Debug.LogError ("NOW: Start Blinking en GoToSearchEsponjaP320Phase2Step3");
		//asignando el texto para el feedback directamente a la interfaz:
		blinking_search_phase2step3.feedback_info_text_path = text_feedback_phase2step3;
		
		//definiendo los ordenes que controlara esta interfaz:
		order_in_process = new int[1];
		order_in_process [0] = 13; //buscando esponja P320
		blinking_search_phase2step3.ordenes = order_in_process;
		
		//Asignando el metodo que se debe ejecutar cuando se hace click sobre el btn Select de la interfaz
		blinking_search_phase2step3.interface_going_from = "Phase2Step3";  //la variable Phase1Step3 permite obtener la interfaz para obtenerla en OnBackButtonTapped
		blinking_search_phase2step3.onClickSelectBtn += OnClickSelectEsponjaPhase2Step3;
		
		//iniciando el proceso blinking:
		blinking_search_phase2step3.should_be_blinking = true;

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: I46");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "I46", "9", "-1");

	} //cierra GoToSearchEsponjaP320Phase2Step3


	public void GoToSearchObjetoLimpiezaPhase2Step3(){

		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS) {
			Destroy(ToolsAndProductsPhase2Step3_interface_instance);
		} else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP3)
			Destroy (AR_Mode_Search_interface_instance);
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE2_STEP3;
		
		//indica que entramos en la fase 2 del tutorial:
		inTutorialPhase2 = false;
		Debug.Log ("--> Iniciando modo RA en GoToSearchObjetoLimpiezaPhase2Step3");
		//indica que se ingresa al modo RA fuera de los tutoriales:
		in_RA_mode = true;
		
		//ahora aqui debo instanciar la interfaz del tutorial 2 y asignarle todas las propiedades:
		AR_Mode_Search_interface_instance = Instantiate (AR_Mode_interface);
		ControllerBlinkingARGeneric blinking_search_phase2step3_second = AR_Mode_Search_interface_instance.GetComponent<ControllerBlinkingARGeneric> ();
		
		Debug.Log ("Nueva Interfaz Instanciada en GoToSearchObjetoLimpiezaPhase2Step3!!");
		
		//hay que asignar la interfaz activa tambien:
		interfaceInstanceActive = AR_Mode_Search_interface_instance;
		
		//localizando el marcador que se debe buscar en esta fase del proceso y cargando datos respectivos:
		markerInScene = GameObject.Find ("FrameMarker25");
		ControllerAddInfoInMarker controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		//ATENCION: En este caso en particular se configuran otras imagenes al marcador 25 porque en este paso se
		//permite seleccionar varias opciones concretamente el marcador 25,24 y 23, por lo tanto se va a mostrar
		//una imagen compuesta y diferente  para que el estudiante sepa que puede seleccionar alguno de los elementos:
		controller_info_marker.image_marker_path = "Sprites/markers/frameMarker25_24_23_elemento_limpieza";
		controller_info_marker.image_marker_real_path = "Sprites/phase2step3/FrameMarker25_24_23_limpieza";
				
		//NOTA: antes de hacer esto es importante que la variable interfaceInstanceActive este asignada:
		//tambien es importante haber asignado la variable inTutorialPhase1
		controller_info_marker.LoadInformationToInterface (false,false,true);
		
		Debug.LogError ("NOW: Start Blinking en GoToSearchObjetoLimpiezaPhase2Step3");
		//asignando el texto para el feedback directamente a la interfaz:
		blinking_search_phase2step3_second.feedback_info_text_path = text_feedback_phase2step3;
		
		//definiendo los ordenes que controlara esta interfaz:
		order_in_process = new int[1];
		order_in_process [0] = 14; //buscando objeto de limpieza paso3 fase2
		blinking_search_phase2step3_second.ordenes = order_in_process;
		
		//Asignando el metodo que se debe ejecutar cuando se hace click sobre el btn Select de la interfaz
		blinking_search_phase2step3_second.interface_going_from = "Phase2Step3";  //la variable Phase1Step3 permite obtener la interfaz para obtenerla en OnBackButtonTapped
		blinking_search_phase2step3_second.onClickSelectBtn += OnClickSelectObjetoLimpiezaPhase2Step3;
		
		//iniciando el proceso blinking:
		blinking_search_phase2step3_second.should_be_blinking = true;

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: I47");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "I47", "9", "-1");

	} //cierra metodo GoToSearchObjetoLimpiezaPhase2Step3


	public void GoToSearchEsponjaP400Phase2Step4(){

		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS) {
			Destroy(ToolsAndProductsPhase2Step4_interface_instance);
		} else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP4)
			Destroy (AR_Mode_Search_interface_instance);
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE2_STEP4;
		
		//indica que entramos en la fase 2 del tutorial:
		inTutorialPhase2 = false;
		Debug.Log ("--> Iniciando modo RA en GoToSearchEsponjaP400Phase2Step4");
		//indica que se ingresa al modo RA fuera de los tutoriales:
		in_RA_mode = true;
		
		//ahora aqui debo instanciar la interfaz del tutorial 2 y asignarle todas las propiedades:
		AR_Mode_Search_interface_instance = Instantiate (AR_Mode_interface);
		ControllerBlinkingARGeneric blinking_search_phase2step4 = AR_Mode_Search_interface_instance.GetComponent<ControllerBlinkingARGeneric> ();
		
		Debug.Log ("Nueva Interfaz Instanciada en GoToSearchEsponjaP400Phase2Step4!!");
		
		//hay que asignar la interfaz activa tambien:
		interfaceInstanceActive = AR_Mode_Search_interface_instance;
		
		//localizando el marcador que se debe buscar en esta fase del proceso y cargando datos respectivos:
		markerInScene = GameObject.Find ("FrameMarker46");
		ControllerAddInfoInMarker controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		controller_info_marker.image_marker_path = "Sprites/markers/frameMarker46_esponja_p400";
		controller_info_marker.image_marker_real_path = "Sprites/phase2step4/FrameMarker46_esponja_p400_icon";
		
		//NOTA: antes de hacer esto es importante que la variable interfaceInstanceActive este asignada:
		//tambien es importante haber asignado la variable inTutorialPhase1
		controller_info_marker.LoadInformationToInterface (false,false,true);
		
		Debug.LogError ("NOW: Start Blinking en GoToSearchEsponjaP400Phase2Step4");
		//asignando el texto para el feedback directamente a la interfaz:
		blinking_search_phase2step4.feedback_info_text_path = text_feedback_phase2step4;
		
		//definiendo los ordenes que controlara esta interfaz:
		order_in_process = new int[1];
		order_in_process [0] = 15; //buscando esponja P400
		blinking_search_phase2step4.ordenes = order_in_process;
		
		//Asignando el metodo que se debe ejecutar cuando se hace click sobre el btn Select de la interfaz
		blinking_search_phase2step4.interface_going_from = "Phase2Step4";  //la variable Phase1Step3 permite obtener la interfaz para obtenerla en OnBackButtonTapped
		blinking_search_phase2step4.onClickSelectBtn += OnClickSelectEsponjaP400Phase2Step4;
		
		//iniciando el proceso blinking:
		blinking_search_phase2step4.should_be_blinking = true;

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: I48");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "I48", "10", "-1");
	} //cierra  GoToSearchEsponjaP400Phase2Step4


	public void GoToSearchObjetoLimpiezaPhase2Step4(){

		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS) {
			Destroy(ToolsAndProductsPhase2Step4_interface_instance);
		} else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP4)
			Destroy (AR_Mode_Search_interface_instance);
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE2_STEP4;
		
		//indica que entramos en la fase 2 del tutorial:
		inTutorialPhase2 = false;
		Debug.Log ("--> Iniciando modo RA en GoToSearchObjetoLimpiezaPhase2Step4");
		//indica que se ingresa al modo RA fuera de los tutoriales:
		in_RA_mode = true;
		
		//ahora aqui debo instanciar la interfaz del tutorial 2 y asignarle todas las propiedades:
		AR_Mode_Search_interface_instance = Instantiate (AR_Mode_interface);
		ControllerBlinkingARGeneric blinking_search_phase2step4_second = AR_Mode_Search_interface_instance.GetComponent<ControllerBlinkingARGeneric> ();
		
		Debug.Log ("Nueva Interfaz Instanciada en GoToSearchObjetoLimpiezaPhase2Step4!!");
		
		//hay que asignar la interfaz activa tambien:
		interfaceInstanceActive = AR_Mode_Search_interface_instance;
		
		//localizando el marcador que se debe buscar en esta fase del proceso y cargando datos respectivos:
		markerInScene = GameObject.Find ("FrameMarker25");
		ControllerAddInfoInMarker controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		//ATENCION: En este caso en particular se configuran otras imagenes al marcador 25 porque en este paso se
		//permite seleccionar varias opciones concretamente el marcador 25,24 y 23, por lo tanto se va a mostrar
		//una imagen compuesta y diferente  para que el estudiante sepa que puede seleccionar alguno de los elementos:
		controller_info_marker.image_marker_path = "Sprites/markers/frameMarker25_24_23_elemento_limpieza";
		controller_info_marker.image_marker_real_path = "Sprites/phase2step4/FrameMarker25_24_23_limpieza";
		
		//NOTA: antes de hacer esto es importante que la variable interfaceInstanceActive este asignada:
		//tambien es importante haber asignado la variable inTutorialPhase1
		controller_info_marker.LoadInformationToInterface (false,false,true);
		
		Debug.LogError ("NOW: Start Blinking en GoToSearchObjetoLimpiezaPhase2Step3");
		//asignando el texto para el feedback directamente a la interfaz:
		blinking_search_phase2step4_second.feedback_info_text_path = text_feedback_phase2step4;
		
		//definiendo los ordenes que controlara esta interfaz:
		order_in_process = new int[1];
		order_in_process [0] = 16; //Buscando objeto de limpieza paso 4 fase2

		blinking_search_phase2step4_second.ordenes = order_in_process;
		
		//Asignando el metodo que se debe ejecutar cuando se hace click sobre el btn Select de la interfaz
		blinking_search_phase2step4_second.interface_going_from = "Phase2Step4";  //la variable Phase1Step3 permite obtener la interfaz para obtenerla en OnBackButtonTapped
		blinking_search_phase2step4_second.onClickSelectBtn += OnClickSelectObjetoLimpiezaPhase2Step4;
		
		//iniciando el proceso blinking:
		blinking_search_phase2step4_second.should_be_blinking = true;

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: I49");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "I49", "10", "-1");
	} //cierra GoToSearchObjetoLimpiezaPhase2Step4


	public void GoToSearchRotoOrbitalPhase2Step5(){

		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS) {
			Destroy(ToolsAndProductsPhase2Step5_interface_instance);
		} else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP5)
			Destroy (AR_Mode_Search_interface_instance);
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE2_STEP5;
		
		//indica que entramos en la fase 2 del tutorial:
		inTutorialPhase2 = false;
		Debug.Log ("--> Iniciando modo RA en GoToSearchRotoOrbitalPhase2Step5");
		//indica que se ingresa al modo RA fuera de los tutoriales:
		in_RA_mode = true;
		
		//ahora aqui debo instanciar la interfaz del tutorial 2 y asignarle todas las propiedades:
		AR_Mode_Search_interface_instance = Instantiate (AR_Mode_interface);
		ControllerBlinkingARGeneric blinking_search_phase2step5 = AR_Mode_Search_interface_instance.GetComponent<ControllerBlinkingARGeneric> ();
		
		Debug.Log ("Nueva Interfaz Instanciada en GoToSearchRotoOrbitalPhase2Step5!!");
		
		//hay que asignar la interfaz activa tambien:
		interfaceInstanceActive = AR_Mode_Search_interface_instance;
		
		//localizando el marcador que se debe buscar en esta fase del proceso y cargando datos respectivos:
		markerInScene = GameObject.Find ("FrameMarker99");
		ControllerAddInfoInMarker controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
				
		//NOTA: antes de hacer esto es importante que la variable interfaceInstanceActive este asignada:
		//tambien es importante haber asignado la variable inTutorialPhase1
		controller_info_marker.LoadInformationToInterface (false,false,true);
		
		Debug.LogError ("NOW: Start Blinking en GoToSearchObjetoLimpiezaPhase2Step3");
		//asignando el texto para el feedback directamente a la interfaz:
		blinking_search_phase2step5.feedback_info_text_path = text_feedback_phase2step5;
		
		//definiendo los ordenes que controlara esta interfaz:
		order_in_process = new int[1];
		order_in_process [0] = 17; //buscando roto-orbital
		blinking_search_phase2step5.ordenes = order_in_process;
		
		//Asignando el metodo que se debe ejecutar cuando se hace click sobre el btn Select de la interfaz
		blinking_search_phase2step5.interface_going_from = "Phase2Step5";  //la variable Phase1Step3 permite obtener la interfaz para obtenerla en OnBackButtonTapped
		blinking_search_phase2step5.onClickSelectBtn += OnClickSelectRotoOrbitalPhase2Step5;
		
		//iniciando el proceso blinking:
		blinking_search_phase2step5.should_be_blinking = true;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: I50");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "I50", "11", "-1");

	} //cierra GoToSearchRotoOrbitalPhase2Step5


	public void GoToSearchDiscoP80Phase2Step5(){

		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS) {
			Destroy(ToolsAndProductsPhase2Step5_interface_instance);
		} else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP5)
			Destroy (AR_Mode_Search_interface_instance);
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE2_STEP5;
		
		//indica que entramos en la fase 2 del tutorial:
		inTutorialPhase2 = false;
		Debug.Log ("--> Iniciando modo RA en GoToSearchRotoOrbitalPhase2Step5");
		//indica que se ingresa al modo RA fuera de los tutoriales:
		in_RA_mode = true;
		
		//ahora aqui debo instanciar la interfaz del tutorial 2 y asignarle todas las propiedades:
		AR_Mode_Search_interface_instance = Instantiate (AR_Mode_interface);
		ControllerBlinkingARGeneric blinking_search_phase2step5_second = AR_Mode_Search_interface_instance.GetComponent<ControllerBlinkingARGeneric> ();
		
		Debug.Log ("Nueva Interfaz Instanciada en GoToSearchRotoOrbitalPhase2Step5!!");
		//hay que asignar la interfaz activa tambien:
		interfaceInstanceActive = AR_Mode_Search_interface_instance;
		
		//localizando el marcador que se debe buscar en esta fase del proceso y cargando datos respectivos:
		markerInScene = GameObject.Find ("FrameMarker30");
		ControllerAddInfoInMarker controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		controller_info_marker.image_marker_path = "Sprites/markers/frameMarker30_32_p80_p120";
		controller_info_marker.image_marker_real_path = "Sprites/phase2step5/FrameMarker30_32_disco_p80_p120";
		//NOTA: antes de hacer esto es importante que la variable interfaceInstanceActive este asignada:
		//tambien es importante haber asignado la variable inTutorialPhase1
		controller_info_marker.LoadInformationToInterface (false,false,true);
		
		Debug.LogError ("NOW: Start Blinking en GoToSearchDiscoP80Phase2Step5");
		//asignando el texto para el feedback directamente a la interfaz:
		blinking_search_phase2step5_second.feedback_info_text_path = text_feedback_phase2step5;
		
		//definiendo los ordenes que controlara esta interfaz:
		order_in_process = new int[1];
		order_in_process [0] = 18;//buscando disco P80
		blinking_search_phase2step5_second.ordenes = order_in_process;
		
		//Asignando el metodo que se debe ejecutar cuando se hace click sobre el btn Select de la interfaz
		blinking_search_phase2step5_second.interface_going_from = "Phase2Step5";  //la variable Phase1Step3 permite obtener la interfaz para obtenerla en OnBackButtonTapped
		blinking_search_phase2step5_second.onClickSelectBtn += OnClickSelectDiscoP80Phase2Step5;
		
		//iniciando el proceso blinking:
		blinking_search_phase2step5_second.should_be_blinking = true;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: I51");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "I51", "11", "-1");
	} //cierra GoToSearchDiscoP80Phase2Step5


	public void GoToSearchObjetoLimpiezaPhase2Step5(){

		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS) {
			Destroy(ToolsAndProductsPhase2Step5_interface_instance);
		} else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP5)
			Destroy (AR_Mode_Search_interface_instance);
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE2_STEP5;
		
		//indica que entramos en la fase 2 del tutorial:
		inTutorialPhase2 = false;
		Debug.Log ("--> Iniciando modo RA en GoToSearchRotoOrbitalPhase2Step5");
		//indica que se ingresa al modo RA fuera de los tutoriales:
		in_RA_mode = true;
		
		//ahora aqui debo instanciar la interfaz del tutorial 2 y asignarle todas las propiedades:
		AR_Mode_Search_interface_instance = Instantiate (AR_Mode_interface);
		ControllerBlinkingARGeneric blinking_search_phase2step5_third = AR_Mode_Search_interface_instance.GetComponent<ControllerBlinkingARGeneric> ();
		
		Debug.Log ("Nueva Interfaz Instanciada en GoToSearchRotoOrbitalPhase2Step5!!");
		//hay que asignar la interfaz activa tambien:
		interfaceInstanceActive = AR_Mode_Search_interface_instance;
		
		//localizando el marcador que se debe buscar en esta fase del proceso y cargando datos respectivos:
		markerInScene = GameObject.Find ("FrameMarker25");
		ControllerAddInfoInMarker controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		controller_info_marker.image_marker_path = "Sprites/markers/frameMarker25_24_23_elemento_limpieza";
		controller_info_marker.image_marker_real_path = "Sprites/phase2step5/FrameMarker25_24_23_limpieza";
		//NOTA: antes de hacer esto es importante que la variable interfaceInstanceActive este asignada:
		//tambien es importante haber asignado la variable inTutorialPhase1
		controller_info_marker.LoadInformationToInterface (false,false,true);
		
		Debug.LogError ("NOW: Start Blinking en GoToSearchObjetoLimpiezaPhase2Step5");
		//asignando el texto para el feedback directamente a la interfaz:
		blinking_search_phase2step5_third.feedback_info_text_path = text_feedback_phase2step5;
		
		//definiendo los ordenes que controlara esta interfaz:
		order_in_process = new int[1];
		order_in_process [0] = 19; //buscando objeto de limpieza paso 5 fase2
		blinking_search_phase2step5_third.ordenes = order_in_process;
		
		//Asignando el metodo que se debe ejecutar cuando se hace click sobre el btn Select de la interfaz
		blinking_search_phase2step5_third.interface_going_from = "Phase2Step5";  //la variable Phase1Step3 permite obtener la interfaz para obtenerla en OnBackButtonTapped
		blinking_search_phase2step5_third.onClickSelectBtn += OnClickSelectObjLimpiezaPhase2Step5;
		
		//iniciando el proceso blinking:
		blinking_search_phase2step5_third.should_be_blinking = true;

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: I52");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "I52", "11", "-1");
	}// cierra metodo GoToSearchObjetoLimpiezaPhase2Step5


	public void GoToSearchDiscoP150Phase2Step6(){

		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS) {
			Destroy(ToolsAndProductsPhase2Step6_interface_instance);
		} else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP6)
			Destroy (AR_Mode_Search_interface_instance);
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE2_STEP6;
		
		//indica que entramos en la fase 2 del tutorial:
		inTutorialPhase2 = false;
		Debug.Log ("--> Iniciando modo RA en GoToSearchDiscoP150Phase2Step6");
		//indica que se ingresa al modo RA fuera de los tutoriales:
		in_RA_mode = true;
		
		//ahora aqui debo instanciar la interfaz del tutorial 2 y asignarle todas las propiedades:
		AR_Mode_Search_interface_instance = Instantiate (AR_Mode_interface);
		ControllerBlinkingARGeneric blinking_search_phase2step6 = AR_Mode_Search_interface_instance.GetComponent<ControllerBlinkingARGeneric> ();
		
		Debug.Log ("Nueva Interfaz Instanciada en GoToSearchDiscoP150Phase2Step6!!");
		//hay que asignar la interfaz activa tambien:
		interfaceInstanceActive = AR_Mode_Search_interface_instance;
		
		//localizando el marcador que se debe buscar en esta fase del proceso y cargando datos respectivos:
		markerInScene = GameObject.Find ("FrameMarker33");
		ControllerAddInfoInMarker controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		if (last_markerid_scanned == 32) {
			Debug.Log ("-->SEQUENCE:: Se va a cargar la informacion del DISCO P180 y el last_marker es:" + last_markerid_scanned);
			controller_info_marker.image_marker_path = "Sprites/markers/frameMarker34_disco_p180";
			controller_info_marker.image_marker_real_path = "Sprites/phase2step6/FrameMarker34_disco_p180";
		} else {
			Debug.Log ("--> SEQUENCE: El last marker scanned fue: " + last_markerid_scanned);
			controller_info_marker.image_marker_path = "Sprites/markers/frameMarker33_disco_p150";
			controller_info_marker.image_marker_real_path = "Sprites/phase2step6/FrameMarker33_disco_p150";
		}
		//NOTA: antes de hacer esto es importante que la variable interfaceInstanceActive este asignada:
		//tambien es importante haber asignado la variable inTutorialPhase1
		controller_info_marker.LoadInformationToInterface (false,false,true);

		//NOTA: Especialmente para este marcador se va a habilitar la validacion de secuencia:
		blinking_search_phase2step6.validate_sequence_of_markers = true;
		blinking_search_phase2step6.previous_marker_id = this.last_markerid_scanned;
		if (this.last_markerid_scanned == 30)
			blinking_search_phase2step6.next_marker_id = 33;
		else
			blinking_search_phase2step6.next_marker_id = 34;


		Debug.LogError ("NOW: Start Blinking en GoToSearchDiscoP150Phase2Step6");
		//asignando el texto para el feedback directamente a la interfaz:
		if(last_markerid_scanned == 32)
			blinking_search_phase2step6.feedback_info_text_path = text_feedback_phase2step6_p180;
		else blinking_search_phase2step6.feedback_info_text_path = text_feedback_phase2step6;
		
		//definiendo los ordenes que controlara esta interfaz:
		order_in_process = new int[1];
		order_in_process [0] = 20; //buscando disco P150-P120
		blinking_search_phase2step6.ordenes = order_in_process;
		
		//Asignando el metodo que se debe ejecutar cuando se hace click sobre el btn Select de la interfaz
		blinking_search_phase2step6.interface_going_from = "Phase2Step6";  //la variable Phase1Step3 permite obtener la interfaz para obtenerla en OnBackButtonTapped
		blinking_search_phase2step6.onClickSelectBtn += OnClickSelectDiscoP150Phase2Step6;
		
		//iniciando el proceso blinking:
		blinking_search_phase2step6.should_be_blinking = true;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: I53");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "I53", "12", "-1");
	} //cierra GoToSearchDiscoP150Phase2Step6


	public void GoToSearchObjetoLimpiezaPhase2Step6(){
		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS) {
			Destroy(ToolsAndProductsPhase2Step6_interface_instance);
		} else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP6)
			Destroy (AR_Mode_Search_interface_instance);
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE2_STEP6;
		
		//indica que entramos en la fase 2 del tutorial:
		inTutorialPhase2 = false;
		Debug.Log ("--> Iniciando modo RA en GoToSearchObjetoLimpiezaPhase2Step6");
		//indica que se ingresa al modo RA fuera de los tutoriales:
		in_RA_mode = true;
		
		//ahora aqui debo instanciar la interfaz del tutorial 2 y asignarle todas las propiedades:
		AR_Mode_Search_interface_instance = Instantiate (AR_Mode_interface);
		ControllerBlinkingARGeneric blinking_search_phase2step6_second = AR_Mode_Search_interface_instance.GetComponent<ControllerBlinkingARGeneric> ();
		
		Debug.Log ("Nueva Interfaz Instanciada en GoToSearchDiscoP150Phase2Step6!!");
		//hay que asignar la interfaz activa tambien:
		interfaceInstanceActive = AR_Mode_Search_interface_instance;
		
		//localizando el marcador que se debe buscar en esta fase del proceso y cargando datos respectivos:
		markerInScene = GameObject.Find ("FrameMarker25");
		ControllerAddInfoInMarker controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		controller_info_marker.image_marker_path = "Sprites/markers/frameMarker25_24_23_elemento_limpieza";
		controller_info_marker.image_marker_real_path = "Sprites/phase2step6/FrameMarker25_24_23_limpieza";

		//NOTA: antes de hacer esto es importante que la variable interfaceInstanceActive este asignada:
		//tambien es importante haber asignado la variable inTutorialPhase1
		controller_info_marker.LoadInformationToInterface (false,false,true);
		
		Debug.LogError ("NOW: Start Blinking en GoToSearchDiscoP150Phase2Step6");
		//asignando el texto para el feedback directamente a la interfaz:
		blinking_search_phase2step6_second.feedback_info_text_path = text_feedback_phase2step6;
		
		//definiendo los ordenes que controlara esta interfaz:
		order_in_process = new int[1];
		order_in_process [0] = 21; //Buscando objeto de limpieza paso 6 fase2
		blinking_search_phase2step6_second.ordenes = order_in_process;
		
		//Asignando el metodo que se debe ejecutar cuando se hace click sobre el btn Select de la interfaz
		blinking_search_phase2step6_second.interface_going_from = "Phase2Step6";  //la variable Phase1Step3 permite obtener la interfaz para obtenerla en OnBackButtonTapped
		blinking_search_phase2step6_second.onClickSelectBtn += OnClickSelectObjLimpiezaPhase2Step6;
		
		//iniciando el proceso blinking:
		blinking_search_phase2step6_second.should_be_blinking = true;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: I54");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "I54", "12", "-1");
	}//cierra GoToSearchObjetoLimpiezaPhase2Step6


	public void GoToSearchDiscoP240Phase2Step7(){

		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS) {
			Destroy(ToolsAndProductsPhase2Step7_interface_instance);
		} else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP7)
			Destroy (AR_Mode_Search_interface_instance);
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE2_STEP7;
		
		//indica que entramos en la fase 2 del tutorial:
		inTutorialPhase2 = false;
		Debug.Log ("--> Iniciando modo RA en GoToSearchDiscoP240Phase2Step7");
		//indica que se ingresa al modo RA fuera de los tutoriales:
		in_RA_mode = true;
		
		//ahora aqui debo instanciar la interfaz del tutorial 2 y asignarle todas las propiedades:
		AR_Mode_Search_interface_instance = Instantiate (AR_Mode_interface);
		ControllerBlinkingARGeneric blinking_search_phase2step7 = AR_Mode_Search_interface_instance.GetComponent<ControllerBlinkingARGeneric> ();
		
		Debug.Log ("Nueva Interfaz Instanciada en GoToSearchDiscoP240Phase2Step7!!");
		//hay que asignar la interfaz activa tambien:
		interfaceInstanceActive = AR_Mode_Search_interface_instance;
		
		//localizando el marcador que se debe buscar en esta fase del proceso y cargando datos respectivos:
		markerInScene = GameObject.Find ("FrameMarker36");
		ControllerAddInfoInMarker controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
				
		//NOTA: antes de hacer esto es importante que la variable interfaceInstanceActive este asignada:
		//tambien es importante haber asignado la variable inTutorialPhase1
		controller_info_marker.LoadInformationToInterface (false,false,true);
		
		Debug.LogError ("NOW: Start Blinking en GoToSearchDiscoP240Phase2Step7");
		//asignando el texto para el feedback directamente a la interfaz:
		blinking_search_phase2step7.feedback_info_text_path = text_feedback_phase2step7;
		
		//definiendo los ordenes que controlara esta interfaz:
		order_in_process = new int[1];
		order_in_process [0] = 22; //Buscando disco P240
		blinking_search_phase2step7.ordenes = order_in_process;
		
		//Asignando el metodo que se debe ejecutar cuando se hace click sobre el btn Select de la interfaz
		blinking_search_phase2step7.interface_going_from = "Phase2Step7";  //la variable Phase1Step3 permite obtener la interfaz para obtenerla en OnBackButtonTapped
		blinking_search_phase2step7.onClickSelectBtn += OnClickSelectDiscoP240Phase2Step7;
		
		//iniciando el proceso blinking:
		blinking_search_phase2step7.should_be_blinking = true;

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: I55");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "I55", "13", "-1");

	} // cierra GoToSearchDiscoP240Phase2Step7

	public void GoToSearchObjetoLimpiezaPhase2Step7(){

		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS) {
			Destroy(ToolsAndProductsPhase2Step7_interface_instance);
		} else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP7)
			Destroy (AR_Mode_Search_interface_instance);
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE2_STEP7;
		
		//indica que entramos en la fase 2 del tutorial:
		inTutorialPhase2 = false;
		Debug.Log ("--> Iniciando modo RA en GoToSearchObjetoLimpiezaPhase2Step7");
		//indica que se ingresa al modo RA fuera de los tutoriales:
		in_RA_mode = true;
		
		//ahora aqui debo instanciar la interfaz del tutorial 2 y asignarle todas las propiedades:
		AR_Mode_Search_interface_instance = Instantiate (AR_Mode_interface);
		ControllerBlinkingARGeneric blinking_search_phase2step7_second = AR_Mode_Search_interface_instance.GetComponent<ControllerBlinkingARGeneric> ();
		
		Debug.Log ("Nueva Interfaz Instanciada en GoToSearchObjetoLimpiezaPhase2Step7!!");
		//hay que asignar la interfaz activa tambien:
		interfaceInstanceActive = AR_Mode_Search_interface_instance;
		
		//localizando el marcador que se debe buscar en esta fase del proceso y cargando datos respectivos:
		markerInScene = GameObject.Find ("FrameMarker25");
		ControllerAddInfoInMarker controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		controller_info_marker.image_marker_path = "Sprites/markers/frameMarker25_24_23_elemento_limpieza";
		controller_info_marker.image_marker_real_path = "Sprites/phase2step7/FrameMarker25_24_23_limpieza";

		//NOTA: antes de hacer esto es importante que la variable interfaceInstanceActive este asignada:
		//tambien es importante haber asignado la variable inTutorialPhase1
		controller_info_marker.LoadInformationToInterface (false,false,true);
		
		Debug.LogError ("NOW: Start Blinking en GoToSearchObjetoLimpiezaPhase2Step7");
		//asignando el texto para el feedback directamente a la interfaz:
		blinking_search_phase2step7_second.feedback_info_text_path = text_feedback_phase2step7;
		
		//definiendo los ordenes que controlara esta interfaz:
		order_in_process = new int[1];
		order_in_process [0] = 23; //buscando objeto de limpieza
		blinking_search_phase2step7_second.ordenes = order_in_process;
		
		//Asignando el metodo que se debe ejecutar cuando se hace click sobre el btn Select de la interfaz
		blinking_search_phase2step7_second.interface_going_from = "Phase2Step7";  //la variable Phase1Step3 permite obtener la interfaz para obtenerla en OnBackButtonTapped
		blinking_search_phase2step7_second.onClickSelectBtn += OnClickSelectObjLimpiezaPhase2Step7;
		
		//iniciando el proceso blinking:
		blinking_search_phase2step7_second.should_be_blinking = true;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: I56");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "I56", "13", "-1");

	} //cierra GoToSearchObjetoLimpiezaPhase2Step7


	public void GoToSearchDiscoP320Phase2Step8(){

		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS) {
			Destroy(ToolsAndProductsPhase2Step8_interface_instance);
		} else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP8)
			Destroy (AR_Mode_Search_interface_instance);
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE2_STEP8;
		
		//indica que entramos en la fase 2 del tutorial:
		inTutorialPhase2 = false;
		Debug.Log ("--> Iniciando modo RA en GoToSearchObjetoLimpiezaPhase2Step7");
		//indica que se ingresa al modo RA fuera de los tutoriales:
		in_RA_mode = true;
		
		//ahora aqui debo instanciar la interfaz del tutorial 2 y asignarle todas las propiedades:
		AR_Mode_Search_interface_instance = Instantiate (AR_Mode_interface);
		ControllerBlinkingARGeneric blinking_search_phase2step8 = AR_Mode_Search_interface_instance.GetComponent<ControllerBlinkingARGeneric> ();
		
		Debug.Log ("Nueva Interfaz Instanciada en GoToSearchObjetoLimpiezaPhase2Step7!!");
		//hay que asignar la interfaz activa tambien:
		interfaceInstanceActive = AR_Mode_Search_interface_instance;
		
		//localizando el marcador que se debe buscar en esta fase del proceso y cargando datos respectivos:
		markerInScene = GameObject.Find ("FrameMarker38");
		ControllerAddInfoInMarker controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();

		
		//NOTA: antes de hacer esto es importante que la variable interfaceInstanceActive este asignada:
		//tambien es importante haber asignado la variable inTutorialPhase1
		controller_info_marker.LoadInformationToInterface (false,false,true);
		
		Debug.LogError ("NOW: Start Blinking en GoToSearchObjetoLimpiezaPhase2Step7");
		//asignando el texto para el feedback directamente a la interfaz:
		blinking_search_phase2step8.feedback_info_text_path = text_feedback_phase2step8;
		
		//definiendo los ordenes que controlara esta interfaz:
		order_in_process = new int[1];
		order_in_process [0] = 24; //buscando disco P320
		blinking_search_phase2step8.ordenes = order_in_process;
		
		//Asignando el metodo que se debe ejecutar cuando se hace click sobre el btn Select de la interfaz
		blinking_search_phase2step8.interface_going_from = "Phase2Step8";  //la variable Phase1Step3 permite obtener la interfaz para obtenerla en OnBackButtonTapped
		blinking_search_phase2step8.onClickSelectBtn += OnClickSelectDiscoP320Phase2Step8;
		
		//iniciando el proceso blinking:
		blinking_search_phase2step8.should_be_blinking = true;

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: I57");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "I57", "14", "-1");

	}//cierra GoToSearchDiscoP320Phase2Step8


	public void GoToSearchObjetoLimpiezaPhase2Step8(){

		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS) {
			Destroy(ToolsAndProductsPhase2Step8_interface_instance);
		} else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP8)
			Destroy (AR_Mode_Search_interface_instance);
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE2_STEP8;
		
		//indica que entramos en la fase 2 del tutorial:
		inTutorialPhase2 = false;
		Debug.Log ("--> Iniciando modo RA en GoToSearchObjetoLimpiezaPhase2Step7");
		//indica que se ingresa al modo RA fuera de los tutoriales:
		in_RA_mode = true;
		
		//ahora aqui debo instanciar la interfaz del tutorial 2 y asignarle todas las propiedades:
		AR_Mode_Search_interface_instance = Instantiate (AR_Mode_interface);
		ControllerBlinkingARGeneric blinking_search_phase2step8_second = AR_Mode_Search_interface_instance.GetComponent<ControllerBlinkingARGeneric> ();


		Debug.Log ("Nueva Interfaz Instanciada en GoToSearchObjetoLimpiezaPhase2Step7!!");
		//hay que asignar la interfaz activa tambien:
		interfaceInstanceActive = AR_Mode_Search_interface_instance;
		
		//localizando el marcador que se debe buscar en esta fase del proceso y cargando datos respectivos:
		markerInScene = GameObject.Find ("FrameMarker25");
		ControllerAddInfoInMarker controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		controller_info_marker.image_marker_path = "Sprites/markers/frameMarker25_24_23_elemento_limpieza";
		controller_info_marker.image_marker_real_path = "Sprites/phase2step8/FrameMarker25_24_23_limpieza";
		
		//NOTA: antes de hacer esto es importante que la variable interfaceInstanceActive este asignada:
		//tambien es importante haber asignado la variable inTutorialPhase1
		controller_info_marker.LoadInformationToInterface (false,false,true);
		
		Debug.LogError ("NOW: Start Blinking en GoToSearchObjetoLimpiezaPhase2Step7");
		//asignando el texto para el feedback directamente a la interfaz:
		blinking_search_phase2step8_second.feedback_info_text_path = text_feedback_phase2step8;
		
		//definiendo los ordenes que controlara esta interfaz:
		order_in_process = new int[1];
		order_in_process [0] = 25; //buscando objeto de limpieza paso8 fase2
		blinking_search_phase2step8_second.ordenes = order_in_process;
		
		//Asignando el metodo que se debe ejecutar cuando se hace click sobre el btn Select de la interfaz
		blinking_search_phase2step8_second.interface_going_from = "Phase2Step8";  //la variable Phase1Step3 permite obtener la interfaz para obtenerla en OnBackButtonTapped
		blinking_search_phase2step8_second.onClickSelectBtn += OnClickSelectObjLimpiezaPhase2Step8;
		
		//iniciando el proceso blinking:
		blinking_search_phase2step8_second.should_be_blinking = true;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: I58");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "I58", "14", "-1");
	} //cierra GoToSearchObjetoLimpiezaPhase2Step8

	#region PRIVATE_METHODS

    private void OnSingleTapped()
    {	
		if (interfaceInstanceActive != null)
			Debug.Log ("Llamado al OnSingleTapped: EvaluationMode=" + in_Evaluation_mode + ", InstanceActive=" + interfaceInstanceActive.name + ", InfoLoaded=" + informationLoadedFromMarker);
		else {
			Debug.Log ("OnSingleTapped con interfaceInstanceActive NULL");
		}
		if (in_informative_mode) {
			Debug.Log("Se va a notificar a la clase InformativeMode del TAP sobre la interfaz!!");
			//notificando a la clase InformativeMode que se ha realizado un tap sobre la intefaz
			informative_mode.OnSingleTapped();
		} else {
			
			if (info_additional_displayed) {

			Debug.Log("Ingresa al TAP con info_add_displayed= " + info_additional_displayed);

			} else {

					if (inTutorialPhase1 && interfaceInstanceActive != null && informationLoadedFromMarker){
						Debug.LogError("Ingresa a Info Additional Displayed para tutorial phase 1!!!");
						interfaceInstanceActive.GetComponent<ControllerBlinkingMarker>().PrepareAdditionalIcons();
						interfaceInstanceActive.GetComponent<ControllerBlinkingMarker>().ShowAdditionalIncons(true,false);
						if(interfaceInstanceActive.GetComponent<ControllerBlinkingMarker>().is_add_info_displayed)
							info_additional_displayed=true;
						else info_additional_displayed = false;
					}else if (inTutorialPhase2 && interfaceInstanceActive != null && informationLoadedFromMarker){
						interfaceInstanceActive.GetComponent<ControllerBlinkingAddInfo>().PrepareAdditionalIcons();
						interfaceInstanceActive.GetComponent<ControllerBlinkingAddInfo>().ShowAdditionalIncons(false,true);
						if(interfaceInstanceActive.GetComponent<ControllerBlinkingAddInfo>().is_add_info_displayed)
							info_additional_displayed=true;
						else info_additional_displayed = false;
					} else if (in_RA_mode && interfaceInstanceActive != null && informationLoadedFromMarker){
						interfaceInstanceActive.GetComponent<ControllerBlinkingARGeneric>().PrepareAdditionalIcons();
						interfaceInstanceActive.GetComponent<ControllerBlinkingARGeneric>().ShowAdditionalIncons();
						if(interfaceInstanceActive.GetComponent<ControllerBlinkingARGeneric>().is_add_info_displayed)
							info_additional_displayed=true;
						else info_additional_displayed = false;
					} else if(in_Evaluation_mode && interfaceInstanceActive != null && informationLoadedFromMarker){
						interfaceInstanceActive.GetComponent<ControllerBlinkARModeEvaluation>().PrepareAdditionalIcons();
						interfaceInstanceActive.GetComponent<ControllerBlinkARModeEvaluation>().ShowAdditionalIncons();
						if(interfaceInstanceActive.GetComponent<ControllerBlinkARModeEvaluation>().is_add_info_displayed)
							info_additional_displayed=true;
						else info_additional_displayed = false;
					}
			}
	  }

    }

    private void OnDoubleTapped()
    {	
		if (mActiveViewType == ViewType.ARCAMERAVIEW)
		{
			// trigger focus once
			m_UIEventHandler.TriggerAutoFocus();
		}
        /*
		if (mActiveViewType == ViewType.ARCAMERAVIEW)
        {
            mActiveViewType = ViewType.UIVIEW;
        }
        */
    }

    private void OnTappedOnGoToAboutPage()
    {
        mActiveViewType = ViewType.ABOUTVIEW;
    }

    private void OnBackButtonTapped()
    {
        if (mActiveViewType == ViewType.ABOUTVIEW)
        {
            Application.Quit();
        }
        else if (mActiveViewType == ViewType.UIVIEW) //Hide UIMenu and Show ARCameraView
        {
            mActiveViewType = ViewType.ARCAMERAVIEW;
        }
        else if (mActiveViewType == ViewType.ARCAMERAVIEW) //if it's in ARCameraView
        {
            mActiveViewType = ViewType.ABOUTVIEW;
        }

		//validaciones para habilitar el boton atras:
		/*
		Debug.LogError ("Se ha presionado el boton back");
		if(interfaceComingBackFrom == "MenuPhases"){
			GoToSelectionOfMode();
				
		}else if (interfaceComingBackFrom == "Phase1"){
			GoToMenuPhases();
		}
			goBackFromOtherInterface = false;
			interfaceComingBackFrom = "";
		*/

		//Si el estudiante esta en el modo informativo entonces se llama al evento que controla las interfaces de ese modo
		//a traves de la instancia informative_mode

		if (current_interface == CurrentInterface.CHALLENGE)
			Application.Quit ();
		else if (current_interface == CurrentInterface.SELECTION_OF_MODE)
			GoToChallengeInterface ();
		else if (current_interface == CurrentInterface.INFORMATIVE_MODE) {
			Debug.Log ("BOTON ATRAS: Modo informativo se notifica al metodo en la clase InformativeMode-->");
			informative_mode.OnBackButtonTapped ();
		} else if (current_interface == CurrentInterface.MENU_PHASES || current_interface == CurrentInterface.MENU_PHASES_EV)
			GoToSelectionOfMode ();
		else if (current_interface == CurrentInterface.MENU_STEPS_PHASE1 || current_interface == CurrentInterface.MENU_STEPS_PHASE2)
			GoToMenuPhases ();
		else if (current_interface == CurrentInterface.ACTIVITIES_PHASE1_STEP1)
			GoToMenuStepsPhase1 ();
		else if (current_interface == CurrentInterface.MENU_SUB_STEPS_PHASE2 || current_interface == CurrentInterface.MENU_SUB_STEPS_INTERIORES_PHASE2)
			GoToMenuStepsPhase2 ();
		else if (current_interface == CurrentInterface.ACTIVITIES_PHASE1_STEP1_EV)
			GoToMenuStepsPhase1Eval ();
		else if (current_interface == CurrentInterface.ACTIVITIES_PHASE1_STEP2)
			GoToMenuStepsPhase1 ();
		else if (current_interface == CurrentInterface.ACTIVITIES_PHASE1_STEP3)
			GoToMenuStepsPhase1 ();
		else if (current_interface == CurrentInterface.ACTIVITIES_PHASE1_STEP4)
			GoToMenuStepsPhase1 ();
		else if (current_interface == CurrentInterface.ACTIVITIES_PHASE1_STEP4)
			GoToMenuStepsPhase1 ();
		else if (current_interface == CurrentInterface.ACTIVITIES_PHASE1_STEP5)
			GoToMenuStepsPhase1 ();
		else if (current_interface == CurrentInterface.ACTIVITIES_PHASE1_STEP6)
			GoToMenuStepsPhase1 ();
		else if (current_interface == CurrentInterface.ACTIVITIES_PHASE2_STEP1 || current_interface == CurrentInterface.ACTIVITIES_PHASE2_STEP2)
			GoToMenuStepsPhase2 ();
		else if (current_interface == CurrentInterface.ACTIVITIES_PHASE2_STEP3 || current_interface == CurrentInterface.ACTIVITIES_PHASE2_STEP4)
			GoToSubMenuStepsLijadoCantos ();
		else if (current_interface == CurrentInterface.ACTIVITIES_PHASE2_STEP5 || current_interface == CurrentInterface.ACTIVITIES_PHASE2_STEP6 || current_interface == CurrentInterface.ACTIVITIES_PHASE2_STEP7 || current_interface == CurrentInterface.ACTIVITIES_PHASE2_STEP8)
			GoToSubMenuStepsLijadoInteriores ();
		else if (current_interface == CurrentInterface.AR_SEARCH_CAR_HOOD)
			GoToActivitiesPhase1Step1 ();
		else if (current_interface == CurrentInterface.AR_SEARCH_CAR_HOOD_EVAL)
			GoToActivitiesPhase1Step1EvalMode ();
		else if (current_interface == CurrentInterface.MENU_STEPS_PHASE1_EV || current_interface == CurrentInterface.MENU_STEPS_PHASE2_EV)
			GoToMenuPhasesEvaluationMode ();
		else if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS) {
				CanvasToolsAndProductsManager canvas_mng = this.interfaceInstanceActive.GetComponent<CanvasToolsAndProductsManager>();
				if(canvas_mng != null){
					GoBackToMenuActivitiesFromToolsProducts(canvas_mng.interfaceGoingBackFrom);
				}else {
					Debug.Log("--> ERROR: No se pudo obtener el CanvasToolsAndProductsManager en OnBackButtonTapped");
				}
		} else if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_EVAL) {
			CanvasToolsAndProductsManager canvas_mng_eval = this.interfaceInstanceActive.GetComponent<CanvasToolsAndProductsManager>();
			if(canvas_mng_eval != null){
				GoBackMenuActivFromToolsEvalMode(canvas_mng_eval.interfaceGoingBackFrom);
			}else {
				Debug.Log("--> ERROR: No se pudo obtener el CanvasToolsAndProductsManager en OnBackButtonTapped");
			}
		} else if (current_interface == CurrentInterface.AR_SEARCH_PRODUCTS_TUTORIAL || current_interface == CurrentInterface.AR_SEARCH_BAYETA_PRODUCT_TUTORIAL) {
			ControllerBlinkingAddInfo controller_script = this.interfaceInstanceActive.GetComponent<ControllerBlinkingAddInfo>();
			if(controller_script != null){
				string interface_coming_from = controller_script.interface_going_from;
				inTutorialPhase2 = false;
				GoToToolsAndProducts (interface_coming_from);
			}else {
				Debug.Log("ERROR: EL SCRIPT QUE SE OBTIENE EN ONBACKBUTTONTAPPED ES NULL");
				GoToMenuPhases ();
			}
		}else if (current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP3 || current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP4 || current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP5 || current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP6 ){
			ControllerBlinkingARGeneric controller_generic = this.interfaceInstanceActive.GetComponent<ControllerBlinkingARGeneric>();
			in_RA_mode = false;
			if(controller_generic != null){
				string interface_coming_fr = controller_generic.interface_going_from;
				GoToToolsAndProducts (interface_coming_fr);
			}else {
				Debug.Log("ERROR: EL SCRIPT QUE SE OBTIENE EN ONBACKBUTTONTAPPED ES NULL");
				GoToMenuPhases ();
			}
		} else if (current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP2_EV || current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP3_EV  || current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP4_EV  || current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP5_EV  || current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP6_EV){
			ControllerBlinkARModeEvaluation controller_blink_eval = this.interfaceInstanceActive.GetComponent<ControllerBlinkARModeEvaluation>();
			in_RA_mode = false;
			if(controller_blink_eval != null){
				string interface_coming_fr_ev = controller_blink_eval.interface_going_from;
				GoToToolsAndProductsEvalMode (interface_coming_fr_ev);
			}else {
				Debug.Log("ERROR: EL SCRIPT QUE SE OBTIENE EN ONBACKBUTTONTAPPED ES NULL");
				GoToMenuPhasesEvaluationMode ();
			}
		} else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP2_EV || current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP3_EV || current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP4_EV || current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP5_EV || current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP6_EV || current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP7_EV || current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP8_EV){
			ControllerBlinkARModeEvaluation controller_blink_eval = this.interfaceInstanceActive.GetComponent<ControllerBlinkARModeEvaluation>();
			in_RA_mode = false;
			if(controller_blink_eval != null){
				string interface_coming_fr_ev = controller_blink_eval.interface_going_from;
				GoToToolsAndProductsEvalMode (interface_coming_fr_ev);
			}else {
				Debug.Log("ERROR: EL SCRIPT QUE SE OBTIENE EN ONBACKBUTTONTAPPED ES NULL");
				GoToMenuPhasesEvaluationMode ();
			}
		} else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP2 || current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP3 || current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP4 || current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP5 || current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP6 || current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP7 || current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP8){
			ControllerBlinkingARGeneric controller_generic = this.interfaceInstanceActive.GetComponent<ControllerBlinkingARGeneric>();
			in_RA_mode = false;
			if(controller_generic != null){
				string interface_coming_fr = controller_generic.interface_going_from;
				GoToToolsAndProducts (interface_coming_fr);
			}else {
				Debug.Log("ERROR: EL SCRIPT QUE SE OBTIENE EN ONBACKBUTTONTAPPED ES NULL");
				GoToMenuPhases ();
			}
		} else if (current_interface == CurrentInterface.TAKE_AND_SEND_THE_PICTURE && can_return_from_take_photo) { //OJO en este caso en particular el estudiante puede regresar siempre y cuando esta variable este en true y eso se modifica desd script ControllerPinturaCameraImage
			ControllerPinturaCameraImage controller_pictures = this.interfaceInstanceActive.GetComponent<ControllerPinturaCameraImage>();
			if(controller_pictures != null){
				string interface_from_pict = controller_pictures.interface_coming_from;
				GoToTakePictures(interface_from_pict);
			}else {
				Debug.Log("--> ERROR: No se pudo obtener el ControllerPinturaCameraImage en OnBackButtonTapped");
			}
		} else if (current_interface == CurrentInterface.TAKE_PICTURES) {
			CanvasTakePictureInterfaceManager canvas_pictures = this.interfaceInstanceActive.GetComponent<CanvasTakePictureInterfaceManager>();
			if(canvas_pictures != null){
				GoBackToMenuActivitiesFromTakePictures(canvas_pictures.interfaceGoingBackFrom);
			}else {
				Debug.Log("--> ERROR: No se pudo obtener el CanvasTakePictureInterfaceManager en OnBackButtonTapped");
			}
		}
		
		
	}

    private void OnTappedOnCloseButton()
    {
        mActiveViewType = ViewType.ARCAMERAVIEW;
    }

    private void OnAboutStartButtonTapped()
    {
		mActiveViewType = ViewType.ARCAMERAVIEW;
		mAboutView.UnLoadView ();
	}

	public int getProcessOrder(){
		return processOrder;
	}

	/*
	/// <summary>
	/// This method starts the blinking process of the appropriate marker depending on the step of the process.
	/// 
	/// </summary>
	public void MarkerLostAndStartBlinking(){
		Debug.LogError ("Llamado al metodo MarkerLostAndStartBlinking");
		markers = GameObject.FindGameObjectsWithTag ("MarkerObjectScene");
		foreach (GameObject mark in markers) {
			markerHandler = mark.GetComponent<DefaultTrackableEventHandler> ();
			//este codigo se debe mejorar porque fue puesto como una solucion rapida y temporal unicamente
			/*
			if(this.processOrder==8 || this.processOrder==10 || this.processOrder==12 || this.processOrder==14){
				texture_blinking_marker = GameObject.Find ("FrameMarker4_blinking_marker");
				texture_blinking_marker.GetComponent<ControllerBlinkingMarker> ().should_be_blinking = true;
				break;
			} //fin de solucion temporal


			//obteniendo el conjunto de ordenes de los marcadores 
			order_in_markerhandler = markerHandler.order;
			for (int i=0; i<order_in_markerhandler.Length; i++) {
				if (order_in_markerhandler[i] == processOrder) {

					OptionsInterface.GetComponent<ControllerBlinkingMarker> ().should_be_blinking = true;
				}
			}


			if(markerHandler.order==this.processOrder && markerHandler.order != 1){
				controller_blinking_mark = markerHandler.texture_blinking_marker.GetComponent<ControllerBlinkingMarker>();
				controller_blinking_mark.should_be_blinking = true;
				Debug.LogError("Activando el BLINKING DE: " + mark.name);
				break;
			}
		}
	} //cierra marker lost and start blinking
		*/


    private IEnumerator LoadAboutPageForFirstTime()
    {
        yield return new WaitForSeconds(mSecondsVisible);
        mSplashView.UnLoadView();
        //mAboutView.LoadView();
        //mActiveViewType = ViewType.ABOUTVIEW;
		/*
		instance_interface = Instantiate(interfaz);
		CanvasManager cManager = instance_interface.GetComponent<CanvasManager> ();
		cManager.ChangeImage ("Sprites/1_Limpieza_FaseProceso");
		cManager.ChangeStepTitle ("Paso 1: Limpieza General");
		cManager.btn1_action += GoToToolsAndProducts;
		cManager.ChangeButtonAction ();
		cManager.self_asessment_action += GoToSelfAssessment;
		cManager.SetAssessmentButtonAction ();
		//algo.GetComponent<CanvasManager> ().ChangeImage ("ImagesPhases/1_Limpieza_FaseProceso");
		*/

		//cargar la interfaz que muestra la informacion del reto:
		GoToChallengeInterface ();

		mActiveViewType = ViewType.TESTING;
        m_UIEventHandler.Bind();
        yield return null;
    }



    #endregion PRIVATE_METHODS
	/// <summary>
	/// Metodo que se llama cuando se presiona el boton select de la info addicional del capo del carro (phase 1-step1)
	/// en el modo RA.
	/// </summary>
	public void onClickSelectCapoCarroSearch(){
		Debug.LogError ("AppManager: Lamado al metodo onClickSelectCapoCarroSearch - Click en boton Select");

		//La informacion adicional ahora no se despliega:
		info_additional_displayed = false;

		//colocando la variable de tutorial en false porque ya ha finalizado esa parte:
		inTutorialPhase1 = false;

		//validando que ya se ha completado la actividad de herramientas y productos para este paso del capo del coche:
		this.steps_phase_one_completed [0].activity_tools_and_products = true;
		this.steps_phase_one_completed [0].videos_about_process = true;
		this.steps_phase_one_completed [0].selfevaluation = true;
		this.steps_phase_one_completed [0].CheckStepCompletion ();

		Debug.Log ("AppManager: onClickSelectCapoCarroSearch - Se ha completado el SelectCapoCarroSearch completado=" + this.steps_phase_one_completed [0].step_completed);

		//se llama al metodo que carga el menu de pasos de la fase 1 porque ya se ha completado el paso:
		GoToMenuStepsPhase1 ();
	}



	/// <summary>
	///Metodo que se llama cuando se presiona el boton select de la info addicional del agua a pressio (phase1-step2)
	/// en el modo RA. Este metodo lanza la segunda parte de la busqueda de la bayeta
	/// </summary>
	/// <param name="interface_from">Interface_from.</param>
	public void OnClickSelectAguaPressioSearch(string interface_from){
		Debug.LogError ("AppManager: Lamado al metodo OnClickSelectAguaPressioSearch - Click en boton Select");

		//La informacion adicional ahora no se esta desplegando
		info_additional_displayed = false;
		
		//Todavia estamos en la fase del tutorial 2:
		inTutorialPhase2 = true;

		//Llamando al destroy de las posibles instancias existentes de la interfaz MenuOfStepsPhase1:
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhase1");

		//registrando la navegacion de la interfaz:
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: S2 ");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "S2", "1", "-1");

		//Hasta este punto se ha completado la primera parte del tutorial que se refiere a buscar el agua a presion
		//ahora se debe seguir con la segunda parte que es buscar lel agua y el jabon:
		//se va a iniciar la segunda parte de la busqueda con RA:
		GoToSearchAguaJabonTutPhase2 ();
	}

	public void OnClickSelectAguaJabTutorial(string interface_from){
		Debug.LogError ("AppManager: Lamado al metodo OnClickSelectAguaJabTutorial - Click en boton Select");
		
		//La informacion adicional ahora no se esta desplegando
		info_additional_displayed = false;
		
		//Todavia estamos en la fase del tutorial 2:
		inTutorialPhase2 = true;
		
		//Llamando al destroy de las posibles instancias existentes de la interfaz MenuOfStepsPhase1:
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhase1");

		//registrar la navegacion:
		//registrando la navegacion de la interfaz:
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:S3");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "S3", "1", "-1");
		
		//Hasta este punto se ha completado la primera parte del tutorial que se refiere a buscar el agua y el jabon
		//ahora se debe seguir con la segunda parte que es buscar la bayeta:
		//se va a iniciar la segunda parte de la busqueda con RA:
		GoToSearchBayetaPhase1Step2 ();
	}


	/// <summary>
	/// Metodo que se llama cuando se presiona el boton select de la info adicional de la bayeta (phase1-step2)
	/// Esto finaliza el tutorial 2 y vuelve a cargar el menu de pasos
	/// </summary>
	/// <param name="interface_from">Interface_from.</param>
	public void OnClickSelectBayetaSearch(string interface_from){
		Debug.LogError ("AppManager: Lamado al metodo OnClickSelectBayetaSearch - Click en boton Select!!");

		//La informacion adicional ahora no se esta desplegando
		info_additional_displayed = false;
		
		//Aqui termina el tutorial fase 2:
		inTutorialPhase2 = false;

		//Llamando al destroy de las posibles instancias existentes de la interfaz MenuOfStepsPhase1:
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhase1");

		//Ya se ha completado la actividad de herramientas y productos para el paso 2 - Limpieza de la fase 1:
		steps_phase_one_completed [1].activity_tools_and_products = true;
		steps_phase_one_completed [1].CheckStepCompletion ();

		Debug.Log ("AppManager: OnClickSelectBayetaSearch - ya se ha completado ProductsTools, Completion= " + steps_phase_one_completed [1].step_completed);

		//se va a guardar el estado en el que se encuentra la aplicacion actualmente:
		SaveDataForStudent ();

		//registrando la navegacion de la interfaz:
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:S4");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "S4", "1", "-1");

		//se llama al metodo que carga el menu de pasos de la fase 1 porque ya se ha completado el paso:
		//GoToMenuStepsPhase1 ();
		//se llama al metodo que carga el menu de actividades del paso 2 porque ya se ha completado una actividad de este paso:
		GoToActivitiesPhase1Step2 ();

	}

	/// <summary>
	/// Metodo que se llama cuando se presiona el boton select de la info adicional de agua a presion (phase1-step3)
	/// </summary>
	/// <param name="interface_from">Interface_from.</param>
	public void OnClickSelectAguaPresion(string interface_from){
		Debug.LogError ("AppManager: Lamado al metodo OnClickSelectAguaPresion - Click en boton Select!!");

		//Llamando al destroy de las posibles instancias existentes de la interfaz MenuOfStepsPhase1:
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");

		//registrando la navegacion de la interfaz:
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:S5");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "S5", "2", "-1");

		//Lamado al metodo para buscar el segundo objeto en el modo RA
		GoToSearchPapelAbsorbentePhase1Step3 ();
	}

	/// <summary>
	/// Metodo que se llama cuando se presiona el boton select de la info adicional del papel absorbente (phase1-step3)
	/// </summary>
	/// <param name="interface_from">Interface_from.</param>
	public void OnClickSelectPapelAbsorbente(string interface_from){
		Debug.LogError ("AppManager: Lamado al metodo OnClickSelectPapelAbsorbente - Click en boton Select!!");
		
		//Llamando al destroy de las posibles instancias existentes de la interfaz MenuOfStepsPhase1:
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");

		//Setting RA_mode en false porque ya se termina el modo RA aqui:
		Debug.Log ("--> Finalizando modo RA en OnClickSelectPapelAbsorbente");
		in_RA_mode = false;

		//Ya se ha completado la actividad de herramientas y productos para el paso 3 - Secado de la fase 1:
		steps_phase_one_completed [2].activity_tools_and_products = true;
		steps_phase_one_completed [2].CheckStepCompletion ();

		//guardando el estado en el que se encuentra la aplicacion en este momento:
		SaveDataForStudent ();

		//registrando la navegacion de la interfaz:
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:S6");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "S6", "2", "-1");

		//Antes: Se llamaba al medo de pasos de la fase 1:
		//GoToMenuStepsPhase1 ();
		//Ahora se llama al menu de actividades del paso porque ya se ha completado la actividad de RA:
		GoToActivitiesPhase1Step3 ();
	}

	/// <summary>
	/// Metodo que se llama cuando se presiona el boton select de la info adicional de la lija fina (phase1-step4)
	/// </summary>
	/// <param name="interface_from">Interface_from.</param>
	public void OnClickSelectLijaFina(string interface_from){
		Debug.LogError ("AppManager: Lamado al metodo OnClickSelectLijaFina - Click en boton Select!!");
		
		//Llamando al destroy de las posibles instancias existentes de la interfaz MenuOfStepsPhase1:
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");
		
		//Setting RA_mode en false porque ya se termina el modo RA aqui:
		Debug.Log ("--> AppManager: Finalizando modo RA en OnClickSelectLijaFina");
		in_RA_mode = false;

		//Ya se ha completado la actividad de herramientas y productos para el paso 2 - Limpieza de la fase 1:
		steps_phase_one_completed [3].activity_tools_and_products = true;
		steps_phase_one_completed [3].CheckStepCompletion ();

		//guardando el estado en el que se encuentra la aplicacion en este momento:
		SaveDataForStudent ();

		//registrando la navegacion de la interfaz:
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:S7");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "S7", "3", "-1");
				
		//Antes: Cuando termina el modo RA se iba al listado de pasos de la fase 1:
		//GoToMenuStepsPhase1 ();
		//Ahora: Cuando se termina el modod RA se redirije hacia el listado de actividades del paso correspondiente:
		GoToActivitiesPhase1Step4 ();
	}


	/// <summary>
	/// Metodo que se llama cuando se presiona el boton select de la info adicional del martillo (phase1-step5)
	/// </summary>
	/// <param name="interface_from">Interface_from.</param>
	public void OnClickSelectMartillo(string interface_from){
		Debug.LogError ("AppManager: Lamado al metodo OnClickSelectMartillo - Click en boton Select!!");
		
		//Llamando al destroy de las posibles instancias existentes de la interfaz MenuOfStepsPhase1:
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");
		
		//Setting RA_mode en false porque ya se termina el modo RA aqui:
		Debug.Log ("--> AppManager: Finalizando modo RA en OnClickSelectMartillo");
		in_RA_mode = false;

		//Ya se ha completado la actividad de herramientas y productos para el paso 2 - Limpieza de la fase 1:
		steps_phase_one_completed [4].activity_tools_and_products = true;
		steps_phase_one_completed [4].CheckStepCompletion ();

		//guardando el estado en el que se encuentra la aplicacion en este momento:
		SaveDataForStudent ();

		//registrando la navegacion de la interfaz:
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:S8");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "S8", "4", "-1");
				
		//ANTES: Se llamaba al metodo que cargaba el listado de pasos de la fase
		//GoToMenuStepsPhase1 ();
		//AHORA: Se llama al metodo que carga el listado de actividades de ese paso porque ya se ha completado 1 actividad:
		GoToActivitiesPhase1Step5 ();
	}

	/// <summary>
	/// Metodo que se llama cuando se presiona el boton select de la info adicional del desengrasante (phase1-step6)
	/// </summary>
	/// <param name="interface_from">Interface_from.</param>
	public void OnClickSelectDesengrasante(string interface_from){
		Debug.LogError ("AppManager: Lamado al metodo OnClickSelectDesengrasante - Click en boton Select!!");
		
		//Llamando al destroy de las posibles instancias existentes de la interfaz MenuOfStepsPhase1:
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");
		
		//Setting RA_mode en false porque ya se termina el modo RA aqui:
		Debug.Log ("--> AppManager: Finalizando modo RA en OnClickSelectDesengrasante");
		in_RA_mode = false;

		//registrando la navegacion de la interfaz:
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:S9");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "S9", "5", "-1");
		
		//Lamado al metodo para buscar el segundo objeto en el modo RA
		GoToSearchBayetaPhase1Step6 ();
	}


	/// <summary>
	/// Metodo que se llama cuando se presiona el boton select de la info adicional de la bayeta (phase1-step6)
	/// </summary>
	/// <param name="interface_from">Interface_from.</param>
	public void OnClickSelectBayetaStep6(string interface_from){
		Debug.LogError ("AppManager: Lamado al metodo OnClickSelectBayetaStep6 - Click en boton Select!!");
		
		//Llamando al destroy de las posibles instancias existentes de la interfaz MenuOfStepsPhase1:
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");
		
		//Setting RA_mode en false porque ya se termina el modo RA aqui:
		Debug.Log ("--> AppManager: Finalizando modo RA en OnClickSelectBayetaStep6");
		in_RA_mode = false;

		//Ya se ha completado la actividad de herramientas y productos para el paso 2 - Limpieza de la fase 1:
		steps_phase_one_completed [5].activity_tools_and_products = true;
		steps_phase_one_completed [5].CheckStepCompletion ();

		//guardando el estado en el que se encuentra la aplicacion en este momento:
		SaveDataForStudent ();

		//registrando la navegacion de la interfaz:
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:S10");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "S10", "5", "-1");

		if (steps_phase_one_completed [5].step_completed) //Lamado al metodo que muestra el listado de pasos de la FASE 1. Porque ya se han completado todos los pasos
			GoToMenuStepsPhase1 ();
		else
			GoToActivitiesPhase1Step6 (); //en este caso no se han completado las actividades y por lo tanto se regresa al menu de actividades
	}

	/// <summary>
	/// Metodo que se llama cuando se presiona el boton select de la info adicional de la cinta de enmascarar (phase2-step2)
	/// </summary>
	/// <param name="interface_from">Interface_from.</param>
	public void OnClickSelectCintaEmascarar(string interface_from){
		Debug.LogError ("AppManager: Lamado al metodo OnClickSelectCintaEmascarar - Click en boton Select!!");
		
		//Llamando al destroy de las posibles instancias existentes de la interfaz MenuOfStepsPhase1:
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");
		
		//Setting RA_mode en false porque ya se termina el modo RA aqui:
		Debug.Log ("--> AppManager: Finalizando modo RA en OnClickSelectCintaEmascarar");
		in_RA_mode = false;

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:S11");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "S11", "7", "-1");
		
		//Lamado al metodo para buscar el segundo objeto en el modo RA
		GoToSearchPapelEnmascPhase2Step2 ();
	} //cierra metodo OnClickSelectCintaEmascarar


	public void OnClickSelectPapelEnmascPhase2Step2(string interface_from){
		Debug.LogError ("AppManager: Lamado al metodo OnClickSelectPapelEnmascPhase2Step2 - Click en boton Select!!");
		
		//Llamando al destroy de las posibles instancias existentes de la interfaz MenuOfStepsPhase1:
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");
		
		//Setting RA_mode en false porque ya se termina el modo RA aqui:
		Debug.Log ("--> AppManager: Finalizando modo RA en OnClickSelectPapelEnmascPhase2Step2");
		in_RA_mode = false;

		//registrando en el sistema que ya se ha terminado esta actividad satisfactoriamente:
		steps_phase_two_completed [1].activity_tools_and_products = true;
		steps_phase_two_completed [1].CheckStepCompletion ();
		
		//guardando el estado en el que se encuentra la aplicacion en este momento:
		SaveDataForStudent ();

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:S12");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "S12", "7", "-1");

		if (steps_phase_two_completed [1].step_completed) //Lamado al metodo que muestra el listado de pasos de la FASE 2. Porque ya se han completado todos los pasos
			GoToMenuStepsPhase2 ();
		else
			GoToActivitiesPhase2Step2 (); //en este caso no se han completado las actividades y por lo tanto se regresa al menu de actividades
	} //cierra metodo OnClickSelectPapelEnmascPhase2Step2


	public void OnClickSelectEsponjaPhase2Step3(string interface_from){
		Debug.LogError ("AppManager: Lamado al metodo OnClickSelectEsponjaPhase2Step3 - Click en boton Select!!");
		
		//Llamando al destroy de las posibles instancias existentes de la interfaz MenuOfStepsPhase1:
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");
		
		//Setting RA_mode en false porque ya se termina el modo RA aqui:
		Debug.Log ("--> AppManager: Finalizando modo RA en OnClickSelectEsponjaPhase2Step3");
		in_RA_mode = false;

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: S13");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "S13", "9", "-1");
		
		//Lamado al metodo para buscar el segundo objeto en el modo RA
		GoToSearchObjetoLimpiezaPhase2Step3 ();
	} //cierra OnClickSelectEsponjaPhase2Step3


	public void OnClickSelectObjetoLimpiezaPhase2Step3(string interface_from){
		Debug.LogError ("AppManager: Lamado al metodo OnClickSelectObjetoLimpiezaPhase2Step3 - Click en boton Select!!");
		
		//Llamando al destroy de las posibles instancias existentes de la interfaz MenuOfStepsPhase1:
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");
		
		//Setting RA_mode en false porque ya se termina el modo RA aqui:
		Debug.Log ("--> AppManager: Finalizando modo RA en OnClickSelectObjetoLimpiezaPhase2Step3");
		in_RA_mode = false;
		
		//registrando en el sistema que ya se ha terminado esta actividad satisfactoriamente:
		steps_phase_two_completed [2].activity_tools_and_products = true;
		steps_phase_two_completed [2].CheckStepCompletion ();
		
		//guardando el estado en el que se encuentra la aplicacion en este momento:
		SaveDataForStudent ();

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:S14");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "S14", "9", "-1");
		
		if (steps_phase_two_completed [2].step_completed) //Lamado al metodo que muestra el listado de pasos de la FASE 2. Porque ya se han completado todos los pasos
			GoToSubMenuStepsLijadoCantos ();
		else
			GoToActivitiesPhase2Step3 (); //en este caso no se han completado las actividades y por lo tanto se regresa al menu de actividades
	} //cierra metodo OnClickSelectObjetoLimpiezaPhase2Step3


	public void OnClickSelectEsponjaP400Phase2Step4(string interface_from){

		Debug.LogError ("AppManager: Lamado al metodo OnClickSelectEsponjaP400Phase2Step4 - Click en boton Select!!");
		
		//Llamando al destroy de las posibles instancias existentes de la interfaz MenuOfStepsPhase1:
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");
		
		//Setting RA_mode en false porque ya se termina el modo RA aqui:
		Debug.Log ("--> AppManager: Finalizando modo RA en OnClickSelectEsponjaP400Phase2Step4");
		in_RA_mode = false;

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:S15");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "S15", "10", "-1");
		
		//Lamado al metodo para buscar el segundo objeto en el modo RA
		GoToSearchObjetoLimpiezaPhase2Step4 ();

	} //cierra OnClickSelectEsponjaP400Phase2Step4

	public void OnClickSelectObjetoLimpiezaPhase2Step4 (string interface_from){
		Debug.LogError ("AppManager: Lamado al metodo OnClickSelectObjetoLimpiezaPhase2Step4 - Click en boton Select!!");
		
		//Llamando al destroy de las posibles instancias existentes de la interfaz MenuOfStepsPhase1:
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");
		
		//Setting RA_mode en false porque ya se termina el modo RA aqui:
		Debug.Log ("--> AppManager: Finalizando modo RA en OnClickSelectObjetoLimpiezaPhase2Step4");
		in_RA_mode = false;
		
		//registrando en el sistema que ya se ha terminado esta actividad satisfactoriamente:
		steps_phase_two_completed [3].activity_tools_and_products = true;
		steps_phase_two_completed [3].CheckStepCompletion ();
		
		//guardando el estado en el que se encuentra la aplicacion en este momento:
		SaveDataForStudent ();

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:S16");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "S16", "10", "-1");
		
		if (steps_phase_two_completed [3].step_completed) //Lamado al metodo que muestra el listado de pasos de la FASE 2. Porque ya se han completado todos los pasos
			GoToSubMenuStepsLijadoCantos ();
		else
			GoToActivitiesPhase2Step4 (); //en este caso no se han completado las actividades y por lo tanto se regresa al menu de actividades
	} //cierra OnClickSelectObjetoLimpiezaPhase2Step4


	public void OnClickSelectRotoOrbitalPhase2Step5(string interface_from){
		Debug.LogError ("AppManager: Lamado al metodo OnClickSelectRotoOrbitalPhase2Step5 - Click en boton Select!!");
		//Llamando al destroy de las posibles instancias existentes de la interfaz MenuOfStepsPhase1:
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");
		//Setting RA_mode en false porque ya se termina el modo RA aqui:
		Debug.Log ("--> AppManager: Finalizando modo RA en OnClickSelectEsponjaP400Phase2Step4");
		in_RA_mode = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:S17");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "S17", "11", "-1");
		//Lamado al metodo para buscar el segundo objeto en el modo RA
		GoToSearchDiscoP80Phase2Step5 ();
	} //cierraOnClickSelectRotoOrbitalPhase2Step5

	public void OnClickSelectDiscoP80Phase2Step5(string interface_from){
		Debug.LogError ("AppManager: Lamado al metodo OnClickSelectDiscoP80Phase2Step5 - Click en boton Select!!");
		//Llamando al destroy de las posibles instancias existentes de la interfaz MenuOfStepsPhase1:
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");
		//Setting RA_mode en false porque ya se termina el modo RA aqui:
		Debug.Log ("--> AppManager: Finalizando modo RA en OnClickSelectDiscoP80Phase2Step5");
		in_RA_mode = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:S18");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "S18", "11", "-1");
		//Lamado al metodo para buscar el segundo objeto en el modo RA
		GoToSearchObjetoLimpiezaPhase2Step5 ();
	}

	public void OnClickSelectObjLimpiezaPhase2Step5(string interface_from){
		Debug.LogError ("AppManager: Lamado al metodo OnClickSelectObjLimpiezaPhase2Step5 - Click en boton Select!!");
		
		//Llamando al destroy de las posibles instancias existentes de la interfaz MenuOfStepsPhase1:
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");
		
		//Setting RA_mode en false porque ya se termina el modo RA aqui:
		Debug.Log ("--> AppManager: Finalizando modo RA en OnClickSelectObjLimpiezaPhase2Step5");
		in_RA_mode = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:S19");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "S19", "11", "-1");
		
		//registrando en el sistema que ya se ha terminado esta actividad satisfactoriamente:
		steps_phase_two_completed [4].activity_tools_and_products = true;
		steps_phase_two_completed [4].CheckStepCompletion ();
		
		//guardando el estado en el que se encuentra la aplicacion en este momento:
		SaveDataForStudent ();
		
		if (steps_phase_two_completed [4].step_completed) //Lamado al metodo que muestra el listado de pasos de la FASE 2. Porque ya se han completado todos los pasos
			GoToSubMenuStepsLijadoInteriores ();
		else
			GoToActivitiesPhase2Step5 (); //en este caso no se han completado las actividades y por lo tanto se regresa al menu de actividades
	} //cierra OnClickSelectObjLimpiezaPhase2Step5

	public void OnClickSelectDiscoP150Phase2Step6(string interface_from){
		Debug.LogError ("AppManager: Lamado al metodo OnClickSelectDiscoP150Phase2Step6 - Click en boton Select!!");
		//Llamando al destroy de las posibles instancias existentes de la interfaz MenuOfStepsPhase1:
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");
		//Setting RA_mode en false porque ya se termina el modo RA aqui:
		Debug.Log ("--> AppManager: Finalizando modo RA en OnClickSelectDiscoP150Phase2Step6");
		in_RA_mode = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:S20");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "S21", "12", "-1");
		//Lamado al metodo para buscar el segundo objeto en el modo RA
		GoToSearchObjetoLimpiezaPhase2Step6 ();
	}

	public void OnClickSelectObjLimpiezaPhase2Step6(string interface_from){
		Debug.LogError ("AppManager: Lamado al metodo OnClickSelectObjLimpiezaPhase2Step6 - Click en boton Select!!");
		
		//Llamando al destroy de las posibles instancias existentes de la interfaz MenuOfStepsPhase1:
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");
		
		//Setting RA_mode en false porque ya se termina el modo RA aqui:
		Debug.Log ("--> AppManager: Finalizando modo RA en OnClickSelectObjLimpiezaPhase2Step6");
		in_RA_mode = false;
		
		//registrando en el sistema que ya se ha terminado esta actividad satisfactoriamente:
		steps_phase_two_completed [5].activity_tools_and_products = true;
		steps_phase_two_completed [5].CheckStepCompletion ();
		
		//guardando el estado en el que se encuentra la aplicacion en este momento:
		SaveDataForStudent ();

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:S22");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "S22", "12", "-1");
		
		if (steps_phase_two_completed [5].step_completed) //Lamado al metodo que muestra el listado de pasos de la FASE 2. Porque ya se han completado todos los pasos
			GoToSubMenuStepsLijadoInteriores ();
		else
			GoToActivitiesPhase2Step6 (); //en este caso no se han completado las actividades y por lo tanto se regresa al menu de actividades
	}//cierra OnClickSelectObjLimpiezaPhase2Step6


	public void OnClickSelectDiscoP240Phase2Step7(string interface_from){
		Debug.LogError ("AppManager: Lamado al metodo OnClickSelectDiscoP240Phase2Step7 - Click en boton Select!!");
		//Llamando al destroy de las posibles instancias existentes de la interfaz MenuOfStepsPhase1:
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");
		//Setting RA_mode en false porque ya se termina el modo RA aqui:
		Debug.Log ("--> AppManager: Finalizando modo RA en OnClickSelectDiscoP240Phase2Step7");
		in_RA_mode = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:S23");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "S23", "13", "-1");
		//Lamado al metodo para buscar el segundo objeto en el modo RA
		GoToSearchObjetoLimpiezaPhase2Step7 ();
	}//cierra OnClickSelectDiscoP240Phase2Step7

	public void OnClickSelectObjLimpiezaPhase2Step7(string interface_from){
		Debug.LogError ("AppManager: Lamado al metodo OnClickSelectObjLimpiezaPhase2Step7 - Click en boton Select!!");
		
		//Llamando al destroy de las posibles instancias existentes de la interfaz MenuOfStepsPhase1:
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");
		
		//Setting RA_mode en false porque ya se termina el modo RA aqui:
		Debug.Log ("--> AppManager: Finalizando modo RA en OnClickSelectObjLimpiezaPhase2Step7");
		in_RA_mode = false;
		
		//registrando en el sistema que ya se ha terminado esta actividad satisfactoriamente:
		steps_phase_two_completed [6].activity_tools_and_products = true;
		steps_phase_two_completed [6].CheckStepCompletion ();
		
		//guardando el estado en el que se encuentra la aplicacion en este momento:
		SaveDataForStudent ();

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:S24");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "S24", "13", "-1");
		
		if (steps_phase_two_completed [6].step_completed) //Lamado al metodo que muestra el listado de pasos de la FASE 2. Porque ya se han completado todos los pasos
			GoToSubMenuStepsLijadoInteriores ();
		else
			GoToActivitiesPhase2Step7 (); //en este caso no se han completado las actividades y por lo tanto se regresa al menu de actividades
	} //cierra OnClickSelectObjLimpiezaPhase2Step7

	public void OnClickSelectDiscoP320Phase2Step8(string interface_from){
		Debug.LogError ("AppManager: Lamado al metodo OnClickSelectDiscoP320Phase2Step8 - Click en boton Select!!");
		//Llamando al destroy de las posibles instancias existentes de la interfaz MenuOfStepsPhase1:
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");
		//Setting RA_mode en false porque ya se termina el modo RA aqui:
		Debug.Log ("--> AppManager: Finalizando modo RA en OnClickSelectDiscoP320Phase2Step8");
		in_RA_mode = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:25");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "S25", "14", "-1");
		//Lamado al metodo para buscar el segundo objeto en el modo RA
		GoToSearchObjetoLimpiezaPhase2Step8 ();
	}

	public void OnClickSelectObjLimpiezaPhase2Step8(string interface_from){
		Debug.LogError ("AppManager: Lamado al metodo OnClickSelectObjLimpiezaPhase2Step8 - Click en boton Select!!");
		
		//Llamando al destroy de las posibles instancias existentes de la interfaz MenuOfStepsPhase1:
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");
		
		//Setting RA_mode en false porque ya se termina el modo RA aqui:
		Debug.Log ("--> AppManager: Finalizando modo RA en OnClickSelectObjLimpiezaPhase2Step8");
		in_RA_mode = false;
		
		//registrando en el sistema que ya se ha terminado esta actividad satisfactoriamente:
		steps_phase_two_completed [7].activity_tools_and_products = true;
		steps_phase_two_completed [7].CheckStepCompletion ();
		
		//guardando el estado en el que se encuentra la aplicacion en este momento:
		SaveDataForStudent ();

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:S26");
		NavigationControllerObject.navigation.RegistrarInterfazModoGuiado (this.codigo_estudiante, fecha, "S26", "14", "-1");
		
		if (steps_phase_two_completed [7].step_completed) //Lamado al metodo que muestra el listado de pasos de la FASE 2. Porque ya se han completado todos los pasos
			GoToSubMenuStepsLijadoInteriores ();
		else
			GoToActivitiesPhase2Step8 (); //en este caso no se han completado las actividades y por lo tanto se regresa al menu de actividades
	} // cierra OnClickSelectObjLimpiezaPhase2Step8




	/// <summary>
	/// This method destroies all the objects in memory identified with a specific tag
	/// The string containing the tag of the object that should be deleted. Bear in mind that the object should be tagged in the unity editor using the tag attribute.
	/// </summary>
	/// <param name="object_tag">Object_tag: </param>
	public void DestroyInstancesWithTag(string object_tag){
		Debug.Log ("Ingresa a la funcion para DESTROY interfaces!!!");
		
		GameObject[] arreglo = GameObject.FindGameObjectsWithTag (object_tag);
		
		foreach(GameObject obj_app in arreglo){
			Debug.Log("--> INSTANCIA A DESTRUIR: " + obj_app.name);
			DestroyImmediate(obj_app);
			
		}
	}

	//**************************************************************************************************************************************************************************************
	//**************************************************************************************************************************************************************************************
	//**************************************************************************************************************************************************************************************
	//From now on, the methods that control the EVALUATION MODE

	public void GoToMenuPhasesEvaluationMode(){
		Debug.LogError ("Llamado al metodo go to Menu Phases EVALUATION MODE");

		//validando que no vamos a iniciar el modo informativo:
		in_informative_mode = false;

		if (current_interface == CurrentInterface.SELECTION_OF_MODE)
			Destroy (selectionOfMode_interface_instance);
		else if (current_interface == CurrentInterface.MENU_STEPS_PHASE1_EV) 
			Destroy (menuStepsPhase1_int_eval_instance);
		else if (current_interface == CurrentInterface.MENU_STEPS_PHASE2_EV)
			Destroy (menuStepsPhase2_int_eval_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_PRODUCTS_TUTORIAL)
			Destroy (TutorialPhaseTwoSearchProd_interface_instance);
		
		current_interface = CurrentInterface.MENU_PHASES_EV;
		
		menuProcessPhases_int_eval_instance = Instantiate (menuProcessPhasesEval);
		CanvasProcessPhasesMangEvaluation cProcessPhaseManager = menuProcessPhases_int_eval_instance.GetComponent<CanvasProcessPhasesMangEvaluation> ();
		cProcessPhaseManager.titulo = this.title_ordering_phases;
		//asignando imagenes a los botones de la interfaz
		cProcessPhaseManager.introduction_text_path = this.menuPhases_eval_introduction_text_path;

		string[] imagenes = new string[6];
		imagenes [0] = this.menuPhases_interface_button_uno_image;
		imagenes [1] = this.menuPhases_interface_button_dos_image;
		imagenes [2] = this.menuPhases_interface_button_tres_image;
		imagenes [3] = this.menuPhases_interface_button_cuatro_image;
		imagenes [4] = this.menuPhases_interface_button_cinco_image;
		imagenes [5] = this.menuPhases_interface_button_seis_image;

		string[] imagenes_gray = new string[6];
		imagenes_gray [0] = this.menuPhases_interface_button_uno_image; //la primera imagen va en color porque no se debe deshabilitar
		imagenes_gray [1] = this.menuPhases_int_btn_dos_image_gray;
		imagenes_gray [2] = this.menuPhases_int_btn_tres_image_gray;
		imagenes_gray [3] = this.menuPhases_int_btn_cuatro_image_gray;
		imagenes_gray [4] = this.menuPhases_int_btn_cinco_image_gray;
		imagenes_gray [5] = this.menuPhases_int_btn_seis_image_gray;

		string[] nombres_pasos = new string[6];
		nombres_pasos [0] = menuPhases_int_eval_button_uno_text;
		nombres_pasos [1] = menuPhases_int_eval_button_dos_text;
		nombres_pasos [2] = menuPhases_int_eval_button_tres_text;
		nombres_pasos [3] = menuPhases_int_eval_button_cuatro_text;
		nombres_pasos [4] = menuPhases_int_eval_button_cinco_text;
		nombres_pasos [5] = menuPhases_int_eval_button_seis_text;

		//vector que contendra las acciones disponibles, es decir las funciones que se pueden llamar:
		System.Action[] acciones_disponibles;
		acciones_disponibles = new System.Action[6];

		acciones_disponibles [0] += GoToMenuStepsPhase1Eval;
		acciones_disponibles [1] += GoToMenuStepsPhase2Eval;
		acciones_disponibles [2] += GoToMenuStepsPhase2Eval;
		acciones_disponibles [3] += GoToMenuStepsPhase2Eval;
		acciones_disponibles [4] += GoToMenuStepsPhase2Eval;
		acciones_disponibles [5] += GoToMenuStepsPhase2Eval;

		//Vector que almacena las acciones asignadas segun el orden aleatorio que se establezca:
		System.Action[] acciones_asignadas;
		acciones_asignadas = new System.Action[6];

		//Vector que almacena las imagenes asignadas segun el orden aleatorio:
		string[] imags_asignadas = new string[6];

		//Vector que almacena los nombres de fase asignados segun el orden aleatorio:
		string[] nombres_asignados = new string[6];

		//Vector que almacena las imagenes en gris para mostrar los pasos inhabilitados:
		string[] imgs_gray_asignadas = new string[6];

		//Vector que almacena los numeros de las fases segun el orden aleatorio:
		int[] numeros_fase = new int[6];

		//Vector que almacena los numeros de las fases en el ORDEN ORIGINAL:
		int[] numeros_originales = new int[6];

		numeros_originales [0] = 1;
		numeros_originales [1] = 2;
		numeros_originales [2] = 3;
		numeros_originales [3] = 4;
		numeros_originales [4] = 5;
		numeros_originales [5] = 6;

		List<int> randomNumbers = new List<int>();

		Debug.Log ("Se va a comenzar el ciclo de definicion de numeros");
		for (int i = 1; i <= 6; i++)
			randomNumbers.Add(i);

		foreach (int num in randomNumbers) {
			Debug.Log("Numero: " + num);
		}

		for (int j = 0; j <= 5; j++) {
			if(randomNumbers.Count <= 0)
				j=6;
			int index_num = UnityEngine.Random.Range(0,randomNumbers.Count);
			int rand_num = randomNumbers[index_num];
			Debug.Log ("rand_num=" + rand_num);
			//llenando los vectores con la info correspondiente:
			numeros_fase[rand_num-1] = numeros_originales[j];
			acciones_asignadas[rand_num-1] = acciones_disponibles[j];
			imags_asignadas[rand_num-1] = imagenes[j];
			nombres_asignados[rand_num-1] = nombres_pasos[j];
			imgs_gray_asignadas[rand_num-1] = imagenes_gray[j];
			//eliminando el elemento de la lista para acortar el vector y evitar repetir numeros:
			randomNumbers.RemoveAt(index_num);
			Debug.Log("Iteracion asignando: " + j);
		}

		for (int k = 0; k <= 5; k++) {
			Debug.Log ("accion " + k + ": " + acciones_asignadas[0].ToString());
		}

		//asignando imagenes y textos a los botones de acuerdo con el orden aleatorio definido:
		cProcessPhaseManager.image_uno_limpieza = imags_asignadas[0];
		cProcessPhaseManager.image_dos_matizado = imags_asignadas[1];
		cProcessPhaseManager.image_tres_masillado = imags_asignadas[2];
		cProcessPhaseManager.image_cuatro_aparejado = imags_asignadas[3];
		cProcessPhaseManager.image_cinco_pintado = imags_asignadas[4];
		cProcessPhaseManager.image_seis_barnizado = imags_asignadas[5];
		//asignando textos a los botones de la interfaz de acuerdo con el orden aleatorio definido:
		cProcessPhaseManager.button_uno_text_limpieza = nombres_asignados[0];
		cProcessPhaseManager.button_dos_text_matizado = nombres_asignados[1];
		cProcessPhaseManager.button_tres_text_masillado = nombres_asignados[2];
		cProcessPhaseManager.button_cuatro_text_aparejado = nombres_asignados[3];
		cProcessPhaseManager.button_cinco_text_pintado = nombres_asignados[4];
		cProcessPhaseManager.button_seis_text_barnizado = nombres_asignados[5];

		//asignando las acciones que tendrian los botones
		cProcessPhaseManager.goToMenuStepsOfPhase1_action += acciones_asignadas[0];
		cProcessPhaseManager.goToMenuStepsOfPhase2_action += acciones_asignadas[1];
		cProcessPhaseManager.goToMenuStepsOfPhase3_action += acciones_asignadas[2];
		cProcessPhaseManager.goToMenuStepsOfPhase4_action += acciones_asignadas[3];
		cProcessPhaseManager.goToMenuStepsOfPhase5_action += acciones_asignadas[4];
		cProcessPhaseManager.goToMenuStepsOfPhase6_action += acciones_asignadas[5];

		cProcessPhaseManager.phase_number_button_one = numeros_fase [0];
		cProcessPhaseManager.phase_number_button_two = numeros_fase [1];
		cProcessPhaseManager.phase_number_button_three = numeros_fase [2];
		cProcessPhaseManager.phase_number_button_four = numeros_fase [3];
		cProcessPhaseManager.phase_number_button_five = numeros_fase [4];
		cProcessPhaseManager.phase_number_button_six = numeros_fase [5];
		//Se agrega el metodo que se debe ejecutar para notificarle al AppManager que ya se han organizado
		//los pasos correctamente: (ver metodo NotifyPhasesOrganized aqui en el AppManager)
		cProcessPhaseManager.NotifyPhasesOrganized += NotifyPhasesOrganized;
		//asignando la accion para regresar desde el menu de fases hacia seleccion de modo:
		cProcessPhaseManager.goBackToSelectionOfMode += GoToSelectionOfMode;

		//Se llama al metodo para organizar los pasos en el orden correcto cuando ya se han organizado antes por parte
		//del estudiante
		if (eval_mode_phases_organized) {

			//notificando al script que los pasos se van a organizar y activar/desactivar desde el AppManager
			cProcessPhaseManager.phases_organized_from_manager = true;
			
			//el boton del primer paso siempre estara activo por defecto porque se comienza por ahi:
			cProcessPhaseManager.step_btn_one_enable = true;
			cProcessPhaseManager.image_uno_limpieza = this.menuPhases_interface_button_uno_image;
			
			//asignando imagenes a los demas pasos dependiendo de si van habilitados o no:
			Debug.Log ("Fase1 completada?: " + phase_one_enable_eval_mode);
			if (phase_two_enable_eval_mode) {
				cProcessPhaseManager.step_btn_two_enable = true;
				cProcessPhaseManager.image_dos_matizado= this.menuPhases_interface_button_dos_image;
			} else {
				cProcessPhaseManager.step_btn_two_enable = false;
				cProcessPhaseManager.image_dos_matizado = this.menuPhases_int_btn_dos_image_gray;
			}
			
			Debug.Log ("Fase2 completada?: " + phase_two_enable_eval_mode);
			if (phase_three_enable_eval_mode) {
				cProcessPhaseManager.step_btn_three_enable = true;
				cProcessPhaseManager.image_tres_masillado = this.menuPhases_interface_button_tres_image;
			} else {
				cProcessPhaseManager.step_btn_three_enable = false;
				cProcessPhaseManager.image_tres_masillado = this.menuPhases_int_btn_tres_image_gray;
			}
			
			Debug.Log ("Fase3 completada?: " + phase_three_enable_eval_mode);
			if (phase_four_enable_eval_mode) {
				cProcessPhaseManager.step_btn_four_enable = true;
				cProcessPhaseManager.image_cuatro_aparejado = this.menuPhases_interface_button_cuatro_image;
			} else {
				cProcessPhaseManager.step_btn_four_enable = false;
				cProcessPhaseManager.image_cuatro_aparejado = this.menuPhases_int_btn_cuatro_image_gray;
			}
			
			Debug.Log ("Fase4 completado?: " + phase_four_enable_eval_mode);
			if (phase_five_enable_eval_mode) {
				cProcessPhaseManager.step_btn_five_enable = true;
				cProcessPhaseManager.image_cinco_pintado = this.menuPhases_interface_button_cinco_image;
			} else {
				cProcessPhaseManager.step_btn_five_enable = false;
				cProcessPhaseManager.image_cinco_pintado = this.menuPhases_int_btn_cinco_image_gray;
			}
			
			Debug.Log ("Fase5 completado?: " + phase_five_enable_eval_mode);
			if (phase_six_enable_eval_mode) {
				cProcessPhaseManager.step_btn_six_enable = true;
				cProcessPhaseManager.image_seis_barnizado = this.menuPhases_interface_button_seis_image;
			} else {
				cProcessPhaseManager.step_btn_six_enable = false;
				cProcessPhaseManager.image_seis_barnizado = this.menuPhases_int_btn_seis_image_gray;
			}
			
			Debug.Log ("Paso6 completado?: " + phase_six_enable_eval_mode);

			//asignando textos a los botones de la interfaz:
			cProcessPhaseManager.button_uno_text_limpieza = this.menuPhases_int_eval_button_uno_text;
			cProcessPhaseManager.button_dos_text_matizado = this.menuPhases_int_eval_button_dos_text;
			cProcessPhaseManager.button_tres_text_masillado = this.menuPhases_int_eval_button_tres_text;
			cProcessPhaseManager.button_cuatro_text_aparejado = this.menuPhases_int_eval_button_cuatro_text;
			cProcessPhaseManager.button_cinco_text_pintado = this.menuPhases_int_eval_button_cinco_text;
			cProcessPhaseManager.button_seis_text_barnizado = this.menuPhases_int_eval_button_seis_text;
			//estas acciones disponibles se definen un poco mas arriba en este script:
			cProcessPhaseManager.goToMenuStepsOfPhase1_action += acciones_disponibles [0];
			cProcessPhaseManager.goToMenuStepsOfPhase2_action += acciones_disponibles [1];
			cProcessPhaseManager.goToMenuStepsOfPhase3_action += acciones_disponibles [2];
			cProcessPhaseManager.goToMenuStepsOfPhase4_action += acciones_disponibles [3];
			cProcessPhaseManager.goToMenuStepsOfPhase5_action += acciones_disponibles [4];
			cProcessPhaseManager.goToMenuStepsOfPhase6_action += acciones_disponibles [5];

			cProcessPhaseManager.phase_number_button_one = numeros_originales [0];
			cProcessPhaseManager.phase_number_button_two = numeros_originales [1];
			cProcessPhaseManager.phase_number_button_three = numeros_originales [2];
			cProcessPhaseManager.phase_number_button_four = numeros_originales [3];
			cProcessPhaseManager.phase_number_button_five = numeros_originales [4];
			cProcessPhaseManager.phase_number_button_six = numeros_originales [5];
			//llamado al metodo que coloca los botones en el orden correcto:
			cProcessPhaseManager.OrganizarPasosOrdenCorrecto ();
		} else { //si los pasos no se han organizado entonces se habilita la posibilidad de que una vez se organicen
				//se deshabiliten todos los iconos menos el primero
			cProcessPhaseManager.phases_organized_from_manager = false;
			cProcessPhaseManager.imgs_gray_random_phases = imgs_gray_asignadas;
		}

		interfaceComingBackFrom = "MenuPhases";
		goBackFromOtherInterface = true;
		//registrando navegacion:
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: IE1");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "IE1", "0", "-1","eval");
	}

	/// <summary>
	/// Notifies that the phases are organized correctly in order to avoid restarting the interface when it is instatiated again
	/// </summary>
	/// <param name="organized">If set to <c>true</c> organized.</param>
	public void NotifyPhasesOrganized(bool organized){
		//normalmente aca se notifica que es true:
		Debug.Log ("Llamado al metodo de notificacion de fases no organizados con organized = " + organized);
		this.eval_mode_phases_organized = organized;
		//Si las fases ya se han organizado, entonces se guarda el estado de la aplicacion:
		if (organized)
			SaveDataForStudent ();
	}

	/// <summary>
	/// Goes to menu steps phase1 in eval mode (for organizing the steps)
	/// </summary>
	public void GoToMenuStepsPhase1Eval(){
		Debug.LogError ("Llamado al metodo go to Menu steps phase 1 in Evaluation Mode");

		if (current_interface == CurrentInterface.MENU_PHASES_EV)
			Destroy (menuProcessPhases_int_eval_instance);
		else if (current_interface == CurrentInterface.ACTIVITIES_PHASE1_STEP1_EV)
			Destroy (ActivitiesForPhase1Step1_int_eval_instance);
		else if (current_interface == CurrentInterface.ACTIVITIES_PHASE1_STEP2_EV)
			Destroy (ActivitiesForPhase1Step2_int_eval_instance);
		else if (current_interface == CurrentInterface.ACTIVITIES_PHASE1_STEP3_EV)
			Destroy (ActivitiesForPhase1Step3_int_eval_instance);
		else if (current_interface == CurrentInterface.ACTIVITIES_PHASE1_STEP4_EV)
			Destroy (ActivitiesForPhase1Step4_int_eval_instance);
		else if (current_interface == CurrentInterface.ACTIVITIES_PHASE1_STEP5_EV)
			Destroy (ActivitiesForPhase1Step5_int_eval_instance);
		else if (current_interface == CurrentInterface.ACTIVITIES_PHASE1_STEP6_EV)
			Destroy (ActivitiesForPhase1Step6_int_eval_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_CAR_HOOD_EVAL)
			Destroy (SearchCapoCarroEvaluationMode_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP2_EV || current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP3_EV || current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP4_EV || current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP5_EV || current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP6_EV)
			Destroy (ARSearch_eval_mode_instance);
				
		processOrder = 0;
		
		current_interface = CurrentInterface.MENU_STEPS_PHASE1_EV;
		
		//Llamado al metodo para destruir instancias existentes de esta interfaz para evitar duplicidad:
		//esto se hace antes de instanciar la nueva interfaz mas adelante
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhase1");
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhaseOneEval");
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhaseTwoEval");
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");

		Debug.Log ("SE VAN A IMPRIMIR LAS INSTANCIAS DISPONIBLES:");
		MenuOfStepsPhaseOneManagerEval[] arreglo = FindObjectsOfType(typeof(MenuOfStepsPhaseOneManagerEval)) as MenuOfStepsPhaseOneManagerEval[];
		foreach(MenuOfStepsPhaseOneManagerEval canv in arreglo){
			Debug.Log("--> INSTANCIA " + canv.name);
		}

		Debug.Log ("SE VAN A IMPRIMIR LAS INSTANCIAS NORMALES DISPONIBLES:");
		MenuOfStepsPhase1Manager[] arreglo_dos = FindObjectsOfType(typeof(MenuOfStepsPhase1Manager)) as MenuOfStepsPhase1Manager[];
		foreach(MenuOfStepsPhase1Manager canv in arreglo_dos){
			Debug.Log("--> INSTANCIA Normal: " + canv.name);
		}
		
		menuStepsPhase1_int_eval_instance = Instantiate (menuStepsPhase1Eval);
		//Es importante asignarle un nombre a la interfaz para despues poder destruir todas las instancias 
		//de esa interfaz:
		menuStepsPhase1_int_eval_instance.name = "InterfaceMenuOfStepsPhase1Eval";
		//Obteniendo referencia al script
		MenuOfStepsPhaseOneManagerEval cMenuStepsPhase1Manager = menuStepsPhase1_int_eval_instance.GetComponent<MenuOfStepsPhaseOneManagerEval> ();
		Debug.Log ("El titulo de la interfaz desde el AppManager es: " + menuStepsPhase1_int_eval_title);
		cMenuStepsPhase1Manager.titulo = menuStepsPhase1_int_eval_title;
		cMenuStepsPhase1Manager.introduction_text_path = menuStepsPhase1_introduction_text_path_eval;
		cMenuStepsPhase1Manager.image_header_phase1 = menuStepsPhase1_image_header;
		Debug.Log ("Ya se ha cargado la introduccion y el header..." + menuStepsPhase1_int_eval_title);


		string[] imagenes = new string[6];
		imagenes [0] = this.menuStepsPhase1_interface_button_uno_image;
		imagenes [1] = this.menuStepsPhase1_interface_button_dos_image;
		imagenes [2] = this.menuStepsPhase1_interface_button_tres_image;
		imagenes [3] = this.menuStepsPhase1_interface_button_cuatro_image;
		imagenes [4] = this.menuStepsPhase1_interface_button_cinco_image;
		imagenes [5] = this.menuStepsPhase1_interface_button_seis_image;

		string[] imagenes_gray = new string[6];
		imagenes_gray [0] = menuStepsPhase1_interface_button_uno_image; //la primera imagen va en color porque no se debe deshabilitar
		imagenes_gray [1] = menuStepsPhase1_int_btn_dos_image_gray;
		imagenes_gray [2] = menuStepsPhase1_int_btn_tres_image_gray;
		imagenes_gray [3] = menuStepsPhase1_int_btn_cuatro_image_gray;
		imagenes_gray [4] = menuStepsPhase1_int_btn_cinco_image_gray;
		imagenes_gray [5] = menuStepsPhase1_int_btn_seis_image_gray;
		
		string[] nombres_pasos = new string[6];
		nombres_pasos [0] = this.menuStepsPhase1_button_uno_text_eval;
		nombres_pasos [1] = this.menuStepsPhase1_button_dos_text_eval;
		nombres_pasos [2] = this.menuStepsPhase1_button_tres_text_eval;
		nombres_pasos [3] = this.menuStepsPhase1_button_cuatro_text_eval;
		nombres_pasos [4] = this.menuStepsPhase1_button_cinco_text_eval;
		nombres_pasos [5] = this.menuStepsPhase1_button_seis_text_eval;
		
		//vector que contendra las acciones disponibles, es decir las funciones que se pueden llamar:
		System.Action[] acciones_disponibles;
		acciones_disponibles = new System.Action[6];
		
		acciones_disponibles [0] += GoToActivitiesPhase1Step1EvalMode;
		acciones_disponibles [1] += GoToActivitiesPhase1Step2EvalMode;
		acciones_disponibles [2] += GoToActivitiesPhase1Step3EvalMode;
		acciones_disponibles [3] += GoToActivitiesPhase1Step4EvalMode;
		acciones_disponibles [4] += GoToActivitiesPhase1Step5EvalMode;
		acciones_disponibles [5] += GoToActivitiesPhase1Step6EvalMode;
		
		//Vector que almacena las acciones asignadas segun el orden aleatorio que se establezca:
		System.Action[] acciones_asignadas;
		acciones_asignadas = new System.Action[6];
		
		//Vector que almacena las imagenes asignadas segun el orden aleatorio:
		string[] imags_asignadas = new string[6];

		//Vector que almacena las imagenes en gris para mostrar los pasos inhabilitados:
		string[] imgs_gray_asignadas = new string[6];
		
		//Vector que almacena los nombres de fase asignados segun el orden aleatorio:
		string[] nombres_asignados = new string[6];
		
		//Vector que almacena los numeros de las fases segun el orden aleatorio:
		int[] numeros_fase = new int[6];
		
		//Vector que almacena los numeros de las fases en el ORDEN ORIGINAL:
		int[] numeros_originales = new int[6];
		
		numeros_originales [0] = 1;
		numeros_originales [1] = 2;
		numeros_originales [2] = 3;
		numeros_originales [3] = 4;
		numeros_originales [4] = 5;
		numeros_originales [5] = 6;
		
				
		List<int> randomNumbers = new List<int>();
		
		Debug.Log ("AppManager.GoToMenuStepsPhase1Eval: Se va a comenzar el ciclo de definicion de numeros");
		for (int i = 1; i <= 6; i++)
			randomNumbers.Add(i);
		/*
		foreach (int num in randomNumbers) {
			Debug.Log("Numero: " + num);
		}
		*/
		
		for (int j = 0; j <= 5; j++) {
			if(randomNumbers.Count <= 0)
				j=6;
			int index_num = UnityEngine.Random.Range(0,randomNumbers.Count);
			int rand_num = randomNumbers[index_num];
			//Debug.Log ("rand_num=" + rand_num);
			//llenando los vectores con la info correspondiente:
			numeros_fase[rand_num-1] = numeros_originales[j];
			acciones_asignadas[rand_num-1] = acciones_disponibles[j];
			imags_asignadas[rand_num-1] = imagenes[j];
			nombres_asignados[rand_num-1] = nombres_pasos[j];
			imgs_gray_asignadas[rand_num-1] = imagenes_gray[j];
			//eliminando el elemento de la lista para acortar el vector y evitar repetir numeros:
			randomNumbers.RemoveAt(index_num);
			//Debug.Log("AppManager.GoToMenuStepsPhase1Eval: Iteracion asignando: " + j);
		}
		
		for (int k = 0; k <= 5; k++) {
			Debug.Log ("AppManager.GoToMenuStepsPhase1Eval accion " + k + ": " + acciones_asignadas[0].ToString());
		}

		//asignando imagenes:
		cMenuStepsPhase1Manager.image_uno_capo_carro = imags_asignadas[0];
		cMenuStepsPhase1Manager.image_dos_limpieza = imags_asignadas[1];
		cMenuStepsPhase1Manager.image_tres_secado = imags_asignadas[2];
		cMenuStepsPhase1Manager.image_cuatro_irregularidades = imags_asignadas[3];
		cMenuStepsPhase1Manager.image_cinco_corregir = imags_asignadas[4];
		cMenuStepsPhase1Manager.image_seis_desengrasar = imags_asignadas[5];
		//Asignando textos para cada boton en el orden aleatorio correspondiente:
		cMenuStepsPhase1Manager.button_uno_text_capo_carro = nombres_asignados[0];
		cMenuStepsPhase1Manager.button_dos_text_limpieza =  nombres_asignados[1];
		cMenuStepsPhase1Manager.button_tres_text_secado = nombres_asignados[2];
		cMenuStepsPhase1Manager.button_cuatro_text_irregularidades =  nombres_asignados[3];
		cMenuStepsPhase1Manager.button_cinco_text_corregir =  nombres_asignados[4];
		cMenuStepsPhase1Manager.button_seis_text_desengrasar =  nombres_asignados[5];
			
		cMenuStepsPhase1Manager.goBackToMenuPhases += GoToMenuPhasesEvaluationMode;

		//asignando las acciones para cada boton
		cMenuStepsPhase1Manager.goToActivitiesPhase1Step1 += acciones_asignadas[0];
		cMenuStepsPhase1Manager.goToActivitiesPhase1Step2 += acciones_asignadas[1];
		cMenuStepsPhase1Manager.goToActivitiesPhase1Step3 += acciones_asignadas[2];
		cMenuStepsPhase1Manager.goToActivitiesPhase1Step4 += acciones_asignadas[3];
		cMenuStepsPhase1Manager.goToActivitiesPhase1Step5 += acciones_asignadas[4];
		cMenuStepsPhase1Manager.goToActivitiesPhase1Step6 += acciones_asignadas[5];

		//asignando los numeros de fase correspondientes:
		cMenuStepsPhase1Manager.phase_number_button_one = numeros_fase [0];
		cMenuStepsPhase1Manager.phase_number_button_two = numeros_fase [1];
		cMenuStepsPhase1Manager.phase_number_button_three = numeros_fase [2];
		cMenuStepsPhase1Manager.phase_number_button_four = numeros_fase [3];
		cMenuStepsPhase1Manager.phase_number_button_five = numeros_fase [4];
		cMenuStepsPhase1Manager.phase_number_button_six = numeros_fase [5];

		//Se agrega el metodo que se debe ejecutar para notificarle al AppManager que ya se han organizado
		//los pasos correctamente: (ver metodo NotifyPhasesOrganized aqui en el AppManager)
		cMenuStepsPhase1Manager.NotifyStepsOrganized += NotifyStepsOrganizedPhase1;

		if (eval_mode_phase1_steps_organized) {

			//notificando al script que los pasos se van a organizar y activar/desactivar desde el AppManager
			cMenuStepsPhase1Manager.steps_organized_from_manager = true;

			//el boton del primer paso siempre estara activo por defecto porque se comienza por ahi:
			cMenuStepsPhase1Manager.step_btn_one_enable = true;
			cMenuStepsPhase1Manager.image_uno_capo_carro = this.menuStepsPhase1_interface_button_uno_image;

			//asignando imagenes a los demas pasos dependiendo de si van habilitados o no:
			Debug.Log ("Paso1 completado?: " + steps_p_one_eval_completed [0].step_completed);
			if (steps_p_one_eval_completed [0].step_completed) {
				cMenuStepsPhase1Manager.step_btn_two_enable = true;
				cMenuStepsPhase1Manager.image_dos_limpieza = this.menuStepsPhase1_interface_button_dos_image;
			} else {
				cMenuStepsPhase1Manager.step_btn_two_enable = false;
				cMenuStepsPhase1Manager.image_dos_limpieza = this.menuStepsPhase1_int_btn_dos_image_gray;
			}

			Debug.Log ("Paso2 completado?: " + steps_p_one_eval_completed [1].step_completed);
			if (steps_p_one_eval_completed [1].step_completed) {
				cMenuStepsPhase1Manager.step_btn_three_enable = true;
				cMenuStepsPhase1Manager.image_tres_secado = this.menuStepsPhase1_interface_button_tres_image;
			} else {
				cMenuStepsPhase1Manager.step_btn_three_enable = false;
				cMenuStepsPhase1Manager.image_tres_secado = this.menuStepsPhase1_int_btn_tres_image_gray;
			}

			Debug.Log ("Paso3 completado?: " + steps_p_one_eval_completed [2].step_completed);
			if (steps_p_one_eval_completed [2].step_completed) {
				cMenuStepsPhase1Manager.step_btn_four_enable = true;
				cMenuStepsPhase1Manager.image_cuatro_irregularidades = this.menuStepsPhase1_interface_button_cuatro_image;
			} else {
				cMenuStepsPhase1Manager.step_btn_four_enable = false;
				cMenuStepsPhase1Manager.image_cuatro_irregularidades = this.menuStepsPhase1_int_btn_cuatro_image_gray;
			}

			Debug.Log ("Paso4 completado?: " + steps_p_one_eval_completed [3].step_completed);
			if (steps_p_one_eval_completed [3].step_completed) {
				cMenuStepsPhase1Manager.step_btn_five_enable = true;
				cMenuStepsPhase1Manager.image_cinco_corregir = this.menuStepsPhase1_interface_button_cinco_image;
			} else {
				cMenuStepsPhase1Manager.step_btn_five_enable = false;
				cMenuStepsPhase1Manager.image_cinco_corregir = this.menuStepsPhase1_int_btn_cinco_image_gray;
			}

			Debug.Log ("Paso5 completado?: " + steps_p_one_eval_completed [4].step_completed);
			if (steps_p_one_eval_completed [4].step_completed) {
				cMenuStepsPhase1Manager.step_btn_six_enable = true;
				cMenuStepsPhase1Manager.image_seis_desengrasar = this.menuStepsPhase1_interface_button_seis_image;
			} else {
				cMenuStepsPhase1Manager.step_btn_six_enable = false;
				cMenuStepsPhase1Manager.image_seis_desengrasar = this.menuStepsPhase1_int_btn_seis_image_gray;
			}

			Debug.Log ("Paso6 completado?: " + steps_p_one_eval_completed [5].step_completed);
			//aca se debe validar que si se completan los pasos de la fase 1 entonces se puede iniciar
			//la fase 2. Cuando programe la parte de la fase 2 entonces habilito lo siguiente:
			if(steps_p_one_eval_completed[5].step_completed){
				phase_two_enable_eval_mode = true;
			}

			//Asignando textos
			cMenuStepsPhase1Manager.button_uno_text_capo_carro = this.menuStepsPhase1_button_uno_text_eval;
			cMenuStepsPhase1Manager.button_dos_text_limpieza = this.menuStepsPhase1_button_dos_text_eval;
			cMenuStepsPhase1Manager.button_tres_text_secado = this.menuStepsPhase1_button_tres_text_eval;
			cMenuStepsPhase1Manager.button_cuatro_text_irregularidades = this.menuStepsPhase1_button_cuatro_text_eval;
			cMenuStepsPhase1Manager.button_cinco_text_corregir = this.menuStepsPhase1_button_cinco_text_eval;
			cMenuStepsPhase1Manager.button_seis_text_desengrasar = this.menuStepsPhase1_button_seis_text_eval;
				
			//asignando las acciones para cada boton
			cMenuStepsPhase1Manager.goToActivitiesPhase1Step1 += acciones_disponibles [0];
			cMenuStepsPhase1Manager.goToActivitiesPhase1Step2 += acciones_disponibles [1];
			cMenuStepsPhase1Manager.goToActivitiesPhase1Step3 += acciones_disponibles [2];
			cMenuStepsPhase1Manager.goToActivitiesPhase1Step4 += acciones_disponibles [3];
			cMenuStepsPhase1Manager.goToActivitiesPhase1Step5 += acciones_disponibles [4];
			cMenuStepsPhase1Manager.goToActivitiesPhase1Step6 += acciones_disponibles [5];

			cMenuStepsPhase1Manager.phase_number_button_one = numeros_originales [0];
			cMenuStepsPhase1Manager.phase_number_button_two = numeros_originales [1];
			cMenuStepsPhase1Manager.phase_number_button_three = numeros_originales [2];
			cMenuStepsPhase1Manager.phase_number_button_four = numeros_originales [3];
			cMenuStepsPhase1Manager.phase_number_button_five = numeros_originales [4];
			cMenuStepsPhase1Manager.phase_number_button_six = numeros_originales [5];

			cMenuStepsPhase1Manager.OrganizarPasosOrdenCorrecto ();

		} else { //si el eval_mode_phase1_steps_organized es false
			//entonces se notifica al script MenuOfStepsPhaseOneManagerEval que los pasos no se organizaron desde el AppManager
			//y por tanto se pueden inhabilitar los pasos que sean necesarios cuando se terminan de organizar:
			cMenuStepsPhase1Manager.steps_organized_from_manager = false;
			cMenuStepsPhase1Manager.imgs_gray_random = imgs_gray_asignadas;

		}
		//La siguiente asignacion de imagenes es comun e independiente del orden de los pasos:
		//Asignando rutas a la imagenes de las fases para mostrarlos como guia para el estudiante en el encabezado de la interfaz:
		cMenuStepsPhase1Manager.image_phase1_path = phase1_with_text_image_path;
		cMenuStepsPhase1Manager.image_phase2_path = phase2_with_text_image_gray_path;
		cMenuStepsPhase1Manager.image_phase3_path = phase3_with_text_image_gray_path;
		cMenuStepsPhase1Manager.image_phase4_path = phase4_with_text_image_gray_path;
		cMenuStepsPhase1Manager.image_phase5_path = phase5_with_text_image_gray_path;
		cMenuStepsPhase1Manager.image_phase6_path = phase6_with_text_image_gray_path;

		goBackFromOtherInterface = true;
		interfaceComingBackFrom = "Phase1";

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: IE2");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "IE2", "0", "-1","eval");
	} //cierra GoToMenuStepsPhase1Eval


	/// <summary>
	/// Goes to menu steps phase2 in eval mode (for organizing the steps of phase2)
	/// </summary>
	public void GoToMenuStepsPhase2Eval(){
		Debug.LogError ("Llamado al metodo go to Menu steps phase 2 in Evaluation Mode");
		
		if (current_interface == CurrentInterface.MENU_PHASES_EV)
			Destroy (menuProcessPhases_int_eval_instance);
		else if (current_interface == CurrentInterface.ACTIVITIES_PHASE2_STEP1_EV)
			Destroy (ActivitiesForPhase2Step1_int_eval_instance);
		else if (current_interface == CurrentInterface.ACTIVITIES_PHASE2_STEP2_EV)
			Destroy (ActivitiesForPhase2Step2_int_eval_instance);
		else if (current_interface == CurrentInterface.ACTIVITIES_PHASE2_STEP3_EV)
			Destroy (ActivitiesForPhase2Step3_int_eval_instance);
		else if (current_interface == CurrentInterface.ACTIVITIES_PHASE2_STEP4_EV)
			Destroy (ActivitiesForPhase2Step4_int_eval_instance);
		else if (current_interface == CurrentInterface.ACTIVITIES_PHASE2_STEP5_EV)
			Destroy (ActivitiesForPhase2Step5_int_eval_instance);
		else if (current_interface == CurrentInterface.ACTIVITIES_PHASE2_STEP6_EV)
			Destroy (ActivitiesForPhase2Step6_int_eval_instance);
		else if (current_interface == CurrentInterface.ACTIVITIES_PHASE2_STEP7_EV)
			Destroy (ActivitiesForPhase2Step7_int_eval_instance);
		else if (current_interface == CurrentInterface.ACTIVITIES_PHASE2_STEP8_EV)
			Destroy (ActivitiesForPhase2Step8_int_eval_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_CAR_HOOD_EVAL)
			Destroy (SearchCapoCarroEvaluationMode_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP2_EV || current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP3_EV || current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP4_EV || current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP5_EV || current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP6_EV || current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP7_EV || current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP8_EV)
			Destroy (ARSearch_eval_mode_instance);
		
		processOrder = 0;
		
		current_interface = CurrentInterface.MENU_STEPS_PHASE2_EV;
		
		//Llamado al metodo para destruir instancias existentes de esta interfaz para evitar duplicidad:
		//esto se hace antes de instanciar la nueva interfaz mas adelante
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhase1");
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhaseTwoEval");
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");
		
		Debug.Log ("SE VAN A IMPRIMIR LAS INSTANCIAS DISPONIBLES:");
		MenuOfStepsPhaseOneManagerEval[] arreglo = FindObjectsOfType(typeof(MenuOfStepsPhaseOneManagerEval)) as MenuOfStepsPhaseOneManagerEval[];
		foreach(MenuOfStepsPhaseOneManagerEval canv in arreglo){
			Debug.Log("--> INSTANCIA " + canv.name);
		}
		
		Debug.Log ("SE VAN A IMPRIMIR LAS INSTANCIAS NORMALES DISPONIBLES:");
		MenuOfStepsPhase1Manager[] arreglo_dos = FindObjectsOfType(typeof(MenuOfStepsPhase1Manager)) as MenuOfStepsPhase1Manager[];
		foreach(MenuOfStepsPhase1Manager canv in arreglo_dos){
			Debug.Log("--> INSTANCIA Normal: " + canv.name);
		}
		
		menuStepsPhase2_int_eval_instance = Instantiate (menuStepsPhase2Eval);
		//Es importante asignarle un nombre a la interfaz para despues poder destruir todas las instancias 
		//de esa interfaz:
		menuStepsPhase2_int_eval_instance.name = "InterfaceMenuOfStepsPhase2Eval";
		//Obteniendo referencia al script
		MenuOfStepsPhaseTwoManagerEval cMenuStepsPhase2Manager = menuStepsPhase2_int_eval_instance.GetComponent<MenuOfStepsPhaseTwoManagerEval> ();
		Debug.Log ("El titulo de la interfaz desde el AppManager es: " + menuStepsPhase2_int_eval_title);
		cMenuStepsPhase2Manager.titulo = menuStepsPhase2_int_eval_title;
		cMenuStepsPhase2Manager.introduction_text_path = menuStepsPhase2_introduction_text_path_eval;
		cMenuStepsPhase2Manager.image_header_phase1 = menuStepsPhase2_image_header;
		
		
		string[] imagenes = new string[8];
		imagenes [0] = this.phaseTwo_img_step_one_no_text;
		imagenes [1] = this.phaseTwo_img_step_two_no_text;
		imagenes [2] = this.phaseTwo_img_step_three_no_text;
		imagenes [3] = this.phaseTwo_img_step_four_no_text;
		imagenes [4] = this.phaseTwo_img_step_five_no_text;
		imagenes [5] = this.phaseTwo_img_step_six_no_text;
		imagenes [6] = this.phaseTwo_img_step_seven_no_text;
		imagenes [7] = this.phaseTwo_img_step_eight_no_text;
		
		string[] imagenes_gray = new string[8];
		imagenes_gray [0] = phaseTwo_img_step_one_no_text; //la primera imagen va en color porque no se debe deshabilitar
		imagenes_gray [1] = stepsPhaseTwo_img_two_gray_notxt;
		imagenes_gray [2] = stepsPhaseTwo_img_three_gray_notxt;
		imagenes_gray [3] = stepsPhaseTwo_img_four_gray_notxt;
		imagenes_gray [4] = stepsPhaseTwo_img_five_gray_notxt;
		imagenes_gray [5] = stepsPhaseTwo_img_six_gray_notxt;
		imagenes_gray [6] = stepsPhaseTwo_img_seven_gray_notxt;
		imagenes_gray [7] = stepsPhaseTwo_img_eight_gray_notxt;
		
		string[] nombres_pasos = new string[8];
		nombres_pasos [0] = this.menuStepsPhase2_button_uno_text_eval;
		nombres_pasos [1] = this.menuStepsPhase2_button_dos_text_eval;
		nombres_pasos [2] = this.menuStepsPhase2_button_tres_text_eval;
		nombres_pasos [3] = this.menuStepsPhase2_button_cuatro_text_eval;
		nombres_pasos [4] = this.menuStepsPhase2_button_cinco_text_eval;
		nombres_pasos [5] = this.menuStepsPhase2_button_seis_text_eval;
		nombres_pasos [6] = this.menuStepsPhase2_button_siete_text_eval;
		nombres_pasos [7] = this.menuStepsPhase2_button_ocho_text_eval;
		
		//vector que contendra las acciones disponibles, es decir las funciones que se pueden llamar:
		System.Action[] acciones_disponibles;
		acciones_disponibles = new System.Action[8];
		
		acciones_disponibles [0] += GoToActivitiesPhase2Step1EvalMode;
		acciones_disponibles [1] += GoToActivitiesPhase2Step2EvalMode;
		acciones_disponibles [2] += GoToActivitiesPhase2Step3EvalMode;
		acciones_disponibles [3] += GoToActivitiesPhase2Step4EvalMode;
		acciones_disponibles [4] += GoToActivitiesPhase2Step5EvalMode;
		acciones_disponibles [5] += GoToActivitiesPhase2Step6EvalMode;
		acciones_disponibles [6] += GoToActivitiesPhase2Step7EvalMode;
		acciones_disponibles [7] += GoToActivitiesPhase2Step8EvalMode;
		
		//Vector que almacena las acciones asignadas segun el orden aleatorio que se establezca:
		System.Action[] acciones_asignadas;
		acciones_asignadas = new System.Action[8];
		
		//Vector que almacena las imagenes asignadas segun el orden aleatorio:
		string[] imags_asignadas = new string[8];
		
		//Vector que almacena las imagenes en gris para mostrar los pasos inhabilitados:
		string[] imgs_gray_asignadas = new string[8];
		
		//Vector que almacena los nombres de fase asignados segun el orden aleatorio:
		string[] nombres_asignados = new string[8];
		
		//Vector que almacena los numeros de las fases segun el orden aleatorio:
		int[] numeros_fase = new int[8];
		
		//Vector que almacena los numeros de las fases en el ORDEN ORIGINAL:
		int[] numeros_originales = new int[8];
		
		numeros_originales [0] = 1;
		numeros_originales [1] = 2;
		numeros_originales [2] = 3;
		numeros_originales [3] = 4;
		numeros_originales [4] = 5;
		numeros_originales [5] = 6;
		numeros_originales [6] = 7;
		numeros_originales [7] = 8;
		
		
		List<int> randomNumbers = new List<int>();
		
		Debug.Log ("AppManager.GoToMenuStepsPhase2Eval: Se va a comenzar el ciclo de definicion de numeros");
		for (int i = 1; i <= 8; i++)
			randomNumbers.Add(i);
		/*
		foreach (int num in randomNumbers) {
			Debug.Log("Numero: " + num);
		}
		*/
		
		for (int j = 0; j <= 7; j++) {
			if(randomNumbers.Count <= 0)
				j=8;
			int index_num = UnityEngine.Random.Range(0,randomNumbers.Count);
			int rand_num = randomNumbers[index_num];
			//Debug.Log ("rand_num=" + rand_num);
			//llenando los vectores con la info correspondiente:
			numeros_fase[rand_num-1] = numeros_originales[j];
			acciones_asignadas[rand_num-1] = acciones_disponibles[j];
			imags_asignadas[rand_num-1] = imagenes[j];
			nombres_asignados[rand_num-1] = nombres_pasos[j];
			imgs_gray_asignadas[rand_num-1] = imagenes_gray[j];
			//eliminando el elemento de la lista para acortar el vector y evitar repetir numeros:
			randomNumbers.RemoveAt(index_num);
			//Debug.Log("AppManager.GoToMenuStepsPhase1Eval: Iteracion asignando: " + j);
		}
		
		for (int k = 0; k <= 7; k++) {
			Debug.Log ("AppManager.GoToMenuStepsPhase2Eval accion " + k + ": " + acciones_asignadas[k].ToString());
		}
		
		//asignando imagenes:
		cMenuStepsPhase2Manager.img_one_to_order = imags_asignadas[0];
		cMenuStepsPhase2Manager.img_two_to_order = imags_asignadas[1];
		cMenuStepsPhase2Manager.img_three_to_order = imags_asignadas[2];
		cMenuStepsPhase2Manager.img_four_to_order = imags_asignadas[3];
		cMenuStepsPhase2Manager.img_five_to_order = imags_asignadas[4];
		cMenuStepsPhase2Manager.img_six_to_order = imags_asignadas[5];
		cMenuStepsPhase2Manager.img_seven_to_order = imags_asignadas[6];
		cMenuStepsPhase2Manager.img_eight_to_order = imags_asignadas[7];
		//Asignando textos para cada boton en el orden aleatorio correspondiente:
		cMenuStepsPhase2Manager.string_btn_one_text = nombres_asignados[0];
		cMenuStepsPhase2Manager.string_btn_two_text =  nombres_asignados[1];
		cMenuStepsPhase2Manager.string_btn_three_text = nombres_asignados[2];
		cMenuStepsPhase2Manager.string_btn_four_text =  nombres_asignados[3];
		cMenuStepsPhase2Manager.string_btn_five_text =  nombres_asignados[4];
		cMenuStepsPhase2Manager.string_btn_six_text =  nombres_asignados[5];
		cMenuStepsPhase2Manager.string_btn_seven_text =  nombres_asignados[6];
		cMenuStepsPhase2Manager.string_btn_eight_text =  nombres_asignados[7];
		
		cMenuStepsPhase2Manager.goBackToMenuPhases += GoToMenuPhasesEvaluationMode;
		
		//asignando las acciones para cada boton
		cMenuStepsPhase2Manager.goToActionBtnOne += acciones_asignadas[0];
		cMenuStepsPhase2Manager.goToActionBtnTwo += acciones_asignadas[1];
		cMenuStepsPhase2Manager.goToActionBtnThree += acciones_asignadas[2];
		cMenuStepsPhase2Manager.goToActionBtnFour += acciones_asignadas[3];
		cMenuStepsPhase2Manager.goToActionBtnFive += acciones_asignadas[4];
		cMenuStepsPhase2Manager.goToActionBtnSix += acciones_asignadas[5];
		cMenuStepsPhase2Manager.goToActionBtnSeven += acciones_asignadas[6];
		cMenuStepsPhase2Manager.goToActionBtnEight += acciones_asignadas[7];
		
		//asignando los numeros de fase correspondientes:
		cMenuStepsPhase2Manager.step_number_btn_one = numeros_fase [0];
		cMenuStepsPhase2Manager.step_number_btn_two = numeros_fase [1];
		cMenuStepsPhase2Manager.step_number_btn_three = numeros_fase [2];
		cMenuStepsPhase2Manager.step_number_btn_four = numeros_fase [3];
		cMenuStepsPhase2Manager.step_number_btn_five = numeros_fase [4];
		cMenuStepsPhase2Manager.step_number_btn_six = numeros_fase [5];
		cMenuStepsPhase2Manager.step_number_btn_seven = numeros_fase [6];
		cMenuStepsPhase2Manager.step_number_btn_eight = numeros_fase [7];
		
		//Se agrega el metodo que se debe ejecutar para notificarle al AppManager que ya se han organizado
		//los pasos correctamente: (ver metodo NotifyPhasesOrganized aqui en el AppManager)
		cMenuStepsPhase2Manager.NotifyStepsOrganized += NotifyStepsOrganizedPhase2;
		
		if (eval_mode_phase2_steps_organized) {
			
			//notificando al script que los pasos se van a organizar y activar/desactivar desde el AppManager
			cMenuStepsPhase2Manager.steps_organized_from_manager = true;
			
			//el boton del primer paso siempre estara activo por defecto porque se comienza por ahi:
			cMenuStepsPhase2Manager.step_btn_one_enable = true;
			cMenuStepsPhase2Manager.img_one_to_order = this.phaseTwo_img_step_one_no_text;
			
			//asignando imagenes a los demas pasos dependiendo de si van habilitados o no:
			Debug.Log ("Paso1 completado?: " + steps_p_two_eval_completed [0].step_completed);
			if (steps_p_two_eval_completed [0].step_completed) {
				cMenuStepsPhase2Manager.step_btn_two_enable = true;
				cMenuStepsPhase2Manager.img_two_to_order = this.phaseTwo_img_step_two_no_text;
			} else {
				cMenuStepsPhase2Manager.step_btn_two_enable = false;
				cMenuStepsPhase2Manager.img_two_to_order = this.stepsPhaseTwo_img_two_gray_notxt;
			}
			
			Debug.Log ("Paso2 completado?: " + steps_p_two_eval_completed [1].step_completed);
			if (steps_p_two_eval_completed [1].step_completed) {
				cMenuStepsPhase2Manager.step_btn_three_enable = true;
				cMenuStepsPhase2Manager.img_three_to_order = this.phaseTwo_img_step_three_no_text;
			} else {
				cMenuStepsPhase2Manager.step_btn_three_enable = false;
				cMenuStepsPhase2Manager.img_three_to_order = this.stepsPhaseTwo_img_three_gray_notxt;
			}
			
			Debug.Log ("Paso3 completado?: " + steps_p_two_eval_completed [2].step_completed);
			if (steps_p_two_eval_completed [2].step_completed) {
				cMenuStepsPhase2Manager.step_btn_four_enable = true;
				cMenuStepsPhase2Manager.img_four_to_order = this.phaseTwo_img_step_four_no_text;
			} else {
				cMenuStepsPhase2Manager.step_btn_four_enable = false;
				cMenuStepsPhase2Manager.img_four_to_order = this.stepsPhaseTwo_img_four_gray_notxt;
			}
			
			Debug.Log ("Paso4 completado?: " + steps_p_two_eval_completed [3].step_completed);
			if (steps_p_two_eval_completed [3].step_completed) {
				cMenuStepsPhase2Manager.step_btn_five_enable = true;
				cMenuStepsPhase2Manager.img_five_to_order = this.phaseTwo_img_step_five_no_text;
			} else {
				cMenuStepsPhase2Manager.step_btn_five_enable = false;
				cMenuStepsPhase2Manager.img_five_to_order = this.stepsPhaseTwo_img_five_gray_notxt;
			}
			
			Debug.Log ("Paso5 completado?: " + steps_p_two_eval_completed [4].step_completed);
			if (steps_p_two_eval_completed [4].step_completed) {
				cMenuStepsPhase2Manager.step_btn_six_enable = true;
				cMenuStepsPhase2Manager.img_six_to_order = this.phaseTwo_img_step_six_no_text;
			} else {
				cMenuStepsPhase2Manager.step_btn_six_enable = false;
				cMenuStepsPhase2Manager.img_six_to_order = this.stepsPhaseTwo_img_six_gray_notxt;
			}
			
			Debug.Log ("Paso6 completado?: " + steps_p_two_eval_completed [5].step_completed);
			if (steps_p_two_eval_completed [5].step_completed) {
				cMenuStepsPhase2Manager.step_btn_seven_enable = true;
				cMenuStepsPhase2Manager.img_seven_to_order = this.phaseTwo_img_step_seven_no_text;
			} else {
				cMenuStepsPhase2Manager.step_btn_seven_enable = false;
				cMenuStepsPhase2Manager.img_seven_to_order = this.stepsPhaseTwo_img_seven_gray_notxt;
			}

			Debug.Log ("Paso6 completado?: " + steps_p_two_eval_completed [6].step_completed);
			if (steps_p_two_eval_completed [6].step_completed) {
				cMenuStepsPhase2Manager.step_btn_eight_enable = true;
				cMenuStepsPhase2Manager.img_eight_to_order = this.phaseTwo_img_step_eight_no_text;
			} else {
				cMenuStepsPhase2Manager.step_btn_eight_enable = false;
				cMenuStepsPhase2Manager.img_eight_to_order = this.stepsPhaseTwo_img_eight_gray_notxt;
			}

			Debug.Log ("Paso7 completado?: " + steps_p_two_eval_completed [7].step_completed);
			//aca se debe validar que si se completan los pasos de la fase 1 entonces se puede iniciar
			//la fase 2. Cuando programe la parte de la fase 2 entonces habilito lo siguiente:
			//if(steps_p_one_eval_completed[5].step_completed){
			//habilitar AQUI LA FASE 3
			//}
			
			//Asignando textos
			cMenuStepsPhase2Manager.string_btn_one_text = this.menuStepsPhase2_button_uno_text_eval;
			cMenuStepsPhase2Manager.string_btn_two_text = this.menuStepsPhase2_button_dos_text_eval;
			cMenuStepsPhase2Manager.string_btn_three_text = this.menuStepsPhase2_button_tres_text_eval;
			cMenuStepsPhase2Manager.string_btn_four_text = this.menuStepsPhase2_button_cuatro_text_eval;
			cMenuStepsPhase2Manager.string_btn_five_text = this.menuStepsPhase2_button_cinco_text_eval;
			cMenuStepsPhase2Manager.string_btn_six_text = this.menuStepsPhase2_button_seis_text_eval;
			cMenuStepsPhase2Manager.string_btn_seven_text = this.menuStepsPhase2_button_siete_text_eval;
			cMenuStepsPhase2Manager.string_btn_eight_text = this.menuStepsPhase2_button_ocho_text_eval;

			
			//asignando las acciones para cada boton
			cMenuStepsPhase2Manager.goToActionBtnOne += acciones_disponibles [0];
			cMenuStepsPhase2Manager.goToActionBtnTwo += acciones_disponibles [1];
			cMenuStepsPhase2Manager.goToActionBtnThree += acciones_disponibles [2];
			cMenuStepsPhase2Manager.goToActionBtnFour += acciones_disponibles [3];
			cMenuStepsPhase2Manager.goToActionBtnFive += acciones_disponibles [4];
			cMenuStepsPhase2Manager.goToActionBtnSix += acciones_disponibles [5];
			cMenuStepsPhase2Manager.goToActionBtnSeven += acciones_disponibles [6];
			cMenuStepsPhase2Manager.goToActionBtnEight += acciones_disponibles [7];
			
			cMenuStepsPhase2Manager.step_number_btn_one = numeros_originales [0];
			cMenuStepsPhase2Manager.step_number_btn_two = numeros_originales [1];
			cMenuStepsPhase2Manager.step_number_btn_three = numeros_originales [2];
			cMenuStepsPhase2Manager.step_number_btn_four = numeros_originales [3];
			cMenuStepsPhase2Manager.step_number_btn_five = numeros_originales [4];
			cMenuStepsPhase2Manager.step_number_btn_six = numeros_originales [5];
			cMenuStepsPhase2Manager.step_number_btn_seven = numeros_originales [6];
			cMenuStepsPhase2Manager.step_number_btn_eight = numeros_originales [7];
					
			cMenuStepsPhase2Manager.OrganizarPasosOrdenCorrecto ();
			
		} else { //si el eval_mode_phase2_steps_organized es false
			//entonces se notifica al script MenuOfStepsPhaseOneManagerEval que los pasos no se organizaron desde el AppManager
			//y por tanto se pueden inhabilitar los pasos que sean necesarios cuando se terminan de organizar:
			cMenuStepsPhase2Manager.steps_organized_from_manager = false;
			cMenuStepsPhase2Manager.imgs_gray_random = imgs_gray_asignadas;
			
		}
		//La siguiente asignacion de imagenes es comun e independiente del orden de los pasos:
		//Asignando rutas a la imagenes de las fases para mostrarlos como guia para el estudiante en el encabezado de la interfaz:
		cMenuStepsPhase2Manager.image_phase1_path = phase1_with_text_image_gray_path;
		cMenuStepsPhase2Manager.image_phase2_path = phase2_with_text_image_path;
		cMenuStepsPhase2Manager.image_phase3_path = phase3_with_text_image_gray_path;
		cMenuStepsPhase2Manager.image_phase4_path = phase4_with_text_image_gray_path;
		cMenuStepsPhase2Manager.image_phase5_path = phase5_with_text_image_gray_path;
		cMenuStepsPhase2Manager.image_phase6_path = phase6_with_text_image_gray_path;
		
		goBackFromOtherInterface = true;
		interfaceComingBackFrom = "Phase2";

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: IE3");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "IE3", "0", "-1","eval");
	} //cierra GoToMenuStepsPhase2Eval

	//Este metodo se llama desde el MenuOfStepsPhaseOneManager para solicitar que los pasos
	//se organicen correctamente para poder deshabilitar todos los pasos y solamente habilitar el 
	//primero para que el estudiante comience a desarrollar cada uno:
	public void OrganizeStepsPhase1(){

		string[] imagenes = new string[6];
		imagenes [0] = this.menuStepsPhase1_interface_button_uno_image;
		imagenes [1] = this.menuStepsPhase1_interface_button_dos_image;;
		imagenes [2] = this.menuStepsPhase1_interface_button_tres_image;;
		imagenes [3] = this.menuStepsPhase1_interface_button_cuatro_image;;
		imagenes [4] = this.menuStepsPhase1_interface_button_cinco_image;;
		imagenes [5] = this.menuStepsPhase1_interface_button_seis_image;;
		
		string[] nombres_pasos = new string[6];
		nombres_pasos [0] = this.menuStepsPhase1_button_uno_text_eval;
		nombres_pasos [1] = this.menuStepsPhase1_button_dos_text_eval;
		nombres_pasos [2] = this.menuStepsPhase1_button_tres_text_eval;
		nombres_pasos [3] = this.menuStepsPhase1_button_cuatro_text_eval;
		nombres_pasos [4] = this.menuStepsPhase1_button_cinco_text_eval;
		nombres_pasos [5] = this.menuStepsPhase1_button_seis_text_eval;
		
		//vector que contendra las acciones disponibles, es decir las funciones que se pueden llamar:
		System.Action[] acciones_disponibles;
		acciones_disponibles = new System.Action[6];
		
		acciones_disponibles [0] += GoToActivitiesPhase1Step1EvalMode;
		acciones_disponibles [1] += GoToActivitiesPhase1Step2EvalMode;
		acciones_disponibles [2] += GoToActivitiesPhase1Step1EvalMode;
		acciones_disponibles [3] += GoToActivitiesPhase1Step1EvalMode;
		acciones_disponibles [4] += GoToActivitiesPhase1Step1EvalMode;
		acciones_disponibles [5] += GoToActivitiesPhase1Step1EvalMode;
	} //cierra OrganizeStepsPhase1

	/// <summary>
	/// Notifica al AppManager que se han organizado (bien o mal) los pasos de la fase1
	/// Si organized es true entonces se guarda el estado en el dispositivo
	/// </summary>
	/// <param name="organized">If set to <c>true</c> organized.</param>
	public void NotifyStepsOrganizedPhase1(bool organized){
		Debug.Log ("Llamado al metodo de notificacion de pasos no organizados FASE1 con organized = " + organized);
		this.eval_mode_phase1_steps_organized = organized;
		if (organized)
			SaveDataForStudent ();
	}

	/// <summary>
	/// Notifica al AppManager que se han organizado (bien o mal) los pasos de la fase 2
	/// Si organized es true entonces se guarda el estado en el dispositivo
	/// </summary>
	/// <param name="organized">If set to <c>true</c> organized.</param>
	public void NotifyStepsOrganizedPhase2(bool organized){
		Debug.Log ("Llamado al metodo de notificacion de pasos no organizados FASE2 con organized = " + organized);
		this.eval_mode_phase2_steps_organized = organized;
		if (organized)
			SaveDataForStudent ();
	}


	/// <summary>
	/// Goes to activities phase1 step1. Metodo que instancia la interfaz del Phase1 - Step 1 (buscar capo del carro) en modo evaluacion
	/// </summary>
	public void GoToActivitiesPhase1Step1EvalMode(){
		Debug.Log ("Llamado al metodo GoToActivitiesPhase1Step1EvalMode");
		if (current_interface == CurrentInterface.MENU_STEPS_PHASE1_EV)
			Destroy (menuStepsPhase1_int_eval_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_CAR_HOOD_EVAL)
			Destroy (SearchCapoCarroEvaluationMode_instance);
		
		current_interface = CurrentInterface.ACTIVITIES_PHASE1_STEP1_EV;
		//Ojo con lo siguiente estoy destruyendo todas las instancias que hayan de la interfaz de actividades por cada paso
		//DestroyInstancesWithTag ("ActivitiesForEachStep");
		
		if (BuscarCapoCarro == null)
			Debug.LogError ("GoToActivitiesPhase1Step1EvalMode: BuscarCapoCarro no esta definido en el AppManager!!");
		
		//incrementando el numero del proceso a 1 poque comenzamos el paso 1:
		processOrder = 1;

		DestroyInstancesWithTag ("BuscarCapoCarroInterface");

		Debug.Log ("SE VAN A IMPRIMIR LAS INSTANCIAS DISPONIBLES:");
		CanvasBuscarCapoCocheManager[] arreglo = FindObjectsOfType(typeof(CanvasBuscarCapoCocheManager)) as CanvasBuscarCapoCocheManager[];
		foreach(CanvasBuscarCapoCocheManager canv in arreglo){
			Debug.Log("--> INSTANCIA " + canv.name);
		}
		
		ActivitiesForPhase1Step1_int_eval_instance = Instantiate (BuscarCapoCarro);
		CanvasBuscarCapoCocheManager cBuscarCapoCoche = ActivitiesForPhase1Step1_int_eval_instance.GetComponent<CanvasBuscarCapoCocheManager> ();
		cBuscarCapoCoche.image_header_buscar_capo = image_buscar_capo_path;
		cBuscarCapoCoche.image_content_capo_carro_marker = question_mark_path;
		cBuscarCapoCoche.titulo_buscar_capo_carro = title_phase1_step1;
		cBuscarCapoCoche.introduction_text_path_1 = intro_text_phase1Step1_eval_path_one;
		cBuscarCapoCoche.introduction_text_path_2 = intro_text_phase1Step1_eval_path_two;
		cBuscarCapoCoche.text_btn_continuar = "Buscar";
		cBuscarCapoCoche.goBackToMenuActivities += GoToMenuStepsPhase1Eval;
		cBuscarCapoCoche.goToSearchCapoCarro += GoToSearchCapoCocheEvalMode;

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: IE4");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "IE4", "0", "-1","eval");
	} //cierra GoToActivitiesPhase1Step1EvalMode


	public void GoToActivitiesPhase1Step2EvalMode(){
		if (current_interface == CurrentInterface.MENU_STEPS_PHASE1_EV)
			Destroy (menuStepsPhase1_int_eval_instance);
		else if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_EVAL)
			Destroy (ToolsAndProductsPhase1Step2_int_eval_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP2_EV)
			Destroy (ARSearch_eval_mode_instance);


		current_interface = CurrentInterface.ACTIVITIES_PHASE1_STEP2_EV;
		
		//Ojo con lo siguiente estoy destruyendo todas las instancias que hayan de la interfaz de actividades por cada paso
		DestroyInstancesWithTag ("ActivitiesForEachStep");
		DestroyInstancesWithTag ("BuscarCapoCarroInterface");
		DestroyInstancesWithTag ("MenuPhasesEvalMode");
		DestroyInstancesWithTag ("LoginInterface");
						
		ActivitiesForPhase1Step2_int_eval_instance = Instantiate (ActivitiesForEachStepEvalMode);
		ActivitiesForEachStepEvalMode cActivitiesForStepP1S2 = ActivitiesForPhase1Step2_int_eval_instance.GetComponent<ActivitiesForEachStepEvalMode> ();
		cActivitiesForStepP1S2.titulo_current_step = title_phase1_step2_eval_mode;
		cActivitiesForStepP1S2.introduction_text_path = intro_text_phase1Step2_path_eval;
		
		//agregando imagenes que se cargan en el header para el conjunto de fases del proceso:
		cActivitiesForStepP1S2.image_phase_one_header = phase1_with_text_image_path;
		cActivitiesForStepP1S2.image_phase_two_header = phase2_with_text_image_gray_path;
		cActivitiesForStepP1S2.image_phase_three_header = phase3_with_text_image_gray_path;
		cActivitiesForStepP1S2.image_phase_four_header = phase4_with_text_image_gray_path;
		cActivitiesForStepP1S2.image_phase_five_header = phase5_with_text_image_gray_path;
		cActivitiesForStepP1S2.image_phase_six_header = phase6_with_text_image_gray_path;
		
		//agregando imagenes que se cargan en el header para el conjunto de pasos de la fase correspondiente:
		cActivitiesForStepP1S2.image_step_one_header = image_phase1_step1_text_gray;
		cActivitiesForStepP1S2.image_step_two_header = image_header_phase1Step2; //esta imagen se muestra activa NO en gris porque es el paso correspondiente
		cActivitiesForStepP1S2.image_step_three_header = image_phase1_step3_text_gray;
		cActivitiesForStepP1S2.image_step_four_header = image_phase1_step4_text_gray;
		cActivitiesForStepP1S2.image_step_five_header = image_phase1_step5_text_gray;
		cActivitiesForStepP1S2.image_step_six_header = image_phase1_step6_text_gray;

		//definiendo cuales botones deben estar activos:
		cActivitiesForStepP1S2.btn_one_enable = true;
		cActivitiesForStepP1S2.btn_two_enable = true;
		cActivitiesForStepP1S2.btn_three_enable = false;
		cActivitiesForStepP1S2.btn_four_enable = false;
		cActivitiesForStepP1S2.btn_five_enable = false;
		cActivitiesForStepP1S2.btn_six_enable = false;
		cActivitiesForStepP1S2.btn_seven_enable = false;
		
		//asignando imagenes de los botones del contenido que redireccionan a cada actividad disponible:
		cActivitiesForStepP1S2.image_uno_tools_and_products = image_uno_tools_and_products;
		cActivitiesForStepP1S2.image_dos_videos = image_tres_self_assessment; //Aunque la variable se llama videos voy a asignarle la img del self assessment
		cActivitiesForStepP1S2.goToToolsAndProd += GoToToolsAndProductsEvalMode;
		cActivitiesForStepP1S2.interfaceCallingGoToTools = "Phase1Step2Eval";
		cActivitiesForStepP1S2.goToMenuSteps += GoBackToMenuOfStepsFromActivitiesEvalMode;
		cActivitiesForStepP1S2.interfaceGoingBackFrom = "Phase1Step2Eval";
		cActivitiesForStepP1S2.goToVideosForStep += GoToSelfAssessmentEvalMode; //Aunque la variable se llama videos voy a asignarle el llamado al modulo de self-assessment

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: IE5");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "IE5", "1", "-1","eval");

	} //cierra GoToActivitiesPhase1Step2EvalMode

	public void GoToActivitiesPhase1Step3EvalMode(){
		if (current_interface == CurrentInterface.MENU_STEPS_PHASE1_EV)
			Destroy (menuStepsPhase1_int_eval_instance);
		else if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_EVAL)
			Destroy (ToolsAndProductsPhase1Step3_int_eval_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP3_EV)
			Destroy (ARSearch_eval_mode_instance);
		
		
		current_interface = CurrentInterface.ACTIVITIES_PHASE1_STEP3_EV;
		
		//Ojo con lo siguiente estoy destruyendo todas las instancias que hayan de la interfaz de actividades por cada paso
		DestroyInstancesWithTag ("ActivitiesForEachStep");
		DestroyInstancesWithTag ("BuscarCapoCarroInterface");
		DestroyInstancesWithTag ("MenuPhasesEvalMode");
		DestroyInstancesWithTag ("LoginInterface");
		
		ActivitiesForPhase1Step3_int_eval_instance = Instantiate (ActivitiesForEachStepEvalMode);
		ActivitiesForEachStepEvalMode cActivitiesForStepP1S3 = ActivitiesForPhase1Step3_int_eval_instance.GetComponent<ActivitiesForEachStepEvalMode> ();
		cActivitiesForStepP1S3.titulo_current_step = title_phase1_step3_eval_mode;
		cActivitiesForStepP1S3.introduction_text_path = intro_text_phase1Step3_path_eval;
		
		//agregando imagenes que se cargan en el header para el conjunto de fases del proceso:
		cActivitiesForStepP1S3.image_phase_one_header = phase1_with_text_image_path;
		cActivitiesForStepP1S3.image_phase_two_header = phase2_with_text_image_gray_path;
		cActivitiesForStepP1S3.image_phase_three_header = phase3_with_text_image_gray_path;
		cActivitiesForStepP1S3.image_phase_four_header = phase4_with_text_image_gray_path;
		cActivitiesForStepP1S3.image_phase_five_header = phase5_with_text_image_gray_path;
		cActivitiesForStepP1S3.image_phase_six_header = phase6_with_text_image_gray_path;
		
		//agregando imagenes que se cargan en el header para el conjunto de pasos de la fase correspondiente:
		cActivitiesForStepP1S3.image_step_one_header = image_phase1_step1_text_gray;
		cActivitiesForStepP1S3.image_step_two_header =  image_phase1_step2_text_gray;//esta imagen se muestra activa NO en gris porque es el paso correspondiente
		cActivitiesForStepP1S3.image_step_three_header = image_header_phase1Step3;
		cActivitiesForStepP1S3.image_step_four_header = image_phase1_step4_text_gray;
		cActivitiesForStepP1S3.image_step_five_header = image_phase1_step5_text_gray;
		cActivitiesForStepP1S3.image_step_six_header = image_phase1_step6_text_gray;
		
		//definiendo cuales botones deben estar activos:
		cActivitiesForStepP1S3.btn_one_enable = true;
		cActivitiesForStepP1S3.btn_two_enable = true;
		cActivitiesForStepP1S3.btn_three_enable = false;
		cActivitiesForStepP1S3.btn_four_enable = false;
		cActivitiesForStepP1S3.btn_five_enable = false;
		cActivitiesForStepP1S3.btn_six_enable = false;
		cActivitiesForStepP1S3.btn_seven_enable = false;
		
		//asignando imagenes de los botones del contenido que redireccionan a cada actividad disponible:
		cActivitiesForStepP1S3.image_uno_tools_and_products = image_uno_tools_and_products;
		cActivitiesForStepP1S3.image_dos_videos = image_tres_self_assessment; //Aunque la variable se llama videos voy a asignarle la img del self assessment
		cActivitiesForStepP1S3.goToToolsAndProd += GoToToolsAndProductsEvalMode;
		cActivitiesForStepP1S3.interfaceCallingGoToTools = "Phase1Step3Eval";
		cActivitiesForStepP1S3.goToMenuSteps += GoBackToMenuOfStepsFromActivitiesEvalMode;
		cActivitiesForStepP1S3.interfaceGoingBackFrom = "Phase1Step3Eval";
		cActivitiesForStepP1S3.goToVideosForStep += GoToSelfAssessmentEvalMode; //Aunque la variable se llama videos voy a asignarle el llamado al modulo de self-assessment

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: IE6");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "IE6", "2", "-1","eval");
		
	} //cierra GoToActivitiesPhase1Step3EvalMode


	public void GoToActivitiesPhase1Step4EvalMode(){
		if (current_interface == CurrentInterface.MENU_STEPS_PHASE1_EV)
			Destroy (menuStepsPhase1_int_eval_instance);
		else if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_EVAL)
			Destroy (ToolsAndProductsPhase1Step4_int_eval_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP4_EV)
			Destroy (ARSearch_eval_mode_instance);
		
		
		current_interface = CurrentInterface.ACTIVITIES_PHASE1_STEP4_EV;
		
		//Ojo con lo siguiente estoy destruyendo todas las instancias que hayan de la interfaz de actividades por cada paso
		DestroyInstancesWithTag ("ActivitiesForEachStep");
		DestroyInstancesWithTag ("BuscarCapoCarroInterface");
		DestroyInstancesWithTag ("MenuPhasesEvalMode");
		DestroyInstancesWithTag ("LoginInterface");
		
		ActivitiesForPhase1Step4_int_eval_instance = Instantiate (ActivitiesForEachStepEvalMode);
		ActivitiesForEachStepEvalMode cActivitiesForStepP1S4 = ActivitiesForPhase1Step4_int_eval_instance.GetComponent<ActivitiesForEachStepEvalMode> ();
		cActivitiesForStepP1S4.titulo_current_step = title_phase1_step4_eval_mode;
		cActivitiesForStepP1S4.introduction_text_path = intro_text_phase1Step4_path_eval;
		
		//agregando imagenes que se cargan en el header para el conjunto de fases del proceso:
		cActivitiesForStepP1S4.image_phase_one_header = phase1_with_text_image_path;
		cActivitiesForStepP1S4.image_phase_two_header = phase2_with_text_image_gray_path;
		cActivitiesForStepP1S4.image_phase_three_header = phase3_with_text_image_gray_path;
		cActivitiesForStepP1S4.image_phase_four_header = phase4_with_text_image_gray_path;
		cActivitiesForStepP1S4.image_phase_five_header = phase5_with_text_image_gray_path;
		cActivitiesForStepP1S4.image_phase_six_header = phase6_with_text_image_gray_path;
		
		//agregando imagenes que se cargan en el header para el conjunto de pasos de la fase correspondiente:
		cActivitiesForStepP1S4.image_step_one_header = image_phase1_step1_text_gray;
		cActivitiesForStepP1S4.image_step_two_header =  image_phase1_step2_text_gray;//esta imagen se muestra activa NO en gris porque es el paso correspondiente
		cActivitiesForStepP1S4.image_step_three_header = image_phase1_step3_text_gray;
		cActivitiesForStepP1S4.image_step_four_header = image_header_phase1Step4;
		cActivitiesForStepP1S4.image_step_five_header = image_phase1_step5_text_gray;
		cActivitiesForStepP1S4.image_step_six_header = image_phase1_step6_text_gray;
		
		//definiendo cuales botones deben estar activos:
		cActivitiesForStepP1S4.btn_one_enable = true;
		cActivitiesForStepP1S4.btn_two_enable = true;
		cActivitiesForStepP1S4.btn_three_enable = false;
		cActivitiesForStepP1S4.btn_four_enable = false;
		cActivitiesForStepP1S4.btn_five_enable = false;
		cActivitiesForStepP1S4.btn_six_enable = false;
		cActivitiesForStepP1S4.btn_seven_enable = false;
		
		//asignando imagenes de los botones del contenido que redireccionan a cada actividad disponible:
		cActivitiesForStepP1S4.image_uno_tools_and_products = image_uno_tools_and_products;
		cActivitiesForStepP1S4.image_dos_videos = image_tres_self_assessment; //Aunque la variable se llama videos voy a asignarle la img del self assessment
		cActivitiesForStepP1S4.goToToolsAndProd += GoToToolsAndProductsEvalMode;
		cActivitiesForStepP1S4.interfaceCallingGoToTools = "Phase1Step4Eval";
		cActivitiesForStepP1S4.goToMenuSteps += GoBackToMenuOfStepsFromActivitiesEvalMode;
		cActivitiesForStepP1S4.interfaceGoingBackFrom = "Phase1Step4Eval";
		cActivitiesForStepP1S4.goToVideosForStep += GoToSelfAssessmentEvalMode; //Aunque la variable se llama videos voy a asignarle el llamado al modulo de self-assessment

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: IE7");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "IE7", "3", "-1","eval");
		
	} //cierra GoToActivitiesPhase1Step4EvalMode


	public void GoToActivitiesPhase1Step5EvalMode(){
		if (current_interface == CurrentInterface.MENU_STEPS_PHASE1_EV)
			Destroy (menuStepsPhase1_int_eval_instance);
		else if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_EVAL)
			Destroy (ToolsAndProductsPhase1Step5_int_eval_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP5_EV)
			Destroy (ARSearch_eval_mode_instance);
		
		
		current_interface = CurrentInterface.ACTIVITIES_PHASE1_STEP5_EV;
		
		//Ojo con lo siguiente estoy destruyendo todas las instancias que hayan de la interfaz de actividades por cada paso
		DestroyInstancesWithTag ("ActivitiesForEachStep");
		DestroyInstancesWithTag ("BuscarCapoCarroInterface");
		DestroyInstancesWithTag ("MenuPhasesEvalMode");
		DestroyInstancesWithTag ("LoginInterface");
		
		ActivitiesForPhase1Step5_int_eval_instance = Instantiate (ActivitiesForEachStepEvalMode);
		ActivitiesForEachStepEvalMode cActivitiesForStepP1S5 = ActivitiesForPhase1Step5_int_eval_instance.GetComponent<ActivitiesForEachStepEvalMode> ();
		cActivitiesForStepP1S5.titulo_current_step = title_phase1_step5_eval_mode;
		cActivitiesForStepP1S5.introduction_text_path = intro_text_phase1Step5_path_eval;
		
		//agregando imagenes que se cargan en el header para el conjunto de fases del proceso:
		cActivitiesForStepP1S5.image_phase_one_header = phase1_with_text_image_path;
		cActivitiesForStepP1S5.image_phase_two_header = phase2_with_text_image_gray_path;
		cActivitiesForStepP1S5.image_phase_three_header = phase3_with_text_image_gray_path;
		cActivitiesForStepP1S5.image_phase_four_header = phase4_with_text_image_gray_path;
		cActivitiesForStepP1S5.image_phase_five_header = phase5_with_text_image_gray_path;
		cActivitiesForStepP1S5.image_phase_six_header = phase6_with_text_image_gray_path;
		
		//agregando imagenes que se cargan en el header para el conjunto de pasos de la fase correspondiente:
		cActivitiesForStepP1S5.image_step_one_header = image_phase1_step1_text_gray;
		cActivitiesForStepP1S5.image_step_two_header =  image_phase1_step2_text_gray;//esta imagen se muestra activa NO en gris porque es el paso correspondiente
		cActivitiesForStepP1S5.image_step_three_header = image_phase1_step3_text_gray;
		cActivitiesForStepP1S5.image_step_four_header = image_phase1_step4_text_gray;
		cActivitiesForStepP1S5.image_step_five_header = image_header_phase1Step5;
		cActivitiesForStepP1S5.image_step_six_header = image_phase1_step6_text_gray;
		
		//definiendo cuales botones deben estar activos:
		cActivitiesForStepP1S5.btn_one_enable = true;
		cActivitiesForStepP1S5.btn_two_enable = true;
		cActivitiesForStepP1S5.btn_three_enable = false;
		cActivitiesForStepP1S5.btn_four_enable = false;
		cActivitiesForStepP1S5.btn_five_enable = false;
		cActivitiesForStepP1S5.btn_six_enable = false;
		cActivitiesForStepP1S5.btn_seven_enable = false;
		
		//asignando imagenes de los botones del contenido que redireccionan a cada actividad disponible:
		cActivitiesForStepP1S5.image_uno_tools_and_products = image_uno_tools_and_products;
		cActivitiesForStepP1S5.image_dos_videos = image_tres_self_assessment; //Aunque la variable se llama videos voy a asignarle la img del self assessment
		cActivitiesForStepP1S5.goToToolsAndProd += GoToToolsAndProductsEvalMode;
		cActivitiesForStepP1S5.interfaceCallingGoToTools = "Phase1Step5Eval";
		cActivitiesForStepP1S5.goToMenuSteps += GoBackToMenuOfStepsFromActivitiesEvalMode;
		cActivitiesForStepP1S5.interfaceGoingBackFrom = "Phase1Step5Eval";
		cActivitiesForStepP1S5.goToVideosForStep += GoToSelfAssessmentEvalMode; //Aunque la variable se llama videos voy a asignarle el llamado al modulo de self-assessment

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: IE8");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "IE8", "4", "-1","eval");
		
	} //cierra GoToActivitiesPhase1Step5EvalMode

	public void GoToActivitiesPhase1Step6EvalMode(){
		if (current_interface == CurrentInterface.MENU_STEPS_PHASE1_EV)
			Destroy (menuStepsPhase1_int_eval_instance);
		else if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_EVAL)
			Destroy (ToolsAndProductsPhase1Step6_int_eval_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP6_EV)
			Destroy (ARSearch_eval_mode_instance);
		
		
		current_interface = CurrentInterface.ACTIVITIES_PHASE1_STEP6_EV;
		
		//Ojo con lo siguiente estoy destruyendo todas las instancias que hayan de la interfaz de actividades por cada paso
		DestroyInstancesWithTag ("ActivitiesForEachStep");
		DestroyInstancesWithTag ("BuscarCapoCarroInterface");
		DestroyInstancesWithTag ("MenuPhasesEvalMode");
		DestroyInstancesWithTag ("LoginInterface");
		
		ActivitiesForPhase1Step6_int_eval_instance = Instantiate (ActivitiesForEachStepEvalMode);
		ActivitiesForEachStepEvalMode cActivitiesForStepP1S6 = ActivitiesForPhase1Step6_int_eval_instance.GetComponent<ActivitiesForEachStepEvalMode> ();
		cActivitiesForStepP1S6.titulo_current_step = title_phase1_step6_eval_mode;
		cActivitiesForStepP1S6.introduction_text_path = intro_text_phase1Step6_path_eval;
		
		//agregando imagenes que se cargan en el header para el conjunto de fases del proceso:
		cActivitiesForStepP1S6.image_phase_one_header = phase1_with_text_image_path;
		cActivitiesForStepP1S6.image_phase_two_header = phase2_with_text_image_gray_path;
		cActivitiesForStepP1S6.image_phase_three_header = phase3_with_text_image_gray_path;
		cActivitiesForStepP1S6.image_phase_four_header = phase4_with_text_image_gray_path;
		cActivitiesForStepP1S6.image_phase_five_header = phase5_with_text_image_gray_path;
		cActivitiesForStepP1S6.image_phase_six_header = phase6_with_text_image_gray_path;
		
		//agregando imagenes que se cargan en el header para el conjunto de pasos de la fase correspondiente:
		cActivitiesForStepP1S6.image_step_one_header = image_phase1_step1_text_gray;
		cActivitiesForStepP1S6.image_step_two_header =  image_phase1_step2_text_gray;//esta imagen se muestra activa NO en gris porque es el paso correspondiente
		cActivitiesForStepP1S6.image_step_three_header = image_phase1_step3_text_gray;
		cActivitiesForStepP1S6.image_step_four_header = image_phase1_step4_text_gray;
		cActivitiesForStepP1S6.image_step_five_header = image_phase1_step5_text_gray;
		cActivitiesForStepP1S6.image_step_six_header = image_header_phase1Step6;
		
		//definiendo cuales botones deben estar activos:
		cActivitiesForStepP1S6.btn_one_enable = true;
		cActivitiesForStepP1S6.btn_two_enable = true;
		cActivitiesForStepP1S6.btn_three_enable = false;
		cActivitiesForStepP1S6.btn_four_enable = false;
		cActivitiesForStepP1S6.btn_five_enable = false;
		cActivitiesForStepP1S6.btn_six_enable = false;
		cActivitiesForStepP1S6.btn_seven_enable = false;
		
		//asignando imagenes de los botones del contenido que redireccionan a cada actividad disponible:
		cActivitiesForStepP1S6.image_uno_tools_and_products = image_uno_tools_and_products;
		cActivitiesForStepP1S6.image_dos_videos = image_tres_self_assessment; //Aunque la variable se llama videos voy a asignarle la img del self assessment
		cActivitiesForStepP1S6.goToToolsAndProd += GoToToolsAndProductsEvalMode;
		cActivitiesForStepP1S6.interfaceCallingGoToTools = "Phase1Step6Eval";
		cActivitiesForStepP1S6.goToMenuSteps += GoBackToMenuOfStepsFromActivitiesEvalMode;
		cActivitiesForStepP1S6.interfaceGoingBackFrom = "Phase1Step6Eval";
		cActivitiesForStepP1S6.goToVideosForStep += GoToSelfAssessmentEvalMode; //Aunque la variable se llama videos voy a asignarle el llamado al modulo de self-assessment

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: IE9");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "IE9", "5", "-1","eval");
		
	} //cierra GoToActivitiesPhase1Step2EvalMode

	//********************************************************************************************************
	//********************************************************************************************************
	//METODOS DE ACTIVIDADES PARA LA FASE 2 - MODO EVALUACION

	public void GoToActivitiesPhase2Step1EvalMode(){
		Debug.Log ("Llamado al metodo GoToActivitiesPhase2Step1EvalMode");

		if (current_interface == CurrentInterface.MENU_STEPS_PHASE2_EV)
			Destroy (menuStepsPhase2_int_eval_instance);

		
		current_interface = CurrentInterface.ACTIVITIES_PHASE2_STEP1_EV;
		//Ojo con lo siguiente estoy destruyendo todas las instancias que hayan de la interfaz de actividades por cada paso
		//DestroyInstancesWithTag ("ActivitiesForEachStep");
		
		if (BuscarCapoCarro == null)
			Debug.LogError ("GoToActivitiesPhase2Step1EvalMode: BuscarCapoCarro no esta definido en el AppManager!!");
		
		//incrementando el numero del proceso a 1 poque comenzamos el paso 1:
		processOrder = 1;
		
		Debug.Log ("SE VAN A IMPRIMIR LAS INSTANCIAS DISPONIBLES:");
		CanvasBuscarCapoCocheManager[] arreglo = FindObjectsOfType(typeof(CanvasBuscarCapoCocheManager)) as CanvasBuscarCapoCocheManager[];
		foreach(CanvasBuscarCapoCocheManager canv in arreglo){
			Debug.LogError("--> INSTANCIA ACTIVA" + canv.name);
		}

		DestroyInstancesWithTag ("BuscarCapoCarroInterface");
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhaseTwoEval");
		
		ActivitiesForPhase2Step1_int_eval_instance = Instantiate (BuscarCapoCarro);
		CanvasBuscarCapoCocheManager cBuscarCapoCoche = ActivitiesForPhase2Step1_int_eval_instance.GetComponent<CanvasBuscarCapoCocheManager> ();
		cBuscarCapoCoche.image_header_buscar_capo = image_intro_matizado_header_path;
		cBuscarCapoCoche.image_content_capo_carro_marker = image_intro_matizado_header_path;
		cBuscarCapoCoche.titulo_buscar_capo_carro = title_phase2_step1;
		cBuscarCapoCoche.introduction_text_path_1 = intro_text_phase2Step1_eval_path_one;
		cBuscarCapoCoche.introduction_text_path_2 = intro_text_phase2Step1_eval_path_two;
		cBuscarCapoCoche.text_btn_continuar = "Continuar";
		cBuscarCapoCoche.goBackToMenuActivities += GoToMenuStepsPhase2Eval;
		cBuscarCapoCoche.goToSearchCapoCarro += GoToMenuStepsPhase2Eval;
		//ahora se notifica que el paso 1 se ha completado para que se pueda activar el segundo paso:
		steps_p_two_eval_completed [0].activity_tools_and_products = true;
		steps_p_two_eval_completed [0].selfevaluation = true;
		steps_p_two_eval_completed [0].CheckStepCompletion ();

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: IE25");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "IE25", "0", "-1","eval");
	} //cierra GoToActivitiesPhase2Step1EvalMode


	public void GoToActivitiesPhase2Step2EvalMode(){
		if (current_interface == CurrentInterface.MENU_STEPS_PHASE2_EV)
			Destroy (menuStepsPhase2_int_eval_instance);
		else if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_EVAL)
			Destroy (ToolsAndProductsPhase2Step2_int_eval_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP2_EV)
			Destroy (ARSearch_eval_mode_instance);
		
		
		current_interface = CurrentInterface.ACTIVITIES_PHASE2_STEP2_EV;
		
		//Ojo con lo siguiente estoy destruyendo todas las instancias que hayan de la interfaz de actividades por cada paso
		DestroyInstancesWithTag ("ActivitiesForEachStep");
		DestroyInstancesWithTag ("BuscarCapoCarroInterface");
		DestroyInstancesWithTag ("MenuPhasesEvalMode");
		DestroyInstancesWithTag ("LoginInterface");
		
		ActivitiesForPhase2Step2_int_eval_instance = Instantiate (ActivitiesForEachStepEvalMode);
		ActivitiesForEachStepEvalMode cActivitiesForStepP2S2 = ActivitiesForPhase2Step2_int_eval_instance.GetComponent<ActivitiesForEachStepEvalMode> ();
		cActivitiesForStepP2S2.titulo_current_step = title_phase2_step2_eval_mode;
		cActivitiesForStepP2S2.introduction_text_path = intro_text_phase2Step2_path_eval;
		
		//agregando imagenes que se cargan en el header para el conjunto de fases del proceso:
		cActivitiesForStepP2S2.image_phase_one_header = phase1_with_text_image_gray_path;
		cActivitiesForStepP2S2.image_phase_two_header = phase2_with_text_image_path;
		cActivitiesForStepP2S2.image_phase_three_header = phase3_with_text_image_gray_path;
		cActivitiesForStepP2S2.image_phase_four_header = phase4_with_text_image_gray_path;
		cActivitiesForStepP2S2.image_phase_five_header = phase5_with_text_image_gray_path;
		cActivitiesForStepP2S2.image_phase_six_header = phase6_with_text_image_gray_path;
		
		//agregando imagenes que se cargan en el header para el conjunto de pasos de la fase correspondiente:
		cActivitiesForStepP2S2.image_step_one_header = image_phase2step1_with_text_gray;
		cActivitiesForStepP2S2.image_step_two_header =  image_phase2step2_with_text;//esta imagen se muestra activa NO en gris porque es el paso correspondiente
		cActivitiesForStepP2S2.image_step_three_header = image_phase2step3_with_text_gray;
		cActivitiesForStepP2S2.image_step_four_header = image_phase2step4_with_text_gray;
		cActivitiesForStepP2S2.image_step_five_header = "";
		cActivitiesForStepP2S2.image_step_six_header = "";
		
		//definiendo cuales botones deben estar activos:
		cActivitiesForStepP2S2.btn_one_enable = true;
		cActivitiesForStepP2S2.btn_two_enable = true;
		cActivitiesForStepP2S2.btn_three_enable = false;
		cActivitiesForStepP2S2.btn_four_enable = false;
		cActivitiesForStepP2S2.btn_five_enable = false;
		cActivitiesForStepP2S2.btn_six_enable = false;
		cActivitiesForStepP2S2.btn_seven_enable = false;
		
		//asignando imagenes de los botones del contenido que redireccionan a cada actividad disponible:
		cActivitiesForStepP2S2.image_uno_tools_and_products = image_uno_tools_and_products;
		cActivitiesForStepP2S2.image_dos_videos = image_tres_self_assessment; //Aunque la variable se llama videos voy a asignarle la img del self assessment
		cActivitiesForStepP2S2.goToToolsAndProd += GoToToolsAndProductsEvalMode;
		cActivitiesForStepP2S2.interfaceCallingGoToTools = "Phase2Step2Eval";
		cActivitiesForStepP2S2.goToMenuSteps += GoBackToMenuOfStepsFromActivitiesEvalMode;
		cActivitiesForStepP2S2.interfaceGoingBackFrom = "Phase2Step2Eval";
		cActivitiesForStepP2S2.goToVideosForStep += GoToSelfAssessmentEvalMode; //Aunque la variable se llama videos voy a asignarle el llamado al modulo de self-assessment

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: IE26");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "IE26", "7", "-1","eval");
	} // cierra GoToActivitiesPhase2Step2EvalMode


	public void GoToActivitiesPhase2Step3EvalMode(){
		if (current_interface == CurrentInterface.MENU_STEPS_PHASE2_EV)
			Destroy (menuStepsPhase2_int_eval_instance);
		else if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_EVAL)
			Destroy (ToolsAndProductsPhase2Step3_int_eval_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP3_EV)
			Destroy (ARSearch_eval_mode_instance);
		
		
		current_interface = CurrentInterface.ACTIVITIES_PHASE2_STEP3_EV;
		
		//Ojo con lo siguiente estoy destruyendo todas las instancias que hayan de la interfaz de actividades por cada paso
		DestroyInstancesWithTag ("ActivitiesForEachStep");
		DestroyInstancesWithTag ("BuscarCapoCarroInterface");
		DestroyInstancesWithTag ("MenuPhasesEvalMode");
		DestroyInstancesWithTag ("LoginInterface");
		
		ActivitiesForPhase2Step3_int_eval_instance = Instantiate (ActivitiesForEachStepEvalMode);
		ActivitiesForEachStepEvalMode cActivitiesForStepP2S3 = ActivitiesForPhase2Step3_int_eval_instance.GetComponent<ActivitiesForEachStepEvalMode> ();
		cActivitiesForStepP2S3.titulo_current_step = title_phase2_step3_eval_mode;
		cActivitiesForStepP2S3.introduction_text_path = intro_text_phase2Step3_path_eval;
		
		//agregando imagenes que se cargan en el header para el conjunto de fases del proceso:
		cActivitiesForStepP2S3.image_phase_one_header = phase1_with_text_image_gray_path;
		cActivitiesForStepP2S3.image_phase_two_header = phase2_with_text_image_path;
		cActivitiesForStepP2S3.image_phase_three_header = phase3_with_text_image_gray_path;
		cActivitiesForStepP2S3.image_phase_four_header = phase4_with_text_image_gray_path;
		cActivitiesForStepP2S3.image_phase_five_header = phase5_with_text_image_gray_path;
		cActivitiesForStepP2S3.image_phase_six_header = phase6_with_text_image_gray_path;
		
		//agregando imagenes que se cargan en el header para el conjunto de pasos de la fase correspondiente:
		cActivitiesForStepP2S3.image_step_one_header = image_phase2step1_with_text_gray;
		cActivitiesForStepP2S3.image_step_two_header =  image_phase2step2_with_text_gray;//esta imagen se muestra activa NO en gris porque es el paso correspondiente
		cActivitiesForStepP2S3.image_step_three_header = image_phase2step3_with_text;
		cActivitiesForStepP2S3.image_step_four_header = image_phase2step4_with_text_gray;
		cActivitiesForStepP2S3.image_step_five_header = "";
		cActivitiesForStepP2S3.image_step_six_header = "";
		
		//definiendo cuales botones deben estar activos:
		cActivitiesForStepP2S3.btn_one_enable = true;
		cActivitiesForStepP2S3.btn_two_enable = true;
		cActivitiesForStepP2S3.btn_three_enable = false;
		cActivitiesForStepP2S3.btn_four_enable = false;
		cActivitiesForStepP2S3.btn_five_enable = false;
		cActivitiesForStepP2S3.btn_six_enable = false;
		cActivitiesForStepP2S3.btn_seven_enable = false;
		
		//asignando imagenes de los botones del contenido que redireccionan a cada actividad disponible:
		cActivitiesForStepP2S3.image_uno_tools_and_products = image_uno_tools_and_products;
		cActivitiesForStepP2S3.image_dos_videos = image_tres_self_assessment; //Aunque la variable se llama videos voy a asignarle la img del self assessment
		cActivitiesForStepP2S3.goToToolsAndProd += GoToToolsAndProductsEvalMode;
		cActivitiesForStepP2S3.interfaceCallingGoToTools = "Phase2Step3Eval";
		cActivitiesForStepP2S3.goToMenuSteps += GoBackToMenuOfStepsFromActivitiesEvalMode;
		cActivitiesForStepP2S3.interfaceGoingBackFrom = "Phase2Step3Eval";
		cActivitiesForStepP2S3.goToVideosForStep += GoToSelfAssessmentEvalMode; //Aunque la variable se llama videos voy a asignarle el llamado al modulo de self-assessment
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: IE27");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "IE27", "9", "-1","eval");
	} //cierra metodo GoToActivitiesPhase2Step3EvalMode


	public void GoToActivitiesPhase2Step4EvalMode(){
		if (current_interface == CurrentInterface.MENU_STEPS_PHASE2_EV)
			Destroy (menuStepsPhase2_int_eval_instance);
		else if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_EVAL)
			Destroy (ToolsAndProductsPhase2Step4_int_eval_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP4_EV)
			Destroy (ARSearch_eval_mode_instance);
		
		
		current_interface = CurrentInterface.ACTIVITIES_PHASE2_STEP4_EV;
		
		//Ojo con lo siguiente estoy destruyendo todas las instancias que hayan de la interfaz de actividades por cada paso
		DestroyInstancesWithTag ("ActivitiesForEachStep");
		DestroyInstancesWithTag ("BuscarCapoCarroInterface");
		DestroyInstancesWithTag ("MenuPhasesEvalMode");
		DestroyInstancesWithTag ("LoginInterface");
		
		ActivitiesForPhase2Step4_int_eval_instance = Instantiate (ActivitiesForEachStepEvalMode);
		ActivitiesForEachStepEvalMode cActivitiesForStepP2S4 = ActivitiesForPhase2Step4_int_eval_instance.GetComponent<ActivitiesForEachStepEvalMode> ();
		cActivitiesForStepP2S4.titulo_current_step = title_phase2_step4_eval_mode;
		cActivitiesForStepP2S4.introduction_text_path = intro_text_phase2Step4_path_eval;
		
		//agregando imagenes que se cargan en el header para el conjunto de fases del proceso:
		cActivitiesForStepP2S4.image_phase_one_header = phase1_with_text_image_gray_path;
		cActivitiesForStepP2S4.image_phase_two_header = phase2_with_text_image_path;
		cActivitiesForStepP2S4.image_phase_three_header = phase3_with_text_image_gray_path;
		cActivitiesForStepP2S4.image_phase_four_header = phase4_with_text_image_gray_path;
		cActivitiesForStepP2S4.image_phase_five_header = phase5_with_text_image_gray_path;
		cActivitiesForStepP2S4.image_phase_six_header = phase6_with_text_image_gray_path;
		
		//agregando imagenes que se cargan en el header para el conjunto de pasos de la fase correspondiente:
		cActivitiesForStepP2S4.image_step_one_header = image_phase2step1_with_text_gray;
		cActivitiesForStepP2S4.image_step_two_header =  image_phase2step2_with_text_gray;//esta imagen se muestra activa NO en gris porque es el paso correspondiente
		cActivitiesForStepP2S4.image_step_three_header = image_phase2step3_with_text;
		cActivitiesForStepP2S4.image_step_four_header = image_phase2step4_with_text_gray;
		cActivitiesForStepP2S4.image_step_five_header = "";
		cActivitiesForStepP2S4.image_step_six_header = "";
		
		//definiendo cuales botones deben estar activos:
		cActivitiesForStepP2S4.btn_one_enable = true;
		cActivitiesForStepP2S4.btn_two_enable = true;
		cActivitiesForStepP2S4.btn_three_enable = false;
		cActivitiesForStepP2S4.btn_four_enable = false;
		cActivitiesForStepP2S4.btn_five_enable = false;
		cActivitiesForStepP2S4.btn_six_enable = false;
		cActivitiesForStepP2S4.btn_seven_enable = false;
		
		//asignando imagenes de los botones del contenido que redireccionan a cada actividad disponible:
		cActivitiesForStepP2S4.image_uno_tools_and_products = image_uno_tools_and_products;
		cActivitiesForStepP2S4.image_dos_videos = image_tres_self_assessment; //Aunque la variable se llama videos voy a asignarle la img del self assessment
		cActivitiesForStepP2S4.goToToolsAndProd += GoToToolsAndProductsEvalMode;
		cActivitiesForStepP2S4.interfaceCallingGoToTools = "Phase2Step4Eval";
		cActivitiesForStepP2S4.goToMenuSteps += GoBackToMenuOfStepsFromActivitiesEvalMode;
		cActivitiesForStepP2S4.interfaceGoingBackFrom = "Phase2Step4Eval";
		cActivitiesForStepP2S4.goToVideosForStep += GoToSelfAssessmentEvalMode; //Aunque la variable se llama videos voy a asignarle el llamado al modulo de self-assessment
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:IE28");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "IE28", "10", "-1","eval");
	}//cierra GoToActivitiesPhase2Step4EvalMode


	public void GoToActivitiesPhase2Step5EvalMode(){
		if (current_interface == CurrentInterface.MENU_STEPS_PHASE2_EV)
			Destroy (menuStepsPhase2_int_eval_instance);
		else if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_EVAL)
			Destroy (ToolsAndProductsPhase2Step5_int_eval_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP5_EV)
			Destroy (ARSearch_eval_mode_instance);
		
		
		current_interface = CurrentInterface.ACTIVITIES_PHASE2_STEP5_EV;
		
		//Ojo con lo siguiente estoy destruyendo todas las instancias que hayan de la interfaz de actividades por cada paso
		DestroyInstancesWithTag ("ActivitiesForEachStep");
		DestroyInstancesWithTag ("BuscarCapoCarroInterface");
		DestroyInstancesWithTag ("MenuPhasesEvalMode");
		DestroyInstancesWithTag ("LoginInterface");
		
		ActivitiesForPhase2Step5_int_eval_instance = Instantiate (ActivitiesForEachStepEvalMode);
		ActivitiesForEachStepEvalMode cActivitiesForStepP2S5 = ActivitiesForPhase2Step5_int_eval_instance.GetComponent<ActivitiesForEachStepEvalMode> ();
		cActivitiesForStepP2S5.titulo_current_step = title_phase2_step5_eval_mode;
		cActivitiesForStepP2S5.introduction_text_path = intro_text_phase2Step5_path_eval;
		
		//agregando imagenes que se cargan en el header para el conjunto de fases del proceso:
		cActivitiesForStepP2S5.image_phase_one_header = phase1_with_text_image_gray_path;
		cActivitiesForStepP2S5.image_phase_two_header = phase2_with_text_image_path;
		cActivitiesForStepP2S5.image_phase_three_header = phase3_with_text_image_gray_path;
		cActivitiesForStepP2S5.image_phase_four_header = phase4_with_text_image_gray_path;
		cActivitiesForStepP2S5.image_phase_five_header = phase5_with_text_image_gray_path;
		cActivitiesForStepP2S5.image_phase_six_header = phase6_with_text_image_gray_path;
		
		//agregando imagenes que se cargan en el header para el conjunto de pasos de la fase correspondiente:
		cActivitiesForStepP2S5.image_step_one_header = image_phase2step1_with_text_gray;
		cActivitiesForStepP2S5.image_step_two_header =  image_phase2step2_with_text_gray;
		cActivitiesForStepP2S5.image_step_three_header = image_phase2step3_with_text_gray;
		cActivitiesForStepP2S5.image_step_four_header = image_phase2step4_with_text; //esta imagen se muestra activa NO en gris porque es el paso correspondiente
		cActivitiesForStepP2S5.image_step_five_header = "";
		cActivitiesForStepP2S5.image_step_six_header = "";
		
		//definiendo cuales botones deben estar activos:
		cActivitiesForStepP2S5.btn_one_enable = true;
		cActivitiesForStepP2S5.btn_two_enable = true;
		cActivitiesForStepP2S5.btn_three_enable = false;
		cActivitiesForStepP2S5.btn_four_enable = false;
		cActivitiesForStepP2S5.btn_five_enable = false;
		cActivitiesForStepP2S5.btn_six_enable = false;
		cActivitiesForStepP2S5.btn_seven_enable = false;
		
		//asignando imagenes de los botones del contenido que redireccionan a cada actividad disponible:
		cActivitiesForStepP2S5.image_uno_tools_and_products = image_uno_tools_and_products;
		cActivitiesForStepP2S5.image_dos_videos = image_tres_self_assessment; //Aunque la variable se llama videos voy a asignarle la img del self assessment
		cActivitiesForStepP2S5.goToToolsAndProd += GoToToolsAndProductsEvalMode;
		cActivitiesForStepP2S5.interfaceCallingGoToTools = "Phase2Step5Eval";
		cActivitiesForStepP2S5.goToMenuSteps += GoBackToMenuOfStepsFromActivitiesEvalMode;
		cActivitiesForStepP2S5.interfaceGoingBackFrom = "Phase2Step5Eval";
		cActivitiesForStepP2S5.goToVideosForStep += GoToSelfAssessmentEvalMode; //Aunque la variable se llama videos voy a asignarle el llamado al modulo de self-assessment
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: IE29");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "IE29", "11", "-1","eval");
	}//cierra GoToActivitiesPhase2Step5EvalMode

	public void GoToActivitiesPhase2Step6EvalMode(){
		if (current_interface == CurrentInterface.MENU_STEPS_PHASE2_EV)
			Destroy (menuStepsPhase2_int_eval_instance);
		else if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_EVAL)
			Destroy (ToolsAndProductsPhase2Step6_int_eval_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP6_EV)
			Destroy (ARSearch_eval_mode_instance);
		
		
		current_interface = CurrentInterface.ACTIVITIES_PHASE2_STEP6_EV;
		
		//Ojo con lo siguiente estoy destruyendo todas las instancias que hayan de la interfaz de actividades por cada paso
		DestroyInstancesWithTag ("ActivitiesForEachStep");
		DestroyInstancesWithTag ("BuscarCapoCarroInterface");
		DestroyInstancesWithTag ("MenuPhasesEvalMode");
		DestroyInstancesWithTag ("LoginInterface");
		
		ActivitiesForPhase2Step6_int_eval_instance = Instantiate (ActivitiesForEachStepEvalMode);
		ActivitiesForEachStepEvalMode cActivitiesForStepP2S6 = ActivitiesForPhase2Step6_int_eval_instance.GetComponent<ActivitiesForEachStepEvalMode> ();
		cActivitiesForStepP2S6.titulo_current_step = title_phase2_step6_eval_mode;
		cActivitiesForStepP2S6.introduction_text_path = intro_text_phase2Step6_path_eval;
		
		//agregando imagenes que se cargan en el header para el conjunto de fases del proceso:
		cActivitiesForStepP2S6.image_phase_one_header = phase1_with_text_image_gray_path;
		cActivitiesForStepP2S6.image_phase_two_header = phase2_with_text_image_path;
		cActivitiesForStepP2S6.image_phase_three_header = phase3_with_text_image_gray_path;
		cActivitiesForStepP2S6.image_phase_four_header = phase4_with_text_image_gray_path;
		cActivitiesForStepP2S6.image_phase_five_header = phase5_with_text_image_gray_path;
		cActivitiesForStepP2S6.image_phase_six_header = phase6_with_text_image_gray_path;
		
		//agregando imagenes que se cargan en el header para el conjunto de pasos de la fase correspondiente:
		cActivitiesForStepP2S6.image_step_one_header = image_phase2step1_with_text_gray;
		cActivitiesForStepP2S6.image_step_two_header =  image_phase2step2_with_text_gray;
		cActivitiesForStepP2S6.image_step_three_header = image_phase2step3_with_text_gray;
		cActivitiesForStepP2S6.image_step_four_header = image_phase2step4_with_text; //esta imagen se muestra activa NO en gris porque es el paso correspondiente
		cActivitiesForStepP2S6.image_step_five_header = "";
		cActivitiesForStepP2S6.image_step_six_header = "";
		
		//definiendo cuales botones deben estar activos:
		cActivitiesForStepP2S6.btn_one_enable = true;
		cActivitiesForStepP2S6.btn_two_enable = true;
		cActivitiesForStepP2S6.btn_three_enable = false;
		cActivitiesForStepP2S6.btn_four_enable = false;
		cActivitiesForStepP2S6.btn_five_enable = false;
		cActivitiesForStepP2S6.btn_six_enable = false;
		cActivitiesForStepP2S6.btn_seven_enable = false;
		
		//asignando imagenes de los botones del contenido que redireccionan a cada actividad disponible:
		cActivitiesForStepP2S6.image_uno_tools_and_products = image_uno_tools_and_products;
		cActivitiesForStepP2S6.image_dos_videos = image_tres_self_assessment; //Aunque la variable se llama videos voy a asignarle la img del self assessment
		cActivitiesForStepP2S6.goToToolsAndProd += GoToToolsAndProductsEvalMode;
		cActivitiesForStepP2S6.interfaceCallingGoToTools = "Phase2Step6Eval";
		cActivitiesForStepP2S6.goToMenuSteps += GoBackToMenuOfStepsFromActivitiesEvalMode;
		cActivitiesForStepP2S6.interfaceGoingBackFrom = "Phase2Step6Eval";
		cActivitiesForStepP2S6.goToVideosForStep += GoToSelfAssessmentEvalMode; //Aunque la variable se llama videos voy a asignarle el llamado al modulo de self-assessment
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:IE30");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "IE30", "12", "-1","eval");
	}//cierra GoToActivitiesPhase2Step6EvalMode


	public void GoToActivitiesPhase2Step7EvalMode(){
		if (current_interface == CurrentInterface.MENU_STEPS_PHASE2_EV)
			Destroy (menuStepsPhase2_int_eval_instance);
		else if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_EVAL)
			Destroy (ToolsAndProductsPhase2Step7_int_eval_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP7_EV)
			Destroy (ARSearch_eval_mode_instance);
		
		
		current_interface = CurrentInterface.ACTIVITIES_PHASE2_STEP7_EV;
		
		//Ojo con lo siguiente estoy destruyendo todas las instancias que hayan de la interfaz de actividades por cada paso
		DestroyInstancesWithTag ("ActivitiesForEachStep");
		DestroyInstancesWithTag ("BuscarCapoCarroInterface");
		DestroyInstancesWithTag ("MenuPhasesEvalMode");
		DestroyInstancesWithTag ("LoginInterface");
		
		ActivitiesForPhase2Step7_int_eval_instance = Instantiate (ActivitiesForEachStepEvalMode);
		ActivitiesForEachStepEvalMode cActivitiesForStepP2S7 = ActivitiesForPhase2Step7_int_eval_instance.GetComponent<ActivitiesForEachStepEvalMode> ();
		cActivitiesForStepP2S7.titulo_current_step = title_phase2_step7_eval_mode;
		cActivitiesForStepP2S7.introduction_text_path = intro_text_phase2Step7_path_eval;
		
		//agregando imagenes que se cargan en el header para el conjunto de fases del proceso:
		cActivitiesForStepP2S7.image_phase_one_header = phase1_with_text_image_gray_path;
		cActivitiesForStepP2S7.image_phase_two_header = phase2_with_text_image_path;
		cActivitiesForStepP2S7.image_phase_three_header = phase3_with_text_image_gray_path;
		cActivitiesForStepP2S7.image_phase_four_header = phase4_with_text_image_gray_path;
		cActivitiesForStepP2S7.image_phase_five_header = phase5_with_text_image_gray_path;
		cActivitiesForStepP2S7.image_phase_six_header = phase6_with_text_image_gray_path;
		
		//agregando imagenes que se cargan en el header para el conjunto de pasos de la fase correspondiente:
		cActivitiesForStepP2S7.image_step_one_header = image_phase2step1_with_text_gray;
		cActivitiesForStepP2S7.image_step_two_header =  image_phase2step2_with_text_gray;
		cActivitiesForStepP2S7.image_step_three_header = image_phase2step3_with_text_gray;
		cActivitiesForStepP2S7.image_step_four_header = image_phase2step4_with_text; //esta imagen se muestra activa NO en gris porque es el paso correspondiente
		cActivitiesForStepP2S7.image_step_five_header = "";
		cActivitiesForStepP2S7.image_step_six_header = "";
		
		//definiendo cuales botones deben estar activos:
		cActivitiesForStepP2S7.btn_one_enable = true;
		cActivitiesForStepP2S7.btn_two_enable = true;
		cActivitiesForStepP2S7.btn_three_enable = false;
		cActivitiesForStepP2S7.btn_four_enable = false;
		cActivitiesForStepP2S7.btn_five_enable = false;
		cActivitiesForStepP2S7.btn_six_enable = false;
		cActivitiesForStepP2S7.btn_seven_enable = false;
		
		//asignando imagenes de los botones del contenido que redireccionan a cada actividad disponible:
		cActivitiesForStepP2S7.image_uno_tools_and_products = image_uno_tools_and_products;
		cActivitiesForStepP2S7.image_dos_videos = image_tres_self_assessment; //Aunque la variable se llama videos voy a asignarle la img del self assessment
		cActivitiesForStepP2S7.goToToolsAndProd += GoToToolsAndProductsEvalMode;
		cActivitiesForStepP2S7.interfaceCallingGoToTools = "Phase2Step7Eval";
		cActivitiesForStepP2S7.goToMenuSteps += GoBackToMenuOfStepsFromActivitiesEvalMode;
		cActivitiesForStepP2S7.interfaceGoingBackFrom = "Phase2Step7Eval";
		cActivitiesForStepP2S7.goToVideosForStep += GoToSelfAssessmentEvalMode; //Aunque la variable se llama videos voy a asignarle el llamado al modulo de self-assessment
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:IE31");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "IE31", "13", "-1","eval");
	} // cierra GoToActivitiesPhase2Step7EvalMode

	public void GoToActivitiesPhase2Step8EvalMode(){
		if (current_interface == CurrentInterface.MENU_STEPS_PHASE2_EV)
			Destroy (menuStepsPhase2_int_eval_instance);
		else if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_EVAL)
			Destroy (ToolsAndProductsPhase2Step8_int_eval_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP8_EV)
			Destroy (ARSearch_eval_mode_instance);
		
		
		current_interface = CurrentInterface.ACTIVITIES_PHASE2_STEP8_EV;
		
		//Ojo con lo siguiente estoy destruyendo todas las instancias que hayan de la interfaz de actividades por cada paso
		DestroyInstancesWithTag ("ActivitiesForEachStep");
		DestroyInstancesWithTag ("BuscarCapoCarroInterface");
		DestroyInstancesWithTag ("MenuPhasesEvalMode");
		DestroyInstancesWithTag ("LoginInterface");
		
		ActivitiesForPhase2Step8_int_eval_instance = Instantiate (ActivitiesForEachStepEvalMode);
		ActivitiesForEachStepEvalMode cActivitiesForStepP2S8 = ActivitiesForPhase2Step8_int_eval_instance.GetComponent<ActivitiesForEachStepEvalMode> ();
		cActivitiesForStepP2S8.titulo_current_step = title_phase2_step8_eval_mode;
		cActivitiesForStepP2S8.introduction_text_path = intro_text_phase2Step8_path_eval;
		
		//agregando imagenes que se cargan en el header para el conjunto de fases del proceso:
		cActivitiesForStepP2S8.image_phase_one_header = phase1_with_text_image_gray_path;
		cActivitiesForStepP2S8.image_phase_two_header = phase2_with_text_image_path;
		cActivitiesForStepP2S8.image_phase_three_header = phase3_with_text_image_gray_path;
		cActivitiesForStepP2S8.image_phase_four_header = phase4_with_text_image_gray_path;
		cActivitiesForStepP2S8.image_phase_five_header = phase5_with_text_image_gray_path;
		cActivitiesForStepP2S8.image_phase_six_header = phase6_with_text_image_gray_path;
		
		//agregando imagenes que se cargan en el header para el conjunto de pasos de la fase correspondiente:
		cActivitiesForStepP2S8.image_step_one_header = image_phase2step1_with_text_gray;
		cActivitiesForStepP2S8.image_step_two_header =  image_phase2step2_with_text_gray;
		cActivitiesForStepP2S8.image_step_three_header = image_phase2step3_with_text_gray;
		cActivitiesForStepP2S8.image_step_four_header = image_phase2step4_with_text; //esta imagen se muestra activa NO en gris porque es el paso correspondiente
		cActivitiesForStepP2S8.image_step_five_header = "";
		cActivitiesForStepP2S8.image_step_six_header = "";
		
		//definiendo cuales botones deben estar activos:
		cActivitiesForStepP2S8.btn_one_enable = true;
		cActivitiesForStepP2S8.btn_two_enable = true;
		cActivitiesForStepP2S8.btn_three_enable = false;
		cActivitiesForStepP2S8.btn_four_enable = false;
		cActivitiesForStepP2S8.btn_five_enable = false;
		cActivitiesForStepP2S8.btn_six_enable = false;
		cActivitiesForStepP2S8.btn_seven_enable = false;
		
		//asignando imagenes de los botones del contenido que redireccionan a cada actividad disponible:
		cActivitiesForStepP2S8.image_uno_tools_and_products = image_uno_tools_and_products;
		cActivitiesForStepP2S8.image_dos_videos = image_tres_self_assessment; //Aunque la variable se llama videos voy a asignarle la img del self assessment
		cActivitiesForStepP2S8.goToToolsAndProd += GoToToolsAndProductsEvalMode;
		cActivitiesForStepP2S8.interfaceCallingGoToTools = "Phase2Step8Eval";
		cActivitiesForStepP2S8.goToMenuSteps += GoBackToMenuOfStepsFromActivitiesEvalMode;
		cActivitiesForStepP2S8.interfaceGoingBackFrom = "Phase2Step8Eval";
		cActivitiesForStepP2S8.goToVideosForStep += GoToSelfAssessmentEvalMode; //Aunque la variable se llama videos voy a asignarle el llamado al modulo de self-assessment
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:IE32");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "IE32", "14", "-1","eval");
	}//cierra GoToActivitiesPhase2Step8EvalMode

	public void GoToToolsAndProductsEvalMode(string interface_from){
		
		Debug.LogError ("Llamado al metodo go to tools and products!!");
		//Destruyendo posibles instancias de interfaces que se estan creando: 
		DestroyInstancesWithTag ("LoginInterface");
		DestroyInstancesWithTag ("BuscarCapoCarroInterface");
		string fecha;
		switch (interface_from) {
			case "Phase1Step2Eval":
				Debug.Log ("Ingresa al case Phase1Step2Eval... Cargando Interfaz en GoToToolsAndProductsEvalMode");
				if (current_interface == CurrentInterface.ACTIVITIES_PHASE1_STEP2_EV)
					Destroy (ActivitiesForPhase1Step2_int_eval_instance);
				else if (current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP2_EV)
					Destroy (ARSearch_eval_mode_instance);
						
				current_interface = CurrentInterface.TOOLS_AND_PRODUCTS_EVAL;
				
				ToolsAndProductsPhase1Step2_int_eval_instance = Instantiate (ToolsAndProductsInterface);
				CanvasToolsAndProductsManager cToolsAndProductsManagerP1S2_eval = ToolsAndProductsPhase1Step2_int_eval_instance.GetComponent<CanvasToolsAndProductsManager> ();
				cToolsAndProductsManagerP1S2_eval.image_header_path = "Sprites/tools_and_products/tools";
				cToolsAndProductsManagerP1S2_eval.title_header_text_path = "Texts/EvalMode/Phase1Step2/2_title_header_tools_products";
				cToolsAndProductsManagerP1S2_eval.title_intro_content_text_path = "Texts/EvalMode/Phase1Step2/3_introduction_text";
				cToolsAndProductsManagerP1S2_eval.tool_one_text_path = "Texts/EvalMode/Phase1Step2/4_tool_text_one";
				cToolsAndProductsManagerP1S2_eval.tool_two_text_path = "Texts/EvalMode/Phase1Step2/5_tool_text_two";
				cToolsAndProductsManagerP1S2_eval.ruta_img_one_tool_path = "Sprites/phase1step1_eval/question_mark";
				cToolsAndProductsManagerP1S2_eval.ruta_img_two_tool_path = "Sprites/phase1step1_eval/question_mark";
				cToolsAndProductsManagerP1S2_eval.ruta_img_four_tool_path = "Sprites/phase1step1_eval/question_mark";
				cToolsAndProductsManagerP1S2_eval.footer_search_text_path = "Texts/EvalMode/Phase1Step2/6_ending_search_text";
				cToolsAndProductsManagerP1S2_eval.goBackButtonAction += GoToActivitiesPhase1Step2EvalMode;
				cToolsAndProductsManagerP1S2_eval.goToSearchProductsTools += GoToSearchAguaPresionP1S2Eval;
				cToolsAndProductsManagerP1S2_eval.interfaceGoingBackFrom = interface_from;
				//asignando la interfaz activa para controlar el regreso:
				this.interfaceInstanceActive = ToolsAndProductsPhase1Step2_int_eval_instance;
				fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:IE10");
				NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "IE10", "1", "-1","eval");
				
			break;
		case "Phase1Step3Eval":
			Debug.Log ("Ingresa al case Phase1Step3Eval... Cargando Interfaz en GoToToolsAndProductsEvalMode");
			if (current_interface == CurrentInterface.ACTIVITIES_PHASE1_STEP3_EV)
				Destroy (ActivitiesForPhase1Step3_int_eval_instance);
			else if (current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP3_EV)
				Destroy (ARSearch_eval_mode_instance);
			
			current_interface = CurrentInterface.TOOLS_AND_PRODUCTS_EVAL;
			
			ToolsAndProductsPhase1Step3_int_eval_instance = Instantiate (ToolsAndProductsInterface);
			CanvasToolsAndProductsManager cToolsAndProductsManagerP1S3_eval = ToolsAndProductsPhase1Step3_int_eval_instance.GetComponent<CanvasToolsAndProductsManager> ();
			cToolsAndProductsManagerP1S3_eval.image_header_path = "Sprites/tools_and_products/tools";
			cToolsAndProductsManagerP1S3_eval.title_header_text_path = "Texts/EvalMode/phase1step3/2_title_header_tools_products";
			cToolsAndProductsManagerP1S3_eval.title_intro_content_text_path = "Texts/EvalMode/phase1step3/3_introduction_text";
			cToolsAndProductsManagerP1S3_eval.tool_one_text_path = "Texts/EvalMode/phase1step3/4_tool_text_one";
			cToolsAndProductsManagerP1S3_eval.tool_two_text_path = "Texts/EvalMode/phase1step3/5_tool_text_two";
			cToolsAndProductsManagerP1S3_eval.ruta_img_one_tool_path = "Sprites/phase1step1_eval/question_mark";
			cToolsAndProductsManagerP1S3_eval.ruta_img_four_tool_path = "Sprites/phase1step1_eval/question_mark";
			cToolsAndProductsManagerP1S3_eval.footer_search_text_path = "Texts/EvalMode/phase1step3/6_ending_search_text";
			cToolsAndProductsManagerP1S3_eval.goBackButtonAction += GoToActivitiesPhase1Step3EvalMode;
			cToolsAndProductsManagerP1S3_eval.goToSearchProductsTools += GoToSearchAguaPresionP1S3Eval;
			cToolsAndProductsManagerP1S3_eval.interfaceGoingBackFrom = interface_from;
			//asignando la interfaz activa para controlar el regreso:
			this.interfaceInstanceActive = ToolsAndProductsPhase1Step3_int_eval_instance;
			fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:IE11");
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "IE11", "2", "-1","eval");
			break;
		case "Phase1Step4Eval":
			Debug.Log ("Ingresa al case Phase1Step4Eval... Cargando Interfaz en GoToToolsAndProductsEvalMode");
			if (current_interface == CurrentInterface.ACTIVITIES_PHASE1_STEP4_EV)
				Destroy (ActivitiesForPhase1Step4_int_eval_instance);
			else if (current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP4_EV)
				Destroy (ARSearch_eval_mode_instance);
			
			current_interface = CurrentInterface.TOOLS_AND_PRODUCTS_EVAL;
			
			ToolsAndProductsPhase1Step4_int_eval_instance = Instantiate (ToolsAndProductsInterface);
			CanvasToolsAndProductsManager cToolsAndProductsManagerP1S4_eval = ToolsAndProductsPhase1Step4_int_eval_instance.GetComponent<CanvasToolsAndProductsManager> ();
			cToolsAndProductsManagerP1S4_eval.image_header_path = "Sprites/tools_and_products/tools";
			cToolsAndProductsManagerP1S4_eval.title_header_text_path = "Texts/EvalMode/phase1step4/2_title_header_tools_products";
			cToolsAndProductsManagerP1S4_eval.title_intro_content_text_path = "Texts/EvalMode/phase1step4/3_introduction_text";
			cToolsAndProductsManagerP1S4_eval.tool_one_text_path = "Texts/EvalMode/phase1step4/4_tool_text_one";
			cToolsAndProductsManagerP1S4_eval.tool_two_text_path = "Texts/EvalMode/phase1step4/5_tool_text_two";
			cToolsAndProductsManagerP1S4_eval.ruta_img_one_tool_path = "Sprites/phase1step1_eval/question_mark";
			cToolsAndProductsManagerP1S4_eval.footer_search_text_path = "Texts/EvalMode/phase1step4/6_ending_search_text";
			cToolsAndProductsManagerP1S4_eval.goBackButtonAction += GoToActivitiesPhase1Step4EvalMode;
			cToolsAndProductsManagerP1S4_eval.goToSearchProductsTools += GoToSearchEsponjaP1S4Eval;
			cToolsAndProductsManagerP1S4_eval.interfaceGoingBackFrom = interface_from;
			//asignando la interfaz activa para controlar el regreso:
			this.interfaceInstanceActive = ToolsAndProductsPhase1Step4_int_eval_instance;
			fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:IE12");
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "IE12", "3", "-1","eval");
			break;
		case "Phase1Step5Eval":
			Debug.Log ("Ingresa al case Phase1Step5Eval... Cargando Interfaz en GoToToolsAndProductsEvalMode");
			if (current_interface == CurrentInterface.ACTIVITIES_PHASE1_STEP5_EV)
				Destroy (ActivitiesForPhase1Step5_int_eval_instance);
			else if (current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP5_EV)
				Destroy (ARSearch_eval_mode_instance);
			
			current_interface = CurrentInterface.TOOLS_AND_PRODUCTS_EVAL;
			
			ToolsAndProductsPhase1Step5_int_eval_instance = Instantiate (ToolsAndProductsInterface);
			CanvasToolsAndProductsManager cToolsAndProductsManagerP1S5_eval = ToolsAndProductsPhase1Step5_int_eval_instance.GetComponent<CanvasToolsAndProductsManager> ();
			cToolsAndProductsManagerP1S5_eval.image_header_path = "Sprites/tools_and_products/tools";
			cToolsAndProductsManagerP1S5_eval.title_header_text_path = "Texts/EvalMode/phase1step5/2_title_header_tools_products";
			cToolsAndProductsManagerP1S5_eval.title_intro_content_text_path = "Texts/EvalMode/phase1step5/3_introduction_text";
			cToolsAndProductsManagerP1S5_eval.tool_one_text_path = "Texts/EvalMode/phase1step5/4_tool_text_one";
			cToolsAndProductsManagerP1S5_eval.tool_two_text_path = "Texts/EvalMode/phase1step5/5_tool_text_two";
			cToolsAndProductsManagerP1S5_eval.ruta_img_one_tool_path = "Sprites/phase1step1_eval/question_mark";
			cToolsAndProductsManagerP1S5_eval.footer_search_text_path = "Texts/EvalMode/phase1step5/6_ending_search_text";
			cToolsAndProductsManagerP1S5_eval.goBackButtonAction += GoToActivitiesPhase1Step5EvalMode;
			cToolsAndProductsManagerP1S5_eval.goToSearchProductsTools += GoToSearchMartilloP1S5Eval;
			cToolsAndProductsManagerP1S5_eval.interfaceGoingBackFrom = interface_from;
			//asignando la interfaz activa para controlar el regreso:
			this.interfaceInstanceActive = ToolsAndProductsPhase1Step5_int_eval_instance;
			fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:IE13");
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "IE13", "4", "-1","eval");
			break;
		case "Phase1Step6Eval":
			Debug.Log ("Ingresa al case Phase1Step6Eval... Cargando Interfaz en GoToToolsAndProductsEvalMode");
			if (current_interface == CurrentInterface.ACTIVITIES_PHASE1_STEP6_EV)
				Destroy (ActivitiesForPhase1Step6_int_eval_instance);
			else if (current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP6_EV)
				Destroy (ARSearch_eval_mode_instance);
			
			current_interface = CurrentInterface.TOOLS_AND_PRODUCTS_EVAL;
			
			ToolsAndProductsPhase1Step6_int_eval_instance = Instantiate (ToolsAndProductsInterface);
			CanvasToolsAndProductsManager cToolsAndProductsManagerP1S6_eval = ToolsAndProductsPhase1Step6_int_eval_instance.GetComponent<CanvasToolsAndProductsManager> ();
			cToolsAndProductsManagerP1S6_eval.image_header_path = "Sprites/tools_and_products/tools";
			cToolsAndProductsManagerP1S6_eval.title_header_text_path = "Texts/EvalMode/phase1step6/2_title_header_tools_products";
			cToolsAndProductsManagerP1S6_eval.title_intro_content_text_path = "Texts/EvalMode/phase1step6/3_introduction_text";
			cToolsAndProductsManagerP1S6_eval.tool_one_text_path = "Texts/EvalMode/phase1step6/4_tool_text_one";
			cToolsAndProductsManagerP1S6_eval.tool_two_text_path = "Texts/EvalMode/phase1step6/5_tool_text_two";
			cToolsAndProductsManagerP1S6_eval.ruta_img_one_tool_path = "Sprites/phase1step1_eval/question_mark";
			cToolsAndProductsManagerP1S6_eval.ruta_img_four_tool_path = "Sprites/phase1step1_eval/question_mark";
			cToolsAndProductsManagerP1S6_eval.footer_search_text_path = "Texts/EvalMode/phase1step6/6_ending_search_text";
			cToolsAndProductsManagerP1S6_eval.goBackButtonAction += GoToActivitiesPhase1Step6EvalMode;
			cToolsAndProductsManagerP1S6_eval.goToSearchProductsTools += GoToSearchDesengrasanteP1S6Eval;
			cToolsAndProductsManagerP1S6_eval.interfaceGoingBackFrom = interface_from;
			//asignando la interfaz activa para controlar el regreso:
			this.interfaceInstanceActive = ToolsAndProductsPhase1Step6_int_eval_instance;
			fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:IE14");
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "IE14", "5", "-1","eval");
			break;
		case "Phase2Step2Eval":
			Debug.Log ("Ingresa al case Phase2Step2Eval... Cargando Interfaz en GoToToolsAndProductsEvalMode");
			if (current_interface == CurrentInterface.ACTIVITIES_PHASE2_STEP2_EV)
				Destroy (ActivitiesForPhase2Step2_int_eval_instance);
			else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP2_EV)
				Destroy (ARSearch_eval_mode_instance);
			
			current_interface = CurrentInterface.TOOLS_AND_PRODUCTS_EVAL;
			
			ToolsAndProductsPhase2Step2_int_eval_instance = Instantiate (ToolsAndProductsInterface);
			CanvasToolsAndProductsManager cToolsAndProductsManagerP2S2_eval = ToolsAndProductsPhase2Step2_int_eval_instance.GetComponent<CanvasToolsAndProductsManager> ();
			cToolsAndProductsManagerP2S2_eval.image_header_path = "Sprites/tools_and_products/tools";
			cToolsAndProductsManagerP2S2_eval.title_header_text_path = "Texts/EvalMode/phase2step2/2_title_header_tools_products";
			cToolsAndProductsManagerP2S2_eval.title_intro_content_text_path = "Texts/EvalMode/phase2step2/3_introduction_text";
			cToolsAndProductsManagerP2S2_eval.tool_one_text_path = "Texts/EvalMode/phase2step2/4_tool_text_one";
			cToolsAndProductsManagerP2S2_eval.tool_two_text_path = "Texts/EvalMode/phase2step2/5_tool_text_two";
			cToolsAndProductsManagerP2S2_eval.ruta_img_one_tool_path = "Sprites/phase1step1_eval/question_mark";
			cToolsAndProductsManagerP2S2_eval.ruta_img_four_tool_path = "Sprites/phase1step1_eval/question_mark";
			cToolsAndProductsManagerP2S2_eval.footer_search_text_path = "Texts/EvalMode/phase1step6/6_ending_search_text";
			cToolsAndProductsManagerP2S2_eval.goBackButtonAction += GoToActivitiesPhase2Step2EvalMode;
			cToolsAndProductsManagerP2S2_eval.goToSearchProductsTools += GoToSearchCintaEnmasP2S2Eval;
			cToolsAndProductsManagerP2S2_eval.interfaceGoingBackFrom = interface_from;
			//asignando la interfaz activa para controlar el regreso:
			this.interfaceInstanceActive = ToolsAndProductsPhase2Step2_int_eval_instance;
			fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:IE33");
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "IE33", "7", "-1","eval");
			break;
		case "Phase2Step3Eval":
			Debug.Log ("Ingresa al case Phase2Step3Eval... Cargando Interfaz en GoToToolsAndProductsEvalMode");
			if (current_interface == CurrentInterface.ACTIVITIES_PHASE2_STEP3_EV)
				Destroy (ActivitiesForPhase2Step3_int_eval_instance);
			else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP3_EV)
				Destroy (ARSearch_eval_mode_instance);
			
			current_interface = CurrentInterface.TOOLS_AND_PRODUCTS_EVAL;
			
			ToolsAndProductsPhase2Step3_int_eval_instance = Instantiate (ToolsAndProductsInterface);
			CanvasToolsAndProductsManager cToolsAndProductsManagerP2S3_eval = ToolsAndProductsPhase2Step3_int_eval_instance.GetComponent<CanvasToolsAndProductsManager> ();
			cToolsAndProductsManagerP2S3_eval.image_header_path = "Sprites/tools_and_products/tools";
			cToolsAndProductsManagerP2S3_eval.title_header_text_path = "Texts/EvalMode/phase2step3/2_title_header_tools_products";
			cToolsAndProductsManagerP2S3_eval.title_intro_content_text_path = "Texts/EvalMode/phase2step3/3_introduction_text";
			cToolsAndProductsManagerP2S3_eval.tool_one_text_path = "Texts/EvalMode/phase2step3/4_tool_text_one";
			cToolsAndProductsManagerP2S3_eval.tool_two_text_path = "Texts/EvalMode/phase2step3/5_tool_text_two";
			cToolsAndProductsManagerP2S3_eval.ruta_img_one_tool_path = "Sprites/phase1step1_eval/question_mark";
			cToolsAndProductsManagerP2S3_eval.ruta_img_four_tool_path = "Sprites/phase1step1_eval/question_mark";
			cToolsAndProductsManagerP2S3_eval.footer_search_text_path = "Texts/EvalMode/phase2step3/6_ending_search_text";
			cToolsAndProductsManagerP2S3_eval.goBackButtonAction += GoToActivitiesPhase2Step3EvalMode;
			cToolsAndProductsManagerP2S3_eval.goToSearchProductsTools += GoToSearchEsponjaP320P2S3Eval;
			cToolsAndProductsManagerP2S3_eval.interfaceGoingBackFrom = interface_from;
			//asignando la interfaz activa para controlar el regreso:
			this.interfaceInstanceActive = ToolsAndProductsPhase2Step3_int_eval_instance;
			fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:IE34");
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "IE34", "9", "-1","eval");
			break;
		case "Phase2Step4Eval":
			Debug.Log ("Ingresa al case Phase2Step4Eval... Cargando Interfaz en GoToToolsAndProductsEvalMode");
			if (current_interface == CurrentInterface.ACTIVITIES_PHASE2_STEP4_EV)
				Destroy (ActivitiesForPhase2Step4_int_eval_instance);
			else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP4_EV)
				Destroy (ARSearch_eval_mode_instance);
			
			current_interface = CurrentInterface.TOOLS_AND_PRODUCTS_EVAL;
			
			ToolsAndProductsPhase2Step4_int_eval_instance = Instantiate (ToolsAndProductsInterface);
			CanvasToolsAndProductsManager cToolsAndProductsManagerP2S4_eval = ToolsAndProductsPhase2Step4_int_eval_instance.GetComponent<CanvasToolsAndProductsManager> ();
			cToolsAndProductsManagerP2S4_eval.image_header_path = "Sprites/tools_and_products/tools";
			cToolsAndProductsManagerP2S4_eval.title_header_text_path = "Texts/EvalMode/phase2step4/2_title_header_tools_products";
			cToolsAndProductsManagerP2S4_eval.title_intro_content_text_path = "Texts/EvalMode/phase2step4/3_introduction_text";
			cToolsAndProductsManagerP2S4_eval.tool_one_text_path = "Texts/EvalMode/phase2step4/4_tool_text_one";
			cToolsAndProductsManagerP2S4_eval.tool_two_text_path = "Texts/EvalMode/phase2step4/5_tool_text_two";
			cToolsAndProductsManagerP2S4_eval.ruta_img_one_tool_path = "Sprites/phase1step1_eval/question_mark";
			cToolsAndProductsManagerP2S4_eval.ruta_img_four_tool_path = "Sprites/phase1step1_eval/question_mark";
			cToolsAndProductsManagerP2S4_eval.footer_search_text_path = "Texts/EvalMode/phase2step4/6_ending_search_text";
			cToolsAndProductsManagerP2S4_eval.goBackButtonAction += GoToActivitiesPhase2Step4EvalMode;
			cToolsAndProductsManagerP2S4_eval.goToSearchProductsTools += GoToSearchEsponjaP400P2S4Eval;
			cToolsAndProductsManagerP2S4_eval.interfaceGoingBackFrom = interface_from;
			//asignando la interfaz activa para controlar el regreso:
			this.interfaceInstanceActive = ToolsAndProductsPhase2Step4_int_eval_instance;
			fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:IE35");
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "IE35", "10", "-1","eval");
			break;
		case "Phase2Step5Eval":
			Debug.Log ("Ingresa al case Phase2Step5Eval... Cargando Interfaz en GoToToolsAndProductsEvalMode");
			if (current_interface == CurrentInterface.ACTIVITIES_PHASE2_STEP5_EV)
				Destroy (ActivitiesForPhase2Step5_int_eval_instance);
			else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP5_EV)
				Destroy (ARSearch_eval_mode_instance);
			
			current_interface = CurrentInterface.TOOLS_AND_PRODUCTS_EVAL;
			
			ToolsAndProductsPhase2Step5_int_eval_instance = Instantiate (ToolsAndProductsInterface);
			CanvasToolsAndProductsManager cToolsAndProductsManagerP2S5_eval = ToolsAndProductsPhase2Step5_int_eval_instance.GetComponent<CanvasToolsAndProductsManager> ();
			cToolsAndProductsManagerP2S5_eval.image_header_path = "Sprites/tools_and_products/tools";
			cToolsAndProductsManagerP2S5_eval.title_header_text_path = "Texts/EvalMode/phase2step5/2_title_header_tools_products";
			cToolsAndProductsManagerP2S5_eval.title_intro_content_text_path = "Texts/EvalMode/phase2step5/3_introduction_text";
			cToolsAndProductsManagerP2S5_eval.tool_one_text_path = "Texts/EvalMode/phase2step5/4_tool_text_one";
			cToolsAndProductsManagerP2S5_eval.tool_two_text_path = "Texts/EvalMode/phase2step5/5_tool_text_two";
			cToolsAndProductsManagerP2S5_eval.ruta_img_one_tool_path = "Sprites/phase1step1_eval/question_mark";
			cToolsAndProductsManagerP2S5_eval.ruta_img_two_tool_path = "Sprites/phase1step1_eval/question_mark";
			cToolsAndProductsManagerP2S5_eval.ruta_img_four_tool_path = "Sprites/phase1step1_eval/question_mark";
			cToolsAndProductsManagerP2S5_eval.footer_search_text_path = "Texts/EvalMode/phase2step5/6_ending_search_text";
			cToolsAndProductsManagerP2S5_eval.goBackButtonAction += GoToActivitiesPhase2Step5EvalMode;
			cToolsAndProductsManagerP2S5_eval.goToSearchProductsTools += GoToSearchRotoOrbitalP2S5Eval;
			cToolsAndProductsManagerP2S5_eval.interfaceGoingBackFrom = interface_from;
			//asignando la interfaz activa para controlar el regreso:
			this.interfaceInstanceActive = ToolsAndProductsPhase2Step5_int_eval_instance;
			fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:IE36");
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "IE36", "11", "-1","eval");
			break;
		case "Phase2Step6Eval":
			Debug.Log ("Ingresa al case Phase2Step6Eval... Cargando Interfaz en GoToToolsAndProductsEvalMode");
			if (current_interface == CurrentInterface.ACTIVITIES_PHASE2_STEP6_EV)
				Destroy (ActivitiesForPhase2Step6_int_eval_instance);
			else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP6_EV)
				Destroy (ARSearch_eval_mode_instance);
			
			current_interface = CurrentInterface.TOOLS_AND_PRODUCTS_EVAL;
			
			ToolsAndProductsPhase2Step6_int_eval_instance = Instantiate (ToolsAndProductsInterface);
			CanvasToolsAndProductsManager cToolsAndProductsManagerP2S6_eval = ToolsAndProductsPhase2Step6_int_eval_instance.GetComponent<CanvasToolsAndProductsManager> ();
			cToolsAndProductsManagerP2S6_eval.image_header_path = "Sprites/tools_and_products/tools";
			cToolsAndProductsManagerP2S6_eval.title_header_text_path = "Texts/EvalMode/phase2step6/2_title_header_tools_products";
			cToolsAndProductsManagerP2S6_eval.title_intro_content_text_path = "Texts/EvalMode/phase2step6/3_introduction_text";
			cToolsAndProductsManagerP2S6_eval.tool_one_text_path = "Texts/EvalMode/phase2step6/4_tool_text_one";
			cToolsAndProductsManagerP2S6_eval.tool_two_text_path = "Texts/EvalMode/phase2step6/5_tool_text_two";
			cToolsAndProductsManagerP2S6_eval.ruta_img_one_tool_path = "Sprites/phase1step1_eval/question_mark";
			cToolsAndProductsManagerP2S6_eval.ruta_img_four_tool_path = "Sprites/phase1step1_eval/question_mark";
			cToolsAndProductsManagerP2S6_eval.footer_search_text_path = "Texts/EvalMode/phase2step6/6_ending_search_text";
			cToolsAndProductsManagerP2S6_eval.goBackButtonAction += GoToActivitiesPhase2Step6EvalMode;
			cToolsAndProductsManagerP2S6_eval.goToSearchProductsTools += GoToSearchDiscoP150P2S6Eval;
			cToolsAndProductsManagerP2S6_eval.interfaceGoingBackFrom = interface_from;
			//asignando la interfaz activa para controlar el regreso:
			this.interfaceInstanceActive = ToolsAndProductsPhase2Step6_int_eval_instance;
			fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:IE37");
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "IE37", "12", "-1","eval");
			break;
		case "Phase2Step7Eval":
			Debug.Log ("Ingresa al case Phase2Step7Eval... Cargando Interfaz en GoToToolsAndProductsEvalMode");
			if (current_interface == CurrentInterface.ACTIVITIES_PHASE2_STEP7_EV)
				Destroy (ActivitiesForPhase2Step7_int_eval_instance);
			else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP7_EV)
				Destroy (ARSearch_eval_mode_instance);
			
			current_interface = CurrentInterface.TOOLS_AND_PRODUCTS_EVAL;
			
			ToolsAndProductsPhase2Step7_int_eval_instance = Instantiate (ToolsAndProductsInterface);
			CanvasToolsAndProductsManager cToolsAndProductsManagerP2S7_eval = ToolsAndProductsPhase2Step7_int_eval_instance.GetComponent<CanvasToolsAndProductsManager> ();
			cToolsAndProductsManagerP2S7_eval.image_header_path = "Sprites/tools_and_products/tools";
			cToolsAndProductsManagerP2S7_eval.title_header_text_path = "Texts/EvalMode/phase2step7/2_title_header_tools_products";
			cToolsAndProductsManagerP2S7_eval.title_intro_content_text_path = "Texts/EvalMode/phase2step7/3_introduction_text";
			cToolsAndProductsManagerP2S7_eval.tool_one_text_path = "Texts/EvalMode/phase2step7/4_tool_text_one";
			cToolsAndProductsManagerP2S7_eval.tool_two_text_path = "Texts/EvalMode/phase2step7/5_tool_text_two";
			cToolsAndProductsManagerP2S7_eval.ruta_img_one_tool_path = "Sprites/phase1step1_eval/question_mark";
			cToolsAndProductsManagerP2S7_eval.ruta_img_four_tool_path = "Sprites/phase1step1_eval/question_mark";
			cToolsAndProductsManagerP2S7_eval.footer_search_text_path = "Texts/EvalMode/phase2step7/6_ending_search_text";
			cToolsAndProductsManagerP2S7_eval.goBackButtonAction += GoToActivitiesPhase2Step7EvalMode;
			cToolsAndProductsManagerP2S7_eval.goToSearchProductsTools += GoToSearchDiscoP240P2S7Eval;
			cToolsAndProductsManagerP2S7_eval.interfaceGoingBackFrom = interface_from;
			//asignando la interfaz activa para controlar el regreso:
			this.interfaceInstanceActive = ToolsAndProductsPhase2Step7_int_eval_instance;
			fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:IE38");
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "IE38", "13", "-1","eval");
			break;
		case "Phase2Step8Eval":
			Debug.Log ("Ingresa al case Phase2Step8Eval... Cargando Interfaz en GoToToolsAndProductsEvalMode");
			if (current_interface == CurrentInterface.ACTIVITIES_PHASE2_STEP8_EV)
				Destroy (ActivitiesForPhase2Step8_int_eval_instance);
			else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP8_EV)
				Destroy (ARSearch_eval_mode_instance);
			
			current_interface = CurrentInterface.TOOLS_AND_PRODUCTS_EVAL;
			
			ToolsAndProductsPhase2Step8_int_eval_instance = Instantiate (ToolsAndProductsInterface);
			CanvasToolsAndProductsManager cToolsAndProductsManagerP2S8_eval = ToolsAndProductsPhase2Step8_int_eval_instance.GetComponent<CanvasToolsAndProductsManager> ();
			cToolsAndProductsManagerP2S8_eval.image_header_path = "Sprites/tools_and_products/tools";
			cToolsAndProductsManagerP2S8_eval.title_header_text_path = "Texts/EvalMode/phase2step8/2_title_header_tools_products";
			cToolsAndProductsManagerP2S8_eval.title_intro_content_text_path = "Texts/EvalMode/phase2step8/3_introduction_text";
			cToolsAndProductsManagerP2S8_eval.tool_one_text_path = "Texts/EvalMode/phase2step7/4_tool_text_one";
			cToolsAndProductsManagerP2S8_eval.tool_two_text_path = "Texts/EvalMode/phase2step7/5_tool_text_two";
			cToolsAndProductsManagerP2S8_eval.ruta_img_one_tool_path = "Sprites/phase1step1_eval/question_mark";
			cToolsAndProductsManagerP2S8_eval.ruta_img_four_tool_path = "Sprites/phase1step1_eval/question_mark";
			cToolsAndProductsManagerP2S8_eval.footer_search_text_path = "Texts/EvalMode/phase2step7/6_ending_search_text";
			cToolsAndProductsManagerP2S8_eval.goBackButtonAction += GoToActivitiesPhase2Step8EvalMode;
			cToolsAndProductsManagerP2S8_eval.goToSearchProductsTools += GoToSearchDiscoP320P2S8Eval;
			cToolsAndProductsManagerP2S8_eval.interfaceGoingBackFrom = interface_from;
			//asignando la interfaz activa para controlar el regreso:
			this.interfaceInstanceActive = ToolsAndProductsPhase2Step8_int_eval_instance;
			fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:IE39");
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "IE39", "14", "-1","eval");
			break;
		}//cierra switch-case
		
	}//cierra metodo GoToToolsAndProductsEvalMode


	public void GoBackToMenuOfStepsFromActivitiesEvalMode(string interface_coming_from){
		
		Debug.LogError ("Llamado al metodo go back to menu of Steps from Activities!!");
		
		switch (interface_coming_from) {
			case "Phase1Step2Eval":
				Debug.Log ("Ingresa a Phase1Step2Eval en GoBackToMenuOfStepsFromActivities con interface_coming_from= " + interface_coming_from);
				GoToMenuStepsPhase1Eval ();
			break;
			case "Phase1Step3Eval":
				Debug.Log ("Ingresa a Phase1Step3Eval en GoBackToMenuOfStepsFromActivities con interface_coming_from= " + interface_coming_from);
				GoToMenuStepsPhase1Eval ();
			break;
			case "Phase1Step4Eval":
				Debug.Log ("Ingresa a Phase1Step4Eval en GoBackToMenuOfStepsFromActivities con interface_coming_from= " + interface_coming_from);
				GoToMenuStepsPhase1Eval ();
			break;
			case "Phase1Step5Eval":
				Debug.Log ("Ingresa a Phase1Step5Eval en GoBackToMenuOfStepsFromActivities con interface_coming_from= " + interface_coming_from);
				GoToMenuStepsPhase1Eval ();
			break;
			case "Phase1Step6Eval":
				Debug.Log ("Ingresa a Phase1Step6Eval en GoBackToMenuOfStepsFromActivities con interface_coming_from= " + interface_coming_from);
				GoToMenuStepsPhase1Eval ();
				break;
			case "Phase2Step2Eval":
				Debug.Log ("Ingresa a Phase2Step2Eval en GoBackToMenuOfStepsFromActivities con interface_coming_from= " + interface_coming_from);
				GoToMenuStepsPhase2Eval ();
				break;
			case "Phase2Step3Eval":
				Debug.Log ("Ingresa a Phase2Step3Eval en GoBackToMenuOfStepsFromActivities con interface_coming_from= " + interface_coming_from);
				GoToMenuStepsPhase2Eval ();
				break;
			case "Phase2Step4Eval":
				Debug.Log ("Ingresa a Phase2Step4Eval en GoBackToMenuOfStepsFromActivities con interface_coming_from= " + interface_coming_from);
				GoToMenuStepsPhase2Eval ();
				break;
			case "Phase2Step5Eval":
				Debug.Log ("Ingresa a Phase2Step5Eval en GoBackToMenuOfStepsFromActivities con interface_coming_from= " + interface_coming_from);
				GoToMenuStepsPhase2Eval ();
				break;
			case "Phase2Step6Eval":
			Debug.Log ("Ingresa a Phase2Step6Eval en GoBackToMenuOfStepsFromActivities con interface_coming_from= " + interface_coming_from);
				GoToMenuStepsPhase2Eval ();
				break;
			case "Phase2Step7Eval":
				Debug.Log ("Ingresa a Phase2Step7Eval en GoBackToMenuOfStepsFromActivities con interface_coming_from= " + interface_coming_from);
				GoToMenuStepsPhase2Eval ();
				break;
			case "Phase2Step8Eval":
				Debug.Log ("Ingresa a Phase2Step7Eval en GoBackToMenuOfStepsFromActivities con interface_coming_from= " + interface_coming_from);
				GoToMenuStepsPhase2Eval ();
				break;
		}
	}//cierra GoBackToMenuOfStepsFromActivitiesEvalMode


	/// <summary>
	/// Goes the back to menu of activities from tools products.
	/// </summary>
	/// <param name="interface_coming_from">Interface_coming_from.</param>
	public void GoBackMenuActivFromToolsEvalMode(string interface_coming_from){
		
		Debug.LogError ("Llamado al metodo GoBackMenuActivFromToolsEvalMode con interfaz=" + interface_coming_from);
		
		switch (interface_coming_from) {
		case "Phase1Step2Eval":
			GoToActivitiesPhase1Step2EvalMode();
			break;
		case "Phase1Step3Eval":
			GoToActivitiesPhase1Step3EvalMode();
			break;
		case "Phase1Step4Eval":
			GoToActivitiesPhase1Step4EvalMode();
			break;
		case "Phase1Step5Eval":
			GoToActivitiesPhase1Step5EvalMode();
			break;
		case "Phase1Step6Eval":
			GoToActivitiesPhase1Step6EvalMode();
			break;
		case "Phase2Step2Eval":
			GoToActivitiesPhase2Step2EvalMode();
			break;
		case "Phase2Step3Eval":
			GoToActivitiesPhase2Step3EvalMode();
			break;
		case "Phase2Step4Eval":
			GoToActivitiesPhase2Step4EvalMode();
			break;
		case "Phase2Step5Eval":
			GoToActivitiesPhase2Step5EvalMode();
			break;
		case "Phase2Step6Eval":
			GoToActivitiesPhase2Step6EvalMode();
			break;
		case "Phase2Step7Eval":
			GoToActivitiesPhase2Step7EvalMode();
			break;
		case "Phase2Step8Eval":
			GoToActivitiesPhase2Step8EvalMode();
			break;
			
		case "":
			Debug.Log("ERROR: No hay interface_coming_from definido en el metodo GoBackFromToolsAndProducts");
			break;
		}//cierra switch
	} //cierra GoBackToMenuActivitiesFromToolsProducts
	
	
	public void GoToSelfAssessmentEvalMode(string interface_from){
		string numero_paso = "";
		string numero_de_pregs = "5"; //este parametro ahora se fija desde la interfaz web de la App Pintura. Por lo tanto este es un valor por defecto.
		string tipo_test = ""; //variable que define el tipo de test 1 = modo guiado, 2 = modo evaluacion.
		//obteniendo la fecha para registrar los eventos de interfaz:
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

		switch (interface_from) {
			case "Phase1Step2Eval":
				Debug.Log ("Llamando al metodo GoToSelfAssessment con parametro Phase1Step2");
				numero_paso = "1";
				tipo_test = "2";
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:AE1");
				NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "AE1", "1", "-1","eval");
				break;
			case "Phase1Step3Eval":
				Debug.Log ("Llamando al metodo GoToSelfAssessment con parametro Phase1Step3");
				numero_paso = "2";
				tipo_test = "2";
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:AE2");
				NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "AE2", "2", "-1","eval");
				break;
			case "Phase1Step4Eval":
				Debug.Log ("Llamando al metodo GoToSelfAssessment con parametro Phase1Step4");
				numero_paso = "3";
				tipo_test = "2";
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:AE3");
				NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "AE3", "3", "-1","eval");
				break;
			case "Phase1Step5Eval":
				Debug.Log ("Llamando al metodo GoToSelfAssessment con parametro Phase1Step5");
				numero_paso = "4";
				tipo_test = "2";
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:AE4");
				NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "AE4", "4", "-1","eval");
				break;
			case "Phase1Step6Eval":
				Debug.Log ("Llamando al metodo GoToSelfAssessment con parametro Phase1Step6");
				numero_paso = "5";
				tipo_test = "2";
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:AE5");
				NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "AE5", "5", "-1","eval");
				break;
			case "Phase2Step2Eval":
				Debug.Log ("Llamando al metodo GoToSelfAssessment con parametro Phase2Step2");
				numero_paso = "7";
				tipo_test = "2";
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:AE6");
				NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "AE6", "7", "-1","eval");
				break;
			case "Phase2Step3Eval":
				Debug.Log ("Llamando al metodo GoToSelfAssessment con parametro Phase2Step3");
				numero_paso = "9"; //el numero de paso en este caso SI ES 9 porque en la BD web es asi
				tipo_test = "2";
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:AE7");
				NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "AE7", "9", "-1","eval");
				break;
			case "Phase2Step4Eval":
				Debug.Log ("Llamando al metodo GoToSelfAssessment con parametro Phase2Step4");
				numero_paso = "10"; 
				tipo_test = "2";
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:AE8");
				NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "AE8", "10", "-1","eval");
				break;
			case "Phase2Step5Eval":
				Debug.Log ("Llamando al metodo GoToSelfAssessment con parametro Phase2Step5");
				numero_paso = "11"; 
				tipo_test = "2";
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:AE9");
				NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "AE9", "11", "-1","eval");
				break;
			case "Phase2Step6Eval":
				Debug.Log ("Llamando al metodo GoToSelfAssessment con parametro Phase2Step6");
				numero_paso = "12"; 
				tipo_test = "2";
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:AE10");
				NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "AE10", "12", "-1","eval");
				break;
			case "Phase2Step7Eval":
				Debug.Log ("Llamando al metodo GoToSelfAssessment con parametro Phase2Step7");
				numero_paso = "13"; 
				tipo_test = "2";
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:AE11");
				NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "AE11", "13", "-1","eval");
				break;
			case "Phase2Step8Eval":
				Debug.Log ("Llamando al metodo GoToSelfAssessment con parametro Phase2Step8");
				numero_paso = "14"; 
				tipo_test = "2";
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:AE12");
				NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "AE12", "14", "-1","eval");
				break;
			
		} //cierra metodo switch
		
		//Cargando la URL desde un archivo de texto para que se pueda parametrizar facilmente:
		TextAsset url_server = Resources.Load<TextAsset> ("Texts/00_server_base_path");
		string url_conexion = "";
		if (url_server != null)
			url_conexion = url_server.text;
		
		Debug.Log ("Click en el Metodo SelfAssessment!!!"); 
		var and_unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		var current_act = and_unity.GetStatic<AndroidJavaObject>("currentActivity");
		Debug.Log("Se ha obtenido current activity...");
		// Accessing the class to call a static method on it
		var assessment_activity = new AndroidJavaClass("edu.udg.bcds.pintura.arapp.SelfAssessmentMain");
		//var jc = new AndroidJavaClass("edu.udg.bcds.pintura.tools.SelfAssessment");
		//var video_activity = new AndroidJavaClass("edu.udg.bcds.pintura.arapp.VideoActivity");
		Debug.Log ("Se ha obtenido StartActivity...");
		
		object[] parameters = new object[6]; 
		parameters [0] = current_act; //pasando el argumento de la actividad actual que se debe reproducir
		parameters [1] = numero_de_pregs; //definiendo el numero de preguntas que se quiere que aparezcan
		parameters [2] = numero_paso; //definiendo el paso para el cual se quieren cargar las preguntas
		parameters [3] = url_conexion;
		parameters [4] = this.codigo_estudiante;
		parameters [5] = tipo_test;
		// Calling a Call method to which the current activity is passed
		assessment_activity.CallStatic("StartModule", parameters);
		
	} // cierra metodo GoToSelfAssessment


	public void ResultSelfAssessmEvalMode(string msg){
		Debug.Log ("**********Se ha invocado el metodo ResultSelfAssessmEvalMode msg=" + msg);

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		switch (msg) {
		 	case "1":
				Debug.Log ("AppManager: Se ha completado el test del paso 1");
				steps_p_one_eval_completed[1].selfevaluation = true;
				steps_p_one_eval_completed[1].CheckStepCompletion();
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:SAE11");
				NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "SAE1", msg, "-1","eval");
			break;
			case "2":
				Debug.Log ("AppManager: Se ha completado el test del paso 2");
				steps_p_one_eval_completed[2].selfevaluation = true;
				steps_p_one_eval_completed[2].CheckStepCompletion();
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:SAE2");
				NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "SAE2", msg, "-1","eval");
			break;
			case "3":
				Debug.Log ("AppManager: Se ha completado el test del paso 3");
				steps_p_one_eval_completed[3].selfevaluation = true;
				steps_p_one_eval_completed[3].CheckStepCompletion();
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:SAE3");
				NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "SAE3", msg, "-1","eval");
			break;
			case "4":
				Debug.Log ("AppManager: Se ha completado el test del paso 4");
				steps_p_one_eval_completed[4].selfevaluation = true;
				steps_p_one_eval_completed[4].CheckStepCompletion();
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:SAE4");
				NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "SAE4", msg, "-1","eval");
				break;
			case "5":
				Debug.Log ("AppManager: Se ha completado el test del paso 5 - Densengrasado");
				steps_p_one_eval_completed[5].selfevaluation = true;
				steps_p_one_eval_completed[5].CheckStepCompletion();
				if(steps_p_one_eval_completed[5].step_completed)
					phase_two_enable_eval_mode = true;
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:SAE5");
				NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "SAE5", msg, "-1","eval");
				break;
			case "7": //AQUI COMIENZAN LOS PASOS DE LA FASE DE MATIZADO EN EL MODO EVALUACION
				Debug.Log ("AppManager: Se ha completado el test del paso 7 - Proteccion de Superf");
				steps_p_two_eval_completed[1].selfevaluation = true;
				steps_p_two_eval_completed[1].CheckStepCompletion();
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:SAE6");
				NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "SAE6", msg, "-1","eval");
				break;
			case "9": 
				Debug.Log ("AppManager: Se ha completado el test del paso 9 - Lijado cantos primera pasada");
				steps_p_two_eval_completed[2].selfevaluation = true;
				steps_p_two_eval_completed[2].CheckStepCompletion();
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:SAE7");
				NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "SAE7", msg, "-1","eval");
				break;
			case "10": 
				Debug.Log ("AppManager: Se ha completado el test del paso 10 - Lijado cantos pasada final");
				steps_p_two_eval_completed[3].selfevaluation = true;
				steps_p_two_eval_completed[3].CheckStepCompletion();	
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:SAE8");
				NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "SAE8", msg, "-1","eval");
				break;
			case "11": 
				Debug.Log ("AppManager: Se ha completado el test del paso 11 - Lijado interiores - primera pasada");
				steps_p_two_eval_completed[4].selfevaluation = true;
				steps_p_two_eval_completed[4].CheckStepCompletion();
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:SAE9");
				NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "SAE9", msg, "-1","eval");
				break;
			case "12": 
				Debug.Log ("AppManager: Se ha completado el test del paso 12 - Lijado interiores - segunda pasada");
				steps_p_two_eval_completed[5].selfevaluation = true;
				steps_p_two_eval_completed[5].CheckStepCompletion();
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:SAE10");
				NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "SAE10", msg, "-1","eval");
				break;
			case "13": 
				Debug.Log ("AppManager: Se ha completado el test del paso 13 - Lijado interiores - tercera pasada");
				steps_p_two_eval_completed[6].selfevaluation = true;
				steps_p_two_eval_completed[6].CheckStepCompletion();
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:SAE11");
				NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "SAE11", msg, "-1","eval");
				break;
			case "14": 
				Debug.Log ("AppManager: Se ha completado el test del paso 14 - Lijado interiores - pasada final");
				steps_p_two_eval_completed[7].selfevaluation = true;
				steps_p_two_eval_completed[7].CheckStepCompletion();
				Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:SAE12");
				NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "SAE12", msg, "-1","eval");
				break;
		} //cierra switch
		//guardando el estado de la aplicacion en este momento:
		SaveDataForStudent ();
	} //cierra ResultSelfAssessmEvalMode

	
	/// <summary>
	/// Goes to search capo coche. In RA mode from the Evaluation mode
	/// </summary>
	public void GoToSearchCapoCocheEvalMode(){
		Debug.Log ("Entra al metodo GoToSearchCapoCocheEvalMode... interfaz: "  + current_interface);
		if (current_interface == CurrentInterface.ACTIVITIES_PHASE1_STEP1_EV) {
			Destroy(ActivitiesForPhase1Step1_int_eval_instance);
		}

		//Verificar porque se estan quedando instanciadas las interfaces para evitar hacer estos dos llamados:
		DestroyInstancesWithTag ("MenuPhasesEvalMode");
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhaseOneEval");
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhaseTwoEval");
		DestroyInstancesWithTag ("ActivitiesForEachStep");
		DestroyInstancesWithTag ("LoginInterface");
		
		current_interface = CurrentInterface.AR_SEARCH_CAR_HOOD_EVAL;
		
		SearchCapoCarroEvaluationMode_instance = Instantiate (BuscarCapoCarroEvaluationMode);
		ControllerBlinkARModeEvaluation controller_blinking = SearchCapoCarroEvaluationMode_instance.GetComponent<ControllerBlinkARModeEvaluation> ();
		
		//Es iportante asignar esta variable para poder controlar los marcadores
		interfaceInstanceActive = SearchCapoCarroEvaluationMode_instance;
		
		//localizando el marcador que se debe buscar en esta fase del proceso y cargando datos respectivos:
		markerInScene = GameObject.Find ("FrameMarker1");
		ControllerAddInfoInMarker controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		
		//asignando el texto que se debe mostrar al momento del feedback:
		controller_blinking.feedback_info_text_path = feedback_capo_text_path;
		
		//definiendo los ordenes que controlara esta interfaz:
		order_in_process = new int[1];
		order_in_process [0] = 1;
		controller_blinking.ordenes = order_in_process;

		//El llamado al siguiente metodo carga la informacion correspondiente en la interfaz:
		//NOTA: antes de hacer esto es importante que la variable interfaceInstanceActive este asignada:
		//tambien es importante haber asignado la variable inTutorialPhase1
		controller_info_marker.LoadInformationForEvaluationMode ();

		controller_blinking.interface_going_from = "Phase1Step1Eval";
		controller_blinking.onClickSelectBtn += OnClickSelectCapoCarroEvalMode;

		//asignando el texto que se debe mostrar de guia:
		controller_blinking.texto_guia_producto = "Busca el producto 1";

		//inicializando la variable que controla si estamos en el modo de evaluacion en RA:
		in_Evaluation_mode = true;

		//iniciando el proceso blinking:
		//blinkingMarker.should_be_blinking = true;
		
		//colocando en false la informacion adicional por si se le habia dado atras en algun momento en la interfaz:
		info_additional_displayed = false;

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: IE15");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "IE15", "0", "-1","eval");

		Debug.LogError ("Finaliza el metodo GoToSearchCapoCocheEvalMode");
		
	} // cierra GoToSearchCapoCocheEvalMode

	public void OnClickSelectCapoCarroEvalMode(string interface_from){
		Debug.Log ("Llamado al metodo OnClickSelectCapoCarroEvalMode!!");

		info_additional_displayed = false;

		in_Evaluation_mode = false;

		steps_p_one_eval_completed [0].activity_tools_and_products = true;
		steps_p_one_eval_completed [0].selfevaluation = true;
		steps_p_one_eval_completed [0].CheckStepCompletion ();

		SaveDataForStudent ();

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: SE1");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "SE1", "0", "-1","eval");

		GoToMenuStepsPhase1Eval ();
	} // cierra OnClickSelectCapoCarroEvalMode


	public void GoToSearchAguaPresionP1S2Eval(){

		Debug.Log ("Entra al metodo GoToSearchAguaPresionP1S2Eval... interfaz: "  + current_interface);
		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_EVAL) {
			Destroy(ToolsAndProductsPhase1Step2_int_eval_instance);
		}
		
		//Verificar porque se estan quedando instanciadas las interfaces para evitar hacer estos dos llamados:
		DestroyInstancesWithTag ("MenuPhasesEvalMode");
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhaseOneEval");
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhaseTwoEval");
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");
		DestroyInstancesWithTag ("LoginInterface");
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE1_STEP2_EV;
		
		ARSearch_eval_mode_instance = Instantiate (ARModeEvaluation);
		ControllerBlinkARModeEvaluation controller_blinking = ARSearch_eval_mode_instance.GetComponent<ControllerBlinkARModeEvaluation> ();
		
		//Es iportante asignar esta variable para poder controlar los marcadores
		interfaceInstanceActive = ARSearch_eval_mode_instance;
		
		//localizando el marcador que se debe buscar en esta fase del proceso y cargando datos respectivos:
		markerInScene = GameObject.Find ("FrameMarker16");
		ControllerAddInfoInMarker controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		
		//asignando el texto que se debe mostrar al momento del feedback:
		controller_blinking.feedback_info_text_path = feedback_marker16_eval_mode;
		
		//definiendo los ordenes que controlara esta interfaz:
		order_in_process = new int[1];
		order_in_process [0] = 2;
		controller_blinking.ordenes = order_in_process;
		
		//El llamado al siguiente metodo carga la informacion correspondiente en la interfaz:
		//NOTA: antes de hacer esto es importante que la variable interfaceInstanceActive este asignada:
		//tambien es importante haber asignado la variable inTutorialPhase1
		controller_info_marker.LoadInformationForEvaluationMode ();
		
		controller_blinking.interface_going_from = "Phase1Step2Eval";
		controller_blinking.onClickSelectBtn += OnClickSelectAguaPresionP1S2Eval;

		//asignando el texto que se debe mostrar de guia:
		controller_blinking.texto_guia_producto = "Busca el producto 1";

		//inicializando la variable que controla si estamos en el modo de evaluacion en RA:
		in_Evaluation_mode = true;
		
		//colocando en false la informacion adicional por si se le habia dado atras en algun momento en la interfaz:
		info_additional_displayed = false;

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:IE16");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "IE16", "1", "-1","eval");

	} // cierra GoToSearchAguaPresionP1S2Eval


	public void OnClickSelectAguaPresionP1S2Eval(string interface_from){
		Debug.Log ("Llamado al metodo OnClickSelectCapoCarroEvalMode!!");
		info_additional_displayed = false;
		in_Evaluation_mode = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:SE2");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "SE2", "1", "-1","eval");
		GoToSearchAguaJabonP1S2Eval ();
	} // cierra OnClickSelectAguaPresionP1S2Eval

	public void GoToSearchAguaJabonP1S2Eval(){
		Debug.Log ("Entra al metodo GoToSearchAguaJabonP1S2Eval... interfaz: "  + current_interface);
		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_EVAL) {
			Destroy (ToolsAndProductsPhase1Step2_int_eval_instance);
		} else if (current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP2_EV)
			Destroy (ARSearch_eval_mode_instance);
		
		//Verificar porque se estan quedando instanciadas las interfaces para evitar hacer estos dos llamados:
		DestroyInstancesWithTag ("MenuPhasesEvalMode");
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhaseOneEval");
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhaseTwoEval");
		DestroyInstancesWithTag ("LoginInterface");
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE1_STEP2_EV;
		
		ARSearch_eval_mode_instance = Instantiate (ARModeEvaluation);
		ControllerBlinkARModeEvaluation controller_blinking_second = ARSearch_eval_mode_instance.GetComponent<ControllerBlinkARModeEvaluation> ();
		
		//Es iportante asignar esta variable para poder controlar los marcadores
		interfaceInstanceActive = ARSearch_eval_mode_instance;
		
		//localizando el marcador que se debe buscar en esta fase del proceso y cargando datos respectivos:
		markerInScene = GameObject.Find ("FrameMarker19");
		ControllerAddInfoInMarker controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		
		//asignando el texto que se debe mostrar al momento del feedback:
		controller_blinking_second.feedback_info_text_path = feedback_marker19_eval_mode;
		
		//definiendo los ordenes que controlara esta interfaz:
		order_in_process = new int[1];
		order_in_process [0] = 3;
		controller_blinking_second.ordenes = order_in_process;
		
		//El llamado al siguiente metodo carga la informacion correspondiente en la interfaz:
		//NOTA: antes de hacer esto es importante que la variable interfaceInstanceActive este asignada:
		//tambien es importante haber asignado la variable inTutorialPhase1
		controller_info_marker.LoadInformationForEvaluationMode ();
		
		controller_blinking_second.interface_going_from = "Phase1Step2Eval";
		controller_blinking_second.onClickSelectBtn += OnClickSelectAguaJabonP1S2Eval;

		//asignando el texto que se debe mostrar de guia:
		controller_blinking_second.texto_guia_producto = "Busca el producto 2";
		
		//inicializando la variable que controla si estamos en el modo de evaluacion en RA:
		in_Evaluation_mode = true;
		
		//colocando en false la informacion adicional por si se le habia dado atras en algun momento en la interfaz:
		info_additional_displayed = false;

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:IE17");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "IE17", "1", "-1","eval");

	} //cierra GoToSearchAguaJabonP1S2Eval


	public void OnClickSelectAguaJabonP1S2Eval(string interface_from){
		Debug.Log ("Llamado al metodo OnClickSelectAguaJabonP1S2Eval!!");
		info_additional_displayed = false;
		in_Evaluation_mode = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:SE3");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "SE3", "1", "-1","eval");
		GoToSearchBayetaP1S2Eval ();
	} //cierra OnClickSelectAguaJabonP1S2Eval

	public void GoToSearchBayetaP1S2Eval(){
		Debug.Log ("Entra al metodo GoToSearchAguaJabonP1S2Eval... interfaz: "  + current_interface);
		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_EVAL) {
			Destroy (ToolsAndProductsPhase1Step2_int_eval_instance);
		} else if (current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP2_EV)
			Destroy (ARSearch_eval_mode_instance);
		
		//Verificar porque se estan quedando instanciadas las interfaces para evitar hacer estos dos llamados:
		DestroyInstancesWithTag ("MenuPhasesEvalMode");
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhaseOneEval");
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE1_STEP2_EV;
		
		ARSearch_eval_mode_instance = Instantiate (ARModeEvaluation);
		ControllerBlinkARModeEvaluation controller_blinking_third = ARSearch_eval_mode_instance.GetComponent<ControllerBlinkARModeEvaluation> ();
		
		//Es iportante asignar esta variable para poder controlar los marcadores
		interfaceInstanceActive = ARSearch_eval_mode_instance;
		
		//localizando el marcador que se debe buscar en esta fase del proceso y cargando datos respectivos:
		markerInScene = GameObject.Find ("FrameMarker21");
		ControllerAddInfoInMarker controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		
		//asignando el texto que se debe mostrar al momento del feedback:
		controller_blinking_third.feedback_info_text_path = feedback_marker21_eval_mode;
		
		//definiendo los ordenes que controlara esta interfaz:
		order_in_process = new int[1];
		order_in_process [0] = 4;
		controller_blinking_third.ordenes = order_in_process;
		
		//El llamado al siguiente metodo carga la informacion correspondiente en la interfaz:
		//NOTA: antes de hacer esto es importante que la variable interfaceInstanceActive este asignada:
		//tambien es importante haber asignado la variable inTutorialPhase1
		controller_info_marker.LoadInformationForEvaluationMode ();
		
		controller_blinking_third.interface_going_from = "Phase1Step2Eval";
		controller_blinking_third.onClickSelectBtn += OnClickSelectBayetaP1S2Eval;

		//asignando el texto que se debe mostrar de guia:
		controller_blinking_third.texto_guia_producto = "Busca el producto 3";
		
		//inicializando la variable que controla si estamos en el modo de evaluacion en RA:
		in_Evaluation_mode = true;
		
		//colocando en false la informacion adicional por si se le habia dado atras en algun momento en la interfaz:
		info_additional_displayed = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:IE18");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "IE18", "1", "-1","eval");
	} // cierra GoToSearchBayetaP1S2Eval

	public void OnClickSelectBayetaP1S2Eval(string interface_from){
		Debug.Log ("Llamado al metodo OnClickSelectBayetaP1S2Eval!!");
		info_additional_displayed = false;
		in_Evaluation_mode = false;
		//notificando que ya se ha completado la parte de productos y herramientas
		steps_p_one_eval_completed [1].activity_tools_and_products = true;
		steps_p_one_eval_completed [1].CheckStepCompletion ();

		SaveDataForStudent ();

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:SE4");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "SE4", "1", "-1","eval");

		if (steps_p_one_eval_completed [1].step_completed)
			GoToMenuStepsPhase1Eval ();
		else 
			GoToActivitiesPhase1Step2EvalMode ();
	}


	public void GoToSearchAguaPresionP1S3Eval(){
		Debug.Log ("Entra al metodo GoToSearchAguaPresionP1S3Eval... interfaz: "  + current_interface);
		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_EVAL) {
			Destroy (ToolsAndProductsPhase1Step3_int_eval_instance);
		} else if (current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP3_EV)
			Destroy (ARSearch_eval_mode_instance);
		
		//Verificar porque se estan quedando instanciadas las interfaces para evitar hacer estos dos llamados:
		DestroyInstancesWithTag ("MenuPhasesEvalMode");
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhaseOneEval");
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhaseTwoEval");
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE1_STEP3_EV;
		
		ARSearch_eval_mode_instance = Instantiate (ARModeEvaluation);
		ControllerBlinkARModeEvaluation controller_blinking = ARSearch_eval_mode_instance.GetComponent<ControllerBlinkARModeEvaluation> ();
		
		//Es iportante asignar esta variable para poder controlar los marcadores
		interfaceInstanceActive = ARSearch_eval_mode_instance;
		
		//localizando el marcador que se debe buscar en esta fase del proceso y cargando datos respectivos:
		markerInScene = GameObject.Find ("FrameMarker16");
		ControllerAddInfoInMarker controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		
		//asignando el texto que se debe mostrar al momento del feedback:
		controller_blinking.feedback_info_text_path = feedback_marker16_eval_mode_step3;
		
		//definiendo los ordenes que controlara esta interfaz:
		order_in_process = new int[1];
		order_in_process [0] = 5;
		controller_blinking.ordenes = order_in_process;
		
		//El llamado al siguiente metodo carga la informacion correspondiente en la interfaz:
		//NOTA: antes de hacer esto es importante que la variable interfaceInstanceActive este asignada:
		//tambien es importante haber asignado la variable inTutorialPhase1
		controller_info_marker.LoadInformationForEvaluationMode ();
		
		controller_blinking.interface_going_from = "Phase1Step3Eval";
		controller_blinking.onClickSelectBtn += OnClickSelectAguaPresP1S3Eval;
		
		//asignando el texto que se debe mostrar de guia:
		controller_blinking.texto_guia_producto = "Busca el elemento 1";
		
		//inicializando la variable que controla si estamos en el modo de evaluacion en RA:
		in_Evaluation_mode = true;
		
		//colocando en false la informacion adicional por si se le habia dado atras en algun momento en la interfaz:
		info_additional_displayed = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:IE19");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "IE19", "2", "-1","eval");
	}//cierra GoToSearchAguaPresionP1S3Eval

	public void OnClickSelectAguaPresP1S3Eval(string interface_from){
		Debug.Log ("Llamado al metodo OnClickSelectAguaPresP1S3Eval!!");
		info_additional_displayed = false;
		in_Evaluation_mode = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:SE5");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "SE5", "2", "-1","eval");
		GoToSearchPapelLimpiezaP1S3Eval ();
	}// cierra OnClickSelectAguaPresP1S3Eval


	public void GoToSearchPapelLimpiezaP1S3Eval(){
		Debug.Log ("Entra al metodo GoToSearchPapelLimpiezaP1S3Eval... interfaz: "  + current_interface);
		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_EVAL) {
			Destroy (ToolsAndProductsPhase1Step3_int_eval_instance);
		} else if (current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP3_EV)
			Destroy (ARSearch_eval_mode_instance);
		
		//Verificar porque se estan quedando instanciadas las interfaces para evitar hacer estos dos llamados:
		DestroyInstancesWithTag ("MenuPhasesEvalMode");
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhaseOneEval");
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhaseTwoEval");
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE1_STEP3_EV;
		
		ARSearch_eval_mode_instance = Instantiate (ARModeEvaluation);
		ControllerBlinkARModeEvaluation controller_blinking_second = ARSearch_eval_mode_instance.GetComponent<ControllerBlinkARModeEvaluation> ();
		
		//Es iportante asignar esta variable para poder controlar los marcadores
		interfaceInstanceActive = ARSearch_eval_mode_instance;
		
		//localizando el marcador que se debe buscar en esta fase del proceso y cargando datos respectivos:
		markerInScene = GameObject.Find ("FrameMarker24");
		ControllerAddInfoInMarker controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		controller_info_marker.image_real_help_path = img_help_marker24_25_eval_step3;

		//asignando el texto que se debe mostrar al momento del feedback:
		controller_blinking_second.feedback_info_text_path = feedback_marker24_25_eval_mode_step3;
		
		//definiendo los ordenes que controlara esta interfaz:
		order_in_process = new int[1];
		order_in_process [0] = 6;
		controller_blinking_second.ordenes = order_in_process;
		
		//El llamado al siguiente metodo carga la informacion correspondiente en la interfaz:
		//NOTA: antes de hacer esto es importante que la variable interfaceInstanceActive este asignada:
		//tambien es importante haber asignado la variable inTutorialPhase1
		controller_info_marker.LoadInformationForEvaluationMode ();
		
		controller_blinking_second.interface_going_from = "Phase1Step3Eval";
		controller_blinking_second.onClickSelectBtn += OnClickSelectPapelAbsorbP1S3Eval;
		
		//asignando el texto que se debe mostrar de guia:
		controller_blinking_second.texto_guia_producto = "Busca el elemento 2";
		
		//inicializando la variable que controla si estamos en el modo de evaluacion en RA:
		in_Evaluation_mode = true;
		
		//colocando en false la informacion adicional por si se le habia dado atras en algun momento en la interfaz:
		info_additional_displayed = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:IE20");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "IE20", "2", "-1","eval");
	} //cierra 

	public void OnClickSelectPapelAbsorbP1S3Eval(string interface_from){
		Debug.Log ("Llamado al metodo OnClickSelectPapelAbsorbP1S3Eval!!");
		info_additional_displayed = false;
		in_Evaluation_mode = false;
		//notificando que ya se ha completado la parte de productos y herramientas
		steps_p_one_eval_completed [2].activity_tools_and_products = true;
		steps_p_one_eval_completed [2].CheckStepCompletion ();

		SaveDataForStudent ();
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:SE6");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "SE6", "2", "-1","eval");
		if (steps_p_one_eval_completed [2].step_completed)
			GoToMenuStepsPhase1Eval ();
		else 
			GoToActivitiesPhase1Step3EvalMode ();

	}//cierra OnClickSelectPapelAbsorbP1S3Eval

	public void GoToSearchEsponjaP1S4Eval(){
		Debug.Log ("Entra al metodo GoToSearchEsponjaP1S4Eval... interfaz: "  + current_interface);
		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_EVAL) {
			Destroy (ToolsAndProductsPhase1Step4_int_eval_instance);
		} else if (current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP4_EV)
			Destroy (ARSearch_eval_mode_instance);
		
		//Verificar porque se estan quedando instanciadas las interfaces para evitar hacer estos dos llamados:
		DestroyInstancesWithTag ("MenuPhasesEvalMode");
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhaseOneEval");
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhaseTwoEval");
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE1_STEP4_EV;
		
		ARSearch_eval_mode_instance = Instantiate (ARModeEvaluation);
		ControllerBlinkARModeEvaluation controller_blinking = ARSearch_eval_mode_instance.GetComponent<ControllerBlinkARModeEvaluation> ();
		
		//Es iportante asignar esta variable para poder controlar los marcadores
		interfaceInstanceActive = ARSearch_eval_mode_instance;
		
		//localizando el marcador que se debe buscar en esta fase del proceso y cargando datos respectivos:
		markerInScene = GameObject.Find ("FrameMarker45");
		ControllerAddInfoInMarker controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
				
		//asignando el texto que se debe mostrar al momento del feedback:
		controller_blinking.feedback_info_text_path = feedback_marker45_46_eval_mode_step4;
		
		//definiendo los ordenes que controlara esta interfaz:
		order_in_process = new int[1];
		order_in_process [0] = 7;
		controller_blinking.ordenes = order_in_process;
		
		//El llamado al siguiente metodo carga la informacion correspondiente en la interfaz:
		//NOTA: antes de hacer esto es importante que la variable interfaceInstanceActive este asignada:
		//tambien es importante haber asignado la variable inTutorialPhase1
		controller_info_marker.LoadInformationForEvaluationMode ();
		
		controller_blinking.interface_going_from = "Phase1Step4Eval";
		controller_blinking.onClickSelectBtn += OnClickSelectAbrasivoP1S4Eval;
		
		//asignando el texto que se debe mostrar de guia:
		controller_blinking.texto_guia_producto = "Busca el producto 1";
		
		//inicializando la variable que controla si estamos en el modo de evaluacion en RA:
		in_Evaluation_mode = true;
		
		//colocando en false la informacion adicional por si se le habia dado atras en algun momento en la interfaz:
		info_additional_displayed = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:IE21");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "IE21", "3", "-1","eval");
	} //cierra GoToSearchEsponjaP1S4Eval

	public void OnClickSelectAbrasivoP1S4Eval(string interface_from){
		Debug.Log ("Llamado al metodo OnClickSelectAbrasivoP1S4Eval!!");
		info_additional_displayed = false;
		in_Evaluation_mode = false;
		//notificando que ya se ha completado la parte de productos y herramientas
		steps_p_one_eval_completed [3].activity_tools_and_products = true;
		steps_p_one_eval_completed [3].CheckStepCompletion ();

		SaveDataForStudent ();
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:SE7");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "SE7", "3", "-1","eval");
		
		if (steps_p_one_eval_completed [3].step_completed)
			GoToMenuStepsPhase1Eval ();
		else 
			GoToActivitiesPhase1Step4EvalMode ();
	}//cierra OnClickSelectAbrasivoP1S4Eval


	public void GoToSearchMartilloP1S5Eval(){
		Debug.Log ("Entra al metodo GoToSearchMartilloP1S5Eval... interfaz: "  + current_interface);
		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_EVAL) {
			Destroy (ToolsAndProductsPhase1Step5_int_eval_instance);
		} else if (current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP5_EV)
			Destroy (ARSearch_eval_mode_instance);
		
		//Verificar porque se estan quedando instanciadas las interfaces para evitar hacer estos dos llamados:
		DestroyInstancesWithTag ("MenuPhasesEvalMode");
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhaseOneEval");
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhaseTwoEval");
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE1_STEP5_EV;
		
		ARSearch_eval_mode_instance = Instantiate (ARModeEvaluation);
		ControllerBlinkARModeEvaluation controller_blinking = ARSearch_eval_mode_instance.GetComponent<ControllerBlinkARModeEvaluation> ();
		
		//Es iportante asignar esta variable para poder controlar los marcadores
		interfaceInstanceActive = ARSearch_eval_mode_instance;
		
		//localizando el marcador que se debe buscar en esta fase del proceso y cargando datos respectivos:
		markerInScene = GameObject.Find ("FrameMarker100");
		ControllerAddInfoInMarker controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		
		//asignando el texto que se debe mostrar al momento del feedback:
		controller_blinking.feedback_info_text_path = feedback_marker100_eval_mode_step5;
		
		//definiendo los ordenes que controlara esta interfaz:
		order_in_process = new int[1];
		order_in_process [0] = 8;
		controller_blinking.ordenes = order_in_process;
		
		//El llamado al siguiente metodo carga la informacion correspondiente en la interfaz:
		//NOTA: antes de hacer esto es importante que la variable interfaceInstanceActive este asignada:
		//tambien es importante haber asignado la variable inTutorialPhase1
		controller_info_marker.LoadInformationForEvaluationMode ();
		
		controller_blinking.interface_going_from = "Phase1Step5Eval";
		controller_blinking.onClickSelectBtn += OnClickSelectMartilloP1S5Eval;
		
		//asignando el texto que se debe mostrar de guia:
		controller_blinking.texto_guia_producto = "Busca la herramienta";
		
		//inicializando la variable que controla si estamos en el modo de evaluacion en RA:
		in_Evaluation_mode = true;
		
		//colocando en false la informacion adicional por si se le habia dado atras en algun momento en la interfaz:
		info_additional_displayed = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:IE22");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "IE22", "4", "-1","eval");
	} //cierra GoToSearchMartilloP1S5Eval

	public void OnClickSelectMartilloP1S5Eval(string interface_from){
		Debug.Log ("Llamado al metodo OnClickSelectAbrasivoP1S4Eval!!");
		info_additional_displayed = false;
		in_Evaluation_mode = false;
		//notificando que ya se ha completado la parte de productos y herramientas
		steps_p_one_eval_completed [4].activity_tools_and_products = true;
		steps_p_one_eval_completed [4].CheckStepCompletion ();

		SaveDataForStudent ();
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:SE8");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "SE8", "4", "-1","eval");
		if (steps_p_one_eval_completed [4].step_completed)
			GoToMenuStepsPhase1Eval ();
		else 
			GoToActivitiesPhase1Step5EvalMode ();
	}// cierra OnClickSelectMartilloP1S5Eval

	public void GoToSearchDesengrasanteP1S6Eval(){
		Debug.Log ("Entra al metodo GoToSearchDesengrasanteP1S6Eval... interfaz: "  + current_interface);
		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_EVAL) {
			Destroy (ToolsAndProductsPhase1Step6_int_eval_instance);
		} else if (current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP6_EV)
			Destroy (ARSearch_eval_mode_instance);
		
		//Verificar porque se estan quedando instanciadas las interfaces para evitar hacer estos dos llamados:
		DestroyInstancesWithTag ("MenuPhasesEvalMode");
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhaseOneEval");
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhaseTwoEval");
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE1_STEP6_EV;
		
		ARSearch_eval_mode_instance = Instantiate (ARModeEvaluation);
		ControllerBlinkARModeEvaluation controller_blinking = ARSearch_eval_mode_instance.GetComponent<ControllerBlinkARModeEvaluation> ();
		
		//Es iportante asignar esta variable para poder controlar los marcadores
		interfaceInstanceActive = ARSearch_eval_mode_instance;
		
		//localizando el marcador que se debe buscar en esta fase del proceso y cargando datos respectivos:
		markerInScene = GameObject.Find ("FrameMarker26");
		ControllerAddInfoInMarker controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		
		//asignando el texto que se debe mostrar al momento del feedback:
		controller_blinking.feedback_info_text_path = feedback_marker26_eval_mode_step6;
		
		//definiendo los ordenes que controlara esta interfaz:
		order_in_process = new int[1];
		order_in_process [0] = 9;
		controller_blinking.ordenes = order_in_process;
		
		//El llamado al siguiente metodo carga la informacion correspondiente en la interfaz:
		//NOTA: antes de hacer esto es importante que la variable interfaceInstanceActive este asignada:
		//tambien es importante haber asignado la variable inTutorialPhase1
		controller_info_marker.LoadInformationForEvaluationMode ();
		
		controller_blinking.interface_going_from = "Phase1Step6Eval";
		controller_blinking.onClickSelectBtn += OnClickSelectDesengrasanteP1S6Eval;
		
		//asignando el texto que se debe mostrar de guia:
		controller_blinking.texto_guia_producto = "Busca el producto 1";
		
		//inicializando la variable que controla si estamos en el modo de evaluacion en RA:
		in_Evaluation_mode = true;
		
		//colocando en false la informacion adicional por si se le habia dado atras en algun momento en la interfaz:
		info_additional_displayed = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:IE23");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "IE23", "5", "-1","eval");
	}//cierra GoToSearchDesengrasanteP1S6Eval

	public void OnClickSelectDesengrasanteP1S6Eval(string interface_from){
		Debug.Log ("Llamado al metodo OnClickSelectDesengrasanteP1S6Eval!!");
		info_additional_displayed = false;
		in_Evaluation_mode = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:SE9");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "SE9", "5", "-1","eval");
		GoToSearchPapelAbsP1S6Eval ();
	}//cierra OnClickSelectDesengrasanteP1S6Eval

	public void GoToSearchPapelAbsP1S6Eval(){
		Debug.Log ("Entra al metodo GoToSearchPapelAbsP1S6Eval... interfaz: "  + current_interface);
		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_EVAL) {
			Destroy (ToolsAndProductsPhase1Step6_int_eval_instance);
		} else if (current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP6_EV)
			Destroy (ARSearch_eval_mode_instance);
		
		//Verificar porque se estan quedando instanciadas las interfaces para evitar hacer estos dos llamados:
		DestroyInstancesWithTag ("MenuPhasesEvalMode");
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhaseOneEval");
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhaseTwoEval");
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE1_STEP6_EV;
		
		ARSearch_eval_mode_instance = Instantiate (ARModeEvaluation);
		ControllerBlinkARModeEvaluation controller_blinking = ARSearch_eval_mode_instance.GetComponent<ControllerBlinkARModeEvaluation> ();
		
		//Es iportante asignar esta variable para poder controlar los marcadores
		interfaceInstanceActive = ARSearch_eval_mode_instance;
		
		//localizando el marcador que se debe buscar en esta fase del proceso y cargando datos respectivos:
		markerInScene = GameObject.Find ("FrameMarker25");
		ControllerAddInfoInMarker controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();


		//asignando el texto que se debe mostrar al momento del feedback:
		controller_blinking.feedback_info_text_path = feedback_marker25_eval_mode_step6;
		
		//definiendo los ordenes que controlara esta interfaz:
		order_in_process = new int[1];
		order_in_process [0] = 10;
		controller_blinking.ordenes = order_in_process;
		
		//El llamado al siguiente metodo carga la informacion correspondiente en la interfaz:
		//NOTA: antes de hacer esto es importante que la variable interfaceInstanceActive este asignada:
		//tambien es importante haber asignado la variable inTutorialPhase1
		controller_info_marker.LoadInformationForEvaluationMode ();
		
		controller_blinking.interface_going_from = "Phase1Step6Eval";
		controller_blinking.onClickSelectBtn += OnClickSelectPapelAbsP1S6Eval;
		
		//asignando el texto que se debe mostrar de guia:
		controller_blinking.texto_guia_producto = "Busca el producto 2";
		
		//inicializando la variable que controla si estamos en el modo de evaluacion en RA:
		in_Evaluation_mode = true;
		
		//colocando en false la informacion adicional por si se le habia dado atras en algun momento en la interfaz:
		info_additional_displayed = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:IE24");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "IE24", "5", "-1","eval");
	}// GoToSearchPapelAbsP1S6Eval

	public void OnClickSelectPapelAbsP1S6Eval(string interface_from){
		Debug.Log ("Llamado al metodo OnClickSelectPapelAbsP1S6Eval!!");
		info_additional_displayed = false;
		in_Evaluation_mode = false;
		//notificando que ya se ha completado la parte de productos y herramientas
		steps_p_one_eval_completed [5].activity_tools_and_products = true;
		steps_p_one_eval_completed [5].CheckStepCompletion ();

		if (steps_p_one_eval_completed [5].step_completed)
			phase_two_enable_eval_mode = true;
		
		SaveDataForStudent ();
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:SE10");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "SE10", "5", "-1","eval");
		if (steps_p_one_eval_completed [5].step_completed)
			GoToMenuStepsPhase1Eval ();
		else 
			GoToActivitiesPhase1Step6EvalMode ();
	}//cierra OnClickSelectPapelAbsP1S6Eval

	//***************************************************************************************************
	//Metodos para configurar la busqueda para la FASE 2:

	public void GoToSearchCintaEnmasP2S2Eval(){
		Debug.Log ("Entra al metodo GoToSearchCintaEnmasP2S2Eval... interfaz: "  + current_interface);
		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_EVAL) {
			Destroy (ToolsAndProductsPhase2Step2_int_eval_instance);
		} else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP2_EV)
			Destroy (ARSearch_eval_mode_instance);
		
		//Verificar porque se estan quedando instanciadas las interfaces para evitar hacer estos dos llamados:
		DestroyInstancesWithTag ("MenuPhasesEvalMode");
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhaseOneEval");
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhaseTwoEval");
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE2_STEP2_EV;
		
		ARSearch_eval_mode_instance = Instantiate (ARModeEvaluation);
		ControllerBlinkARModeEvaluation controller_blinking = ARSearch_eval_mode_instance.GetComponent<ControllerBlinkARModeEvaluation> ();
		
		//Es iportante asignar esta variable para poder controlar los marcadores
		interfaceInstanceActive = ARSearch_eval_mode_instance;
		
		//localizando el marcador que se debe buscar en esta fase del proceso y cargando datos respectivos:
		markerInScene = GameObject.Find ("FrameMarker65");
		ControllerAddInfoInMarker controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		
		//asignando el texto que se debe mostrar al momento del feedback:
		controller_blinking.feedback_info_text_path = feedback_marker65_eval_mode_step2;
		
		//definiendo los ordenes que controlara esta interfaz:
		order_in_process = new int[1];
		order_in_process [0] = 11;
		controller_blinking.ordenes = order_in_process;
		
		//El llamado al siguiente metodo carga la informacion correspondiente en la interfaz:
		//NOTA: antes de hacer esto es importante que la variable interfaceInstanceActive este asignada:
		//tambien es importante haber asignado la variable inTutorialPhase1
		controller_info_marker.LoadInformationForEvaluationMode ();
		
		controller_blinking.interface_going_from = "Phase2Step2Eval";
		controller_blinking.onClickSelectBtn += OnClickSelectCintaEnmP2S2Eval;
		
		//asignando el texto que se debe mostrar de guia:
		controller_blinking.texto_guia_producto = "Busca el producto 1";
		
		//inicializando la variable que controla si estamos en el modo de evaluacion en RA:
		in_Evaluation_mode = true;
		
		//colocando en false la informacion adicional por si se le habia dado atras en algun momento en la interfaz:
		info_additional_displayed = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:IE40");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "IE40", "7", "-1","eval");
	}//cierra GoToSearchCintaEnmasP2S2Eval

	public void OnClickSelectCintaEnmP2S2Eval(string interface_from){
		Debug.Log ("Llamado al metodo OnClickSelectCintaEnmP2S2Eval!!");
		info_additional_displayed = false;
		in_Evaluation_mode = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:SE11");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "SE11", "7", "-1","eval");
		GoToSearchPapelEnmascararP2S2Eval ();
	}//cierra OnClickSelectCintaEnmP2S2Eval

	public void GoToSearchPapelEnmascararP2S2Eval(){
		Debug.Log ("Entra al metodo GoToSearchCintaEnmasP2S2Eval... interfaz: "  + current_interface);
		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_EVAL) {
			Destroy (ToolsAndProductsPhase2Step2_int_eval_instance);
		} else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP2_EV)
			Destroy (ARSearch_eval_mode_instance);
		
		//Verificar porque se estan quedando instanciadas las interfaces para evitar hacer estos dos llamados:
		DestroyInstancesWithTag ("MenuPhasesEvalMode");
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhaseOneEval");
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhaseTwoEval");
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE2_STEP2_EV;
		
		ARSearch_eval_mode_instance = Instantiate (ARModeEvaluation);
		ControllerBlinkARModeEvaluation controller_blinking_second = ARSearch_eval_mode_instance.GetComponent<ControllerBlinkARModeEvaluation> ();
		
		//Es iportante asignar esta variable para poder controlar los marcadores
		interfaceInstanceActive = ARSearch_eval_mode_instance;
		
		//localizando el marcador que se debe buscar en esta fase del proceso y cargando datos respectivos:
		markerInScene = GameObject.Find ("FrameMarker69");
		ControllerAddInfoInMarker controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		
		//asignando el texto que se debe mostrar al momento del feedback:
		controller_blinking_second.feedback_info_text_path = feedback_marker69_eval_mode_step2;
		
		//definiendo los ordenes que controlara esta interfaz:
		order_in_process = new int[1];
		order_in_process [0] = 12;
		controller_blinking_second.ordenes = order_in_process;
		
		//El llamado al siguiente metodo carga la informacion correspondiente en la interfaz:
		//NOTA: antes de hacer esto es importante que la variable interfaceInstanceActive este asignada:
		//tambien es importante haber asignado la variable inTutorialPhase1
		controller_info_marker.LoadInformationForEvaluationMode ();
		
		controller_blinking_second.interface_going_from = "Phase2Step2Eval";
		controller_blinking_second.onClickSelectBtn += OnClickSelectPapelEnmascP2S2Eval;
		
		//asignando el texto que se debe mostrar de guia:
		controller_blinking_second.texto_guia_producto = "Busca el producto 2";
		
		//inicializando la variable que controla si estamos en el modo de evaluacion en RA:
		in_Evaluation_mode = true;
		
		//colocando en false la informacion adicional por si se le habia dado atras en algun momento en la interfaz:
		info_additional_displayed = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:IE41");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "IE41", "7", "-1","eval");
	}//cierra GoToSearchPapelEnmascararP2S2Eval

	public void OnClickSelectPapelEnmascP2S2Eval(string interface_from){
		Debug.Log ("Llamado al metodo OnClickSelectPapelEnmascP2S2Eval!!");
		info_additional_displayed = false;
		in_Evaluation_mode = false;
		//notificando que ya se ha completado la parte de productos y herramientas
		steps_p_two_eval_completed [1].activity_tools_and_products = true;
		steps_p_two_eval_completed [1].CheckStepCompletion ();
		
		SaveDataForStudent ();
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:SE12");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "SE12", "7", "-1","eval");
		if (steps_p_two_eval_completed [1].step_completed)
			GoToMenuStepsPhase2Eval ();
		else 
			GoToActivitiesPhase2Step2EvalMode ();
	}//cierra OnClickSelectPapelEnmascP2S2Eval


	public void GoToSearchEsponjaP320P2S3Eval(){
		Debug.Log ("Entra al metodo GoToSearchEsponjaP320P2S3Eval... interfaz: "  + current_interface);
		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_EVAL) {
			Destroy (ToolsAndProductsPhase2Step3_int_eval_instance);
		} else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP3_EV)
			Destroy (ARSearch_eval_mode_instance);
		
		//Verificar porque se estan quedando instanciadas las interfaces para evitar hacer estos dos llamados:
		DestroyInstancesWithTag ("MenuPhasesEvalMode");
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhaseOneEval");
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhaseTwoEval");
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE2_STEP3_EV;
		
		ARSearch_eval_mode_instance = Instantiate (ARModeEvaluation);
		ControllerBlinkARModeEvaluation controller_blinking = ARSearch_eval_mode_instance.GetComponent<ControllerBlinkARModeEvaluation> ();
		
		//Es iportante asignar esta variable para poder controlar los marcadores
		interfaceInstanceActive = ARSearch_eval_mode_instance;
		
		//localizando el marcador que se debe buscar en esta fase del proceso y cargando datos respectivos:
		markerInScene = GameObject.Find ("FrameMarker45");
		ControllerAddInfoInMarker controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		
		//asignando el texto que se debe mostrar al momento del feedback:
		controller_blinking.feedback_info_text_path = feedback_marker45_eval_mode_step3;
		
		//definiendo los ordenes que controlara esta interfaz:
		order_in_process = new int[1];
		order_in_process [0] = 13;
		controller_blinking.ordenes = order_in_process;
		
		//El llamado al siguiente metodo carga la informacion correspondiente en la interfaz:
		//NOTA: antes de hacer esto es importante que la variable interfaceInstanceActive este asignada:
		//tambien es importante haber asignado la variable inTutorialPhase1
		controller_info_marker.LoadInformationForEvaluationMode ();
		
		controller_blinking.interface_going_from = "Phase2Step3Eval";
		controller_blinking.onClickSelectBtn += OnClickSelectLijaP320P2S3Eval;
		
		//asignando el texto que se debe mostrar de guia:
		controller_blinking.texto_guia_producto = "Busca el producto 1";
		
		//inicializando la variable que controla si estamos en el modo de evaluacion en RA:
		in_Evaluation_mode = true;
		
		//colocando en false la informacion adicional por si se le habia dado atras en algun momento en la interfaz:
		info_additional_displayed = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:IE42");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "IE42", "9", "-1","eval");
	}// cierra GoToSearchEsponjaP320P2S3Eval

	public void OnClickSelectLijaP320P2S3Eval(string interface_from){
		Debug.Log ("Llamado al metodo OnClickSelectLijaP320P2S3Eval!!");
		info_additional_displayed = false;
		in_Evaluation_mode = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:SE13");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "SE13", "9", "-1","eval");
		GoToSearchElementoLimpiezaP2S3Eval ();
	} //cierra OnClickSelectLijaP320P2S3Eval

	public void GoToSearchElementoLimpiezaP2S3Eval(){
		Debug.Log ("Entra al metodo GoToSearchElementoLimpiezaP2S3Eval... interfaz: "  + current_interface);
		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_EVAL) {
			Destroy (ToolsAndProductsPhase2Step3_int_eval_instance);
		} else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP3_EV)
			Destroy (ARSearch_eval_mode_instance);
		
		//Verificar porque se estan quedando instanciadas las interfaces para evitar hacer estos dos llamados:
		DestroyInstancesWithTag ("MenuPhasesEvalMode");
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhaseOneEval");
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhaseTwoEval");
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE2_STEP3_EV;
		
		ARSearch_eval_mode_instance = Instantiate (ARModeEvaluation);
		ControllerBlinkARModeEvaluation controller_blinking_second = ARSearch_eval_mode_instance.GetComponent<ControllerBlinkARModeEvaluation> ();
		
		//Es iportante asignar esta variable para poder controlar los marcadores
		interfaceInstanceActive = ARSearch_eval_mode_instance;
		
		//localizando el marcador que se debe buscar en esta fase del proceso y cargando datos respectivos:
		markerInScene = GameObject.Find ("FrameMarker25");
		ControllerAddInfoInMarker controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		controller_info_marker.image_real_help_path = img_eval_mode_hint_marker25_24_23;

		//asignando el texto que se debe mostrar al momento del feedback:
		controller_blinking_second.feedback_info_text_path = feedback_marker25_24_23_eval_mode_step3;
		
		//definiendo los ordenes que controlara esta interfaz:
		order_in_process = new int[1];
		order_in_process [0] = 14;
		controller_blinking_second.ordenes = order_in_process;
		
		//El llamado al siguiente metodo carga la informacion correspondiente en la interfaz:
		//NOTA: antes de hacer esto es importante que la variable interfaceInstanceActive este asignada:
		//tambien es importante haber asignado la variable inTutorialPhase1
		controller_info_marker.LoadInformationForEvaluationMode ();
		
		controller_blinking_second.interface_going_from = "Phase2Step3Eval";
		controller_blinking_second.onClickSelectBtn += OnClickSelectElemLimpiezaP2S3Eval;
		
		//asignando el texto que se debe mostrar de guia:
		controller_blinking_second.texto_guia_producto = "Busca el elemento 2";
		
		//inicializando la variable que controla si estamos en el modo de evaluacion en RA:
		in_Evaluation_mode = true;
		
		//colocando en false la informacion adicional por si se le habia dado atras en algun momento en la interfaz:
		info_additional_displayed = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:IE43");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "IE43", "9", "-1","eval");
	}//cierra GoToSearchElementoLimpiezaP2S3Eval

	public void OnClickSelectElemLimpiezaP2S3Eval(string interface_from){
		Debug.Log ("Llamado al metodo OnClickSelectElemLimpiezaP2S3Eval!!");
		info_additional_displayed = false;
		in_Evaluation_mode = false;
		//notificando que ya se ha completado la parte de productos y herramientas
		steps_p_two_eval_completed [2].activity_tools_and_products = true;
		steps_p_two_eval_completed [2].CheckStepCompletion ();
		
		SaveDataForStudent ();
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:SE14");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "SE14", "9", "-1","eval");
		if (steps_p_two_eval_completed [2].step_completed)
			GoToMenuStepsPhase2Eval ();
		else 
			GoToActivitiesPhase2Step3EvalMode ();
	} //cierra OnClickSelectElemLimpiezaP2S3Eval


	public void GoToSearchEsponjaP400P2S4Eval(){
		Debug.Log ("Entra al metodo GoToSearchEsponjaP400P2S4Eval... interfaz: "  + current_interface);
		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_EVAL) {
			Destroy (ToolsAndProductsPhase2Step4_int_eval_instance);
		} else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP4_EV)
			Destroy (ARSearch_eval_mode_instance);
		
		//Verificar porque se estan quedando instanciadas las interfaces para evitar hacer estos dos llamados:
		DestroyInstancesWithTag ("MenuPhasesEvalMode");
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhaseOneEval");
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhaseTwoEval");
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE2_STEP4_EV;
		
		ARSearch_eval_mode_instance = Instantiate (ARModeEvaluation);
		ControllerBlinkARModeEvaluation controller_blinking = ARSearch_eval_mode_instance.GetComponent<ControllerBlinkARModeEvaluation> ();
		
		//Es iportante asignar esta variable para poder controlar los marcadores
		interfaceInstanceActive = ARSearch_eval_mode_instance;
		
		//localizando el marcador que se debe buscar en esta fase del proceso y cargando datos respectivos:
		markerInScene = GameObject.Find ("FrameMarker46");
		ControllerAddInfoInMarker controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
				
		//asignando el texto que se debe mostrar al momento del feedback:
		controller_blinking.feedback_info_text_path = feedback_marker46_eval_mode_step4;
		
		//definiendo los ordenes que controlara esta interfaz:
		order_in_process = new int[1];
		order_in_process [0] = 15;
		controller_blinking.ordenes = order_in_process;
		
		//El llamado al siguiente metodo carga la informacion correspondiente en la interfaz:
		//NOTA: antes de hacer esto es importante que la variable interfaceInstanceActive este asignada:
		//tambien es importante haber asignado la variable inTutorialPhase1
		controller_info_marker.LoadInformationForEvaluationMode ();
		
		controller_blinking.interface_going_from = "Phase2Step4Eval";
		controller_blinking.onClickSelectBtn += OnClickSelectLijaP400P2S4Eval;
		
		//asignando el texto que se debe mostrar de guia:
		controller_blinking.texto_guia_producto = "Busca el producto 1";
		
		//inicializando la variable que controla si estamos en el modo de evaluacion en RA:
		in_Evaluation_mode = true;
		
		//colocando en false la informacion adicional por si se le habia dado atras en algun momento en la interfaz:
		info_additional_displayed = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:IE44");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "IE44", "10", "-1","eval");
	}//cierra GoToSearchEsponjaP400P2S4Eval

	public void OnClickSelectLijaP400P2S4Eval(string interface_from){
		Debug.Log ("Llamado al metodo OnClickSelectLijaP400P2S4Eval!!");
		info_additional_displayed = false;
		in_Evaluation_mode = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:SE15");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "SE15", "10", "-1","eval");
		GoToSearchElemenLimpiezaP2S4Eval ();
	}//cierra OnClickSelectLijaP400P2S4Eval

	public void GoToSearchElemenLimpiezaP2S4Eval(){
		Debug.Log ("Entra al metodo GoToSearchElemenLimpiezaP2S4Eval... interfaz: "  + current_interface);
		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_EVAL) {
			Destroy (ToolsAndProductsPhase2Step4_int_eval_instance);
		} else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP4_EV)
			Destroy (ARSearch_eval_mode_instance);
		
		//Verificar porque se estan quedando instanciadas las interfaces para evitar hacer estos dos llamados:
		DestroyInstancesWithTag ("MenuPhasesEvalMode");
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhaseOneEval");
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhaseTwoEval");
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE2_STEP4_EV;
		
		ARSearch_eval_mode_instance = Instantiate (ARModeEvaluation);
		ControllerBlinkARModeEvaluation controller_blinking_second = ARSearch_eval_mode_instance.GetComponent<ControllerBlinkARModeEvaluation> ();
		
		//Es iportante asignar esta variable para poder controlar los marcadores
		interfaceInstanceActive = ARSearch_eval_mode_instance;
		
		//localizando el marcador que se debe buscar en esta fase del proceso y cargando datos respectivos:
		markerInScene = GameObject.Find ("FrameMarker25");
		ControllerAddInfoInMarker controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		controller_info_marker.image_real_help_path = img_eval_mode_hint_marker25_24_23;

		//asignando el texto que se debe mostrar al momento del feedback:
		controller_blinking_second.feedback_info_text_path = feedback_marker25_24_23_eval_mode_step4;
		
		//definiendo los ordenes que controlara esta interfaz:
		order_in_process = new int[1];
		order_in_process [0] = 16;
		controller_blinking_second.ordenes = order_in_process;
		
		//El llamado al siguiente metodo carga la informacion correspondiente en la interfaz:
		//NOTA: antes de hacer esto es importante que la variable interfaceInstanceActive este asignada:
		//tambien es importante haber asignado la variable inTutorialPhase1
		controller_info_marker.LoadInformationForEvaluationMode ();
		
		controller_blinking_second.interface_going_from = "Phase2Step4Eval";
		controller_blinking_second.onClickSelectBtn += OnClickSelectElemenLimpiezaP2S4;
		
		//asignando el texto que se debe mostrar de guia:
		controller_blinking_second.texto_guia_producto = "Busca el producto 2";
		
		//inicializando la variable que controla si estamos en el modo de evaluacion en RA:
		in_Evaluation_mode = true;
		
		//colocando en false la informacion adicional por si se le habia dado atras en algun momento en la interfaz:
		info_additional_displayed = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:IE45");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "IE45", "10", "-1","eval");
	} //cierra GoToSearchElemenLimpiezaP2S4Eval

	public void OnClickSelectElemenLimpiezaP2S4(string interface_from){
		Debug.Log ("Llamado al metodo OnClickSelectElemenLimpiezaP2S4!!");
		info_additional_displayed = false;
		in_Evaluation_mode = false;
		//notificando que ya se ha completado la parte de productos y herramientas
		steps_p_two_eval_completed [3].activity_tools_and_products = true;
		steps_p_two_eval_completed [3].CheckStepCompletion ();
		
		SaveDataForStudent ();
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:SE16");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "SE16", "10", "-1","eval");
		
		if (steps_p_two_eval_completed [3].step_completed)
			GoToMenuStepsPhase2Eval ();
		else 
			GoToActivitiesPhase2Step4EvalMode ();
	} //cierra OnClickSelectElemenLimpiezaP2S4

	public void GoToSearchRotoOrbitalP2S5Eval(){
		Debug.Log ("Entra al metodo GoToSearchRotoOrbitalP2S5Eval... interfaz: "  + current_interface);
		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_EVAL) {
			Destroy (ToolsAndProductsPhase2Step5_int_eval_instance);
		} else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP5_EV)
			Destroy (ARSearch_eval_mode_instance);
		
		//Verificar porque se estan quedando instanciadas las interfaces para evitar hacer estos dos llamados:
		DestroyInstancesWithTag ("MenuPhasesEvalMode");
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhaseOneEval");
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhaseTwoEval");
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE2_STEP5_EV;
		
		ARSearch_eval_mode_instance = Instantiate (ARModeEvaluation);
		ControllerBlinkARModeEvaluation controller_blinking = ARSearch_eval_mode_instance.GetComponent<ControllerBlinkARModeEvaluation> ();
		
		//Es iportante asignar esta variable para poder controlar los marcadores
		interfaceInstanceActive = ARSearch_eval_mode_instance;
		
		//localizando el marcador que se debe buscar en esta fase del proceso y cargando datos respectivos:
		markerInScene = GameObject.Find ("FrameMarker99");
		ControllerAddInfoInMarker controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
				
		//asignando el texto que se debe mostrar al momento del feedback:
		controller_blinking.feedback_info_text_path = feedback_marker99_eval_mode_step5;
		
		//definiendo los ordenes que controlara esta interfaz:
		order_in_process = new int[1];
		order_in_process [0] = 17;
		controller_blinking.ordenes = order_in_process;
		
		//El llamado al siguiente metodo carga la informacion correspondiente en la interfaz:
		//NOTA: antes de hacer esto es importante que la variable interfaceInstanceActive este asignada:
		//tambien es importante haber asignado la variable inTutorialPhase1
		controller_info_marker.LoadInformationForEvaluationMode ();
		
		controller_blinking.interface_going_from = "Phase2Step5Eval";
		controller_blinking.onClickSelectBtn += OnClickSelectRotoOrbitalP2S5;
		
		//asignando el texto que se debe mostrar de guia:
		controller_blinking.texto_guia_producto = "Busca la herramienta 1:";
		
		//inicializando la variable que controla si estamos en el modo de evaluacion en RA:
		in_Evaluation_mode = true;
		
		//colocando en false la informacion adicional por si se le habia dado atras en algun momento en la interfaz:
		info_additional_displayed = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:IE46");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "IE46", "11", "-1","eval");
	}//cierra GoToSearchRotoOrbitalP2S5Eval

	public void OnClickSelectRotoOrbitalP2S5(string interface_from){
		Debug.Log ("Llamado al metodo OnClickSelectRotoOrbitalP2S5!!");
		info_additional_displayed = false;
		in_Evaluation_mode = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:SE17");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "SE17", "11", "-1","eval");
		GoToSearchDiscoP80P2S5Eval ();
	}//cierra OnClickSelectRotoOrbitalP2S5

	public void GoToSearchDiscoP80P2S5Eval(){
		Debug.Log ("Entra al metodo GoToSearchDiscoP80P2S5Eval... interfaz: "  + current_interface);
		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_EVAL) {
			Destroy (ToolsAndProductsPhase2Step5_int_eval_instance);
		} else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP5_EV)
			Destroy (ARSearch_eval_mode_instance);
		
		//Verificar porque se estan quedando instanciadas las interfaces para evitar hacer estos dos llamados:
		DestroyInstancesWithTag ("MenuPhasesEvalMode");
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhaseOneEval");
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhaseTwoEval");
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE2_STEP5_EV;
		
		ARSearch_eval_mode_instance = Instantiate (ARModeEvaluation);
		ControllerBlinkARModeEvaluation controller_blinking_second = ARSearch_eval_mode_instance.GetComponent<ControllerBlinkARModeEvaluation> ();
		
		//Es iportante asignar esta variable para poder controlar los marcadores
		interfaceInstanceActive = ARSearch_eval_mode_instance;
		
		//localizando el marcador que se debe buscar en esta fase del proceso y cargando datos respectivos:
		markerInScene = GameObject.Find ("FrameMarker30");
		ControllerAddInfoInMarker controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		
		//asignando el texto que se debe mostrar al momento del feedback:
		controller_blinking_second.feedback_info_text_path = feedback_marker30_eval_mode_step5;
		
		//definiendo los ordenes que controlara esta interfaz:
		order_in_process = new int[1];
		order_in_process [0] = 18;
		controller_blinking_second.ordenes = order_in_process;
		
		//El llamado al siguiente metodo carga la informacion correspondiente en la interfaz:
		//NOTA: antes de hacer esto es importante que la variable interfaceInstanceActive este asignada:
		//tambien es importante haber asignado la variable inTutorialPhase1
		controller_info_marker.LoadInformationForEvaluationMode ();
		
		controller_blinking_second.interface_going_from = "Phase2Step5Eval";
		controller_blinking_second.onClickSelectBtn += OnClickSelectDiscop80P2S5;
		
		//asignando el texto que se debe mostrar de guia:
		controller_blinking_second.texto_guia_producto = "Busca el producto 2:";
		
		//inicializando la variable que controla si estamos en el modo de evaluacion en RA:
		in_Evaluation_mode = true;
		
		//colocando en false la informacion adicional por si se le habia dado atras en algun momento en la interfaz:
		info_additional_displayed = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:IE47");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "IE47", "11", "-1","eval");
	}//cierra GoToSearchDiscoP80P2S5Eval

	public void OnClickSelectDiscop80P2S5(string interface_from){
		Debug.Log ("Llamado al metodo OnClickSelectDiscop80P2S5!!");
		info_additional_displayed = false;
		in_Evaluation_mode = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:SE18");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "SE18", "11", "-1","eval");
		GoToSearchElemLimpiezaP2S5Eval ();
	}//cierra OnClickSelectDiscop80P2S5

	public void GoToSearchElemLimpiezaP2S5Eval(){
		Debug.Log ("Entra al metodo GoToSearchElemLimpiezaP2S5Eval... interfaz: "  + current_interface);
		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_EVAL) {
			Destroy (ToolsAndProductsPhase2Step5_int_eval_instance);
		} else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP5_EV)
			Destroy (ARSearch_eval_mode_instance);
		
		//Verificar porque se estan quedando instanciadas las interfaces para evitar hacer estos dos llamados:
		DestroyInstancesWithTag ("MenuPhasesEvalMode");
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhaseOneEval");
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhaseTwoEval");
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE2_STEP5_EV;
		
		ARSearch_eval_mode_instance = Instantiate (ARModeEvaluation);
		ControllerBlinkARModeEvaluation controller_blinking_third = ARSearch_eval_mode_instance.GetComponent<ControllerBlinkARModeEvaluation> ();
		
		//Es iportante asignar esta variable para poder controlar los marcadores
		interfaceInstanceActive = ARSearch_eval_mode_instance;
		
		//localizando el marcador que se debe buscar en esta fase del proceso y cargando datos respectivos:
		markerInScene = GameObject.Find ("FrameMarker25");
		ControllerAddInfoInMarker controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		
		//asignando el texto que se debe mostrar al momento del feedback:
		controller_blinking_third.feedback_info_text_path = feedback_marker25_24_23_eval_mode_step6;
		
		//definiendo los ordenes que controlara esta interfaz:
		order_in_process = new int[1];
		order_in_process [0] = 19;
		controller_blinking_third.ordenes = order_in_process;
		
		//El llamado al siguiente metodo carga la informacion correspondiente en la interfaz:
		//NOTA: antes de hacer esto es importante que la variable interfaceInstanceActive este asignada:
		//tambien es importante haber asignado la variable inTutorialPhase1
		controller_info_marker.LoadInformationForEvaluationMode ();
		
		controller_blinking_third.interface_going_from = "Phase2Step5Eval";
		controller_blinking_third.onClickSelectBtn += OnClickSelectElemnLimpiezP2S5;
		
		//asignando el texto que se debe mostrar de guia:
		controller_blinking_third.texto_guia_producto = "Busca el producto 3:";
		
		//inicializando la variable que controla si estamos en el modo de evaluacion en RA:
		in_Evaluation_mode = true;
		
		//colocando en false la informacion adicional por si se le habia dado atras en algun momento en la interfaz:
		info_additional_displayed = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:IE48");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "IE48", "11", "-1","eval");
	}//cierra GoToSearchElemLimpiezaP2S5Eval

	public void OnClickSelectElemnLimpiezP2S5(string interface_from){
		Debug.Log ("Llamado al metodo OnClickSelectElemnLimpiezP2S5!!");
		info_additional_displayed = false;
		in_Evaluation_mode = false;
		//notificando que ya se ha completado la parte de productos y herramientas
		steps_p_two_eval_completed [4].activity_tools_and_products = true;
		steps_p_two_eval_completed [4].CheckStepCompletion ();
		
		SaveDataForStudent ();
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:SE19");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "SE19", "11", "-1","eval");
		if (steps_p_two_eval_completed [4].step_completed)
			GoToMenuStepsPhase2Eval ();
		else 
			GoToActivitiesPhase2Step5EvalMode ();
	}//cierra OnClickSelectElemnLimpiezP2S5


	public void GoToSearchDiscoP150P2S6Eval(){
		Debug.Log ("Entra al metodo GoToSearchDiscoP150P2S6Eval... interfaz: "  + current_interface);
		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_EVAL) {
			Destroy (ToolsAndProductsPhase2Step6_int_eval_instance);
		} else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP6_EV)
			Destroy (ARSearch_eval_mode_instance);
		
		//Verificar porque se estan quedando instanciadas las interfaces para evitar hacer estos dos llamados:
		DestroyInstancesWithTag ("MenuPhasesEvalMode");
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhaseOneEval");
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhaseTwoEval");
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE2_STEP6_EV;
		
		ARSearch_eval_mode_instance = Instantiate (ARModeEvaluation);
		ControllerBlinkARModeEvaluation controller_blinking = ARSearch_eval_mode_instance.GetComponent<ControllerBlinkARModeEvaluation> ();
		
		//Es iportante asignar esta variable para poder controlar los marcadores
		interfaceInstanceActive = ARSearch_eval_mode_instance;
		
		//localizando el marcador que se debe buscar en esta fase del proceso y cargando datos respectivos:
		markerInScene = GameObject.Find ("FrameMarker33");
		ControllerAddInfoInMarker controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		if (last_marker_id_evalmode == 32) {
			Debug.Log ("-->SEQUENCE:: Se va a cargar la informacion del DISCO P180 y el last_marker es:" + last_marker_id_evalmode);
			controller_info_marker.image_real_help_path = marker33_32_hint_image;
		} else {
			Debug.Log ("--> SEQUENCE: El last marker scanned fue: " + last_marker_id_evalmode);
			controller_info_marker.image_real_help_path = marker33_32_hint_image;
		}

		//asignando el texto que se debe mostrar al momento del feedback:
		controller_blinking.feedback_info_text_path = feedback_marker33_34eval_mode_step6;
		
		//definiendo los ordenes que controlara esta interfaz:
		order_in_process = new int[1];
		order_in_process [0] = 20;
		controller_blinking.ordenes = order_in_process;
		
		//El llamado al siguiente metodo carga la informacion correspondiente en la interfaz:
		//NOTA: antes de hacer esto es importante que la variable interfaceInstanceActive este asignada:
		//tambien es importante haber asignado la variable inTutorialPhase1
		controller_info_marker.LoadInformationForEvaluationMode ();

		//NOTA: Especialmente para este marcador se va a habilitar la validacion de secuencia:
		controller_blinking.validate_sequence_of_markers = true;
		controller_blinking.previous_marker_id = this.last_marker_id_evalmode;
		if (this.last_marker_id_evalmode == 30)
			controller_blinking.next_marker_id = 33;
		else
			controller_blinking.next_marker_id = 34;
		
		
		Debug.LogError ("NOW: Start Blinking en GoToSearchDiscoP150P2S6Eval");
		//asignando el texto para el feedback directamente a la interfaz:
		if(last_marker_id_evalmode == 32)
			controller_blinking.feedback_info_text_path = text_feedback_phase2step6_eval_p180;
		else controller_blinking.feedback_info_text_path = text_feedback_phase2step6_eval;
		
		controller_blinking.interface_going_from = "Phase2Step6Eval";
		controller_blinking.onClickSelectBtn += OnClickSelectDiscoP150P2S6;
		
		//asignando el texto que se debe mostrar de guia:
		controller_blinking.texto_guia_producto = "Busca el producto 1:";
		
		//inicializando la variable que controla si estamos en el modo de evaluacion en RA:
		in_Evaluation_mode = true;
		
		//colocando en false la informacion adicional por si se le habia dado atras en algun momento en la interfaz:
		info_additional_displayed = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:IE49");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "IE49", "12", "-1","eval");
	}//cierra GoToSearchDiscoP150P2S6Eval

	public void OnClickSelectDiscoP150P2S6(string interface_from){
		Debug.Log ("Llamado al metodo OnClickSelectElemnLimpiezP2S6!!");
		info_additional_displayed = false;
		in_Evaluation_mode = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:SE21");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "SE21", "12", "-1","eval");
		GoToSearchElemLimpiezaP2S6Eval ();
	}//cierra OnClickSelectDiscop80P2S5

	public void GoToSearchElemLimpiezaP2S6Eval(){
		Debug.Log ("Entra al metodo GoToSearchElemLimpiezaP2S6Eval... interfaz: "  + current_interface);
		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_EVAL) {
			Destroy (ToolsAndProductsPhase2Step6_int_eval_instance);
		} else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP6_EV)
			Destroy (ARSearch_eval_mode_instance);
		
		//Verificar porque se estan quedando instanciadas las interfaces para evitar hacer estos dos llamados:
		DestroyInstancesWithTag ("MenuPhasesEvalMode");
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhaseOneEval");
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhaseTwoEval");
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE2_STEP6_EV;
		
		ARSearch_eval_mode_instance = Instantiate (ARModeEvaluation);
		ControllerBlinkARModeEvaluation controller_blinking_second = ARSearch_eval_mode_instance.GetComponent<ControllerBlinkARModeEvaluation> ();
		
		//Es iportante asignar esta variable para poder controlar los marcadores
		interfaceInstanceActive = ARSearch_eval_mode_instance;
		
		//localizando el marcador que se debe buscar en esta fase del proceso y cargando datos respectivos:
		markerInScene = GameObject.Find ("FrameMarker25");
		ControllerAddInfoInMarker controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		
		//asignando el texto que se debe mostrar al momento del feedback:
		controller_blinking_second.feedback_info_text_path = feedback_marker25_24_23_eval_mode_step6;
		
		//definiendo los ordenes que controlara esta interfaz:
		order_in_process = new int[1];
		order_in_process [0] = 21;
		controller_blinking_second.ordenes = order_in_process;
		
		//El llamado al siguiente metodo carga la informacion correspondiente en la interfaz:
		//NOTA: antes de hacer esto es importante que la variable interfaceInstanceActive este asignada:
		//tambien es importante haber asignado la variable inTutorialPhase1
		controller_info_marker.LoadInformationForEvaluationMode ();
		
		controller_blinking_second.interface_going_from = "Phase2Step6Eval";
		controller_blinking_second.onClickSelectBtn += OnClickSelectElemnLimpiezP2S6;
		
		//asignando el texto que se debe mostrar de guia:
		controller_blinking_second.texto_guia_producto = "Busca el producto 2:";
		
		//inicializando la variable que controla si estamos en el modo de evaluacion en RA:
		in_Evaluation_mode = true;
		
		//colocando en false la informacion adicional por si se le habia dado atras en algun momento en la interfaz:
		info_additional_displayed = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:IE50");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "IE50", "12", "-1","eval");
	} //cierra GoToSearchElemLimpiezaP2S6Eval

	public void OnClickSelectElemnLimpiezP2S6(string interface_from){
		Debug.Log ("Llamado al metodo OnClickSelectElemnLimpiezP2S6!!");
		info_additional_displayed = false;
		in_Evaluation_mode = false;
		//notificando que ya se ha completado la parte de productos y herramientas
		steps_p_two_eval_completed [5].activity_tools_and_products = true;
		steps_p_two_eval_completed [5].CheckStepCompletion ();
		
		SaveDataForStudent ();
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:SE22");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "SE22", "12", "-1","eval");
		
		if (steps_p_two_eval_completed [5].step_completed)
			GoToMenuStepsPhase2Eval ();
		else 
			GoToActivitiesPhase2Step6EvalMode ();
	}//cierra OnClickSelectElemnLimpiezP2S6


	public void GoToSearchDiscoP240P2S7Eval(){
		Debug.Log ("Entra al metodo GoToSearchDiscoP240P2S7Eval... interfaz: "  + current_interface);
		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_EVAL) {
			Destroy (ToolsAndProductsPhase2Step7_int_eval_instance);
		} else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP7_EV)
			Destroy (ARSearch_eval_mode_instance);
		
		//Verificar porque se estan quedando instanciadas las interfaces para evitar hacer estos dos llamados:
		DestroyInstancesWithTag ("MenuPhasesEvalMode");
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhaseOneEval");
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhaseTwoEval");
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE2_STEP7_EV;
		
		ARSearch_eval_mode_instance = Instantiate (ARModeEvaluation);
		ControllerBlinkARModeEvaluation controller_blinking = ARSearch_eval_mode_instance.GetComponent<ControllerBlinkARModeEvaluation> ();
		
		//Es iportante asignar esta variable para poder controlar los marcadores
		interfaceInstanceActive = ARSearch_eval_mode_instance;
		
		//localizando el marcador que se debe buscar en esta fase del proceso y cargando datos respectivos:
		markerInScene = GameObject.Find ("FrameMarker36");
		ControllerAddInfoInMarker controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		
		//asignando el texto que se debe mostrar al momento del feedback:
		controller_blinking.feedback_info_text_path = feedback_marker36_eval_mode_step7;
		
		//definiendo los ordenes que controlara esta interfaz:
		order_in_process = new int[1];
		order_in_process [0] = 22;
		controller_blinking.ordenes = order_in_process;
		
		//El llamado al siguiente metodo carga la informacion correspondiente en la interfaz:
		//NOTA: antes de hacer esto es importante que la variable interfaceInstanceActive este asignada:
		//tambien es importante haber asignado la variable inTutorialPhase1
		controller_info_marker.LoadInformationForEvaluationMode ();
		
		controller_blinking.interface_going_from = "Phase2Step7Eval";
		controller_blinking.onClickSelectBtn += OnClickSelectDiscoP240P2S7;
		
		//asignando el texto que se debe mostrar de guia:
		controller_blinking.texto_guia_producto = "Busca el producto 1:";
		
		//inicializando la variable que controla si estamos en el modo de evaluacion en RA:
		in_Evaluation_mode = true;
		
		//colocando en false la informacion adicional por si se le habia dado atras en algun momento en la interfaz:
		info_additional_displayed = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:IE51");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "IE51", "13", "-1","eval");
	}//cierra GoToSearchDiscoP240P2S7Eval


	public void OnClickSelectDiscoP240P2S7(string interface_from){
		Debug.Log ("Llamado al metodo OnClickSelectDiscoP240P2S7!!");
		info_additional_displayed = false;
		in_Evaluation_mode = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:IE23");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "SE23", "13", "-1","eval");
		GoToSearchElemenLimpP2S7Eval ();
	} //cierra OnClickSelectDiscoP240P2S7

	public void GoToSearchElemenLimpP2S7Eval(){
		Debug.Log ("Entra al metodo GoToSearchElemenLimpP2S7Eval... interfaz: "  + current_interface);
		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_EVAL) {
			Destroy (ToolsAndProductsPhase2Step7_int_eval_instance);
		} else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP7_EV)
			Destroy (ARSearch_eval_mode_instance);
		
		//Verificar porque se estan quedando instanciadas las interfaces para evitar hacer estos dos llamados:
		DestroyInstancesWithTag ("MenuPhasesEvalMode");
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhaseOneEval");
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhaseTwoEval");
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE2_STEP7_EV;
		
		ARSearch_eval_mode_instance = Instantiate (ARModeEvaluation);
		ControllerBlinkARModeEvaluation controller_blinking_second = ARSearch_eval_mode_instance.GetComponent<ControllerBlinkARModeEvaluation> ();
		
		//Es iportante asignar esta variable para poder controlar los marcadores
		interfaceInstanceActive = ARSearch_eval_mode_instance;
		
		//localizando el marcador que se debe buscar en esta fase del proceso y cargando datos respectivos:
		markerInScene = GameObject.Find ("FrameMarker25");
		ControllerAddInfoInMarker controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		
		//asignando el texto que se debe mostrar al momento del feedback:
		controller_blinking_second.feedback_info_text_path = feedback_marker24_25_eval_mode_step7;
		
		//definiendo los ordenes que controlara esta interfaz:
		order_in_process = new int[1];
		order_in_process [0] = 23;
		controller_blinking_second.ordenes = order_in_process;
		
		//El llamado al siguiente metodo carga la informacion correspondiente en la interfaz:
		//NOTA: antes de hacer esto es importante que la variable interfaceInstanceActive este asignada:
		//tambien es importante haber asignado la variable inTutorialPhase1
		controller_info_marker.LoadInformationForEvaluationMode ();
		
		controller_blinking_second.interface_going_from = "Phase2Step7Eval";
		controller_blinking_second.onClickSelectBtn += OnClickSelectElemeLimpieP2S7;
		
		//asignando el texto que se debe mostrar de guia:
		controller_blinking_second.texto_guia_producto = "Busca el elemento 2:";
		
		//inicializando la variable que controla si estamos en el modo de evaluacion en RA:
		in_Evaluation_mode = true;
		
		//colocando en false la informacion adicional por si se le habia dado atras en algun momento en la interfaz:
		info_additional_displayed = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:IE52");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "IE52", "13", "-1","eval");
	}//cierra GoToSearchElemenLimpP2S7Eval

	public void OnClickSelectElemeLimpieP2S7(string interface_from){
		Debug.Log ("Llamado al metodo OnClickSelectElemeLimpieP2S7!!");
		info_additional_displayed = false;
		in_Evaluation_mode = false;
		//notificando que ya se ha completado la parte de productos y herramientas
		steps_p_two_eval_completed [6].activity_tools_and_products = true;
		steps_p_two_eval_completed [6].CheckStepCompletion ();
		
		SaveDataForStudent ();
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:SE24");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "SE24", "13", "-1","eval");
		if (steps_p_two_eval_completed [6].step_completed)
			GoToMenuStepsPhase2Eval ();
		else 
			GoToActivitiesPhase2Step7EvalMode ();
	}//cierra OnClickSelectElemeLimpieP2S7

	public void GoToSearchDiscoP320P2S8Eval(){
		Debug.Log ("Entra al metodo GoToSearchDiscoP320P2S8Eval... interfaz: "  + current_interface);
		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_EVAL) {
			Destroy (ToolsAndProductsPhase2Step8_int_eval_instance);
		} else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP8_EV)
			Destroy (ARSearch_eval_mode_instance);
		
		//Verificar porque se estan quedando instanciadas las interfaces para evitar hacer estos dos llamados:
		DestroyInstancesWithTag ("MenuPhasesEvalMode");
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhaseOneEval");
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhaseTwoEval");
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE2_STEP8_EV;
		
		ARSearch_eval_mode_instance = Instantiate (ARModeEvaluation);
		ControllerBlinkARModeEvaluation controller_blinking = ARSearch_eval_mode_instance.GetComponent<ControllerBlinkARModeEvaluation> ();
		
		//Es iportante asignar esta variable para poder controlar los marcadores
		interfaceInstanceActive = ARSearch_eval_mode_instance;
		
		//localizando el marcador que se debe buscar en esta fase del proceso y cargando datos respectivos:
		markerInScene = GameObject.Find ("FrameMarker38");
		ControllerAddInfoInMarker controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		
		//asignando el texto que se debe mostrar al momento del feedback:
		controller_blinking.feedback_info_text_path = feedback_marker38_eval_mode_step8;
		
		//definiendo los ordenes que controlara esta interfaz:
		order_in_process = new int[1];
		order_in_process [0] = 24;
		controller_blinking.ordenes = order_in_process;
		
		//El llamado al siguiente metodo carga la informacion correspondiente en la interfaz:
		//NOTA: antes de hacer esto es importante que la variable interfaceInstanceActive este asignada:
		//tambien es importante haber asignado la variable inTutorialPhase1
		controller_info_marker.LoadInformationForEvaluationMode ();
		
		controller_blinking.interface_going_from = "Phase2Step8Eval";
		controller_blinking.onClickSelectBtn += OnClickSelectDiscoP320P2S8;
		
		//asignando el texto que se debe mostrar de guia:
		controller_blinking.texto_guia_producto = "Busca el elemento 1:";
		
		//inicializando la variable que controla si estamos en el modo de evaluacion en RA:
		in_Evaluation_mode = true;
		
		//colocando en false la informacion adicional por si se le habia dado atras en algun momento en la interfaz:
		info_additional_displayed = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:IE53");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "IE53", "14", "-1","eval");
	}//cierra GoToSearchDiscoP320P2S8Eval

	public void OnClickSelectDiscoP320P2S8(string interface_from){
		Debug.Log ("Llamado al metodo OnClickSelectElemLimpiezaP2S8!!");
		info_additional_displayed = false;
		in_Evaluation_mode = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:SE25");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "SE25", "14", "-1","eval");
		GoToSearchElemenLimpiezP2S8Eval ();
	} //cierra OnClickSelectElemLimpiezaP2S8

	public void GoToSearchElemenLimpiezP2S8Eval(){
		Debug.Log ("Entra al metodo GoToSearchElemenLimpiezP2S8Eval... interfaz: "  + current_interface);
		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_EVAL) {
			Destroy (ToolsAndProductsPhase2Step8_int_eval_instance);
		} else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP8_EV)
			Destroy (ARSearch_eval_mode_instance);
		
		//Verificar porque se estan quedando instanciadas las interfaces para evitar hacer estos dos llamados:
		DestroyInstancesWithTag ("MenuPhasesEvalMode");
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhaseOneEval");
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhaseTwoEval");
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE2_STEP8_EV;
		
		ARSearch_eval_mode_instance = Instantiate (ARModeEvaluation);
		ControllerBlinkARModeEvaluation controller_blinking_second = ARSearch_eval_mode_instance.GetComponent<ControllerBlinkARModeEvaluation> ();
		
		//Es iportante asignar esta variable para poder controlar los marcadores
		interfaceInstanceActive = ARSearch_eval_mode_instance;
		
		//localizando el marcador que se debe buscar en esta fase del proceso y cargando datos respectivos:
		markerInScene = GameObject.Find ("FrameMarker25");
		ControllerAddInfoInMarker controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		
		//asignando el texto que se debe mostrar al momento del feedback:
		controller_blinking_second.feedback_info_text_path = feedback_marker25_24_23_EvalMode_step8;
		
		//definiendo los ordenes que controlara esta interfaz:
		order_in_process = new int[1];
		order_in_process [0] = 25;
		controller_blinking_second.ordenes = order_in_process;
		
		//El llamado al siguiente metodo carga la informacion correspondiente en la interfaz:
		//NOTA: antes de hacer esto es importante que la variable interfaceInstanceActive este asignada:
		//tambien es importante haber asignado la variable inTutorialPhase1
		controller_info_marker.LoadInformationForEvaluationMode ();
		
		controller_blinking_second.interface_going_from = "Phase2Step8Eval";
		controller_blinking_second.onClickSelectBtn += OnClickSelectElemLiempiezP2S8;
		
		//asignando el texto que se debe mostrar de guia:
		controller_blinking_second.texto_guia_producto = "Busca el elemento 2:";
		
		//inicializando la variable que controla si estamos en el modo de evaluacion en RA:
		in_Evaluation_mode = true;
		
		//colocando en false la informacion adicional por si se le habia dado atras en algun momento en la interfaz:
		info_additional_displayed = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:IE54");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "IE54", "14", "-1","eval");
	}// cierra GoToSearchElemenLimpiezP2S8Eval

	public void OnClickSelectElemLiempiezP2S8(string interface_from){
		Debug.Log ("Llamado al metodo OnClickSelectElemLiempiezP2S8!!");
		info_additional_displayed = false;
		in_Evaluation_mode = false;
		//notificando que ya se ha completado la parte de productos y herramientas
		steps_p_two_eval_completed [7].activity_tools_and_products = true;
		steps_p_two_eval_completed [7].CheckStepCompletion ();
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:SE26");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (this.codigo_estudiante, fecha, "SE26", "14", "-1","eval");
		SaveDataForStudent ();
		
		if (steps_p_two_eval_completed [7].step_completed)
			GoToMenuStepsPhase2Eval ();
		else 
			GoToActivitiesPhase2Step8EvalMode ();
	}// cierra OnClickSelectElemLiempiezP2S8

}//cierra AppManager

[Serializable]
class StudentData{
	//variable que controla si el modo evaluacion estara activado:
	public bool evaluat_mode_enabled;

	//variable que controla si el estudiante ya ha organizado correctamente las fases del proceso:
	public bool evaluat_mode_phases_organized;

	//variable que controla si el estudiante ya ha organizado correctamente los pasos de la FASE 1:
	public bool evaluat_mode_phase1_steps_organized;
	//variable que controla si el estudiante ya ha organizado correctamente los pasos de la FASE 2:
	public bool evaluat_mode_phase2_steps_organized;

	//variable que indica que el estudiante ya se ha logueado:
	public bool student_logged;

	//variable que almacena el codigo del estudiante:
	public string cod_estudiante;

	//variable que almacena el ultimo marcador de disco P80 o P120 escaneado por el estudiante en la Fase 2 Paso 4
	//porque eso modifica el paso siguiente por lo tanto hay que guardarlo por si el estudiante no continua usando la apliacion despues de ese punto:
	public int last_marker_step4;

	//variable que almacena el ultimo marcador de disco P80 o P120 escaneado por el estudiante en la Fase 2 Paso 4
	//MODO EVALUACION
	public int last_marker_step4_eval_mode;

	//variables que almacenan el estado de las fases:
	public bool phaseOneEnable;
	public bool phaseTwoEnable;
	public bool phaseThreeEnable;
	public bool phaseFourEnable;
	public bool phaseFiveEnable;
	public bool phaseSixEnable;

	//variables que almacenan el estado de las fases para el modo evaluacion:
	public bool phaseOneEnableEval;
	public bool phaseTwoEnableEval;
	public bool phaseThreeEnableEval;
	public bool phaseFourEnableEval;
	public bool phaseFiveEnableEval;
	public bool phaseSixEnableEval;

	//variables que controlan el estado de los pasos de la fase 1:
	public bool step_one_phase_one;
	public bool step_two_phase_one;
	public bool step_three_phase_one;
	public bool step_four_phase_one;
	public bool step_five_phase_one;
	public bool step_six_phase_one;

	//variables que controlan el estado de los pasos de la fase 2:
	public bool step_one_phase_two;
	public bool step_two_phase_two;
	public bool step_three_phase_two;
	public bool step_four_phase_two;
	public bool step_five_phase_two;
	public bool step_six_phase_two;
	public bool step_seven_phase_two;
	public bool step_eight_phase_two;

	//variables que controlan el estado de los pasos EN MODO EVALUACION (FASE 1):
	public bool step_one_p1_eval_mode;
	public bool step_two_p1_eval_mode;
	public bool step_three_p1_eval_mode;
	public bool step_four_p1_eval_mode;
	public bool step_five_p1_eval_mode;
	public bool step_six_p1_eval_mode;

	//variables que controlan el estado de los pasos EN MODO EVALUACION (FASE 2):
	public bool step_one_p2_eval_mode;
	public bool step_two_p2_eval_mode;
	public bool step_three_p2_eval_mode;
	public bool step_four_p2_eval_mode;
	public bool step_five_p2_eval_mode;
	public bool step_six_p2_eval_mode;
	public bool step_seven_p2_eval_mode;
	public bool step_eight_p2_eval_mode;

} //cierra clase StudentData

class StepOfProcess{

	public StepOfProcess(bool toolsAndProducts, bool videos, bool evaluation, bool photo_tecnica, bool photo_seguridad){
		this.activity_tools_and_products = toolsAndProducts;
		this.videos_about_process = videos;
		this.take_photo_ficha_tecnica = photo_tecnica;
		this.take_photo_ficha_seguridad = photo_seguridad;
		this.step_completed = false;
		//this.forced_to_complete = false;
	}
	//variables que representan las actividades del paso:
	public bool activity_tools_and_products; //actividad de buscar herram y prods con RA
	public bool videos_about_process; //videos sobre el proceso
	public bool selfevaluation; //completar la evaluacion del proceso
	public bool take_photo_ficha_tecnica; //completar actividad de tomar foto a la ficha tecnica
	public bool take_photo_ficha_seguridad; //actividad de tomar foto a la ficha de seguridad

	//public bool forced_to_complete; //variable que indica si el paso debe aparecer completado debido a que asi se carga del archivo de datos del estudiante

	//variable que representa si las actividades ya se han completado satisfactoriamente:
	public bool step_completed;

	public void CheckStepCompletion(){
		if (activity_tools_and_products && videos_about_process && selfevaluation && take_photo_ficha_tecnica && take_photo_ficha_seguridad)
			step_completed = true;

	}

	/// <summary>
	/// Notifies the loading step completed. Method that is called when the step was completed and is loaded from the saved data
	/// for the student
	/// </summary>
	public void NotifyLoadingStepCompleted(){
		this.videos_about_process = true;
		this.activity_tools_and_products = true;
		this.selfevaluation = true;
		this.take_photo_ficha_tecnica = true;
		this.take_photo_ficha_seguridad = true;
		//se esta forzando el paso a ser completado

		CheckStepCompletion ();
	}
} //cierra clase StepOfProcess


class StepOfProcessEvalMode{
	
	public StepOfProcessEvalMode(bool toolsAndProducts, bool evaluation){
		this.activity_tools_and_products = toolsAndProducts;
		this.selfevaluation = evaluation;
		this.step_completed = false;
	}
	//variables que representan las actividades del paso: (en el modo evaluacion solamente preguntas y herramientas y productos)
	public bool activity_tools_and_products; //actividad de buscar herram y prods con RA
	//public bool videos_about_process; //videos sobre el proceso
	public bool selfevaluation; //completar la evaluacion del proceso
	//public bool take_photo_ficha_tecnica; //completar actividad de tomar foto a la ficha tecnica
	//public bool take_photo_ficha_seguridad; //actividad de tomar foto a la ficha de seguridad
	
	//variable que representa si las actividades ya se han completado satisfactoriamente:
	public bool step_completed;
	
	public void CheckStepCompletion(){
		if (activity_tools_and_products && selfevaluation)
			step_completed = true;
	}
	
	/// <summary>
	/// Notifies the loading step completed. Method that is called when the step was completed and is loaded from the saved data
	/// for the student
	/// </summary>
	public void NotifyLoadingStepCompleted(){
		//this.videos_about_process = true;
		this.activity_tools_and_products = true;
		this.selfevaluation = true;
		//this.take_photo_ficha_tecnica = true;
		//this.take_photo_ficha_seguridad = true;
		CheckStepCompletion ();
	}
}