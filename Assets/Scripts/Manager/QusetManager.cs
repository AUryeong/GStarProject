using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class QusetManager : Singleton<QusetManager>
{
    public QusetScriptable[] qusetScriptables;
    public Sprite[] rewardSprite;
    protected override void Awake()
    {
        base.Awake();
        if (Instance == this)
            DontDestroyOnLoad(gameObject);
    }
    public void QusetUpdate(QusetType Type, int QusetId , float Progress)
    {
        switch (Type){
            case QusetType.Day:
                qusetScriptables[0].QusetList[QusetId].qusetCondition += Progress;
                return; 
            case QusetType.Aweek:
                qusetScriptables[1].QusetList[QusetId].qusetCondition += Progress;
                return;
            case QusetType.Main:
                qusetScriptables[2].QusetList[QusetId].qusetCondition += Progress;
                return;
        }
    }
}
