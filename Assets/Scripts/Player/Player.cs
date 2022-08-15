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

//플레이어 능력같은게 있을 가능성이 높기에 대부분의 함수를 Virtual로 작성함
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

    [Header("플레이어 점프 관련")]
    public float fJumpSpeed;
    private float jumpCheckDistance = 0.1f;
    private int jumpCount = 0;
    public int jumpMaxCount = 2;

    //장애물 충돌
    protected bool hitable = true;
    protected float hitDamage = 10;
    private float hitFadeInTime = 0.1f;
    private float hitFadeInAlpha = 0.5f;
    private float hitFadeOutTime = 1f;

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

    //점프 땅에 닿음을 감지하는 함수
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
            if (jumpCount == 0) // 땅에서 떨어질때는 점프 횟수 줄이기 위함
                jumpCount++;
        }
    }

    //이동을 관리하는 함수
    protected virtual void Move(float deltaTime)
    {
        transform.Translate(deltaTime * fSpeed * Vector2.right);
    }

    //컴퓨터일때 키보드 감지를 담당하는 함수
    protected virtual void CheckPressKey()
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.DownArrow)) // 점프 중에 슬라이딩 눌러도 땅 도착하자마자 슬라이딩함
                Sliding();
            else if (Input.GetKeyDown(KeyCode.UpArrow)) // 점프는 누를때만 감지함
                Jump();
            if (Input.GetKeyUp(KeyCode.DownArrow))
                ReturnToIdle();
        }
    }


    //애니메이터 관련 업데이트
    protected virtual void CheckAnimator()
    {
        if (animator.GetInteger("State") != (int)state)
        {
            animator.SetInteger("State", (int)state);
        }
        colider2D.size = spriteRenderer.sprite.bounds.size;
    }

    //버튼을 위한 public, 함수화
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

    //재료 획득
    protected void AddIngredient(Ingredient ingredient)
    {
        ingredient.OnGet();
        ingredients.Add(ingredient.ingredientIdx);

        ingredient.gameObject.SetActive(false);
    }

    //장애물에 부딛혔을 경우
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
