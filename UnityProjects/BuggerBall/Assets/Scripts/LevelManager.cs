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

    public LevelData CurrentLevelData { get; private set; }

    private float startTime;
    private int levelNumber;

    void Start ()
    {
        startTime = Time.time;
        instance = this;
        isGameRunning = true;
        levelNumber = SceneManager.GetActiveScene().buildIndex - 1;

        if (SoundHandler.Instance != null)
            SoundHandler.Instance.PlayMusicForLevel(levelNumber);

        if (GameHandler.Instance != null && GameHandler.Instance.CurrentGameData != null)
            CurrentLevelData = GameHandler.Instance.CurrentGameData.GetLevelDataForLevelNumber(levelNumber);
        else
            CurrentLevelData = new LevelData();
    }
	
	void Update () {
		
	}

    public void TryAndWinCheck()
    {
        if(Score >= ScoreNeededToWin)
        {
            if (CurrentLevelData != null)
            {
                CurrentLevelData.CurrentPlayTime += Time.time - startTime;
                CurrentLevelData.CompletedTime = CurrentLevelData.CurrentPlayTime;
            }

            OverlayGuiHandler.Instance.ShowWinPanel();
            isGameRunning = false;
        }
    }

    public void LoseLevel()
    {
        if (CurrentLevelData != null)
        {
            CurrentLevelData.DeathCount++;
            CurrentLevelData.CurrentPlayTime += Time.time - startTime;
        }

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
