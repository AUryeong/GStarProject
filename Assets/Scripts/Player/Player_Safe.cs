using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Safe : Player
{
    bool magnetActive;

    protected override void OnEnable()
    {
        base.OnEnable();
        hpRemoveValue = 0.5f;
        magnetActive = false;
    }

    protected override void LiveUpdate(float deltaTime)
    {
        if (hp / fHp >= 0.5f && !magnetActive)
        {
            magnetActive = true;
            magnetSize = 3;
        }
        else if (magnetActive)
        {
            magnetActive = false;
            magnetSize = 0;
        }
    }
}
