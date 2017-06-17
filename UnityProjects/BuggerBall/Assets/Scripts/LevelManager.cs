using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    private static LevelManager instance;
    public static LevelManager Instance
    {
        get
        {
            return instance;
        }
    }
    
    public int Score;

    void Start () {
        instance = this;

    }
	
	void Update () {
		
	}

    public void WinGameButtonClicked()
    {
        var sceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneIndex + 1);
        Debug.Log(sceneIndex);
    }

    public void LoseGameButtonClicked()
    {
        int loseSceenIndex = SceneManager.sceneCountInBuildSettings - 1;
        SceneManager.LoadScene(loseSceenIndex);
        Debug.Log(loseSceenIndex);
    }
}
