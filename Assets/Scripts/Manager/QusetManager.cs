using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class QusetManager : Singleton<QusetManager>
{
    [SerializeField] QuestScriptable dayQuset;
    [SerializeField] QuestScriptable aweekQuset;
    [SerializeField] QuestScriptable mainQuset;
    public Sprite[] rewardSprite;
    public void QusetUpdate(QuestType Type, int QusetId , float Progress)
    {
        switch (Type){
            case QuestType.Day:
                dayQuset.QusetList[QusetId].questCondition += Progress;
                return; 
            case QuestType.Aweek:
                aweekQuset.QusetList[QusetId].questCondition += Progress;
                return;
            case QuestType.Main:
                mainQuset.QusetList[QusetId].questCondition += Progress;
                return;
        }
    }
}
