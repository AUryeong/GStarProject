using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public Ingredients Inside;
    private Vector3 cameraDistance = new Vector3(6, 2.5f, -10);
    public bool inGaming = true;
    private bool pressSliding = false;
    

    //�� �����°� ������ ���ɼ��� ���Ƽ� �ڵ� ����«
    List<int> ingredientIdxList = new List<int>();

    protected override void Awake()
    {
        if (Instance != this)
            Destroy(gameObject);
        else
            DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        if (inGaming)
        {
            CameraMove();
            CheckSliding();
        }
    }

    private void CameraMove()
    {
        Camera.main.transform.position = new Vector3(Player.Instance.transform.position.x + cameraDistance.x, cameraDistance.y, cameraDistance.z);
    }
    private void CheckSliding()
    {
        if (pressSliding)
            Player.Instance.Sliding();
    }

    public void GameOver()
    {
        inGaming = false;
        ingredientIdxList = Player.Instance.ingredients;

        //�� �Ѿ�°� ������ ���ɼ��� ���⿡ ����¥�� �ڵ� ������
        SceneManager.LoadScene("Ending");
        SceneManager.sceneLoaded += EndingSceneLoadComplete;
    }

    public void EndingSceneLoadComplete(Scene scene, LoadSceneMode loadSceneMode)
    {
        EndingSpawn.Instance.Spawn(0, ingredientIdxList);
        SceneManager.sceneLoaded -= EndingSceneLoadComplete;
    }

    //�Ʒ����� ��ư
    public void PressDownSliding()
    {
        if (inGaming)
            pressSliding = true;
    }
    public void PressUpSliding()
    {
        if (inGaming)
        {
            pressSliding = false;
            Player.Instance.ReturnToIdle();
        }
    }
    public void PressJump()
    {
        if (inGaming)
            Player.Instance.Jump();
    }
}
