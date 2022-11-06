using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Ingredient : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    public int ingredientIdx = 0;
    public static readonly int negative = (int)Ingredients.Type.Kimchi;
    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    protected virtual void OnEnable()
    {
        ingredientIdx = Random.Range(0, GameManager.Instance.Inside.Stats.Count);
        gameObject.layer = ingredientIdx >= negative ? LayerMask.NameToLayer("Getable") : LayerMask.NameToLayer("Default");
        spriteRenderer.sprite = GameManager.Instance.Inside.Stats[ingredientIdx].IconSprite;
    }
    public virtual void OnGet()
    {
    }
}
