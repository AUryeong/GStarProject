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

//플레이어 능력같은게 있을 가능성이 높기에 대부분의 함수를 Virtual로 작성함
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


    [Header("플레이어 점프 관련")]
    protected float fJumpSpeed = 16;
    protected readonly float jumpCheckDistance = 0.1f;
    protected int jumpCount = 0;
    protected int jumpMaxCount = 2;

    //장애물 충돌
    protected bool hitable = true;
    protected readonly float hitDamage = 25;
    protected float hitFadeInTime = 0.1f;
    protected readonly float hitFadeInAlpha = 0.5f;
    protected float hitFadeOutTime = 0.9f;

    [Header("자석")]
    protected float magnetMoveSpeed = 10f;
    public float magnetSize = 0f;
    protected readonly float itemMagnetSize = 4;
    protected readonly float itemMagnetDuration = 5;

    [Header("부스트")]
    protected readonly float boostMovePercent = 5;
    protected readonly float itemBoostDuration = 5;
    public float boostDuration = 0;

    [Header("토스터기")]
    protected readonly float toasterHealValue = 20;
    protected readonly float toasterInvDuration = 2f;

    [Header("오븐")]
    protected readonly float ovenBoostDuration = 5;
    protected readonly float ovenHealValue = 40;

    [Header("장애물 충돌 판정")]
    public ColiderPos idleColiderSize;
    public ColiderPos slidingColiderSize;
    public ColiderPos jumpingColiderSize;

    [Header("부활 관련")]
    protected bool resurrection;//부활 했는지 체크
    private Vector2 diePos;//죽었을 당시 위치

    protected float downGameoverY = -4.5f;

    //시간 지날때마다 다는 hp
    protected float hpRemoveCool = 1;
    protected float hpRemoveDuration = 0;
    protected float hpRemoveValue = 1;

    //30퍼센트 아래일때 효과음 나왔는지 체크
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

        //좋긴한데 따로 함수로 빼서 GetIsDie()같은 bool반환형 함수를 사용해도 괜찮다 생각 , 선택은 자유
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

    //부스트 작동
    protected virtual void BoostUpdate(float deltaTime)
    {
        //if문이 너무 복잡하게 얽혀있음 안쪽 내용을 따로 함수로 빼두던지 하는것이 좋을것 또는 중복되는 내용을 생략
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
                    //ONComplete도 좋은데 역시 보기가 영... 코류틴을 이용한 딜레이 주는 방식을 어떨까 생각
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

    //살아있으면 작동하는 업데이트, 능력에서 사용 예정
    protected virtual void LiveUpdate(float deltaTime)
    {
    }

    //자석
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

    //점프 땅에 닿음을 감지하는 함수
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
            if (jumpCount == 0) // 땅에서 떨어질때는 점프 횟수 줄이기 위함
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
    
    //부활
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

    //체력이 30% 아래인지 체크
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

    //이동을 관리하는 함수
    protected virtual void Move(float deltaTime)
    {
        float speedMultipler = GetSpeedMultipler();
        if (boostDuration > 0)
            speedMultipler *= boostMovePercent;
        transform.Translate(deltaTime * speedMultipler * fSpeed * Vector2.right);
    }

    //컴퓨터일때 키보드 감지를 담당하는 함수
    protected virtual void CheckPressKey()
    {
        if (Application.platform != RuntimePlatform.Android && isControllable && GameManager.Instance.inGaming)
        {
            if (Input.GetKey(KeyCode.DownArrow)) // 점프 중에 슬라이딩 눌러도 땅 도착하자마자 슬라이딩함
                Sliding();
            else if (Input.GetKeyDown(KeyCode.UpArrow)) // 점프는 누를때만 감지함
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

    //애니메이터 관련 업데이트
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

    //버튼을 위한 public, 함수화
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
        //TODO 자석 이펙트
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

    //재료 획득
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
        // 이펙트 넣기 TODO
        InGameManager.Instance.GoldEffect(obj.transform.position);
        InGameManager.Instance.gold++;

        QusetManager.Instance.QusetUpdate(QuestType.Day, 3, 1);//일일 골드 흭득
        QusetManager.Instance.QusetUpdate(QuestType.Aweek, 3, 1);//주간 골드 흭득
        QusetManager.Instance.QusetUpdate(QuestType.Main, 4, 1);//메인 골드 흭득

        SoundManager.Instance.PlaySoundClip("SFX_InGame_Get_Coin", ESoundType.SFX);
    }

    protected virtual float GetDamage(float damage)
    {
        return damage * (1 - (GameManager.Instance.defenseLv / 20));

    }

    //장애물에 부딛혔을 경우
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
