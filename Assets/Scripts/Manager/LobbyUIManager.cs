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
    private int SelectAbility = 0;//0 : MaxHp , 1 : Defense
    #endregion
    [Header("Top UI")]
    [SerializeField] Text GoldText;
    [SerializeField] Text BuyGoldText;
    [SerializeField] Text StaminaText;
    [SerializeField] Image StaminaImage;

    [Header("Shop Object")]
    [SerializeField] GameObject ShopBgPanel;
    [SerializeField] GameObject Bread;
    [SerializeField] GameObject Quset;
    [SerializeField] GameObject Map;
    private GameObject ShopPanelObject;

    [Header("Bread")]
    [SerializeField] Breads BreadScriptable;
    [SerializeField] GameObject BreadPrefab;
    [SerializeField] GameObject BreadContent;
    //[SerializeField] List<GameObject> BreadPanels = new List<GameObject>();//빵 정보창 오브젝트
    private List<Text> LVTexts = new List<Text>();//레벨 텍스트 리스트
    private List<Slider> EXPSliders = new List<Slider>();//경험치바 리스트
    private List<Text> HPTexts = new List<Text>();//체력 텍스트 리스트
    private List<Text> BuyBtuttonText = new List<Text>();//선택 버튼 텍스트
    private int SelectBread;
    private Text SelectButtonTxt;

    [Header("Quset")]
    public QusetScriptable[] qusetScriptables;
    public ScrollRect qusetScroll;
    public RectTransform[] qusetPanel;
    public Button[] qusetButtons;
    public GameObject qusetPrefab;
    private int openingQusetPanel = 0;
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
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        SettingBreadShop(BreadScriptable);
        SettingQusetPanel();
    }
    void Start()
    {
        Quset.SetActive(false);
        Map.SetActive(false);
        ShopBgPanel.SetActive(false);
    }


    public void OpenShopPanel(GameObject Object)
    {
        ShopPanelObject = Object;
        ShopBgPanel.SetActive(true);
        ShopPanelObject.SetActive(true);
    }
    public void ClosePanel()
    {
        ShopBgPanel.SetActive(false);
        ShopPanelObject.SetActive(false);
    }

    void SettingBreadShop(Breads BreadList)
    {
        int BreadCount = 0;
        foreach (BreadStat Bread in BreadList.Stats)
        {
            //BreadPanels[BreadCount].SetActive(true);
            //GameObject BreadPanelObject = BreadPanels[BreadCount];
            GameObject BreadPanelObject = Instantiate(BreadPrefab, transform.position, transform.rotation, BreadContent.transform);

            BreadPanelObject.transform.Find("BreadName").GetComponent<Text>().text = Bread.Name;//BreadName Text
            BreadPanelObject.transform.Find("BreadImage").GetComponent<Image>().sprite = Bread.ImageSprite;//BreadImage
            BreadPanelObject.transform.Find("BreadInspector").GetChild(0).GetChild(1).GetComponent<Image>().sprite = Bread.ImageSprite;//BreadAction
            BreadPanelObject.transform.Find("BreadInspector").GetChild(1).GetComponent<Button>().onClick.AddListener(() => BuyButton(BreadCount));//price

            BuyBtuttonText.Add(BreadPanelObject.transform.Find("BreadInspector").GetChild(1).GetChild(0).GetComponent<Text>());//Buy.Text
            BuyBtuttonText[BreadCount].text = "선택하기";

            LVTexts.Add(BreadPanelObject.transform.Find("LV").GetChild(0).GetComponent<Text>());//LV Texts
            EXPSliders.Add(BreadPanelObject.transform.Find("LV").GetChild(1).GetComponent<Slider>());//Exp Texts
            HPTexts.Add(BreadPanelObject.transform.Find("BreadInspector").GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>());//Hp Texts

            LVTexts[BreadCount].text = $"{Bread.LV}.LV";
            EXPSliders[BreadCount].value = Bread.EXP / Bread.MaxEXP;
            HPTexts[BreadCount].text = $"{Bread.HP}";

            GameObject Ranks = BreadPanelObject.transform.Find("BreadRankGroup").gameObject;
            for (int Rank = 0; Rank < Bread.Rank; Rank++)
                Ranks.transform.GetChild(Rank).gameObject.SetActive(true);

            BreadCount++;
        }
        Bread.SetActive(false);
    }
    void SettingQusetPanel()
    {
        for (int i = 0; i < 3; i++)//0 : 일일 / 1 : 주간 / 2 : 메인
        {
            for (int j = 0; j < qusetScriptables[i].QusetList.Count; j++)
            {
                Instantiate(qusetPrefab, transform.position, transform.rotation, qusetPanel[i].transform)
                    .GetComponent<QusetScript>().SettingQuset(qusetScriptables[i], j);
            }
        }
    }
    public void OpenQusetPanel(int Type)
    {
        qusetScroll.content = qusetPanel[Type];//콘텐츠 변경
        qusetPanel[openingQusetPanel].gameObject.SetActive(false);//이전 퀘스트창 비활성화
        qusetPanel[Type].gameObject.SetActive(true);//열려는 퀘스트창 활성화
        qusetButtons[openingQusetPanel].transform.position -= new Vector3(0, 20, 0);//이전 선택창 내리기
        qusetButtons[Type].transform.position += new Vector3(0, 20, 0);//선택창 올리기
        Image openingColor = qusetButtons[openingQusetPanel].gameObject.GetComponent<Image>();
        Image openPanelColor = qusetButtons[Type].gameObject.GetComponent<Image>();
        openingColor.color = new Color(0.75f, 0.75f, 0.75f);//버튼 회색으로 변경
        openPanelColor.color = new Color(1, 1, 1);//버튼 흰색으로 변경
        openingQusetPanel = Type;
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
    public void AbilitySelect(int idx)
    {
        SelectAbility = idx;
        switch (SelectAbility)
        {
            case 0:
                {

                    AbilityImage_Main.sprite = AbilitySprite[SelectAbility];
                    AbilityNameAndLV.text = $"최대 체력 LV.{GameManager.Instance.MaxHpLv}";
                    UpgradeMoney.text = $"{5000 + (GameManager.Instance.MaxHpLv - 1) * 500} Gold";
                    AbilityExplanation.text = $"추가 체력이 총 {GameManager.Instance.MaxHpLv * 5} 늘어납니다.체력이 떨어지면 빵들은 더 이상 재료를 모으지 못하니 지속적으로 강화합시다.";
                    break;
                }
            case 1:
                {
                    AbilityImage_Main.sprite = AbilitySprite[SelectAbility];
                    AbilityNameAndLV.text = $"충돌 데미지 감소 LV.{GameManager.Instance.DefenseLv}";
                    UpgradeMoney.text = $"{5000 + (GameManager.Instance.DefenseLv - 1) * 500} Gold";
                    AbilityExplanation.text = $"장애물 충돌 시 데미지를 {GameManager.Instance.DefenseLv * 5} %만큼 감소됩니다. 무슨일이 일어날지 모르니 만일을위해 강화해둡시다.";
                    break;
                }
        }
    }
    public void UpgradeAbility()
    {
        switch (SelectAbility)
        {
            case 0:
                {
                    AbilityImage_Main.sprite = AbilitySprite[SelectAbility];
                    UpgradeMoney.text = $"{5000 + (++GameManager.Instance.MaxHpLv - 1) * 500} Gold";
                    AbilityNameAndLV.text = $"최대 체력 LV.{GameManager.Instance.MaxHpLv}";
                    AbilityExplanation.text = $"추가 체력이 총 {GameManager.Instance.MaxHpLv * 5} 늘어납니다.체력이 떨어지면 빵들은 더 이상 재료를 모으지 못하니 지속적으로 강화합시다.";
                    break;
                }
            case 1:
                {
                    AbilityImage_Main.sprite = AbilitySprite[SelectAbility];
                    UpgradeMoney.text = $"{5000 + (++GameManager.Instance.DefenseLv - 1) * 500} Gold";
                    AbilityNameAndLV.text = $"충돌 데미지 감소 LV.{GameManager.Instance.DefenseLv}";
                    AbilityExplanation.text = $"장애물 충돌 시 데미지를 {GameManager.Instance.DefenseLv * 5} %만큼 감소됩니다. 무슨일이 일어날지 모르니 만일을위해 강화해둡시다.";
                    break;
                }
        }
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
}
