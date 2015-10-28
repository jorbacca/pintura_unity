/*==============================================================================
Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Qualcomm Connected Experiences, Inc.
==============================================================================*/

using UnityEngine;

namespace Vuforia
{
    /// <summary>
    /// A custom handler that implements the ITrackableEventHandler interface.
    /// </summary>
    public class DefaultTrackableEventHandler : MonoBehaviour,
                                                ITrackableEventHandler
    {
        #region PRIVATE_MEMBER_VARIABLES
 
        private TrackableBehaviour mTrackableBehaviour;
		private MarkerBehaviour mMarkerBehav;
		private int contadorOrdenes;

        #endregion // PRIVATE_MEMBER_VARIABLES

		#region PUBLIC_MEMBER_VARIABLES
		public AppManager manager;
		public GameObject OptionsInterface;
		public int[] order;
		public bool tracked;
		#endregion PUBLIC_MEMBER_VARIABLES


        #region UNTIY_MONOBEHAVIOUR_METHODS
    
        void Start()
        {
            mTrackableBehaviour = GetComponent<TrackableBehaviour>();
            if (mTrackableBehaviour)
            {
                mTrackableBehaviour.RegisterTrackableEventHandler(this);
            }

			contadorOrdenes = 0;
			tracked = false;

			mMarkerBehav = GetComponent<MarkerBehaviour> ();

        }

        #endregion // UNTIY_MONOBEHAVIOUR_METHODS



        #region PUBLIC_METHODS

        /// <summary>
        /// Implementation of the ITrackableEventHandler function called when the
        /// tracking state changes.
        /// </summary>
        public void OnTrackableStateChanged(
                                        TrackableBehaviour.Status previousStatus,
                                        TrackableBehaviour.Status newStatus)
        {
            if (newStatus == TrackableBehaviour.Status.DETECTED ||
                newStatus == TrackableBehaviour.Status.TRACKED ||
                newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
            {
                OnTrackingFound();
            }
            else
            {
                OnTrackingLost();
            }
        }

        #endregion // PUBLIC_METHODS



        #region PRIVATE_METHODS


        private void OnTrackingFound()
        {
            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

            // Enable rendering:
            foreach (Renderer component in rendererComponents)
            {
                component.enabled = true;
            }

            // Enable colliders:
            foreach (Collider component in colliderComponents)
            {
                component.enabled = true;
            }
			contadorOrdenes=0;
			Debug.Log("Valor del managerProcesOrder = " + manager.getProcessOrder()+  " y orderActual= " + order [contadorOrdenes] + " orderLenght= " + order.Length);

			//se cambia el valor de la variable tracked para indicar que el marcador esta siendo detectado:
			tracked = true;

			//si se esta en fase de tutorial entones se notifica a la clase que se ha encontrado el marcador
			if (manager.inTutorialPhase1) {
				Debug.Log ("TUTORIAL 1: Notificando el marcador con ID: " + mMarkerBehav.Marker.MarkerID);
				//para la primera fase de tutorial el objeto debe continuar haciendo blinking
				if (mMarkerBehav.Marker.MarkerID == 1) //OJO: Este tiene que ser el marcador del primer tutorial
					manager.interfaceInstanceActive.GetComponent<ControllerBlinkingMarker> ().should_be_blinking = true;
				else
					manager.interfaceInstanceActive.GetComponent<ControllerBlinkingMarker> ().should_be_blinking = false;
				manager.interfaceInstanceActive.GetComponent<ControllerBlinkingMarker> ().markerFound = true;
				manager.interfaceInstanceActive.GetComponent<ControllerBlinkingMarker> ().marker_id_loading_from = mMarkerBehav.Marker.MarkerID;
				this.GetComponent<ControllerAddInfoInMarker> ().LoadInformationToInterface (manager.inTutorialPhase1, manager.inTutorialPhase2, false);
			} else if (manager.inTutorialPhase2) {
				Debug.Log ("TUTORIAL2: Notificando el marcador con ID: " + mMarkerBehav.Marker.MarkerID);
				manager.interfaceInstanceActive.GetComponent<ControllerBlinkingAddInfo> ().should_be_blinking = false;
				manager.interfaceInstanceActive.GetComponent<ControllerBlinkingAddInfo> ().markerFound = true;
				manager.interfaceInstanceActive.GetComponent<ControllerBlinkingAddInfo> ().marker_id_loading_from = mMarkerBehav.Marker.MarkerID;
				this.GetComponent<ControllerAddInfoInMarker> ().LoadInformationToInterface (manager.inTutorialPhase1, manager.inTutorialPhase2, false);
			} else if (manager.in_RA_mode) {
				Debug.Log ("MODO RA: Notificando el marcador con ID: " + mMarkerBehav.Marker.MarkerID);
				manager.interfaceInstanceActive.GetComponent<ControllerBlinkingARGeneric> ().should_be_blinking = false;
				manager.interfaceInstanceActive.GetComponent<ControllerBlinkingARGeneric> ().markerFound = true;
				manager.interfaceInstanceActive.GetComponent<ControllerBlinkingARGeneric> ().marker_id_loading_from = mMarkerBehav.Marker.MarkerID;
				this.GetComponent<ControllerAddInfoInMarker> ().LoadInformationToInterface (manager.inTutorialPhase1, manager.inTutorialPhase2, true);
			} else if (manager.in_Evaluation_mode) {
				Debug.Log ("MODO EVALUACION: Notificando el marcador con ID: " + mMarkerBehav.Marker.MarkerID);
				manager.interfaceInstanceActive.GetComponent<ControllerBlinkARModeEvaluation>().markerFound = true;
				manager.interfaceInstanceActive.GetComponent<ControllerBlinkARModeEvaluation> ().marker_id_loading_from = mMarkerBehav.Marker.MarkerID;
				this.GetComponent<ControllerAddInfoInMarker>().LoadInformationForEvaluationMode();
			}

			/*
			//lines for testing singleton:
			AppManager.manager.mensaje_single = "Hola desde el DefaultTrackable";
			AppManager.manager.TestingSingle ();
			*/
			/*
			for (contadorOrdenes=0; contadorOrdenes<order.Length; contadorOrdenes++) {
				if (order [contadorOrdenes] == manager.getProcessOrder ()) {
					Debug.LogError("FOUND --> Se va a invocar el should_be_blinking del order = " + order[contadorOrdenes]);
					manager.interfaceInstanceActive.GetComponent<ControllerBlinkingMarker>().should_be_blinking=false;
				}
			}*/

            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
        }


        private void OnTrackingLost()
        {
            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

            // Disable rendering:
            foreach (Renderer component in rendererComponents)
            {
                component.enabled = false;
            }

            // Disable colliders:
            foreach (Collider component in colliderComponents)
            {
                component.enabled = false;
            }

            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
			//habilitando el blinking del objeto de la interfaz actual porque se ha perdido el marcador

			//se cambia el valor de la variable tracked para indicar que el marcador ha sido perdido:
			tracked = false;

			Debug.LogError("Antes de comparar si estamos en tutorial fase 1 o 2...");
			//si se esta en fase de tutorial entones se notifica a la clase que se ha perdido el marcador
			if (manager.inTutorialPhase1) {
				//hide_Add_Info_To_ShowTick sera true cuando se despliega el tick y entonces se debe detener el blinking:
				if (!manager.interfaceInstanceActive.GetComponent<ControllerBlinkingMarker> ().hide_Add_Info_To_ShowTick) //verificando que no se haya mostrado el tick para evitar el blinking
					manager.interfaceInstanceActive.GetComponent<ControllerBlinkingMarker> ().should_be_blinking = true;
					manager.interfaceInstanceActive.GetComponent<ControllerBlinkingMarker> ().markerFound = false;
				//manager.interfaceInstanceActive.GetComponent<ControllerBlinkingMarker> ().HideAllAdditionalInformation(true,false);

				if (manager.info_additional_displayed) {
					manager.interfaceInstanceActive.GetComponent<ControllerBlinkingMarker> ().HideAllAdditionalInformation (true, false);
					manager.info_additional_displayed = false;
				}
				//OJO tener en cuenta que esta variable impide los clicks o touch sobre la interfaz a menos que la info
				//del marcador ya se haya cargado
				manager.informationLoadedFromMarker = false;

			} else if (manager.inTutorialPhase2) {
				Debug.LogError ("Ingreso al manager.TutorialPhaseTwo...");
				if (!manager.interfaceInstanceActive.GetComponent<ControllerBlinkingAddInfo> ().hide_Add_Info_To_ShowTick) //verificando que no se haya mostrado el tick para evitar el blinking
					manager.interfaceInstanceActive.GetComponent<ControllerBlinkingAddInfo> ().should_be_blinking = true;
				manager.interfaceInstanceActive.GetComponent<ControllerBlinkingAddInfo> ().markerFound = false;
				if (manager.info_additional_displayed) {
					Debug.Log ("INGRESANDO A INFORMACION DESPLEGADA");
					manager.interfaceInstanceActive.GetComponent<ControllerBlinkingAddInfo> ().HideAllAdditionalInformation (false, true);
					manager.info_additional_displayed = false;
				}

				manager.informationLoadedFromMarker = false;

			} else if (manager.in_RA_mode) { //verificando que estamos en el modo RA
				Debug.Log ("Valor de RA_MODE en DefaultTrackableEventHandler: " + manager.in_RA_mode);
				if (!manager.interfaceInstanceActive.GetComponent<ControllerBlinkingARGeneric> ().hide_Add_Info_To_ShowTick)
					manager.interfaceInstanceActive.GetComponent<ControllerBlinkingARGeneric> ().should_be_blinking = true;
					manager.interfaceInstanceActive.GetComponent<ControllerBlinkingARGeneric> ().markerFound = false;
				if (manager.info_additional_displayed) {
					Debug.Log ("SE VA A OCULTAR LA INFO DESPLEGADA EN MarkerLost del DefaultTrackableHandler AR Generic");
					manager.interfaceInstanceActive.GetComponent<ControllerBlinkingARGeneric> ().HideAllAdditionalInformation ();
					manager.info_additional_displayed = false;
				}
				manager.informationLoadedFromMarker = false;
			} else if (manager.in_Evaluation_mode) {
				Debug.Log ("Valor de Evaluation_mode en DefaultTrackableEventHandler: " + manager.in_Evaluation_mode);
				manager.interfaceInstanceActive.GetComponent<ControllerBlinkARModeEvaluation> ().markerFound = false;
				if (manager.info_additional_displayed) {
					Debug.Log ("SE VA A OCULTAR LA INFO DESPLEGADA EN MarkerLost del DefaultTrackableHandler AR Evaluation Mode");
					manager.interfaceInstanceActive.GetComponent<ControllerBlinkARModeEvaluation> ().HideAllAdditionalInformation ();
					manager.info_additional_displayed = false;
				}
				manager.informationLoadedFromMarker = false;
			}
			/*
			MenuOfStepsPhase1Manager[] arreglo = FindObjectsOfType(typeof(MenuOfStepsPhase1Manager)) as MenuOfStepsPhase1Manager[];
			foreach(MenuOfStepsPhase1Manager canv in arreglo){
				Debug.Log("--> INSTANCIA " + canv.name);
				//Destroy(canv);
			}*/

			/*
			GameObject[] arreglo = GameObject.FindGameObjectsWithTag ("InterfaceMenuOfStepsPhase1");
			foreach(GameObject obj_app in arreglo){
				Debug.Log("--> En MarkerLost las instancias: " + obj_app.name);
				//Destroy(obj_app);
			}*/

			ControllerBlinkingAddInfo[] arreglo = FindObjectsOfType (typeof(ControllerBlinkingAddInfo)) as ControllerBlinkingAddInfo[];
			foreach(ControllerBlinkingAddInfo obj_app in arreglo){
				Debug.Log("--> En MarkerLost las instancias: " + obj_app.name);
				//Destroy(obj_app);
			}

			/*
			contadorOrdenes=0;
			for (contadorOrdenes=0; contadorOrdenes<order.Length; contadorOrdenes++) {
				if (order [contadorOrdenes] == manager.getProcessOrder ()) {
					Debug.LogError("LOST --> Se va a inocar el should_be_blinking del order = " + order[contadorOrdenes]);
					manager.interfaceInstanceActive.GetComponent<ControllerBlinkingMarker>().should_be_blinking=true;
				}
			}*/
        }

        #endregion // PRIVATE_METHODS
    }
}
