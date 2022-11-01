using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QusetScript : MonoBehaviour
{
    private Quset qusetContents;
    [SerializeField] Image qusetImage;//����Ʈ ���� �̹���
    [SerializeField] TextMeshProUGUI qusetrewardText;//����Ʈ ���� �ؽ�Ʈ
    [SerializeField] TextMeshProUGUI qusetText; //����Ʈ ���� �ؽ�Ʈ
    [SerializeField] Button qusetButton; //����Ʈ ��ư
    [SerializeField] Sprite[] qusetButtonImages;//��ư �̹���
    [SerializeField] Slider qusetSliders;//����Ʈ ���൵ �����̴�
    [SerializeField] GameObject fadePanel;//����Ʈ ���൵ �����̴�
    private int qusetIdx;
    // Start is called before the first frame update
    private void Start()
    {

    }
    private void Update()
    {
        if (qusetContents.isClear == false)
        {
            qusetSliders.value = qusetContents.questSituation / qusetContents.qusetCondition;
            if (qusetContents.qusetCondition <= qusetContents.questSituation)
            {
                qusetButton.image.sprite = qusetButtonImages[1];
                qusetButton.interactable = true;
            }
            else
            {
                qusetButton.image.sprite = qusetButtonImages[0];
                qusetButton.interactable = false;
            }
        }
    }
    public void SettingQuset(QusetScriptable scriptable, int Idx)
    {
        qusetContents = scriptable.QusetList[Idx];
        qusetIdx = Idx;
        qusetImage.sprite = QusetManager.Instance.rewardSprite[(int)qusetContents.rewardType];
        qusetrewardText.text = "" + qusetContents.rewards;
        qusetButton.onClick.AddListener(QusetClear);
        qusetButton.interactable = false;
        qusetButton.image.sprite = qusetButtonImages[0];
        if (qusetContents.qusetType == QusetType.Main)
            qusetText.text = $"{qusetContents.text[0]}{qusetContents.qusetCondition}{qusetContents.text[1]}";
        else
            qusetText.text = qusetContents.text[0];


        if (qusetContents.isClear)
        {
            if (qusetContents.qusetType == QusetType.Main)
            {
                qusetButton.image.sprite = qusetButtonImages[0];
                qusetContents.isClear = false;
                qusetContents.qusetCondition = qusetContents.rewards + qusetContents.M_UpCondition * qusetContents.M_ClearCount;
                qusetText.text = $"{qusetContents.text[0]} {qusetContents.qusetCondition} {qusetContents.text[1]}";
            }
            else
            {
                qusetButton.gameObject.SetActive(false);
                fadePanel.SetActive(true);
                transform.SetAsLastSibling();
            }
        }
    }
    public void QusetClear()
    {
        qusetContents.isClear = true;
        //�����϶��� �޼��� �ٷ� �ʱ�ȭ
        if (qusetContents.qusetType == QusetType.Main)
        {
            qusetButton.image.sprite = qusetButtonImages[0];
            qusetContents.isClear = false;
            qusetContents.qusetCondition = qusetContents.rewards + qusetContents.M_UpCondition * qusetContents.M_ClearCount;
            qusetText.text = $"{qusetContents.text[0]} {qusetContents.qusetCondition} {qusetContents.text[1]}";
        }
        else
        {
            qusetButton.gameObject.SetActive(false);
            fadePanel.SetActive(true);
            transform.SetAsLastSibling();
        }

        //QusetUpdate - Ŭ���� Ƚ�� �߰�
        switch (qusetContents.qusetType)
        {
            case QusetType.Day:
                {
                    QusetManager.Instance.QusetUpdate(qusetContents.qusetType, 5, 1);//5 - ���� ����Ʈ Ŭ���� ����
                    break;
                }
            case QusetType.Aweek:
                {
                    QusetManager.Instance.QusetUpdate(qusetContents.qusetType, 7, 1);//7 - �ְ� ����Ʈ Ŭ���� ����
                    break;
                }
        }

        switch (qusetContents.rewardType)
        {
            case RewardType.Gold:
                {
                    GameManager.Instance.gold += qusetContents.rewards;
                    return;
                }
            case RewardType.Heart:
                {
                    GameManager.Instance.heart += qusetContents.rewards;
                    return;
                }
            case RewardType.Macaron:
                {
                    GameManager.Instance.macaron += qusetContents.rewards;
                    return;
                }
        }

    }
}
