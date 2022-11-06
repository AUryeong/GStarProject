using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : Singleton<InGameManager>
{
    private List<int> ingredients = new List<int>();
    private Vector3 cameraDistance = new Vector3(6, 2.5f, -10);
    public IngameUIManager uiManager;
    public Player player
    {
        get; private set;
    }

    [Header("∏ ")]
    [SerializeField] protected List<GameObject> firstMapList;
    [SerializeField] protected GameObject ovenMap;
    protected int mapSize = 1;
    public readonly int ovenMapSize = 10;
    public float mapLength { get; private set; } = 20;
    public readonly float platformMapLength = 44;
    protected List<GameObject> mapList = new List<GameObject>();

    [Header("¿Ã∆Â∆Æ")]
    public ParticleSystem boostEffect;
    public SpriteRenderer magnetEffect;
    public ParticleSystem toasterEffect;
    [SerializeField] ParticleSystem goldEffect;

    [Header("ªß")]
    private Breads.Type breadType;
    [SerializeField] GameObject[] breadArray;
    [SerializeField] Vector3 startPos;

    [Header("¿Ãø‹")]
    public int gold;

    public void GoldEffect(Vector3 pos)
    {
        GameObject obj = PoolManager.Instance.Init(goldEffect.gameObject);
        AutoDestruct.Add(obj, 0.5f);
        obj.transform.position = pos;
    }

    protected override void Awake()
    {
        base.Awake();
        breadType = GameManager.Instance.selectBread;
        player = Instantiate(breadArray[(int)breadType].gameObject, startPos, transform.rotation).GetComponent<Player>();
        boostEffect.transform.SetParent(player.transform);
        boostEffect.transform.localPosition = Vector3.zero;
        magnetEffect.transform.SetParent(player.transform);
        magnetEffect.transform.localPosition = Vector3.zero;
        toasterEffect.transform.SetParent(player.transform);
        toasterEffect.transform.localPosition = Vector3.zero;
        AddNewPlatform();
    }
    protected void AddNewPlatform()
    {
        foreach (GameObject obj in mapList)
        {
            if (obj.gameObject.activeSelf)
                if (mapLength - obj.transform.position.x > 45)
                    obj.gameObject.SetActive(false);
        }
        GameObject platform;
        if (mapSize % ovenMapSize == 0)
            platform = PoolManager.Instance.Init(ovenMap);
        else
            platform = PoolManager.Instance.Init(firstMapList[Random.Range(0, firstMapList.Count)]);
        platform.transform.position = new Vector3(mapLength + platformMapLength / 2, 1.5f, 0);
        mapLength += platformMapLength;
        mapSize++;
        if (!mapList.Contains(platform))
            mapList.Add(platform);
    }
    private void Update()
    {
        if (player.isControllable)
            CameraMove();
        if (mapLength - player.transform.position.x < 15)
        {
            AddNewPlatform();
        }
    }
    protected void CameraMove()
    {
        Camera.main.transform.position = new Vector3(player.transform.position.x + cameraDistance.x, cameraDistance.y, cameraDistance.z);
    }
    public void GameOver()
    {
        player.gameObject.layer = LayerMask.NameToLayer("PlayerInv");
        boostEffect.gameObject.SetActive(false);
        toasterEffect.gameObject.SetActive(false);
        magnetEffect.gameObject.SetActive(false);
        player.MoveCenter();
        Camera.main.DOShakePosition(0.5f, 6);
    }

    public void GameOverMoveCP()
    {
        GameManager.Instance.gold += gold;
        GameManager.Instance.GameOver(ingredients);
    }

    public void AddIngredients(Ingredient ingredient)
    {
        ingredients.Add(ingredient.ingredientIdx);
        uiManager.UpdateIngredientsCount(ingredient.ingredientIdx, ingredients.Count);
    }
}
