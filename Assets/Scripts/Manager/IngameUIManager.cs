using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IngameUIManager : MonoBehaviour
{
    [SerializeField] GameObject PausePanel;
    [SerializeField] GameObject ExitPanel;
    [SerializeField] GameObject SettingPanel;

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
        if(yes)
        {
            SceneManager.LoadScene(2);
        }
        else
        {
            ExitPanel.SetActive(false);
        }
    }
}
