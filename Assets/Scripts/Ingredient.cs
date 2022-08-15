using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Ingredient : MonoBehaviour
{
    SpriteRenderer spriteRenderer;


    public int ingredientIdx = 0;
    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    protected virtual void OnEnable()
    {
        ingredientIdx = Random.Range(0, GameManager.Instance.Inside.Stats.Count);
        spriteRenderer.sprite = GameManager.Instance.Inside.Stats[Random.Range(0, GameManager.Instance.Inside.Stats.Count)].ImageSprite;
    }
    public virtual void OnGet()
    {
    }
}
