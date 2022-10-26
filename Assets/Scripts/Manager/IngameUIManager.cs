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
    [SerializeField] GameObject SettingPanel;

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
    [SerializeField] GameObject StartPoint;
    [SerializeField] GameObject EndPoint;

    [Header("재료")]
    [SerializeField] Image ingredientsIcon;
    [SerializeField] TextMeshProUGUI ingredientsCount;
    [SerializeField] Sprite ingredientsIconBlank;
    [SerializeField] Sprite ingredientsIconNeg;
    [SerializeField] Sprite ingredientsIconPos;

    [SerializeField] Image onHItImage;
    private bool pressSliding = false;

    private void OnEnable()
    {
        hpBarRect.sizeDelta = new Vector2(hpSizeX * Player.Instance.fHp / 100, hpBarRect.sizeDelta.y);
        hpBarShakePos = hpBarRect.anchoredPosition;
        hpIconImage.sprite = hpIconSprites[GameManager.Instance.MaxHpLv / 10];
        UpdateOvenBar();
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
            float timer = 3;
            while(timer > 0 )
            {
                timer -= Time.deltaTime;
                //텍스트에 타이머 올림해서 적용
            }
            Time.timeScale = 1;
        }
    }
    public void SettingButton()
    {
        SettingPanel.SetActive(false);
    }
    public void ExitButton(bool yes)
    {
        ExitPanel.SetActive(true);
    }
    public void Exit(bool yes)
    {
        if (yes)
        {
            SceneManager.LoadScene(2);
        }
        else
        {
            ExitPanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (GameManager.Instance.inGaming)
        {
            CheckSliding();
            UpdateHealthBar();
        }
    }

    public void UpdateHealthBar()
    {
        hpBarSlider.value = Mathf.Lerp(hpBarSlider.value, Player.Instance.hp / Player.Instance.fHp, Time.deltaTime * 20);
        if (Player.Instance.hp <= 20)
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
    public void UpdateIngredientsCount(bool isNegative, int count)
    {
        ingredientsCount.text = "X" + count.ToString();
        ingredientsCount.rectTransform.DOPunchScale(Vector3.one * 0.4f, 0.2f);
        if (isNegative)
            ingredientsIcon.sprite = ingredientsIconNeg;
        else
            ingredientsIcon.sprite = ingredientsIconPos;
    }
    //아래부터 버튼

    public void PressDownSliding()
    {
        if (GameManager.Instance.inGaming && Player.Instance.isControllable)
            pressSliding = true;
    }
    public void PressUpSliding()
    {
        if (GameManager.Instance.inGaming && Player.Instance.isControllable)
        {
            pressSliding = false;
            Player.Instance.ReturnToIdle();
        }
    }
    public void PressJump()
    {
        if (GameManager.Instance.inGaming && Player.Instance.isControllable)
            Player.Instance.Jump();
    }
    private void CheckSliding()
    {
        if (pressSliding)
            Player.Instance.Sliding();
    }
    private void UpdateOvenBar()
    {
        OvenBar.value = Vector2.Distance(StartPoint.transform.position, Player.Instance.transform.position) / 
                        Vector2.Distance(StartPoint.transform.position, EndPoint.transform.position);
    }
}
