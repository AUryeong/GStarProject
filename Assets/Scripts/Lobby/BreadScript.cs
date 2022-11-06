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
    [SerializeField] Image mainExp;
    [SerializeField] Sprite[] expSprite;


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
        detailButton.onClick.AddListener(() => DetailPanel.instance.OpenPanel(scriptable, this));

        mainName.text = scriptable.Name;
        mainImage.sprite = scriptable.ImageSprite;

        mainSelectButton.onClick.RemoveAllListeners();
        mainSelectButton.onClick.AddListener(() => SelectButton());

        LobbyUIManager.Instance.breadSelectText.Add(mainSelectText);
        if (!scriptable.isBuy)//�������� �ʾ����� ����
            mainSelectText.text = "" + scriptable.Price + " ���";
        else if (LobbyUIManager.Instance.selectBread == type)//���� �� ���õ� ���϶�
            mainSelectText.text = "���õ�";
        else//���� �� ���õ��� �ʾ�����
            mainSelectText.text = "�����ϱ�";

        mainLv.text = $"{scriptable.LV}.LV";
        mainExp.sprite = expSprite[scriptable.LV];

        mainHp.text = $"{scriptable.GetHp()}";
        mainAbility.text = $"{scriptable.AbilityText_2}";

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
                GameManager.Instance.gold -= (int)scriptable.Price;
                scriptable.isBuy = true;

                Upgrade();

                ChangeBread();
            }
            else
            {
                LobbyUIManager.Instance.MoneyLess();
            }
        }
    }
    public void Upgrade()
    {
        scriptable.LV++;

        mainLv.text = $"{scriptable.LV}.LV";
        mainExp.sprite = expSprite[scriptable.LV];

        if (!scriptable.isBuy)
        {
            mainSelectText.text = "�����ϱ�";
            scriptable.isBuy = true;
        }
        else
            mainHp.text = $"{scriptable.GetHp()}";
    }

    void ChangeBread()
    {
        //���õ� �� �г� �ؽ�Ʈ�� �����ϱ�� ����
        LobbyUIManager.Instance.breadSelectText
            [(int)LobbyUIManager.Instance.selectBread].text = "�����ϱ�";

        //�ؽ�Ʈ�� ���õ����� ����
        mainSelectText.text = "���õ�";
        LobbyUIManager.Instance.selectBread = type;
        LobbyUIManager.Instance.ChangeBread();
    }
}
