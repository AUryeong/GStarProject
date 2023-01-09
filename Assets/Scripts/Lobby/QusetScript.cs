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

    [SerializeField] ParticleSystem particle;//Ŭ����� ������ ���� ��ƼŬ�Դϴ�.
    [SerializeField] ParticleSystem starParticle;//Ŭ����� ������ �� ��ƼŬ�Դϴ�.
    [SerializeField] ParticleSystemRenderer particleSystemRenderer;//���� ���� ���͸����� �ٲܶ� ���˴ϴ�

    private ParticleSystem.ExternalForcesModule externalForcesModule;
    private ParticleSystem.TriggerModule triggerModule;//��ǥ ������ �� �����ϸ� �ٷ� ��������� �մϴ�.
    private LayerMask windfildLayerMask;//���� ��ƼŬ�� ���̰��� �� ���˴ϴ�.

    private void Start()
    {
        if (questContents.isClear == true)
        {
            transform.SetAsLastSibling();
        }
    }
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
        //����Ʈ ���� ����
        questContents = scriptable.QusetList[Idx];

        //����Ʈ ���� ��������Ʈ ����
        qusetSprite = QusetManager.Instance.rewardSprite[(int)questContents.rewardType];
        questImage.sprite = qusetSprite;

        //����Ʈ ���� ǥ��
        questrewardText.text = "" + questContents.rewards;

        //����Ʈ �޼� �̴޼� ��������Ʈ ����, Ŭ�� �Լ� ����
        questButton.image.sprite = questButtonImages[0];
        questButton.interactable = false;
        questButton.onClick.AddListener(QusetClear);

        //���� ��ƼŬ ��� ����
        externalForcesModule = particle.externalForces;
        triggerModule = particle.trigger;

        //��ƼŬ ���� ��������Ʈ ����
        particleSystemRenderer.material = QusetManager.Instance.m_RewardParticle[(int)questContents.rewardType];

        //���� �´� ��ġ�� ���̾ ����
        windfildLayerMask = LayerMask.GetMask(questContents.rewardType.ToString());

        //��ǥ �ؽ�Ʈ ����
        if (questContents.questEnd == true)
            questText.text = $"{questContents.text[0]}";
        else
            questText.text = $"{questContents.text[0]}<color=#48FFFF>{questContents.questCondition}</color>{questContents.text[1]}";

        //Ŭ���� �Ѱ� �� �� Ŭ���� ǥ�÷� ����
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
            }
        }
    }
    public void QusetClear()
    {
        questContents.isClear = true;
        StartCoroutine(QuestClearParticle());
        StartCoroutine(C_QusetClear());
    }
    private IEnumerator C_QusetClear()
    {
        float timer = 1;
        float timespeed = 3;

        //�����϶��� �޼��� �ٷ� �ʱ�ȭ
        if (questContents.questType == QuestType.Main)
        {
            //��� �۾����ٰ� �ٽ� Ŀ���� ����
            while (timer >= 0.9f)
            {
                timer -= Time.deltaTime * timespeed;
                transform.localScale = Vector3.one * timer;
                yield return null;
            }
            while (timer <= 1f)
            {
                timer += Time.deltaTime * timespeed;
                transform.localScale = Vector3.one * timer;
                yield return null;
            }
            transform.localScale = Vector3.one;

            questButton.image.sprite = questButtonImages[0];
            questContents.isClear = false;
            questContents.questCondition = questContents.questCondition + questContents.M_UpCondition * ++questContents.M_ClearCount;
            questText.text = $"{questContents.text[0]} {questContents.questCondition}{questContents.text[1]}";
        }
        else
        {
            //�۾����鼭 ������� ����
            while (timer > 0)
            {
                timer -= Time.deltaTime * timespeed;
                transform.localScale = Vector3.one * timer;
                yield return null;
            }

            questButton.gameObject.SetActive(false);
            fadePanel.SetActive(true);
            transform.SetAsLastSibling();
            transform.localScale = Vector3.one;
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
        //���� �߰�
        switch (questContents.rewardType)
        {
            case RewardType.Gold:
                {
                    GameManager.Instance.gold += questContents.rewards;
                    break;
                }
            case RewardType.Heart:
                {
                    GameManager.Instance.heart += questContents.rewards;
                    break;
                }
            case RewardType.Macaron:
                {
                    GameManager.Instance.macaron += questContents.rewards;
                    break;
                }
        }
    }
    private IEnumerator QuestClearParticle()
    {
        particle.Play();
        starParticle.Play();

        externalForcesModule.influenceMask = new LayerMask();//WindFild�� �ʱ�ȭ�մϴ�
        triggerModule.RemoveCollider(0);//triggerModule�� �ʱ�ȭ �մϴ�

        yield return new WaitForSeconds(0.5f);

        externalForcesModule.influenceMask = windfildLayerMask;//WindFild�� ���̾ �ִ´�
        triggerModule.AddCollider(LobbyUIManager.Instance.topUICoilider[(int)questContents.rewardType]);//������ ��������� �ݶ��̴��� �����Ѵ�
    }

}
