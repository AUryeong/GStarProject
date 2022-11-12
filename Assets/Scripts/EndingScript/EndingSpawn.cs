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

    [HideInInspector] public float spawnSpeed;

    [SerializeField] float ZoomOut;
    [SerializeField] float maxZoomLimite;

    public void Spawn(int breadIdx, List<int> insideList)
    {
        StartCoroutine(MakeSandwich(breadIdx, insideList));
    }

    private float LimitValue = 0;//ī�޶� �̵� ���� ��
    private int CM = 0;//�� ���� CM
    IEnumerator MakeSandwich(int BreadIdx, List<int> insideList)
    {

        SpawnBread(BreadIdx, 0);//�Ʒ��� �� ����

        yield return new WaitForSeconds(1.5f);

        for (int i = 0; i < insideList.Count; i++)
        {
            Stats stats = Inside.Stats[insideList[i]];
            Debug.Log(stats.name);

            SandWichObject = Instantiate(SpawnObject, transform.position, transform.rotation);
            SandWichObject.GetComponent<SideObject>().SettingObject(stats);

            transform.position += Vector3.up * (stats.coliderSize * 5);
            if (i >= 5)//�߰� ������ ȭ���� �ö�
            {
                float timer = 0;

                LimitValue += stats.coliderSize;

                Vector3 CameraPos = Camera.main.transform.position;
                CM += stats.Size;

                while (timer < 1f)
                {
                    Camera.main.transform.position = Vector3.Lerp(CameraPos, CameraPos + Vector3.up * stats.coliderSize, timer);// �ݶ��̴� �����ŭ ī�޶� ���� �̵�
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

        SpawnBread(BreadIdx, 1);//���� �� ����

        CmText(CM);
        yield return null;
    }
    private void SpawnBread(int BreadIdx, int idx)
    {
        SandWichObject = Instantiate(SpawnObject, transform.position, transform.rotation);
        SandWichObject.GetComponent<SpriteRenderer>().sprite = Bread.Stats[BreadIdx].stackSprite[idx];
        SandWichObject.GetComponent<BoxCollider2D>().size = new Vector2(0.5f, 1);
        SandWichObject.transform.localScale += Vector3.right * 2;
    }

    private void CmText(int CM)
    {
        CMTextObcject.gameObject.SetActive(true);
        CMTextObcject.text = $"�Ϳ�!! ���� {CM}Cm�� �׾ҽ��ϴ�!!";
    }

    public void ExitEndingScene()
    {
        SceneManager.LoadScene(0);
    }

}
