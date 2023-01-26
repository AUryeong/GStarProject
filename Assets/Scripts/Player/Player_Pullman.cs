using UnityEngine;

public class Player_Pullman : Player
{
    protected override void AddIngredient(Ingredient ingredient)
    {
        base.AddIngredient(ingredient);
        if (jumpCount > 0)
            jumpCount--;
    }
}