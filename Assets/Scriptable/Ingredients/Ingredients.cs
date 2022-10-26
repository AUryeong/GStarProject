using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ingredients Data" , menuName = "ScriptableObject/Ingredients",order = int.MaxValue)]
[System.Serializable]
public class stats
{
    [HideInInspector] public Sprite SandwichSprite;
    [HideInInspector] public Sprite IconSprite;
    [HideInInspector] public Sprite OutlineSprite;
    public int Size;
}
public class Ingredients : ScriptableObject
{
    public List<stats> Stats = new List<stats>();
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
        PoplingCandy,
        LiveOctopus
    }
}
