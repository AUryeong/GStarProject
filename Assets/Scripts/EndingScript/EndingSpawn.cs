using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class EndingSpawn : Singleton<EndingSpawn>
{
    [SerializeField] private Breads Bread;
    [SerializeField] private Ingredients Inside;
    [SerializeField] private List<Stats> stats = new List<Stats>();
    private List<GameObject> SandWichObject;//샌드위치에 있는 오브젝트들

    [SerializeField] private GameObject SpawnObject;

    [SerializeField] private TextMeshProUGUI CMTextObcject;


    [HideInInspector] public float spawnSpeed;

    [SerializeField] float ZoomOut;
    [SerializeField] float maxZoomLimite;

    private float limitValue = 0;//카메라 이동 범위 값
    private int cm = 0;//총 쌓인 CM
    private int totalSideCount;
    private int a_sideCount = 0;//재료 능력 발동할때 순서를 체크할때 사용합니다.

    public void Spawn(int breadIdx, List<int> insideList)
    {
        StartCoroutine(MakeSandwich(breadIdx, insideList));
    }

    IEnumerator MakeSandwich(int BreadIdx, List<int> insideList)
    {
        totalSideCount = insideList.Count;
        SpawnBread(BreadIdx, 0);//아래쪽 빵 스폰

        yield return new WaitForSeconds(1.5f);

        float totalValue = 0;

        for (int i = 0; i < insideList.Count; i++)
        {
            stats.Add(Inside.Stats[insideList[i]]);

            SandWichObject[i] = Instantiate(SpawnObject, transform.position, transform.rotation);
            SandWichObject[i].GetComponent<SideObject>().SettingObject(stats[i], i + 2);//재료 정보와 레이어 순서는 2번부터 시작

            Vector3 upPos = Vector3.up * (stats[i].coliderSize * 5);
            transform.position += upPos;

            totalValue += stats[i].coliderSize * 5;
            if (totalValue >= 2)//화면의 중간까지 올라왔을떄 화면이 올라감
            {
                float timer = 0;

                limitValue += stats[i].coliderSize * 5;

                Vector3 CameraPos = Camera.main.transform.position;
                Vector3 targetPos = CameraPos + upPos;
                cm += stats[i].Size;

                while (timer < 1f)
                {
                    Camera.main.transform.position = Vector3.Lerp(CameraPos, targetPos, timer);// 콜라이더 사이즈만큼 카메라 위로 이동
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

        SpawnBread(BreadIdx, 1);//위쪽 빵 스폰

        CmText(cm);
        yield return null;
    }
    private void SpawnBread(int BreadIdx, int overObject)
    {
        print(BreadIdx);
        GameObject BreadObject = Instantiate(SpawnObject, transform.position, transform.rotation);
        SpriteRenderer spriteRenderer = BreadObject.GetComponent<SpriteRenderer>();
        BoxCollider2D boxCollider = BreadObject.GetComponent<BoxCollider2D>();

        spriteRenderer.sprite = Bread.Stats[BreadIdx].stackSprite[overObject];
        spriteRenderer.flipY = Bread.Stats[BreadIdx].isFlip && overObject == 1;//flip이고 위에 쌓이는 오브젝트일결우 Flip
        spriteRenderer.sortingOrder = totalSideCount + 1;//bread

        boxCollider.size = new Vector2(1, 0.7f);

        BreadObject.transform.localScale = new Vector3(1.2f, 1);
    }
    private void UseAbility()
    {
        foreach (Stats stat in stats)
        {
            switch (stat.name)
            {
                case Ingredients.Type.Kimchi://위 아래 재료들 사이즈 50%감소
                    {
                        for (int i = 1; i > -2 && a_sideCount != 0; i--)
                            stats[a_sideCount + i].Size = stats[a_sideCount + i].Size / 2;
                        break;
                    }
                case Ingredients.Type.MintChoco://위 2개 재료 2CM감소
                    {
                        for (int i = 1; i <= 2; i++)
                        {
                            stats[a_sideCount + i].Size -= 2;
                        }
                        break;
                    }
                case Ingredients.Type.Oyster://모든 재료들 10%감소
                    {
                        for(int i = a_sideCount - 1;i >= 0;i++)
                            stats[i].Size -= stats[i].Size / 10;
                        break;
                    }
                case Ingredients.Type.Cilantro://모든 재료들 10%감소
                    {
                        for (int i = a_sideCount - 1; i >= 0; i++)
                            if(stats[i].Size > 1) stats[i].Size -= 1;
                        break;
                    }
                case Ingredients.Type.Cucumber://위에 쌓이는 재료의 사이즈만큼 점수 감소
                    {
                        stat.Size -= stats[a_sideCount + 1].Size;
                        break;
                    }
                case Ingredients.Type.PoppingCandy://지금 까지 쌓인 재료 20%증가 , 위에 5개 재료들 삭제
                    {
                        for (int i = a_sideCount - 1; i >= 0; i++)
                            stats[i].Size += stats[i].Size / 20;
                        for (int i = 1; i < 10; i++)
                        { 
                            stats.Remove(stats[a_sideCount + 1]);
                            SandWichObject.Remove(SandWichObject[a_sideCount + 1]);
                        }
                        break;
                    }
            }
            a_sideCount++;
        }
    }
    private void CmText(int CM)
    {
        CMTextObcject.gameObject.SetActive(true);
        CMTextObcject.text = $"와우!! 무려 {CM}Cm를 쌓았습니다!!";
    }

    public void ExitEndingScene()
    {
        //TODO 번호별로 또는 string별로 씬을 가지고 있는 또는 반환하는 함수를 만들어두는것을 추천 매직넘버 또는 string은 실수가 생길확률이 높음
        SceneManager.LoadScene(0);
    }

}
