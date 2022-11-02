using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QusetScript : MonoBehaviour
{
    private Quest questContents;
    [SerializeField] Image questImage;//����Ʈ ���� �̹���
    [SerializeField] TextMeshProUGUI questrewardText;//����Ʈ ���� �ؽ�Ʈ
    [SerializeField] TextMeshProUGUI questText; //����Ʈ ���� �ؽ�Ʈ
    [SerializeField] Button questButton; //����Ʈ ��ư
    [SerializeField] Sprite[] questButtonImages;//��ư �̹���
    [SerializeField] Slider questSliders;//����Ʈ ���൵ �����̴�
    [SerializeField] GameObject fadePanel;//����Ʈ ���൵ �����̴�
    // Start is called before the first frame update
    private void Start()
    {

    }
    private void Update()
    {
        if (questContents.isClear == false)
        {
            questSliders.value = questContents.questSituation / questContents.questCondition;
            if (questContents.questCondition <= questContents.questSituation)
            {
                questButton.image.sprite = questButtonImages[1];
                questButton.interactable = true;
            }
            else
            {
                questButton.image.sprite = questButtonImages[0];
                questButton.interactable = false;
            }
        }
    }

    public void SettingQuset(QuestScriptable scriptable, int Idx)
    {
        questContents = scriptable.QusetList[Idx];

        questImage.sprite = QusetManager.Instance.rewardSprite[(int)questContents.rewardType];
        questrewardText.text = "" + questContents.rewards;
        questButton.onClick.AddListener(QusetClear);
        questButton.interactable = false;
        questButton.image.sprite = questButtonImages[0];

        if (questContents.questType == QuestType.Main)
            questText.text = $"{questContents.text[0]}{questContents.questCondition}{questContents.text[1]}";
        else
            questText.text = questContents.text[0];


        if (questContents.isClear)
        {
            if (questContents.questType == QuestType.Main)
            {
                questButton.image.sprite = questButtonImages[0];
                questContents.isClear = false;
                questContents.questCondition = questContents.rewards + questContents.M_UpCondition * questContents.M_ClearCount;
                questText.text = $"{questContents.text[0]} {questContents.questCondition} {questContents.text[1]}";
            }
            else
            {
                questButton.gameObject.SetActive(false);
                fadePanel.SetActive(true);
                transform.SetAsLastSibling();
            }
        }
    }
    public void QusetClear()
    {
        questContents.isClear = true;
        //�����϶��� �޼��� �ٷ� �ʱ�ȭ
        if (questContents.questType == QuestType.Main)
        {
            questButton.image.sprite = questButtonImages[0];
            questContents.isClear = false;
            questContents.questCondition = questContents.rewards + questContents.M_UpCondition * questContents.M_ClearCount;
            questText.text = $"{questContents.text[0]} {questContents.questCondition} {questContents.text[1]}";
        }
        else
        {
            questButton.gameObject.SetActive(false);
            fadePanel.SetActive(true);
            transform.SetAsLastSibling();
        }

        //QusetUpdate - Ŭ���� Ƚ�� �߰�
        switch (questContents.questType)
        {
            case QuestType.Day:
                {
                    QusetManager.Instance.QusetUpdate(questContents.questType, 5, 1);//5 - ���� ����Ʈ Ŭ���� ����
                    break;
                }
            case QuestType.Aweek:
                {
                    QusetManager.Instance.QusetUpdate(questContents.questType, 7, 1);//7 - �ְ� ����Ʈ Ŭ���� ����
                    break;
                }
        }

        switch (questContents.rewardType)
        {
            case RewardType.Gold:
                {
                    GameManager.Instance.gold += questContents.rewards;
                    return;
                }
            case RewardType.Heart:
                {
                    GameManager.Instance.heart += questContents.rewards;
                    return;
                }
            case RewardType.Macaron:
                {
                    GameManager.Instance.macaron += questContents.rewards;
                    return;
                }
        }

    }
}
