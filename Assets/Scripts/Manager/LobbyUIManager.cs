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
    int Gold = 10000;//테스팅용 추후에 게임매니저로 이동
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
    [Header("Quset")]
    [SerializeField] List<GameObject> QusetPanels = new List<GameObject>();
    private List<Image> QusetImage = new List<Image>();//퀘스트 보상 이미지
    private List<Text> Qusetrewards = new List<Text>();//퀘스트 보상 텍스트
    private List<Text> QusetText = new List<Text>();//퀘스트 내용 텍스트
    private List<Slider> QusetSliders = new List<Slider>();//퀘스트 진행도 슬라이더
    [Header("Map")]
    [SerializeField] List<GameObject> MapPanels = new List<GameObject>();
    private List<Text> MapName = new List<Text>();//맵 이름
    private List<Text> Mapdescription = new List<Text>();//맵 설명 텍스트
    private List<GameObject> LockPanels = new List<GameObject>();

    [Header("Start")]
    [SerializeField] GameObject StartPanel;
    [SerializeField] Image AbilityImage_1;// 능력 선택 버튼 이미지
    [SerializeField] Image AbilityImage_2;//능력 선택 버튼 이미지
    [SerializeField] Image AbilityImage_Main;//클릭 했을떄 나오는 이미지
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
        //UpgradeMoney.text = 기획 나오면
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
        //UpgradeMoney.text = 기획 나오면
        AbilityExplanation.text = (string)GetType().GetField($"Bread.AbilityText_{idx}").GetValue(this); 
    }
   
    
}
