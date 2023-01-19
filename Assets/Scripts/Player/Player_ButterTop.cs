using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_ButterTop : Player
{
    private float butterCreateCooltime = 15;
    private float butterCreateDuration = 0;

    [SerializeField] private Buttertop_Ingredient buttertopIngredient;

    protected override void AddIngredient(Ingredient ingredient)
    {
        ingredient.OnGet();
        ingredient.gameObject.SetActive(false);
        InGameManager.Instance.AddIngredients(Ingredients.Type.Butter);
        SoundManager.Instance.PlaySoundClip("SFX_InGame_Get_Ingredient", ESoundType.SFX);
    }

    protected override void LiveUpdate(float deltaTime)
    {
        butterCreateDuration += deltaTime;
        if (butterCreateDuration >= butterCreateCooltime)
        {
            butterCreateDuration -= butterCreateCooltime;
            //버터 생성!

            GameObject obj = PoolManager.Instance.Init(buttertopIngredient.gameObject);
            obj.transform.position = new Vector3(transform.position.x + 20, Random.Range(2.5f, 5.5f), 0);
        }
    }
}