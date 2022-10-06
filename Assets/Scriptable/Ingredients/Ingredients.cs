using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ingredients Data" , menuName = "ScriptableObject/Ingredients",order = int.MaxValue)]
[System.Serializable]
public class stats
{
    public Sprite SandwichSprite;
    public Sprite IconSprite;
    public Sprite OutlineSprite;
    public int Size;
}
public class Ingredients : ScriptableObject
{
    public List<stats> Stats = new List<stats>();
}
