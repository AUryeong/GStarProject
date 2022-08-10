using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingCamera : MonoBehaviour
{
    [Header("SandWhich")]
    public bool CameraMove;         //샌드위치가 다 쌓였는지 확인합니다.   
    public float MoveLimitValue;    //샌드위치가 쌓인 높이 값을 저장합니다.
    private Vector3 FirstClickPos;  //처음 터치한 위치 체크

    [Header("ZoomInOut")]
    float m_fOldToucDis = 0f;       // 터치 이전 거리를 저장합니다.
    float m_fFieldOfView = 5f;     // 카메라의 FieldOfView의 기본값을 5으로 정합니다.
    [SerializeField] private float M_ZoomSpeed;
    [SerializeField] private float P_ZoomSpeed;

    void Start()
    {
    }

    void Update()
    {
        DragMove();
        ZoomInOut();
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
        float fDis = 0f;

        // 터치가 두개이고, 두 터치중 하나라도 이동한다면 카메라의 orthographicSize를 조정합니다.
        if (nTouch == 2 && (Input.touches[0].phase == TouchPhase.Moved || Input.touches[1].phase == TouchPhase.Moved)) //Mobile
        {
            fToucDis = (Input.touches[0].position - Input.touches[1].position).sqrMagnitude;

            fDis = (fToucDis - m_fOldToucDis) * M_ZoomSpeed;

            // 이전 두 터치의 거리와 지금 두 터치의 거리의 차이를 FleldOfView를 차감합니다.
            m_fFieldOfView -= fDis;

            // 최대는 5, 최소는 20으로 더이상 증가 혹은 감소가 되지 않도록 합니다.
            m_fFieldOfView = Mathf.Clamp(m_fFieldOfView, 5.0f, 10.0f);

            // 확대 / 축소가 갑자기 되지않도록 보간합니다.
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, m_fFieldOfView, Time.deltaTime * 5);

            m_fOldToucDis = fToucDis;
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0)//PC
        {

            fToucDis = Input.GetAxis("Mouse ScrollWheel") * P_ZoomSpeed;

            m_fFieldOfView = Camera.main.orthographicSize - fToucDis;

            // 확대 / 축소가 갑자기 되지않도록 보간합니다.
            Camera.main.orthographicSize = Mathf.Clamp(m_fFieldOfView, 5.0f, 10.0f);
        }
    }
}
