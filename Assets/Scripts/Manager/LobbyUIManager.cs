using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
class BreadInspector
{
    Text LVText;
    Slider Text;
}
public class LobbyUIManager : Singleton<LobbyUIManager>
{
    #region Test
    public int Gold = 10000;//테스팅용 추후에 게임매니저로 이동
    int MaxHpLv = 0;
    int DefenseLv = 0;
    int SelectAbility = 0;//0 : MaxHp , 1 : Defense
    #endregion
    [Header("Top UI")]
    [SerializeField] Text GoldText;
    [SerializeField] Text BuyGoldText;
    [SerializeField] Text StaminaText;
    [SerializeField] Image StaminaImage;

    [Header("Shop Object")]
    [SerializeField] GameObject BackGroundObjcet;
    [SerializeField] GameObject Bread;
    [SerializeField] GameObject Quset;
    [SerializeField] GameObject Map;
    private GameObject OpenObject;

    [Header("Bread")]
    [SerializeField] Breads BreadScriptable;
    [SerializeField] List<GameObject> BreadPanels = new List<GameObject>();//빵 정보창 오브젝트
    private List<Text> LVTexts = new List<Text>();//레벨 텍스트 리스트
    private List<Slider> EXPSliders = new List<Slider>();//경험치바 리스트
    private List<Text> HPTexts = new List<Text>();//체력 텍스트 리스트
    private List<Text> BuyButtonText = new List<Text>();//레벨업 버튼 텍스트
    private int SelectBread;
    private Text SelectButtonTxt;

    [Header("Quset")]
    [SerializeField] List<GameObject> QusetPanels = new List<GameObject>();
    private List<Image> QusetImage = new List<Image>();//퀘스트 보상 이미지
    private List<Text> Qusetrewards = new List<Text>();//퀘스트 보상 텍스트
    private List<Text> QusetText = new List<Text>();//퀘스트 내용 텍스트
    private List<Slider> QusetSliders = new List<Slider>();//퀘스트 진행도 슬라이더
    [Header("Map")]
    [SerializeField] List<GameObject> MapPanel = new List<GameObject>();
    [SerializeField] bool[] MapLock;
    [Header("Start")]
    [SerializeField] GameObject StartPanel;//정비 화면
    [SerializeField] GameObject ReCheckPanel;//정비 화면에서 시작을 누를떄 나오는 이미지
    [SerializeField] Image AbilityImage_Main;//클릭 했을떄 나오는 이미지
    [SerializeField] Text AbilityNameAndLV;//스킬 이름과 레벨텍스트
    [SerializeField] Text UpgradeMoney;//가격 텍스트
    [SerializeField] Text AbilityExplanation;//스킬 설명 텍스트
    [SerializeField] Sprite[] AbilitySprite = new Sprite[2];//스킬 이미지
    [SerializeField] string[,] AbilityText = new string[2,2];//스킬 설명 내용
    // Start is called before the first frame update
    void Start()
    {
        SettingBreadShop(BreadScriptable);
        OpenMap();
        Quset.SetActive(false);
        Map.SetActive(false);
        BackGroundObjcet.SetActive(false);
    }
    public void OpenPanel(GameObject Object)
    {
        BackGroundObjcet.SetActive(true);
        Object.SetActive(true);
        OpenObject = Object;
    }
    public void ClosePanel()
    {
        BackGroundObjcet.SetActive(false);
        OpenObject.SetActive(false);
    }

    void SettingBreadShop(Breads BreadList)
    {
        int BreadCount = 0;
        foreach (BreadStat Bread in BreadList.Stats)
        {
            BreadPanels[BreadCount].SetActive(true);
            int idx = BreadCount;
            GameObject BreadPanelObject = BreadPanels[BreadCount];

            BreadPanelObject.transform.Find("BreadName").GetComponent<Text>().text = Bread.Name;//BreadName Text
            BreadPanelObject.transform.Find("BreadImage").GetComponent<Image>().sprite = Bread.ImageSprite;//BreadImage
            BreadPanelObject.transform.Find("BreadInspector").GetChild(0).GetChild(1).GetComponent<Image>().sprite = Bread.ImageSprite;//BreadAction
            BreadPanelObject.transform.Find("BreadInspector").GetChild(1).GetComponent<Button>().onClick.AddListener(() => BuyButton(idx));//price

            SelectButtonTxt = BreadPanelObject.transform.Find("BreadInspector").GetChild(1).GetChild(0).GetComponent<Text>();
            SelectButtonTxt.text = "선택하기";
            LVTexts.Add(BreadPanelObject.transform.Find("LV").GetChild(0).GetComponent<Text>());
            EXPSliders.Add(BreadPanelObject.transform.Find("LV").GetChild(1).GetComponent<Slider>());
            HPTexts.Add(BreadPanelObject.transform.Find("BreadInspector").GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>());
            LVTexts[BreadCount].text = $"{Bread.LV}.LV";//LvText
            EXPSliders[BreadCount].value = Bread.EXP / Bread.MaxEXP;//ExpSlider
            HPTexts[BreadCount].text = $"{Bread.HP}";//BreadHP

            for (int Rank = 0; Rank < Bread.Rank; Rank++)
                BreadPanelObject.transform.Find("BreadRankGroup").transform.GetChild(Rank).gameObject.SetActive(true);

            BreadCount++;
        }
        Bread.SetActive(false);
    }
    void BuyButton(in int idx)
    {
        Debug.Log(idx);
        SelectBread = idx;
        GameObject ClickButton = EventSystem.current.currentSelectedGameObject;
        SelectButtonTxt.text = "선택하기";
        SelectButtonTxt = ClickButton.GetComponentInChildren<Text>();
        SelectButtonTxt.text = "선택됨";

    }
    public void OpenStartPanel()
    {
        StartPanel.SetActive(true);
    }
    public void CloseStartPanel()
    {
        StartPanel.SetActive(false);
    }
    public void OpenReCheck()
    {
        ReCheckPanel.SetActive(true);
    }
    public void CloseReCheck()
    {
        ReCheckPanel.SetActive(true);
    }
    public void StartButton()
    {
        SceneManager.LoadScene(0);
    }
    public void AbilitySelect(int idx)
    {
        AbilityImage_Main.sprite = AbilitySprite[idx];
        AbilityNameAndLV.text = idx == 0 ? $"최대 체력 LV.{MaxHpLv}" : $"충돌 데미지 감소 LV.{DefenseLv}";
        UpgradeMoney.text = idx == 0 ? $"{5000 + (MaxHpLv - 1) * 500} Gold" : $"{5000 + (DefenseLv - 1) * 500} Gold";
        SelectAbility = idx;
    }
    public void UpgradeAbility()
    {
        Gold -= SelectAbility == 0 ? 5000 - (MaxHpLv++ - 1) * 500 : 5000 + (DefenseLv++ - 1) * 500;
        AbilityNameAndLV.text = SelectAbility == 0 ? $"최대 체력 LV.{MaxHpLv}" : $"충돌 데미지 감소 LV.{DefenseLv}";
        UpgradeMoney.text = SelectAbility == 0 ? $"{5000 + (MaxHpLv - 1) * 500} Gold" : $"{5000 + (DefenseLv - 1) * 500} Gold";
    }
    private void OpenMap()
    {
        for (int i = MapPanel.Count; i > 0; i--)
        {
            MapPanel[i].SetActive(MapLock[i]);
        }
    }
}
