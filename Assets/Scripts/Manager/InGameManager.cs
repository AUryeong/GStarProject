using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : Singleton<InGameManager>
{
    private List<int> ingredients = new List<int>();
    public IngameUIManager uiManager;
    public void GameOver()
    {
        GameManager.Instance.GameOver(ingredients);
    }

    public void AddIngredients(int index)
    {
        ingredients.Add(index);
        uiManager.UpdateIngredientsCount(ingredients.Count);
    }
}
