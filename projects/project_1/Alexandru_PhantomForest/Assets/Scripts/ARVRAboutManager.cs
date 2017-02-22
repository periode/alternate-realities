/*============================================================================== 
Copyright (c) 2015-2016 PTC Inc. All Rights Reserved.
Vuforia is a trademark of PTC Inc., registered in the United States and other 
countries.   
==============================================================================*/
using UnityEngine;
using System.Collections;

public class ARVRAboutManager : MonoBehaviour
{
    #region PUBLIC_METHODS
    public void OnStartFullScreen()
    {
        ModeConfig.isFullScreenMode = true;
        LoadNextScene();
    }

    public void OnStartViewer()
    {
        ModeConfig.isFullScreenMode = false;
        LoadNextScene();
    }
    #endregion // PUBLIC_METHODS


    #region PRIVATE_METHODS
    private void LoadNextScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex+1);
    }
    #endregion //PRIVATE_METHODS


    #region MONOBEHAVIOUR_METHODS
    void Update()
    {
#if UNITY_ANDROID
        // On Android, the Back button is mapped to the Esc key
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            // Exit app
            Application.Quit();
        }
#endif
    }
    #endregion // MONOBEHAVIOUR_METHODS
}
