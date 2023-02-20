using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Ingredient : MonoBehaviour
{
    protected SpriteRenderer spriteRenderer;

    public int ingredientIdx { get; protected set; }

    [SerializeField] protected bool isNegative = true;
    private readonly int negative = (int)Ingredients.Type.Kimchi;

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void OnEnable()
    {
        ingredientIdx = isNegative ? Random.Range(negative, GameManager.Instance.Inside.Stats.Count) : Random.Range(0, negative);
        gameObject.layer = IsPositive() ? LayerMask.NameToLayer("Getable") : LayerMask.NameToLayer("Default");
        spriteRenderer.sprite = GameManager.Instance.Inside.Stats[ingredientIdx].OutlineSprite;
    }

    public virtual void OnGet()
    {
        if (isNegative)
        {
            QusetManager.Instance.QusetUpdate(QuestType.Aweek, 5, 1);
            QusetManager.Instance.QusetUpdate(QuestType.Main, 6, 1);
        }
        else
        {
            QusetManager.Instance.QusetUpdate(QuestType.Day, 4, 1);
            QusetManager.Instance.QusetUpdate(QuestType.Aweek, 4, 1);
            QusetManager.Instance.QusetUpdate(QuestType.Main, 5, 1);
        }
    }

    public bool IsPositive()
    {
        return !isNegative;
    }
}