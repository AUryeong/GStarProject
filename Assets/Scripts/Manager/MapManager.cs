using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum Map
{
    Cafe,
    Food,
    Kitchen,
    BreadShop
}

public class MapManager : MonoBehaviour
{
    [Header("맵")]
    public float platformMapLength = 86.96436f;
    
    [Space(20)]
    [SerializeField] protected GameObject firstMap;
    [SerializeField] protected GameObject ovenMap;

    [FormerlySerializedAs("firstMapList")] [SerializeField]
    protected List<GameObject> mapList;

    protected int mapSize = 1;
    public readonly int ovenMapSize = 10;
    private float mapLength;
    private List<GameObject> createdMapList = new List<GameObject>();

    private float[] mapBGLength;
    [SerializeField] protected SpriteRenderer[] mapBGList;
    protected Dictionary<int, List<GameObject>> mapDictionary = new Dictionary<int, List<GameObject>>();
    [SerializeField] protected float[] platformmapBGLength;

    [SerializeField] protected float[] platformmapBGSpeed;

    // Start is called before the first frame update
    public void Init()
    {
        mapBGLength = new float[mapBGList.Length];
        mapLength = platformMapLength / 2;

        for (int i = 0; i < mapBGLength.Length; i++)
            mapDictionary.Add(i, new List<GameObject>());

        CreateFirstPlatform();
        AddNewPlatform();
    }

    private void CreateFirstPlatform()
    {
        var platform = PoolManager.Instance.Init(firstMap);
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
            platform = PoolManager.Instance.Init(ovenMap);
        else
            platform = PoolManager.Instance.Init(mapList[Random.Range(0, mapList.Count)]);
        platform.transform.position = new Vector3(mapLength + platformMapLength / 2, 1.5f, 0);
        mapLength += platformMapLength;
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
            if (platformmapBGSpeed.Length > i)
            {
                if (platformmapBGSpeed[i] != 0)
                {
                    mapBGLength[i] += platformmapBGSpeed[i] * Time.deltaTime;
                    var power = new Vector3(platformmapBGSpeed[i] * Time.deltaTime, 0, 0);
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

        GameObject obj = PoolManager.Instance.Init(mapBGList[index].gameObject);
        obj.transform.position = new Vector3(mapBGLength[index], mapBGList[index].transform.position.y, mapBGList[index].transform.position.z);
        mapBGLength[index] += platformmapBGLength[index];
        if (!createdMapList.Contains(obj))
            createdMapList.Add(obj);
        if (!mapDictionary[index].Contains(obj))
            mapDictionary[index].Add(obj);
    }
}