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
    [Header("Shop Object")]
    [SerializeField] GameObject ShopBgPanel;
    [SerializeField] GameObject Bread;
    [SerializeField] GameObject Quset;
    [SerializeField] GameObject Map;
    [SerializeField] Vector3 shopOpenPos;
    [SerializeField] Vector3 shopClosePos;
    private GameObject ShopPanelObject;

    [Header("Top UI")]
    [Space(10f)]
    [SerializeField] Text GoldText;
    [SerializeField] Text MacaronText;
    [SerializeField] Text StaminaText;
    [SerializeField] Image StaminaImage;

    [Header("Mid UI")]
    [Space(10f)]
    [SerializeField] Animator breadAnim;

    [Header("Bottom UI")]
    [Header("Shop UI")]
    [SerializeField] GameObject shopUIGroup;
    [SerializeField] Vector3 shopUIOpenPos;
    [SerializeField] Vector3 shopUIClosePos;
    [Header("Bread")]
    [Space(10f)]
    [SerializeField] Breads breadScriptable;
    [SerializeField] GameObject breadPrefab;
    [SerializeField] GameObject breadContent;
    //[SerializeField] List<GameObject> BreadPanels = new List<GameObject>();//�� ����â ������Ʈ
    public Breads.Type selectBread;

    [Header("Quset")]
    public QusetScriptable[] qusetScriptables;
    public ScrollRect qusetScroll;
    public RectTransform[] qusetPanel;
    public Button[] qusetButtons;
    public Sprite[] qusetButtonsSprite;
    public GameObject qusetPrefab;
    private int openingQusetPanel = 0;
    [Header("Map")]
    [SerializeField] List<GameObject> mapPanel = new List<GameObject>();
    [SerializeField] bool[] mapLock;
    [Header("Start")]
    [SerializeField] GameObject startPanel;//���� ȭ��
    [SerializeField] GameObject reCheckPanel;//���� ȭ�鿡�� ������ ������ ������ �̹���
    [SerializeField] Image abilityImage_Main;//Ŭ�� ������ ������ �̹���
    [SerializeField] Text abilityNameAndLV;//��ų �̸��� �����ؽ�Ʈ
    [SerializeField] Text upgradeMoney;//���� �ؽ�Ʈ
    [SerializeField] Text abilityExplanation;//��ų ���� �ؽ�Ʈ
    [SerializeField] Sprite[] abilitySprite = new Sprite[2];//��ų �̹���
    [SerializeField] Vector3 startOpenPos;
    [SerializeField] Vector3 startClosePos;
    private bool reCheck;
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        SettingBreadShop(breadScriptable);
        SettingQusetPanel();

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
            GameObject breadPanelObject = Instantiate(breadPrefab, transform.position, transform.rotation, breadContent.transform);
            breadPanelObject.transform.GetComponent<BreadScript>().BreadSetting(breadScriptable,(Breads.Type)breadCount);
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

    public void SettingButton()
    {
        GameManager.Instance.OnOffSetting();
    }
    public void AbilitySelect(int idx)
    {
        SelectAbility = idx;
        switch (SelectAbility)
        {
            case 0:
                {

                    abilityImage_Main.sprite = abilitySprite[SelectAbility];
                    abilityNameAndLV.text = $"�ִ� ü�� LV.{GameManager.Instance.MaxHpLv}";
                    upgradeMoney.text = $"{5000 + (GameManager.Instance.MaxHpLv - 1) * 500} Gold";
                    abilityExplanation.text = $"�߰� ü���� �� {GameManager.Instance.MaxHpLv * 5} �þ�ϴ�.ü���� �������� ������ �� �̻� ��Ḧ ������ ���ϴ� ���������� ��ȭ�սô�.";
                    break;
                }
            case 1:
                {
                    abilityImage_Main.sprite = abilitySprite[SelectAbility];
                    abilityNameAndLV.text = $"�浹 ������ ���� LV.{GameManager.Instance.DefenseLv}";
                    upgradeMoney.text = $"{5000 + (GameManager.Instance.DefenseLv - 1) * 500} Gold";
                    abilityExplanation.text = $"��ֹ� �浹 �� �������� {GameManager.Instance.DefenseLv * 5} %��ŭ ���ҵ˴ϴ�. �������� �Ͼ�� �𸣴� ���������� ��ȭ�صӽô�.";
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
                    abilityImage_Main.sprite = abilitySprite[SelectAbility];
                    upgradeMoney.text = $"{5000 + (++GameManager.Instance.MaxHpLv - 1) * 500} Gold";
                    abilityNameAndLV.text = $"�ִ� ü�� LV.{GameManager.Instance.MaxHpLv}";
                    abilityExplanation.text = $"�߰� ü���� �� {GameManager.Instance.MaxHpLv * 5} �þ�ϴ�.ü���� �������� ������ �� �̻� ��Ḧ ������ ���ϴ� ���������� ��ȭ�սô�.";
                    break;
                }
            case 1:
                {
                    abilityImage_Main.sprite = abilitySprite[SelectAbility];
                    upgradeMoney.text = $"{5000 + (++GameManager.Instance.DefenseLv - 1) * 500} Gold";
                    abilityNameAndLV.text = $"�浹 ������ ���� LV.{GameManager.Instance.DefenseLv}";
                    abilityExplanation.text = $"��ֹ� �浹 �� �������� {GameManager.Instance.DefenseLv * 5} %��ŭ ���ҵ˴ϴ�. �������� �Ͼ�� �𸣴� ���������� ��ȭ�صӽô�.";
                    break;
                }
        }
    }


    public void OpenStartPanel()
    {
        if(reCheck == false)
        {
            startPanel.transform.DOLocalMove(startOpenPos, 0.5f).SetEase(Ease.OutBack);
            shopUIGroup.transform.DOLocalMove(shopUIClosePos, 0.5f).SetEase(Ease.OutQuad);
            reCheck = true;
        }
        else
        {
            reCheckPanel.SetActive(true);
        }
    }
    public void CloseStartPanel()
    {
        startPanel.transform.DOLocalMove(startClosePos, 0.5f).SetEase(Ease.InBack);
        shopUIGroup.transform.DOLocalMove(shopUIOpenPos, 0.5f).SetEase(Ease.InQuad);
        reCheckPanel.SetActive(false);
        reCheck = false;
    }
    public void StartYesNoButton(bool _bool)
    {
        if(_bool)
        {
            GameManager.Instance.selectBread = selectBread;
            SceneManager.LoadScene("InGame");
        }
        else
        {
            reCheck = false;
            reCheckPanel.SetActive(false);
        }
    }
    public void ChangeBread()
    {

    }
}
