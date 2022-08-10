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
        int LimitValue = 0;//ī�޶� �̵� ���� ��
        SpawnBread(BreadIdx);
        yield return new WaitForSeconds(1.5f);

        GameObject Object;
        //��� �ױ�
        for (int i = 0; i < InsideList.Count; i++)
        {
            Object = Instantiate(SpawnObject, transform.position, transform.rotation);
            Object.GetComponent<SpriteRenderer>().sprite = Inside.Stats[InsideList[i]].ImageSprite;
            transform.position += Vector3.up;
            if(i>=5)//�߰� ������ ȭ���� �ö�
            {
                LimitValue++;
                float timer = 0;
                Vector3 CameraPos = MainCamera.transform.position;
                while(timer < 1f)
                {
                    MainCamera.transform.position = Vector3.Lerp(CameraPos, CameraPos + Vector3.up, timer);
                    timer += Time.deltaTime*3;
                    yield return null;
                }
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
        Object.GetComponent<BoxCollider2D>().size = new Vector2(0.5f,1);
        Object.transform.localScale += Vector3.right * 3;
        
    }
}
