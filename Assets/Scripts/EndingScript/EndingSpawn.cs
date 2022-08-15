using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingSpawn : Singleton<EndingSpawn>
{
    [SerializeField] Ingredients Bread;
    [SerializeField] Ingredients Inside;
    [SerializeField] GameObject SpawnObject;
    public List<int> InsideIdx = new List<int>();


    [SerializeField] float ZoomOut;

    private void Start()
    {
        Spawn(0, InsideIdx);
    }
    public void Spawn(int breadIdx, List<int> insideList)
    {
        StartCoroutine(SpawnIngredients(breadIdx, insideList));
    }
    IEnumerator SpawnIngredients(int BreadIdx, List<int> InsideList)
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
            transform.position += Vector3.up;
            if (i >= 5)//중간 갔을떄 화면이 올라감
            {
                LimitValue++;
                float timer = 0;
                Vector3 CameraPos = Camera.main.transform.position;
                while (timer < 1f)
                {
                    Camera.main.transform.position = Vector3.Lerp(CameraPos, CameraPos + Vector3.up, timer);
                    timer += Time.deltaTime * 3;
                    yield return null;
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(1.5f);
        Camera.main.GetComponent<Camera>().orthographicSize += ZoomOut;
        Camera.main.GetComponent<EndingCamera>().MoveLimitValue = LimitValue;
        Camera.main.GetComponent<EndingCamera>().CameraMove = true;
        SpawnBread(BreadIdx);

        yield return null;
    }
    void SpawnBread(int BreadIdx)
    {
        GameObject Object = Instantiate(SpawnObject, transform.position, transform.rotation);
        Object.GetComponent<SpriteRenderer>().sprite = Bread.Stats[BreadIdx].ImageSprite;
        Object.GetComponent<BoxCollider2D>().size = new Vector2(0.5f, 1);
        Object.transform.localScale += Vector3.right * 3;

    }
}
