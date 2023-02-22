using UnityEngine;

public class SideBlockObject : MonoBehaviour
{
    public void SettingObject(Sprite sprite, Vector2 colliderOffset, int layer)
    {
        BoxCollider2D collider2D = GetComponent<BoxCollider2D>();
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        collider2D.offset = colliderOffset;

        spriteRenderer.sprite = sprite;
        spriteRenderer.sortingOrder = layer;
    }
}