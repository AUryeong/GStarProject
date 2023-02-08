using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameManager : Singleton<InGameManager>
{
    [SerializeField] private List<int> ingredients = new List<int>();
    private Vector3 cameraDistance = new Vector3(6, 2.5f, -10);
    public IngameUIManager uiManager;
    public MapManager mapManager;
    public Player player
    {
        get; private set;
    }

    [Header("이펙트")]
    public ParticleSystem boostEffect;
    public SpriteRenderer magnetEffect;
    public ParticleSystem toasterEffect;
    [SerializeField] ParticleSystem goldEffect;

    [Header("빵")]
    private Breads.Type breadType;
    [SerializeField] GameObject[] breadArray;
    [SerializeField] Vector3 startPos;

    [Header("참깨빵용")]
    public RectTransform sesameIngredientsParent;
    public Image[] sesameIngredients;

    [Header("이외")]
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
        player.transform.localScale = Vector3.one * 1.2f;

        boostEffect.transform.SetParent(player.transform);
        boostEffect.transform.localPosition = Vector3.zero;
        magnetEffect.transform.SetParent(player.transform);
        magnetEffect.transform.localPosition = Vector3.zero;
        toasterEffect.transform.SetParent(player.transform);
        toasterEffect.transform.localPosition = Vector3.zero;
        mapManager.Init();

        if (player is Player_Sesame)
            sesameIngredientsParent.gameObject.SetActive(true);

        SoundManager.Instance.PlaySoundClip("BGM_Cafe", ESoundType.BGM);
    }
   
    private void Update()
    {
        if (player.isControllable)
            CameraMove();
        mapManager.MapUpdate();
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

    public void AddIngredients(Ingredients.Type ingredientType)
    {
        ingredients.Add((int)ingredientType);
        uiManager.UpdateIngredientsCount((int)ingredientType, ingredients.Count);
    }
    public void AddIngredients(Ingredient ingredient)
    {
        ingredients.Add(ingredient.ingredientIdx);
        uiManager.UpdateIngredientsCount(ingredient.ingredientIdx, ingredients.Count);
    }
}
