using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_WaterMelon : Player
{
    protected override void HpRemove()
    {
        hpRemoveDuration += Time.deltaTime;
        if (hpRemoveDuration >= hpRemoveCool)
        {
            hpRemoveDuration -= hpRemoveCool;
            if (state != PlayerState.Sliding)
                hp -= hpRemoveValue * 2;
            if (hp <= 0)
            {
                GameOver();
                return;
            }
        }
    }
}
