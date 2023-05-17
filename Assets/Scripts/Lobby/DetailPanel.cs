using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;


public class DetailPanel : MonoBehaviour
{
    static public DetailPanel instance { get; private set; }
    private BreadStats scriptable;

    [Header("Image")]
    [SerializeField] private GameObject[] rankGroup = new GameObject[3];
    [SerializeField] private Image breadImage;

    [Header("More Information")]
    [SerializeField] private TextMeshProUGUI breadInspector;

    [Header("Inspector")]
    [SerializeField] private TextMeshProUGUI breadName;

    [SerializeField] private TextMeshProUGUI lvText;
    [SerializeField] private Image expBar;
    [SerializeField] private Sprite[] expSprite;

    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI abilityText;

    [Header("UpgradeButton")]
    [SerializeField] private Button upgradeButton;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI upgradeHp;
    private float priceUpValue = 0;
    private BreadScript breadScript;
    private void Awake()
    {
        instance = this;
    }

    public void OpenPanel(BreadStats breadstats , BreadScript bread)
    {
        transform.DOScale(1, 0.5f);

        breadScript = bread;
        scriptable = breadstats;
        for (int idx = 0; idx < 3; idx++)
        {
            rankGroup[idx].SetActive((idx < scriptable.Rank));
        }

        breadImage.sprite = scriptable.ImageSprite;
        breadInspector.text = scriptable.AbilityText_1;

        breadName.text = scriptable.Name;

        lvText.text = $"LV.{scriptable.LV}";
        expBar.sprite = expSprite[scriptable.LV];


        hpText.text = $"{scriptable.GetHp()}";
        abilityText.text = scriptable.AbilityText_2;

        upgradeButton.onClick.RemoveAllListeners();
        upgradeButton.onClick.AddListener(() => UpgradeBread());
        TextChange();
    }

    private void UpgradeBread()
    {
        print(priceUpValue);
        if (GameManager.Instance.gold <= scriptable.Price + priceUpValue)
        {
            LobbyUIManager.Instance.MoneyLess();
            return;
        }
        else
            GameManager.Instance.gold -= (int)(scriptable.Price + priceUpValue);

        breadScript.Upgrade();

        TextChange();

        hpText.text = $"{scriptable.GetHp()}";
        lvText.text = $"LV.{scriptable.LV}";
        expBar.sprite = expSprite[scriptable.LV];
    }
    private void TextChange()
    {
        if(scriptable.isBuy == false)
        {
            priceUpValue = scriptable.Price;
            priceText.text = $"{scriptable.Price}";

            upgradeHp.text = $"{scriptable.GetHp()}";
            return;
        }
        if(scriptable.LV == 6)
        {
            priceText.text = $"최대 레벨!";
            upgradeHp.text = $"Max";
            return;
        }
        switch (scriptable.Rank)//업그레이드 수치
        {
            case 1:
                {
                    //기존 가격 * (0.현재 레벨) + 1성 성장수치
                    priceUpValue = (scriptable.Price * ((float)scriptable.LV / 10) + 2100)*scriptable.LV;
                    priceText.text = $"{scriptable.Price + priceUpValue}";

                    //1성 성장수치
                    upgradeHp.text = $"{scriptable.GetHp() + 20}";

                    break;
                }
            case 2:
                {
                    //기존 가격 * (0.현재 레벨) + 2성 성장수치
                    priceUpValue = (scriptable.Price * (scriptable.LV / 10) + 2800) * scriptable.LV;
                    priceText.text = $"{scriptable.Price + priceUpValue}";

                    //2성 성장수치
                    upgradeHp.text = $"{scriptable.GetHp() + 35}";

                    break;
                }
            case 3:
                {
                    //기존 가격 * (0.현재 레벨) + 3성 성장수치
                    priceUpValue = (scriptable.Price * (scriptable.LV / 10) + 3500) * scriptable.LV;
                    priceText.text = $"{scriptable.Price + priceUpValue}";

                    //3성 성장수치
                    upgradeHp.text = $"{scriptable.GetHp() + 40}";

                    break;
                }
        }
    }
    public void CloseButton()
    {
        transform.DOScale(0, 0.5f);
    }
}
