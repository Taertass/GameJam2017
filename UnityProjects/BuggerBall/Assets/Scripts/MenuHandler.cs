using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour {

    public UnityEngine.UI.Text versionText;

    public GameObject startGamePanel;
    public GameObject highScorePanel;
    public UnityEngine.UI.InputField playerNameInput;
    public UnityEngine.UI.Text highScoreText;

    private bool isStartGamePanel = false;
    private bool isHighScorePanelShown = false;

    private void Start()
    {
        if(versionText != null)
            versionText.text = "v " + Application.version;

        if (SoundHandler.Instance != null)
            SoundHandler.Instance.PlayMenuMusic();

        UpdateHighScoreText();
    }

    public void UpdateHighScoreText()
    {
        if (highScoreText == null)
            return;

        var sb = new StringBuilder();

        if (GameHandler.Instance == null)
            sb.Append("No high score");
        else
        {
            IEnumerable<GameData> highScores = GameHandler.Instance.GetHighScore();
            if(highScores.Count() == 0)
            {
                sb.Append("No high score");
            }
            else
            {
                int index = 1;
                foreach(var highScore in highScores)
                {
                    sb.AppendLine(string.Format("{0}.  {1} ({2})      {3} seconds", index, highScore.PlayerName, highScore.GameStartedOn, highScore.GetTotalTime()));
                    sb.AppendLine("");
                    sb.AppendLine(string.Format("      Deaths: {0}, Jumps:{1}", highScore.GetTotalDeaths(), highScore.GetTotalJumps()));
                    sb.AppendLine("");
                    sb.AppendLine("-o-");
                    sb.AppendLine("");
                }
            }
        }
        
        highScoreText.text = sb.ToString();
    }

    public void ShowStartGamePanelClicked()
    {
        if(startGamePanel != null)
        {
            isStartGamePanel = true;
            startGamePanel.gameObject.SetActive(true);
            playerNameInput.Select();
        }
    }

    private void Update()
    {
        if(isStartGamePanel && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
        {
            Debug.Log("ENTER");
            Debug.Log(playerNameInput.text);

            if(!string.IsNullOrEmpty(playerNameInput.text))
                StartGameButtonClicked();
        }
    }

    public void StartGameButtonClicked()
    {
        var playerName = playerNameInput.text;
        if (string.IsNullOrEmpty(playerName))
            playerName = "Unknown";

        if (GameHandler.Instance != null)
            GameHandler.Instance.StartNewGame(playerName);

        SceneManager.LoadScene(1);
    }

    public void SettingsButtonClicked()
    {
        
    }

    public void QuitButtonClicked()
    {
        Application.Quit();
    }

    public void BackButtonClicked()
    {
        isStartGamePanel = false;

        if (startGamePanel != null)
            startGamePanel.gameObject.SetActive(false);
    }

    public void ShowHighScoreButtonClicked()
    {
        isHighScorePanelShown = true;

        if (highScorePanel != null)
            highScorePanel.gameObject.SetActive(true);

        UpdateHighScoreText();
    }

    public void BackHighScoreButtonClicked()
    {
        isHighScorePanelShown = false;

        if (highScorePanel != null)
            highScorePanel.gameObject.SetActive(false);
    }
}
