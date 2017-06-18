using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
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

    public string levelName;

    public bool isGameRunning = true;

    public LevelData CurrentLevelData { get; private set; }

    public float startTime;
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

        Invoke("PrintStartOrders", 0.1f);
    }
	
	void Update () {
    }

    public void TryAndWinCheck()
    {
        if (!isGameRunning)
            return;

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
        else
        {
            PrintNotEnoughOrbsCollectedMessage();
        }
    }

    public void PrintStartOrders()
    {
        //ONLY FIRST TIME

        if(CurrentLevelData.CurrentPlayTime == 0)
        {
            if(levelNumber == 1)
            {
                var sb = new StringBuilder();
                sb.AppendLine("Now my un-willing minion! You must scale a series of obstacles, in the search of more snot-globules. Don’t worry about how they got there, they are definitely not the sad remains of previous test-subjects!");
                sb.AppendLine("  * Collect all the green globs in each level, before continuing to the exit");
                if(OverlayGuiHandler.Instance != null)
                    OverlayGuiHandler.Instance.ShowMessage(sb.ToString(), 10f);
            }
        }
    }

    private bool isPrintCollectedLasterOrbMessage = false;
    public void PrintCollectedLasterOrbMessage()
    {
        if (isPrintCollectedLasterOrbMessage)
            return;

        isPrintCollectedLasterOrbMessage = true;
        var sb = new StringBuilder();
        sb.AppendLine("Ah, well done! That was the last of the globs on this stage.");
        sb.AppendLine("  * Proceed to the exit");

        OverlayGuiHandler.Instance.ShowMessage(sb.ToString());
    }

    private bool isPrintNotEnoughOrbsCollectedMessage = false;
    public void PrintNotEnoughOrbsCollectedMessage()
    {
        if (isPrintNotEnoughOrbsCollectedMessage)
            return;

        isPrintNotEnoughOrbsCollectedMessage = true;

        var sb = new StringBuilder();
        sb.AppendLine("You haven’t collected enough globules you fool!");
        sb.AppendLine("  * Come back when you have found and collected all the globs in this stage.");

        OverlayGuiHandler.Instance.ShowMessage(sb.ToString());
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
