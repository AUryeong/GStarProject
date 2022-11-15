using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Sesame : Player
{
    [SerializeField] SpriteRenderer[] ingredientsSprites;
    protected int[] ingredientIdxes = new int[5];
    public int idx = 0;
    protected float hpHealValue = 50;

    protected override void OnEnable()
    {
        base.OnEnable();
        ReRoll();
    }

    protected override void AddIngredient(Ingredient ingredient)
    {
        base.AddIngredient(ingredient);
        if (ingredientIdxes[idx] == ingredient.ingredientIdx)
        {
            ingredientsSprites[idx].gameObject.SetActive(false);
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
            ingredientsSprites[i].sprite = GameManager.Instance.Inside.Stats[idx].OutlineSprite;
            ingredientsSprites[i].gameObject.SetActive(true);
        }
    }
}
