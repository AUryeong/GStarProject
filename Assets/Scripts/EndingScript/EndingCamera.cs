using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingCamera : MonoBehaviour
{
    [Header("SandWhich")]
    public bool CameraMove;         //������ġ�� �� �׿����� Ȯ���մϴ�.   
    public float MoveLimitValue;    //������ġ�� ���� ���� ���� �����մϴ�.
    private Vector3 FirstClickPos;  //ó�� ��ġ�� ��ġ üũ

    [Header("ZoomInOut")]
    float m_fOldToucDis = 0f;       // ��ġ ���� �Ÿ��� �����մϴ�.
    float m_fFieldOfView = 5f;     // ī�޶��� FieldOfView�� �⺻���� 5���� ���մϴ�.
    [SerializeField] private float M_ZoomSpeed;
    [SerializeField] private float P_ZoomSpeed;

    void Start()
    {
    }

    void Update()
    {
        DragMove();
        //ZoomInOut();
    }
    void DragMove()
    {
        if (CameraMove && Input.GetMouseButtonDown(0))
        {
            FirstClickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (CameraMove && Input.GetMouseButton(0))
        {
            float MousePos = FirstClickPos.y - Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
            transform.position = new Vector3(0, Mathf.Clamp(transform.position.y + MousePos, 0, MoveLimitValue), -10);
        }
    }
    void ZoomInOut()
    {
        int nTouch = Input.touchCount;
        float fToucDis = 0f;

        // ��ġ�� �ΰ��̰�, �� ��ġ�� �ϳ��� �̵��Ѵٸ� ī�޶��� orthographicSize�� �����մϴ�.
        if (nTouch == 2 && (Input.touches[0].phase == TouchPhase.Moved || Input.touches[1].phase == TouchPhase.Moved)) //Mobile
        {
            fToucDis = (Input.touches[0].position - Input.touches[1].position).sqrMagnitude;

            // ���� ��ġ �Ÿ��� ���� ���� �Ÿ��� ���ؼ� ���ݴϴ�
            m_fFieldOfView -= (fToucDis - m_fOldToucDis) * M_ZoomSpeed;

            // �ִ�� 5, �ּҴ� 10���� ���̻� ���� Ȥ�� ���Ұ� ���� �ʵ��� �մϴ�.
            m_fFieldOfView = Mathf.Clamp(m_fFieldOfView, 5.0f, 10.0f);

            // Ȯ�� / ��Ұ� ���ڱ� �����ʵ��� �����մϴ�.
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, m_fFieldOfView, Time.deltaTime * 5);

            m_fOldToucDis = fToucDis;
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0)//PC
        {

            fToucDis = Input.GetAxis("Mouse ScrollWheel") * P_ZoomSpeed;

            // ���� �� ��ġ�� �Ÿ��� ���� �� ��ġ�� �Ÿ��� ���̸� FleldOfView�� �����մϴ�.
            m_fFieldOfView = Camera.main.orthographicSize - fToucDis;
            // �ִ�� 5, �ּҴ� 10���� ���̻� ���� Ȥ�� ���Ұ� ���� �ʵ��� �մϴ�.
            m_fFieldOfView = Mathf.Clamp(m_fFieldOfView, 5.0f, 10.0f);

            // Ȯ�� / ��Ұ� ���ڱ� �����ʵ��� �����մϴ�.
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, m_fFieldOfView, Time.deltaTime * 5);
        }
    }

    public void PushUpButton()
    {
        if (CameraMove)
            StartCoroutine(C_UpButton());
    }
    IEnumerator C_UpButton()
    {
        CameraMove = false;
        Vector3 FirstPos = transform.position;
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(FirstPos, new Vector3(0, MoveLimitValue, -10), t);// �ִ�� �ö� �� �ִ� ���̷� �̵�
            yield return null;
        }
        yield return CameraMove = true;
    }
    public void PushDownButton()
    {
        if (CameraMove)
        {
            CameraMove = false;
            StartCoroutine(C_DownButton());
        }
    }
    IEnumerator C_DownButton()
    {
        Vector3 FirstPos = transform.position;
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(FirstPos, new Vector3(0, 0, -10), t);// �⺻ ī�޶� ��ġ�� �̵�
            yield return null;
        }
        yield return CameraMove = true;
    }
    public void ExitButton()
    {
        SceneManager.LoadScene("Lobby");
    }
}
