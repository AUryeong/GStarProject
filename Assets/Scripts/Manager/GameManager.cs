using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameManager: Singleton<GameManager>
{
    public Ingredients Inside;
    public Breads breads;
    public bool inGaming = true;
    [Header("Setting")]
    [SerializeField] GameObject settingObject;
    public bool buttonReversal;
    [Header("InGameStats")]
    public Breads.Type selectBread;
    public int selectMap;

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
        StartCoroutine(C_OnOffSetting());
    }
    private IEnumerator C_OnOffSetting()
    {
        if (settingObject.activeSelf)
        {
            settingObject.transform.DOScale(Vector3.zero, 0.5f);
            yield return new WaitForSeconds(0.5f);
            settingObject.SetActive(false);
        }
        else
        {
            settingObject.SetActive(true);
            settingObject.transform.DOScale(Vector3.one, 0.5f);
        }
    }
    public void ButtonReversal()
    {
        buttonReversal = !buttonReversal;
    }
}
