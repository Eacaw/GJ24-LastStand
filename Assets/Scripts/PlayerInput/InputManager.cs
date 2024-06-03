using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private Camera sceneCamera;

    [SerializeField]
    private LayerMask placementLayerMask;

    [SerializeField]
    private UIDocument _uiDocument;

    public event Action OnLmb,
        OnRmb,
        OnR;
    private Vector3 lastPosition;

    private void Update()
    {
        // Click to add new object when not on UI
        if (Input.GetMouseButtonDown(0) && !IsPointerOverUI())
        {
            OnLmb?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            OnR?.Invoke();
        }
        if (Input.GetMouseButtonDown(1))
        {
            OnRmb?.Invoke();
        }
    }

    public bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    public Vector3 getMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = sceneCamera.nearClipPlane;
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, placementLayerMask))
        {
            lastPosition = hit.point;
        }
        return new Vector3(lastPosition.x, 0, lastPosition.z);
    }
}
