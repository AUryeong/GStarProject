using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Tufu : Player
{
    protected override void HurtByBlock(Block block)
    {
        if (!hitable)
            return;

        hitable = false;
        hp = 0;
        GameOver();
    }

    protected override void AddIngredient(Ingredient ingredient)
    {
        base.AddIngredient(ingredient);
        InGameManager.Instance.AddIngredients(ingredient);
        hp += 3;
    }
}
