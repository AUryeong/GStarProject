using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PoolManager : Singleton<PoolManager>
{

    Dictionary<GameObject, List<GameObject>> pools = new Dictionary<GameObject, List<GameObject>>();

    public void AddPooling(GameObject origin, Transform parent)
    {
        if (!pools.ContainsKey(origin))
        {
            pools.Add(origin, new List<GameObject>());
        }
        foreach (Transform trans in parent)
        {
            GameObject obj = trans.gameObject;
            if (obj != origin)
                pools[origin].Add(obj);
        }
    }
    public void OriginChange(Transform origin, Transform obj)
    {
        obj.gameObject.layer = origin.gameObject.layer;
        obj.DOKill();
        obj.localPosition = origin.transform.localPosition;
        obj.rotation = origin.transform.rotation;
        obj.localScale = origin.transform.localScale;
        obj.gameObject.SetActive(true);
        if (obj.transform.childCount > 0)
            for (int i = 0; i < obj.childCount; i++)
                OriginChange(origin.GetChild(i), obj.GetChild(i));
    }
    public GameObject Init(GameObject origin)
    {
        if (origin != null)
        {
            GameObject copy = null;
            if (pools.ContainsKey(origin))
            {
                if (pools[origin].FindAll((GameObject x) => !x.activeSelf).Count > 0)
                {
                    copy = pools[origin].Find((GameObject x) => !x.activeSelf);
                    OriginChange(origin.transform, copy.transform);
                    return copy;
                }
            }
            else
            {
                pools.Add(origin, new List<GameObject>());
            }
            copy = Instantiate(origin);
            pools[origin].Add(copy);
            copy.SetActive(true);
            return copy;
        }
        return null;
    }
}
