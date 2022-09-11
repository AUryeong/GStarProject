using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ingredients Data" , menuName = "ScriptableObject/Ingredients",order = int.MaxValue)]
[System.Serializable]
public class stats
{
    public Sprite ImageSprite;
    public string Name;
    public int Rank;
    public int LV;
    public float MaxEXP;
    public float EXP;
    public int HP;
    public int Price;
    public int Size;
}
public class Ingredients : ScriptableObject
{
    public List<stats> Stats = new List<stats>();
}
