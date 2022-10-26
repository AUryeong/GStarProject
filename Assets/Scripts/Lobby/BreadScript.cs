using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BreadScript : MonoBehaviour
{
    [SerializeField] BreadStat scriptable;
    [Header("Main")]
    [SerializeField] Text mainName;
    [SerializeField] Button detailButton;
    [SerializeField] Image mainImage;
    [SerializeField] GameObject[] mainRanks;
    [Space(10f)]
    [SerializeField] Text mainLv;
    [SerializeField] Slider mainExp;
    [SerializeField] Text mainHp;
    [SerializeField] Text mainAbility;
    [Space(10f)]
    [SerializeField] Button mainSelectButton;
    [SerializeField] Text mainSelectText;
    [Header("DetailPanel")]
    [SerializeField] GameObject detailPanel;
    [Space(10f)]
    [SerializeField] GameObject[] detailRank;
    [SerializeField] Image detailImage;
    [SerializeField] Text detailName;
    [SerializeField] Text detailText;
    [Space(10f)]
    [SerializeField] Text detailLv;
    [SerializeField] Slider detailExp;
    [SerializeField] Text detailHp;
    [SerializeField] Text detailAbility;
    [Space(10f)]
    [SerializeField] Button detailLvUp;
    [SerializeField] Text detailPrice;
    [SerializeField] Text detailHpUp;
    [SerializeField] Text detailAbilityUp;
    void Update()
    {

    }
    public void BreadSetting(Breads breadscriptable,int breadCount)
    {
        scriptable = breadscriptable.Stats[breadCount];
        mainName.text = scriptable.Name;
        mainImage.sprite = scriptable.ImageSprite;
        mainSelectButton.onClick.AddListener(() => BuyButton(breadCount));

        if(scriptable.isBuy == true)
        {
            mainSelectText.text = "선택하기";
        }

        mainLv.text = $"{scriptable.LV}.LV";
        mainExp.value = scriptable.EXP / scriptable.MaxEXP;
        mainHp.text = $"{scriptable.HP}";

        for (int Rank = 0; Rank < scriptable.Rank; Rank++)
           mainRanks[Rank].gameObject.SetActive(true);
    }
    void BuyButton(in int idx)
    {
        LobbyUIManager.Instance.SelectBread = idx;
    /*    GameObject ClickButton = EventSystem.current.currentSelectedGameObject;
        SelectButtonTxt.text = "선택하기";
        SelectButtonTxt = ClickButton.GetComponentInChildren<Text>();
        SelectButtonTxt.text = "선택됨";*/

    }
}
