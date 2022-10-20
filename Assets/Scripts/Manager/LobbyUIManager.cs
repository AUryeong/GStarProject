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
    //[SerializeField] List<GameObject> BreadPanels = new List<GameObject>();//�� ����â ������Ʈ
    private List<Text> LVTexts = new List<Text>();//���� �ؽ�Ʈ ����Ʈ
    private List<Slider> EXPSliders = new List<Slider>();//����ġ�� ����Ʈ
    private List<Text> HPTexts = new List<Text>();//ü�� �ؽ�Ʈ ����Ʈ
    private List<Text> BuyBtuttonText = new List<Text>();//���� ��ư �ؽ�Ʈ
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
    [SerializeField] GameObject StartPanel;//���� ȭ��
    [SerializeField] GameObject ReCheckPanel;//���� ȭ�鿡�� ������ ������ ������ �̹���
    [SerializeField] Image AbilityImage_Main;//Ŭ�� ������ ������ �̹���
    [SerializeField] Text AbilityNameAndLV;//��ų �̸��� �����ؽ�Ʈ
    [SerializeField] Text UpgradeMoney;//���� �ؽ�Ʈ
    [SerializeField] Text AbilityExplanation;//��ų ���� �ؽ�Ʈ
    [SerializeField] Sprite[] AbilitySprite = new Sprite[2];//��ų �̹���
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
            BuyBtuttonText[BreadCount].text = "�����ϱ�";

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
        for (int i = 0; i < 3; i++)//0 : ���� / 1 : �ְ� / 2 : ����
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
        qusetScroll.content = qusetPanel[Type];//������ ����
        qusetPanel[openingQusetPanel].gameObject.SetActive(false);//���� ����Ʈâ ��Ȱ��ȭ
        qusetPanel[Type].gameObject.SetActive(true);//������ ����Ʈâ Ȱ��ȭ
        qusetButtons[openingQusetPanel].transform.position -= new Vector3(0, 20, 0);//���� ����â ������
        qusetButtons[Type].transform.position += new Vector3(0, 20, 0);//����â �ø���
        Image openingColor = qusetButtons[openingQusetPanel].gameObject.GetComponent<Image>();
        Image openPanelColor = qusetButtons[Type].gameObject.GetComponent<Image>();
        openingColor.color = new Color(0.75f, 0.75f, 0.75f);//��ư ȸ������ ����
        openPanelColor.color = new Color(1, 1, 1);//��ư ������� ����
        openingQusetPanel = Type;
    }
    void BuyButton(in int idx)
    {
        Debug.Log(idx);
        SelectBread = idx;
        GameObject ClickButton = EventSystem.current.currentSelectedGameObject;
        SelectButtonTxt.text = "�����ϱ�";
        SelectButtonTxt = ClickButton.GetComponentInChildren<Text>();
        SelectButtonTxt.text = "���õ�";

    }
    public void AbilitySelect(int idx)
    {
        SelectAbility = idx;
        switch (SelectAbility)
        {
            case 0:
                {

                    AbilityImage_Main.sprite = AbilitySprite[SelectAbility];
                    AbilityNameAndLV.text = $"�ִ� ü�� LV.{GameManager.Instance.MaxHpLv}";
                    UpgradeMoney.text = $"{5000 + (GameManager.Instance.MaxHpLv - 1) * 500} Gold";
                    AbilityExplanation.text = $"�߰� ü���� �� {GameManager.Instance.MaxHpLv * 5} �þ�ϴ�.ü���� �������� ������ �� �̻� ��Ḧ ������ ���ϴ� ���������� ��ȭ�սô�.";
                    break;
                }
            case 1:
                {
                    AbilityImage_Main.sprite = AbilitySprite[SelectAbility];
                    AbilityNameAndLV.text = $"�浹 ������ ���� LV.{GameManager.Instance.DefenseLv}";
                    UpgradeMoney.text = $"{5000 + (GameManager.Instance.DefenseLv - 1) * 500} Gold";
                    AbilityExplanation.text = $"��ֹ� �浹 �� �������� {GameManager.Instance.DefenseLv * 5} %��ŭ ���ҵ˴ϴ�. �������� �Ͼ�� �𸣴� ���������� ��ȭ�صӽô�.";
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
                    AbilityNameAndLV.text = $"�ִ� ü�� LV.{GameManager.Instance.MaxHpLv}";
                    AbilityExplanation.text = $"�߰� ü���� �� {GameManager.Instance.MaxHpLv * 5} �þ�ϴ�.ü���� �������� ������ �� �̻� ��Ḧ ������ ���ϴ� ���������� ��ȭ�սô�.";
                    break;
                }
            case 1:
                {
                    AbilityImage_Main.sprite = AbilitySprite[SelectAbility];
                    UpgradeMoney.text = $"{5000 + (++GameManager.Instance.DefenseLv - 1) * 500} Gold";
                    AbilityNameAndLV.text = $"�浹 ������ ���� LV.{GameManager.Instance.DefenseLv}";
                    AbilityExplanation.text = $"��ֹ� �浹 �� �������� {GameManager.Instance.DefenseLv * 5} %��ŭ ���ҵ˴ϴ�. �������� �Ͼ�� �𸣴� ���������� ��ȭ�صӽô�.";
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
