using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class GameDataService
{
    private static GameDataService instance;
    public static GameDataService Instance
    {
        get
        {
            if (instance == null)
                instance = new GameDataService();

            return instance;
        }
    }
    private GameDataList gameDataList { get; set; }

    private const string gameDataFilePath = "GameData.json";
    private GameDataService()
    {
        gameDataList = new GameDataList();

        //Load data
        try
        {
            var fileDataJsonText = File.ReadAllText(gameDataFilePath);
           
            var loadedGameDataFromFile = JsonUtility.FromJson<GameDataList>(fileDataJsonText);
            if (loadedGameDataFromFile != null)
                gameDataList = loadedGameDataFromFile;
        }
        catch (Exception ex)
        {
            Debug.Log("Could not load GameData from file ex " + ex.ToString());
        }
    }

    public void AddGameData(GameData gameData)
    {
        if (gameData == null)
            return;

        gameDataList.Add(gameData);

        try
        {
            var fileDataJson = JsonUtility.ToJson(gameDataList);
            File.WriteAllText(gameDataFilePath, fileDataJson);
        }
        catch (Exception ex)
        {
            Debug.Log("Could not save GameData to file ex " + ex.ToString());
        }
    }

    public IEnumerable<GameData> GetHighScore()
    {
        return gameDataList.GetHighScore();
    }
}

[Serializable]
public class GameDataList : System.Object
{
    public List<GameData> gameDatas;

    public void Add(GameData gameData)
    {
        if (gameDatas == null)
            gameDatas = new List<GameData>();
        gameDatas.Add(gameData);
    }

    public IEnumerable<GameData> GetHighScore()
    {
        if (gameDatas == null)
            return new List<GameData>();

        return gameDatas.OrderByDescending(gd => gd.GetNumberOfLevelsCompleted()).ThenBy(gd => gd.GetTotalTime()).ToList();
    }
}

[Serializable]
public class GameData : System.Object
{
    public string Id;

    public string PlayerName;

    public string GameStartedOn;

    public List<LevelData> LevelDatas;


    public GameData()
    {
        LevelDatas = new List<LevelData>();
    }

    public int GetTotalScore()
    {
        return LevelDatas.Sum(ld => ld.Score);
    }

    public int GetTotalDeaths()
    {
        return LevelDatas.Sum(ld => ld.DeathCount);
    }

    public int GetTotalJumps()
    {
        return LevelDatas.Sum(ld => ld.JumpCount);
    }

    public float GetTotalTime()
    {
        return LevelDatas.Sum(ld => ld.CompletedTime);
    }

    public int GetNumberOfLevelsCompleted()
    {
        return LevelDatas.Count(ld => ld.CompletedTime > 0);
    }

    public LevelData GetLevelDataForLevelNumber(int levelNumber)
    {
        LevelData foundLevelData = LevelDatas.FirstOrDefault(ld => ld.LevelNumber == levelNumber);
        if(foundLevelData == null)
        {
            Debug.Log("Created data for level " + levelNumber);
            foundLevelData = new LevelData()
            {
                LevelNumber = levelNumber,
            };
            LevelDatas.Add(foundLevelData);
        }
        return foundLevelData;
    }
}

[Serializable]
public class LevelData : System.Object
{
    public int LevelNumber;

    public string LevelName;

    public int Score;

    public float CurrentPlayTime;

    public float CompletedTime;

    public int DeathCount;

    public int JumpCount;
}

public class GameHandler : MonoBehaviour {

    private static GameHandler instance;
    public static GameHandler Instance
    {
        get
        {
            return instance;
        }
    }

    public GameData CurrentGameData { get; private set; }

	void Start ()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);
	}

    public void StartNewGame(string playerName)
    {
        CurrentGameData = new GameData();
        CurrentGameData.Id = Guid.NewGuid().ToString();
        CurrentGameData.GameStartedOn = DateTime.Now.ToString("dd-MM-yy HH:mm:ss");
        CurrentGameData.PlayerName = playerName;
    }

    public void SaveCurrentGameData()
    {
        if (CurrentGameData == null)
            return;

        GameDataService.Instance.AddGameData(CurrentGameData);
    }

    public IEnumerable<GameData> GetHighScore()
    {
        if (GameDataService.Instance == null)
            return new List<GameData>();

        return GameDataService.Instance.GetHighScore();
    }
}
