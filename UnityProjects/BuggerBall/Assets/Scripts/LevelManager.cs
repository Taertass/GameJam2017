using System;
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
    public int ScoreNeededToWin;

    public bool isGameRunning = true;

    void Start () {
        instance = this;
        isGameRunning = true;
    }
	
	void Update () {
		
	}

    public void TryAndWinCheck()
    {
        if(Score >= ScoreNeededToWin)
        {
            OverlayGuiHandler.Instance.ShowWinPanel();
            isGameRunning = false;
        }
    }

    public void LoseLevel()
    {
        isGameRunning = false;
        OverlayGuiHandler.Instance.ShowDeathPanel();
    }

    public void WinGameButtonClicked()
    {
        isGameRunning = false;
        var sceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneIndex + 1);
    }

    public void LoseGameButtonClicked()
    {
        int loseSceenIndex = SceneManager.sceneCountInBuildSettings - 1;
        SceneManager.LoadScene(loseSceenIndex);
    }

    public void RetryLevelButtonClicked()
    {
        var sceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneIndex);
    }

    public void BackToStartButtonClicked()
    {
        SceneManager.LoadScene(0);
    }
}
