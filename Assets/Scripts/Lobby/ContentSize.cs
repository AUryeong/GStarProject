using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContentSize : MonoBehaviour
{
    private RectTransform Rect;
    [SerializeField] float Size;
    [SerializeField] bool Quset;
    private int Count;
    void Start()
    {
        Rect = GetComponent<RectTransform>();

        for (int Contentidx = transform.childCount; Contentidx > 0; Contentidx--)
        {
            Count++;
        }

        if (!Quset)
            Rect.sizeDelta += new Vector2(Size * Count, 0);
        else
        {
            // 홀수일때 Size를 한번 더해준다
            Rect.sizeDelta += new Vector2(0, Size * Count + Size * (Count % 2));
        }
    }

}
