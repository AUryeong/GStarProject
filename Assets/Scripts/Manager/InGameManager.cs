using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : Singleton<InGameManager>
{
    private List<int> ingredients = new List<int>();
    private Vector3 cameraDistance = new Vector3(6, 2.5f, -10);
    public IngameUIManager uiManager;

    public int gold;

    private void Update()
    {
        if (GameManager.Instance.inGaming)
            CameraMove();
    }
    protected void CameraMove()
    {
        Camera.main.transform.position = new Vector3(Player.Instance.transform.position.x + cameraDistance.x, cameraDistance.y, cameraDistance.z);
    }
    public void GameOver()
    {
        GameManager.Instance.inGaming = false;
        Player.Instance.gameObject.layer = LayerMask.NameToLayer("PlayerInv");
        Camera.main.DOShakePosition(0.5f, 6);
        Player.Instance.MoveCenter();
    }

    public void AddIngredients(Ingredient ingredient)
    {
        ingredients.Add(ingredient.ingredientIdx);
        uiManager.UpdateIngredientsCount(ingredient.isNegative, ingredients.Count);
    }
}
