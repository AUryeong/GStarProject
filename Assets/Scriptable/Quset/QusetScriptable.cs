using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Quset Data", menuName = "ScriptableObject/Qusets", order = int.MaxValue)]
public class QusetScriptable : ScriptableObject
{
    public List<Quset> QusetList;
}
[System.Serializable]
public class Quset
{
    public bool isClear = false;//Ŭ���� bool
    public float qusetCondition;//����Ʈ ����
    public float questSituation;//����Ʈ ��Ȳ
    public Sprite sprite;//�̹���
    public string[] text;//����Ʈ ����
    public int rewards;//���� ��ġ
    public float M_UpCondition;//(���� ����) ���� ���� ��ġ
    public float M_ClearCount;//(���� ����) Ŭ���� Ƚ��
    public RewardType rewardType;
    public QusetType qusetType;
}
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
