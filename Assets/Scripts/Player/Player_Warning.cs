using UnityEngine;

public class Player_Warning : Player
{
    protected override void HpRemove()
    {
        hpRemoveDuration += Time.deltaTime;
        if (hpRemoveDuration >= hpRemoveCool)
        {
            hpRemoveDuration -= hpRemoveCool;
            hp -= hpRemoveValue /2;
            if (hp <= 0)
            {
                GameOver();
                return;
            }
        }
    }
    
    protected override void AddIngredient(Ingredient ingredient)
    {
        if (!ingredient.IsPositive())
            hp -= 5f;
        base.AddIngredient(ingredient);
    }
}