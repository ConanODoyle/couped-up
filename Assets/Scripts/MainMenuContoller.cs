using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuContoller : MonoBehaviour
{
    #region Initialization 
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
    }
    #endregion

    #region Play Button Methods
    public void PlayGame()
    {
        SceneManager.LoadScene("Main");
    }
    #endregion

}
