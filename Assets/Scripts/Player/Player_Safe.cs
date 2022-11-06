using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Safe : Player
{
    bool magnetActive;

    protected override void OnEnable()
    {
        base.OnEnable();
        magnetActive = false;
    }
    protected override void HpRemove()
    {
        hpRemoveDuration += Time.deltaTime;
        if (hpRemoveDuration >= hpRemoveCool)
        {
            hpRemoveDuration -= hpRemoveCool;
            if (magnetSize != 0 && magnetMoveSpeed != 0)
                hp -= hpRemoveValue/2;
            else
                hp -= hpRemoveValue;
            if (hp <= 0)
            {
                GameOver();
                return;
            }
        }
    }

    protected override void LiveUpdate(float deltaTime)
    {
        if (hp / fHp >= 0.5f)
        {
            if (!magnetActive)
            {
                magnetActive = true;
                magnetSize += 3;
            }
        }
        else
        {
            if (magnetActive)
            {
                magnetActive = false;
                magnetSize -= 3;
            }
        }
    }
}
