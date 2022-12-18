using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideObject : MonoBehaviour
{
    private Stats stats;

    private const int goodCount = 15;//좋은 재료 갯수
    public void SettingObject(Stats inspector,int layer)
    {
        stats = inspector;
        
        BoxCollider2D collider2D = GetComponent<BoxCollider2D>();
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        collider2D.size = new Vector3(0.6f, stats.coliderSize);
        collider2D.offset = new Vector3(0, stats.coliderPos);

        spriteRenderer.sprite = stats.SandwichSprite;
        spriteRenderer.sortingOrder = layer;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //부정적인 재료일때
        if((int)stats.name > goodCount && collision.tag == "Ingredients")
        {
            switch(stats.name)
            {
                case Ingredients.Type.Kimchi:
                    {

                        break;
                    }
            }
        }    
    }
}
