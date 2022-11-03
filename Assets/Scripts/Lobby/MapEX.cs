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
    public bool Select { set 
        {
            Debug.Log(value);
            if(value == false)
                SelectButton.image.sprite = buttonImage[0];
            else
                SelectButton.image.sprite = buttonImage[1];
        } }
    [SerializeField] Sprite[] buttonImage = new Sprite[2];

    [SerializeField] private Text MapName;
    [SerializeField] private Image MapImage;
    [SerializeField] private Image[] Ranks;
    [SerializeField] private Text MapInspector;
    [SerializeField] private GameObject LockPanel;
    [SerializeField] private Button SelectButton;

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
        SelectButton.onClick.AddListener(() => LobbyUIManager.Instance.ChangeMap(this));
    }
}
