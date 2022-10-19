using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QusetScript : MonoBehaviour
{
    private Quset QusetContents;
    [SerializeField] Image QusetImage;//����Ʈ ���� �̹���
    [SerializeField] Text QusetrewardText;//����Ʈ ���� �ؽ�Ʈ
    [SerializeField] Text QusetText; //����Ʈ ���� �ؽ�Ʈ
    [SerializeField] Button QusetButton; //����Ʈ ���� �ؽ�Ʈ
    [SerializeField] Text QusetButtonText; //����Ʈ ���� �ؽ�Ʈ
    [SerializeField] Slider QusetSliders;//����Ʈ ���൵ �����̴�
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
                QusetButtonText.text = "�޼�";
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
        QusetText.text = QusetContents.text;
        QusetButton.onClick.AddListener(QusetClear);
        QusetButton.interactable = false;
    }
    public void QusetClear()
    {
        if (QusetContents.qusetType == QusetType.Main)
        {
            QusetButtonText.text = "�̴޼�";
            QusetContents.isClear = false;
            QusetContents.qusetCondition = QusetContents.rewards + QusetContents.M_UpCondition * QusetContents.M_ClearCount;
        }
        switch (QusetContents.rewardType)
        {
            case RewardType.Gold:
                {
                    return;
                }
            case RewardType.Heart:
                {
                    return;
                }
            case RewardType.Macaron:
                {
                    return;
                }
        }
    }
}
