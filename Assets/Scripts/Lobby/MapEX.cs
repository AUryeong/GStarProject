using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapEX : MonoBehaviour
{
    [SerializeField] string Name;
    [SerializeField] Sprite Sprite;
    [SerializeField] int Rank;
    [SerializeField] string Inspector;
    [SerializeField] bool Lock;

    [SerializeField] Text MapName;
    [SerializeField] Image MapImage;
    [SerializeField] Image[] Ranks;
    [SerializeField] Text MapInspector;
    [SerializeField] GameObject LockPanel;

    private void Start()
    {
        MapName.text = Name;
        MapImage.sprite = Sprite;
        MapInspector.text = Inspector;
        LockPanel.SetActive(Lock);
        for(int i = 0; i < Rank;i++)
        {
            Ranks[i].color = Color.white;
        }
    }
}
