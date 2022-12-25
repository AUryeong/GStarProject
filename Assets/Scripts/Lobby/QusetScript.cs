using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QusetScript : MonoBehaviour
{
    private Quest questContents;

    [SerializeField] Image questImage;//퀘스트 보상 이미지
    [SerializeField] Sprite qusetSprite;//퀘스트 보상 스프라이트

    [SerializeField] TextMeshProUGUI questrewardText;//퀘스트 보상 텍스트
    [SerializeField] TextMeshProUGUI questText; //퀘스트 내용 텍스트

    [SerializeField] Button questButton; //퀘스트 버튼
    [SerializeField] Sprite[] questButtonImages;//버튼 이미지

    [SerializeField] Slider questSliders;//퀘스트 진행도 슬라이더
    [SerializeField] GameObject fadePanel;//퀘스트 클리어 검정색 패널

    [SerializeField] ParticleSystem particle;
    [SerializeField] ParticleSystemRenderer particleSystemRenderer;
    private ParticleSystem.ExternalForcesModule externalForcesModule;
    private void Update()
    {
        if (questContents.isClear == false)
        {
            //퀘스트 진행도
            questSliders.value = questContents.questSituation / questContents.questCondition;
            if (questContents.questCondition <= questContents.questSituation)
            {
                //퀘스트 클리어시 완료로 바꾸기
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

        particleSystemRenderer.material = QusetManager.Instance.rewardParticle[(int)questContents.questType];
        externalForcesModule = particle.externalForces;

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

        //메인일때는 달성시 바로 초기화
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

        //QusetUpdate - 클리어 횟수 추가
        switch (questContents.questType)
        {
            case QuestType.Day:
                {
                    QusetManager.Instance.QusetUpdate(questContents.questType, 5, 1);//5 - 일일 퀘스트 클리어 갯수
                    break;
                }
            case QuestType.Aweek:
                {
                    QusetManager.Instance.QusetUpdate(questContents.questType, 7, 1);//7 - 주간 퀘스트 클리어 갯수
                    break;
                }
        }
        //보상
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
        yield return new WaitForSeconds(0.5f);
        LayerMask layerMask = LayerMask.GetMask(questContents.rewardType.ToString());
        externalForcesModule.influenceMask = layerMask;
    }
   
}
