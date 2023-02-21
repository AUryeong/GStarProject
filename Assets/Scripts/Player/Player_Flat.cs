using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Flat : Player
{
    private float abilityDuration;
    private float abilityCooltime = 0.5f;

    private bool abilityActive;

    public override void ReturnToIdle()
    {
        if (state == PlayerState.Sliding)
        {
            state = PlayerState.Idle;
            abilityDuration = abilityCooltime;
        }
    }

    protected override void CheckJumpReset()
    {
        var raycastHit2D = Physics2D.BoxCast((Vector2)transform.position + colider2D.offset, colider2D.size * transform.localScale.y, 0, Vector2.down, jumpCheckDistance,
            LayerMask.GetMask("Platform"));
        if (raycastHit2D.collider != null)
            abilityActive = false;

        base.CheckJumpReset();
    }

    protected override void AddIngredient(Ingredient ingredient)
    {
        if (abilityActive)
            InGameManager.Instance.AddIngredients(ingredient);
        base.AddIngredient(ingredient);
    }

    protected override void LiveUpdate(float deltaTime)
    {
        if (abilityDuration > 0)
            abilityDuration -= abilityCooltime;
    }

    public override void Jump()
    {
        if (jumpCount >= jumpMaxCount)
        {
            if (abilityDuration > 0 || state == PlayerState.Sliding)
            {
                hp += 3;
                abilityActive = true;
            }

            base.Jump();
        }
    }
}