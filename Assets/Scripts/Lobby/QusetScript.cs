using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QusetScript : MonoBehaviour
{
    private Quest questContents;

    [SerializeField] Image questImage;//����Ʈ ���� �̹���
    [SerializeField] Sprite qusetSprite;//����Ʈ ���� ��������Ʈ

    [SerializeField] TextMeshProUGUI questrewardText;//����Ʈ ���� �ؽ�Ʈ
    [SerializeField] TextMeshProUGUI questText; //����Ʈ ���� �ؽ�Ʈ

    [SerializeField] Button questButton; //����Ʈ ��ư
    [SerializeField] Sprite[] questButtonImages;//��ư �̹���

    [SerializeField] Slider questSliders;//����Ʈ ���൵ �����̴�
    [SerializeField] GameObject fadePanel;//����Ʈ Ŭ���� ������ �г�

    [SerializeField] ParticleSystem particle;//Ŭ����� ������ ��ƼŬ�Դϴ�.
    [SerializeField] ParticleSystemRenderer particleSystemRenderer;//���� ���� ���͸����� �ٲܶ� ���˴ϴ�

    private ParticleSystem.ExternalForcesModule externalForcesModule;
    private ParticleSystem.TriggerModule triggerModule;//��ǥ ������ �� �����ϸ� �ٷ� ��������� �մϴ�.
    private LayerMask windfildLayerMask;//���� ��ƼŬ�� ���̰��� �� ���˴ϴ�.

    private void Update()
    {
        if (questContents.isClear == false)
        {
            //����Ʈ ���൵
            questSliders.value = questContents.questSituation / questContents.questCondition;
            if (questContents.questCondition <= questContents.questSituation)
            {
                //����Ʈ Ŭ����� �Ϸ�� �ٲٱ�
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

        qusetSprite = QusetManager.Instance.rewardSprite[(int)questContents.rewardType];
        questImage.sprite = qusetSprite;

        questrewardText.text = "" + questContents.rewards;

        questButton.image.sprite = questButtonImages[0]; 
        questButton.interactable = false;
        questButton.onClick.AddListener(QusetClear);

        externalForcesModule = particle.externalForces;
        triggerModule = particle.trigger;
        
        particleSystemRenderer.material = QusetManager.Instance.m_RewardParticle[(int)questContents.questType];

        windfildLayerMask = LayerMask.GetMask(questContents.rewardType.ToString());//���� �´� ��ġ�� ���̾ �����մϴ�.

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
        StartCoroutine(QuestClearParticle());

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
        //����
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

    private IEnumerator QuestClearParticle()
    {
        particle.Play();
        externalForcesModule.influenceMask = new LayerMask();//WindFild�� �ʱ�ȭ�մϴ�
        triggerModule.RemoveCollider(0);//triggerModule�� �ʱ�ȭ �մϴ�

        yield return new WaitForSeconds(0.5f);

        externalForcesModule.influenceMask = windfildLayerMask;//WindFild�� ���̾ �ִ´�
        triggerModule.AddCollider(QusetManager.Instance.topUICoilider[(int)questContents.rewardType]);//������ ��������� �ݶ��̴��� �����Ѵ�
    }
   
}
