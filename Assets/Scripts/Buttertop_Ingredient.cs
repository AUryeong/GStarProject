using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttertop_Ingredient : Ingredient
{
    protected override void OnEnable()
    {
        ingredientIdx = (int)Ingredients.Type.Butter;
        gameObject.layer = LayerMask.NameToLayer("Getable");
        spriteRenderer.sprite = GameManager.Instance.Inside.Stats[ingredientIdx].OutlineSprite;
    }
}
