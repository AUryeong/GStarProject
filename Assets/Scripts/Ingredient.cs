using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Ingredient : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    public bool isNegative = false;
    public int ingredientIdx = 0;
    public static readonly int negative = (int)Ingredients.Type.Kimchi;
    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    protected virtual void OnEnable()
    {
        ingredientIdx = Random.Range(0, GameManager.Instance.Inside.Stats.Count);
        isNegative = ingredientIdx >= negative;
        spriteRenderer.sprite = GameManager.Instance.Inside.Stats[ingredientIdx].IconSprite;
    }
    public virtual void OnGet()
    {
    }
}
