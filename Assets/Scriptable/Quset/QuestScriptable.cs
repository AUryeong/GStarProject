using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Quest Data", menuName = "ScriptableObject/Quests", order = int.MaxValue)]
public class QuestScriptable : ScriptableObject
{
    public List<Quest> QusetList;
}
[System.Serializable]
public class Quest
{
    public bool isClear = false;//클리어 bool
    public float questCondition;//퀘스트 조건
    public float questSituation;//퀘스트 상황
    public Sprite sprite;//이미지
    public string[] text;//퀘스트 내용
    public int rewards;//보상 수치
    public RewardType rewardType;
    public QuestType questType;
    public bool questEnd;//마지막 퀘스트인지 체크
    [Header("Main")]
    public float M_UpCondition;//(메인 전용) 조건 증가 수치
    public float M_ClearCount;//(메인 전용) 클리어 횟수
}
public enum RewardType
{
    Gold,
    Heart,
    Macaron
}
public enum QuestType
{
    Day,
    Aweek,
    Main
}
