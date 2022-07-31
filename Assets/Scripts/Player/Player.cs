using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    //쿠키런의 경우 아이템에 따라 달라져서 일단 public
    public float speed;
    public float hp;


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (hp > 0)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
    }
}
