using UnityEngine;
using System.Collections;

public class InformativeMode : MonoBehaviour {

	//referencias a los objetos de la interfaz:
	private GameObject menuProcessPhases_int_im_instance;
	private GameObject menuStepsPhase1_int_im_instance;
	private GameObject menuStepsPhaseTwo_int_im_instance;
	private GameObject menuSubStepsPhaseTwo_int_im_instance;
	private GameObject menuSubStepsPhaseTwoInterior_int_im_instance;

	private GameObject ActivitiesForPhase1Step1_int_im_instance;
	private GameObject ActivitiesForPhase1Step2_int_im_instance;
	private GameObject ActivitiesForPhase1Step3_int_im_instance;
	private GameObject ActivitiesForPhase1Step4_int_im_instance;
	private GameObject ActivitiesForPhase1Step5_int_im_instance;
	private GameObject ActivitiesForPhase1Step6_int_im_instance;

	//Para la fase de Matizado:
	private GameObject ActivitiesForPhase2Step1_int_im_instance;
	private GameObject ActivitiesForPhase2Step2_int_im_instance;
	private GameObject ActivitiesForPhase2Step3_int_im_instance;
	private GameObject ActivitiesForPhase2Step4_int_im_instance;
	private GameObject ActivitiesForPhase2Step5_int_im_instance;
	private GameObject ActivitiesForPhase2Step6_int_im_instance;
	private GameObject ActivitiesForPhase2Step7_int_im_instance;
	private GameObject ActivitiesForPhase2Step8_int_im_instance;

	private GameObject ToolsAndProductsPhase1Step2_int_im_instance;
	private GameObject ToolsAndProductsPhase1Step3_int_im_instance;
	private GameObject ToolsAndProductsPhase1Step4_int_im_instance;
	private GameObject ToolsAndProductsPhase1Step5_int_im_instance;
	private GameObject ToolsAndProductsPhase1Step6_int_im_instance;
	//para la fase de matizado
	private GameObject ToolsAndProductsPhase2Step2_int_im_instance;
	private GameObject ToolsAndProductsPhase2Step3_int_im_instance;
	private GameObject ToolsAndProductsPhase2Step4_int_im_instance;
	private GameObject ToolsAndProductsPhase2Step5_int_im_instance;
	private GameObject ToolsAndProductsPhase2Step6_int_im_instance;
	private GameObject ToolsAndProductsPhase2Step7_int_im_instance;
	private GameObject ToolsAndProductsPhase2Step8_int_im_instance;

	private GameObject TurorialSearchCapoCarro_int_im_instance;
	private GameObject TutorialPhaseTwoSearchProd_int_im_instance;
	private GameObject TutorialTwoSearchBayeta_int_im_instance;
	private GameObject AR_Mode_Search_int_im_instance;



	//Variable para obtener referencias a los objetos tipo marcador:
	private GameObject markerInScene;

	//variable para almacenar el orden de los pasos que va a controlar cada interfaz del proceso:
	public int[] order_in_process;

	//variables que se utilizan cuando se hace un tap sobre la interfaz
	private bool in_tutorial_phase1;
	private bool in_tutorial_phase2;
	private bool information_loaded_from_marker;
	private bool inRAmode;
	private bool inEvaluationMode;
	private GameObject interface_active;


	//Referencias para controlar la interfaz actual:
	public static CurrentInterface current_interface;
		
	public enum CurrentInterface 
	{
		SELECTION_OF_MODE, 
		MENU_PHASES_IM,
		MENU_STEPS_PHASE1_IM,
		MENU_STEPS_PHASE2_IM,
		MENU_SUB_STEPS_PHASE2_IM,
		MENU_SUB_STEPS_INTERIORES_PHASE2_IM,
		ACTIVITIES_PHASE1_STEP1_IM,
		ACTIVITIES_PHASE1_STEP2_IM,
		ACTIVITIES_PHASE1_STEP3_IM,
		ACTIVITIES_PHASE1_STEP4_IM,
		ACTIVITIES_PHASE1_STEP5_IM,
		ACTIVITIES_PHASE1_STEP6_IM,
		ACTIVITIES_PHASE2_STEP1_IM,
		ACTIVITIES_PHASE2_STEP2_IM,
		ACTIVITIES_PHASE2_STEP3_IM,
		ACTIVITIES_PHASE2_STEP4_IM,
		ACTIVITIES_PHASE2_STEP5_IM,
		ACTIVITIES_PHASE2_STEP6_IM,
		ACTIVITIES_PHASE2_STEP7_IM,
		ACTIVITIES_PHASE2_STEP8_IM,
		AR_SEARCH_CAR_HOOD_IM,
		TOOLS_AND_PRODUCTS_IM,
		AR_SEARCH_PRODUCTS_TUTORIAL_IM,
		AR_SEARCH_AGUA_JABON_IM,
		AR_SEARCH_BAYETA_PRODUCT_TUTORIAL_IM,
		AR_SEARCH_PHASE1_STEP3_IM,
		AR_SEARCH_PHASE1_STEP4_IM,
		AR_SEARCH_PHASE1_STEP5_IM,
		AR_SEARCH_PHASE1_STEP6_IM,
		AR_SEARCH_PHASE2_STEP2_IM,
		AR_SEARCH_PHASE2_STEP3_IM,
		AR_SEARCH_PHASE2_STEP4_IM,
		AR_SEARCH_PHASE2_STEP5_IM,
		AR_SEARCH_PHASE2_STEP6_IM,
		AR_SEARCH_PHASE2_STEP7_IM,
		AR_SEARCH_PHASE2_STEP8_IM
		
	};

	//Variables para controlar lo que se muestra en el menu de fases del proceso:
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

	//listado de variables que definen las rutas a las imgs de las fases con fondo gris:
	private string menuPhases_int_btn_uno_image_gray = "Sprites/1_Limpieza_FaseProceso_gray";
	private string menuPhases_int_btn_dos_image_gray = "Sprites/2_matizado_FaseProceso_gray";
	private string menuPhases_int_btn_tres_image_gray = "Sprites/3_masillado_FaseProceso_gray";
	private string menuPhases_int_btn_cuatro_image_gray = "Sprites/4_aparejado_FaseProceso_gray";
	private string menuPhases_int_btn_cinco_image_gray = "Sprites/5_pintado_FaseProceso_gray";
	private string menuPhases_int_btn_seis_image_gray = "Sprites/6_barnizado_FaseProceso_gray";

	//Variables para configurar el menu de pasos de la fase 1:
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
	private string title_phase1_step1 = "Buscar el capo del coche";
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
	private string title_phase1_step2 = "Limpieza de la superficie";
	private string image_header_phase1Step2 = "Sprites/phase1/Step2_Phase1_text";
	private string introduction_text_phase1Step2_path = "Texts/Phase1Step2/0_Phase1Step2_text";
	
	//variables para configurar la parte del tutorial 2 para la busqueda de productos y herramientas (maquina de agua a presion):
	private string image_marker_tutorial2_p = "Sprites/markers/frameMarker_016";
	private string image_marker_tutorial2_real_p = "Sprites/phase1step2/FrameMarker16_maquina_agua";
	private string image_btn_select_path = "Sprites/buttons/btn_seleccionar";
	private string image_btn_one_path = "Sprites/buttons/guantes_info";
	private string image_btn_two_path = "Sprites/buttons/";
	private string text_add_info_btn_one = "Texts/Phase1Step2/6_add_info_btn_one_text";
	private string text_feedback = "Texts/Phase1Step2/7_feedback_text";

	//variables para configurar la parte del tutorial 2 para la busqueda de productos y herramientas (agua y jabon):
	private string image_marker_tut2_p = "Sprites/markers/frameMarker_019";
	private string image_marker_tut2_real_p = "Sprites/phase1step2/FrameMarker19_agua_jabon";
	private string text_feedback_jabon = "Texts/Phase1Step2/7_feedback_text_jabon";
	
	//variables para configurar la parte de buscar la bayeta como parte final del tutorial 2: (Bayeta)
	private string image_marker3_tutorial2_p = "Sprites/markers/frameMarker_021";
	private string image_marker3_tutorial2_real_p = "Sprites/phase1step2/FrameMarker21_baieta_neteja";
	private string text_feedback_bayeta = "Texts/Phase1Step2/7_feedback_text_bayeta";

	//variable que configura el feedback del agua a presion en la Fase 1 - Paso 3 (secado):
	private string feedback_phase1step3_agua = "Texts/Phase1Step3/7_feedback_text_agua";

	//Variables para configurar la interfaz del listado de actividades Fase 1 - Paso 3 (secado):
	private string title_phase1_step3 = "Secado";
	private string image_header_phase1Step3 = "Sprites/phase1/Step3_Phase1_text";
	private string introduction_text_phase1Step3_path = "Texts/Phase1Step3/0_Phase1Step3_text";
	//variables que configuran la interfaz del modo RA para la fase 1 paso 3 (secado) - Objeto - Agua a presion:
	private string image_marker25_p = "Sprites/markers/frameMarker_025";
	private string image_marker25_real_p = "Sprites/phase1step3/FrameMarker25_papel_dc3430";
	private string text_feedback_marker25_dc3430 = "Texts/Phase1Step3/8_feedback_text_papel_absorb";
	private string marker25_text_add_info_btn_one = "Texts/Phase1Step3/6_add_info_btn_one_text";
	
	//variables que configuran la interfaz del modo RA par ala fase 1 del paso 3 (secado) - Objeto - Papel absorbente:
	private string image_marker24_p = "Sprites/markers/frameMarker_024";
	private string image_marker24_real_p = "Sprites/phase1step3/FrameMarker24_paper_neteja";
	private string text_feedback_marker24_paper = "Texts/Phase1Step3/8_feedback_text_paper_neteja";
	private string marker24_text_add_info_btn_one = "Texts/Phase1Step3/6_add_info_btn_one_text";

	//variables para configurar la interfaz del listado de actividades Fase1 - Paso 4 (Localizar irregularidades):
	private string title_phase1_step4 = "Localizar Irregularidades";
	private string image_header_phase1Step4 = "Sprites/phase1/Step4_Phase1_text";
	private string introduction_text_phase1Step4_path = "Texts/Phase1Step4/0_Phase1Step4_text";
	//variables que configuran la interfaz del modo RA para la fase 1 paso 4 (Localizar Irregularidades) - Objeto: esponja paper P320
	private string image_marker45_p = "Sprites/markers/frameMarker_045";
	private string image_marker45_real_p = "Sprites/phase1step4/FrameMarker45_esponja_p320";
	private string text_feedback_marker45_lija = "Texts/Phase1Step4/8_feedback_text_lija";
	private string marker5_text_add_info_btn_one = "Texts/Phase1Step4/6_add_info_btn_one_text";
	private string marker5_text_add_info_btn_two = "Texts/Phase1Step4/7_add_info_btn_two_text";
	private string image_button_two_mascara_polv = "Sprites/buttons/mascara_info";

	//variables para configurar la interfaz del listado de actividades de la Fase1-Paso5 (Corregir irregularidades)
	private string title_phase1_step5 = "Corregir Irregularidades";
	private string image_header_phase1Step5 = "Sprites/phase1/Step5_Phase1_text";
	private string introduction_text_phase1Step5_path = "Texts/Phase1Step5/0_Phase1Step5_text";
	//variables que configuran la interfaz del modo RA para la fase 1 paso 5 (corregir Irregularidades) - Objeto: martillo repasar
	private string image_marker100_p = "Sprites/markers/frameMarker100_martillo"; 
	private string image_marker100_real_p = "Sprites/phase1step5/FrameMarker100_martillo_repasar";
	private string text_feedback_marker6_martillo = "Texts/Phase1Step5/4_feedback_text_martillo_repasar";
	private string marker6_text_add_info_btn_one = "Texts/Phase1Step5/5_add_info_btn_one_text";

	//variables para configurar la interfaz del listado de actividades de la Fase1-Paso6 (Desengrasado):
	private string title_phase1_step6 = "Desengrasado";
	private string image_header_phase1Step6 = "Sprites/phase1/Step6_Phase1_text";
	private string introduction_text_phase1Step6_path = "Texts/Phase1Step6/0_Phase1Step6_text";
	//variables que configuran la interfaz del modo RA para la fase 1 paso 6 (desengrasado) - Objeto: desengrasante
	private string image_marker26_p = "Sprites/markers/frameMarker_026";
	private string image_marker26_real_p = "Sprites/phase1step6/8_feedback_text_desengrasante_bayeta";
	private string text_feedback_marker26_desengras = "Texts/Phase1Step6/8_feedback_text_desengrasante_bayeta";
	private string marker26_text_add_info_btn_one = "Texts/Phase1Step6/6_add_info_btn_one_text";
	private string marker26_text_add_info_btn_two = "Texts/Phase1Step6/7_add_info_btn_two_text";
	//variables que configuran la interfaz del modo RA para la fase 1 paso 6 segunda parte : - Objeto: bayeta
	private string image_button_two_mascara_gas = "Sprites/buttons/mascara_gas";

	//variables que definen las URL a los videos que se van a reproducir:
	private string video_phase1_step2 = "http://piranya.udg.edu/pintuRA/videos/phase1_step2.mp4";
	private string video_phase1_step3 = "http://piranya.udg.edu/pintuRA/videos/phase1_step3.mp4";
	private string video_phase1_step4 = "http://piranya.udg.edu/pintuRA/videos/phase1_step4.mp4";
	private string video_phase1_step5 = "http://piranya.udg.edu/pintuRA/videos/1_limpieza_convert.mp4";
	private string video_phase1_step6 = "http://piranya.udg.edu/pintuRA/videos/phase1_step6.mp4";
	private string video_matizado_phase = "http://piranya.udg.edu/pintuRA/videos/phase2.mp4";

	//*****************************************************************************************************************
	//*****************************************************************************************************************
	//Imagenes que configuran la FASE DE MATIZADO
	//Imagenes de los pasos generales del proceso:
	private string image_phase2step1_with_text = "Sprites/phase2/Step1_Phase2_text";
	private string image_phase2step2_with_text = "Sprites/phase2/Step2_Phase2_text";
	private string image_phase2step3_with_text = "Sprites/phase2/Step3_Phase2_text";
	private string image_phase2step4_with_text = "Sprites/phase2/Step4_Phase2_text";
	private string image_phase2step5_with_text = "";
	private string image_phase2step6_with_text = "";
	
	//Variables de las imagenes en gris de cada uno de los pasos del proceso:
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
	//NOTA: Las siguientes imagenes no tienen el texto embebido:
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

	//Variables que configuran la interfaz del Phase 1 Step 1 (INTRODUCCION AL MATIZADO):
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
	//marcador del papel de enmascarar:
	private string image_marker69_p = "Sprites/markers/frameMarker69_papel_enmascarar";
	private string image_marker69_real_p = "Sprites/phase2step2/FrameMarker69_papel_enmascarar";
	private string marker69_text_add_info_btn_one = "Texts/Phase2Step2/7_add_info_btn_one_marker2";
	
	//Variables paa configurar las actividades de la FASE2 - PASO3 (Lijado de Cantos - Primera pasada):
	private string title_phase2_step3 = "3.1 Primera pasada del lijado de cantos";
	private string image_header_phase2Step3 = "Sprites/phase2/Step3_1_Phase2_text";
	private string introduction_text_phase2Step3_path = "Texts/Phase2Step3/0_introduction_text";
	private string text_feedback_phase2step3 = "Texts/Phase2Step3/6_feedback_text_phase2step3";
	
	//Variables para configurar las actividades de la FASE2 - PASO4 (Lijado de cantos - Pasada Final):
	private string title_phase2_step4 = "3.2 Segunda Pasada del lijado de cantos";
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

	/// <summary>
	/// Initializes a new instance of the <see cref="InformativeMode"/> class.
	/// Constructor
	/// </summary>
	public InformativeMode(){
		Debug.Log ("Llamado al constructor de la clase InformativeMode");
		current_interface = CurrentInterface.SELECTION_OF_MODE;
	}


	public void GoToMenuPhasesInformativeMode(){
		Debug.LogError ("Llamado al metodo go to Menu Phases del INFORMATIVE MODE");
		
		if (current_interface == CurrentInterface.SELECTION_OF_MODE)
			Destroy (AppManager.manager.selectionOfMode_interface_instance);
		else if (current_interface == CurrentInterface.MENU_STEPS_PHASE1_IM)
			Destroy (menuStepsPhase1_int_im_instance);
		else if (current_interface == CurrentInterface.MENU_STEPS_PHASE2_IM)
			Destroy (menuSubStepsPhaseTwo_int_im_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_PRODUCTS_TUTORIAL_IM)
			Destroy (TutorialPhaseTwoSearchProd_int_im_instance);

		current_interface = CurrentInterface.MENU_PHASES_IM;
		
		menuProcessPhases_int_im_instance = Instantiate (AppManager.manager.menuProcessPhases);
		CanvasProcessPhasesManager cProcessPhaseManager = menuProcessPhases_int_im_instance.GetComponent<CanvasProcessPhasesManager> ();
		cProcessPhaseManager.titulo = this.menuPhases_interface_title;
		//asignando imagenes a los botones de la interfaz
		cProcessPhaseManager.introduction_text_path = this.menuPhases_interface_introduction_text_path;
		cProcessPhaseManager.image_uno_limpieza = this.menuPhases_interface_button_uno_image;
		cProcessPhaseManager.image_dos_matizado = this.menuPhases_interface_button_dos_image;
		cProcessPhaseManager.image_tres_masillado = this.menuPhases_int_btn_tres_image_gray;
		cProcessPhaseManager.image_cuatro_aparejado = this.menuPhases_int_btn_cuatro_image_gray;
		cProcessPhaseManager.image_cinco_pintado = this.menuPhases_int_btn_cinco_image_gray;
		cProcessPhaseManager.image_seis_barnizado = this.menuPhases_int_btn_seis_image_gray;
		//asignando textos a los botones de la interfaz:
		cProcessPhaseManager.button_uno_text_limpieza = this.menuPhases_interface_button_uno_text;
		cProcessPhaseManager.button_dos_text_matizado = this.menuPhases_interface_button_dos_text;
		cProcessPhaseManager.button_tres_text_masillado = this.menuPhases_interface_button_tres_text;
		cProcessPhaseManager.button_cuatro_text_aparejado = this.menuPhases_interface_button_cuatro_text;
		cProcessPhaseManager.button_cinco_text_pintado = this.menuPhases_interface_button_cinco_text;
		cProcessPhaseManager.button_seis_text_barnizado = this.menuPhases_interface_button_seis_text;
		
		cProcessPhaseManager.goToMenuStepsOfPhase1_action += GoToMenuStepsPhase1InformativeMode;
		cProcessPhaseManager.goToMenuStepsOfPhase2_action += GoToMenuStepsPhase2InformativeMode;

		cProcessPhaseManager.goBackToSelectionOfMode += GoToSelectionModeFromInfoMode;

		//Activando las fases que se pueden consultar desde el modo informativo:
		cProcessPhaseManager.phase_one_button_enable = true;
		cProcessPhaseManager.phase_two_button_enable = true;
		cProcessPhaseManager.phase_three_button_enable = false;
		cProcessPhaseManager.phase_four_button_enable = false;
		cProcessPhaseManager.phase_five_button_enable = false;
		cProcessPhaseManager.phase_six_button_enable = false;
		//registrando la navegacion de la interfaz:
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "II1", "0", "-1","consulta");
	} //cierra GoToMenuPhasesInformativeMode

	/// <summary>
	/// Goes to selection mode from info mode. This method is called from GoToMenuPhasesInformativeMode in order to
	/// finish the informative mode and go back to selection of mode usign the button regresar
	/// </summary>
	public void GoToSelectionModeFromInfoMode(){
		if (current_interface == CurrentInterface.MENU_PHASES_IM)
			Destroy (menuProcessPhases_int_im_instance);
		//finalizando el modo informative mode:
		AppManager.manager.in_informative_mode = false;
		//llamando al metodo GoToSelectionOfMode:
		AppManager.manager.GoToSelectionOfMode ();
		
	} // cierra metodo GoToSelectionModeFromInfoMode

	/// <summary>
	/// Goes to menu steps phase1.
	/// Metodo que controla el menu de pasos de la fase 1:
	/// </summary>
	public void GoToMenuStepsPhase1InformativeMode(){
		Debug.LogError ("Llamado al metodo GoToMenuStepsPhase1InformativeMode INFORMATIVE!!");
		
		if (current_interface == CurrentInterface.MENU_PHASES_IM)
			Destroy (menuProcessPhases_int_im_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_CAR_HOOD_IM)
			Destroy (TurorialSearchCapoCarro_int_im_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_BAYETA_PRODUCT_TUTORIAL_IM)
			Destroy (TutorialTwoSearchBayeta_int_im_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_PRODUCTS_TUTORIAL_IM)
			Destroy (TutorialPhaseTwoSearchProd_int_im_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP3_IM)
			Destroy (AR_Mode_Search_int_im_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP4_IM)
			Destroy (AR_Mode_Search_int_im_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP5_IM)
			Destroy (AR_Mode_Search_int_im_instance);
		else if (current_interface == CurrentInterface.ACTIVITIES_PHASE1_STEP1_IM)
			Destroy (ActivitiesForPhase1Step1_int_im_instance);
		else if (current_interface == CurrentInterface.ACTIVITIES_PHASE1_STEP2_IM)
			Destroy (ActivitiesForPhase1Step2_int_im_instance);
		else if (current_interface == CurrentInterface.ACTIVITIES_PHASE1_STEP3_IM)
			Destroy (ActivitiesForPhase1Step3_int_im_instance);
		else if (current_interface == CurrentInterface.ACTIVITIES_PHASE1_STEP4_IM)
			Destroy (ActivitiesForPhase1Step4_int_im_instance);
		else if(current_interface == CurrentInterface.ACTIVITIES_PHASE1_STEP5_IM)
			Destroy (ActivitiesForPhase1Step5_int_im_instance);
		else if(current_interface == CurrentInterface.ACTIVITIES_PHASE1_STEP6_IM)
			Destroy (ActivitiesForPhase1Step6_int_im_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP4_IM)
			Destroy (AR_Mode_Search_int_im_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP6_IM)
			Destroy (AR_Mode_Search_int_im_instance);

					
		current_interface = CurrentInterface.MENU_STEPS_PHASE1_IM;
		
		//Llamado al metodo para destruir instancias existentes de esta interfaz para evitar duplicidad:
		//esto se hace antes de instanciar la nueva interfaz mas adelante
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhase1");
		
		menuStepsPhase1_int_im_instance = Instantiate (AppManager.manager.menuStepsPhase1);
		//Es importante asignarle un nombre a la interfaz para despues poder destruir todas las instancias 
		//de esa interfaz:
		menuStepsPhase1_int_im_instance.name = "InterfaceMenuOfStepsPhase1";
		//Obteniendo referencia al script
		MenuOfStepsPhase1Manager cMenuStepsPhase1Manager = menuStepsPhase1_int_im_instance.GetComponent<MenuOfStepsPhase1Manager> ();
		Debug.Log ("El titulo de la interfaz desde el AppManager es: " + menuStepsPhase1_interface_title);
		cMenuStepsPhase1Manager.titulo = menuStepsPhase1_interface_title;
		cMenuStepsPhase1Manager.introduction_text_path = menuStepsPhase1_introduction_text_path;
		cMenuStepsPhase1Manager.image_header_phase1 = menuStepsPhase1_image_header;
		//asignando imagenes:
		cMenuStepsPhase1Manager.image_uno_capo_carro = this.menuStepsPhase1_interface_button_uno_image;
		cMenuStepsPhase1Manager.image_dos_limpieza = this.menuStepsPhase1_interface_button_dos_image;
		cMenuStepsPhase1Manager.image_tres_secado = this.menuStepsPhase1_interface_button_tres_image;
		cMenuStepsPhase1Manager.image_cuatro_irregularidades = this.menuStepsPhase1_interface_button_cuatro_image;
		cMenuStepsPhase1Manager.image_cinco_corregir = this.menuStepsPhase1_interface_button_cinco_image;
		cMenuStepsPhase1Manager.image_seis_desengrasar = this.menuStepsPhase1_interface_button_seis_image;
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
		
		cMenuStepsPhase1Manager.goBackToMenuPhases += GoToMenuPhasesInformativeMode;

		cMenuStepsPhase1Manager.goToActivitiesPhase1Step1 += GoToActivitiesPhase1Step1Informative;
		cMenuStepsPhase1Manager.goToActivitiesPhase1Step2 += GoToActivitiesPhase1Step2Informative;
		cMenuStepsPhase1Manager.goToActivitiesPhase1Step3 += GoToActivitiesPhase1Step3Informative;
		cMenuStepsPhase1Manager.goToActivitiesPhase1Step4 += GoToActivitiesPhase1Step4Informative;
		cMenuStepsPhase1Manager.goToActivitiesPhase1Step5 += GoToActivitiesPhase1Step5Informative;
		cMenuStepsPhase1Manager.goToActivitiesPhase1Step6 += GoToActivitiesPhase1Step6Informative;

		//Activando los pasos de la fase 1 completamente:
		cMenuStepsPhase1Manager.step_one_enabled = true;
		cMenuStepsPhase1Manager.step_two_enabled = true;
		cMenuStepsPhase1Manager.step_three_enabled = true;
		cMenuStepsPhase1Manager.step_four_enabled = true;
		cMenuStepsPhase1Manager.step_five_enabled = true;
		cMenuStepsPhase1Manager.step_six_enabled = true;

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "II2", "0", "-1","consulta");
				
		//cMenuStepsPhase1Manager.LoadInformationIntoInterface ();

	} //cierra metodo GoToMenuStepsPhase1InformativeMode


	//Metodo que instancia la interfaz del Phase1 - Step 1 (buscar capo del carro)
	public void GoToActivitiesPhase1Step1Informative(){

		Debug.Log ("InformativeMode: Llamado al metodo GoToActivitiesPhase1Step1Informative - Capo del Carro");

		if (current_interface == CurrentInterface.MENU_STEPS_PHASE1_IM)
			Destroy (menuStepsPhase1_int_im_instance);
		 else if (current_interface == CurrentInterface.AR_SEARCH_CAR_HOOD_IM)
			Destroy (TurorialSearchCapoCarro_int_im_instance);
			
		
		current_interface = CurrentInterface.ACTIVITIES_PHASE1_STEP1_IM;
		//Ojo con lo siguiente estoy destruyendo todas las instancias que hayan de la interfaz de actividades por cada paso
		DestroyInstancesWithTag ("ActivitiesForEachStep");
		
		if (AppManager.manager.BuscarCapoCarro == null)
			Debug.LogError ("BuscarCapoCarro no esta definido en el AppManager invocando desde InformativeMode!!");
		
			
		ActivitiesForPhase1Step1_int_im_instance = Instantiate (AppManager.manager.BuscarCapoCarro);
		CanvasBuscarCapoCocheManager cBuscarCapoCoche = ActivitiesForPhase1Step1_int_im_instance.GetComponent<CanvasBuscarCapoCocheManager> ();
		cBuscarCapoCoche.image_header_buscar_capo = image_buscar_capo_path;
		cBuscarCapoCoche.image_content_capo_carro_marker = image_content_marker;
		cBuscarCapoCoche.titulo_buscar_capo_carro = title_phase1_step1;
		cBuscarCapoCoche.introduction_text_path_1 = introduction_text_phase1Step1_path_one;
		cBuscarCapoCoche.introduction_text_path_2 = introduction_text_phase1Step1_path_two;
		cBuscarCapoCoche.goBackToMenuActivities += GoToMenuStepsPhase1InformativeMode;
		cBuscarCapoCoche.goToSearchCapoCarro += GoToSearchCapoCocheInformative;
		cBuscarCapoCoche.text_btn_continuar = "Buscar"; //asignando el texto que se debe mostrar en el boton
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "II4", "1", "-1","consulta");
	}

	/// <summary>
	/// Goes to activities phase1 step2.
	/// </summary>
	public void GoToActivitiesPhase1Step2Informative(){
		
		if (current_interface == CurrentInterface.MENU_STEPS_PHASE1_IM)
			Destroy (menuStepsPhase1_int_im_instance);
		 else if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_IM)
			Destroy (ToolsAndProductsPhase1Step2_int_im_instance);
			
		
		current_interface = CurrentInterface.ACTIVITIES_PHASE1_STEP2_IM;
		
		//Ojo con lo siguiente estoy destruyendo todas las instancias que hayan de la interfaz de actividades por cada paso
		DestroyInstancesWithTag ("ActivitiesForEachStep");
		
		ActivitiesForPhase1Step2_int_im_instance = Instantiate (AppManager.manager.ActivitiesForEachStep);
		CanvasActivitiesForEachStepHeaders cActivitiesForStep = ActivitiesForPhase1Step2_int_im_instance.GetComponent<CanvasActivitiesForEachStepHeaders> ();
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
		cActivitiesForStep.image_tres_self_assessment = image_tres_self_assessment_gray;
		cActivitiesForStep.image_cuatro_simulations = image_cuatro_fotos_gray;
		cActivitiesForStep.image_cinco_personal_notes = image_cinco_personal_notes_gray;
		cActivitiesForStep.image_seis_frequently_questions = image_seis_frequently_questions_gray;
		cActivitiesForStep.image_siete_ask_your_teacher = image_siete_ask_your_teacher_gray;
		cActivitiesForStep.goToToolsAndProd += GoToToolsAndProductsInfoMode;
		cActivitiesForStep.interfaceCallingGoToTools = "Phase1Step2";
		cActivitiesForStep.goToMenuSteps += GoBackToMenuOfStepsFromActivInformative;
		cActivitiesForStep.interfaceGoingBackFrom = "Phase1Step2";
		cActivitiesForStep.goToVideosForStep += GoToVideoOfEachStepInformativeMode;

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "II5", "1", "-1","consulta");
		
	}

	/// <summary>
	/// Goes to activities phase1 step3.
	/// </summary>
	public void GoToActivitiesPhase1Step3Informative(){
		
		if (current_interface == CurrentInterface.MENU_STEPS_PHASE1_IM)
			Destroy (menuStepsPhase1_int_im_instance);
		else if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_IM)
			Destroy (ToolsAndProductsPhase1Step3_int_im_instance); 
		
		current_interface = CurrentInterface.ACTIVITIES_PHASE1_STEP3_IM;
		
		//Ojo con lo siguiente estoy destruyendo todas las instancias que hayan de la interfaz de actividades por cada paso
		DestroyInstancesWithTag ("ActivitiesForEachStep");
		
		ActivitiesForPhase1Step3_int_im_instance = Instantiate (AppManager.manager.ActivitiesForEachStep);
		CanvasActivitiesForEachStepHeaders cActivitiesForStep = ActivitiesForPhase1Step3_int_im_instance.GetComponent<CanvasActivitiesForEachStepHeaders> ();
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
		cActivitiesForStep.image_tres_self_assessment = image_tres_self_assessment_gray;
		cActivitiesForStep.image_cuatro_simulations = image_cuatro_fotos_gray;
		cActivitiesForStep.image_cinco_personal_notes = image_cinco_personal_notes_gray;
		cActivitiesForStep.image_seis_frequently_questions = image_seis_frequently_questions_gray;
		cActivitiesForStep.image_siete_ask_your_teacher = image_siete_ask_your_teacher_gray;
		cActivitiesForStep.goToToolsAndProd += GoToToolsAndProductsInfoMode;
		cActivitiesForStep.interfaceCallingGoToTools = "Phase1Step3";
		cActivitiesForStep.goToMenuSteps += GoBackToMenuOfStepsFromActivInformative;
		cActivitiesForStep.interfaceGoingBackFrom = "Phase1Step3";
		cActivitiesForStep.goToVideosForStep += GoToVideoOfEachStepInformativeMode;

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "II6", "2", "-1","consulta");
	} //cierra menu of activities phase1step3


	/// <summary>
	/// Goes to activities phase1 step4.
	/// </summary>
	public void GoToActivitiesPhase1Step4Informative(){
		
		if (current_interface == CurrentInterface.MENU_STEPS_PHASE1_IM)
			Destroy (menuStepsPhase1_int_im_instance);
		else if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_IM)
			Destroy (ToolsAndProductsPhase1Step4_int_im_instance);
		
		current_interface = CurrentInterface.ACTIVITIES_PHASE1_STEP4_IM;
		
		//Ojo con lo siguiente estoy destruyendo todas las instancias que hayan de la interfaz de actividades por cada paso
		DestroyInstancesWithTag ("ActivitiesForEachStep");
		
		ActivitiesForPhase1Step4_int_im_instance = Instantiate (AppManager.manager.ActivitiesForEachStep);
		CanvasActivitiesForEachStepHeaders cActivitiesForStep = ActivitiesForPhase1Step4_int_im_instance.GetComponent<CanvasActivitiesForEachStepHeaders> ();
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
		cActivitiesForStep.image_tres_self_assessment = image_tres_self_assessment_gray;
		cActivitiesForStep.image_cuatro_simulations = image_cuatro_fotos_gray;
		cActivitiesForStep.image_cinco_personal_notes = image_cinco_personal_notes_gray;
		cActivitiesForStep.image_seis_frequently_questions = image_seis_frequently_questions_gray;
		cActivitiesForStep.image_siete_ask_your_teacher = image_siete_ask_your_teacher_gray;
		cActivitiesForStep.goToToolsAndProd += GoToToolsAndProductsInfoMode;
		cActivitiesForStep.interfaceCallingGoToTools = "Phase1Step4";
		cActivitiesForStep.goToMenuSteps += GoBackToMenuOfStepsFromActivInformative;
		cActivitiesForStep.interfaceGoingBackFrom = "Phase1Step4";
		cActivitiesForStep.goToVideosForStep += GoToVideoOfEachStepInformativeMode;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "II7", "3", "-1","consulta");
	}//cierra GoToActivitiesPhase1Step4InfoMode

	/// <summary>
	/// Goes to activities phase1 step5.
	/// </summary>
	public void GoToActivitiesPhase1Step5Informative(){
		
		if (current_interface == CurrentInterface.MENU_STEPS_PHASE1_IM)
			Destroy (menuStepsPhase1_int_im_instance);
		else if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_IM)
			Destroy (ToolsAndProductsPhase1Step5_int_im_instance); 
		
		current_interface = CurrentInterface.ACTIVITIES_PHASE1_STEP5_IM;
		
		//Ojo con lo siguiente estoy destruyendo todas las instancias que hayan de la interfaz de actividades por cada paso
		DestroyInstancesWithTag ("ActivitiesForEachStep");
		
		ActivitiesForPhase1Step5_int_im_instance = Instantiate (AppManager.manager.ActivitiesForEachStep);
		CanvasActivitiesForEachStepHeaders cActivitiesForStep = ActivitiesForPhase1Step5_int_im_instance.GetComponent<CanvasActivitiesForEachStepHeaders> ();
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
		cActivitiesForStep.image_tres_self_assessment = image_tres_self_assessment_gray;
		cActivitiesForStep.image_cuatro_simulations = image_cuatro_fotos_gray;
		cActivitiesForStep.image_cinco_personal_notes = image_cinco_personal_notes_gray;
		cActivitiesForStep.image_seis_frequently_questions = image_seis_frequently_questions_gray;
		cActivitiesForStep.image_siete_ask_your_teacher = image_siete_ask_your_teacher_gray;
		cActivitiesForStep.goToToolsAndProd += GoToToolsAndProductsInfoMode;
		cActivitiesForStep.interfaceCallingGoToTools = "Phase1Step5";
		cActivitiesForStep.goToMenuSteps += GoBackToMenuOfStepsFromActivInformative;
		cActivitiesForStep.interfaceGoingBackFrom = "Phase1Step5";
		cActivitiesForStep.goToVideosForStep += GoToVideoOfEachStepInformativeMode;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "II8", "4", "-1","consulta");
	}//cierra GoToActivitiesPhase1Step5


	/// <summary>
	/// Goes to activities phase1 step6.
	/// </summary>
	public void GoToActivitiesPhase1Step6Informative(){
		
		if (current_interface == CurrentInterface.MENU_STEPS_PHASE1_IM)
			Destroy (menuStepsPhase1_int_im_instance );
		else if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_IM)
			Destroy (ToolsAndProductsPhase1Step6_int_im_instance); 
		
		current_interface = CurrentInterface.ACTIVITIES_PHASE1_STEP6_IM;
		
		//Ojo con lo siguiente estoy destruyendo todas las instancias que hayan de la interfaz de actividades por cada paso
		DestroyInstancesWithTag ("ActivitiesForEachStep");
		
		ActivitiesForPhase1Step6_int_im_instance = Instantiate (AppManager.manager.ActivitiesForEachStep);
		CanvasActivitiesForEachStepHeaders cActivitiesForStep = ActivitiesForPhase1Step6_int_im_instance.GetComponent<CanvasActivitiesForEachStepHeaders> ();
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
		cActivitiesForStep.image_tres_self_assessment = image_tres_self_assessment_gray;
		cActivitiesForStep.image_cuatro_simulations = image_cuatro_fotos;
		cActivitiesForStep.image_cinco_personal_notes = image_cinco_personal_notes_gray;
		cActivitiesForStep.image_seis_frequently_questions = image_seis_frequently_questions_gray;
		cActivitiesForStep.image_siete_ask_your_teacher = image_siete_ask_your_teacher_gray;
		cActivitiesForStep.goToToolsAndProd += GoToToolsAndProductsInfoMode;
		cActivitiesForStep.interfaceCallingGoToTools = "Phase1Step6";
		cActivitiesForStep.goToMenuSteps += GoBackToMenuOfStepsFromActivInformative;
		cActivitiesForStep.interfaceGoingBackFrom = "Phase1Step6";
		cActivitiesForStep.goToVideosForStep += GoToVideoOfEachStepInformativeMode;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "II9", "5", "-1","consulta");
	}//cierra GoToActivitiesPhase1Step6

	
	/// <summary>
	/// Gos to menu steps phase1.
	/// Method that is called for loading the interface of steps for phase 2 (MATIZADO):
	/// </summary>
	public void GoToMenuStepsPhase2InformativeMode(){
		Debug.LogError ("Llamado al metodo go to Menu steps phase 2 - ModoInformativo");

		if (current_interface == CurrentInterface.MENU_PHASES_IM)
			Destroy (menuProcessPhases_int_im_instance);
		else if (current_interface == CurrentInterface.MENU_SUB_STEPS_PHASE2_IM)
			Destroy (menuSubStepsPhaseTwo_int_im_instance);
		else if (current_interface == CurrentInterface.MENU_SUB_STEPS_INTERIORES_PHASE2_IM)
			Destroy (menuSubStepsPhaseTwoInterior_int_im_instance);
		else if (current_interface == CurrentInterface.ACTIVITIES_PHASE2_STEP1_IM)
			Destroy (ActivitiesForPhase2Step1_int_im_instance);
		else if (current_interface == CurrentInterface.ACTIVITIES_PHASE2_STEP2_IM)
			Destroy (ActivitiesForPhase2Step2_int_im_instance);
		/*
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
		*/
						
		current_interface = CurrentInterface.MENU_STEPS_PHASE2_IM;
		
		//Llamado al metodo para destruir instancias existentes de esta interfaz para evitar duplicidad:
		//esto se hace antes de instanciar la nueva interfaz mas adelante
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhase2");
		DestroyInstancesWithTag ("BlinkingTutorialPhaseOne");
		
		menuStepsPhaseTwo_int_im_instance = Instantiate (AppManager.manager.menuStepsPhase2);
		//Es importante asignarle un nombre a la interfaz para despues poder destruir todas las instancias 
		//de esa interfaz:
		menuStepsPhaseTwo_int_im_instance.name = "InterfaceMenuOfStepsPhase2InfoMode";
		//Obteniendo referencia al script
		MenuOfStepsMatizadoManager cMenuStepsMatizadoManager = menuStepsPhaseTwo_int_im_instance.GetComponent<MenuOfStepsMatizadoManager> ();
		Debug.Log ("El titulo de la interfaz desde el AppManager es: " + menuStepsPhaseTwo_interface_title);
		cMenuStepsMatizadoManager.titulo = menuStepsPhaseTwo_interface_title;
		cMenuStepsMatizadoManager.introduction_text_path = menuStepsPhaseTwo_introduction_text_path;
		cMenuStepsMatizadoManager.image_header_phase1 = menuStepsPhaseTwo_image_header;
		
		//definiendo cuales botones de los pasos se deben habilitar dependiendo de si el estudiante ya ha completado
		//las actividades anteriores:
		//por defecto el paso 1 se habilita inicialmente y con la imagen normal:
		cMenuStepsMatizadoManager.step_one_enabled = true;
		cMenuStepsMatizadoManager.image_one_path = this.menuStepsPhaseTwo_interface_button_uno_image;
		
		//Aqui se indica a la interfaz cuales botones deben ser visibles:
		cMenuStepsMatizadoManager.step_one_btn_visible = true;
		cMenuStepsMatizadoManager.step_two_btn_visible = true;
		cMenuStepsMatizadoManager.step_three_btn_visible = true;
		cMenuStepsMatizadoManager.step_four_btn_visible = true;
		cMenuStepsMatizadoManager.step_five_btn_visible = false;
		cMenuStepsMatizadoManager.step_six_btn_visible = false;
		
		
		cMenuStepsMatizadoManager.step_two_enabled = true;
		cMenuStepsMatizadoManager.image_two_path = this.menuStepsPhaseTwo_interface_button_dos_image;
			
		cMenuStepsMatizadoManager.step_three_enabled = true;
		cMenuStepsMatizadoManager.image_three_path = this.menuStepsPhaseTwo_interface_button_tres_image;

		cMenuStepsMatizadoManager.step_four_enabled = true;
		cMenuStepsMatizadoManager.image_four_path = this.menuStepsPhaseTwo_interface_button_cuatro_image;

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
		
		cMenuStepsMatizadoManager.goBackToMenuPhases += GoToMenuPhasesInformativeMode;
		cMenuStepsMatizadoManager.goToActionButtoOne += GoToActivitiesPhase2Step1Informative;
		cMenuStepsMatizadoManager.goToActionButtoTwo += GoToActivitiesPhase2Step2Informative;
		cMenuStepsMatizadoManager.goToActionButtoThree += GoToSubMenuStepsLijadoCantosInfoMode;
		cMenuStepsMatizadoManager.goToActionButtoFour += GoToSubMenuStepsLijadoInterioresInfoMode;

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "II3", "0", "-1","consulta");
		
	} //cierra GoToMenuStepsPhase2


	/// <summary>
	/// Gos to sub menu steps lijado cantos. Metodo que configura la interfaz del sub-menu del lijado de cantos:
	/// </summary>
	public void GoToSubMenuStepsLijadoCantosInfoMode(){
		Debug.LogError ("Llamado al metodo go to SubMenu LijadoCantos phase 2");
		
		if (current_interface == CurrentInterface.MENU_STEPS_PHASE2_IM)
			Destroy (menuStepsPhaseTwo_int_im_instance);
		else if (current_interface == CurrentInterface.ACTIVITIES_PHASE2_STEP3_IM)
			Destroy (ActivitiesForPhase2Step3_int_im_instance);
		else if (current_interface == CurrentInterface.ACTIVITIES_PHASE2_STEP4_IM)
			Destroy (ActivitiesForPhase2Step4_int_im_instance);
		
		current_interface = CurrentInterface.MENU_SUB_STEPS_PHASE2_IM;
		
		//Llamado al metodo para destruir instancias existentes de esta interfaz para evitar duplicidad:
		//esto se hace antes de instanciar la nueva interfaz mas adelante
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhase2");
		//DestroyInstancesWithTag ("BlinkingTutorialPhaseOne");
		
		menuSubStepsPhaseTwo_int_im_instance = Instantiate (AppManager.manager.menuSubStepsPhase2);
		//Es importante asignarle un nombre a la interfaz para despues poder destruir todas las instancias 
		//de esa interfaz:
		menuSubStepsPhaseTwo_int_im_instance.name = "InterfaceMenuOfSubForStepsPhase2";
		//Obteniendo referencia al script
		MenuOfStepsMatizadoSubMenu cMenuSubStepsMatizado = menuSubStepsPhaseTwo_int_im_instance.GetComponent<MenuOfStepsMatizadoSubMenu> ();
		Debug.Log ("El titulo de la interfaz desde el AppManager es: " + menuStepsPhaseTwo_interface_title);
		cMenuSubStepsMatizado.titulo = menuSubStepsPhaseTwo_interface_title;
				
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
		

		cMenuSubStepsMatizado.step_one_enabled = true; //ojo aca SI es el btn one porque es el primer boton del submenu
		cMenuSubStepsMatizado.image_one_path = this.menuSubStepsPhaseTwo_int_button_uno_image;
				

		cMenuSubStepsMatizado.step_two_enabled = true; //ojo aca SI es el btn one porque es el primer boton del submenu
		cMenuSubStepsMatizado.image_two_path = this.menuSubStepsPhaseTwo_int_button_dos_image;

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
		cMenuSubStepsMatizado.goBackToMenuPhases += GoToMenuStepsPhase2InformativeMode;
		cMenuSubStepsMatizado.goToActivitiesPhase1Step1 += GoToActivitiesPhase2Step3Informative;
		cMenuSubStepsMatizado.goToActivitiesPhase1Step2 += GoToActivitiesPhase2Step4Informative;
		
		
	} //cierra GoToSubMenuStepsLijadoCantos
	
	/// <summary>
	/// Gos to sub menu steps lijado cantos. Metodo que configura el sub-menu de pasos del lijado de interiores
	/// </summary>
	public void GoToSubMenuStepsLijadoInterioresInfoMode(){
		Debug.LogError ("Llamado al metodo go to SubMenu LijadoInteriores phase 2");
		
		if (current_interface == CurrentInterface.MENU_STEPS_PHASE2_IM)
			Destroy (menuStepsPhaseTwo_int_im_instance);
		else if (current_interface == CurrentInterface.ACTIVITIES_PHASE2_STEP5_IM)
			Destroy (ActivitiesForPhase2Step5_int_im_instance);
		else if (current_interface == CurrentInterface.ACTIVITIES_PHASE2_STEP6_IM)
			Destroy (ActivitiesForPhase2Step6_int_im_instance);
		else if (current_interface == CurrentInterface.ACTIVITIES_PHASE2_STEP7_IM)
			Destroy (ActivitiesForPhase2Step7_int_im_instance);
		else if (current_interface == CurrentInterface.ACTIVITIES_PHASE2_STEP8_IM)
			Destroy (ActivitiesForPhase2Step8_int_im_instance);

		
		current_interface = CurrentInterface.MENU_SUB_STEPS_INTERIORES_PHASE2_IM;
		
		//Llamado al metodo para destruir instancias existentes de esta interfaz para evitar duplicidad:
		//esto se hace antes de instanciar la nueva interfaz mas adelante
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhase2");
		//DestroyInstancesWithTag ("BlinkingTutorialPhaseOne");
		
		menuSubStepsPhaseTwoInterior_int_im_instance = Instantiate (AppManager.manager.menuSubStepsPhase2);
		//Es importante asignarle un nombre a la interfaz para despues poder destruir todas las instancias 
		//de esa interfaz:
		menuSubStepsPhaseTwoInterior_int_im_instance.name = "InterfaceMenuOfSubForStepsPhase2_InterioresInfoMode";
		//Obteniendo referencia al script
		MenuOfStepsMatizadoSubMenu cMenuSubStepsMatizado = menuSubStepsPhaseTwoInterior_int_im_instance.GetComponent<MenuOfStepsMatizadoSubMenu> ();
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
		

		cMenuSubStepsMatizado.step_one_enabled = true; //ojo aca SI es el btn one porque es el primer boton del submenu
		cMenuSubStepsMatizado.image_one_path = this.menuSubStepsP2_int_btn_uno_image;

		cMenuSubStepsMatizado.step_two_enabled = true; //ojo aca SI es el btn one porque es el primer boton del submenu
		cMenuSubStepsMatizado.image_two_path = this.menuSubStepsP2_int_btn_dos_image;

		cMenuSubStepsMatizado.step_three_enabled = true; //ojo aca SI es el btn one porque es el primer boton del submenu
		cMenuSubStepsMatizado.image_three_path = this.menuSubStepsP2_int_btn_tres_image;

		cMenuSubStepsMatizado.step_four_enabled = true; //ojo aca SI es el btn one porque es el primer boton del submenu
		cMenuSubStepsMatizado.image_four_path = this.menuSubStepsP2_int_btn_cuatro_image;

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
		cMenuSubStepsMatizado.goBackToMenuPhases += GoToMenuStepsPhase2InformativeMode;
		cMenuSubStepsMatizado.goToActivitiesPhase1Step1 += GoToActivitiesPhase2Step5Informative;
		cMenuSubStepsMatizado.goToActivitiesPhase1Step2 += GoToActivitiesPhase2Step6Informative;
		cMenuSubStepsMatizado.goToActivitiesPhase1Step3 += GoToActivitiesPhase2Step7Informative;
		cMenuSubStepsMatizado.goToActivitiesPhase1Step4 += GoToActivitiesPhase2Step8Informative;
			
	} //cierra GoToSubMenuStepsLijadoInteriores


	//Metodo que instancia la interfaz del Phase2 - Step 1 (Introduccion al Matizado)
	public void GoToActivitiesPhase2Step1Informative(){
		
		if (current_interface == CurrentInterface.MENU_STEPS_PHASE2_IM)
			Destroy (menuStepsPhaseTwo_int_im_instance);
		
		
		current_interface = CurrentInterface.ACTIVITIES_PHASE2_STEP1_IM;
		//Ojo con lo siguiente estoy destruyendo todas las instancias que hayan de la interfaz de actividades por cada paso
		//DestroyInstancesWithTag ("ActivitiesForEachStep");
		
		if (AppManager.manager.BuscarCapoCarro == null)
			Debug.LogError ("GoToActivitiesPhase2Step1Informative: BuscarCapoCarro no esta definido para ser usado como interfaz de introduccion al matizado en el AppManager!!");
		
		ActivitiesForPhase2Step1_int_im_instance = Instantiate (AppManager.manager.BuscarCapoCarro);
		CanvasBuscarCapoCocheManager cBuscarCapoCoche = ActivitiesForPhase2Step1_int_im_instance.GetComponent<CanvasBuscarCapoCocheManager> ();
		cBuscarCapoCoche.image_header_buscar_capo = image_intro_matizado_header_path;
		cBuscarCapoCoche.image_content_capo_carro_marker = image_intro_matizado_header_path;
		cBuscarCapoCoche.titulo_buscar_capo_carro = title_phase2_step1;
		cBuscarCapoCoche.introduction_text_path_1 = introduction_text_phase2Step1_path_one;
		cBuscarCapoCoche.introduction_text_path_2 = introduction_text_phase2Step1_path_two;
		cBuscarCapoCoche.text_btn_continuar = "Continuar"; //texto que se mustra en el btn de la parte inferior de la interfaz
		cBuscarCapoCoche.goBackToMenuActivities += GoToMenuStepsPhase2InformativeMode;
		cBuscarCapoCoche.goToSearchCapoCarro += GoToMenuStepsPhase2InformativeMode; //Aunque dice goToSearchCapoCarro realmente este metodo se ejecuta cuando se hace click sobre el boton

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "II10", "0", "-1","consulta");

	} //cierra metodo GoToActivitiesPhase2Step1Informative

	/// <summary>
	/// Goes to activities phase2 step2. (Actividades de Proteccion de la Superficie (Fase Matizado))
	/// </summary>
	public void GoToActivitiesPhase2Step2Informative(){
		
		if (current_interface == CurrentInterface.MENU_STEPS_PHASE2_IM)
			Destroy (menuStepsPhaseTwo_int_im_instance);
		else if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_IM)
			Destroy (ToolsAndProductsPhase2Step2_int_im_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP2_IM)
			Destroy (AR_Mode_Search_int_im_instance);
		
		
		current_interface = CurrentInterface.ACTIVITIES_PHASE2_STEP2_IM;
		
		//Ojo con lo siguiente estoy destruyendo todas las instancias que hayan de la interfaz de actividades por cada paso
		DestroyInstancesWithTag ("ActivitiesForEachStep");
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");
		
		ActivitiesForPhase2Step2_int_im_instance = Instantiate (AppManager.manager.ActivitiesForEachStep);
		CanvasActivitiesForEachStepHeaders cActivitiesForStep = ActivitiesForPhase2Step2_int_im_instance.GetComponent<CanvasActivitiesForEachStepHeaders> ();
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
		cActivitiesForStep.image_tres_self_assessment = image_tres_self_assessment_gray;
		cActivitiesForStep.image_cuatro_simulations = image_cuatro_fotos_gray;
		cActivitiesForStep.image_cinco_personal_notes = image_cinco_personal_notes_gray;
		cActivitiesForStep.image_seis_frequently_questions = image_seis_frequently_questions_gray;
		cActivitiesForStep.image_siete_ask_your_teacher = image_siete_ask_your_teacher_gray;
		cActivitiesForStep.goToToolsAndProd += GoToToolsAndProductsInfoMode;
		cActivitiesForStep.interfaceCallingGoToTools = "Phase2Step2";
		cActivitiesForStep.goToMenuSteps += GoBackToMenuOfStepsFromActivInformative;
		cActivitiesForStep.interfaceGoingBackFrom = "Phase2Step2";
		cActivitiesForStep.goToVideosForStep += GoToVideoOfEachStepInformativeMode;

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "II11", "7", "-1","consulta");

	}//cierra GoToActivitiesPhase2Step2
	
	
	/// <summary>
	/// Goes to activities phase2 step3. (Actividades de Matizado de Cantos Primera pasada(Fase Matizado))
	/// </summary>
	public void GoToActivitiesPhase2Step3Informative(){
		
		if (current_interface == CurrentInterface.MENU_SUB_STEPS_PHASE2_IM)
			Destroy (menuStepsPhaseTwo_int_im_instance);
		else if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_IM)
			Destroy (ToolsAndProductsPhase2Step3_int_im_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP3_IM)
			Destroy (AR_Mode_Search_int_im_instance);
		
		
		current_interface = CurrentInterface.ACTIVITIES_PHASE2_STEP3_IM;
		
		//Ojo con lo siguiente estoy destruyendo todas las instancias que hayan de la interfaz de actividades por cada paso
		DestroyInstancesWithTag ("ActivitiesForEachStep");
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");
		
		ActivitiesForPhase2Step3_int_im_instance = Instantiate (AppManager.manager.ActivitiesForEachStep);
		CanvasActivitiesForEachStepHeaders cActivitiesForStep = ActivitiesForPhase2Step3_int_im_instance.GetComponent<CanvasActivitiesForEachStepHeaders> ();
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
		cActivitiesForStep.image_tres_self_assessment = image_tres_self_assessment_gray;
		cActivitiesForStep.image_cuatro_simulations = image_cuatro_fotos_gray;
		cActivitiesForStep.image_cinco_personal_notes = image_cinco_personal_notes_gray;
		cActivitiesForStep.image_seis_frequently_questions = image_seis_frequently_questions_gray;
		cActivitiesForStep.image_siete_ask_your_teacher = image_siete_ask_your_teacher_gray;
		cActivitiesForStep.goToToolsAndProd += GoToToolsAndProductsInfoMode;
		cActivitiesForStep.interfaceCallingGoToTools = "Phase2Step3";
		cActivitiesForStep.goToMenuSteps += GoBackToMenuOfStepsFromActivInformative;
		cActivitiesForStep.interfaceGoingBackFrom = "Phase2Step3";
		cActivitiesForStep.goToVideosForStep += GoToVideoOfEachStepInformativeMode;

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "II12", "9", "-1","consulta");

		
	}//cierra GoToActivitiesPhase2Step3
	
	
	/// <summary>
	/// Goes to activities phase2 step4. (Actividades de Matizado de Cantos Pasada Final(Fase Matizado))
	/// </summary>
	public void GoToActivitiesPhase2Step4Informative(){
		
		if (current_interface == CurrentInterface.MENU_SUB_STEPS_INTERIORES_PHASE2_IM)
			Destroy (menuSubStepsPhaseTwo_int_im_instance);
		else if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_IM)
			Destroy (ToolsAndProductsPhase2Step4_int_im_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP4_IM)
			Destroy (AR_Mode_Search_int_im_instance);
		
		
		current_interface = CurrentInterface.ACTIVITIES_PHASE2_STEP4_IM;
		
		//Ojo con lo siguiente estoy destruyendo todas las instancias que hayan de la interfaz de actividades por cada paso
		DestroyInstancesWithTag ("ActivitiesForEachStep");
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");
		
		ActivitiesForPhase2Step4_int_im_instance = Instantiate (AppManager.manager.ActivitiesForEachStep);
		CanvasActivitiesForEachStepHeaders cActivitiesForStep = ActivitiesForPhase2Step4_int_im_instance.GetComponent<CanvasActivitiesForEachStepHeaders> ();
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
		cActivitiesForStep.image_step_three_header = image_phase2step3_with_text;
		cActivitiesForStep.image_step_four_header = image_phase2step4_with_text_gray;
		cActivitiesForStep.image_step_five_header = image_phase2step5_with_text_gray;
		cActivitiesForStep.image_step_six_header = image_phase2step6_with_text_gray;
		
		//agregando las imagenes a los botones 
		cActivitiesForStep.image_uno_tools_and_products = image_uno_tools_and_products;
		cActivitiesForStep.image_dos_videos = image_dos_videos;
		cActivitiesForStep.image_tres_self_assessment = image_tres_self_assessment_gray;
		cActivitiesForStep.image_cuatro_simulations = image_cuatro_fotos_gray;
		cActivitiesForStep.image_cinco_personal_notes = image_cinco_personal_notes_gray;
		cActivitiesForStep.image_seis_frequently_questions = image_seis_frequently_questions_gray;
		cActivitiesForStep.image_siete_ask_your_teacher = image_siete_ask_your_teacher_gray;
		cActivitiesForStep.goToToolsAndProd += GoToToolsAndProductsInfoMode;
		cActivitiesForStep.interfaceCallingGoToTools = "Phase2Step4";
		cActivitiesForStep.goToMenuSteps += GoBackToMenuOfStepsFromActivInformative;
		cActivitiesForStep.interfaceGoingBackFrom = "Phase2Step4";
		cActivitiesForStep.goToVideosForStep += GoToVideoOfEachStepInformativeMode;

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "II13", "10", "-1","consulta");
		
	}//cierra GoToActivitiesPhase2Step4Informative
		
	
	/// <summary>
	/// Goes to activities phase2 step5. (Actividades de Matizado de interiores Primera pasada(Fase Matizado))
	/// </summary>
	public void GoToActivitiesPhase2Step5Informative(){
		
		if (current_interface == CurrentInterface.MENU_SUB_STEPS_INTERIORES_PHASE2_IM)
			Destroy (menuSubStepsPhaseTwoInterior_int_im_instance);
		else if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_IM)
			Destroy (ToolsAndProductsPhase2Step5_int_im_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP5_IM)
			Destroy (AR_Mode_Search_int_im_instance);
		
		
		current_interface = CurrentInterface.ACTIVITIES_PHASE2_STEP5_IM;
		
		//Ojo con lo siguiente estoy destruyendo todas las instancias que hayan de la interfaz de actividades por cada paso
		DestroyInstancesWithTag ("ActivitiesForEachStep");
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");
		
		ActivitiesForPhase2Step5_int_im_instance = Instantiate (AppManager.manager.ActivitiesForEachStep);
		CanvasActivitiesForEachStepHeaders cActivitiesForStep = ActivitiesForPhase2Step5_int_im_instance.GetComponent<CanvasActivitiesForEachStepHeaders> ();
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
		cActivitiesForStep.image_tres_self_assessment = image_tres_self_assessment_gray;
		cActivitiesForStep.image_cuatro_simulations = image_cuatro_fotos_gray;
		cActivitiesForStep.image_cinco_personal_notes = image_cinco_personal_notes_gray;
		cActivitiesForStep.image_seis_frequently_questions = image_seis_frequently_questions_gray;
		cActivitiesForStep.image_siete_ask_your_teacher = image_siete_ask_your_teacher_gray;
		cActivitiesForStep.goToToolsAndProd += GoToToolsAndProductsInfoMode;
		cActivitiesForStep.interfaceCallingGoToTools = "Phase2Step5";
		cActivitiesForStep.goToMenuSteps += GoBackToMenuOfStepsFromActivInformative;
		cActivitiesForStep.interfaceGoingBackFrom = "Phase2Step5";
		cActivitiesForStep.goToVideosForStep += GoToVideoOfEachStepInformativeMode;

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "II14", "11", "-1","consulta");
	}//cierra GoToActivitiesPhase2Step5Informative
	
	
	/// <summary>
	/// Goes to activities phase2 step6. (Actividades de Matizado de interiores Segunda pasada(Fase Matizado))
	/// </summary>
	public void GoToActivitiesPhase2Step6Informative(){
		
		if (current_interface == CurrentInterface.MENU_SUB_STEPS_INTERIORES_PHASE2_IM)
			Destroy (menuSubStepsPhaseTwoInterior_int_im_instance);
		else if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_IM)
			Destroy (ToolsAndProductsPhase2Step6_int_im_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP6_IM)
			Destroy (AR_Mode_Search_int_im_instance);
		
		
		current_interface = CurrentInterface.ACTIVITIES_PHASE2_STEP6_IM;
		
		//Ojo con lo siguiente estoy destruyendo todas las instancias que hayan de la interfaz de actividades por cada paso
		DestroyInstancesWithTag ("ActivitiesForEachStep");
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");
		
		ActivitiesForPhase2Step6_int_im_instance = Instantiate (AppManager.manager.ActivitiesForEachStep);
		CanvasActivitiesForEachStepHeaders cActivitiesForStep = ActivitiesForPhase2Step6_int_im_instance.GetComponent<CanvasActivitiesForEachStepHeaders> ();
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
		cActivitiesForStep.image_tres_self_assessment = image_tres_self_assessment_gray;
		cActivitiesForStep.image_cuatro_simulations = image_cuatro_fotos_gray;
		cActivitiesForStep.image_cinco_personal_notes = image_cinco_personal_notes_gray;
		cActivitiesForStep.image_seis_frequently_questions = image_seis_frequently_questions_gray;
		cActivitiesForStep.image_siete_ask_your_teacher = image_siete_ask_your_teacher_gray;
		cActivitiesForStep.goToToolsAndProd += GoToToolsAndProductsInfoMode;
		cActivitiesForStep.interfaceCallingGoToTools = "Phase2Step6";
		cActivitiesForStep.goToMenuSteps += GoBackToMenuOfStepsFromActivInformative;
		cActivitiesForStep.interfaceGoingBackFrom = "Phase2Step6";
		cActivitiesForStep.goToVideosForStep += GoToVideoOfEachStepInformativeMode;

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "II15", "12", "-1","consulta");
				
	}//cierra GoToActivitiesPhase2Step6Informative
	
	/// <summary>
	/// Goes to activities phase2 step7. (Actividades de Matizado de interiores Tercera pasada(Fase Matizado))
	/// </summary>
	public void GoToActivitiesPhase2Step7Informative(){
		
		if (current_interface == CurrentInterface.MENU_SUB_STEPS_INTERIORES_PHASE2_IM)
			Destroy (menuSubStepsPhaseTwoInterior_int_im_instance);
		else if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_IM)
			Destroy (ToolsAndProductsPhase2Step7_int_im_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP7_IM)
			Destroy (AR_Mode_Search_int_im_instance);
		
		
		current_interface = CurrentInterface.ACTIVITIES_PHASE2_STEP7_IM;
		
		//Ojo con lo siguiente estoy destruyendo todas las instancias que hayan de la interfaz de actividades por cada paso
		DestroyInstancesWithTag ("ActivitiesForEachStep");
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");
		
		ActivitiesForPhase2Step7_int_im_instance = Instantiate (AppManager.manager.ActivitiesForEachStep);
		CanvasActivitiesForEachStepHeaders cActivitiesForStep = ActivitiesForPhase2Step7_int_im_instance.GetComponent<CanvasActivitiesForEachStepHeaders> ();
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
		cActivitiesForStep.image_tres_self_assessment = image_tres_self_assessment_gray;
		cActivitiesForStep.image_cuatro_simulations = image_cuatro_fotos_gray;
		cActivitiesForStep.image_cinco_personal_notes = image_cinco_personal_notes_gray;
		cActivitiesForStep.image_seis_frequently_questions = image_seis_frequently_questions_gray;
		cActivitiesForStep.image_siete_ask_your_teacher = image_siete_ask_your_teacher_gray;
		cActivitiesForStep.goToToolsAndProd += GoToToolsAndProductsInfoMode;
		cActivitiesForStep.interfaceCallingGoToTools = "Phase2Step7";
		cActivitiesForStep.goToMenuSteps += GoBackToMenuOfStepsFromActivInformative;
		cActivitiesForStep.interfaceGoingBackFrom = "Phase2Step7";
		cActivitiesForStep.goToVideosForStep += GoToVideoOfEachStepInformativeMode;

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "II16", "13", "-1","consulta");
				
	}//cierra GoToActivitiesPhase2Step7
	
	
	// <summary>
	/// Goes to activities phase2 step8. (Actividades de Matizado de interiores Pasada Final(Fase Matizado))
	/// </summary>
	public void GoToActivitiesPhase2Step8Informative(){
		
		if (current_interface == CurrentInterface.MENU_SUB_STEPS_INTERIORES_PHASE2_IM)
			Destroy (menuSubStepsPhaseTwoInterior_int_im_instance);
		else if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_IM)
			Destroy (ToolsAndProductsPhase2Step8_int_im_instance);
		else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP8_IM)
			Destroy (AR_Mode_Search_int_im_instance);
		
		
		current_interface = CurrentInterface.ACTIVITIES_PHASE2_STEP8_IM;
		
		//Ojo con lo siguiente estoy destruyendo todas las instancias que hayan de la interfaz de actividades por cada paso
		DestroyInstancesWithTag ("ActivitiesForEachStep");
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");
		
		ActivitiesForPhase2Step8_int_im_instance = Instantiate (AppManager.manager.ActivitiesForEachStep);
		CanvasActivitiesForEachStepHeaders cActivitiesForStep = ActivitiesForPhase2Step8_int_im_instance.GetComponent<CanvasActivitiesForEachStepHeaders> ();
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
		cActivitiesForStep.image_tres_self_assessment = image_tres_self_assessment_gray;
		cActivitiesForStep.image_cuatro_simulations = image_cuatro_fotos_gray;
		cActivitiesForStep.image_cinco_personal_notes = image_cinco_personal_notes_gray;
		cActivitiesForStep.image_seis_frequently_questions = image_seis_frequently_questions_gray;
		cActivitiesForStep.image_siete_ask_your_teacher = image_siete_ask_your_teacher_gray;
		cActivitiesForStep.goToToolsAndProd += GoToToolsAndProductsInfoMode;
		cActivitiesForStep.interfaceCallingGoToTools = "Phase2Step8";
		cActivitiesForStep.goToMenuSteps += GoBackToMenuOfStepsFromActivInformative;
		cActivitiesForStep.interfaceGoingBackFrom = "Phase2Step8";
		cActivitiesForStep.goToVideosForStep += GoToVideoOfEachStepInformativeMode;

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "II17", "14", "-1","consulta");
				
	}//cierra GoToActivitiesPhase2Step8Informative

	/// <summary>
	/// Goes to tools and products info mode.
	/// Method for loading tools and products intefaces in the Informtive Mode
	/// </summary>
	/// <param name="interface_from">Interface_from.</param>
	public void GoToToolsAndProductsInfoMode(string interface_from){
		
		Debug.LogError ("Llamado al metodo go to tools and products en INFORMATIVE MODE!!");
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		switch (interface_from) {
		case "Phase1Step2":
			Debug.Log("InformativeMODE: Ingresa al case Phase1Step2... Cargando Interfaz en GoToToolsAndProductsInfoMode");
			if (current_interface == CurrentInterface.ACTIVITIES_PHASE1_STEP2_IM)
				Destroy (ActivitiesForPhase1Step2_int_im_instance);
			else if(current_interface == CurrentInterface.AR_SEARCH_PRODUCTS_TUTORIAL_IM)
				Destroy(TutorialPhaseTwoSearchProd_int_im_instance);
			else if (current_interface == CurrentInterface.AR_SEARCH_BAYETA_PRODUCT_TUTORIAL_IM)
				Destroy (TutorialTwoSearchBayeta_int_im_instance);

			current_interface = CurrentInterface.TOOLS_AND_PRODUCTS_IM;
			
			ToolsAndProductsPhase1Step2_int_im_instance = Instantiate (AppManager.manager.ToolsAndProductsInterface);
			CanvasToolsAndProductsManager cToolsAndProductsManager = ToolsAndProductsPhase1Step2_int_im_instance.GetComponent<CanvasToolsAndProductsManager> ();
			cToolsAndProductsManager.image_header_path = "Sprites/tools_and_products/tools";
			cToolsAndProductsManager.title_header_text_path = "Texts/Phase1Step2/1_title_header_text";
			cToolsAndProductsManager.title_intro_content_text_path = "Texts/Phase1Step2/2_introduction_text";
			cToolsAndProductsManager.tool_one_text_path = "Texts/Phase1Step2/3_tool_one_text";
			cToolsAndProductsManager.tool_two_text_path = "Texts/Phase1Step2/4_tool_two_text";
			cToolsAndProductsManager.ruta_img_one_tool_path = "Sprites/phase1step2/FrameMarker16_maquina_agua_icon";
			cToolsAndProductsManager.ruta_img_two_tool_path = "Sprites/phase1step2/FrameMarker19_agua_jabon_icon";
			cToolsAndProductsManager.ruta_img_four_tool_path = "Sprites/phase1step2/FrameMarker21_baieta_neteja_icon";
			cToolsAndProductsManager.footer_search_text_path = "Texts/Phase1Step2/5_ending_search_text";
			cToolsAndProductsManager.goBackButtonAction += GoToActivitiesPhase1Step2Informative;
			cToolsAndProductsManager.goToSearchProductsTools += GoToSearchObjectsTutorialPhase2InfoMode;
			cToolsAndProductsManager.interfaceGoingBackFrom = interface_from;
			
			//asignando la interfaz activa para controlar el regreso:
			AppManager.manager.interfaceInstanceActive = ToolsAndProductsPhase1Step2_int_im_instance;

			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "II18", "1", "-1","consulta");
					
			break;
		case "Phase1Step3":
			Debug.Log("InformativeMode: Ingresa al case Phase1Step3... Cargando Interfaz en GoToToolsAndProducts");
			if (current_interface == CurrentInterface.ACTIVITIES_PHASE1_STEP3_IM)
				Destroy (ActivitiesForPhase1Step3_int_im_instance);
			 else if (current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP3_IM)
				Destroy(AR_Mode_Search_int_im_instance);
			
			current_interface = CurrentInterface.TOOLS_AND_PRODUCTS_IM;
			
			ToolsAndProductsPhase1Step3_int_im_instance = Instantiate (AppManager.manager.ToolsAndProductsInterface);
			CanvasToolsAndProductsManager cToolsAndProductsManagerP1S3 = ToolsAndProductsPhase1Step3_int_im_instance.GetComponent<CanvasToolsAndProductsManager> ();
			cToolsAndProductsManagerP1S3.image_header_path = "Sprites/tools_and_products/tools";
			cToolsAndProductsManagerP1S3.title_header_text_path = "Texts/Phase1Step3/1_title_header_text";
			cToolsAndProductsManagerP1S3.title_intro_content_text_path = "Texts/Phase1Step3/2_introduction_text";
			cToolsAndProductsManagerP1S3.tool_one_text_path = "Texts/Phase1Step3/3_tool_one_text";
			cToolsAndProductsManagerP1S3.tool_two_text_path = "Texts/Phase1Step3/4_tool_two_text";
			cToolsAndProductsManagerP1S3.ruta_img_one_tool_path = "Sprites/phase1step3/FrameMarker16_maquina_agua_icon";
			cToolsAndProductsManagerP1S3.ruta_img_four_tool_path = "Sprites/phase1step3/FrameMarker25_papel_dc3430_icon";
			cToolsAndProductsManagerP1S3.footer_search_text_path = "Texts/Phase1Step3/5_ending_search_text";
			cToolsAndProductsManagerP1S3.goBackButtonAction += GoToActivitiesPhase1Step3Informative;
			cToolsAndProductsManagerP1S3.goToSearchProductsTools += GoToSearchAguaPresionPhase1Step3InfoMode;
			//Atencion: Es muy importante definir esta variable para poder ir hacia atras:
			cToolsAndProductsManagerP1S3.interfaceGoingBackFrom = interface_from;
			
			//asignando la interfaz activa para controlar el regreso:
			AppManager.manager.interfaceInstanceActive = ToolsAndProductsPhase1Step3_int_im_instance;
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "II19", "2", "-1","consulta");
			break;
		case "Phase1Step4":
			
			Debug.Log("InformativeMode: Ingresa al case Phase1Step4... Cargando Interfaz en GoToToolsAndProducts");
			if (current_interface == CurrentInterface.ACTIVITIES_PHASE1_STEP4_IM)
				Destroy (ActivitiesForPhase1Step4_int_im_instance);
			else if (current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP4_IM)
				Destroy(AR_Mode_Search_int_im_instance);
			
			current_interface = CurrentInterface.TOOLS_AND_PRODUCTS_IM;
			
			ToolsAndProductsPhase1Step4_int_im_instance = Instantiate (AppManager.manager.ToolsAndProductsInterface);
			CanvasToolsAndProductsManager cToolsAndProductsManagerP1S4 = ToolsAndProductsPhase1Step4_int_im_instance.GetComponent<CanvasToolsAndProductsManager> ();
			cToolsAndProductsManagerP1S4.image_header_path = "Sprites/tools_and_products/tools";
			cToolsAndProductsManagerP1S4.title_header_text_path = "Texts/Phase1Step4/1_title_header_text";
			cToolsAndProductsManagerP1S4.title_intro_content_text_path = "Texts/Phase1Step4/2_introduction_text";
			cToolsAndProductsManagerP1S4.tool_one_text_path = "Texts/Phase1Step4/3_tool_one_text";
			//cToolsAndProductsManagerP1S4.tool_two_text_path = "Texts/Phase1Step4/4_tool_two_text";
			cToolsAndProductsManagerP1S4.ruta_img_one_tool_path = "Sprites/phase1step4/FrameMarker45_esponja_p320_icon";
			cToolsAndProductsManagerP1S4.ruta_img_four_tool_path = "Sprites/phase1step3/FrameMarker46_esponja_p400_icon";
			cToolsAndProductsManagerP1S4.footer_search_text_path = "Texts/Phase1Step4/5_ending_search_text";
			cToolsAndProductsManagerP1S4.goBackButtonAction += GoToActivitiesPhase1Step4Informative;
			cToolsAndProductsManagerP1S4.goToSearchProductsTools += GoToSearchLijaFinaPhase1Step4InfoMode;
			//Atencion: Es muy importante definir esta variable para poder ir hacia atras:
			cToolsAndProductsManagerP1S4.interfaceGoingBackFrom = interface_from;
			
			//asignando la interfaz activa para controlar el regreso:
			AppManager.manager.interfaceInstanceActive = ToolsAndProductsPhase1Step4_int_im_instance;
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "II20", "3", "-1","consulta");
			break;
		case "Phase1Step5":
			Debug.Log("InformativeMode: Ingresa al case Phase1Step5... Cargando Interfaz en GoToToolsAndProducts");
			if (current_interface == CurrentInterface.ACTIVITIES_PHASE1_STEP5_IM)
				Destroy (ActivitiesForPhase1Step5_int_im_instance);
			else if (current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP5_IM)
				Destroy(AR_Mode_Search_int_im_instance);
			
			current_interface = CurrentInterface.TOOLS_AND_PRODUCTS_IM;
			
			ToolsAndProductsPhase1Step5_int_im_instance = Instantiate (AppManager.manager.ToolsAndProductsInterface);
			CanvasToolsAndProductsManager cToolsAndProductsManagerP1S5 = ToolsAndProductsPhase1Step5_int_im_instance.GetComponent<CanvasToolsAndProductsManager> ();
			cToolsAndProductsManagerP1S5.image_header_path = "Sprites/tools_and_products/tools";
			cToolsAndProductsManagerP1S5.title_header_text_path = "Texts/Phase1Step5/1_title_header_text";
			cToolsAndProductsManagerP1S5.title_intro_content_text_path = "Texts/Phase1Step5/2_introduction_text";
			cToolsAndProductsManagerP1S5.tool_one_text_path = "Texts/Phase1Step5/3_tool_one_text";
			//cToolsAndProductsManagerP1S4.tool_two_text_path = "Texts/Phase1Step4/4_tool_two_text";
			cToolsAndProductsManagerP1S5.ruta_img_one_tool_path = "Sprites/phase1step5/FrameMarker100_martillo_repasar_icon";
			//cToolsAndProductsManagerP1S4.ruta_img_four_tool_path = "Sprites/phase1step3/papel_absorbente";
			cToolsAndProductsManagerP1S5.footer_search_text_path = "Texts/Phase1Step5/5_ending_search_text";
			cToolsAndProductsManagerP1S5.goBackButtonAction += GoToActivitiesPhase1Step5Informative;
			cToolsAndProductsManagerP1S5.goToSearchProductsTools += GoToSearchMartilloPhase1Step5InfoMode;
			//Atencion: Es muy importante definir esta variable para poder ir hacia atras:
			cToolsAndProductsManagerP1S5.interfaceGoingBackFrom = interface_from;
			
			//asignando la interfaz activa para controlar el regreso:
			AppManager.manager.interfaceInstanceActive = ToolsAndProductsPhase1Step5_int_im_instance;
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "II21", "4", "-1","consulta");
			break;
			
		case "Phase1Step6":
			Debug.Log("InformativeMode: Ingresa al case Phase1Step6... Cargando Interfaz en GoToToolsAndProducts");
			if (current_interface == CurrentInterface.ACTIVITIES_PHASE1_STEP6_IM)
				Destroy (ActivitiesForPhase1Step6_int_im_instance);
			else if (current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP6_IM)
				Destroy(AR_Mode_Search_int_im_instance);
			
			current_interface = CurrentInterface.TOOLS_AND_PRODUCTS_IM;
			
			ToolsAndProductsPhase1Step6_int_im_instance = Instantiate (AppManager.manager.ToolsAndProductsInterface);
			CanvasToolsAndProductsManager cToolsAndProductsManagerP1S6 = ToolsAndProductsPhase1Step6_int_im_instance.GetComponent<CanvasToolsAndProductsManager> ();
			cToolsAndProductsManagerP1S6.image_header_path = "Sprites/tools_and_products/tools";
			cToolsAndProductsManagerP1S6.title_header_text_path = "Texts/Phase1Step6/1_title_header_text";
			cToolsAndProductsManagerP1S6.title_intro_content_text_path = "Texts/Phase1Step6/2_introduction_text";
			cToolsAndProductsManagerP1S6.tool_one_text_path = "Texts/Phase1Step6/3_tool_one_text";
			cToolsAndProductsManagerP1S6.tool_two_text_path = "Texts/Phase1Step6/4_tool_two_text";
			cToolsAndProductsManagerP1S6.ruta_img_one_tool_path = "Sprites/phase1step6/FrameMarker26_desengrasante_icon";
			cToolsAndProductsManagerP1S6.ruta_img_four_tool_path = "Sprites/phase1step6/FrameMarker25_papel_dc3430_icon";
			cToolsAndProductsManagerP1S6.footer_search_text_path = "Texts/Phase1Step6/5_ending_search_text";
			cToolsAndProductsManagerP1S6.goBackButtonAction += GoToActivitiesPhase1Step6Informative;
			cToolsAndProductsManagerP1S6.goToSearchProductsTools += GoToSearchDesengrasantePhase1Step6InfoMode;
			//Atencion: Es muy importante definir esta variable para poder ir hacia atras:
			cToolsAndProductsManagerP1S6.interfaceGoingBackFrom = interface_from;
			
			//asignando la interfaz activa para controlar el regreso:
			AppManager.manager.interfaceInstanceActive = ToolsAndProductsPhase1Step6_int_im_instance;
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "II22", "5", "-1","consulta");
		break;
		case "Phase2Step2":
			Debug.Log("Ingresa al case Phase2Step2... Cargando Interfaz en GoToToolsAndProducts");
			if (current_interface == CurrentInterface.ACTIVITIES_PHASE2_STEP2_IM)
				Destroy (ActivitiesForPhase2Step2_int_im_instance);
			else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP2_IM)
				Destroy(AR_Mode_Search_int_im_instance);
			
			current_interface = CurrentInterface.TOOLS_AND_PRODUCTS_IM;
			
			ToolsAndProductsPhase2Step2_int_im_instance = Instantiate (AppManager.manager.ToolsAndProductsInterface);
			CanvasToolsAndProductsManager cToolsAndProductsManagerP2S2 = ToolsAndProductsPhase2Step2_int_im_instance.GetComponent<CanvasToolsAndProductsManager> ();
			cToolsAndProductsManagerP2S2.image_header_path = "Sprites/tools_and_products/tools";
			cToolsAndProductsManagerP2S2.title_header_text_path = "Texts/Phase2Step2/1_title_header_text";
			cToolsAndProductsManagerP2S2.title_intro_content_text_path = "Texts/Phase2Step2/0_introduction_text";
			cToolsAndProductsManagerP2S2.tool_one_text_path = "Texts/Phase2Step2/2_tool_one_text";
			cToolsAndProductsManagerP2S2.tool_two_text_path = "Texts/Phase2Step2/3_tool_two_text";
			cToolsAndProductsManagerP2S2.ruta_img_one_tool_path = "Sprites/phase2step2/FrameMarker65_cinta_enmascarar_icon";
			cToolsAndProductsManagerP2S2.ruta_img_four_tool_path = "Sprites/phase2step2/FrameMarker69_papel_enmascarar_icon";
			cToolsAndProductsManagerP2S2.footer_search_text_path = "Texts/Phase2Step2/4_ending_search_text";
			cToolsAndProductsManagerP2S2.goBackButtonAction += GoToActivitiesPhase2Step2Informative;
			cToolsAndProductsManagerP2S2.goToSearchProductsTools += GoToSearchCintaEnmascPhase2Step2InfoMode;
			//Atencion: Es muy importante definir esta variable para poder ir hacia atras:
			cToolsAndProductsManagerP2S2.interfaceGoingBackFrom = interface_from;
			
			//asignando la interfaz activa para controlar el regreso:
			AppManager.manager.interfaceInstanceActive = ToolsAndProductsPhase2Step2_int_im_instance;
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "II23", "7", "-1","consulta");
			break;
		case "Phase2Step3":
			Debug.Log("Ingresa al case Phase2Step3... Cargando Interfaz en GoToToolsAndProducts");
			if (current_interface == CurrentInterface.ACTIVITIES_PHASE2_STEP3_IM)
				Destroy (ActivitiesForPhase2Step3_int_im_instance);
			else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP3_IM)
				Destroy(AR_Mode_Search_int_im_instance);
			
			current_interface = CurrentInterface.TOOLS_AND_PRODUCTS_IM;
			
			ToolsAndProductsPhase2Step3_int_im_instance = Instantiate (AppManager.manager.ToolsAndProductsInterface);
			CanvasToolsAndProductsManager cToolsAndProductsManagerP2S3 = ToolsAndProductsPhase2Step3_int_im_instance.GetComponent<CanvasToolsAndProductsManager> ();
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
			cToolsAndProductsManagerP2S3.goBackButtonAction += GoToActivitiesPhase2Step3Informative;
			cToolsAndProductsManagerP2S3.goToSearchProductsTools += GoToSearchEsponjaP320Phase2Step3InfoMode;
			//Atencion: Es muy importante definir esta variable para poder ir hacia atras:
			cToolsAndProductsManagerP2S3.interfaceGoingBackFrom = interface_from;
			
			//asignando la interfaz activa para controlar el regreso:
			AppManager.manager.interfaceInstanceActive = ToolsAndProductsPhase2Step3_int_im_instance;
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "II24", "9", "-1","consulta");
			break;
		case "Phase2Step4":
			Debug.Log("Ingresa al case Phase2Step4... Cargando Interfaz en GoToToolsAndProducts");
			if (current_interface == CurrentInterface.ACTIVITIES_PHASE2_STEP4_IM)
				Destroy (ActivitiesForPhase2Step4_int_im_instance);
			else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP4_IM)
				Destroy(AR_Mode_Search_int_im_instance);
			
			current_interface = CurrentInterface.TOOLS_AND_PRODUCTS_IM;
			
			ToolsAndProductsPhase2Step4_int_im_instance = Instantiate (AppManager.manager.ToolsAndProductsInterface);
			CanvasToolsAndProductsManager cToolsAndProductsManagerP2S4 = ToolsAndProductsPhase2Step4_int_im_instance.GetComponent<CanvasToolsAndProductsManager> ();
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
			cToolsAndProductsManagerP2S4.goBackButtonAction += GoToActivitiesPhase2Step4Informative;
			cToolsAndProductsManagerP2S4.goToSearchProductsTools += GoToSearchEsponjaP400Phase2Step4Informative;
			//Atencion: Es muy importante definir esta variable para poder ir hacia atras:
			cToolsAndProductsManagerP2S4.interfaceGoingBackFrom = interface_from;
			
			//asignando la interfaz activa para controlar el regreso:
			AppManager.manager.interfaceInstanceActive = ToolsAndProductsPhase2Step4_int_im_instance;
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "II25", "10", "-1","consulta");
			break;
		case "Phase2Step5":
			Debug.Log("Ingresa al case Phase2Step5... Cargando Interfaz en GoToToolsAndProducts");
			if (current_interface == CurrentInterface.ACTIVITIES_PHASE2_STEP5_IM)
				Destroy (ActivitiesForPhase2Step5_int_im_instance);
			else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP5_IM)
				Destroy(AR_Mode_Search_int_im_instance);
			
			current_interface = CurrentInterface.TOOLS_AND_PRODUCTS_IM;
			
			ToolsAndProductsPhase2Step5_int_im_instance = Instantiate (AppManager.manager.ToolsAndProductsInterface);
			CanvasToolsAndProductsManager cToolsAndProductsManagerP2S5 = ToolsAndProductsPhase2Step5_int_im_instance.GetComponent<CanvasToolsAndProductsManager> ();
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
			cToolsAndProductsManagerP2S5.goBackButtonAction += GoToActivitiesPhase2Step5Informative;
			cToolsAndProductsManagerP2S5.goToSearchProductsTools += GoToSearchRotoOrbitalPhase2Step5Informative;
			//Atencion: Es muy importante definir esta variable para poder ir hacia atras:
			cToolsAndProductsManagerP2S5.interfaceGoingBackFrom = interface_from;
			
			//asignando la interfaz activa para controlar el regreso:
			AppManager.manager.interfaceInstanceActive = ToolsAndProductsPhase2Step5_int_im_instance;
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "II26", "11", "-1","consulta");
			break;
		case "Phase2Step6":
			Debug.Log("Ingresa al case Phase2Step6... Cargando Interfaz en GoToToolsAndProducts");
			if (current_interface == CurrentInterface.ACTIVITIES_PHASE2_STEP6_IM)
				Destroy (ActivitiesForPhase2Step6_int_im_instance);
			else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP6_IM)
				Destroy(AR_Mode_Search_int_im_instance);
			
			current_interface = CurrentInterface.TOOLS_AND_PRODUCTS_IM;
			
			ToolsAndProductsPhase2Step6_int_im_instance = Instantiate (AppManager.manager.ToolsAndProductsInterface);
			CanvasToolsAndProductsManager cToolsAndProductsManagerP2S6 = ToolsAndProductsPhase2Step6_int_im_instance.GetComponent<CanvasToolsAndProductsManager> ();
			cToolsAndProductsManagerP2S6.image_header_path = "Sprites/tools_and_products/tools";
			cToolsAndProductsManagerP2S6.title_header_text_path = "Texts/Phase2Step6/1_title_header_text";
			cToolsAndProductsManagerP2S6.title_intro_content_text_path = "Texts/Phase2Step6/2_introduction_text_tools";
			cToolsAndProductsManagerP2S6.tool_one_text_path = "Texts/Phase2Step6/3_tool_one_text";
			cToolsAndProductsManagerP2S6.tool_two_text_path = "Texts/Phase2Step6/4_tool_two_text";
			if(AppManager.manager.last_markerid_scanned == 30)
				cToolsAndProductsManagerP2S6.ruta_img_one_tool_path = "Sprites/phase2step6/FrameMarker33_disco_p150";
			else 
				cToolsAndProductsManagerP2S6.ruta_img_one_tool_path = "Sprites/phase2step6/FrameMarker34_disco_p180";
			cToolsAndProductsManagerP2S6.ruta_img_four_tool_path = "Sprites/phase2step6/FrameMarker23_pistola_aire_icon";
			cToolsAndProductsManagerP2S6.ruta_img_five_tool_path = "Sprites/phase2step6/FrameMarker24_paper_neteja_icon";
			cToolsAndProductsManagerP2S6.ruta_img_six_tool_path = "Sprites/phase2step6/FrameMarker25_papel_dc3430_icon";
			cToolsAndProductsManagerP2S6.footer_search_text_path = "Texts/Phase2Step6/5_ending_search_text";
			cToolsAndProductsManagerP2S6.goBackButtonAction += GoToActivitiesPhase2Step6Informative;
			cToolsAndProductsManagerP2S6.goToSearchProductsTools += GoToSearchDiscoP150Phase2Step6Informative;
			//Atencion: Es muy importante definir esta variable para poder ir hacia atras:
			cToolsAndProductsManagerP2S6.interfaceGoingBackFrom = interface_from;
			
			//asignando la interfaz activa para controlar el regreso:
			AppManager.manager.interfaceInstanceActive = ToolsAndProductsPhase2Step6_int_im_instance;
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "II27", "12", "-1","consulta");
			break;
		case "Phase2Step7":
			Debug.Log("Ingresa al case Phase2Step7... Cargando Interfaz en GoToToolsAndProducts");
			if (current_interface == CurrentInterface.ACTIVITIES_PHASE2_STEP7_IM)
				Destroy (ActivitiesForPhase2Step7_int_im_instance);
			else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP7_IM)
				Destroy(AR_Mode_Search_int_im_instance);
			
			current_interface = CurrentInterface.TOOLS_AND_PRODUCTS_IM;
			
			ToolsAndProductsPhase2Step7_int_im_instance = Instantiate (AppManager.manager.ToolsAndProductsInterface);
			CanvasToolsAndProductsManager cToolsAndProductsManagerP2S7 = ToolsAndProductsPhase2Step7_int_im_instance.GetComponent<CanvasToolsAndProductsManager> ();
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
			cToolsAndProductsManagerP2S7.goBackButtonAction += GoToActivitiesPhase2Step7Informative;
			cToolsAndProductsManagerP2S7.goToSearchProductsTools += GoToSearchDiscoP240Phase2Step7Informative;
			//Atencion: Es muy importante definir esta variable para poder ir hacia atras:
			cToolsAndProductsManagerP2S7.interfaceGoingBackFrom = interface_from;
			
			//asignando la interfaz activa para controlar el regreso:
			AppManager.manager.interfaceInstanceActive = ToolsAndProductsPhase2Step7_int_im_instance;
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "II28", "13", "-1","consulta");
			break;
		case "Phase2Step8":
			Debug.Log("Ingresa al case Phase2Step8... Cargando Interfaz en GoToToolsAndProducts");
			if (current_interface == CurrentInterface.ACTIVITIES_PHASE2_STEP8_IM)
				Destroy (ActivitiesForPhase2Step8_int_im_instance);
			else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP8_IM)
				Destroy(AR_Mode_Search_int_im_instance);
			
			current_interface = CurrentInterface.TOOLS_AND_PRODUCTS_IM;
			
			ToolsAndProductsPhase2Step8_int_im_instance = Instantiate (AppManager.manager.ToolsAndProductsInterface);
			CanvasToolsAndProductsManager cToolsAndProductsManagerP2S8 = ToolsAndProductsPhase2Step8_int_im_instance.GetComponent<CanvasToolsAndProductsManager> ();
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
			cToolsAndProductsManagerP2S8.goBackButtonAction += GoToActivitiesPhase2Step8Informative;
			cToolsAndProductsManagerP2S8.goToSearchProductsTools += GoToSearchDiscoP320Phase2Step8Informative;
			//Atencion: Es muy importante definir esta variable para poder ir hacia atras:
			cToolsAndProductsManagerP2S8.interfaceGoingBackFrom = interface_from;
			
			//asignando la interfaz activa para controlar el regreso:
			AppManager.manager.interfaceInstanceActive = ToolsAndProductsPhase2Step8_int_im_instance;
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "II29", "14", "-1","consulta");
			break;
			
		case "":
			Debug.Log("-->ERROR INFORMATIVE MODE: La interfaz de la cual se pretende retornar viene vacia");
			break;
		}
					
	} //cierra GoToToolsAndProducts

	/// <summary>
	/// Goes the back to menu of activities from tools products.
	/// </summary>
	/// <param name="interface_coming_from">Interface_coming_from.</param>
	public void GoBackToMenuActivitiesFromToolsProductsInfoMode(string interface_coming_from){
		
		Debug.LogError ("Llamado al metodo go to tools and products!!");
		
		switch (interface_coming_from) {
		case "Phase1Step2":
			GoToActivitiesPhase1Step2Informative();
			break;
		case "Phase1Step3":
			GoToActivitiesPhase1Step3Informative();
			break;
		
		case "Phase1Step4":
			GoToActivitiesPhase1Step4Informative();
			break;
		case "Phase1Step5":
			GoToActivitiesPhase1Step5Informative();
			break;
		case "Phase1Step6":
			GoToActivitiesPhase1Step6Informative();
			break;
		case "Phase2Step2":
			GoToActivitiesPhase2Step2Informative();
			break;
		case "Phase2Step3":
			GoToActivitiesPhase2Step3Informative();
			break;
		case "Phase2Step4":
			GoToActivitiesPhase2Step4Informative();
			break;
		case "Phase2Step5":
			GoToActivitiesPhase2Step5Informative();
			break;
		case "Phase2Step6":
			GoToActivitiesPhase2Step6Informative();
			break;
		case "Phase2Step7":
			GoToActivitiesPhase2Step7Informative();
			break;
		case "Phase2Step8":
			GoToActivitiesPhase2Step8Informative();
			break;
		case "":
			Debug.Log("ERROR: No hay interface_coming_from definido en el metodo GoBackFromToolsAndProducts");
			break;
		}
	} //cierra GoBackToMenuActivitiesFromToolsProductsInfoMode


	/// <summary>
	/// Este metodo es invocado desde el action de un boton en la interfaz. Normalmente desde el boton regresar
	/// en el header de la interfaz de actividades de un paso del proceso. 
	/// Previamente antes de llamar a este metodo se debe haber fijado el parametro: interface_coming_from
	/// </summary>
	/// <param name="interface_coming_from">Parametro que indica la interfaz desde la cual se esta regresando</param>
	public void GoBackToMenuOfStepsFromActivInformative(string interface_coming_from){
		
		Debug.LogError ("Llamado al metodo GoBackToMenuOfStepsFromActivInformative!!");
		
		switch (interface_coming_from) {
		case "Phase1Step2":
			Debug.Log("Ingresa a Phase1Step2 en GoBackToMenuOfStepsFromActivInformative con interface_coming_from= " + interface_coming_from);
			//ir al menu de pasos de la fase 1:
			//en este caso no es necesario comparar desde que interfaz viene porque ya se hace en el siguiente metodo:
			GoToMenuStepsPhase1InformativeMode();
			break;
		case "Phase1Step3":
			Debug.Log("Ingresa a Phase1Step3 en GoBackToMenuOfStepsFromActivInformative con interface_coming_from= " + interface_coming_from);
			GoToMenuStepsPhase1InformativeMode();
			break;
		 case "Phase1Step4":
			Debug.Log("Ingresa a Phase1Step4 en GoBackToMenuOfStepsFromActivInformative con interface_coming_from= " + interface_coming_from);
			GoToMenuStepsPhase1InformativeMode();
			break;
		case "Phase1Step5":
			Debug.Log("Ingresa a Phase1Step5 en GoBackToMenuOfStepsFromActivInformative con interface_coming_from= " + interface_coming_from);
			GoToMenuStepsPhase1InformativeMode();
			break;
		case "Phase1Step6":
			Debug.Log("Ingresa a Phase1Step6 en GoBackToMenuOfStepsFromActivInformative con interface_coming_from= " + interface_coming_from);
			GoToMenuStepsPhase1InformativeMode();
			break;
		case "Phase2Step2":
			Debug.Log("Ingresa a Phase2Step2 en GoBackToMenuOfStepsFromActivInformative con interface_coming_from= " + interface_coming_from);
			GoToMenuStepsPhase2InformativeMode();
			break;
		case "Phase2Step3":
			Debug.Log("Ingresa a Phase2Step3 en GoBackToMenuOfStepsFromActivInformative con interface_coming_from= " + interface_coming_from);
			GoToSubMenuStepsLijadoCantosInfoMode();
			break;
		case "Phase2Step4":
			Debug.Log("Ingresa a Phase2Step4 en GoBackToMenuOfStepsFromActivInformative con interface_coming_from= " + interface_coming_from);
			GoToSubMenuStepsLijadoCantosInfoMode();
			break;
		case "Phase2Step5":
			Debug.Log("Ingresa a Phase2Step5 en GoBackToMenuOfStepsFromActivInformative con interface_coming_from= " + interface_coming_from);
			GoToSubMenuStepsLijadoInterioresInfoMode();
			break;
		case "Phase2Step6":
			Debug.Log("Ingresa a Phase2Step6 en GoBackToMenuOfStepsFromActivInformative con interface_coming_from= " + interface_coming_from);
			GoToSubMenuStepsLijadoInterioresInfoMode();
			break;
		case "Phase2Step7":
			Debug.Log("Ingresa a Phase2Step7 en GoBackToMenuOfStepsFromActivInformative con interface_coming_from= " + interface_coming_from);
			GoToSubMenuStepsLijadoInterioresInfoMode();
			break;
		case "Phase2Step8":
			Debug.Log("Ingresa a Phase2Step8 en GoBackToMenuOfStepsFromActivInformative con interface_coming_from= " + interface_coming_from);
			GoToSubMenuStepsLijadoInterioresInfoMode();
			break;

		}
		
	} //cierra GoBackToMenuOfStepsFromActivities



	public void GoToVideoOfEachStepInformativeMode(string interface_from){
		Debug.Log ("Llamando al metodo GoToVideoOfEachStepInformativeMode!!");
		AndroidJavaClass androidJC;
		AndroidJavaObject jo;
		AndroidJavaClass jc;
		string video_url;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		//hay que tener en cuenta que este metodo depende de la variable "interfaceCallingGoToTools" que se define cuando
		//se define el llamado a este metodo por ejemplo en el metodo "GoToActivitiesPhase1Step2"
		switch (interface_from) {
		case "Phase1Step2":

			//asignando la URL dependiendo del paso:
			video_url = video_phase1_step2;
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: VI1");
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "VI1", "1", "-1","consulta");
			break;
		case "Phase1Step3":

			//asignando la URL dependiendo del paso:
			video_url = video_phase1_step3;

			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: VI1");
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "VI2", "2", "-1","consulta");
			break;
		case "Phase1Step4":

			//asignando la URL dependiendo del paso:
			video_url = video_phase1_step4;

			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: VI1");
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "VI3", "3", "-1","consulta");
			break;
		case "Phase1Step5":

			//asignando la URL dependiendo del paso:
			video_url = video_phase1_step5;

			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: VI1");
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "VI4", "4", "-1","consulta");
			break;
		case "Phase1Step6":
			//asignando la URL dependiendo del paso:
			video_url = video_phase1_step6;
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: VI1");
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "VI5", "5", "-1","consulta");
			break;
		case "Phase2Step2":
			//asignando la URL dependiendo del paso:
			video_url = this.video_matizado_phase;
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: VI1");
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "VI6", "7", "-1","consulta");
			break;
		case "Phase2Step3":
			//asignando la URL dependiendo del paso:
			video_url = this.video_matizado_phase;
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: VI1");
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "VI7", "9", "-1","consulta");
			break;
		case "Phase2Step4":
			//asignando la URL dependiendo del paso:
			video_url = this.video_matizado_phase;
			break;Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: VI1");
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "VI8", "10", "-1","consulta");
		case "Phase2Step5":
			//asignando la URL dependiendo del paso:
			video_url = this.video_matizado_phase;
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: VI1");
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "VI9", "11", "-1","consulta");
			break;
		case "Phase2Step6":
			//asignando la URL dependiendo del paso:
			video_url = this.video_matizado_phase;
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: VI1");
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "VI10", "12", "-1","consulta");
			break;
		case "Phase2Step7":
			//asignando la URL dependiendo del paso:
			video_url = this.video_matizado_phase;
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: VI1");
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "VI11", "13", "-1","consulta");
			break;
		case "Phase2Step8":
			//asignando la URL dependiendo del paso:
			video_url = this.video_matizado_phase;
			Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion: VI1");
			NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "VI2", "14", "-1","consulta");
			break;
		default:
			video_url = video_phase1_step2;
			break;
		}
		
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
	}


	/// <summary>
	/// Goes to search capo coche. Metodo que inicia el modo RA para buscar el capo del carro y que incluye la fase 1 del tutorial
	/// </summary>
	public void GoToSearchCapoCocheInformative(){
		Debug.Log ("Entra al metodo GoToSearchCapoCocheInformative en InformativeMode... interfaz: "  + current_interface);
		if (current_interface == CurrentInterface.ACTIVITIES_PHASE1_STEP1_IM) {
			Destroy(ActivitiesForPhase1Step1_int_im_instance);
		}
		
		current_interface = CurrentInterface.AR_SEARCH_CAR_HOOD_IM;
		
		TurorialSearchCapoCarro_int_im_instance = Instantiate (AppManager.manager.TutorialSearchCapoCarroInterface);
		ControllerBlinkingMarker blinkingMarker = TurorialSearchCapoCarro_int_im_instance.GetComponent<ControllerBlinkingMarker> ();
		
		//Es iportante asignar esta variable para poder controlar los marcadores
		AppManager.manager.interfaceInstanceActive = TurorialSearchCapoCarro_int_im_instance;
		
		//definiendo que estamos en tutorial fase 1:
		AppManager.manager.inTutorialPhase1 = true;


		//localizando el marcador que se debe buscar en esta fase del proceso y cargando datos respectivos:
		markerInScene = GameObject.Find ("FrameMarker1");
		ControllerAddInfoInMarker controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		controller_info_marker.onClickSelectButton_tut1 += onClickSelectCapoCarroSearchInformative;
		
		//asignando el texto que se debe mostrar al momento del feedback:
		blinkingMarker.feedback_info_text_path = feedback_text_path;
		
		//definiendo los ordenes que controlara esta interfaz:
		order_in_process = new int[1];
		order_in_process [0] = 1;
		blinkingMarker.ordenes = order_in_process;
		
		//NOTA: antes de hacer esto es importante que la variable interfaceInstanceActive este asignada:
		//tambien es importante haber asignado la variable inTutorialPhase1
		controller_info_marker.LoadInformationToInterface (true, false, false);
		
		Debug.LogError ("NOW: Start Blinking");
		//iniciando el proceso blinking:
		blinkingMarker.should_be_blinking = true;
		
		//colocando en false la informacion adicional por si se le habia dado atras en algun momento en la interfaz:
		//info_additional_displayed = false;
		AppManager.manager.info_additional_displayed = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "II30", "0", "-1","consulta");		
	} //cierra GoToSearchCapoCoche


	/// <summary>
	/// Goes to search objects tutorial phase2.
	/// Method that is called in order to start tutorial phase 2 - Searching Agua y Jabon
	/// </summary>
	public void GoToSearchObjectsTutorialPhase2InfoMode(){

		Debug.Log ("InformativeMode: Llamado al metodo GoToSearchObjectsTutorialPhase2InfoMode ");

		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_IM) {
			Destroy(ToolsAndProductsPhase1Step2_int_im_instance);
		}
		
		current_interface = CurrentInterface.AR_SEARCH_PRODUCTS_TUTORIAL_IM;
		
		//Destruyendo las demas interfaces que pueda haber en memoria en caso de que las haya para
		//evitar problemas con interfaces que se solapan:
		DestroyInstancesWithTag ("TutorialPhaseTwo");
		
		//indica que entramos en la fase 2 del tutorial:
		AppManager.manager.inTutorialPhase2 = true;
				
		//ahora aqui debo instanciar la interfaz del tutorial 2 y asignarle todas las propiedades:
		TutorialPhaseTwoSearchProd_int_im_instance = Instantiate (AppManager.manager.TutorialSearchProductsPhase2);
		ControllerBlinkingAddInfo blinking_search_phase_two = TutorialPhaseTwoSearchProd_int_im_instance.GetComponent<ControllerBlinkingAddInfo> ();
		
		//hay que asignar la interfaz activa tambien:
		AppManager.manager.interfaceInstanceActive = TutorialPhaseTwoSearchProd_int_im_instance;
				
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
		order_in_process [0] = 2;
		blinking_search_phase_two.ordenes = order_in_process;
		
		//Asignando el metodo que se debe ejecutar cuando se hace click sobre el btn Select de la interfaz
		blinking_search_phase_two.interface_going_from = "Phase1Step2";  //la variable Phase1Step2 permite obtener la interfaz para obtenerla en OnBackButtonTapped
		blinking_search_phase_two.onClickSelectBtn += OnClickSelectAguaPressioSearchInfoMode;
		
		//iniciando el proceso blinking:
		blinking_search_phase_two.should_be_blinking = true;

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "II31", "1", "-1","consulta");
	} //cierra GoToSearchObjectsTutorialPhase2


	/// <summary>
	/// Go to search Agua Jabon
	/// Method that is called in order to start tutorial phase 2 - Searching Agua Jabon (parte 2)
	/// </summary>
	public void GoToSearchAguaJabonTutPhase2InfoMode(){
		
		if (current_interface == CurrentInterface.AR_SEARCH_PRODUCTS_TUTORIAL_IM) {
			Destroy(TutorialPhaseTwoSearchProd_int_im_instance);
		}
		
		DestroyInstancesWithTag ("TutorialPhaseTwo");
		
		current_interface = CurrentInterface.AR_SEARCH_AGUA_JABON_IM;
		
		//indica que entramos en la fase 2 del tutorial:
		AppManager.manager.inTutorialPhase2 = true;
		
		//ahora aqui debo instanciar la interfaz del tutorial 2 y asignarle todas las propiedades:
		TutorialPhaseTwoSearchProd_int_im_instance = Instantiate (AppManager.manager.TutorialSearchProductsPhase2);
		ControllerBlinkingAddInfo blinking_search_phase_two = TutorialPhaseTwoSearchProd_int_im_instance.GetComponent<ControllerBlinkingAddInfo> ();
		
		Debug.Log ("---> Nueva Interfaz Instanciada en GoToSearchAguaJabonTutPhase2");
		
		//hay que asignar la interfaz activa tambien:
		AppManager.manager.interfaceInstanceActive = TutorialPhaseTwoSearchProd_int_im_instance;
		
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
		order_in_process [0] = 3;
		blinking_search_phase_two.ordenes = order_in_process;
		
		//Asignando el metodo que se debe ejecutar cuando se hace click sobre el btn Select de la interfaz
		blinking_search_phase_two.interface_going_from = "Phase1Step2";  //la variable Phase1Step2 permite obtener la interfaz para obtenerla en OnBackButtonTapped
		blinking_search_phase_two.onClickSelectBtn += OnClickSelectAguaJabonSearchInfoMode;
		
		//iniciando el proceso blinking:
		blinking_search_phase_two.should_be_blinking = true;

		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "II32", "1", "-1","consulta");
	} //cierra GoToSearchObjectsTutorialPhase2


	/// <summary>
	/// Go to search bayeta
	/// Method that is called in order to start tutorial phase 2 - Searching Bayeta
	/// </summary>
	public void GoToSearchBayetaTutorialPhase2InfoMode(){
		Debug.Log ("InformativeMode: Ingresa al metodo GoToSearchBayetaTutorialPhase2InfoMode");
		
		if (current_interface == CurrentInterface.AR_SEARCH_AGUA_JABON_IM) {
			Destroy(TutorialPhaseTwoSearchProd_int_im_instance);
		}
		
		current_interface = CurrentInterface.AR_SEARCH_BAYETA_PRODUCT_TUTORIAL_IM;
		
		//indica que entramos en la fase 2 del tutorial:
		AppManager.manager.inTutorialPhase2 = true;
		
		//ahora aqui debo instanciar la interfaz del tutorial 2 y asignarle todas las propiedades:
		TutorialTwoSearchBayeta_int_im_instance = Instantiate (AppManager.manager.TutorialSearchProductsPhase2);
		ControllerBlinkingAddInfo blinking_search_phase_two = TutorialTwoSearchBayeta_int_im_instance.GetComponent<ControllerBlinkingAddInfo> ();
		
		Debug.Log ("Nueva Interfaz Instanciada en GoToSearchBayetaTutorialPhase2InfoMode!!");
		
		//hay que asignar la interfaz activa tambien:
		AppManager.manager.interfaceInstanceActive = TutorialTwoSearchBayeta_int_im_instance;
		
		//localizando el marcador que se debe buscar en esta fase del proceso y cargando datos respectivos:
		markerInScene = GameObject.Find ("FrameMarker21");
		ControllerAddInfoInMarker controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		
		//NOTA: antes de hacer esto es importante que la variable interfaceInstanceActive este asignada:
		//tambien es importante haber asignado la variable inTutorialPhase1
		controller_info_marker.LoadInformationToInterface (false,true,false);
		
		Debug.LogError ("NOW: Start Blinking en GoToSearchBayetaTutorialPhase2InfoMode");
		//asignando el texto para el feedback directamente a la interfaz:
		blinking_search_phase_two.feedback_info_text_path = text_feedback_bayeta;
		
		//definiendo los ordenes que controlara esta interfaz:
		order_in_process = new int[1];
		order_in_process [0] = 4;
		blinking_search_phase_two.ordenes = order_in_process;
		
		//Asignando el metodo que se debe ejecutar cuando se hace click sobre el btn Select de la interfaz
		blinking_search_phase_two.interface_going_from = "Phase1Step2";  //la variable Phase1Step2 permite obtener la interfaz para obtenerla en OnBackButtonTapped
		blinking_search_phase_two.onClickSelectBtn += OnClickSelectBayetaSearchInfoMode;
		
		//iniciando el proceso blinking:
		blinking_search_phase_two.should_be_blinking = true;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "II33", "1", "-1","consulta");
	} //cierra GoToSearchObjectsTutorialPhase2


	/// <summary>
	/// Method that is called in order to start the AR mode for searching the tools of Phase 1 - Step3 (papel absorbente)
	/// </summary>
	public void GoToSearchPapelAbsorbentePhase1Step3InfoMode(){

		Debug.Log ("InformativeMode: Llamado al metodo GoToSearchPapelAbsorbentePhase1Step3InfoMode!!");

		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_IM) {
			Destroy (ToolsAndProductsPhase1Step3_int_im_instance);
		} else if (current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP3_IM)
			Destroy (AR_Mode_Search_int_im_instance);
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE1_STEP3_IM;

		
		//indica que entramos en la fase 2 del tutorial:
		AppManager.manager.inTutorialPhase2 = false;
		
		Debug.Log ("--> Iniciando modo RA en GoToSearchPapelAbsorbentePhase1Step3InfoMode");
		//indica que se ingresa al modo RA fuera de los tutoriales:
		AppManager.manager.in_RA_mode = true;
		
		//ahora aqui debo instanciar la interfaz del tutorial 2 y asignarle todas las propiedades:
		AR_Mode_Search_int_im_instance = Instantiate (AppManager.manager.AR_Mode_interface);
		ControllerBlinkingARGeneric blinking_search_phase1step3 = AR_Mode_Search_int_im_instance.GetComponent<ControllerBlinkingARGeneric> ();
		
		Debug.Log ("Nueva Interfaz Instanciada en GoToSearchPapelAbsorbentePhase1Step3InfoMode!!");
		
		//hay que asignar la interfaz activa tambien:
		AppManager.manager.interfaceInstanceActive = AR_Mode_Search_int_im_instance;
		
		//localizando el marcador que se debe buscar en esta fase del proceso y cargando datos respectivos:
		markerInScene = GameObject.Find ("FrameMarker25");
		ControllerAddInfoInMarker controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		
		//NOTA: antes de hacer esto es importante que la variable interfaceInstanceActive este asignada:
		//tambien es importante haber asignado la variable inTutorialPhase1
		controller_info_marker.LoadInformationToInterface (false,false,true);
		
		Debug.LogError ("NOW: Start Blinking en GoToSearchPapelAbsorbentePhase1Step3InfoMode");
		//asignando el texto para el feedback directamente a la interfaz:
		blinking_search_phase1step3.feedback_info_text_path = text_feedback_marker25_dc3430;
		
		//definiendo los ordenes que controlara esta interfaz:
		order_in_process = new int[1];
		order_in_process [0] = 6;
		blinking_search_phase1step3.ordenes = order_in_process;
		
		//Asignando el metodo que se debe ejecutar cuando se hace click sobre el btn Select de la interfaz
		blinking_search_phase1step3.interface_going_from = "Phase1Step3";  //la variable Phase1Step2 permite obtener la interfaz para obtenerla en OnBackButtonTapped
		blinking_search_phase1step3.onClickSelectBtn += OnClickSelectPapelAbsorbenteInfoMode;
		
		//iniciando el proceso blinking:
		blinking_search_phase1step3.should_be_blinking = true;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "II35", "3", "-1","consulta");
	}//cierra GoToSearchPapelAbsorbentePhase1Step3InfoMode

	/// <summary>
	/// Method that is called in order to start the AR mode for searching the tools of Phase 1 - Step3 (agua a presion)
	/// </summary>
	public void GoToSearchAguaPresionPhase1Step3InfoMode(){
		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_IM) {
			Destroy(ToolsAndProductsPhase1Step3_int_im_instance);
		}
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE1_STEP3_IM;
		
		//indica que entramos en la fase 2 del tutorial:
		AppManager.manager.inTutorialPhase2 = false;
		
		Debug.Log ("--> Iniciando modo RA en GoToSearchAguaPresionPhase1Step3InfoMode");
		
		//indica que se ingresa al modo RA fuera de los tutoriales:
		AppManager.manager.in_RA_mode = true;
		
		//ahora aqui debo instanciar la interfaz del tutorial 2 y asignarle todas las propiedades:
		AR_Mode_Search_int_im_instance = Instantiate (AppManager.manager.AR_Mode_interface);
		ControllerBlinkingARGeneric blinking_search_phase1step3 = AR_Mode_Search_int_im_instance.GetComponent<ControllerBlinkingARGeneric> ();
		
		Debug.Log ("Nueva Interfaz Instanciada en GoToSearchAguaPresionPhase1Step3InfoMode!!");
		
		//hay que asignar la interfaz activa tambien:
		AppManager.manager.interfaceInstanceActive = AR_Mode_Search_int_im_instance;
		
		//localizando el marcador que se debe buscar en esta fase del proceso y cargando datos respectivos:
		markerInScene = GameObject.Find ("FrameMarker16");
		ControllerAddInfoInMarker controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		
		//NOTA: antes de hacer esto es importante que la variable interfaceInstanceActive este asignada:
		//tambien es importante haber asignado la variable inTutorialPhase1
		controller_info_marker.LoadInformationToInterface (false,false,true);
		
		Debug.LogError ("NOW: Start Blinking en GoToSearchAguaPresionPhase1Step3InfoMode");
		//asignando el texto para el feedback directamente a la interfaz:
		blinking_search_phase1step3.feedback_info_text_path = feedback_phase1step3_agua;
		
		//definiendo los ordenes que controlara esta interfaz:
		order_in_process = new int[1];
		order_in_process [0] = 5;
		blinking_search_phase1step3.ordenes = order_in_process;
		
		//Asignando el metodo que se debe ejecutar cuando se hace click sobre el btn Select de la interfaz
		blinking_search_phase1step3.interface_going_from = "Phase1Step3";  //la variable Phase1Step3 permite obtener la interfaz para obtenerla en OnBackButtonTapped
		blinking_search_phase1step3.onClickSelectBtn += OnClickSelectAguaPresionInfoMode;
		
		//iniciando el proceso blinking:
		blinking_search_phase1step3.should_be_blinking = true;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "II34", "2", "-1","consulta");
	} //cierra metodo de agua a presion GoToSearchAguaPresionPhase1Step3InfoMode


	/// <summary>
	/// Method that is called in order to start the AR mode for searching the tools of Phase 1 - Step4 (lija fina)
	/// </summary>
	public void GoToSearchLijaFinaPhase1Step4InfoMode(){
		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_IM) {
			Destroy(ToolsAndProductsPhase1Step4_int_im_instance);
		}
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE1_STEP4_IM;
		
		//indica que entramos en la fase 2 del tutorial:
		AppManager.manager.inTutorialPhase2 = false;
		Debug.Log ("--> Iniciando modo RA en GoToSearchLijaFinaPhase1Step4InfoMode");
		//indica que se ingresa al modo RA fuera de los tutoriales:
		AppManager.manager.in_RA_mode = true;
		
		//ahora aqui debo instanciar la interfaz del tutorial 2 y asignarle todas las propiedades:
		AR_Mode_Search_int_im_instance = Instantiate (AppManager.manager.AR_Mode_interface);
		ControllerBlinkingARGeneric blinking_search_phase1step4 = AR_Mode_Search_int_im_instance.GetComponent<ControllerBlinkingARGeneric> ();
		
		Debug.Log ("Nueva Interfaz Instanciada en GoToSearchLijaFinaPhase1Step4InfoMode!!");
		
		//hay que asignar la interfaz activa tambien:
		AppManager.manager.interfaceInstanceActive = AR_Mode_Search_int_im_instance;
		
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
		order_in_process [0] = 7;
		blinking_search_phase1step4.ordenes = order_in_process;
		
		//Asignando el metodo que se debe ejecutar cuando se hace click sobre el btn Select de la interfaz
		blinking_search_phase1step4.interface_going_from = "Phase1Step4";  //la variable Phase1Step3 permite obtener la interfaz para obtenerla en OnBackButtonTapped
		blinking_search_phase1step4.onClickSelectBtn += OnClickSelectLijaFinaInfoMode;
		
		//iniciando el proceso blinking:
		blinking_search_phase1step4.should_be_blinking = true;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "II36", "3", "-1","consulta");
	} //cierra metodo de busqueda de la lija


	/// <summary>
	/// Method that is called in order to start the AR mode for searching the tools of Phase 1 - Step5 (martillo)
	/// </summary>
	public void GoToSearchMartilloPhase1Step5InfoMode(){
		
		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_IM) {
			Destroy(ToolsAndProductsPhase1Step5_int_im_instance);
		}
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE1_STEP5_IM;
		
		//indica que entramos en la fase 2 del tutorial:
		AppManager.manager.inTutorialPhase2 = false;
		Debug.Log ("--> InformativeMode: Iniciando modo RA en GoToSearchMartilloPhase1Step5InfoMode");
		//indica que se ingresa al modo RA fuera de los tutoriales:
		AppManager.manager.in_RA_mode = true;
		
		//ahora aqui debo instanciar la interfaz del tutorial 2 y asignarle todas las propiedades:
		AR_Mode_Search_int_im_instance = Instantiate (AppManager.manager.AR_Mode_interface);
		ControllerBlinkingARGeneric blinking_search_phase1step5 = AR_Mode_Search_int_im_instance.GetComponent<ControllerBlinkingARGeneric> ();
		
		Debug.Log ("InformativeMode: Nueva Interfaz Instanciada en GoToSearchMartilloPhase1Step5InfoMode!!");
		
		//hay que asignar la interfaz activa tambien:
		AppManager.manager.interfaceInstanceActive = AR_Mode_Search_int_im_instance;
		
		//localizando el marcador que se debe buscar en esta fase del proceso y cargando datos respectivos:
		markerInScene = GameObject.Find ("FrameMarker100");
		ControllerAddInfoInMarker controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		
		//NOTA: antes de hacer esto es importante que la variable interfaceInstanceActive este asignada:
		//tambien es importante haber asignado la variable inTutorialPhase1
		controller_info_marker.LoadInformationToInterface (false,false,true);
		
		Debug.LogError ("NOW: Start Blinking en GoToSearchMartilloPhase1Step5InfoMode");
		//asignando el texto para el feedback directamente a la interfaz:
		blinking_search_phase1step5.feedback_info_text_path = text_feedback_marker6_martillo;
		
		//definiendo los ordenes que controlara esta interfaz:
		order_in_process = new int[1];
		order_in_process [0] = 8;
		blinking_search_phase1step5.ordenes = order_in_process;
		
		//Asignando el metodo que se debe ejecutar cuando se hace click sobre el btn Select de la interfaz
		blinking_search_phase1step5.interface_going_from = "Phase1Step5";  //la variable Phase1Step3 permite obtener la interfaz para obtenerla en OnBackButtonTapped
		blinking_search_phase1step5.onClickSelectBtn += OnClickSelectMartilloInfoMode;
		
		//iniciando el proceso blinking:
		blinking_search_phase1step5.should_be_blinking = true;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "II37", "4", "-1","consulta");
	} //cierra metodo de busqueda del martillo


	/// <summary>
	/// Method that is called in order to start the AR mode for searching the tools of Phase 1 - Step6 (desengrasante)
	/// </summary>
	public void GoToSearchDesengrasantePhase1Step6InfoMode(){
		
		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_IM) {
			Destroy(ToolsAndProductsPhase1Step6_int_im_instance);
		}
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE1_STEP6_IM;
		
		//indica que entramos en la fase 2 del tutorial:
		AppManager.manager.inTutorialPhase2 = false;
		Debug.Log ("--> InformativeMode: Iniciando modo RA en GoToSearchDesengrasantePhase1Step6InfoMode");
		//indica que se ingresa al modo RA fuera de los tutoriales:
		AppManager.manager.in_RA_mode = true;
		
		//ahora aqui debo instanciar la interfaz del tutorial 2 y asignarle todas las propiedades:
		AR_Mode_Search_int_im_instance = Instantiate (AppManager.manager.AR_Mode_interface);
		ControllerBlinkingARGeneric blinking_search_phase1step6 = AR_Mode_Search_int_im_instance.GetComponent<ControllerBlinkingARGeneric> ();
		
		Debug.Log ("Nueva Interfaz Instanciada en GoToSearchDesengrasantePhase1Step6InfoMode!!");
		
		//hay que asignar la interfaz activa tambien:
		AppManager.manager.interfaceInstanceActive = AR_Mode_Search_int_im_instance;
		
		//localizando el marcador que se debe buscar en esta fase del proceso y cargando datos respectivos:
		markerInScene = GameObject.Find ("FrameMarker26");
		ControllerAddInfoInMarker controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		
		//NOTA: antes de hacer esto es importante que la variable interfaceInstanceActive este asignada:
		//tambien es importante haber asignado la variable inTutorialPhase1
		controller_info_marker.LoadInformationToInterface (false,false,true);
		
		Debug.LogError ("NOW: Start Blinking en GoToSearchDesengrasantePhase1Step6InfoMode");
		//asignando el texto para el feedback directamente a la interfaz:
		blinking_search_phase1step6.feedback_info_text_path = text_feedback_marker26_desengras;
		
		//definiendo los ordenes que controlara esta interfaz:
		order_in_process = new int[1];
		order_in_process [0] = 9;
		blinking_search_phase1step6.ordenes = order_in_process;
		
		//Asignando el metodo que se debe ejecutar cuando se hace click sobre el btn Select de la interfaz
		blinking_search_phase1step6.interface_going_from = "Phase1Step6";  //la variable Phase1Step3 permite obtener la interfaz para obtenerla en OnBackButtonTapped
		blinking_search_phase1step6.onClickSelectBtn += OnClickSelectDesengrasanteInfoMode;
		
		//iniciando el proceso blinking:
		blinking_search_phase1step6.should_be_blinking = true;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "II38", "5", "-1","consulta");
	} //cierra metodo de busqueda del desengrasante


	/// <summary>
	/// Method that is called in order to start the AR mode for searching the tools of Phase 1 - Step6 (desengrasante)
	/// </summary>
	public void GoToSearchBayetaPhase1Step6InfoMode(){
		
		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_IM) {
			Destroy(ToolsAndProductsPhase1Step6_int_im_instance);
		} else if (current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP6_IM)
			Destroy (AR_Mode_Search_int_im_instance);
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE1_STEP6_IM;
		
		//indica que entramos en la fase 2 del tutorial:
		AppManager.manager.inTutorialPhase2 = false;
		Debug.Log ("--> InformativeMode: Iniciando modo RA en GoToSearchBayetaPhase1Step6InfoMode");
		//indica que se ingresa al modo RA fuera de los tutoriales:
		AppManager.manager.in_RA_mode = true;
		
		//ahora aqui debo instanciar la interfaz del tutorial 2 y asignarle todas las propiedades:
		AR_Mode_Search_int_im_instance = Instantiate (AppManager.manager.AR_Mode_interface);
		ControllerBlinkingARGeneric blinking_search_phase1step6 = AR_Mode_Search_int_im_instance.GetComponent<ControllerBlinkingARGeneric> ();
		
		Debug.Log ("InformativeMode: Nueva Interfaz Instanciada en GoToSearchBayetaPhase1Step6InfoMode!!");
		
		//hay que asignar la interfaz activa tambien:
		AppManager.manager.interfaceInstanceActive = AR_Mode_Search_int_im_instance;
		
		//localizando el marcador que se debe buscar en esta fase del proceso y cargando datos respectivos:
		markerInScene = GameObject.Find ("FrameMarker25");
		ControllerAddInfoInMarker controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		
		//NOTA: antes de hacer esto es importante que la variable interfaceInstanceActive este asignada:
		//tambien es importante haber asignado la variable inTutorialPhase1
		controller_info_marker.LoadInformationToInterface (false,false,true);
		
		Debug.LogError ("NOW: Start Blinking en GoToSearchBayetaPhase1Step6InfoMode");
		//asignando el texto para el feedback directamente a la interfaz:
		blinking_search_phase1step6.feedback_info_text_path = text_feedback_marker26_desengras;
		
		//definiendo los ordenes que controlara esta interfaz:
		order_in_process = new int[1];
		order_in_process [0] = 10;
		blinking_search_phase1step6.ordenes = order_in_process;
		
		//Asignando el metodo que se debe ejecutar cuando se hace click sobre el btn Select de la interfaz
		blinking_search_phase1step6.interface_going_from = "Phase1Step6";  //la variable Phase1Step3 permite obtener la interfaz para obtenerla en OnBackButtonTapped
		blinking_search_phase1step6.onClickSelectBtn += OnClickSelectBayetaStep6InfoMode;
		
		//iniciando el proceso blinking:
		blinking_search_phase1step6.should_be_blinking = true;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "II39", "5", "-1","consulta");
	} //cierra metodo de busqueda de la bayeta segunda parte (paso 6)


	/// <summary>
	/// Method that is called in order to start the AR mode for searching the tools of Phase 2 - Step2 (MATIZADO - Proteccion de la superficie) Parte 1 - Cinta de enmascarar
	/// </summary>
	public void GoToSearchCintaEnmascPhase2Step2InfoMode(){
		
		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_IM) {
			Destroy(ToolsAndProductsPhase2Step2_int_im_instance);
		}
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE2_STEP2_IM;
		
		//indica que entramos en la fase 2 del tutorial:
		AppManager.manager.inTutorialPhase2 = false;
		Debug.Log ("--> Iniciando modo RA en GoToSearchCintaEnmascPhase2Step2");
		//indica que se ingresa al modo RA fuera de los tutoriales:
		AppManager.manager.in_RA_mode = true;
		
		//ahora aqui debo instanciar la interfaz del tutorial 2 y asignarle todas las propiedades:
		AR_Mode_Search_int_im_instance = Instantiate (AppManager.manager.AR_Mode_interface);
		ControllerBlinkingARGeneric blinking_search_phase2step2 = AR_Mode_Search_int_im_instance.GetComponent<ControllerBlinkingARGeneric> ();
		
		Debug.Log ("Nueva Interfaz Instanciada en GoToSearchCintaEnmascPhase2Step2!!");
		
		//hay que asignar la interfaz activa tambien:
		AppManager.manager.interfaceInstanceActive = AR_Mode_Search_int_im_instance;
		
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
		order_in_process [0] = 11;
		blinking_search_phase2step2.ordenes = order_in_process;
		
		//Asignando el metodo que se debe ejecutar cuando se hace click sobre el btn Select de la interfaz
		blinking_search_phase2step2.interface_going_from = "Phase2Step2";  //la variable Phase1Step3 permite obtener la interfaz para obtenerla en OnBackButtonTapped
		blinking_search_phase2step2.onClickSelectBtn += OnClickSelectCintaEmascararInfom;
		
		//iniciando el proceso blinking:
		blinking_search_phase2step2.should_be_blinking = true;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "II40", "7", "-1","consulta");
	} //cierra metodo de busqueda del desengrasante
	
	/// <summary>
	/// Method that is called in order to start the AR mode for searching the tools of Phase 2 - Step2 (MATIZADO - Proteccion de la superficie) Parte 2: papel de enmascarar
	/// </summary>
	public void GoToSearchPapelEnmascPhase2Step2InfoMode(){
		
		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_IM) {
			Destroy(ToolsAndProductsPhase2Step2_int_im_instance);
		} else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP2_IM)
			Destroy (AR_Mode_Search_int_im_instance);
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE2_STEP2_IM;
		
		//indica que entramos en la fase 2 del tutorial:
		AppManager.manager.inTutorialPhase2 = false;
		Debug.Log ("--> Iniciando modo RA en GoToSearchPapelEnmascPhase2Step2");
		//indica que se ingresa al modo RA fuera de los tutoriales:
		AppManager.manager.in_RA_mode = true;

		//ahora aqui debo instanciar la interfaz del tutorial 2 y asignarle todas las propiedades:
		AR_Mode_Search_int_im_instance = Instantiate (AppManager.manager.AR_Mode_interface);
		ControllerBlinkingARGeneric blinking_search_phase2step2_second = AR_Mode_Search_int_im_instance.GetComponent<ControllerBlinkingARGeneric> ();
		
		Debug.Log ("Nueva Interfaz Instanciada en GoToSearchPapelEnmascPhase2Step2!!");
		
		//hay que asignar la interfaz activa tambien:
		AppManager.manager.interfaceInstanceActive = AR_Mode_Search_int_im_instance;
		
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
		order_in_process [0] = 12;
		blinking_search_phase2step2_second.ordenes = order_in_process;
		
		//Asignando el metodo que se debe ejecutar cuando se hace click sobre el btn Select de la interfaz
		blinking_search_phase2step2_second.interface_going_from = "Phase2Step2";  //la variable Phase1Step3 permite obtener la interfaz para obtenerla en OnBackButtonTapped
		blinking_search_phase2step2_second.onClickSelectBtn += OnClickSelectPapelEnmascPhase2Step2Infom;
		
		//iniciando el proceso blinking:
		blinking_search_phase2step2_second.should_be_blinking = true;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "II41", "7", "-1","consulta");
	} //cierra GoToSearchPapelEnmascPhase2Step2
	
	public void GoToSearchEsponjaP320Phase2Step3InfoMode(){
		
		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_IM) {
			Destroy(ToolsAndProductsPhase2Step3_int_im_instance);
		} else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP3_IM)
			Destroy (AR_Mode_Search_int_im_instance);
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE2_STEP3_IM;
		
		//indica que entramos en la fase 2 del tutorial:
		AppManager.manager.inTutorialPhase2 = false;
		Debug.Log ("--> Iniciando modo RA en GoToSearchEsponjaP320Phase2Step3");
		//indica que se ingresa al modo RA fuera de los tutoriales:
		AppManager.manager.in_RA_mode = true;
		
		//ahora aqui debo instanciar la interfaz del tutorial 2 y asignarle todas las propiedades:
		AR_Mode_Search_int_im_instance = Instantiate (AppManager.manager.AR_Mode_interface);
		ControllerBlinkingARGeneric blinking_search_phase2step3 = AR_Mode_Search_int_im_instance.GetComponent<ControllerBlinkingARGeneric> ();
		
		Debug.Log ("Nueva Interfaz Instanciada en GoToSearchPapelEnmascPhase2Step2!!");
		
		//hay que asignar la interfaz activa tambien:
		AppManager.manager.interfaceInstanceActive = AR_Mode_Search_int_im_instance;
		
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
		order_in_process [0] = 13;
		blinking_search_phase2step3.ordenes = order_in_process;
		
		//Asignando el metodo que se debe ejecutar cuando se hace click sobre el btn Select de la interfaz
		blinking_search_phase2step3.interface_going_from = "Phase2Step3";  //la variable Phase1Step3 permite obtener la interfaz para obtenerla en OnBackButtonTapped
		blinking_search_phase2step3.onClickSelectBtn += OnClickSelectEsponjaPhase2Step3Infom;
		
		//iniciando el proceso blinking:
		blinking_search_phase2step3.should_be_blinking = true;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "II42", "9", "-1","consulta");
	} //cierra GoToSearchEsponjaP320Phase2Step3
	
	
	public void GoToSearchObjetoLimpiezaPhase2Step3Informative(){
		
		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_IM) {
			Destroy(ToolsAndProductsPhase2Step3_int_im_instance);
		} else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP3_IM)
			Destroy (AR_Mode_Search_int_im_instance);
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE2_STEP3_IM;
		
		//indica que entramos en la fase 2 del tutorial:
		AppManager.manager.inTutorialPhase2 = false;
		Debug.Log ("--> Iniciando modo RA en GoToSearchObjetoLimpiezaPhase2Step3");
		//indica que se ingresa al modo RA fuera de los tutoriales:
		AppManager.manager.in_RA_mode = true;
		
		//ahora aqui debo instanciar la interfaz del tutorial 2 y asignarle todas las propiedades:
		AR_Mode_Search_int_im_instance = Instantiate (AppManager.manager.AR_Mode_interface);
		ControllerBlinkingARGeneric blinking_search_phase2step3_second = AR_Mode_Search_int_im_instance.GetComponent<ControllerBlinkingARGeneric> ();
		
		Debug.Log ("Nueva Interfaz Instanciada en GoToSearchObjetoLimpiezaPhase2Step3!!");
		
		//hay que asignar la interfaz activa tambien:
		AppManager.manager.interfaceInstanceActive = AR_Mode_Search_int_im_instance;
		
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
		order_in_process [0] = 14;
		blinking_search_phase2step3_second.ordenes = order_in_process;
		
		//Asignando el metodo que se debe ejecutar cuando se hace click sobre el btn Select de la interfaz
		blinking_search_phase2step3_second.interface_going_from = "Phase2Step3";  //la variable Phase1Step3 permite obtener la interfaz para obtenerla en OnBackButtonTapped
		blinking_search_phase2step3_second.onClickSelectBtn += OnClickSelectObjetoLimpiezaPhase2Step3inform;
		
		//iniciando el proceso blinking:
		blinking_search_phase2step3_second.should_be_blinking = true;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "II43", "9", "-1","consulta");
	} //cierra metodo GoToSearchObjetoLimpiezaPhase2Step3
	
	
	public void GoToSearchEsponjaP400Phase2Step4Informative(){
		
		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_IM) {
			Destroy(ToolsAndProductsPhase2Step4_int_im_instance);
		} else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP4_IM)
			Destroy (AR_Mode_Search_int_im_instance);
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE2_STEP4_IM;
		
		//indica que entramos en la fase 2 del tutorial:
		AppManager.manager.inTutorialPhase2 = false;
		Debug.Log ("--> Iniciando modo RA en GoToSearchEsponjaP400Phase2Step4");
		//indica que se ingresa al modo RA fuera de los tutoriales:
		AppManager.manager.in_RA_mode = true;
		
		//ahora aqui debo instanciar la interfaz del tutorial 2 y asignarle todas las propiedades:
		AR_Mode_Search_int_im_instance = Instantiate (AppManager.manager.AR_Mode_interface);
		ControllerBlinkingARGeneric blinking_search_phase2step4 = AR_Mode_Search_int_im_instance.GetComponent<ControllerBlinkingARGeneric> ();
		
		Debug.Log ("Nueva Interfaz Instanciada en GoToSearchEsponjaP400Phase2Step4!!");
		
		//hay que asignar la interfaz activa tambien:
		AppManager.manager.interfaceInstanceActive = AR_Mode_Search_int_im_instance;
		
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
		order_in_process [0] = 15;
		blinking_search_phase2step4.ordenes = order_in_process;
		
		//Asignando el metodo que se debe ejecutar cuando se hace click sobre el btn Select de la interfaz
		blinking_search_phase2step4.interface_going_from = "Phase2Step4";  //la variable Phase1Step3 permite obtener la interfaz para obtenerla en OnBackButtonTapped
		blinking_search_phase2step4.onClickSelectBtn += OnClickSelectEsponjaP400Phase2Step4Infom;
		
		//iniciando el proceso blinking:
		blinking_search_phase2step4.should_be_blinking = true;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "II44", "10", "-1","consulta");
	} //cierra  GoToSearchEsponjaP400Phase2Step4
	
	
	public void GoToSearchObjetoLimpiezaPhase2Step4Informative(){
		
		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_IM) {
			Destroy(ToolsAndProductsPhase2Step4_int_im_instance);
		} else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP4_IM)
			Destroy (AR_Mode_Search_int_im_instance);
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE2_STEP4_IM;
		
		//indica que entramos en la fase 2 del tutorial:
		AppManager.manager.inTutorialPhase2 = false;
		Debug.Log ("--> Iniciando modo RA en GoToSearchObjetoLimpiezaPhase2Step4");
		//indica que se ingresa al modo RA fuera de los tutoriales:
		AppManager.manager.in_RA_mode = true;
		
		//ahora aqui debo instanciar la interfaz del tutorial 2 y asignarle todas las propiedades:
		AR_Mode_Search_int_im_instance = Instantiate (AppManager.manager.AR_Mode_interface);
		ControllerBlinkingARGeneric blinking_search_phase2step4_second = AR_Mode_Search_int_im_instance.GetComponent<ControllerBlinkingARGeneric> ();
		
		Debug.Log ("Nueva Interfaz Instanciada en GoToSearchObjetoLimpiezaPhase2Step4!!");
		
		//hay que asignar la interfaz activa tambien:
		AppManager.manager.interfaceInstanceActive = AR_Mode_Search_int_im_instance;
		
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
		order_in_process [0] = 16;
		blinking_search_phase2step4_second.ordenes = order_in_process;
		
		//Asignando el metodo que se debe ejecutar cuando se hace click sobre el btn Select de la interfaz
		blinking_search_phase2step4_second.interface_going_from = "Phase2Step4";  //la variable Phase1Step3 permite obtener la interfaz para obtenerla en OnBackButtonTapped
		blinking_search_phase2step4_second.onClickSelectBtn += OnClickSelectObjetoLimpiezaPhase2Step4Infom;
		
		//iniciando el proceso blinking:
		blinking_search_phase2step4_second.should_be_blinking = true;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "II45", "10", "-1","consulta");
	} //cierra GoToSearchObjetoLimpiezaPhase2Step4
	
	
	public void GoToSearchRotoOrbitalPhase2Step5Informative(){
		
		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_IM) {
			Destroy(ToolsAndProductsPhase2Step5_int_im_instance);
		} else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP5_IM)
			Destroy (AR_Mode_Search_int_im_instance);
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE2_STEP5_IM;
		
		//indica que entramos en la fase 2 del tutorial:
		AppManager.manager.inTutorialPhase2 = false;
		Debug.Log ("--> Iniciando modo RA en GoToSearchRotoOrbitalPhase2Step5");
		//indica que se ingresa al modo RA fuera de los tutoriales:
		AppManager.manager.in_RA_mode = true;
		
		//ahora aqui debo instanciar la interfaz del tutorial 2 y asignarle todas las propiedades:
		AR_Mode_Search_int_im_instance = Instantiate (AppManager.manager.AR_Mode_interface);
		ControllerBlinkingARGeneric blinking_search_phase2step5 = AR_Mode_Search_int_im_instance.GetComponent<ControllerBlinkingARGeneric> ();
		
		Debug.Log ("Nueva Interfaz Instanciada en GoToSearchRotoOrbitalPhase2Step5!!");
		
		//hay que asignar la interfaz activa tambien:
		AppManager.manager.interfaceInstanceActive = AR_Mode_Search_int_im_instance;
		
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
		order_in_process [0] = 17;
		blinking_search_phase2step5.ordenes = order_in_process;
		
		//Asignando el metodo que se debe ejecutar cuando se hace click sobre el btn Select de la interfaz
		blinking_search_phase2step5.interface_going_from = "Phase2Step5";  //la variable Phase1Step3 permite obtener la interfaz para obtenerla en OnBackButtonTapped
		blinking_search_phase2step5.onClickSelectBtn += OnClickSelectRotoOrbitalPhase2Step5Infom;
		
		//iniciando el proceso blinking:
		blinking_search_phase2step5.should_be_blinking = true;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "II47", "11", "-1","consulta");
	} //cierra GoToSearchRotoOrbitalPhase2Step5
	
	
	public void GoToSearchDiscoP80Phase2Step5Informative(){
		
		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_IM) {
			Destroy(ToolsAndProductsPhase2Step5_int_im_instance);
		} else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP5_IM)
			Destroy (AR_Mode_Search_int_im_instance);
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE2_STEP5_IM;
		
		//indica que entramos en la fase 2 del tutorial:
		AppManager.manager.inTutorialPhase2 = false;
		Debug.Log ("--> Iniciando modo RA en GoToSearchRotoOrbitalPhase2Step5");
		//indica que se ingresa al modo RA fuera de los tutoriales:
		AppManager.manager.in_RA_mode = true;

		//ahora aqui debo instanciar la interfaz del tutorial 2 y asignarle todas las propiedades:
		AR_Mode_Search_int_im_instance = Instantiate (AppManager.manager.AR_Mode_interface);
		ControllerBlinkingARGeneric blinking_search_phase2step5_second = AR_Mode_Search_int_im_instance.GetComponent<ControllerBlinkingARGeneric> ();
		
		Debug.Log ("Nueva Interfaz Instanciada en GoToSearchRotoOrbitalPhase2Step5!!");
		//hay que asignar la interfaz activa tambien:
		AppManager.manager.interfaceInstanceActive = AR_Mode_Search_int_im_instance;
		
		//localizando el marcador que se debe buscar en esta fase del proceso y cargando datos respectivos:
		markerInScene = GameObject.Find ("FrameMarker30");
		ControllerAddInfoInMarker controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		controller_info_marker.image_marker_path = "Sprites/markers/frameMarker30_32_p80_p120";
		controller_info_marker.image_marker_real_path = "Sprites/phase2step5/FrameMarker30_32_disco_p80_p120";
		//NOTA: antes de hacer esto es importante que la variable interfaceInstanceActive este asignada:
		//tambien es importante haber asignado la variable inTutorialPhase1
		controller_info_marker.LoadInformationToInterface (false,false,true);
		
		Debug.LogError ("NOW: Start Blinking en GoToSearchObjetoLimpiezaPhase2Step5");
		//asignando el texto para el feedback directamente a la interfaz:
		blinking_search_phase2step5_second.feedback_info_text_path = text_feedback_phase2step5;
		
		//definiendo los ordenes que controlara esta interfaz:
		order_in_process = new int[1];
		order_in_process [0] = 18;
		blinking_search_phase2step5_second.ordenes = order_in_process;
		
		//Asignando el metodo que se debe ejecutar cuando se hace click sobre el btn Select de la interfaz
		blinking_search_phase2step5_second.interface_going_from = "Phase2Step5";  //la variable Phase1Step3 permite obtener la interfaz para obtenerla en OnBackButtonTapped
		blinking_search_phase2step5_second.onClickSelectBtn += OnClickSelectDiscoP80Phase2Step5Infom;
		
		//iniciando el proceso blinking:
		blinking_search_phase2step5_second.should_be_blinking = true;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "II48", "11", "-1","consulta");
	} //cierra GoToSearchDiscoP80Phase2Step5
	
	
	public void GoToSearchObjetoLimpiezaPhase2Step5Informative(){
		
		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_IM) {
			Destroy(ToolsAndProductsPhase2Step5_int_im_instance);
		} else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP5_IM)
			Destroy (AR_Mode_Search_int_im_instance);
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE2_STEP5_IM;
		
		//indica que entramos en la fase 2 del tutorial:
		AppManager.manager.inTutorialPhase2 = false;
		Debug.Log ("--> Iniciando modo RA en GoToSearchRotoOrbitalPhase2Step5");
		//indica que se ingresa al modo RA fuera de los tutoriales:
		AppManager.manager.in_RA_mode = true;
		
		//ahora aqui debo instanciar la interfaz del tutorial 2 y asignarle todas las propiedades:
		AR_Mode_Search_int_im_instance = Instantiate (AppManager.manager.AR_Mode_interface);
		ControllerBlinkingARGeneric blinking_search_phase2step5_third = AR_Mode_Search_int_im_instance.GetComponent<ControllerBlinkingARGeneric> ();
		
		Debug.Log ("Nueva Interfaz Instanciada en GoToSearchRotoOrbitalPhase2Step5!!");
		//hay que asignar la interfaz activa tambien:
		AppManager.manager.interfaceInstanceActive = AR_Mode_Search_int_im_instance;
		
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
		order_in_process [0] = 19;
		blinking_search_phase2step5_third.ordenes = order_in_process;
		
		//Asignando el metodo que se debe ejecutar cuando se hace click sobre el btn Select de la interfaz
		blinking_search_phase2step5_third.interface_going_from = "Phase2Step5";  //la variable Phase1Step3 permite obtener la interfaz para obtenerla en OnBackButtonTapped
		blinking_search_phase2step5_third.onClickSelectBtn += OnClickSelectObjLimpiezaPhase2Step5Infom;
		
		//iniciando el proceso blinking:
		blinking_search_phase2step5_third.should_be_blinking = true;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "II49", "11", "-1","consulta");
	}
	
	
	public void GoToSearchDiscoP150Phase2Step6Informative(){
		
		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_IM) {
			Destroy(ToolsAndProductsPhase2Step6_int_im_instance);
		} else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP6_IM)
			Destroy (AR_Mode_Search_int_im_instance);
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE2_STEP6_IM;
		
		//indica que entramos en la fase 2 del tutorial:
		AppManager.manager.inTutorialPhase2 = false;
		Debug.Log ("--> Iniciando modo RA en GoToSearchDiscoP150Phase2Step6");
		//indica que se ingresa al modo RA fuera de los tutoriales:
		AppManager.manager.in_RA_mode = true;
		
		//ahora aqui debo instanciar la interfaz del tutorial 2 y asignarle todas las propiedades:
		AR_Mode_Search_int_im_instance = Instantiate (AppManager.manager.AR_Mode_interface);
		ControllerBlinkingARGeneric blinking_search_phase2step6 = AR_Mode_Search_int_im_instance.GetComponent<ControllerBlinkingARGeneric> ();
		
		Debug.Log ("Nueva Interfaz Instanciada en GoToSearchDiscoP150Phase2Step6!!");
		//hay que asignar la interfaz activa tambien:
		AppManager.manager.interfaceInstanceActive = AR_Mode_Search_int_im_instance;
		
		//localizando el marcador que se debe buscar en esta fase del proceso y cargando datos respectivos:
		markerInScene = GameObject.Find ("FrameMarker33");
		ControllerAddInfoInMarker controller_info_marker = markerInScene.GetComponent<ControllerAddInfoInMarker> ();
		if (AppManager.manager.last_markerid_scanned == 32) {
			Debug.Log ("-->SEQUENCE:: Se va a cargar la informacion del DISCO P180 y el last_marker es:" + AppManager.manager.last_markerid_scanned);
			controller_info_marker.image_marker_path = "Sprites/markers/frameMarker34_disco_p180";
			controller_info_marker.image_marker_real_path = "Sprites/phase2step6/FrameMarker34_disco_p180";
		} else {
			Debug.Log ("--> SEQUENCE: El last marker scanned fue: " + AppManager.manager.last_markerid_scanned);
			controller_info_marker.image_marker_path = "Sprites/markers/frameMarker33_disco_p150";
			controller_info_marker.image_marker_real_path = "Sprites/phase2step6/FrameMarker33_disco_p150";
		}
		//NOTA: antes de hacer esto es importante que la variable interfaceInstanceActive este asignada:
		//tambien es importante haber asignado la variable inTutorialPhase1
		controller_info_marker.LoadInformationToInterface (false,false,true);
		
		//NOTA: Especialmente para este marcador se va a habilitar la validacion de secuencia:
		blinking_search_phase2step6.validate_sequence_of_markers = true;
		blinking_search_phase2step6.previous_marker_id = AppManager.manager.last_markerid_scanned;
		if (AppManager.manager.last_markerid_scanned == 30)
			blinking_search_phase2step6.next_marker_id = 33;
		else
			blinking_search_phase2step6.next_marker_id = 34;
		
		
		Debug.LogError ("NOW: Start Blinking en GoToSearchDiscoP150Phase2Step6");
		//asignando el texto para el feedback directamente a la interfaz:
		if(AppManager.manager.last_markerid_scanned == 32)
			blinking_search_phase2step6.feedback_info_text_path = text_feedback_phase2step6_p180;
		else blinking_search_phase2step6.feedback_info_text_path = text_feedback_phase2step6;
		
		//definiendo los ordenes que controlara esta interfaz:
		order_in_process = new int[1];
		order_in_process [0] = 20;
		blinking_search_phase2step6.ordenes = order_in_process;
		
		//Asignando el metodo que se debe ejecutar cuando se hace click sobre el btn Select de la interfaz
		blinking_search_phase2step6.interface_going_from = "Phase2Step6";  //la variable Phase1Step3 permite obtener la interfaz para obtenerla en OnBackButtonTapped
		blinking_search_phase2step6.onClickSelectBtn += OnClickSelectDiscoP150Phase2Step6Infom;
		
		//iniciando el proceso blinking:
		blinking_search_phase2step6.should_be_blinking = true;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "II50", "12", "-1","consulta");
	} //cierra GoToSearchDiscoP150Phase2Step6
	
	
	public void GoToSearchObjetoLimpiezaPhase2Step6Informative(){
		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_IM) {
			Destroy(ToolsAndProductsPhase2Step6_int_im_instance);
		} else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP6_IM)
			Destroy (AR_Mode_Search_int_im_instance);
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE2_STEP6_IM;
		
		//indica que entramos en la fase 2 del tutorial:
		AppManager.manager.inTutorialPhase2 = false;
		Debug.Log ("--> Iniciando modo RA en GoToSearchObjetoLimpiezaPhase2Step6");
		//indica que se ingresa al modo RA fuera de los tutoriales:
		AppManager.manager.in_RA_mode = true;
		
		//ahora aqui debo instanciar la interfaz del tutorial 2 y asignarle todas las propiedades:
		AR_Mode_Search_int_im_instance = Instantiate (AppManager.manager.AR_Mode_interface);
		ControllerBlinkingARGeneric blinking_search_phase2step6_second = AR_Mode_Search_int_im_instance.GetComponent<ControllerBlinkingARGeneric> ();
		
		Debug.Log ("Nueva Interfaz Instanciada en GoToSearchDiscoP150Phase2Step6!!");
		//hay que asignar la interfaz activa tambien:
		AppManager.manager.interfaceInstanceActive = AR_Mode_Search_int_im_instance;
		
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
		order_in_process [0] = 21;
		blinking_search_phase2step6_second.ordenes = order_in_process;
		
		//Asignando el metodo que se debe ejecutar cuando se hace click sobre el btn Select de la interfaz
		blinking_search_phase2step6_second.interface_going_from = "Phase2Step6";  //la variable Phase1Step3 permite obtener la interfaz para obtenerla en OnBackButtonTapped
		blinking_search_phase2step6_second.onClickSelectBtn += OnClickSelectObjLimpiezaPhase2Step6Infom;
		
		//iniciando el proceso blinking:
		blinking_search_phase2step6_second.should_be_blinking = true;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "II51", "12", "-1","consulta");
	}//cierra GoToSearchObjetoLimpiezaPhase2Step6
	
	
	public void GoToSearchDiscoP240Phase2Step7Informative(){
		
		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_IM) {
			Destroy(ToolsAndProductsPhase2Step7_int_im_instance);
		} else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP7_IM)
			Destroy (AR_Mode_Search_int_im_instance);
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE2_STEP7_IM;
		
		//indica que entramos en la fase 2 del tutorial:
		AppManager.manager.inTutorialPhase2 = false;
		Debug.Log ("--> Iniciando modo RA en GoToSearchDiscoP240Phase2Step7");
		//indica que se ingresa al modo RA fuera de los tutoriales:
		AppManager.manager.in_RA_mode = true;
		
		//ahora aqui debo instanciar la interfaz del tutorial 2 y asignarle todas las propiedades:
		AR_Mode_Search_int_im_instance = Instantiate (AppManager.manager.AR_Mode_interface);
		ControllerBlinkingARGeneric blinking_search_phase2step7 = AR_Mode_Search_int_im_instance.GetComponent<ControllerBlinkingARGeneric> ();
		
		Debug.Log ("Nueva Interfaz Instanciada en GoToSearchDiscoP240Phase2Step7!!");
		//hay que asignar la interfaz activa tambien:
		AppManager.manager.interfaceInstanceActive = AR_Mode_Search_int_im_instance;
		
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
		order_in_process [0] = 22;
		blinking_search_phase2step7.ordenes = order_in_process;
		
		//Asignando el metodo que se debe ejecutar cuando se hace click sobre el btn Select de la interfaz
		blinking_search_phase2step7.interface_going_from = "Phase2Step7";  //la variable Phase1Step3 permite obtener la interfaz para obtenerla en OnBackButtonTapped
		blinking_search_phase2step7.onClickSelectBtn += OnClickSelectDiscoP240Phase2Step7Infom;
		
		//iniciando el proceso blinking:
		blinking_search_phase2step7.should_be_blinking = true;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "II52", "13", "-1","consulta");
	} // cierra GoToSearchDiscoP240Phase2Step7
	
	public void GoToSearchObjetoLimpiezaPhase2Step7Informative(){
		
		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_IM) {
			Destroy(ToolsAndProductsPhase2Step7_int_im_instance);
		} else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP7_IM)
			Destroy (AR_Mode_Search_int_im_instance);
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE2_STEP7_IM;
		
		//indica que entramos en la fase 2 del tutorial:
		AppManager.manager.inTutorialPhase2 = false;
		Debug.Log ("--> Iniciando modo RA en GoToSearchObjetoLimpiezaPhase2Step7");
		//indica que se ingresa al modo RA fuera de los tutoriales:
		AppManager.manager.in_RA_mode = true;
		
		//ahora aqui debo instanciar la interfaz del tutorial 2 y asignarle todas las propiedades:
		AR_Mode_Search_int_im_instance = Instantiate (AppManager.manager.AR_Mode_interface);
		ControllerBlinkingARGeneric blinking_search_phase2step7_second = AR_Mode_Search_int_im_instance.GetComponent<ControllerBlinkingARGeneric> ();
		
		Debug.Log ("Nueva Interfaz Instanciada en GoToSearchObjetoLimpiezaPhase2Step7!!");
		//hay que asignar la interfaz activa tambien:
		AppManager.manager.interfaceInstanceActive = AR_Mode_Search_int_im_instance;
		
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
		order_in_process [0] = 23;
		blinking_search_phase2step7_second.ordenes = order_in_process;
		
		//Asignando el metodo que se debe ejecutar cuando se hace click sobre el btn Select de la interfaz
		blinking_search_phase2step7_second.interface_going_from = "Phase2Step7";  //la variable Phase1Step3 permite obtener la interfaz para obtenerla en OnBackButtonTapped
		blinking_search_phase2step7_second.onClickSelectBtn += OnClickSelectObjLimpiezaPhase2Step7Infom;
		
		//iniciando el proceso blinking:
		blinking_search_phase2step7_second.should_be_blinking = true;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "II53", "13", "-1","consulta");
	} //cierra GoToSearchObjetoLimpiezaPhase2Step7
	
	
	public void GoToSearchDiscoP320Phase2Step8Informative(){
		
		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_IM) {
			Destroy(ToolsAndProductsPhase2Step8_int_im_instance);
		} else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP8_IM)
			Destroy (AR_Mode_Search_int_im_instance);
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE2_STEP8_IM;
		
		//indica que entramos en la fase 2 del tutorial:
		AppManager.manager.inTutorialPhase2 = false;
		Debug.Log ("--> Iniciando modo RA en GoToSearchObjetoLimpiezaPhase2Step7");
		//indica que se ingresa al modo RA fuera de los tutoriales:
		AppManager.manager.in_RA_mode = true;
		
		//ahora aqui debo instanciar la interfaz del tutorial 2 y asignarle todas las propiedades:
		AR_Mode_Search_int_im_instance = Instantiate (AppManager.manager.AR_Mode_interface);
		ControllerBlinkingARGeneric blinking_search_phase2step8 = AR_Mode_Search_int_im_instance.GetComponent<ControllerBlinkingARGeneric> ();
		
		Debug.Log ("Nueva Interfaz Instanciada en GoToSearchObjetoLimpiezaPhase2Step7!!");
		//hay que asignar la interfaz activa tambien:
		AppManager.manager.interfaceInstanceActive = AR_Mode_Search_int_im_instance;
		
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
		order_in_process [0] = 24;
		blinking_search_phase2step8.ordenes = order_in_process;
		
		//Asignando el metodo que se debe ejecutar cuando se hace click sobre el btn Select de la interfaz
		blinking_search_phase2step8.interface_going_from = "Phase2Step8";  //la variable Phase1Step3 permite obtener la interfaz para obtenerla en OnBackButtonTapped
		blinking_search_phase2step8.onClickSelectBtn += OnClickSelectDiscoP320Phase2Step8Infom;
		
		//iniciando el proceso blinking:
		blinking_search_phase2step8.should_be_blinking = true;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "II54", "14", "-1","consulta");
	}//cierra GoToSearchDiscoP320Phase2Step8
	
	
	public void GoToSearchObjetoLimpiezaPhase2Step8Informative(){
		
		if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_IM) {
			Destroy(ToolsAndProductsPhase2Step8_int_im_instance);
		} else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP8_IM)
			Destroy (AR_Mode_Search_int_im_instance);
		
		current_interface = CurrentInterface.AR_SEARCH_PHASE2_STEP8_IM;
		
		//indica que entramos en la fase 2 del tutorial:
		AppManager.manager.inTutorialPhase2 = false;
		Debug.Log ("--> Iniciando modo RA en GoToSearchObjetoLimpiezaPhase2Step7");
		//indica que se ingresa al modo RA fuera de los tutoriales:
		AppManager.manager.in_RA_mode = true;
		
		//ahora aqui debo instanciar la interfaz del tutorial 2 y asignarle todas las propiedades:
		AR_Mode_Search_int_im_instance = Instantiate (AppManager.manager.AR_Mode_interface);
		ControllerBlinkingARGeneric blinking_search_phase2step8_second = AR_Mode_Search_int_im_instance.GetComponent<ControllerBlinkingARGeneric> ();
		
		
		Debug.Log ("Nueva Interfaz Instanciada en GoToSearchObjetoLimpiezaPhase2Step7!!");
		//hay que asignar la interfaz activa tambien:
		AppManager.manager.interfaceInstanceActive = AR_Mode_Search_int_im_instance;
		
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
		order_in_process [0] = 25;
		blinking_search_phase2step8_second.ordenes = order_in_process;
		
		//Asignando el metodo que se debe ejecutar cuando se hace click sobre el btn Select de la interfaz
		blinking_search_phase2step8_second.interface_going_from = "Phase2Step8";  //la variable Phase1Step3 permite obtener la interfaz para obtenerla en OnBackButtonTapped
		blinking_search_phase2step8_second.onClickSelectBtn += OnClickSelectObjLimpiezaPhase2Step8Infom;
		
		//iniciando el proceso blinking:
		blinking_search_phase2step8_second.should_be_blinking = true;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "II55", "14", "-1","consulta");
	} //cierra GoToSearchObjetoLimpiezaPhase2Step8


	/// <summary>
	/// Metodo que se llama cuando se presiona el boton select de la info addicional del capo del carro (phase 1-step1)
	/// en el modo RA.
	/// </summary>
	public void onClickSelectCapoCarroSearchInformative(){
		Debug.LogError ("AppManager: Lamado al metodo onClickSelectCapoCarroSearchInformative - Click en boton Select INFORMATIVE");
		
		//La informacion adicional ahora no se despliega:
		AppManager.manager.info_additional_displayed = false;
		
		//colocando la variable de tutorial en false porque ya ha finalizado esa parte:
		AppManager.manager.inTutorialPhase1 = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "SI1", "0", "-1","consulta");		
		//se llama al metodo que carga el menu de pasos de la fase 1 porque ya se ha completado el paso:
		GoToMenuStepsPhase1InformativeMode ();
	}

	public void OnClickSelectAguaPressioSearchInfoMode(string interface_from){
		Debug.LogError ("InformativeMode: Lamado al metodo OnClickSelectAguaPressioSearchInfoMode - Click en boton Select");
		
		//La informacion adicional ahora no se esta desplegando
		AppManager.manager.info_additional_displayed = false;
		
		//Todavia estamos en la fase del tutorial 2:
		AppManager.manager.inTutorialPhase2 = true;
		
		//Llamando al destroy de las posibles instancias existentes de la interfaz MenuOfStepsPhase1:
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhase1");
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "SI2", "1", "-1","consulta");
		
		//Hasta este punto se ha completado la primera parte del tutorial que se refiere a buscar el agua a presion
		//ahora se debe seguir con la segunda parte que es buscar el agua y el jabon:
		//se va a iniciar la segunda parte de la busqueda con RA:
		GoToSearchAguaJabonTutPhase2InfoMode ();
	}



	/// <summary>
	///Metodo que se llama cuando se presiona el boton select de la info addicional del agua y el jabon (phase1-step2)
	/// en el modo RA. Este metodo lanza la segunda parte de la busqueda de la bayeta
	/// </summary>
	/// <param name="interface_from">Interface_from.</param>
	public void OnClickSelectAguaJabonSearchInfoMode(string interface_from){
		Debug.LogError ("InformativeMode: Lamado al metodo OnClickSelectAguaJabonSearchInfoMode - Click en boton Select");
		
		//La informacion adicional ahora no se esta desplegando
		AppManager.manager.info_additional_displayed = false;
		
		//Todavia estamos en la fase del tutorial 2:
		AppManager.manager.inTutorialPhase2 = true;
		
		//Llamando al destroy de las posibles instancias existentes de la interfaz MenuOfStepsPhase1:
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhase1");
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "SI3", "1", "-1","consulta");
		
		//Hasta este punto se ha completado la primera parte del tutorial que se refiere a buscar el agua y el jabon
		//ahora se debe seguir con la segunda parte que es buscar la bayeta:
		//se va a iniciar la segunda parte de la busqueda con RA:
		GoToSearchBayetaTutorialPhase2InfoMode ();
	} //cierra metodo: OnClickSelectAguaJabonSearchInfoMode


	/// <summary>
	/// Metodo que se llama cuando se presiona el boton select de la info adicional de la bayeta (phase1-step2)
	/// Esto finaliza el tutorial 2 y vuelve a cargar el menu de pasos
	/// </summary>
	/// <param name="interface_from">Interface_from.</param>
	public void OnClickSelectBayetaSearchInfoMode(string interface_from){
		Debug.LogError ("InformativeMode: Lamado al metodo OnClickSelectBayetaSearchInfoMode - Click en boton Select!!");
		
		//La informacion adicional ahora no se esta desplegando
		AppManager.manager.info_additional_displayed = false;
		
		//Aqui termina el tutorial fase 2:
		AppManager.manager.inTutorialPhase2 = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "SI4", "1", "-1","consulta");
		//Llamando al destroy de las posibles instancias existentes de la interfaz MenuOfStepsPhase1:
		DestroyInstancesWithTag ("InterfaceMenuOfStepsPhase1");
		
		//se llama al metodo que carga el menu de pasos de la fase 1 porque ya se ha completado el paso:
		GoToMenuStepsPhase1InformativeMode ();
		
	}

	/// <summary>
	/// Metodo que se llama cuando se presiona el boton select de la info adicional de agua a presion (phase1-step3)
	/// </summary>
	/// <param name="interface_from">Interface_from.</param>
	public void OnClickSelectAguaPresionInfoMode(string interface_from){
		Debug.LogError ("InformativeMode: Lamado al metodo OnClickSelectAguaPresionInfoMode - Click en boton Select!!");
		
		//Llamando al destroy de las posibles instancias existentes de la interfaz MenuOfStepsPhase1:
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "SI5", "1", "-1","consulta");
		//Lamado al metodo para buscar el segundo objeto en el modo RA
		GoToSearchPapelAbsorbentePhase1Step3InfoMode ();
	}

	/// <summary>
	/// Metodo que se llama cuando se presiona el boton select de la info adicional del papel absorbente (phase1-step3)
	/// </summary>
	/// <param name="interface_from">Interface_from.</param>
	public void OnClickSelectPapelAbsorbenteInfoMode(string interface_from){
		Debug.LogError ("InformativeMode: Lamado al metodo OnClickSelectPapelAbsorbenteInfoMode - Click en boton Select!!");
		
		//Llamando al destroy de las posibles instancias existentes de la interfaz MenuOfStepsPhase1:
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");
		
		//Setting RA_mode en false porque ya se termina el modo RA aqui:
		Debug.Log ("--> Finalizando modo RA en OnClickSelectPapelAbsorbenteInfoMode");
		AppManager.manager.in_RA_mode = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "SI6", "2", "-1","consulta");
		//Lamado al metodo para buscar el segundo objeto en el modo RA
		GoToMenuStepsPhase1InformativeMode ();
	} //cierra OnClickSelectPapelAbsorbenteInfoMode


	/// <summary>
	/// Metodo que se llama cuando se presiona el boton select de la info adicional de la lija fina (phase1-step4)
	/// </summary>
	/// <param name="interface_from">Interface_from.</param>
	public void OnClickSelectLijaFinaInfoMode(string interface_from){
		Debug.LogError ("InformativeMode: Lamado al metodo OnClickSelectLijaFina - Click en boton Select!!");
		
		//Llamando al destroy de las posibles instancias existentes de la interfaz MenuOfStepsPhase1:
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");
		
		//Setting RA_mode en false porque ya se termina el modo RA aqui:
		Debug.Log ("--> InformativeMode: Finalizando modo RA en OnClickSelectLijaFinaInfoMode");
		AppManager.manager.in_RA_mode = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "SI7", "3", "-1","consulta");
		//Lamado al metodo para buscar el segundo objeto en el modo RA
		GoToMenuStepsPhase1InformativeMode ();
	} //cierra select lika fina


	/// <summary>
	/// Metodo que se llama cuando se presiona el boton select de la info adicional del martillo (phase1-step5)
	/// </summary>
	/// <param name="interface_from">Interface_from.</param>
	public void OnClickSelectMartilloInfoMode(string interface_from){
		Debug.LogError ("InformativeMode: Lamado al metodo OnClickSelectMartilloInfoMode - Click en boton Select!!");
		
		//Llamando al destroy de las posibles instancias existentes de la interfaz MenuOfStepsPhase1:
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");
		
		//Setting RA_mode en false porque ya se termina el modo RA aqui:
		Debug.Log ("--> InformativeMode: Finalizando modo RA en OnClickSelectMartillo");
		AppManager.manager.in_RA_mode = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "SI8", "4", "-1","consulta");
		//Lamado al metodo para buscar el segundo objeto en el modo RA
		GoToMenuStepsPhase1InformativeMode ();
	} //cierra OnClickselectMartilloInfoMode


	/// <summary>
	/// Metodo que se llama cuando se presiona el boton select de la info adicional del desengrasante (phase1-step6)
	/// </summary>
	/// <param name="interface_from">Interface_from.</param>
	public void OnClickSelectDesengrasanteInfoMode(string interface_from){
		Debug.LogError ("InformativeMode: Lamado al metodo OnClickSelectDesengrasante - Click en boton Select!!");
		
		//Llamando al destroy de las posibles instancias existentes de la interfaz MenuOfStepsPhase1:
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");
		
		//Setting RA_mode en false porque ya se termina el modo RA aqui:
		Debug.Log ("--> InformativeMode: Finalizando modo RA en OnClickSelectDesengrasante");
		AppManager.manager.in_RA_mode = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "SI9", "5", "-1","consulta");
		//Lamado al metodo para buscar el segundo objeto en el modo RA
		GoToSearchBayetaPhase1Step6InfoMode ();
	}

	/// <summary>
	/// Metodo que se llama cuando se presiona el boton select de la info adicional de la bayeta (phase1-step6)
	/// </summary>
	/// <param name="interface_from">Interface_from.</param>
	public void OnClickSelectBayetaStep6InfoMode(string interface_from){
		Debug.LogError ("InformativeMode: Lamado al metodo OnClickSelectBayetaStep6InfoMode - Click en boton Select!!");
		
		//Llamando al destroy de las posibles instancias existentes de la interfaz MenuOfStepsPhase1:
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");
		
		//Setting RA_mode en false porque ya se termina el modo RA aqui:
		Debug.Log ("--> InformativeMode: Finalizando modo RA en OnClickSelectBayetaStep6InfoMode");
		AppManager.manager.in_RA_mode = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "SI10", "5", "-1","consulta");
		//Lamado al metodo para buscar el segundo objeto en el modo RA
		GoToMenuStepsPhase1InformativeMode ();
	} // cierra OnClickSelectBayetaStep6InfoMode


	/// <summary>
	/// Metodo que se llama cuando se presiona el boton select de la info adicional de la cinta de enmascarar (phase2-step2)
	/// </summary>
	/// <param name="interface_from">Interface_from.</param>
	public void OnClickSelectCintaEmascararInfom(string interface_from){
		Debug.LogError ("AppManager: Lamado al metodo OnClickSelectCintaEmascarar - Click en boton Select!!");
		
		//Llamando al destroy de las posibles instancias existentes de la interfaz MenuOfStepsPhase1:
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");
		
		//Setting RA_mode en false porque ya se termina el modo RA aqui:
		Debug.Log ("--> AppManager: Finalizando modo RA en OnClickSelectCintaEmascarar");
		AppManager.manager.in_RA_mode = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "SI11", "7", "-1","consulta");
		//Lamado al metodo para buscar el segundo objeto en el modo RA
		GoToSearchPapelEnmascPhase2Step2InfoMode ();
	} //cierra metodo OnClickSelectCintaEmascarar
	
	
	public void OnClickSelectPapelEnmascPhase2Step2Infom(string interface_from){
		Debug.LogError ("AppManager: Lamado al metodo OnClickSelectPapelEnmascPhase2Step2 - Click en boton Select!!");
		
		//Llamando al destroy de las posibles instancias existentes de la interfaz MenuOfStepsPhase1:
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");
		
		//Setting RA_mode en false porque ya se termina el modo RA aqui:
		Debug.Log ("--> AppManager: Finalizando modo RA en OnClickSelectPapelEnmascPhase2Step2");
		AppManager.manager.in_RA_mode = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "SI12", "7", "-1","consulta");
		GoToActivitiesPhase2Step2Informative (); //en este caso no se han completado las actividades y por lo tanto se regresa al menu de actividades
	} //cierra metodo OnClickSelectPapelEnmascPhase2Step2
	
	
	public void OnClickSelectEsponjaPhase2Step3Infom(string interface_from){
		Debug.LogError ("AppManager: Lamado al metodo OnClickSelectEsponjaPhase2Step3 - Click en boton Select!!");
		
		//Llamando al destroy de las posibles instancias existentes de la interfaz MenuOfStepsPhase1:
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");
		
		//Setting RA_mode en false porque ya se termina el modo RA aqui:
		Debug.Log ("--> AppManager: Finalizando modo RA en OnClickSelectEsponjaPhase2Step3");
		AppManager.manager.in_RA_mode = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "SI13", "9", "-1","consulta");
		//Lamado al metodo para buscar el segundo objeto en el modo RA
		GoToSearchObjetoLimpiezaPhase2Step3Informative ();
	} //cierra OnClickSelectEsponjaPhase2Step3
	
	
	public void OnClickSelectObjetoLimpiezaPhase2Step3inform(string interface_from){
		Debug.LogError ("AppManager: Lamado al metodo OnClickSelectObjetoLimpiezaPhase2Step3 - Click en boton Select!!");
		
		//Llamando al destroy de las posibles instancias existentes de la interfaz MenuOfStepsPhase1:
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");
		
		//Setting RA_mode en false porque ya se termina el modo RA aqui:
		Debug.Log ("--> AppManager: Finalizando modo RA en OnClickSelectObjetoLimpiezaPhase2Step3");
		AppManager.manager.in_RA_mode = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "SI14", "9", "-1","consulta");
				
		GoToActivitiesPhase2Step3Informative ();
	} //cierra metodo OnClickSelectObjetoLimpiezaPhase2Step3
	
	
	public void OnClickSelectEsponjaP400Phase2Step4Infom(string interface_from){
		
		Debug.LogError ("AppManager: Lamado al metodo OnClickSelectEsponjaP400Phase2Step4 - Click en boton Select!!");
		
		//Llamando al destroy de las posibles instancias existentes de la interfaz MenuOfStepsPhase1:
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");
		
		//Setting RA_mode en false porque ya se termina el modo RA aqui:
		Debug.Log ("--> AppManager: Finalizando modo RA en OnClickSelectEsponjaP400Phase2Step4");
		AppManager.manager.in_RA_mode = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "SI15", "10", "-1","consulta");
		//Lamado al metodo para buscar el segundo objeto en el modo RA
		GoToSearchObjetoLimpiezaPhase2Step4Informative ();
		
	} //cierra OnClickSelectEsponjaP400Phase2Step4
	
	public void OnClickSelectObjetoLimpiezaPhase2Step4Infom (string interface_from){
		Debug.LogError ("AppManager: Lamado al metodo OnClickSelectObjetoLimpiezaPhase2Step4 - Click en boton Select!!");
		
		//Llamando al destroy de las posibles instancias existentes de la interfaz MenuOfStepsPhase1:
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");
		
		//Setting RA_mode en false porque ya se termina el modo RA aqui:
		Debug.Log ("--> AppManager: Finalizando modo RA en OnClickSelectObjetoLimpiezaPhase2Step4");
		AppManager.manager.in_RA_mode = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "SI16", "10", "-1","consulta");
		
		GoToActivitiesPhase2Step4Informative ();
	} //cierra OnClickSelectObjetoLimpiezaPhase2Step4
	
	
	public void OnClickSelectRotoOrbitalPhase2Step5Infom(string interface_from){
		Debug.LogError ("AppManager: Lamado al metodo OnClickSelectRotoOrbitalPhase2Step5 - Click en boton Select!!");
		//Llamando al destroy de las posibles instancias existentes de la interfaz MenuOfStepsPhase1:
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");
		//Setting RA_mode en false porque ya se termina el modo RA aqui:
		Debug.Log ("--> AppManager: Finalizando modo RA en OnClickSelectEsponjaP400Phase2Step4");
		AppManager.manager.in_RA_mode = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "SI17", "11", "-1","consulta");
		//Lamado al metodo para buscar el segundo objeto en el modo RA
		GoToSearchDiscoP80Phase2Step5Informative ();
	} //cierraOnClickSelectRotoOrbitalPhase2Step5
	
	public void OnClickSelectDiscoP80Phase2Step5Infom(string interface_from){
		Debug.LogError ("AppManager: Lamado al metodo OnClickSelectDiscoP80Phase2Step5 - Click en boton Select!!");
		//Llamando al destroy de las posibles instancias existentes de la interfaz MenuOfStepsPhase1:
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");
		//Setting RA_mode en false porque ya se termina el modo RA aqui:
		Debug.Log ("--> AppManager: Finalizando modo RA en OnClickSelectDiscoP80Phase2Step5");
		AppManager.manager.in_RA_mode = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "SI18", "11", "-1","consulta");
		//Lamado al metodo para buscar el segundo objeto en el modo RA
		GoToSearchObjetoLimpiezaPhase2Step5Informative ();
	}
	
	public void OnClickSelectObjLimpiezaPhase2Step5Infom(string interface_from){
		Debug.LogError ("AppManager: Lamado al metodo OnClickSelectObjLimpiezaPhase2Step5 - Click en boton Select!!");
		
		//Llamando al destroy de las posibles instancias existentes de la interfaz MenuOfStepsPhase1:
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");
		
		//Setting RA_mode en false porque ya se termina el modo RA aqui:
		Debug.Log ("--> AppManager: Finalizando modo RA en OnClickSelectObjLimpiezaPhase2Step5");
		AppManager.manager.in_RA_mode = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "SI19", "11", "-1","consulta");
		GoToActivitiesPhase2Step5Informative();
	} //cierra OnClickSelectObjLimpiezaPhase2Step5
	
	public void OnClickSelectDiscoP150Phase2Step6Infom(string interface_from){
		Debug.LogError ("AppManager: Lamado al metodo OnClickSelectDiscoP150Phase2Step6 - Click en boton Select!!");
		//Llamando al destroy de las posibles instancias existentes de la interfaz MenuOfStepsPhase1:
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");
		//Setting RA_mode en false porque ya se termina el modo RA aqui:
		Debug.Log ("--> AppManager: Finalizando modo RA en OnClickSelectDiscoP150Phase2Step6");
		AppManager.manager.in_RA_mode = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "SI20", "12", "-1","consulta");
		//Lamado al metodo para buscar el segundo objeto en el modo RA
		GoToSearchObjetoLimpiezaPhase2Step6Informative ();
	}
	
	public void OnClickSelectObjLimpiezaPhase2Step6Infom(string interface_from){
		Debug.LogError ("AppManager: Lamado al metodo OnClickSelectObjLimpiezaPhase2Step6 - Click en boton Select!!");
		
		//Llamando al destroy de las posibles instancias existentes de la interfaz MenuOfStepsPhase1:
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");
		
		//Setting RA_mode en false porque ya se termina el modo RA aqui:
		Debug.Log ("--> AppManager: Finalizando modo RA en OnClickSelectObjLimpiezaPhase2Step6");
		AppManager.manager.in_RA_mode = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "SI21", "12", "-1","consulta");
		GoToActivitiesPhase2Step6Informative (); 
	}//cierra OnClickSelectObjLimpiezaPhase2Step6
	
	
	public void OnClickSelectDiscoP240Phase2Step7Infom(string interface_from){
		Debug.LogError ("AppManager: Lamado al metodo OnClickSelectDiscoP240Phase2Step7 - Click en boton Select!!");
		//Llamando al destroy de las posibles instancias existentes de la interfaz MenuOfStepsPhase1:
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");
		//Setting RA_mode en false porque ya se termina el modo RA aqui:
		Debug.Log ("--> AppManager: Finalizando modo RA en OnClickSelectDiscoP240Phase2Step7");
		AppManager.manager.in_RA_mode = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "SI22", "13", "-1","consulta");
		//Lamado al metodo para buscar el segundo objeto en el modo RA
		GoToSearchObjetoLimpiezaPhase2Step7Informative ();
	}//cierra OnClickSelectDiscoP240Phase2Step7
	
	public void OnClickSelectObjLimpiezaPhase2Step7Infom(string interface_from){
		Debug.LogError ("AppManager: Lamado al metodo OnClickSelectObjLimpiezaPhase2Step7 - Click en boton Select!!");
		
		//Llamando al destroy de las posibles instancias existentes de la interfaz MenuOfStepsPhase1:
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");
		
		//Setting RA_mode en false porque ya se termina el modo RA aqui:
		Debug.Log ("--> AppManager: Finalizando modo RA en OnClickSelectObjLimpiezaPhase2Step7");
		AppManager.manager.in_RA_mode = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "SI23", "13", "-1","consulta");
		GoToActivitiesPhase2Step7Informative ();
	} //cierra OnClickSelectObjLimpiezaPhase2Step7
	
	public void OnClickSelectDiscoP320Phase2Step8Infom(string interface_from){
		Debug.LogError ("AppManager: Lamado al metodo OnClickSelectDiscoP320Phase2Step8 - Click en boton Select!!");
		//Llamando al destroy de las posibles instancias existentes de la interfaz MenuOfStepsPhase1:
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");
		//Setting RA_mode en false porque ya se termina el modo RA aqui:
		Debug.Log ("--> AppManager: Finalizando modo RA en OnClickSelectDiscoP320Phase2Step8");
		AppManager.manager.in_RA_mode = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "SI24", "14", "-1","consulta");
		//Lamado al metodo para buscar el segundo objeto en el modo RA
		GoToSearchObjetoLimpiezaPhase2Step8Informative ();
	}
	
	public void OnClickSelectObjLimpiezaPhase2Step8Infom(string interface_from){
		Debug.LogError ("AppManager: Lamado al metodo OnClickSelectObjLimpiezaPhase2Step8 - Click en boton Select!!");
		
		//Llamando al destroy de las posibles instancias existentes de la interfaz MenuOfStepsPhase1:
		DestroyInstancesWithTag ("AR_Mode_Generic_interface");
		
		//Setting RA_mode en false porque ya se termina el modo RA aqui:
		Debug.Log ("--> AppManager: Finalizando modo RA en OnClickSelectObjLimpiezaPhase2Step8");
		AppManager.manager.in_RA_mode = false;
		string fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Debug.Log ("Se va a registrar la interfaz con el controlador de navegacion:");
		NavigationControllerObject.navigation.RegistrarInterfazModoEvalConsult (AppManager.manager.codigo_estudiante, fecha, "SI25", "14", "-1","consulta");
		GoToActivitiesPhase2Step8Informative (); //en este caso no se han completado las actividades y por lo tanto se regresa al menu de actividades
	} // cierra OnClickSelectObjLimpiezaPhase2Step8
	

	/// <summary>
	/// This method destroies all the objects in memory identified with a specific tag
	/// The string containing the tag of the object that should be deleted. Bear in mind that the object should be tagged in the unity editor using the tag attribute.
	/// </summary>
	/// <param name="object_tag">Object_tag: </param>
	public void DestroyInstancesWithTag(string object_tag){
		Debug.Log ("Ingresa a la funcion para DESTROY interfaces EN INFORMATIVE!!!");
		
		GameObject[] arreglo = GameObject.FindGameObjectsWithTag (object_tag);
		
		foreach(GameObject obj_app in arreglo){
			Debug.Log("--> INSTANCIA A DESTRUIR: " + obj_app.name);
			DestroyImmediate(obj_app);
			
		}
	}


	public void OnBackButtonTapped(){
		Debug.Log ("InformativeMode: Llamado al metodo OnBackButtonTapped en la clase");

		if (current_interface == CurrentInterface.MENU_PHASES_IM) {
			Destroy (menuProcessPhases_int_im_instance);
			AppManager.manager.in_informative_mode = false;
			AppManager.manager.GoToSelectionOfMode ();
		} else if (current_interface == CurrentInterface.MENU_STEPS_PHASE1_IM || current_interface == CurrentInterface.MENU_STEPS_PHASE2_IM)
			GoToMenuPhasesInformativeMode ();
		else if (current_interface == CurrentInterface.MENU_SUB_STEPS_PHASE2_IM || current_interface == CurrentInterface.MENU_SUB_STEPS_INTERIORES_PHASE2_IM)
			GoToMenuStepsPhase2InformativeMode ();
		else if (current_interface == CurrentInterface.ACTIVITIES_PHASE1_STEP2_IM)
			GoToMenuStepsPhase1InformativeMode ();
		else if (current_interface == CurrentInterface.ACTIVITIES_PHASE1_STEP2_IM)
			GoToMenuStepsPhase1InformativeMode ();
		else if (current_interface == CurrentInterface.ACTIVITIES_PHASE1_STEP3_IM)
			GoToMenuStepsPhase1InformativeMode ();
		else if (current_interface == CurrentInterface.ACTIVITIES_PHASE1_STEP4_IM)
			GoToMenuStepsPhase1InformativeMode ();
		else if (current_interface == CurrentInterface.ACTIVITIES_PHASE1_STEP5_IM)
			GoToMenuStepsPhase1InformativeMode ();
		else if (current_interface == CurrentInterface.ACTIVITIES_PHASE1_STEP6_IM)
			GoToMenuStepsPhase1InformativeMode ();
		else if (current_interface == CurrentInterface.ACTIVITIES_PHASE2_STEP1_IM || current_interface == CurrentInterface.ACTIVITIES_PHASE2_STEP2_IM)
			GoToMenuStepsPhase2InformativeMode ();
		else if (current_interface == CurrentInterface.ACTIVITIES_PHASE2_STEP3_IM || current_interface == CurrentInterface.ACTIVITIES_PHASE2_STEP4_IM)
			GoToSubMenuStepsLijadoCantosInfoMode ();
		else if (current_interface == CurrentInterface.ACTIVITIES_PHASE2_STEP5_IM || current_interface == CurrentInterface.ACTIVITIES_PHASE2_STEP6_IM || current_interface == CurrentInterface.ACTIVITIES_PHASE2_STEP7_IM || current_interface == CurrentInterface.ACTIVITIES_PHASE2_STEP8_IM)
			GoToSubMenuStepsLijadoInterioresInfoMode ();
		else if (current_interface == CurrentInterface.AR_SEARCH_CAR_HOOD_IM)
			GoToActivitiesPhase1Step1Informative ();
		else if (current_interface == CurrentInterface.TOOLS_AND_PRODUCTS_IM) {
			CanvasToolsAndProductsManager canvas_mng = AppManager.manager.interfaceInstanceActive.GetComponent<CanvasToolsAndProductsManager> ();
			if (canvas_mng != null) {
				GoBackToMenuActivitiesFromToolsProductsInfoMode (canvas_mng.interfaceGoingBackFrom);
			} else {
				Debug.Log ("--> ERROR: No se pudo obtener el CanvasToolsAndProductsManager en OnBackButtonTapped");
			}
		} else if (current_interface == CurrentInterface.AR_SEARCH_PRODUCTS_TUTORIAL_IM || current_interface == CurrentInterface.AR_SEARCH_BAYETA_PRODUCT_TUTORIAL_IM) {
			ControllerBlinkingAddInfo controller_script = AppManager.manager.interfaceInstanceActive.GetComponent<ControllerBlinkingAddInfo> ();
			if (controller_script != null) {
				string interface_coming_from = controller_script.interface_going_from;
				AppManager.manager.inTutorialPhase2 = false;
				GoToToolsAndProductsInfoMode (interface_coming_from);
			} else {
				Debug.Log ("ERROR: EL SCRIPT QUE SE OBTIENE EN ONBACKBUTTONTAPPED ES NULL");
				GoToMenuPhasesInformativeMode ();
			}
		} //cierra else if de AR_SEARCH_PRODUCTS_TUTORIAL_IM
		else if (current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP3_IM || current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP4_IM || current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP5_IM || current_interface == CurrentInterface.AR_SEARCH_PHASE1_STEP6_IM){
			ControllerBlinkingARGeneric controller_generic = AppManager.manager.interfaceInstanceActive.GetComponent<ControllerBlinkingARGeneric>();
			AppManager.manager.in_RA_mode = false;
			if(controller_generic != null){
				string interface_coming_fr = controller_generic.interface_going_from;
				GoToToolsAndProductsInfoMode (interface_coming_fr);
			}else {
				Debug.Log("ERROR: EL SCRIPT QUE SE OBTIENE EN ONBACKBUTTONTAPPED ES NULL");
				GoToMenuPhasesInformativeMode ();
			}
		} else if (current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP2_IM || current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP3_IM || current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP4_IM || current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP5_IM || current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP6_IM || current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP7_IM || current_interface == CurrentInterface.AR_SEARCH_PHASE2_STEP8_IM){
			ControllerBlinkingARGeneric controller_generic = AppManager.manager.interfaceInstanceActive.GetComponent<ControllerBlinkingARGeneric>();
			AppManager.manager.in_RA_mode = false;
			if(controller_generic != null){
				string interface_coming_fr = controller_generic.interface_going_from;
				GoToToolsAndProductsInfoMode (interface_coming_fr);
			}else {
				Debug.Log("ERROR: EL SCRIPT QUE SE OBTIENE EN ONBACKBUTTONTAPPED ES NULL");
				GoToMenuPhasesInformativeMode ();
			}
		}
	} //cierra OnBackButtonTapped

	public void OnSingleTapped(){
		Debug.Log ("Llamado al metodo OnSingleTapped en la clase InformativeMode");

		in_tutorial_phase1 = AppManager.manager.inTutorialPhase1;
		in_tutorial_phase2 = AppManager.manager.inTutorialPhase2;
		information_loaded_from_marker = AppManager.manager.informationLoadedFromMarker;
		interface_active = AppManager.manager.interfaceInstanceActive;
		inRAmode = AppManager.manager.in_RA_mode;
		inEvaluationMode = AppManager.manager.in_Evaluation_mode;

		if (AppManager.manager.info_additional_displayed) {
			
			Debug.Log("Ingresa al TAP con info_add_displayed= " + AppManager.manager.info_additional_displayed);
			
		} else {

			if (in_tutorial_phase1 && interface_active != null && information_loaded_from_marker){
				Debug.LogError("Ingresa a OnSingleTapped para tutorial phase 1!!!");
				interface_active.GetComponent<ControllerBlinkingMarker>().PrepareAdditionalIcons();
				interface_active.GetComponent<ControllerBlinkingMarker>().ShowAdditionalIncons(true,false);
				if(interface_active.GetComponent<ControllerBlinkingMarker>().is_add_info_displayed)
					AppManager.manager.info_additional_displayed = true;
				else AppManager.manager.info_additional_displayed = false;
			}else if (in_tutorial_phase2 && interface_active != null && information_loaded_from_marker){
				Debug.LogError("Ingresa a OnSingleTapped para tutorial phase 2!!!");
				interface_active.GetComponent<ControllerBlinkingAddInfo>().PrepareAdditionalIcons();
				interface_active.GetComponent<ControllerBlinkingAddInfo>().ShowAdditionalIncons(false,true);
				if(interface_active.GetComponent<ControllerBlinkingAddInfo>().is_add_info_displayed)
					AppManager.manager.info_additional_displayed = true;
				else AppManager.manager.info_additional_displayed = false;
			} else if (inRAmode && interface_active != null && information_loaded_from_marker){
				Debug.LogError("Ingresa a OnSingleTapped para RA Mode!!!");
				interface_active.GetComponent<ControllerBlinkingARGeneric>().PrepareAdditionalIcons();
				interface_active.GetComponent<ControllerBlinkingARGeneric>().ShowAdditionalIncons();
				if(interface_active.GetComponent<ControllerBlinkingARGeneric>().is_add_info_displayed)
					AppManager.manager.info_additional_displayed = true;
				else AppManager.manager.info_additional_displayed = false;
			} else if(inEvaluationMode && interface_active != null && information_loaded_from_marker){
				Debug.LogError("Ingresa a OnSingleTapped para EvaluationMode!!!");
				interface_active.GetComponent<ControllerBlinkARModeEvaluation>().PrepareAdditionalIcons();
				interface_active.GetComponent<ControllerBlinkARModeEvaluation>().ShowAdditionalIncons();
				if(interface_active.GetComponent<ControllerBlinkARModeEvaluation>().is_add_info_displayed)
					AppManager.manager.info_additional_displayed = true;
				else AppManager.manager.info_additional_displayed = false;
			}
		}

	}



}
