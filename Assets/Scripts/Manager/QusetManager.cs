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
    public void QusetUpdate(QuestType Type, int QusetId , float Progress)
    {
        switch (Type){
            case QuestType.Day:
                qusetScriptables[0].QusetList[QusetId].questCondition += Progress;
                return; 
            case QuestType.Aweek:
                qusetScriptables[1].QusetList[QusetId].questCondition += Progress;
                return;
            case QuestType.Main:
                qusetScriptables[2].QusetList[QusetId].questCondition += Progress;
                return;
        }
    }
}
