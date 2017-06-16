using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour {

    public void StartGameButtonClicked()
    {
        SceneManager.LoadScene(1);
    }

    public void SettingsButtonClicked()
    {

    }

    public void QuitButtonClicked()
    {
        Application.Quit();
    }
}
