using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Ingredient : MonoBehaviour
{
    protected SpriteRenderer spriteRenderer;

    public int ingredientIdx = 0;
    private readonly int negative = (int)Ingredients.Type.Kimchi;
    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    protected virtual void OnEnable()
    {
        ingredientIdx = Random.Range(0, GameManager.Instance.Inside.Stats.Count);
        gameObject.layer = IsPositive() ? LayerMask.NameToLayer("Getable") : LayerMask.NameToLayer("Default");
        spriteRenderer.sprite = GameManager.Instance.Inside.Stats[ingredientIdx].OutlineSprite;
    }
    public virtual void OnGet()
    {
        if (IsPositive())
        {
            QusetManager.Instance.QusetUpdate(QuestType.Day, 4, 1);
            QusetManager.Instance.QusetUpdate(QuestType.Aweek, 4, 1);
            QusetManager.Instance.QusetUpdate(QuestType.Main, 5, 1);
        }
        else
        {
            QusetManager.Instance.QusetUpdate(QuestType.Aweek, 5, 1);
            QusetManager.Instance.QusetUpdate(QuestType.Main, 6, 1);
        }
    }

    public bool IsPositive()
    {
        return ingredientIdx < negative;
    }
}
