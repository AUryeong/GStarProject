using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum RewardType
{
    Gold,
    Heart,
    Macaron
}
public enum QusetType
{
    Day,
    Aweek,
    Main
}
[CreateAssetMenu(fileName = "Quset Data", menuName = "ScriptableObject2/Quset", order = int.MaxValue)]
[System.Serializable]

public class Quset
{
    public bool isClear;//클리어 bool
    public float qusetCondition;//퀘스트 조건
    public float questSituation;//퀘스트 상황
    public Sprite sprite;//이미지
    public string text;//퀘스트 내용
    public float rewards;//보상 수치
    public float M_UpCondition;//(메인 전용) 조건 증가 수치
    public float M_ClearCount;//(메인 전용) 클리어 횟수
    public RewardType rewardType;
    public QusetType qusetType;
}
public class QusetScriptable : MonoBehaviour
{
    public List<Quset> QusetList;
}
