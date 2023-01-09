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
    public bool isClear = false;//Ŭ���� bool
    public float questCondition;//����Ʈ ����
    public float questSituation;//����Ʈ ��Ȳ
    public Sprite sprite;//�̹���
    public string[] text;//����Ʈ ����
    public int rewards;//���� ��ġ
    public RewardType rewardType;
    public QuestType questType;
    public bool questEnd;//������ ����Ʈ���� üũ
    [Header("Main")]
    public float M_UpCondition;//(���� ����) ���� ���� ��ġ
    public float M_ClearCount;//(���� ����) Ŭ���� Ƚ��
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
