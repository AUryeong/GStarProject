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

    [HideInInspector] public float spawnSpeed;

    [SerializeField] float ZoomOut;
    [SerializeField] float maxZoomLimite;

    public void Spawn(int breadIdx, List<int> insideList)
    {
        StartCoroutine(MakeSandwich(breadIdx, insideList));
    }

    private float limitValue = 0;//ī�޶� �̵� ���� ��
    private int cm = 0;//�� ���� CM
    private int totalSideCount;
    IEnumerator MakeSandwich(int BreadIdx, List<int> insideList)
    {
        totalSideCount = insideList.Count;
        SpawnBread(BreadIdx, 0);//�Ʒ��� �� ����

        yield return new WaitForSeconds(1.5f);

        float totalValue = 0;

        for (int i = 0; i < insideList.Count; i++)
        {
            Stats stats = Inside.Stats[insideList[i]];

            SandWichObject = Instantiate(SpawnObject, transform.position, transform.rotation);
            SandWichObject.GetComponent<SideObject>().SettingObject(stats,i + 2);//��� ������ ���̾� ������ 2������ ����

            Vector3 upPos = Vector3.up * (stats.coliderSize * 5);
            transform.position += upPos;

            totalValue += stats.coliderSize * 5;
            if (totalValue >= 2)//�߰� ������ ȭ���� �ö�
            {
                float timer = 0;

                limitValue += stats.coliderSize * 5;

                Vector3 CameraPos = Camera.main.transform.position;
                Vector3 targetPos = CameraPos + upPos;
                cm += stats.Size;

                while (timer < 1f)
                {
                    Camera.main.transform.position = Vector3.Lerp(CameraPos, targetPos, timer);// �ݶ��̴� �����ŭ ī�޶� ���� �̵�
                    timer += Time.deltaTime * 3;
                    yield return null;
                }
            }
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(1.5f);

        Camera.main.GetComponent<Camera>().orthographicSize += ZoomOut;
        Camera.main.GetComponent<EndingCamera>().MoveLimitValue = limitValue;
        Camera.main.GetComponent<EndingCamera>().CameraMove = true;

        SpawnBread(BreadIdx, 1);//���� �� ����

        CmText(cm);
        yield return null;
    }
    private void SpawnBread(int BreadIdx, int overObject)
    {
        print(BreadIdx);
        SandWichObject = Instantiate(SpawnObject, transform.position, transform.rotation);
        SpriteRenderer spriteRenderer = SandWichObject.GetComponent<SpriteRenderer>();
        BoxCollider2D boxCollider = SandWichObject.GetComponent<BoxCollider2D>();

        spriteRenderer.sprite = Bread.Stats[BreadIdx].stackSprite[overObject];
        spriteRenderer.flipY = Bread.Stats[BreadIdx].isFlip && overObject == 1;//flip�̰� ���� ���̴� ������Ʈ�ϰ�� Flip
        spriteRenderer.sortingOrder = totalSideCount + 1;//bread

        boxCollider.size = new Vector2(1, 0.7f);

        SandWichObject.transform.localScale = new Vector3(1.2f,1); 
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
