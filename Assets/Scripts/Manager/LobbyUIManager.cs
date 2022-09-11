using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] Ingredients BreadScriptable;
    [SerializeField] List<GameObject> BreadPanel = new List<GameObject>();//빵 정보창 오브젝트


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
        BackGroundObjcet.SetActive(true);
        OpenObject.SetActive(true);
    }

    void SettingBreadShop(Ingredients BreadList)
    {
        int BreadCount = 0;
        foreach (stats Bread in BreadList.Stats)
        {
            GameObject BreadPanelObject = BreadPanel[BreadCount];
            BreadPanelObject.transform.Find("BreadName").GetComponent<Text>().text = Bread.Name;
            BreadPanelObject.transform.Find("LV").GetChild(0).GetComponent<Text>().text = "" + Bread.LV;
            BreadPanelObject.transform.Find("LV").GetChild(1).GetComponent<Slider>().value = Bread.EXP / Bread.MaxEXP;
            BreadPanelObject.transform.Find("BreadImage").GetComponent<Image>().sprite = Bread.ImageSprite;
            BreadPanelObject.transform.Find("BreadInspector").GetChild(0).GetChild(0).GetComponent<Text>().text = "" + Bread.HP;
            BreadPanelObject.transform.Find("BreadInspector").GetChild(0).GetChild(1).GetComponent<Image>().sprite = Bread.ImageSprite;
            BreadPanelObject.transform.Find("BreadInspector").GetChild(1).GetComponent<Button>().onClick.AddListener(() => Gold -= Bread.Price);

            for (int Rank = 0; Rank < Bread.Rank; Rank++)
                BreadPanelObject.transform.Find("BreadRankGroup").transform.GetChild(Rank).gameObject.SetActive(true);

            BreadCount++;
        }
        Bread.SetActive(false);
    }
}
