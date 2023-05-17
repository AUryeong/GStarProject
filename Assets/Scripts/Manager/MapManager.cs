using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum Map
{
    Cafe,
    Sandwich,
    Food,
    Kitchen,
    BreadShop
}

public class MapManager : MonoBehaviour
{
    public List<MapData> mapDatas = new List<MapData>();
    public MapData selectMapData { get; private set; }
    [SerializeField] private SpriteRenderer cameraBackground;
    [SerializeField] private SpriteRenderer cameraBackground2;

    private float[] mapBGLength;

    protected int mapSize = 1;
    public readonly int ovenMapSize = 10;
    private float mapLength;
    private List<GameObject> createdMapList = new List<GameObject>();

    protected Dictionary<int, List<GameObject>> mapDictionary = new Dictionary<int, List<GameObject>>();

    public void Init()  
    {
        selectMapData = mapDatas[(int)GameManager.Instance.selectMap];

        cameraBackground.sprite = selectMapData.cameraBackgroundSprite;
        cameraBackground2.sprite = selectMapData.cameraBackgroundSprite;

        mapBGLength = new float[selectMapData.mapBGList.Length];
        mapLength = selectMapData.platformMapLength / 2;

        for (int i = 0; i < mapBGLength.Length; i++)
            mapDictionary.Add(i, new List<GameObject>());

        CreateFirstPlatform();
        AddNewPlatform();
    }

    private void CreateFirstPlatform()
    {
        var platform = PoolManager.Instance.Init(selectMapData.firstMap);
        platform.transform.position = new Vector3(0, 1.5f, 0);
        if (!createdMapList.Contains(platform))
            createdMapList.Add(platform);
    }

    protected void AddNewPlatform()
    {
        foreach (GameObject obj in createdMapList)
        {
            if (obj.gameObject.activeSelf)
                if (mapLength - obj.transform.position.x > 45)
                    obj.gameObject.SetActive(false);
        }

        GameObject platform;
        if (mapSize % ovenMapSize == 0)
            platform = PoolManager.Instance.Init(selectMapData.ovenMap);
        else
            platform = PoolManager.Instance.Init(selectMapData.mapList[Random.Range(0, selectMapData.mapList.Count)]);
        platform.transform.position = new Vector3(mapLength + selectMapData.platformMapLength / 2, 1.5f, 0);
        mapLength += selectMapData.platformMapLength;
        mapSize++;
        if (!createdMapList.Contains(platform))
            createdMapList.Add(platform);
    }

    public void MapUpdate()
    {
        if (mapLength - InGameManager.Instance.player.transform.position.x < 15)
            AddNewPlatform();
        BackgroundUpdate();
    }

    private void BackgroundUpdate()
    {
        for (int i = 0; i < mapBGLength.Length; i++)
        {
            if (selectMapData.platformmapBGSpeed.Length > i)
            {
                if (selectMapData.platformmapBGSpeed[i] != 0)
                {
                    mapBGLength[i] += selectMapData.platformmapBGSpeed[i] * Time.deltaTime;
                    var power = new Vector3(selectMapData.platformmapBGSpeed[i] * Time.deltaTime, 0, 0);
                    foreach (var obj in mapDictionary[i])
                    {
                        if (obj.gameObject.activeSelf)
                            obj.transform.position += power;
                    }
                }
            }

            if (mapBGLength[i] - InGameManager.Instance.player.transform.position.x < 100)
                AddNewBackground(i);
        }
    }

    private void AddNewBackground(int index)
    {
        foreach (var disableObj in mapDictionary[index])
        {
            if (disableObj.gameObject.activeSelf)
                if (mapBGLength[index] - disableObj.transform.position.x > 200)
                    disableObj.gameObject.SetActive(false);
        }

        GameObject obj = PoolManager.Instance.Init(selectMapData.mapBGList[index].gameObject);
        obj.transform.position = new Vector3(mapBGLength[index], selectMapData.mapBGList[index].transform.position.y, selectMapData.mapBGList[index].transform.position.z);
        mapBGLength[index] += selectMapData.platformmapBGLength[index];
        if (!createdMapList.Contains(obj))
            createdMapList.Add(obj);
        if (!mapDictionary[index].Contains(obj))
            mapDictionary[index].Add(obj);
    }
}