using UnityEngine;
using UnityEngine.UI;

public class Player_Toast : Player
{
    public float power;
    public float hpHealValue = 2;
    
    protected override void LiveUpdate(float deltaTime)
    {
        var image = InGameManager.Instance.toastDarkPanel;
        image.color = new Color(0, 0, 0, Mathf.Min(0.4f, image.color.a + Time.deltaTime * power));
    }
    
    protected override void AddIngredient(Ingredient ingredient)
    {
        base.AddIngredient(ingredient);
        hp += hpHealValue;
        InGameManager.Instance.toastDarkPanel.color = new Color(0, 0, 0, 0);
    }
}