using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Runtime.Serialization;

public enum PlayerState
{
    Idle,
    Sliding,
    Jumping
}

[System.Serializable]
public struct ColiderPos
{
    public Vector2 offset;
    public Vector2 size;
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
    protected float fSpeed = 5;
    [HideInInspector] public float fHp;

    public float hp;
    public bool isControllable
    {
        get; protected set;
    } = true;


    [Header("�÷��̾� ���� ����")]
    protected float fJumpSpeed = 13;
    protected float jumpCheckDistance = 0.1f;
    protected int jumpCount = 0;
    protected int jumpMaxCount = 2;

    //��ֹ� �浹
    protected bool hitable = true;
    protected float hitDamage = 10;
    protected float hitFadeInTime = 0.1f;
    protected float hitFadeInAlpha = 0.5f;
    protected float hitFadeOutTime = 0.9f;

    [Header("�ڼ�")]
    protected float magnetMoveSpeed = 4f;
    protected float magnetSize = 0f;
    protected float itemMagnetSize = 6;
    protected float itemMagnetDuration = 5;

    [Header("�ν�Ʈ")]
    protected float boostMovePercent = 2;
    protected float itemBoostDuration = 5;
    public float boostDuration = 0;

    [Header("�佺�ͱ�")]
    protected float toasterHealValue = 20;
    protected float toasterInvDuration = 2f;

    [Header("��ֹ� �浹 ����")]
    public ColiderPos idleColiderSize;
    public ColiderPos slidingColiderSize;
    public ColiderPos jumpingColiderSize;

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
        fHp = GameManager.Instance.breads.Stats[(int)type].HP + GameManager.Instance.maxHpLv * 5;
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
            MagnetUpdate(deltaTime);
            LiveUpdate(deltaTime);
            BoostUpdate(deltaTime);
            if (transform.position.y <= downGameoverY)
            {
                isControllable = false;
                InGameManager.Instance.GameOverMoveCP();
            }
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            GameOver();
        }
    }

    //�ν�Ʈ �۵�
    protected virtual void BoostUpdate(float deltaTime)
    {
        
        if (boostDuration > 0)
        {
            boostDuration -= deltaTime;
            ParticleSystem boostEffect = InGameManager.Instance.boostEffect;
            if(boostDuration <= 0 == boostEffect.gameObject.activeSelf)
            {
                boostEffect.gameObject.SetActive(!boostEffect.gameObject.activeSelf);
            }
        }
    }

    //��������� �۵��ϴ� ������Ʈ, �ɷ¿��� ��� ����
    protected virtual void LiveUpdate(float deltaTime)
    {
    }

    //�ڼ�
    protected virtual void MagnetUpdate(float deltaTime)
    {
        if (magnetSize == 0 || magnetMoveSpeed == 0)
            return;
        Collider2D[] getableColiders = Physics2D.OverlapCircleAll(transform.position, magnetSize * Mathf.Max(colider2D.size.x, colider2D.size.y), LayerMask.GetMask("Getable"));
        foreach (var colider in getableColiders)
        {
            colider.transform.Translate((transform.position - colider.transform.position).normalized * magnetMoveSpeed * deltaTime);
        }
    }

    public void MoveCenter()
    {
        StartCoroutine(MoveCenterCoroutine());
    }

    IEnumerator MoveCenterCoroutine()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Vector3.zero);
        state = PlayerState.Idle;
        CheckAnimator();
        animator.Play("Run");
        while (true)
        {
            if (transform.position.x < pos.x)
                Move(Time.deltaTime);
            else
            {
                RaycastHit2D raycastHit2D = Physics2D.BoxCast((Vector2)transform.position + colider2D.offset, colider2D.size, 0, Vector2.down, jumpCheckDistance, LayerMask.GetMask("Platform"));
                if (raycastHit2D.collider != null)
                {
                    animator.Play("Die");
                    yield return new WaitForSeconds(2);
                    InGameManager.Instance.GameOverMoveCP();
                    yield break;
                }
            }
            yield return null;
        }
    }

    //���� ���� ������ �����ϴ� �Լ�
    protected virtual void CheckJumpReset()
    {
        RaycastHit2D raycastHit2D = Physics2D.BoxCast((Vector2)transform.position + colider2D.offset, colider2D.size, 0, Vector2.down, jumpCheckDistance, LayerMask.GetMask("Platform"));
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
        if (hpRemoveDuration >= hpRemoveCool)
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

    protected virtual float GetSpeedMultipler()
    {
        return 1;
    }

    //�̵��� �����ϴ� �Լ�
    protected virtual void Move(float deltaTime)
    {
        float speedMultipler = GetSpeedMultipler();
        if (boostDuration > 0)
            speedMultipler *= boostMovePercent;
        transform.Translate(deltaTime * speedMultipler * fSpeed * Vector2.right);
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
                colider2D.offset = idleColiderSize.offset;
                colider2D.size = idleColiderSize.size;
                break;
            case PlayerState.Sliding:
                colider2D.offset = slidingColiderSize.offset;
                colider2D.size = slidingColiderSize.size;
                break;
            case PlayerState.Jumping:
                colider2D.offset = jumpingColiderSize.offset;
                colider2D.size = jumpingColiderSize.size;
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

        if (collider2D.CompareTag("Boost"))
            GetBoost(collider2D.gameObject);

        if (collider2D.CompareTag("Magnet"))
            GetMagnet(collider2D.gameObject);

        if (collider2D.CompareTag("Toaster"))
            GetToaster(collider2D.gameObject);
    }

    protected virtual void GetBoost(GameObject obj)
    {
        obj.SetActive(false);
        //TODO �ν�Ʈ ����Ʈ
        boostDuration = Mathf.Max(boostDuration, itemBoostDuration);
    }

    protected virtual void GetMagnet(GameObject obj)
    {
        obj.SetActive(false);
        //TODO �ڼ� ����Ʈ
        StartCoroutine(MagnetRemove());
    }

    protected virtual void GetToaster(GameObject obj)
    {
        obj.SetActive(false);
        //TODO �ڼ� ����Ʈ
        hp += toasterHealValue;
    }

    protected virtual IEnumerator MagnetRemove()
    {
        magnetSize += itemMagnetSize;
        yield return new WaitForSeconds(itemMagnetDuration);
        magnetSize -= itemMagnetSize;
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

    protected virtual float GetDamage(float damage)
    {
        return damage * (1 - (GameManager.Instance.defenseLv / 20));
    }

    //��ֹ��� �ε����� ���
    protected virtual void HurtByBlock(Block block)
    {
        if (!hitable)
            return;
        if (boostDuration > 0)
            return;

        hitable = false;

        hp -= GetDamage(block.damage);
        block.OnHit();
        if (hp <= 0)
        {
            GameOver();
            return;
        }

        IngameUIManager.Instance.PlayerHurt();
        rigid.velocity = Vector2.zero;
        gameObject.layer = LayerMask.NameToLayer("PlayerInv");
        animator.Play("Hurt");
        spriteRenderer.DOFade(hitFadeInAlpha, hitFadeInTime).
            OnComplete(() => spriteRenderer.DOFade(1, hitFadeOutTime).SetEase(Ease.InExpo).
            OnComplete(() =>
            {
                hitable = true;
                gameObject.layer = LayerMask.NameToLayer("Player");
            }));
    }
}
