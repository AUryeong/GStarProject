using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
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
    [SerializeField] Text MacaronText;
    [SerializeField] Text StaminaText;
    [SerializeField] Image StaminaImage;

    [Header("Shop Object")]
    [SerializeField] GameObject ShopBgPanel;
    [SerializeField] GameObject Bread;
    [SerializeField] GameObject Quset;
    [SerializeField] GameObject Map;
    [SerializeField] Vector3 shopOpenPos;
    [SerializeField] Vector3 shopClosePos;
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
    public int SelectBread;
    private Text SelectButtonTxt;

    [Header("Quset")]
    public QusetScriptable[] qusetScriptables;
    public ScrollRect qusetScroll;
    public RectTransform[] qusetPanel;
    public Button[] qusetButtons;
    public Sprite[] qusetButtonsSprite;
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
    [SerializeField] Vector3 startOpenPos;
    [SerializeField] Vector3 startClosePos;
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        SettingBreadShop(BreadScriptable);
        SettingQusetPanel();
    }
    void Start()
    {

    }
    void Update()
    {
        GoldText.text = $"{GameManager.Instance.Gold}";
        MacaronText.text = $"{GameManager.Instance.Macaron}";
    }

    public void OpenShopPanel(GameObject Object)
    {
        if (ShopPanelObject != null)
        {
            ShopPanelObject.transform.DOKill();
            ShopPanelObject.transform.DOLocalMove(shopClosePos, 0.5f).SetEase(Ease.InBack);
        }

        ShopPanelObject = Object;
        ShopPanelObject.transform.DOKill();
        ShopPanelObject.transform.DOLocalMove(shopOpenPos, 0.5f).SetEase(Ease.OutBack);
    }
    public void ClosePanel()
    {
        ShopPanelObject.transform.DOKill();
        ShopPanelObject.transform.DOLocalMove(shopClosePos, 0.5f).SetEase(Ease.InBack);
    }

    void SettingBreadShop(Breads BreadList)
    {
        int breadCount = 0;
        foreach (BreadStat Bread in BreadList.Stats)
        {
            GameObject breadPanelObject = Instantiate(BreadPrefab, transform.position, transform.rotation, BreadContent.transform);
            breadPanelObject.transform.GetComponent<BreadScript>().BreadSetting(BreadScriptable,breadCount);
            breadCount++;
        }
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
        qusetButtons[openingQusetPanel].transform.position -= new Vector3(0, 5, 0);//이전 선택창 내리기
        qusetButtons[Type].transform.position += new Vector3(0, 5, 0);//선택창 올리기
        qusetButtons[openingQusetPanel].image.sprite = qusetButtonsSprite[openingQusetPanel +3];
        qusetButtons[Type].image.sprite = qusetButtonsSprite[Type];
        openingQusetPanel = Type;
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
        StartPanel.transform.DOLocalMove(startOpenPos,0.5f).SetEase(Ease.OutBack);
    }
    public void CloseStartPanel()
    {
        StartPanel.transform.DOLocalMove(startClosePos, 0.5f).SetEase(Ease.InBack);
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
        GameManager.Instance.selectBread = SelectBread;
        SceneManager.LoadScene(0);
    }
}
