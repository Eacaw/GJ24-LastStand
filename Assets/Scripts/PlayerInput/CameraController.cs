using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float panSpeed = 5f,
        scrollSpeed = 5,
        minY = 1f,
        maxY = 20f,
        rotationSpeed = 5f;

    [SerializeField]
    private Vector2 panLimit = new Vector2(11.5f, 7);

    private float horizontalRotation = -50f,
        verticalRotation = 50f;

    private float speedModifier = 3f;

    private void Start()
    {
        // Set up the initial position and rotation of the camera
        transform.position = new Vector3(5, 7.5f, -5);
        transform.rotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0);
    }

    private void Update()
    {
        Vector3 pos = transform.position;
        Quaternion rot = transform.rotation;

        if (Input.GetKey("w"))
        {
            pos += transform.forward * panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("s"))
        {
            pos -= transform.forward * panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("d"))
        {
            pos += transform.right * panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("a"))
        {
            pos -= transform.right * panSpeed * Time.deltaTime;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos += transform.forward * scroll * scrollSpeed * 100f * Time.deltaTime;

        pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);

        transform.position = pos;

        if (Input.GetMouseButton(1))
        {
            horizontalRotation += Input.GetAxis("Mouse X") * rotationSpeed;
            verticalRotation -= Input.GetAxis("Mouse Y") * rotationSpeed;
            verticalRotation = Mathf.Clamp(verticalRotation, -80f, 80f);
            transform.rotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            panSpeed *= speedModifier;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            panSpeed /= speedModifier;
        }
    }
}
