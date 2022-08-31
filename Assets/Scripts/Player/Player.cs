using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum PlayerState
{
    Idle,
    Sliding,
    Jumping
}

//�÷��̾� �ɷ°����� ���� ���ɼ��� ���⿡ ��κ��� �Լ��� Virtual�� �ۼ���
public class Player : Singleton<Player>
{
    protected SpriteRenderer spriteRenderer;
    protected Rigidbody2D rigid;
    protected Animator animator;
    protected BoxCollider2D colider2D;

    private PlayerState state;

    public float fSpeed;
    public float fHp;

    public List<int> ingredients = new List<int>();

    [Header("�÷��̾� ���� ����")]
    public float fJumpSpeed;
    private float jumpCheckDistance = 0.1f;
    private int jumpCount = 0;
    public int jumpMaxCount = 2;

    //��ֹ� �浹
    protected bool hitable = true;
    protected float hitDamage = 10;
    private float hitFadeInTime = 0.1f;
    private float hitFadeInAlpha = 0.5f;
    private float hitFadeOutTime = 1f;

    private Vector2 idleColiderSize = new Vector2(0.8f, 2f);
    private Vector2 slidingColiderSize = new Vector2(2, 0.8f);
    private Vector2 jumpingColiderSize = new Vector2(1f, 1f);

    protected override void Awake()
    {
        base.Awake();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        colider2D = GetComponent<BoxCollider2D>();
    }

    protected virtual void Update()
    {
        float deltaTime = Time.deltaTime;
        if (fHp > 0)
        {
            Move(deltaTime);
            CheckJumpReset();
            CheckPressKey();
            CheckAnimator();
        }
    }

    //���� ���� ������ �����ϴ� �Լ�
    protected virtual void CheckJumpReset()
    {
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(transform.position, colider2D.size, 0, Vector2.down, jumpCheckDistance, LayerMask.GetMask("Platform"));
        if (raycastHit2D.collider != null)
        {
            jumpCount = 0;
            if (state == PlayerState.Jumping)
                state = PlayerState.Idle;
        }
        else
        {
            if (state == PlayerState.Idle)
                state = PlayerState.Jumping;
            if (jumpCount == 0) // ������ ���������� ���� Ƚ�� ���̱� ����
                jumpCount++;
        }
    }

    //�̵��� �����ϴ� �Լ�
    protected virtual void Move(float deltaTime)
    {
        transform.Translate(deltaTime * fSpeed * Vector2.right);
    }

    //��ǻ���϶� Ű���� ������ ����ϴ� �Լ�
    protected virtual void CheckPressKey()
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.DownArrow)) // ���� �߿� �����̵� ������ �� �������ڸ��� �����̵���
                Sliding();
            else if (Input.GetKeyDown(KeyCode.UpArrow)) // ������ �������� ������
                Jump();
            if (Input.GetKeyUp(KeyCode.DownArrow))
                ReturnToIdle();
        }
    }


    //�ִϸ����� ���� ������Ʈ
    protected virtual void CheckAnimator()
    {
        if (animator.GetInteger("State") != (int)state)
            animator.SetInteger("State", (int)state);

        switch (state)
        {
            case PlayerState.Idle:
                colider2D.size = idleColiderSize;
                break;
            case PlayerState.Sliding:
                colider2D.size = slidingColiderSize;
                break;
            case PlayerState.Jumping:
                colider2D.size = jumpingColiderSize;
                break;
        }

        //colider2D.size = spriteRenderer.bounds.size;
    }

    //��ư�� ���� public, �Լ�ȭ
    public virtual void Sliding()
    {
        if (state == PlayerState.Idle)
        {
            state = PlayerState.Sliding;
        }
    }

    public virtual void Jump()
    {
        if (jumpCount >= jumpMaxCount)
            return;
        jumpCount++;

        if (state == PlayerState.Jumping)
            animator.SetTrigger("Jump");

        state = PlayerState.Jumping;

        rigid.velocity = Vector2.zero;
        rigid.AddForce(Vector2.up * fJumpSpeed, ForceMode2D.Impulse);
    }
    public virtual void ReturnToIdle()
    {
        if (state == PlayerState.Sliding)
            state = PlayerState.Idle;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D == null)
            return;

        if (collider2D.CompareTag("Block"))
            HurtByBlock();

        if (collider2D.CompareTag("Ingredient"))
            AddIngredient(collider2D.GetComponent<Ingredient>());
    }

    //��� ȹ��
    protected void AddIngredient(Ingredient ingredient)
    {
        ingredient.OnGet();
        ingredients.Add(ingredient.ingredientIdx);

        ingredient.gameObject.SetActive(false);
    }

    //��ֹ��� �ε����� ���
    protected virtual void HurtByBlock()
    {
        if (!hitable)
            return;

        hitable = false;

        fHp -= hitDamage;
        if (fHp <= 0)
        {
            GameManager.Instance.GameOver();
            return;
        }

        spriteRenderer.DOFade(hitFadeInAlpha, hitFadeInTime).
            OnComplete(() => spriteRenderer.DOFade(1, hitFadeOutTime).SetEase(Ease.InExpo).
            OnComplete(() => hitable = true));
    }
}
