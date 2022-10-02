using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public Ingredients Inside;
    public bool inGaming = true;
    

    //씬 나누는게 삭제될 가능성이 높아서 코드 대충짬
    List<int> ingredientIdxList = new List<int>();

    protected override void Awake()
    {
        if (Instance != this)
            Destroy(gameObject);
        else
            DontDestroyOnLoad(gameObject);
    }


    public void GameOver(List<int> ingredients)
    {
        inGaming = false;
        ingredientIdxList = ingredients;

        //씬 넘어가는게 삭제될 가능성이 높기에 대충짜서 코드 더러움
        SceneManager.LoadScene("Ending");
        SceneManager.sceneLoaded += EndingSceneLoadComplete;
    }

    public void EndingSceneLoadComplete(Scene scene, LoadSceneMode loadSceneMode)
    {
        EndingSpawn.Instance.Spawn(0, ingredientIdxList);
        SceneManager.sceneLoaded -= EndingSceneLoadComplete;
    }

}
