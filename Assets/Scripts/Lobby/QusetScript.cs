using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QusetScript : MonoBehaviour
{
    private Quest qusetContents;
    [SerializeField] Image qusetImage;//퀘스트 보상 이미지
    [SerializeField] TextMeshProUGUI qusetrewardText;//퀘스트 보상 텍스트
    [SerializeField] TextMeshProUGUI qusetText; //퀘스트 내용 텍스트
    [SerializeField] Button qusetButton; //퀘스트 버튼
    [SerializeField] Sprite[] qusetButtonImages;//버튼 이미지
    [SerializeField] Slider qusetSliders;//퀘스트 진행도 슬라이더
    [SerializeField] GameObject fadePanel;//퀘스트 진행도 슬라이더
    // Start is called before the first frame update
    private void Start()
    {

    }
    private void Update()
    {
        if (qusetContents.isClear == false)
        {
            qusetSliders.value = qusetContents.questSituation / qusetContents.questCondition;
            if (qusetContents.questCondition <= qusetContents.questSituation)
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
    public void SettingQuset(QuestScriptable scriptable, int Idx)
    {
        qusetContents = scriptable.QusetList[Idx];
        qusetImage.sprite = QusetManager.Instance.rewardSprite[(int)qusetContents.rewardType];
        qusetrewardText.text = "" + qusetContents.rewards;
        qusetButton.onClick.AddListener(QusetClear);
        qusetButton.interactable = false;
        qusetButton.image.sprite = qusetButtonImages[0];
        if (qusetContents.questType == QuestType.Main)
            qusetText.text = $"{qusetContents.text[0]}{qusetContents.questCondition}{qusetContents.text[1]}";
        else
            qusetText.text = qusetContents.text[0];
    }
    public void QusetClear()
    {
        qusetContents.isClear = true;
        //메인일때는 달성시 바로 초기화
        if (qusetContents.questType == QuestType.Main)
        {
            qusetButton.image.sprite = qusetButtonImages[0];
            qusetContents.isClear = false;
            qusetContents.questCondition = qusetContents.rewards + qusetContents.M_UpCondition * qusetContents.M_ClearCount;
            qusetText.text = $"{qusetContents.text[0]} {qusetContents.questCondition} {qusetContents.text[1]}";
        }
        else
        {
            qusetButton.gameObject.SetActive(false);
            fadePanel.SetActive(true);
            transform.SetAsLastSibling();
        }

        //QusetUpdate - 클리어 횟수 추가
        switch (qusetContents.questType)
        {
            case QuestType.Day:
                {
                    QusetManager.Instance.QusetUpdate(qusetContents.questType, 5, 1);//5 - 일일 퀘스트 클리어 갯수
                    break;
                }
            case QuestType.Aweek:
                {
                    QusetManager.Instance.QusetUpdate(qusetContents.questType, 7, 1);//7 - 주간 퀘스트 클리어 갯수
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
