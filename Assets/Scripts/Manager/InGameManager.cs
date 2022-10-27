using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : Singleton<InGameManager>
{
    private List<int> ingredients = new List<int>();
    private Vector3 cameraDistance = new Vector3(6, 2.5f, -10);
    public IngameUIManager uiManager;
    public Player player {
        get; private set;
    }

    public int gold;

    private Breads.Type breadType;
    [SerializeField] GameObject[] breadArray;
    [SerializeField] Vector3 startPos;
    protected override void Awake()
    {
        base.Awake();
        breadType = GameManager.Instance.selectBread;
        player = Instantiate(breadArray[(int)breadType].gameObject, startPos, transform.rotation).GetComponent<Player>();
    }
    private void Update()
    {
        if (player.isControllable)
            CameraMove();
    }
    protected void CameraMove()
    {
        Camera.main.transform.position = new Vector3(player.transform.position.x + cameraDistance.x, cameraDistance.y, cameraDistance.z);
    }
    public void GameOver()
    {
        player.gameObject.layer = LayerMask.NameToLayer("PlayerInv");
        player.MoveCenter();
        Camera.main.DOShakePosition(0.5f, 6);
    }

    public void GameOverMoveCP()
    {
        GameManager.Instance.GameOver(ingredients);
    }

    public void AddIngredients(Ingredient ingredient)
    {
        ingredients.Add(ingredient.ingredientIdx);
        uiManager.UpdateIngredientsCount(ingredient.ingredientIdx, ingredients.Count);
    }
}
