using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OverlayGuiHandler : MonoBehaviour {

    public UnityEngine.UI.Text scoreText;
    public UnityEngine.UI.Text levelText;
    public UnityEngine.UI.Text timeText;

    public GameObject escapePanel;
    public GameObject deathPanel;
    public GameObject winPanel;

    private GameObject speechPanel;
    private Text speechText;
    private Image speechPanelImage;
    private Image canvasBackground;
    private bool isSpeechPanelActive;
    private float displaySpeechDuration = 2.0f;

    public bool isShowingEscapePane = false;
    public bool isShowingDeathPanel = false;
    public bool isShowingWinPanel = false;


    private static OverlayGuiHandler instance;
    public static OverlayGuiHandler Instance
    {
        get
        {
            return instance;
        }
    }

    // Use this for initialization
    void Start () {
        instance = this;
        levelText.text = "Level  " + (SceneManager.GetActiveScene().buildIndex - 1);

        foreach (var obj in transform.GetComponentsInChildren<Transform>(true))
        {
            if (string.Equals(obj.gameObject.name, "SpeachPanel", System.StringComparison.InvariantCultureIgnoreCase))
                speechPanel = obj.gameObject;
        }

        if (speechPanel != null)
        {
            foreach (var obj in speechPanel.GetComponentsInChildren<Text>(true))
            {
                if (string.Equals(obj.gameObject.name, "SpeechText", System.StringComparison.InvariantCultureIgnoreCase))
                    speechText = obj as Text;
            }

            foreach (var obj in speechPanel.GetComponentsInChildren<Image>(true))
            {
                if (string.Equals(obj.gameObject.name, "Image", System.StringComparison.InvariantCultureIgnoreCase))
                    speechPanelImage = obj as Image;
            }

            canvasBackground = speechPanel.GetComponent<Image>();

            speechPanel.SetActive(true);
        }

        if (speechPanel != null && speechText != null && speechPanelImage != null && canvasBackground != null)
        {
            speechText.CrossFadeAlpha(0, 0, true);
            speechPanelImage.CrossFadeAlpha(0, 0, true);
            canvasBackground.CrossFadeAlpha(0, 0, true);
        }
	}
	
	// Update is called once per frame
	void Update () {
        scoreText.text = string.Format("Boogers  {0} of {1}", LevelManager.Instance.Score, LevelManager.Instance.ScoreNeededToWin);

        if(timeText != null)
        {
            var time = 0f;
            if(GameHandler.Instance != null && LevelManager.Instance != null)
            {
                var tt = GameHandler.Instance.CurrentGameData.GetTotalTime();
                var cpt = 0f;
                var timeElapsedForPlay = 0f;
                if(LevelManager.Instance.isGameRunning)
                {
                    cpt = LevelManager.Instance.CurrentLevelData.CurrentPlayTime;
                    timeElapsedForPlay = (Time.time - LevelManager.Instance.startTime);
                }

                time = tt + cpt + timeElapsedForPlay;
            }
            
            timeText.text = string.Format("Time: {0:N}", time);
        }
            

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(isShowingEscapePane)
            {
                HideEscapePanel();
            }
            else
            {
                ShowEscapePanel();
            }
        }
    }

    public void ShowEscapePanel()
    {
        escapePanel.gameObject.SetActive(true);
        isShowingEscapePane = true;
    }

    public void HideEscapePanel()
    {
        
        escapePanel.gameObject.SetActive(false);
        isShowingEscapePane = false;
    }

    public void ShowWinPanel()
    {
        if (isShowingEscapePane)
            HideEscapePanel();

        winPanel.gameObject.SetActive(true);
    }

    public void ShowDeathPanel()
    {
        if (isShowingEscapePane)
            HideEscapePanel();

        deathPanel.gameObject.SetActive(true);
    }

    public void ShowMessage(string message, float time = 12f)
    {
        speechText.text = message;

        if (speechPanel != null && speechText != null)
        {

            if (canvasBackground == null || speechPanelImage == null || speechText == null)
                return;

            //Toggle
            isSpeechPanelActive = true;
            ToggleSpeechPanel();
            Invoke("ToggleSpeechPanel", time);

        }


    }

    public void ToggleSpeechPanel()
    {
        if (isSpeechPanelActive == true)
        {
            //Fade
            canvasBackground.CrossFadeAlpha(1.0f, displaySpeechDuration, false);
            speechText.CrossFadeAlpha(1.0f, displaySpeechDuration, false);
            speechPanelImage.CrossFadeAlpha(1.0f, displaySpeechDuration, false);

            isSpeechPanelActive = false;
        }
        else
        {
            //Fade
            canvasBackground.CrossFadeAlpha(0.0f, displaySpeechDuration, false);
            speechText.CrossFadeAlpha(0.0f, displaySpeechDuration, false);
            speechPanelImage.CrossFadeAlpha(0.0f, displaySpeechDuration, false);

            isSpeechPanelActive = true;
        }
    }
}
