using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinGameHandler : MonoBehaviour {

    private void Start()
    {
        if(GameHandler.Instance)
            GameHandler.Instance.SaveCurrentGameData();
    }

    public void BackToStartButtonClicked()
    {

        SceneManager.LoadScene(0);
    }
}
