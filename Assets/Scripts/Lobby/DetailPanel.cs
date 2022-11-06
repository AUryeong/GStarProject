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
    private float priceUpValue = 0;
    private BreadScript breadScript;
    private void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }

    public void OpenPanel(BreadStats breadstats , BreadScript bread)
    {
        breadScript = bread;
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

        upgradeButton.onClick.AddListener(() => UpgradeBread());
        switch (scriptable.Rank)
        {
            case 0:
                {
                    //기존 가격 * (0.현재 레벨) + 1성 성장수치
                    priceUpValue = scriptable.Price * (scriptable.LV / 10) + 2100;
                    if (scriptable.isBuy == true)
                    {
                        priceText.text = $"{scriptable.Price + priceUpValue}";

                    }
                    else
                        priceText.text = $"구매 {scriptable.Price}";

                    //레벨-1 * 1성 성장수치
                    upgradeHp.text = $"{scriptable.HP + 20}";

                    break;
                }
            case 1:
                {
                    //기존 가격 * (0.현재 레벨) + 2성 성장수치
                    priceUpValue = scriptable.Price * (scriptable.LV / 10) + 2800;
                    priceText.text = $"{scriptable.Price + priceUpValue}";

                    //레벨-1 * 2성 성장수치
                    upgradeHp.text = $"{scriptable.HP + 35}";

                    break;
                }
            case 2:
                {
                    //기존 가격 * (0.현재 레벨) + 3성 성장수치
                    priceUpValue = scriptable.Price * (scriptable.LV / 10) + 3500;
                    priceText.text = $"{scriptable.Price + priceUpValue}";

                    //레벨-1 * 3성 성장수치
                    upgradeHp.text = $"{scriptable.HP + 40}";

                    break;
                }
        }//업그레이드 수치
    }
    private void UpgradeBread()
    {
        if(GameManager.Instance.gold <= priceUpValue)
        {
            LobbyUIManager.Instance.MoneyLess();
            return;
        }
        if(scriptable.LV == 6)
        {
            return;
        }
        if(scriptable.isBuy == false)
            scriptable.isBuy = true;


        switch (scriptable.Rank)
        {
            case 0:
                {
                    //기존 가격 * (0.현재 레벨) + 1성 성장수치
                    priceUpValue = scriptable.Price * (scriptable.LV / 10) + 2100;
                    priceText.text = $"{scriptable.Price + priceUpValue}";

                    //레벨-1 * 1성 성장수치
                    scriptable.HP += 20;
                    upgradeHp.text = $"{scriptable.HP +20}";

                    break;
                }
            case 1:
                {
                    //기존 가격 * (0.현재 레벨) + 2성 성장수치
                    priceUpValue = scriptable.Price * (scriptable.LV / 10) + 2800;
                    priceText.text = $"{scriptable.Price + priceUpValue}";

                    //레벨-1 * 2성 성장수치
                    scriptable.HP += 35;
                    upgradeHp.text = $"{scriptable.HP +35}";

                    break;
                }
            case 2:
                {
                    //기존 가격 * (0.현재 레벨) + 3성 성장수치
                    priceUpValue = scriptable.Price * (scriptable.LV / 10) + 3500;
                    priceText.text = $"{scriptable.Price + priceUpValue}";

                    //레벨-1 * 3성 성장수치
                    scriptable.HP += 40;
                    upgradeHp.text = $"{scriptable.HP +40}";

                    break;
                }
        }//업그레이드 수치
        breadScript.Upgrade();
        hpText.text = $"{scriptable.HP}";
    }
    public void CloseButton()
    {
        gameObject.SetActive(false);
    }
}
