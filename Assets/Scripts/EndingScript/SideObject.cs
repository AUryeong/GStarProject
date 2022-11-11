using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideObject : MonoBehaviour
{
    public void SettingObject(Stats inspector)
    {
        BoxCollider2D collider2D = GetComponent<BoxCollider2D>();
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        collider2D.size = new Vector3(0.6f, inspector.coliderSize);
        collider2D.offset = new Vector3(0, inspector.coliderPos);
        spriteRenderer.sprite = inspector.SandwichSprite;
    }
}
