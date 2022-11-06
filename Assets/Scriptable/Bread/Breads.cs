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
        Tufu,
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

    public List<BreadStats> Stats = new List<BreadStats>();
}
[System.Serializable]
public class BreadStats
{
    public string Name;
    public Sprite ImageSprite;
    public Sprite stackSprite;
    public int Rank;
    public int LV;
    public int HP;
    public int GetHp()
    {
        int hp = HP;
        switch (Rank)
        {
            case 1:
                hp += 20 * LV;
                break;
            case 2:
                hp += 35 * LV;
                break;
            case 3:
                hp += 40 * LV;
                break;
        }
        return hp;
    }
    public float Price;
    public int AbilityLV_1;
    public int AbilityLV_2;
    public Sprite AbilityImage_1;
    public Sprite AbilityImage_2;
    [TextArea]
    public string AbilityText_1;
    [TextArea]
    public string AbilityText_2;
    public bool isBuy;
}
