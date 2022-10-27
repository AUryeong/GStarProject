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

    //�� �����°� ������ ���ɼ��� ���Ƽ� �ڵ� ����«
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

        //�� �Ѿ�°� ������ ���ɼ��� ���⿡ ����¥�� �ڵ� ������
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
