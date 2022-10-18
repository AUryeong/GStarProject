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
    public bool isClear;//Ŭ���� bool
    public float qusetCondition;//����Ʈ ����
    public float questSituation;//����Ʈ ��Ȳ
    public Sprite sprite;//�̹���
    public string text;//����Ʈ ����
    public float rewards;//���� ��ġ
    public float M_UpCondition;//(���� ����) ���� ���� ��ġ
    public float M_ClearCount;//(���� ����) Ŭ���� Ƚ��
    public RewardType rewardType;
    public QusetType qusetType;
}
public class QusetScriptable : MonoBehaviour
{
    public List<Quset> QusetList;
}
