using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QusetScript : MonoBehaviour
{
    private Quset qusetContents;
    [SerializeField] Image qusetImage;//����Ʈ ���� �̹���
    [SerializeField] Text qusetrewardText;//����Ʈ ���� �ؽ�Ʈ
    [SerializeField] Text qusetText; //����Ʈ ���� �ؽ�Ʈ
    [SerializeField] Button qusetButton; //����Ʈ ��ư
    [SerializeField] Sprite[] qusetButtonImages;//��ư �̹���
    [SerializeField] Slider qusetSliders;//����Ʈ ���൵ �����̴�
    [SerializeField] GameObject fadePanel;//����Ʈ ���൵ �����̴�
    // Start is called before the first frame update
    private void Start()
    {

    }
    private void Update()
    {
        if(qusetContents.isClear == false)
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
        qusetContents.isClear = true;
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
