using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QusetScript : MonoBehaviour
{
    private Quset qusetContents;
    [SerializeField] Image qusetImage;//퀘스트 보상 이미지
    [SerializeField] Text qusetrewardText;//퀘스트 보상 텍스트
    [SerializeField] Text qusetText; //퀘스트 내용 텍스트
    [SerializeField] Button qusetButton; //퀘스트 버튼
    [SerializeField] Sprite[] qusetButtonImages;//버튼 이미지
    [SerializeField] Slider qusetSliders;//퀘스트 진행도 슬라이더
    // Start is called before the first frame update
    private void Start()
    {

    }
    private void Update()
    {
        if (!qusetContents.isClear)
        {
            qusetSliders.value = qusetContents.questSituation / qusetContents.qusetCondition;
            if (qusetContents.qusetCondition <= qusetContents.questSituation)
            {
                qusetButton.image.sprite = qusetButtonImages[1];
                qusetButton.interactable = true;
                qusetContents.isClear = true;
            }
        }
        else if (qusetContents.qusetCondition > qusetContents.questSituation)
        {
            qusetButton.image.sprite = qusetButtonImages[0];
            qusetButton.interactable = false;
            qusetContents.isClear = false;
        }
    }
    public void SettingQuset(QusetScriptable scriptable, int Idx)
    {
        qusetContents = scriptable.QusetList[Idx];
        qusetImage.sprite = qusetContents.sprite;
        qusetrewardText.text = "" + qusetContents.rewards;
        qusetButton.onClick.AddListener(QusetClear);
        qusetButton.interactable = false;
        qusetButton.image.sprite = qusetButtonImages[0];
        if (qusetContents.qusetType == QusetType.Main)
            qusetText.text = $"{qusetContents.text[0]} {qusetContents.qusetCondition} {qusetContents.text[1]}";
        else
            qusetText.text = qusetContents.text[0];
    }
    public void QusetClear()
    {
        if (qusetContents.qusetType == QusetType.Main)
        {
            qusetButton.image.sprite = qusetButtonImages[0];
            qusetContents.isClear = false;
            qusetContents.qusetCondition = qusetContents.rewards + qusetContents.M_UpCondition * qusetContents.M_ClearCount;
            qusetText.text = $"{qusetContents.text[0]} {qusetContents.qusetCondition} {qusetContents.text[1]}";
        }
        switch (qusetContents.rewardType)
        {
            case RewardType.Gold:
                {
                    GameManager.Instance.Gold += qusetContents.rewards;
                    return;
                }
            case RewardType.Heart:
                {
                    GameManager.Instance.Heart += qusetContents.rewards;
                    return;
                }
            case RewardType.Macaron:
                {
                    GameManager.Instance.Macaron += qusetContents.rewards;
                    return;
                }
        }
    }
}
