using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ingredients Data", menuName = "ScriptableObject/Ingredients", order = int.MaxValue)]
public class Ingredients : ScriptableObject, ISerializationCallbackReceiver
{
    public int i;
    public List<Stats> Stats = new List<Stats>();

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
        Meatball,
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
        PoppingCandy,
        LiveOctopus,
        NONE
    }
}

[System.Serializable]
public class Stats
{
    [HideInInspector] public Sprite SandwichSprite; //쌓이는 재료 이미지
    [HideInInspector] public Sprite IconSprite;//테두리가 없는 이미지
    [HideInInspector] public Sprite OutlineSprite;//테두리가 있는 이미지
    public Ingredients.Type name;
    public int Size;
    public float coliderPos;
    public float coliderSize;
}