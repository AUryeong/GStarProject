using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingSpawn : MonoBehaviour
{
    [SerializeField] Ingredients Bread;
    [SerializeField] Ingredients Inside;
    [SerializeField] GameObject SpawnObject;
    [SerializeField] List<int> InsideIdx = new List<int>();
    [SerializeField] Camera MainCamera;
    
    private void Awake()
    {
        StartCoroutine(SpawnIngredients(1, InsideIdx));
    }
    IEnumerator SpawnIngredients(int BreadIdx,List<int> InsideList)
    {
        GameObject Object = Instantiate(SpawnObject, transform.position, transform.rotation);
        Object.GetComponent<SpriteRenderer>().sprite = Bread.ImageSprite[BreadIdx];
        Object.transform.localScale += Vector3.right*2;
        yield return new WaitForSeconds(1.5f);

        for (int i = 0; i < InsideList.Count; i++)
        {
            Object = Instantiate(SpawnObject, transform.position, transform.rotation);
            Object.GetComponent<SpriteRenderer>().sprite = Inside.ImageSprite[InsideList[i]];
            transform.position += Vector3.up*1f;
            /*if(i>5)//중간 갔을떄 화면이 올라감
                MainCamera.transform.position += Vector3.up * 1f;*/
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(1.5f);
        /*MainCamera.GetComponent<Camera>().orthographicSize += MainCamera.transform.position.y;*/
        Object = Instantiate(SpawnObject, transform.position, transform.rotation);
        Object.GetComponent<SpriteRenderer>().sprite = Bread.ImageSprite[BreadIdx];
        Object.transform.localScale += Vector3.right*2;

        yield return null;
    }
}
