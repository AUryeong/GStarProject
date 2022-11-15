using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Car : Player
{
    [SerializeField] SpriteRenderer uiGauge;
    protected int getIngredients = 0;
    protected int boostIngredients = 10;
    protected int abilityBoostDuration = 5;

    protected override void OnEnable()
    {
        base.OnEnable();
        uiGauge.size = new Vector2(0, 1);
    }

    protected override void AddIngredient(Ingredient ingredient)
    {
        base.AddIngredient(ingredient);
        getIngredients++;
        if (getIngredients == boostIngredients)
        {
            boostDuration += abilityBoostDuration;
            getIngredients = 0;
        }
        uiGauge.size = new Vector2((float)getIngredients / boostIngredients, 1);
    }
}
