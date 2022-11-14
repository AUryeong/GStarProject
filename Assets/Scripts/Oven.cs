using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oven : MonoBehaviour
{
    protected float distance;
    protected bool isOvenable = false;
    Animator animator;
    protected void Awake()
    {
        animator = GetComponent<Animator>();
    }
    protected void OnEnable()
    {
        distance = InGameManager.Instance.player.transform.position.x - transform.position.x;
        animator.Play("Idle");
        isOvenable = true;
    }
    protected void Update()
    {
        if (!isOvenable)
            return;

        float dis = InGameManager.Instance.player.transform.position.x - transform.position.x;
        if (Mathf.Abs(dis) > Mathf.Abs(distance) && dis > 0)
        {
            isOvenable = false;
            InGameManager.Instance.player.GetOven(animator);
        }
        else
            distance = dis;

    }
}
