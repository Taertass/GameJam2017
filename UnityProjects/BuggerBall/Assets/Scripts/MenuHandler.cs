using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour {

    public UnityEngine.UI.Text versionText;

    private void Start()
    {
        if(versionText != null)
            versionText.text = "v " + Application.version;

        SoundHandler.Instance.PlayMenuMusic();
    }

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
