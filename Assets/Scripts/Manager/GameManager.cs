using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private Vector3 cameraDistance = new Vector3(6, 2.5f, -10);
    private bool pressSliding = false;
    private void Update()
    {
        CameraMove();
        CheckSliding();
    }

    private void CameraMove()
    {
        Camera.main.transform.position = new Vector3(Player.Instance.transform.position.x + cameraDistance.x, cameraDistance.y, cameraDistance.z);
    }
    private void CheckSliding()
    {
        if (pressSliding)
            Player.Instance.Sliding();
    }

    public void PressDownSliding()
    {
        pressSliding = true;
    }
    public void PressUpSliding()
    {
        pressSliding = false;
        Player.Instance.ReturnToIdle();
    }

    public void PressJump()
    {
        Player.Instance.Jump();
    }
}
