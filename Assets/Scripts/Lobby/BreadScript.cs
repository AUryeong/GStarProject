using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BreadScript : MonoBehaviour
{
    [SerializeField] BreadStat scriptable;
    [Header("Main")]
    [SerializeField] TextMeshProUGUI mainName;
    [SerializeField] Button detailButton;
    [SerializeField] Image mainImage;
    [SerializeField] GameObject[] mainRanks;
    [Space(10f)]
    [SerializeField] TextMeshProUGUI mainLv;
    [SerializeField] Slider mainExp;
    [SerializeField] TextMeshProUGUI mainHp;
    [SerializeField] TextMeshProUGUI mainAbility;
    [Space(10f)]
    [SerializeField] Button mainSelectButton;
    [SerializeField] TextMeshProUGUI mainSelectText;
    [Header("DetailPanel")]
    [SerializeField] GameObject detailPanel;
    [Space(10f)]
    [SerializeField] GameObject[] detailRank;
    [SerializeField] Image detailImage;
    [SerializeField] TextMeshProUGUI detailName;
    [SerializeField] TextMeshProUGUI detailText;
    [Space(10f)]
    [SerializeField] TextMeshProUGUI detailLv;
    [SerializeField] Slider detailExp;
    [SerializeField] TextMeshProUGUI detailHp;
    [SerializeField] TextMeshProUGUI detailAbility;
    [Space(10f)]
    [SerializeField] Button detailLvUp;
    [SerializeField] TextMeshProUGUI detailPrice;
    [SerializeField] TextMeshProUGUI detailHpUp;
    [SerializeField] TextMeshProUGUI detailAbilityUp;
    void Update()
    {

    }
    public void BreadSetting(Breads breadscriptable,Breads.Type breadCount)
    {
        scriptable = breadscriptable.Stats[(int)breadCount];
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
    void BuyButton(Breads.Type idx)
    {
        LobbyUIManager.Instance.selectBread = idx;
    /*    GameObject ClickButton = EventSystem.current.currentSelectedGameObject;
        SelectButtonTxt.text = "선택하기";
        SelectButtonTxt = ClickButton.GetComponentInChildren<Text>();
        SelectButtonTxt.text = "선택됨";*/

    }
}
