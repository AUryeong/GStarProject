using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class EndingSpawn : Singleton<EndingSpawn>
{
    [SerializeField] private Breads Bread; //�� ��ũ���ͺ�
    [SerializeField] private Ingredients Inside; // ��� ��ũ���ͺ�
    [SerializeField] private List<Stats> stats = new List<Stats>();//���� ��Ḧ ����Ҷ� ����մϴ�.

    private List<GameObject> SandWichObject = new List<GameObject>();//������ġ�� �ִ� ������Ʈ��

    [SerializeField] private GameObject SpawnObject;

    [SerializeField] private TextMeshProUGUI CMTextObcject;

    [SerializeField] float ZoomOut;
    [SerializeField] float maxZoomLimite;

    [HideInInspector] public float spawnSpeed;

    private float limitValue = 0;//ī�޶� �̵� ���� ��
    private int cm = 0;//�� ���� CM
    private int totalSideCount;
    private int a_sideCount = 0;//��� �ɷ� �ߵ��Ҷ� ������ üũ�Ҷ� ����մϴ�.

    public void Spawn(int breadIdx, List<int> insideList)
    {
        StartCoroutine(MakeSandwich(breadIdx, insideList));
    }

    IEnumerator MakeSandwich(int BreadIdx, List<int> insideList)
    {
        totalSideCount = insideList.Count;
        SpawnBread(BreadIdx, 0);//�Ʒ��� �� ����

        yield return new WaitForSeconds(1.5f);

        float totalValue = 0;

        for (int i = 0; i < totalSideCount; i++)
        {
            stats.Add(Inside.Stats[insideList[i]]);

            SandWichObject.Add(Instantiate(SpawnObject, transform.position, transform.rotation));
            SandWichObject[i].GetComponent<SideObject>().SettingObject(stats[i], i + 2);//��� ������ ���̾� ������ 2������ ����

            Vector3 upPos = Vector3.up * (stats[i].coliderSize * 5);
            transform.position += upPos;

            totalValue += stats[i].coliderSize * 5;
            if (totalValue >= 2)//ȭ���� �߰����� �ö������ ȭ���� �ö�
            {
                float timer = 0;

                limitValue += stats[i].coliderSize * 5;

                Vector3 CameraPos = Camera.main.transform.position;
                Vector3 targetPos = CameraPos + upPos;
                cm += (int)stats[i].Size;

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
        StartCoroutine(UseAbility());

        CmText(cm);
        yield return null;
    }
    private void SpawnBread(int BreadIdx, int overObject)
    {
        GameObject BreadObject = Instantiate(SpawnObject, transform.position, transform.rotation);
        SpriteRenderer spriteRenderer = BreadObject.GetComponent<SpriteRenderer>();
        BoxCollider2D boxCollider = BreadObject.GetComponent<BoxCollider2D>();

        spriteRenderer.sprite = Bread.Stats[BreadIdx].stackSprite[overObject];
        spriteRenderer.flipY = Bread.Stats[BreadIdx].isFlip && overObject == 1;//isFlip�� Ʈ���̰� ���� ���̴� ������Ʈ�ϰ�� Flip
        spriteRenderer.sortingOrder = totalSideCount + 1;//bread

        boxCollider.size = new Vector2(1, 0.7f);

        BreadObject.transform.localScale = new Vector3(1.2f, 1);
    }
    private IEnumerator UseAbility()
    {
        foreach (Stats stat in stats)
        {
            //                                                                    Z���� -10���� ����
            Vector3 sideObjPos = SandWichObject[a_sideCount].transform.position + Vector3.forward *-10;
            Camera.main.transform.position = sideObjPos;
            switch (stat.name)
            {
                case Ingredients.Type.Kimchi://�� �Ʒ� ���� ������ 50%����
                    {
                        for (int i = 1; i > -2 && a_sideCount != 0; i--)
                            stats[a_sideCount + i].Size = stats[a_sideCount + i].Size / 2;
                        break;
                    }
                case Ingredients.Type.MintChoco://�� 2�� ��� 2CM����
                    {
                        for (int i = 1; i <= 2; i++)
                        {
                            stats[a_sideCount + i].Size -= 2;
                        }
                        break;
                    }
                case Ingredients.Type.Oyster://��� ���� 10%����
                    {
                        for(int i = totalSideCount - 1;i >= 0;i++)
                            stats[i].Size -= stats[i].Size / 10;
                        break;
                    }
                case Ingredients.Type.Cilantro://��� ���� 10%����
                    {
                        for (int i = a_sideCount - 1; i >= 0; i++)
                            if(stats[i].Size > 1) stats[i].Size -= 1;
                        break;
                    }
                case Ingredients.Type.Cucumber://���� ���̴� ����� �����ŭ ���� ����
                    {
                        stat.Size -= stats[a_sideCount + 1].Size;
                        break;
                    }
                case Ingredients.Type.PoppingCandy://���� ���� ���� ��� 20%���� , ���� 5�� ���� ����
                    {
                        for (int i = a_sideCount - 1; i >= 0; i++)
                            stats[i].Size += stats[i].Size / 20;
                        for (int i = 1; i < 5 && totalSideCount < a_sideCount; i++)
                        { 
                            stats.Remove(stats[a_sideCount + 1]);
                            SandWichObject.Remove(SandWichObject[a_sideCount + 1]);
                            totalSideCount--;
                        }
                        break;
                    }
            }
            a_sideCount++;
            yield return new WaitForSeconds(0.5f);
        }
    }
    private void CmText(int CM)
    {
        CMTextObcject.gameObject.SetActive(true);
        CMTextObcject.text = $"�Ϳ�!! ���� {CM}Cm�� �׾ҽ��ϴ�!!";
    }

    public void ExitEndingScene()
    {
        //TODO ��ȣ���� �Ǵ� string���� ���� ������ �ִ� �Ǵ� ��ȯ�ϴ� �Լ��� �����δ°��� ��õ �����ѹ� �Ǵ� string�� �Ǽ��� ����Ȯ���� ����
        SceneManager.LoadScene(0);
    }

}
