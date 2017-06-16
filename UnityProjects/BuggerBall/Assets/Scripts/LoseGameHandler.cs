using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseGameHandler : MonoBehaviour {

    public void BackToStartButtonClicked()
    {
        SceneManager.LoadScene(0);
    }

    public void RetryButtonClicked()
    {
        SceneManager.LoadScene(0);
    }
}
