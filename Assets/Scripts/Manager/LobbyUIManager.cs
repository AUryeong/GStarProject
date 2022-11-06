using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [SerializeField] TextMeshProUGUI moneyLessText;
    [SerializeField] Vector3 shopOpenPos;
    [SerializeField] Vector3 shopClosePos;

    public List<TextMeshProUGUI> breadSelectText = new List<TextMeshProUGUI>();//빵 선택 텍스트들
    private GameObject ShopPanelObject;

    [Header("Top UI")]
    [Space(10f)]
    [SerializeField] TextMeshProUGUI goldText;
    [SerializeField] TextMeshProUGUI macaronText;
    [Header("Stamina")]
    [SerializeField] GameObject[] heartGroup;
    [SerializeField] TextMeshProUGUI staminaText;

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

    public ScrollRect qusetScroll;
    public RectTransform[] qusetPanel;
    public Button[] qusetButtons;
    public Sprite[] qusetButtonsSprite;
    public GameObject qusetPrefab;
    private int openingQusetPanel = 0;
    [Header("Map")]
    public List<GameObject> mapLockPanel = new List<GameObject>();
    public MapEX SelectMap;
    [Header("Start")]
    [SerializeField] GameObject startPanel;//���� ȭ��
    [SerializeField] GameObject reCheckPanel;//���� ȭ�鿡�� ������ ������ ������ �̹���

    [SerializeField] Image abilityImage_Main;//Ŭ�� ������ ������ �̹���

    [SerializeField] TextMeshProUGUI abilityNameAndLV;//��ų �̸��� �����ؽ�Ʈ
    [SerializeField] TextMeshProUGUI upgradeMoney;//���� �ؽ�Ʈ
    [SerializeField] TextMeshProUGUI abilityExplanation;//��ų ���� �ؽ�Ʈ

    [SerializeField] Sprite[] abilitySprite = new Sprite[2];//��ų �̹���

    [SerializeField] Vector3 startOpenPos;
    [SerializeField] Vector3 startClosePos;
    private bool reCheck;
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        selectBread = GameManager.Instance.selectBread;
        SettingBreadShop(breadScriptable);
        SettingQusetPanel();
        StaminaUpdate();
        AbilitySelect(0);
    }
    private void Start()
    {
    }
    public void MoneyLess()
    {
        moneyLessText.gameObject.SetActive(true);
        moneyLessText.color = new Color(moneyLessText.color.r, moneyLessText.color.g, moneyLessText.color.b, 0);
        moneyLessText.DOFade(1, 0.3f).OnComplete(() =>
        {
            moneyLessText.DOFade(0, 1).SetDelay(2).OnComplete(() =>
            {
                moneyLessText.gameObject.SetActive(false);
            });
        });
    }
    void Update()
    {
        goldText.text = $"{GameManager.Instance.gold}";
        macaronText.text = $"{GameManager.Instance.macaron}";
    }
    public void StaminaUpdate()
    {
        int count = GameManager.Instance.heart;
        //                   MaxMarkStamina
        staminaText.text = $"+{count - 7}";
        for (int idx = 0; idx < 8; idx++)
        {
            if (idx < count)
            {
                heartGroup[idx].SetActive(true);
            }
            else
                heartGroup[idx].SetActive(false);
        }
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
        foreach (BreadStats Bread in BreadList.Stats)
        {
            GameObject breadPanelObject = Instantiate(breadPrefab, transform.position, transform.rotation, breadContent.transform);
            breadPanelObject.transform.GetComponent<BreadScript>().BreadSetting(breadScriptable, (Breads.Type)breadCount);
            breadCount++;
        }
    }
    void SettingQusetPanel()
    {
        for (int i = 0; i < 3; i++)//0 : ���� / 1 : �ְ� / 2 : ����
        {
            List<QusetScript> scripts = new List<QusetScript>();
            for (int j = 0; j < QusetManager.Instance.qusetScriptables[i].QusetList.Count; j++)
            {
                scripts.Add(Instantiate(qusetPrefab, transform.position, transform.rotation, qusetPanel[i].transform)
                    .GetComponent<QusetScript>());
            }
            for (int j = 0; j < QusetManager.Instance.qusetScriptables[i].QusetList.Count; j++)
            {
                scripts[j].SettingQuset(QusetManager.Instance.qusetScriptables[i], j);
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
        qusetButtons[openingQusetPanel].image.sprite = qusetButtonsSprite[openingQusetPanel + 3];
        qusetButtons[Type].image.sprite = qusetButtonsSprite[Type];
        openingQusetPanel = Type;
    }

    public void SettingButton()
    {
        GameManager.Instance.OnOffSetting();
    }

    protected void UpdateAbility()
    {
        switch (SelectAbility)
        {
            case 0:
                {

                    abilityImage_Main.sprite = abilitySprite[SelectAbility];
                    abilityNameAndLV.text = $"�ִ� ü�� LV.{GameManager.Instance.maxHpLv}";
                    upgradeMoney.text = $"{5000 + (GameManager.Instance.maxHpLv - 1) * 500} Gold";
                    abilityExplanation.text = $"�߰� ü���� �� {GameManager.Instance.maxHpLv * 5} �þ�ϴ�.\n\nü���� �������� ������ �� �̻� ��Ḧ ������ ���ϴ� ���������� ��ȭ�սô�.";
                    break;
                }
            case 1:
                {
                    abilityImage_Main.sprite = abilitySprite[SelectAbility];
                    abilityNameAndLV.text = $"�浹 ������ ���� LV.{GameManager.Instance.defenseLv}";
                    upgradeMoney.text = $"{5000 + (GameManager.Instance.defenseLv - 1) * 500} Gold";
                    abilityExplanation.text = $"��ֹ� �浹 �� �������� {GameManager.Instance.defenseLv * 5} %��ŭ ���ҵ˴ϴ�.\n\n�������� �Ͼ�� �𸣴� ���������� ��ȭ�صӽô�.";
                    break;
                }
        }
    }
    public void AbilitySelect(int idx)
    {
        SelectAbility = idx;
        UpdateAbility();
    }
    public void UpgradeAbility()
    {
        switch (SelectAbility)
        {
            case 0:
                {
                    GameManager.Instance.maxHpLv++;
                    break;
                }
            case 1:
                {
                    GameManager.Instance.defenseLv++;
                    break;
                }

        }
        UpdateAbility();
    }


    public void OpenStartPanel()
    {
        if (reCheck == false)
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
        if (_bool)
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
    public void ChangeMap(MapEX SelectMap)
    {
        if(this.SelectMap != null) this.SelectMap.Select = false;//원래 선택 맵은 버튼 올리기

        this.SelectMap = SelectMap;
        this.SelectMap.Select = true;//바뀔 맵 버튼 눌리기
    }
}
