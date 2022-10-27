using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Safe : Player
{

    protected override void OnEnable()
    {
        base.OnEnable();
        hpRemoveValue = 0.5f;
    }

    protected override void LiveUpdate(float deltaTime)
    {
        if (hp / fHp >= 0.5f)
            magnetSize = 3;
        else
            magnetSize = 0;
    }
}
