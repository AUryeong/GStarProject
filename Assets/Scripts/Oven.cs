using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oven : MonoBehaviour
{
    protected float distance;
    protected bool isOvenable = false;
    protected void OnEnable()
    {
        distance = InGameManager.Instance.player.transform.position.x - transform.position.x;
        isOvenable = true;
    }
    protected void Update()
    {
        if (!isOvenable)
            return;

        float dis = InGameManager.Instance.player.transform.position.x - transform.position.x;
        if (Mathf.Abs(dis) > Mathf.Abs(distance))
        {
            isOvenable = false;
            InGameManager.Instance.player.GetOven(gameObject);
        }
        else
            distance = dis;

    }
}
