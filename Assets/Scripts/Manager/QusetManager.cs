using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class QusetManager : Singleton<QusetManager>
{
    public QuestScriptable[] qusetScriptables;
    public Sprite[] rewardSprite;
    protected override void Awake()
    {
        base.Awake();
        if (Instance == this)
            DontDestroyOnLoad(gameObject);
    }
    /// <summary>
    /// ����
    /// ����Ʈ �ε���
    /// ���� ��ġ
    /// </summary>
    /// <param name="Type"></param>
    /// <param name="QusetId"></param>
    /// <param name="Progress"></param>
    public void QusetUpdate(QuestType Type, int QusetId, float Progress) 
        => qusetScriptables[(int)Type].QusetList[QusetId].questSituation += Progress;
}
