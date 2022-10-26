using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{
    public Dictionary<string, Sprite> ingSprites = new Dictionary<string, Sprite>();
    public Dictionary<string, Sprite> ingOutlineSprites = new Dictionary<string, Sprite>();
    public Dictionary<string, Sprite> ingSandwichSprites = new Dictionary<string, Sprite>();
    [SerializeField] Ingredients ingredients;
    protected override void Awake()
    {
        base.Awake();
        Sprite[] sprites = Resources.LoadAll<Sprite>("Ingredients");
        Sprite[] spritesIcon = Resources.LoadAll<Sprite>("Ingredients/Icon");
        for (int i = 0; i < sprites.Length; i++)
        {
            if (ingredients.Stats.Count < i)
            {
                ingredients.Stats[i].IconSprite = sprites[i];
                ingredients.Stats[i].IconSprite = sprites[i];
            }
        }
    }
}
