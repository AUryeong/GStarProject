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

    [SerializeField] float ZoomOut;
    private void Awake()
    {
        StartCoroutine(SpawnIngredients(0, InsideIdx));
    }
    IEnumerator SpawnIngredients(int BreadIdx,List<int> InsideList)
    {
        int LimitValue = 0;//카메라 이동 범위 값
        SpawnBread(BreadIdx);
        yield return new WaitForSeconds(1.5f);

        GameObject Object;
        //재료 쌓기
        for (int i = 0; i < InsideList.Count; i++)
        {
            Object = Instantiate(SpawnObject, transform.position, transform.rotation);
            Object.GetComponent<SpriteRenderer>().sprite = Inside.Stats[InsideList[i]].ImageSprite;
            transform.position += Vector3.up*1f;
            if(i>=5)//중간 갔을떄 화면이 올라감
            {
                LimitValue++;
                MainCamera.transform.position += Vector3.up;
            }
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(1.5f);
        MainCamera.GetComponent<Camera>().orthographicSize += ZoomOut;
        MainCamera.GetComponent<EndingCamera>().MoveLimitValue = LimitValue;
        MainCamera.GetComponent<EndingCamera>().CameraMove = true;
        SpawnBread(BreadIdx);

        yield return null;
    }
    void SpawnBread(int BreadIdx)
    {
        GameObject Object = Instantiate(SpawnObject, transform.position, transform.rotation);
        Object.GetComponent<SpriteRenderer>().sprite = Bread.Stats[BreadIdx].ImageSprite;
        Object.transform.localScale += Vector3.right * 2;
    }
}
