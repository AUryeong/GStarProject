using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapEX : MonoBehaviour
{
    public Map map;
    [SerializeField] string Name;
    [SerializeField] Sprite Sprite;
    [SerializeField] int Rank;
    [SerializeField] string Inspector;
    [SerializeField] int price;

    public bool Select { set 
        {
            if(value == false)
                SelectButton.image.sprite = buttonImage[0];
            else
                SelectButton.image.sprite = buttonImage[1];
        } }
    [SerializeField] Sprite[] buttonImage = new Sprite[3];

    [SerializeField] private TextMeshProUGUI MapName;
    [SerializeField] private Image MapImage;
    [SerializeField] private Image[] Ranks;
    [SerializeField] private TextMeshProUGUI MapInspector;
    [SerializeField] private GameObject LockPanel;
    [SerializeField] private Button SelectButton;
    [SerializeField] private TextMeshProUGUI priceText;

    [SerializeField] bool Lock;
    [SerializeField] private bool isBuy;
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
        SelectButton.onClick.AddListener(() => Buy());
    }
    void Buy()
    {
        if (isBuy)
            LobbyUIManager.Instance.ChangeMap(this);
        else if(GameManager.Instance.gold >= price)
        {
            LobbyUIManager.Instance.ChangeMap(this);
            priceText.gameObject.SetActive(false);
            Lock = false;
            LockPanel.gameObject.SetActive(Lock);

            GameManager.Instance.gold -= price;
            isBuy = true;
        }

    }
}
