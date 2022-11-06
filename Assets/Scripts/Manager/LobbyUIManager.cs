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
    [SerializeField] GameObject startPanel;//정비 화면
    [SerializeField] GameObject reCheckPanel;//정비 화면에서 시작을 누를떄 나오는 이미지

    [SerializeField] Image abilityImage_Main;//클릭 했을떄 나오는 이미지

    [SerializeField] TextMeshProUGUI abilityNameAndLV;//스킬 이름과 레벨텍스트
    [SerializeField] TextMeshProUGUI upgradeMoney;//가격 텍스트

    [SerializeField] TextMeshProUGUI abilityExplanation;//스킬 설명 텍스트
    [SerializeField] Sprite[] abilitySprite = new Sprite[2];//스킬 이미지

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
        for (int i = 0; i < 3; i++)//0 : 일일 / 1 : 주간 / 2 : 메인
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
        qusetScroll.content = qusetPanel[Type];//콘텐츠 변경
        qusetPanel[openingQusetPanel].gameObject.SetActive(false);//이전 퀘스트창 비활성화
        qusetPanel[Type].gameObject.SetActive(true);//열려는 퀘스트창 활성화
        qusetButtons[openingQusetPanel].transform.position -= new Vector3(0, 5, 0);//이전 선택창 내리기
        qusetButtons[Type].transform.position += new Vector3(0, 5, 0);//선택창 올리기
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
                    abilityNameAndLV.text = $"최대 체력 LV.{GameManager.Instance.maxHpLv}";
                    upgradeMoney.text = $"{5000 + (GameManager.Instance.maxHpLv - 1) * 500} Gold";
                    abilityExplanation.text = $"추가 체력이 총 {GameManager.Instance.maxHpLv * 5} 늘어납니다.체력이 떨어지면 빵들은 더 이상 재료를 모으지 못하니 지속적으로 강화합시다.";
                    break;
                }
            case 1:
                {
                    abilityImage_Main.sprite = abilitySprite[SelectAbility];
                    abilityNameAndLV.text = $"충돌 데미지 감소 LV.{GameManager.Instance.defenseLv}";
                    upgradeMoney.text = $"{5000 + (GameManager.Instance.defenseLv - 1) * 500} Gold";
                    abilityExplanation.text = $"장애물 충돌 시 데미지를 {GameManager.Instance.defenseLv * 5} %만큼 감소됩니다. 무슨일이 일어날지 모르니 만일을위해 강화해둡시다.";
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
