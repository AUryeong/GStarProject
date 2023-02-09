using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class IngameUIManager : Singleton<IngameUIManager>
{
    [SerializeField] GameObject PausePanel;
    [SerializeField] GameObject ExitPanel;

    [Header("체력바")]
    private float hpSizeX = 500;
    [SerializeField] Slider hpBarSlider;
    [SerializeField] Image hpIconImage;
    [SerializeField] Sprite[] hpIconSprites;
    private bool hpIconBounce = true;
    private float hpIconBouncePower = 1.2f;
    private Vector2 hpBarShakePos;
    RectTransform _hpBarRect;
    RectTransform hpBarRect
    {
        get
        {
            if (_hpBarRect == null)
                _hpBarRect = hpBarSlider.GetComponent<RectTransform>();
            return _hpBarRect;
        }
    }
    [Header("오븐바")]
    [SerializeField] Slider OvenBar;

    [Header("재료")]
    [SerializeField] Image ingredientsIcon;
    [SerializeField] TextMeshProUGUI ingredientsCount;
    [SerializeField] Sprite ingredientsIconBlank;
    [SerializeField] Sprite ingredientsIconNeg;
    [SerializeField] Sprite ingredientsIconPos;

    [Header("설정")]
    [SerializeField] Toggle bgmToggle;
    [SerializeField] Toggle sfxToggle;
    [SerializeField] Toggle flipToggle;
    [SerializeField] GameObject settingObject;

    [Header("부활")]
    [SerializeField] GameObject resurrectionPanel;//부활 패널
    [SerializeField] TextMeshProUGUI resurrectionTimer;//광고보고 부활 타이머
    [SerializeField] Image re_timerCircle;//타이머 표시 원
    private bool resurrection;//재시작 타이머가 끝날때 부활함수 실행용
    private bool adDone;//광고를 완료했는지 확인

    [Header("재시작 타이머")]
    [SerializeField] Image timerCircle;//재시작 표시 원
    [SerializeField] TextMeshProUGUI t_reStartTimer;//재시작 타이머
    private const float restartTime = 4;
    private float f_reStartTimer;

    [SerializeField] Image onHItImage;
    private bool pressSliding = false;

    private void OnEnable()
    {
        hpBarRect.sizeDelta = new Vector2(hpSizeX * InGameManager.Instance.player.fHp / 100, hpBarRect.sizeDelta.y);
        hpBarShakePos = hpBarRect.anchoredPosition;
        UpdateOvenBar();
        hpIconImage.sprite = hpIconSprites[GameManager.Instance.maxHpLv / 10];
        SettingUpdate();
    }
    private void Update()
    {
        if (GameManager.Instance.inGaming)
        {
            CheckSliding();
            UpdateOvenBar();
            UpdateHealthBar();
        }
    }

    public void SettingSave()
    {
        SaveManager.Instance.gameData.bgmActive = bgmToggle.isOn;
        SaveManager.Instance.gameData.sfxActive = sfxToggle.isOn;
        SaveManager.Instance.gameData.uiFlip = flipToggle.isOn;
    }
    protected void SettingUpdate()
    {
        bgmToggle.isOn = SaveManager.Instance.gameData.bgmActive;
        sfxToggle.isOn = SaveManager.Instance.gameData.sfxActive;
        flipToggle.isOn = SaveManager.Instance.gameData.uiFlip;
    }
    public void PauseButton()
    {
        if (GameManager.Instance.inGaming)
        {
            PausePanel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void PlayerHurt()
    {
        Camera.main.DOShakePosition(0.1f);
        onHItImage.gameObject.SetActive(true);
        onHItImage.color = Color.red;
        onHItImage.DOFade(0, 1);
    }
    public void Continue()
    {
        if (GameManager.Instance.inGaming)
        {
            PausePanel.SetActive(false);
            Time.timeScale = 1;
        }
    }
    public void SettingButton()
    {
        StartCoroutine(C_OnOffSetting());
    }
    private IEnumerator C_OnOffSetting()
    {
        if (settingObject.activeSelf)
        {
            settingObject.transform.DOScale(Vector3.zero, 0.5f).SetUpdate(true);
            yield return new WaitForSecondsRealtime(0.5f);
            settingObject.SetActive(false);
        }
        else
        {
            settingObject.SetActive(true);
            settingObject.transform.DOScale(Vector3.one, 0.5f).SetUpdate(true);
        }
    }
    public void ExitButton()
    {
        ExitPanel.SetActive(true);
    }
    public void Exit(bool yes)
    {
        if (yes)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("Lobby");
        }
        else
        {
            ExitPanel.SetActive(false);
        }
    }

    public void UpdateHealthBar()
    {
        hpBarSlider.value = Mathf.Lerp(hpBarSlider.value, InGameManager.Instance.player.hp / InGameManager.Instance.player.fHp, Time.deltaTime * 20);
        if (InGameManager.Instance.player.hp <= 20)
        {
            hpBarRect.anchoredPosition = hpBarShakePos + Random.insideUnitCircle;
            if (hpIconBounce)
            {
                hpIconImage.rectTransform.localScale = Vector3.Lerp(hpIconImage.rectTransform.localScale, Vector3.one * hpIconBouncePower, Time.deltaTime * 5);
                if (hpIconImage.rectTransform.localScale.x >= hpIconBouncePower * 0.98f)
                    hpIconBounce = false;
            }
            else
            {
                hpIconImage.rectTransform.localScale = Vector3.Lerp(hpIconImage.rectTransform.localScale, Vector3.one, Time.deltaTime * 5);
                if (hpIconImage.rectTransform.localScale.x <= 1.02f)

                    hpIconBounce = true;
            }
        }
        else
        {
            hpBarRect.anchoredPosition = hpBarShakePos;
            hpIconImage.rectTransform.localScale = Vector3.one;
        }
    }
    public void UpdateIngredientsCount(int ingredientIdx, int count)
    {
        ingredientsCount.text = "X" + count.ToString();
        ingredientsCount.rectTransform.DOPunchScale(Vector3.one * 0.4f, 0.2f);
        ingredientsIcon.sprite = GameManager.Instance.Inside.Stats[ingredientIdx].OutlineSprite;
    }
    //아래부터 버튼

    public void PressDownSliding()
    {
        if (GameManager.Instance.inGaming && InGameManager.Instance.player.isControllable)
            pressSliding = true;
    }
    public void PressUpSliding()
    {
        if (GameManager.Instance.inGaming && InGameManager.Instance.player.isControllable)
        {
            pressSliding = false;
            InGameManager.Instance.player.ReturnToIdle();
        }
    }
    public void PressDownJump()
    {
        if (GameManager.Instance.inGaming && InGameManager.Instance.player.isControllable)
            InGameManager.Instance.player.Jump();
    }
    private void CheckSliding()
    {
        if (pressSliding)
            InGameManager.Instance.player.Sliding();
    }
    private void UpdateOvenBar()
    {
        var mapManager = InGameManager.Instance.mapManager;
        var mapData = mapManager.selectMapData;
        OvenBar.value = InGameManager.Instance.player.transform.position.x % (mapData.platformMapLength * mapManager.ovenMapSize) / (mapData.platformMapLength * mapManager.ovenMapSize);
    }
    public IEnumerator StartRestartTimer()
    {
        timerCircle.gameObject.SetActive(true);
        f_reStartTimer = restartTime;
        while (f_reStartTimer >= 0)
        {
            f_reStartTimer -= Time.deltaTime;
            t_reStartTimer.text = $"{(int)f_reStartTimer}";
            yield return null;
        }

        timerCircle.gameObject.SetActive(false);
        InGameManager.Instance.player.isControllable = true;
        if (resurrection == true)
        {
            InGameManager.Instance.player.Resurrection();
            resurrection = false;
        }

    }

    //부활패널 오픈
    public void OpenResurrection()
    {
        resurrectionPanel.SetActive(true);
        StartCoroutine(ResurrectionTimer(5));
    }
    //광고 다 시청시 실행
    public void DoneAD()
    {
        resurrectionPanel.SetActive(false);
        adDone = true;
        resurrection = true;
        StartCoroutine(StartRestartTimer());
    }
    private IEnumerator ResurrectionTimer(float time)
    {
        float timer = time;
        while(timer > 0)
        {
            timer -= Time.deltaTime;
            resurrectionTimer.text = $"{(int)timer + 1}";
            re_timerCircle.fillAmount = timer / time;
            yield return null;
        }
        if(adDone == false)
            InGameManager.Instance.GameOverMoveCP();
    }
    public void PressStartAd()
    {
        AdmobManager.Instance.ShowFrontAd();
    }
    public void PressDontRestart()
    {
        InGameManager.Instance.GameOverMoveCP();
    }
}
