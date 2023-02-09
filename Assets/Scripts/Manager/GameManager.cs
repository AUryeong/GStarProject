using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public Ingredients Inside;
    public Breads breads;
    public bool inGaming = true;
    [Header("InGameStats")]
    public Breads.Type selectBread = Breads.Type.Milk;
    public Map selectMap;

    [Header("Assets")]
    public int gold;
    public int macaron;
    public int stamina;
    public int heart
    {
        get { return stamina; }
        set
        {
            stamina = value;
            LobbyUIManager.Instance.StaminaUpdate();
        }
    }
    [Header("Ability")]
    public int maxHpLv;
    public int defenseLv;

    //�� �����°� ������ ���ɼ��� ���Ƽ� �ڵ� ����«
    List<int> ingredientIdxList = new List<int>();

    private List<int> nevIngredientIdxList = new List<int>();

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
        gold += InGameManager.Instance.gold;
        InGameManager.Instance.gold = 0;
        inGaming = false;
        ingredientIdxList = ingredients;

        nevIngredientIdxList = InGameManager.Instance.player is Player_Prison p ? p.ingredientIdxes.ToList() : null;


        //�� �Ѿ�°� ������ ���ɼ��� ���⿡ ����¥�� �ڵ� ������
        SceneManager.LoadScene("Ending");
        SceneManager.sceneLoaded += EndingSceneLoadComplete;
    }

    public void EndingSceneLoadComplete(Scene scene, LoadSceneMode loadSceneMode)
    {
        EndingSpawn.Instance.Spawn((int)selectBread, ingredientIdxList, nevIngredientIdxList);
        SceneManager.sceneLoaded -= EndingSceneLoadComplete;
    }
}
