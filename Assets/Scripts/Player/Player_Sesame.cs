using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Sesame : Player
{
    protected int[] ingredientIdxes = new int[5];
    public int idx = 0;
    protected float hpHealValue = 50;

    protected override void OnEnable()
    {
        base.OnEnable();
        InGameManager.Instance.sesameIngredientsParent.gameObject.SetActive(true);
        ReRoll();
    }

    protected override void AddIngredient(Ingredient ingredient)
    {
        base.AddIngredient(ingredient);
        if (ingredientIdxes[idx] == ingredient.ingredientIdx)
        {
            InGameManager.Instance.sesameIngredients[idx].gameObject.SetActive(false);
            idx++;
            if (idx >= ingredientIdxes.Length)
            {
                hp += hpHealValue;
                ReRoll();
            }
        }
    }

    protected void ReRoll()
    {
        idx = 0;
        ingredientIdxes = new int[5];
        for (int i = 0; i < 5; i++)
        {
            int idx = Random.Range(0, GameManager.Instance.Inside.Stats.Count);
            ingredientIdxes[i] = idx;
            InGameManager.Instance.sesameIngredients[i].sprite = GameManager.Instance.Inside.Stats[idx].OutlineSprite;
            InGameManager.Instance.sesameIngredients[i].gameObject.SetActive(true);
        }
    }
}
