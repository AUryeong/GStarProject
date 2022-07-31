using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    //��Ű���� ��� �����ۿ� ���� �޶����� �ϴ� public
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
