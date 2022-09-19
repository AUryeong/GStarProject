using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
class BreadInspector{
    Text LVText;
    Slider Text;
}
public class LobbyUIManager : MonoBehaviour
{
    int Gold = 10000;//�׽��ÿ� ���Ŀ� ���ӸŴ����� �̵�
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
    [SerializeField] List<GameObject> BreadPanels = new List<GameObject>();//�� ����â ������Ʈ
    private List<Text> LVTexts = new List<Text>();//���� �ؽ�Ʈ ����Ʈ
    private List<Slider> EXPSliders = new List<Slider>();//����ġ�� ����Ʈ
    private List<Text> HPTexts = new List<Text>();//ü�� �ؽ�Ʈ ����Ʈ
    private List<Text> BuyButtonText = new List<Text>();//������ ��ư �ؽ�Ʈ
    private int SelectBread;
    [Header("Quset")]
    [SerializeField] List<GameObject> QusetPanels = new List<GameObject>();
    private List<Image> QusetImage = new List<Image>();//����Ʈ ���� �̹���
    private List<Text> Qusetrewards = new List<Text>();//����Ʈ ���� �ؽ�Ʈ
    private List<Text> QusetText = new List<Text>();//����Ʈ ���� �ؽ�Ʈ
    private List<Slider> QusetSliders = new List<Slider>();//����Ʈ ���൵ �����̴�
    [Header("Map")]
    [SerializeField] List<GameObject> MapPanels = new List<GameObject>();
    private List<Text> MapName = new List<Text>();//�� �̸�
    private List<Text> Mapdescription = new List<Text>();//�� ���� �ؽ�Ʈ
    private List<GameObject> LockPanels = new List<GameObject>();

    [Header("Start")]
    [SerializeField] GameObject StartPanel;
    [SerializeField] Image AbilityImage_1;// �ɷ� ���� ��ư �̹���
    [SerializeField] Image AbilityImage_2;//�ɷ� ���� ��ư �̹���
    [SerializeField] Image AbilityImage_Main;//Ŭ�� ������ ������ �̹���
    [SerializeField] Text AbilityNameAndLV;
    [SerializeField] Text UpgradeMoney;
    [SerializeField] Text AbilityExplanation;
    // Start is called before the first frame update
    void Start()
    {
        SettingBreadShop(BreadScriptable);
        Quset.SetActive(false);
        Map.SetActive(false);
        BackGroundObjcet.SetActive(false);
    }
    void Update()
    {

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
            GameObject BreadPanelObject = BreadPanels[BreadCount];
            BreadPanelObject.transform.Find("BreadName").GetComponent<Text>().text = Bread.Name;//BreadName Text
            BreadPanelObject.transform.Find("BreadImage").GetComponent<Image>().sprite = Bread.ImageSprite;//BreadImage
            BreadPanelObject.transform.Find("BreadInspector").GetChild(0).GetChild(1).GetComponent<Image>().sprite = Bread.ImageSprite;//BreadAction
            BreadPanelObject.transform.Find("BreadInspector").GetChild(1).GetComponent<Button>().onClick.AddListener(() => BuyButton(BreadCount));//price

            LVTexts.Add(BreadPanelObject.transform.Find("LV").GetChild(0).GetComponent<Text>());
            EXPSliders.Add(BreadPanelObject.transform.Find("LV").GetChild(1).GetComponent<Slider>());
            HPTexts.Add(BreadPanelObject.transform.Find("BreadInspector").GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>());
            LVTexts[BreadCount].text = "" + Bread.LV;//LvText
            EXPSliders[BreadCount].value = Bread.EXP / Bread.MaxEXP;//ExpSlider
            HPTexts[BreadCount].text = "" + Bread.HP;//BreadHP

            for (int Rank = 0; Rank < Bread.Rank; Rank++)
                BreadPanelObject.transform.Find("BreadRankGroup").transform.GetChild(Rank).gameObject.SetActive(true);

            BreadCount++;
        }
        Bread.SetActive(false);
    }
    void BuyButton(int idx)
    {

    }
    public void StartPanelSetting()
    {
        BreadStat Bread = BreadScriptable.Stats[SelectBread];
        AbilityImage_1.sprite = Bread.AbilityImage_1;
        AbilityImage_2.sprite = Bread.AbilityImage_2;
        AbilityImage_Main.sprite = Bread.AbilityImage_1;
        AbilityNameAndLV.text = "" + Bread.Name + "LV." + Bread.AbilityLV_1;
        //UpgradeMoney.text = ��ȹ ������
        AbilityExplanation.text = "" + Bread.AbilityText_1;
        StartPanel.SetActive(true);
    }
    public void StartButton()
    {
        SceneManager.LoadScene(0);
    }
    public void StartPanelExit()
    {
        StartPanel.SetActive(false);
    }
    public void AbilitySelect(int idx)
    {
        Sprite s = (Sprite)GetType().GetField("temp1").GetValue(this);
        BreadStat Bread = BreadScriptable.Stats[SelectBread];
        AbilityImage_Main.sprite = (Sprite)GetType().GetField($"Bread.AbilityImage_{idx}").GetValue(this); 
        AbilityNameAndLV.text = "" + Bread.Name + "LV." + (int)GetType().GetField($"Bread.AbilityLV_{idx}").GetValue(this); 
        //UpgradeMoney.text = ��ȹ ������
        AbilityExplanation.text = (string)GetType().GetField($"Bread.AbilityText_{idx}").GetValue(this); 
    }
   
    
}
