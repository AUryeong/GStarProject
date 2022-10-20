using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QusetScript : MonoBehaviour
{
    private Quset QusetContents;
    [SerializeField] Image QusetImage;//퀘스트 보상 이미지
    [SerializeField] Text QusetrewardText;//퀘스트 보상 텍스트
    [SerializeField] Text QusetText; //퀘스트 내용 텍스트
    [SerializeField] Button QusetButton; //퀘스트 내용 텍스트
    [SerializeField] Text QusetButtonText; //퀘스트 내용 텍스트
    [SerializeField] Slider QusetSliders;//퀘스트 진행도 슬라이더
    // Start is called before the first frame update
    private void Start()
    {

    }
    private void Update()
    {
        if (!QusetContents.isClear)
        {
            QusetSliders.value = QusetContents.questSituation / QusetContents.qusetCondition;
            if (QusetContents.qusetCondition >= QusetContents.questSituation)
            {
                QusetButtonText.text = "달성";
                QusetButton.interactable = true;
                QusetContents.isClear = true;
            }
        }
    }
    public void SettingQuset(QusetScriptable scriptable, int Idx)
    {
        QusetContents = scriptable.QusetList[Idx];
        QusetImage.sprite = QusetContents.sprite;
        QusetrewardText.text = "" + QusetContents.rewards;
        QusetButton.onClick.AddListener(QusetClear);
        QusetButton.interactable = false;
        QusetButtonText.text = "미달성";
        if (QusetContents.qusetType == QusetType.Main)
            QusetText.text = $"{QusetContents.text[0]} {QusetContents.qusetCondition} {QusetContents.text[1]}";
        else
            QusetText.text = QusetContents.text[0];
    }
    public void QusetClear()
    {
        if (QusetContents.qusetType == QusetType.Main)
        {
            QusetButtonText.text = "미달성";
            QusetContents.isClear = false;
            QusetContents.qusetCondition = QusetContents.rewards + QusetContents.M_UpCondition * QusetContents.M_ClearCount;
            QusetText.text = $"{QusetContents.text[0]} {QusetContents.qusetCondition} {QusetContents.text[1]}";
        }
        switch (QusetContents.rewardType)
        {
            case RewardType.Gold:
                {
                    GameManager.Instance.Gold += QusetContents.rewards;
                    return;
                }
            case RewardType.Heart:
                {
                    GameManager.Instance.Heart += QusetContents.rewards;
                    return;
                }
            case RewardType.Macaron:
                {
                    GameManager.Instance.Macaron += QusetContents.rewards;
                    return;
                }
        }
    }
}
