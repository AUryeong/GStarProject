using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "MapData", menuName = "ScriptableObject/Map", order = 0)]
public class MapData : ScriptableObject
{
    public Sprite cameraBackgroundSprite;
    [Header("맵")] public float platformMapLength = 86.96436f;

    [Space(20)] public GameObject firstMap;
    public GameObject ovenMap;

    public List<GameObject> mapList;

    public SpriteRenderer[] mapBGList;

    public float[] platformmapBGLength;
    public float[] platformmapBGSpeed;
}