using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinGameHandler : MonoBehaviour {

	public void BackToStartButtonClicked()
    {
        SceneManager.LoadScene(0);
    }
}
