using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bread Data", menuName = "ScriptableObject/Breads", order = int.MaxValue)]
public class Breads : ScriptableObject
{
    public enum Type
    {
        Milk,
        Warning,
        Sesame,
        WaterMelon,
        Punch,
        Tofu,
        Buttertop,
        Pullman,
        Toast,
        Barguette,
        Flat,
        Car,
        Safe,
        Prison,
        Ramen
    }

    public List<BreadStat> Stats = new List<BreadStat>();
}
[System.Serializable]
public class BreadStat
{
    public string Name;
    public Sprite ImageSprite;
    public int Rank;
    public int LV;
    public int EXP;
    public int MaxEXP;
    public int HP;
    public int Price;
}
