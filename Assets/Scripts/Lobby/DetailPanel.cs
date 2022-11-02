using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class DetailPanel : MonoBehaviour
{
    static public DetailPanel instance { get; private set; }
    private BreadStats scriptable;
    //Image
    [SerializeField] private GameObject[] rankGroup = new GameObject[3];
    [SerializeField] private Image breadImage;

    //More Information
    [SerializeField] private TextMeshProUGUI breadInspector;

    //Inspector
    [SerializeField] private TextMeshProUGUI breadName;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI abilityText;

    //UpgradeButton
    [SerializeField] private Button upgradeButton;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI upgradeHp;

    private void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }

    public void OpenPanel(BreadStats breadstats)
    {
        gameObject.SetActive(true);
        scriptable = breadstats;
        for (int idx = 0; idx < 3; idx++)
        {
            rankGroup[idx].SetActive((idx < scriptable.Rank));
        }

        breadImage.sprite = scriptable.ImageSprite;
        breadInspector.text = scriptable.AbilityText_1;

        breadName.text = scriptable.Name;
        hpText.text = $"{scriptable.HP}";
        abilityText.text = scriptable.AbilityText_2;

        float priceUpValue = 0;
        float hpUpValue = 0;
        upgradeButton.onClick.AddListener(() => UpgradeBread());
        switch (scriptable.Rank)
        {
            case 0:
                {
                    //기존 가격 * (0.현재 레벨) + 1성 성장수치
                    priceUpValue = scriptable.Price * (scriptable.LV / 10) + 2100;
                    priceText.text = $"{scriptable.Price + priceUpValue}";

                    //레벨-1 * 1성 성장수치
                    hpUpValue = scriptable.LV * 20;
                    upgradeHp.text = $"{scriptable.HP + hpUpValue}";

                    break;
                }
            case 1:
                {
                    //기존 가격 * (0.현재 레벨) + 2성 성장수치
                    priceUpValue = scriptable.Price * (scriptable.LV / 10) + 2800;
                    priceText.text = $"{scriptable.Price + priceUpValue}";

                    //레벨-1 * 2성 성장수치
                    hpUpValue = scriptable.LV * 35;
                    upgradeHp.text = $"{scriptable.HP + hpUpValue}";

                    break;
                }
            case 2:
                {
                    //기존 가격 * (0.현재 레벨) + 3성 성장수치
                    priceUpValue = scriptable.Price * (scriptable.LV / 10) + 3500;
                    priceText.text = $"{scriptable.Price + priceUpValue}";

                    //레벨-1 * 3성 성장수치
                    hpUpValue = scriptable.LV * 40;
                    upgradeHp.text = $"{scriptable.HP + hpUpValue}";

                    break;
                }
        }//업그레이드 수치
    }
    private void UpgradeBread()
    {
        scriptable.LV++;

        float priceUpValue = 0;
        float hpUpValue = 0;

        switch (scriptable.Rank)
        {
            case 0:
                {
                    //기존 가격 * (0.현재 레벨) + 1성 성장수치
                    priceUpValue = scriptable.Price * (scriptable.LV / 10) + 2100;
                    priceText.text = $"{scriptable.Price + priceUpValue}";

                    //레벨-1 * 1성 성장수치
                    hpUpValue = (scriptable.LV - 1) * 20;
                    upgradeHp.text = $"{scriptable.HP + hpUpValue}";

                    break;
                }
            case 1:
                {
                    //기존 가격 * (0.현재 레벨) + 2성 성장수치
                    priceUpValue = scriptable.Price * (scriptable.LV / 10) + 2800;
                    priceText.text = $"{scriptable.Price + priceUpValue}";

                    //레벨-1 * 2성 성장수치
                    hpUpValue = (scriptable.LV - 1) * 35;
                    upgradeHp.text = $"{scriptable.HP + hpUpValue}";

                    break;
                }
            case 2:
                {
                    //기존 가격 * (0.현재 레벨) + 3성 성장수치
                    priceUpValue = scriptable.Price * (scriptable.LV / 10) + 3500;
                    priceText.text = $"{scriptable.Price + priceUpValue}";

                    //레벨-1 * 3성 성장수치
                    hpUpValue = (scriptable.LV - 1) * 40;
                    upgradeHp.text = $"{scriptable.HP + hpUpValue}";

                    break;
                }
        }//업그레이드 수치

        hpText.text = $"{scriptable.HP + hpUpValue}";
    }
    public void CloseButton()
    {
        gameObject.SetActive(false);
    }
}
