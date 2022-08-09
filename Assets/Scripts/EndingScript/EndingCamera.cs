using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingCamera : MonoBehaviour
{
    public bool CameraMove;
    public float MoveLimitValue;

    private Vector3 FirstClickPos;
    void Start()
    {
        Debug.Log("adas");

    }

    void Update()
    {
        if(CameraMove&& Input.GetMouseButtonDown(0))
        {
            FirstClickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if(CameraMove&&Input.GetMouseButton(0))
        {
            Vector3 MousePos = new Vector3(0, FirstClickPos.y - Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);

            if(MousePos.y > 0 && transform.position.y <= MoveLimitValue)
                transform.position += new Vector3(0, MousePos.y, 0);
            else if(MousePos.y < 0 && transform.position.y >= 0)
                transform.position += new Vector3(0, MousePos.y, 0); 

        }
    }
  
}
