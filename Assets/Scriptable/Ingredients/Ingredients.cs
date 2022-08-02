using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ingredients Data" , menuName = "ScriptableObject/Ingredients",order = int.MaxValue)]
public class Ingredients : ScriptableObject
{
    public List<Sprite> ImageSprite = new List<Sprite>();
}
