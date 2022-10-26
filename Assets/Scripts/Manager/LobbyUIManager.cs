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
    //[SerializeField] List<GameObject> BreadPanels = new List<GameObject>();//�� ����â ������Ʈ
    private List<Text> LVTexts = new List<Text>();//���� �ؽ�Ʈ ����Ʈ
    private List<Slider> EXPSliders = new List<Slider>();//����ġ�� ����Ʈ
    private List<Text> HPTexts = new List<Text>();//ü�� �ؽ�Ʈ ����Ʈ
    private List<Text> BuyBtuttonText = new List<Text>();//���� ��ư �ؽ�Ʈ
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
    [SerializeField] GameObject StartPanel;//���� ȭ��
    [SerializeField] GameObject ReCheckPanel;//���� ȭ�鿡�� ������ ������ ������ �̹���
    [SerializeField] Image AbilityImage_Main;//Ŭ�� ������ ������ �̹���
    [SerializeField] Text AbilityNameAndLV;//��ų �̸��� �����ؽ�Ʈ
    [SerializeField] Text UpgradeMoney;//���� �ؽ�Ʈ
    [SerializeField] Text AbilityExplanation;//��ų ���� �ؽ�Ʈ
    [SerializeField] Sprite[] AbilitySprite = new Sprite[2];//��ų �̹���
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
        qusetButtons[openingQusetPanel].transform.position -= new Vector3(0, 5, 0);//���� ����â ������
        qusetButtons[Type].transform.position += new Vector3(0, 5, 0);//����â �ø���
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
