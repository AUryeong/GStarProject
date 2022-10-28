using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class QusetManager : Singleton<QusetManager>
{
    [SerializeField] QusetScriptable dayQuset;
    [SerializeField] QusetScriptable aweekQuset;
    [SerializeField] QusetScriptable mainQuset;
    public Sprite[] rewardSprite;
    public void QusetUpdate(QusetType Type, int QusetId , float Progress)
    {
        switch (Type){
            case QusetType.Day:
                dayQuset.QusetList[QusetId].qusetCondition += Progress;
                return; 
            case QusetType.Aweek:
                aweekQuset.QusetList[QusetId].qusetCondition += Progress;
                return;
            case QusetType.Main:
                mainQuset.QusetList[QusetId].qusetCondition += Progress;
                return;
        }
    }
}
