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

    [Tooltip("절반을 기준으로 아래에서 들어가는거 위로 들어가는거 배치")] public List<Sprite> mapBlockSpriteList;

    public float[] platformmapBGLength;
    public float[] platformmapBGSpeed;
}