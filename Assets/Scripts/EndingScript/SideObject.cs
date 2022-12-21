using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideObject : MonoBehaviour
{
    public Stats stats;
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(stats.name == Ingredients.Type.Durian && collision.transform.tag == "Side")
        {

        }
    }
}
