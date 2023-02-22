using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Player_Punch : Player
{

    protected override void OnEnable()
    {
        base.OnEnable();
    }
    protected override void AddIngredient(Ingredient ingredient)
    {
    }  
    protected override void HurtByBlock(Collider2D colider)
    {
        InGameManager.Instance.AddIngredientsForPunch(InGameManager.Instance.mapManager.selectMapData.mapBlockSpriteList.IndexOf(colider.GetComponent<SpriteRenderer>().sprite));
        SoundManager.Instance.PlaySoundClip("SFX_InGame_Get_Ingredient", ESoundType.SFX);
        animator.Play("Attack");
        
        colider.gameObject.layer = LayerMask.NameToLayer("BlockInv");
        colider.transform.DOScale(2, 1);
        colider.transform.DORotate(new Vector3(0, 0, 720), 1, RotateMode.FastBeyond360).SetRelative();
        colider.transform.DOMove(new Vector3(20, 20), 1).SetRelative().OnComplete(() =>
        {
            colider.gameObject.SetActive(false);
        });
    }
}