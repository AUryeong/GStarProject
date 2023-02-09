using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Prison : Player
{
    public Queue<int> ingredientIdxes = new Queue<int>();

    protected override void OnEnable()
    {
        base.OnEnable();
        InGameManager.Instance.sesameIngredientsParent.gameObject.SetActive(true);
        IngredientUpdate();
    }

    protected override void AddIngredient(Ingredient ingredient)
    {
        if (ingredient.IsPositive())
        {
            base.AddIngredient(ingredient);
            return;
        }
        ingredient.OnGet();
        if (ingredientIdxes.Count >= 5)
        {
            int nigeruIngred = ingredientIdxes.Dequeue();
            InGameManager.Instance.AddIngredients(nigeruIngred);
        }
        ingredientIdxes.Enqueue(ingredient.ingredientIdx);
        ingredient.gameObject.SetActive(false);
        IngredientUpdate();

        SoundManager.Instance.PlaySoundClip("SFX_InGame_Get_Ingredient", ESoundType.SFX);
    }

    private void IngredientUpdate()
    {
        int[] ingredients = ingredientIdxes.ToArray();
        for (int i = 0; i < 5; i++)
        {
            var image = InGameManager.Instance.sesameIngredients[i];
            if (ingredientIdxes.Count > i)
            {
                int idx = ingredients[i];
                image.sprite = GameManager.Instance.Inside.Stats[idx].OutlineSprite;
                image.gameObject.SetActive(true);
            }
            else
            {
                image.gameObject.SetActive(false);
            }
        }
    }
}