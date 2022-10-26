using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Safe : Player
{

    protected float magnetMoveSpeed = 1;
    protected override void OnEnable()
    {
        base.OnEnable();
        hpRemoveValue = 0.5f;
    }

    protected override void LiveUpdate(float deltaTime)
    {
        base.LiveUpdate(deltaTime);
        if(hp >= fHp * 0.5f)
        {
            Collider2D[] getableColiders = Physics2D.OverlapCircleAll(transform.position, Mathf.Max(colider2D.size.x, colider2D.size.y), LayerMask.NameToLayer("Getable"));
            foreach(var colider in getableColiders)
            {
                colider.transform.Translate((transform.position - colider.transform.position).normalized * magnetMoveSpeed * deltaTime);
            }
        }
    }
}
