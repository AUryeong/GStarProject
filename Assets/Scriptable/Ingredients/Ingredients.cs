using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ingredients Data", menuName = "ScriptableObject/Ingredients", order = int.MaxValue)]
public class Ingredients : ScriptableObject, ISerializationCallbackReceiver
{
    public int i;
    public List<stats> Stats = new List<stats>();

    public void OnAfterDeserialize()
    {
        for (int i = 0; i < Stats.Count; i++)
            Stats[i].name = ((Type)i);
    }

    public void OnBeforeSerialize()
    {
    }

    public enum Type
    {
        Ham,
        Chicken,
        Lettuce,
        Tomato,
        Tuna,
        Cheese,
        Butter,
        Salmon,
        Bacon,
        Egg,
        Meetball,
        BerryJam,
        Potato,
        Cream,
        PeanutButter,
        Kimchi,
        Durian,
        MintChoco,
        Oyster,
        PineApple,
        Cilantro,
        Cucumber,
        KoChuJang,
        Ice,
        PoplingCandy,
        LiveOctopus,
        NONE
    }
}

[System.Serializable]
public class stats
{
    [HideInInspector] public Sprite SandwichSprite;
    [HideInInspector] public Sprite IconSprite;
    [HideInInspector] public Sprite OutlineSprite;
    public Ingredients.Type name;
    public int Size;
}