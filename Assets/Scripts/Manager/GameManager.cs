using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public Ingredients Inside;
    public Breads Breads;
    public bool inGaming = true;
    [Header("Setting")]
    [SerializeField] GameObject settingObject;
    public bool buttonReversal;
    [Header("InGameStats")]
    public Breads.Type selectBread;
    public int selectMap;

    [Header("Assets")]
    public int Gold;
    public int Macaron;
    public int Heart;
    [Header("Ability")]
    public int MaxHpLv;
    public int DefenseLv;

    //씬 나누는게 삭제될 가능성이 높아서 코드 대충짬
    List<int> ingredientIdxList = new List<int>();

    protected override void Awake()
    {
        base.Awake();
        if (Instance != this)
            Destroy(gameObject);
        else
            DontDestroyOnLoad(gameObject);
    }


    public void GameOver(List<int> ingredients)
    {
        inGaming = false;
        ingredientIdxList = ingredients;

        //씬 넘어가는게 삭제될 가능성이 높기에 대충짜서 코드 더러움
        SceneManager.LoadScene("Ending");
        SceneManager.sceneLoaded += EndingSceneLoadComplete;
    }

    public void EndingSceneLoadComplete(Scene scene, LoadSceneMode loadSceneMode)
    {
        EndingSpawn.Instance.Spawn(0, ingredientIdxList);
        SceneManager.sceneLoaded -= EndingSceneLoadComplete;
    }

    //------------------Setting Button------------------
    public void OnOffSetting()
    {
        settingObject.SetActive(!settingObject.activeSelf);
    }
    public void ButtonReversal()
    {
        buttonReversal = !buttonReversal;
    }
}
