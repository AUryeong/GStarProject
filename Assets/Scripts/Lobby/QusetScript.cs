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

    [SerializeField] ParticleSystem particle;//클리어시 나오는 보상 파티클입니다.
    [SerializeField] ParticleSystem starParticle;//클리어시 나오는 별 파티클입니다.
    [SerializeField] ParticleSystemRenderer particleSystemRenderer;//보상에 따라 머터리얼을 바꿀때 사용됩니다

    private ParticleSystem.ExternalForcesModule externalForcesModule;
    private ParticleSystem.TriggerModule triggerModule;//목표 지점에 다 도착하면 바로 사라지도록 합니다.
    private LayerMask windfildLayerMask;//보상 파티클이 모이게할 때 사용됩니다.

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
        //퀘스트 내용 적용
        questContents = scriptable.QusetList[Idx];

        //퀘스트 보상 스프라이트 적용
        qusetSprite = QusetManager.Instance.rewardSprite[(int)questContents.rewardType];
        questImage.sprite = qusetSprite;

        //퀘스트 보상 표시
        questrewardText.text = "" + questContents.rewards;

        //퀘스트 달성 미달성 스프라이트 변경, 클릭 함수 적용
        questButton.image.sprite = questButtonImages[0];
        questButton.interactable = false;
        questButton.onClick.AddListener(QusetClear);

        //보상 파티클 모듈 적용
        externalForcesModule = particle.externalForces;
        triggerModule = particle.trigger;

        //파티클 보상 스프라이트 적용
        particleSystemRenderer.material = QusetManager.Instance.m_RewardParticle[(int)questContents.rewardType];

        //보상에 맞는 위치로 레이어를 설정
        windfildLayerMask = LayerMask.GetMask(questContents.rewardType.ToString());

        //목표 텍스트 변경
        if (questContents.questEnd == true)
            questText.text = $"{questContents.text[0]}";
        else
            questText.text = $"{questContents.text[0]}<color=#48FFFF>{questContents.questCondition}</color>{questContents.text[1]}";

        //클리어 한거 일 시 클리어 표시로 변경
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

        //메인일때는 달성시 바로 초기화
        if (questContents.questType == QuestType.Main)
        {
            //잠깐 작아졌다가 다시 커지는 연출
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
            //작아지면서 사라지는 연출
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
        //보상 추가
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

        externalForcesModule.influenceMask = new LayerMask();//WindFild를 초기화합니다
        triggerModule.RemoveCollider(0);//triggerModule을 초기화 합니다

        yield return new WaitForSeconds(0.5f);

        externalForcesModule.influenceMask = windfildLayerMask;//WindFild의 레이어를 넣는다
        triggerModule.AddCollider(LobbyUIManager.Instance.topUICoilider[(int)questContents.rewardType]);//닿으면 사라지도록 콜라이더를 지정한다
    }

}
