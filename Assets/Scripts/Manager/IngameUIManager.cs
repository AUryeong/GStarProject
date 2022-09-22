using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class IngameUIManager : Singleton<IngameUIManager>
{
    [SerializeField] GameObject PausePanel;
    [SerializeField] GameObject ExitPanel;
    [SerializeField] GameObject SettingPanel;

    [SerializeField] TextMeshProUGUI ingredientsCount;
    private bool pressSliding = false;

    public void PauseButton()
    {
        if (GameManager.Instance.inGaming)
        {
            PausePanel.SetActive(true);
            Time.timeScale = 0;
        }
    }
    public void Continue()
    {
        if (GameManager.Instance.inGaming)
        {
            PausePanel.SetActive(false);
            float timer = 3;
            /*while(timer > 0 )
            {
                timer -= Time.deltaTime;
                //텍스트에 타이머 올림해서 적용
            }*/
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
        }
    }
    //아래부터 버튼

    public void UpdateIngredientsCount(int count)
    {
        ingredientsCount.text = "X" + count.ToString();
    }
    public void PressDownSliding()
    {
        if (GameManager.Instance.inGaming)
            pressSliding = true;
    }
    public void PressUpSliding()
    {
        if (GameManager.Instance.inGaming)
        {
            pressSliding = false;
            Player.Instance.ReturnToIdle();
        }
    }
    public void PressJump()
    {
        if (GameManager.Instance.inGaming)
            Player.Instance.Jump();
    }
    private void CheckSliding()
    {
        if (pressSliding)
            Player.Instance.Sliding();
    }
}
