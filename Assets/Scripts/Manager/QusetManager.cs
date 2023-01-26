using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class QusetManager : Singleton<QusetManager>
{
    public QuestScriptable[] qusetScriptables;
    public Sprite[] rewardSprite;
    public Material[] m_RewardParticle;
    
    protected override void Awake()
    {
        base.Awake();
        if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// ����
    /// ����Ʈ �ε���
    /// ���� ��ġ
    /// </summary>
    public void QusetUpdate(QuestType Type, int QusetId, float Progress) 
        => qusetScriptables[(int)Type].QusetList[QusetId].questSituation += Progress;
}
