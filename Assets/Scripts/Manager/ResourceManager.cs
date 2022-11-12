using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ResourceManager : Singleton<ResourceManager>
{
    public Dictionary<string, Sprite> ingSprites = new Dictionary<string, Sprite>();
    public Dictionary<string, Sprite> ingOutlineSprites = new Dictionary<string, Sprite>();
    public Dictionary<string, Sprite> ingSandwichSprites = new Dictionary<string, Sprite>();
    [SerializeField] Ingredients ingredients;
    protected override void Awake()
    {
        base.Awake();
        Sprite empty = Resources.Load<Sprite>("Ingredients/SandWich/Square");
        for (Ingredients.Type i = 0; i < Ingredients.Type.NONE; i++)
        {
            ingredients.Stats[(int)i].IconSprite = Resources.Load<Sprite>("Ingredients/" + i.ToString());
            ingredients.Stats[(int)i].OutlineSprite = Resources.Load<Sprite>("Ingredients/Icon/Icon_" + i.ToString()); 
            ingredients.Stats[(int)i].SandwichSprite = Resources.Load<Sprite>("Ingredients/Stack/Build_" + i.ToString());
        }
        if (Instance != this)
            Destroy(gameObject);
        else
            DontDestroyOnLoad(gameObject);
    }
}
