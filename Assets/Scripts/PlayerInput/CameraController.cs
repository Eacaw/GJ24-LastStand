using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private int rotation = 0;

    private Vector3 rot0Pos = new Vector3(0, 13.5f, -12);
    private Vector3 rot0Rot = new Vector3(55, 0, 0);

    private Vector3 rot1Pos = new Vector3(-12, 13.5f, 0);
    private Vector3 rot1Rot = new Vector3(55, 90, 0);

    private Vector3 rot2Pos = new Vector3(0, 13.5f, 12);
    private Vector3 rot2Rot = new Vector3(55, 180, 0);

    private Vector3 rot3Pos = new Vector3(12, 13.5f, 0);
    private Vector3 rot3Rot = new Vector3(55, 270, 0);

    private void Start() { }

    public void RotateCameraRight()
    {
        rotation--;
        if (rotation < 0)
        {
            rotation = 3;
        }
        RotateCamera();
    }

    public void RotateCameraLeft()
    {
        rotation++;
        if (rotation > 3)
        {
            rotation = 0;
        }
        RotateCamera();
    }

    private void RotateCamera()
    {
        switch (rotation)
        {
            case 0:
                transform.position = rot0Pos;
                transform.rotation = Quaternion.Euler(rot0Rot);
                break;
            case 1:
                transform.position = rot1Pos;
                transform.rotation = Quaternion.Euler(rot1Rot);
                break;
            case 2:
                transform.position = rot2Pos;
                transform.rotation = Quaternion.Euler(rot2Rot);
                break;
            case 3:
                transform.position = rot3Pos;
                transform.rotation = Quaternion.Euler(rot3Rot);
                break;
        }
    }
}
