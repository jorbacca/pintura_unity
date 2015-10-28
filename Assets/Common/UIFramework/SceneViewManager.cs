/*============================================================================== 
 * Copyright (c) 2012-2014 Qualcomm Connected Experiences, Inc. All Rights Reserved. 
 * ==============================================================================*/

/// <summary>
/// All Initializations, Draw Calls and Update Calls go through here.
/// </summary>
using UnityEngine;
using System.Collections;
using Vuforia;

[RequireComponent(typeof( SampleInitErrorHandler ))]
public class SceneViewManager : MonoBehaviour {
    
    public AppManager mAppManager;
    private SampleInitErrorHandler mPopUpMsg;

    private bool mErrorOccurred;

    void Awake()
    {
        mPopUpMsg = GetComponent<SampleInitErrorHandler>();
        if(!mPopUpMsg)
        {
            mPopUpMsg = gameObject.AddComponent<SampleInitErrorHandler>();
        }

        // Check for an initialization error on start.
        QCARAbstractBehaviour qcarBehaviour = (QCARAbstractBehaviour)FindObjectOfType(typeof(QCARAbstractBehaviour));
        if (qcarBehaviour)
        {
            qcarBehaviour.RegisterQCARInitErrorCallback(OnQCARInitializationError);
        }
    }

    void Start () 
    {
        mPopUpMsg.InitPopUp();
        mAppManager.InitManager();
    }
    
    void Update()
    {	
		//14 de Julio: Estoy comentareando esto para evitar el cuadro de dialogo de que se tiene que estar conectado
		//a internet para ingresar a la aplicacion y utilizarla la primera vez que se instala:
		/*
        if (mErrorOccurred)
            return;
		*/
        InputController.UpdateInput();  
        mAppManager.UpdateManager();
    }

    void OnDestroy()
    {
        QCARAbstractBehaviour qcarBehaviour = (QCARAbstractBehaviour)FindObjectOfType(typeof(QCARAbstractBehaviour));
        if (qcarBehaviour)
        {
            qcarBehaviour.UnregisterQCARInitErrorCallback(OnQCARInitializationError);
        }

        mAppManager.DeInitManager();
    }
    
    void OnGUI () 
    {
		//14 de Julio: Estoy comentareando esto para evitar el cuadro de dialogo de que se tiene que estar conectado
		//a internet para ingresar a la aplicacion y utilizarla la primera vez que se instala:
        /*
		if (mErrorOccurred)
        {
            mPopUpMsg.Draw();
        }
        else
        {
            mAppManager.Draw();
        }*/
		mAppManager.Draw ();
    }

    public void OnQCARInitializationError(QCARUnity.InitError initError)
    {
        if (initError != QCARUnity.InitError.INIT_SUCCESS)
        {
            mErrorOccurred = true;
            mPopUpMsg.SetErrorCode(initError);
        }
    }
}
