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
    protected float fSpeed = 7;
    [HideInInspector] public float fHp;

    public float hp;
    public bool isControllable = true;


    [Header("�÷��̾� ���� ����")]
    protected float fJumpSpeed = 16;
    protected readonly float jumpCheckDistance = 0.1f;
    protected int jumpCount = 0;
    protected int jumpMaxCount = 2;

    //��ֹ� �浹
    protected bool hitable = true;
    protected readonly float hitDamage = 25;
    protected float hitFadeInTime = 0.1f;
    protected readonly float hitFadeInAlpha = 0.5f;
    protected float hitFadeOutTime = 0.9f;

    [Header("�ڼ�")]
    protected float magnetMoveSpeed = 10f;
    public float magnetSize = 0f;
    protected readonly float itemMagnetSize = 4;
    protected readonly float itemMagnetDuration = 5;

    [Header("�ν�Ʈ")]
    protected readonly float boostMovePercent = 5;
    protected readonly float itemBoostDuration = 5;
    public float boostDuration = 0;

    [Header("�佺�ͱ�")]
    protected readonly float toasterHealValue = 20;
    protected readonly float toasterInvDuration = 2f;

    [Header("����")]
    protected readonly float ovenBoostDuration = 5;
    protected readonly float ovenHealValue = 40;

    [Header("��ֹ� �浹 ����")]
    public ColiderPos idleColiderSize;
    public ColiderPos slidingColiderSize;
    public ColiderPos jumpingColiderSize;

    [Header("��Ȱ ����")]
    protected bool resurrection;//��Ȱ �ߴ��� üũ
    private Vector2 diePos;//�׾��� ��� ��ġ

    protected float downGameoverY = -4.5f;

    //�ð� ���������� �ٴ� hp
    protected float hpRemoveCool = 1;
    protected float hpRemoveDuration = 0;
    protected float hpRemoveValue = 1;

    //30�ۼ�Ʈ �Ʒ��϶� ȿ���� ���Դ��� üũ
    protected bool isDangerHpSound;
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        colider2D = GetComponent<BoxCollider2D>();
    }

    protected virtual void OnEnable()
    {
        fHp = GameManager.Instance.breads.Stats[(int)type].GetHp() + GameManager.Instance.maxHpLv * 5;
        hp = fHp;

        rigid.gravityScale = 3.2f;
        fSpeed = 7;
        fJumpSpeed = 16;
    }

    protected virtual void Update()
    {
        float deltaTime = Time.deltaTime;

        //�����ѵ� ���� �Լ��� ���� GetIsDie()���� bool��ȯ�� �Լ��� ����ص� ������ ���� , ������ ����
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
                diePos = new Vector2(transform.position.x, 0);
                if (resurrection == false)
                    IngameUIManager.Instance.OpenResurrection();
                else
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
        //if���� �ʹ� �����ϰ� �������� ���� ������ ���� �Լ��� ���δ��� �ϴ°��� ������ �Ǵ� �ߺ��Ǵ� ������ ����
        if (boostDuration > 0)
        {
            boostDuration -= deltaTime;
            ParticleSystem boostEffect = InGameManager.Instance.boostEffect;
            if (boostDuration <= 0 == boostEffect.gameObject.activeSelf)
            {
                boostEffect.gameObject.SetActive(!boostEffect.gameObject.activeSelf);
                if (boostDuration <= 0)
                {
                    hitable = false;
                    //ONComplete�� ������ ���� ���Ⱑ ��... �ڷ�ƾ�� �̿��� ������ �ִ� ����� ��� ����
                    spriteRenderer.DOFade(hitFadeInAlpha, hitFadeInTime).
                        OnComplete(() => spriteRenderer.DOFade(1, hitFadeOutTime).SetEase(Ease.InExpo).
                        OnComplete(() =>
                        {
                            hitable = true;
                            gameObject.layer = LayerMask.NameToLayer("Player");
                        }));
                }
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
        {
            InGameManager.Instance.magnetEffect.gameObject.SetActive(false);
            return;
        }
        InGameManager.Instance.magnetEffect.transform.localScale = (Vector3.one * (magnetSize * Mathf.Max(idleColiderSize.size.x, idleColiderSize.size.y))) / 1.5f;
        InGameManager.Instance.magnetEffect.gameObject.SetActive(true);
        Collider2D[] getableColiders = Physics2D.OverlapCircleAll(transform.position, magnetSize * Mathf.Max(idleColiderSize.size.x, idleColiderSize.size.y), LayerMask.GetMask("Getable"));
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
        diePos = new Vector2(transform.position.x,0);

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
                RaycastHit2D raycastHit2D = Physics2D.BoxCast((Vector2)transform.position + colider2D.offset, colider2D.size * transform.localScale.y, 0, Vector2.down, jumpCheckDistance, LayerMask.GetMask("Platform"));
                if (raycastHit2D.collider != null)
                {
                    animator.Play("Die");
                    yield return new WaitForSeconds(2);
                    if (resurrection == false)
                        IngameUIManager.Instance.OpenResurrection();
                    else
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
        RaycastHit2D raycastHit2D = Physics2D.BoxCast((Vector2)transform.position + colider2D.offset, colider2D.size * transform.localScale.y, 0, Vector2.down, jumpCheckDistance, LayerMask.GetMask("Platform"));
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
    
    //��Ȱ
    public virtual void Resurrection()
    {
        transform.position = diePos;
        hp = fHp / 2;
        resurrection = true;
        isControllable = true;
        gameObject.layer = LayerMask.NameToLayer("Player");
        animator.Play("Run");

        boostDuration = Mathf.Max(boostDuration, itemBoostDuration);
        SoundManager.Instance.PlaySoundClip("SFX_InGame_Get_Boost", ESoundType.SFX);
    }

    //ü���� 30% �Ʒ����� üũ
    protected virtual void CheckDangerHp()
    {
        if (0.3 <= hp / fHp && isDangerHpSound == false)
        {
            isDangerHpSound = true;
            SoundManager.Instance.PlaySoundClip("SFX_InGame_Danger", ESoundType.SFX);
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

            QusetManager.Instance.QusetUpdate(QuestType.Day, 2, 1);
            QusetManager.Instance.QusetUpdate(QuestType.Aweek, 2, 1);
            QusetManager.Instance.QusetUpdate(QuestType.Main, 2, 1);
        }
    }

    public virtual void Jump()
    {
        if (jumpCount >= jumpMaxCount)
            return;
        SoundManager.Instance.PlaySoundClip("SFX_InGame_Jump", ESoundType.SFX);
        jumpCount++;

        QusetManager.Instance.QusetUpdate(QuestType.Day, 1, 1);
        QusetManager.Instance.QusetUpdate(QuestType.Aweek, 1, 1);
        QusetManager.Instance.QusetUpdate(QuestType.Main, 1, 1);

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
            HurtByBlock(collider2D);

        if (collider2D.CompareTag("Gold"))
        {
            GetGold(collider2D.gameObject);
        }

        if (collider2D.CompareTag("Ingredient"))
            AddIngredient(collider2D.GetComponent<Ingredient>());

        if (collider2D.CompareTag("Boost"))
            GetBoost(collider2D.gameObject);

        if (collider2D.CompareTag("Magnet"))
            GetMagnet(collider2D.gameObject);

        if (collider2D.CompareTag("Toaster"))
            GetToaster(collider2D.gameObject);

    }

    public virtual void GetOven(Animator obj)
    {
        StartCoroutine(OvenCoroutine(obj));
    }
    protected virtual IEnumerator OvenCoroutine(Animator obj)
    {
        obj.Play("In");
        isControllable = false;
        spriteRenderer.sortingLayerName = "Background";

        bool[] bools = new bool[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject boolObj = transform.GetChild(i).gameObject;
            bools[i] = boolObj.activeSelf;
            boolObj.SetActive(false);
        }

        yield return new WaitForSeconds(1.5f);
        obj.Play("Out");

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(bools[i]);
        }

        boostDuration = Mathf.Max(boostDuration, ovenBoostDuration);
        hp += ovenHealValue;
        isControllable = true;
        spriteRenderer.sortingLayerName = nameof(Player);
    }

    protected virtual void GetBoost(GameObject obj)
    {
        obj.SetActive(false);
        boostDuration = Mathf.Max(boostDuration, itemBoostDuration);
        SoundManager.Instance.PlaySoundClip("SFX_InGame_Get_Boost", ESoundType.SFX);
    }

    protected virtual void GetMagnet(GameObject obj)
    {
        obj.SetActive(false);
        //TODO �ڼ� ����Ʈ
        StartCoroutine(MagnetRemove());
        SoundManager.Instance.PlaySoundClip("SFX_InGame_Get_Magnet", ESoundType.SFX);
    }

    protected virtual void GetToaster(GameObject obj)
    {
        obj.SetActive(false);
        InGameManager.Instance.toasterEffect.gameObject.SetActive(true);
        InGameManager.Instance.toasterEffect.Play();
        gameObject.layer = LayerMask.NameToLayer("PlayerInv");
        spriteRenderer.DOFade(hitFadeInAlpha * 2, hitFadeInTime).
            OnComplete(() => spriteRenderer.DOFade(1, hitFadeOutTime * 2).SetEase(Ease.InExpo).
            OnComplete(() =>
            {
                hitable = true;
                gameObject.layer = LayerMask.NameToLayer("Player");
            }));
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

        SoundManager.Instance.PlaySoundClip("SFX_InGame_Get_Ingredient", ESoundType.SFX);
    }

    protected void GetGold(GameObject obj)
    {
        obj.SetActive(false);
        // ����Ʈ �ֱ� TODO
        InGameManager.Instance.GoldEffect(obj.transform.position);
        InGameManager.Instance.gold++;

        QusetManager.Instance.QusetUpdate(QuestType.Day, 3, 1);//���� ��� ŉ��
        QusetManager.Instance.QusetUpdate(QuestType.Aweek, 3, 1);//�ְ� ��� ŉ��
        QusetManager.Instance.QusetUpdate(QuestType.Main, 4, 1);//���� ��� ŉ��

        SoundManager.Instance.PlaySoundClip("SFX_InGame_Get_Coin", ESoundType.SFX);
    }

    protected virtual float GetDamage(float damage)
    {
        return damage * (1 - (GameManager.Instance.defenseLv / 20));

    }

    //��ֹ��� �ε����� ���
    protected virtual void HurtByBlock(Collider2D colider)
    {
        if (!hitable)
            return;
        if (boostDuration > 0)
        {
            colider.gameObject.layer = LayerMask.NameToLayer("BlockInv");
            colider.transform.DOScale(2, 1);
            colider.transform.DORotate(new Vector3(0, 0, 720), 1, RotateMode.FastBeyond360).SetRelative();
            colider.transform.DOMove(new Vector3(20, 20), 1).SetRelative().OnComplete(() =>
            {
                colider.gameObject.SetActive(false);
            });
            return;
        }

        hitable = false;

        SoundManager.Instance.PlaySoundClip("SFX_InGame_Damage", ESoundType.SFX);
        hp -= hitDamage;
        if (hp <= 0)
        {
            GameOver();
            return;
        }
        IngameUIManager.Instance.PlayerHurt();
        rigid.velocity = Vector2.zero;
        Invincibility();
    }
    protected virtual void Invincibility()
    {
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
