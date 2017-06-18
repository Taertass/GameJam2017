using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverlayGuiHandler : MonoBehaviour {

    public UnityEngine.UI.Text scoreText;
    public UnityEngine.UI.Text levelText;

    public GameObject escapePanel;
    public GameObject deathPanel;
    public GameObject winPanel;

    private GameObject speechPanel;

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
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;
        levelText.text = "Level  " + (SceneManager.GetActiveScene().buildIndex - 1);
	}
	
	// Update is called once per frame
	void Update () {
        scoreText.text = string.Format("Score  {0} of {1}", LevelManager.Instance.Score, LevelManager.Instance.ScoreNeededToWin);

        if(Input.GetKeyDown(KeyCode.Escape))
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

    }
}
