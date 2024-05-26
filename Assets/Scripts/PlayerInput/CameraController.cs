using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private int rotation = 0;

    private Vector3 rot0Pos = new Vector3(0, 13.5f, -12);
    private Vector3 rot0Rot = new Vector3(55, 0, 0);

    private Vector3 rot1Pos = new Vector3(-8.5f, 13.5f, -8.5f);
    private Vector3 rot1Rot = new Vector3(55, 45, 0);

    private Vector3 rot2Pos = new Vector3(-12, 13.5f, 0);
    private Vector3 rot2Rot = new Vector3(55, 90, 0);

    private Vector3 rot3Pos = new Vector3(-8.5f, 13.5f, 8.5f);
    private Vector3 rot3Rot = new Vector3(55, 135, 0);

    private Vector3 rot4Pos = new Vector3(0, 13.5f, 12);
    private Vector3 rot4Rot = new Vector3(55, 180, 0);

    private Vector3 rot5Pos = new Vector3(8.5f, 13.5f, 8.5f);
    private Vector3 rot5Rot = new Vector3(55, 225, 0);

    private Vector3 rot6Pos = new Vector3(12, 13.5f, 0);
    private Vector3 rot6Rot = new Vector3(55, 270, 0);

    private Vector3 rot7Pos = new Vector3(8.5f, 13.5f, -8.5f);
    private Vector3 rot7Rot = new Vector3(55, 315, 0);

    private void Start() { }

    public void RotateCameraRight()
    {
        rotation--;
        if (rotation < 0)
        {
            rotation = 7;
        }
        RotateCamera();
    }

    public void RotateCameraLeft()
    {
        rotation++;
        if (rotation > 7)
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
            case 4:
                transform.position = rot4Pos;
                transform.rotation = Quaternion.Euler(rot4Rot);
                break;
            case 5:
                transform.position = rot5Pos;
                transform.rotation = Quaternion.Euler(rot5Rot);
                break;
            case 6:
                transform.position = rot6Pos;
                transform.rotation = Quaternion.Euler(rot6Rot);
                break;
            case 7:
                transform.position = rot7Pos;
                transform.rotation = Quaternion.Euler(rot7Rot);
                break;
        }
    }
}
