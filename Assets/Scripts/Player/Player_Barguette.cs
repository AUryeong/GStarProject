using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Barguette : Player
{
    protected override void OnEnable()
    {
        base.OnEnable();
        hitFadeOutTime = 1.4f;
    }

    protected override void AddIngredient(Ingredient ingredient)
    {
        base.AddIngredient(ingredient);
        if (!hitable)
        {
            InGameManager.Instance.AddIngredients(ingredient);
            InGameManager.Instance.AddIngredients(ingredient);
        }
    }
}
