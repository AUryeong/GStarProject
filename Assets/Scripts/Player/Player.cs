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
public class Player : MonoBehaviour
{
    protected SpriteRenderer spriteRenderer;
    protected Rigidbody2D rigid;
    protected Animator animator;
    protected BoxCollider2D colider2D;

    protected PlayerState state;

    public Breads.Type type;
    public float fSpeed = 5;
    [HideInInspector] public float fHp;

    public float hp;
    public bool isControllable
    {
        get; protected set; 
    } = true;


    [Header("�÷��̾� ���� ����")]
    public float fJumpSpeed = 10;
    protected float jumpCheckDistance = 0.1f;
    protected int jumpCount = 0;
    public int jumpMaxCount = 2;

    //��ֹ� �浹
    protected bool hitable = true;
    protected float hitDamage = 10;
    protected float hitFadeInTime = 0.1f;
    protected float hitFadeInAlpha = 0.5f;
    protected float hitFadeOutTime = 0.9f;

    [Header("��ֹ� �浹 ����")]
    public Vector2 idleColiderSize = new Vector2(0.8f, 2f);
    public Vector2 slidingColiderSize = new Vector2(2, 0.8f);
    public Vector2 jumpingColiderSize = new Vector2(1f, 1f);

    protected float downGameoverY = -4.5f;

    //�ð� ���������� �ٴ� hp
    protected float hpRemoveCool = 1;
    protected float hpRemoveDuration = 0;
    protected float hpRemoveValue = 1;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        colider2D = GetComponent<BoxCollider2D>();
    }

    protected virtual void OnEnable()
    {
        fHp = GameManager.Instance.breads.Stats[(int)type].HP;
        hp = fHp;
    }

    protected virtual void Update()
    {
        float deltaTime = Time.deltaTime;
        if (hp > 0 && isControllable)
        {
            Move(deltaTime);
            CheckJumpReset();
            CheckPressKey();
            CheckAnimator();
            HpRemove();
            LiveUpdate(deltaTime);
            if (transform.position.y <= downGameoverY)
            {
                isControllable = false;
                InGameManager.Instance.GameOverMoveCP();
            }
        }
        if(Input.GetKeyDown(KeyCode.C))
        {
            GameOver();
        }
    }

    //��������� �۵��ϴ� ������Ʈ, �ɷ¿��� ��� ����
    protected virtual void LiveUpdate(float deltaTime)
    {
    }

    public void MoveCenter()
    {
        StartCoroutine(MoveCenterCoroutine());
    }

    IEnumerator MoveCenterCoroutine()
    {
        while (true)
        {
            Move(Time.deltaTime);
            if(transform.position.x >= Camera.main.ScreenToWorldPoint(Vector3.zero).x)
            {
                animator.Play("Die");
                yield return new WaitForSeconds(2);
                InGameManager.Instance.GameOverMoveCP();
                yield break;
            }
            yield return null;
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

    protected virtual void HpRemove()
    {
        hpRemoveDuration += Time.deltaTime;
        if(hpRemoveDuration >= hpRemoveCool)
        {
            hpRemoveDuration -= hpRemoveCool;
            hp -= hpRemoveValue;
            if (hp <= 0)
            {
                GameOver();
                return;
            }
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
        if (Application.platform != RuntimePlatform.Android && isControllable && GameManager.Instance.inGaming)
        {
            if (Input.GetKey(KeyCode.DownArrow)) // ���� �߿� �����̵� ������ �� �������ڸ��� �����̵���
                Sliding();
            else if (Input.GetKeyDown(KeyCode.UpArrow)) // ������ �������� ������
                Jump();
            if (Input.GetKeyUp(KeyCode.DownArrow))
                ReturnToIdle();
        }
    }

    protected virtual void GameOver()
    {
        if (isControllable)
        {
            isControllable = false;
            InGameManager.Instance.GameOver();
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
        if (collider2D == null) return;
        if (!isControllable || hp < 0) return;

        if (collider2D.CompareTag("Block"))
            HurtByBlock(collider2D.GetComponent<Block>());

        if (collider2D.CompareTag("Gold"))
            GetGold(collider2D.gameObject);

        if (collider2D.CompareTag("Ingredient"))
            AddIngredient(collider2D.GetComponent<Ingredient>());
    }

    //��� ȹ��
    protected virtual void AddIngredient(Ingredient ingredient)
    {
        ingredient.OnGet();
        InGameManager.Instance.AddIngredients(ingredient);

        ingredient.gameObject.SetActive(false);
    }

    protected void GetGold(GameObject obj)
    {
        obj.SetActive(false);
        // ����Ʈ �ֱ� TODO
        InGameManager.Instance.gold++;
    }

    //��ֹ��� �ε����� ���
    protected virtual void HurtByBlock(Block block)
    {
        if (!hitable)
            return;

        hitable = false;

        hp -= block.damage;
        block.OnHit();
        if (hp <= 0)
        { 
            GameOver();
            return;
        }

        IngameUIManager.Instance.PlayerHurt();

        gameObject.layer = LayerMask.NameToLayer("PlayerInv");
        spriteRenderer.DOFade(hitFadeInAlpha, hitFadeInTime).
            OnComplete(() => spriteRenderer.DOFade(1, hitFadeOutTime).SetEase(Ease.InExpo).
            OnComplete(() =>
            {
                hitable = true;
                gameObject.layer = LayerMask.NameToLayer("Player");
            }));
    }
}
