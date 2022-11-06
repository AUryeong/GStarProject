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

    [SerializeField] Image onHItImage;
    private bool pressSliding = false;


    private void OnEnable()
    {
        hpBarRect.sizeDelta = new Vector2(hpSizeX * InGameManager.Instance.player.fHp / 100, hpBarRect.sizeDelta.y);
        hpBarShakePos = hpBarRect.anchoredPosition;
        UpdateOvenBar();
        hpIconImage.sprite = hpIconSprites[GameManager.Instance.maxHpLv / 10];
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
            while (timer > 0)
            {
                timer -= Time.deltaTime;
                //텍스트에 타이머 올림해서 적용
            }
            Time.timeScale = 1;
        }
    }
    public void SettingButton()
    {
        GameManager.Instance.OnOffSetting();
    }
    public void ExitButton()
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
            UpdateOvenBar();
            UpdateHealthBar();
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
    public void PressJump()
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
        OvenBar.value = (InGameManager.Instance.player.transform.position.x % (InGameManager.Instance.platformMapLength * InGameManager.Instance.ovenMapSize)) / (InGameManager.Instance.platformMapLength * InGameManager.Instance.ovenMapSize);
    }
}
