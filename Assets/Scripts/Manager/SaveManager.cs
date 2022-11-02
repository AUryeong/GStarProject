using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable] // Á÷·ÄÈ­

public class GameData
{
    //QUEST QUEST,M_Cle,ISCLEAR
    //SCRIPTABLE BREAD LV,EXP,MAXEXP,ISBUY
    //GAMEMANAGER MONEY,MACARONG,STAMINA
    public int maxHpLv = 0;
    public int defenseLv = 0;
    public int gold = 0;
    public int macaron = 0;
    public int stamina = 7;
    public Breads.Type selectBread = Breads.Type.Milk;
    public List<Quests> questData = new List<Quests>();
    public List<Bread> breadData = new List<Bread>();

    [System.Serializable]
    public class Quests
    {
        public QuestType qusetType;
        public List<Quest> questList = new List<Quest>();
    }
    [System.Serializable]
    public class Quest
    {
        public bool isClear = false;
        public float M_ClearCount;
        public float questSituation;
    }
    [System.Serializable]
    public class Bread
    {
        public int LV;
        public float EXP;
        public bool isBuy;
    }
}
public class SaveManager : Singleton<SaveManager>
{
    public string prefsName = "TOSSave";

    public GameData _gameData;
    public GameData gameData
    {
        get
        {
            if (_gameData == null)
            {
                LoadGameData();
            }
            return _gameData;
        }
    }

    protected bool isReset = false;
    protected override void Awake()
    {
        base.Awake();
        if (Instance == this)
        {
            DontDestroyOnLoad(gameObject);
            LoadGameData();
        }
    }

    protected void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            isReset = true;
        }
    }

    public void LoadGameData()
    {
        string s = PlayerPrefs.GetString(prefsName, "null");
        if (s != "null")
            _gameData = JsonUtility.FromJson<GameData>(s);
        else
            _gameData = new GameData();
        GameManager.Instance.gold = gameData.gold;
        GameManager.Instance.stamina = gameData.stamina;
        GameManager.Instance.macaron = gameData.macaron;
        GameManager.Instance.selectBread = gameData.selectBread;
        GameManager.Instance.defenseLv = gameData.defenseLv;
        GameManager.Instance.maxHpLv = gameData.maxHpLv;
        foreach (var quests in gameData.questData)
        {
            var questList = QusetManager.Instance.qusetScriptables[(int)quests.qusetType].QusetList;
            for (int i = 0; i < quests.questList.Count; i++)
            {
                Quest quest = questList[i];
                GameData.Quest questData = quests.questList[i];
                quest.isClear = questData.isClear;
                quest.questSituation = questData.questSituation;
                quest.M_ClearCount = questData.M_ClearCount;
            }
        }
        for (int i = 0; i < gameData.breadData.Count; i++)
        {
            BreadStats breadStat = GameManager.Instance.breads.Stats[i];
            GameData.Bread breadData = gameData.breadData[i];
            breadStat.isBuy = breadData.isBuy;
            breadStat.LV = breadData.LV;
        }
    }



    public void SaveGameData()
    {
        if (isReset)
        {
            _gameData = new GameData();
            PlayerPrefs.SetString(prefsName, JsonUtility.ToJson(gameData));
            return;
        }
        gameData.gold = GameManager.Instance.gold;
        gameData.stamina = GameManager.Instance.stamina;
        gameData.macaron = GameManager.Instance.macaron;
        gameData.selectBread = GameManager.Instance.selectBread;
        gameData.defenseLv = GameManager.Instance.defenseLv;
        gameData.maxHpLv = GameManager.Instance.maxHpLv;
        gameData.questData.Clear();
        for (int i = 0; i < QusetManager.Instance.qusetScriptables.Length; i++)
        {
            var scriptable = QusetManager.Instance.qusetScriptables[i];
            var questsData = new GameData.Quests();

            gameData.questData.Add(questsData);
            questsData.qusetType = (QuestType)i;
            foreach (Quest quest in scriptable.QusetList)
            {
                GameData.Quest questData = new GameData.Quest();

                questsData.questList.Add(questData);
                questData.isClear = quest.isClear;
                questData.questSituation = quest.questSituation;
                questData.M_ClearCount = quest.M_ClearCount;
            }
        }
        gameData.breadData.Clear();
        for (int i = 0; i < GameManager.Instance.breads.Stats.Count; i++)
        {
            BreadStats breadStat = GameManager.Instance.breads.Stats[i];
            GameData.Bread breadData = new GameData.Bread();
            gameData.breadData.Add(breadData);
            breadData.isBuy = breadStat.isBuy;
            breadData.LV = breadStat.LV;
        }
        PlayerPrefs.SetString(prefsName, JsonUtility.ToJson(gameData));
    }

    private void OnApplicationQuit()
    {
        SaveGameData();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
            SaveGameData();
    }
}