using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class EndingSpawn : Singleton<EndingSpawn>
{
    [SerializeField] Breads Bread;
    [SerializeField] Ingredients Inside;
    [SerializeField] GameObject SpawnObject;
    [SerializeField] TextMeshProUGUI CMTextObcject;
    private GameObject SandWichObject;
    public List<int> InsideIdx = new List<int>();


    [SerializeField] float ZoomOut;

    private void Start()
    {
    }
    public void Spawn(int breadIdx, List<int> insideList)
    {
        StartCoroutine(MakeSandwich(breadIdx, insideList));
    }
    IEnumerator MakeSandwich(int BreadIdx, List<int> InsideList)
    {
        int LimitValue = 0;//카메라 이동 범위 값
        int CM = 0;
        SpawnBread(BreadIdx);
        yield return new WaitForSeconds(1.5f);

        //재료 쌓기
        for (int i = 0; i < InsideList.Count; i++)
        {
            SandWichObject = Instantiate(SpawnObject, transform.position, transform.rotation);
            SandWichObject.GetComponent<SpriteRenderer>().sprite = Inside.Stats[InsideList[i]].SandwichSprite;
            transform.position += Vector3.up;
            if (i >= 5)//중간 갔을떄 화면이 올라감
            {
                LimitValue++;
                float timer = 0;
                Vector3 CameraPos = Camera.main.transform.position;
                CM += Inside.Stats[InsideList[i]].Size;
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
        CmText(CM);
        yield return null;
    }
    void SpawnBread(int BreadIdx)
    {
        SandWichObject = Instantiate(SpawnObject, transform.position, transform.rotation);
        SandWichObject.GetComponent<SpriteRenderer>().sprite = Bread.Stats[BreadIdx].stackSprite;
        SandWichObject.GetComponent<BoxCollider2D>().size = new Vector2(0.5f, 1);
        SandWichObject.transform.localScale += Vector3.right * 2;
    }
    public void ExitEndingScene()
    {
        SceneManager.LoadScene(0);
    }
    void CmText(int CM)
    {
        CMTextObcject.gameObject.SetActive(true);
        CMTextObcject.text = $"와우!! 무려 {CM}Cm를 쌓았습니다!!";
    }
}
