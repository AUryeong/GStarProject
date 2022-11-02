using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BreadScript : MonoBehaviour
{
    protected Breads.Type type;
    [SerializeField] BreadStats scriptable;

    [Header("Main")]
    [SerializeField] TextMeshProUGUI mainName;
    [SerializeField] Button detailButton;
    [SerializeField] Image mainImage;
    [SerializeField] GameObject[] mainRanks;

    [Space(10f)]
    [SerializeField] TextMeshProUGUI mainLv;
    [SerializeField] Slider mainExp;
    [SerializeField] TextMeshProUGUI mainHp;
    [SerializeField] TextMeshProUGUI mainAbility;

    [Space(10f)]
    [SerializeField] Button mainSelectButton;
    [SerializeField] TextMeshProUGUI mainSelectText;

    public void BreadSetting(Breads breadscriptable, Breads.Type breadCount)
    {
        type = breadCount;
        scriptable = breadscriptable.Stats[(int)breadCount];

        detailButton.onClick.RemoveAllListeners();
        detailButton.onClick.AddListener(() => DetailPanel.instance.OpenPanel(scriptable));

        mainName.text = scriptable.Name;
        mainImage.sprite = scriptable.ImageSprite;

        mainSelectButton.onClick.RemoveAllListeners();
        mainSelectButton.onClick.AddListener(() => SelectButton());

        LobbyUIManager.Instance.breadSelectText.Add(mainSelectText);
        if (!scriptable.isBuy)//�������� �ʾ����� ����
            mainSelectText.text = "" + scriptable.Price + "";
        else if (LobbyUIManager.Instance.selectBread == type)//���� �� ���õ� ���϶�
            mainSelectText.text = "���õ�";
        else//���� �� ���õ��� �ʾ�����
            mainSelectText.text = "�����ϱ�";

        mainLv.text = $"{scriptable.LV}.LV";
        mainExp.value = scriptable.LV / 6;

        mainHp.text = $"{scriptable.HP}";

        for (int Rank = 0; Rank < scriptable.Rank; Rank++)
            mainRanks[Rank].gameObject.SetActive(true);
    }
    void SelectButton()
    {
        if (scriptable.isBuy)
        {
            ChangeBread();
        }
        else
        {
            if (GameManager.Instance.gold >= scriptable.Price)
            {
                GameManager.Instance.gold -= scriptable.Price;
                scriptable.isBuy = true;
                ChangeBread();

                mainLv.text = $"{++scriptable.LV}.LV";
                mainExp.value = scriptable.LV / 6;
            }
            else
            {
                LobbyUIManager.Instance.MoneyLess();
            }
        }
    }
    void ChangeBread()
    {
        //���õ� �� �г� �ؽ�Ʈ�� �����ϱ�� ����
        LobbyUIManager.Instance.breadSelectText
            [(int)LobbyUIManager.Instance.selectBread].text = "�����ϱ�";

        //�ؽ�Ʈ�� ���õ����� ����
        mainSelectText.text = "���õ�";
        LobbyUIManager.Instance.selectBread = type;
    }
}
