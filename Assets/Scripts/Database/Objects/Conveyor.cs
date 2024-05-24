/*
 *  Copyright Chamber Designs 2024. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class Conveyor : ObjectData
{
    [SerializeField]
    private ConveyorPartsSO conveyorDB;
    private Vector2Int conveyorPartIds;

    public Conveyor()
        : base("Conveyor", Layouts.get1x1Layout())
    {
        // Default it to flat/flat for now
        conveyorPartIds = new Vector2Int(1, 1);
    }

    public override GameObject getGameObject(Vector3 worldPos)
    {
        GameObject conveyorParent = new GameObject("ConveyorParent");
        conveyorParent.transform.position = worldPos;

        Vector3 frontPosition;
        switch (currentRotation)
        {
            case 0:
                frontPosition = worldPos + new Vector3(0, 0, 0.25f);
                break;
            case 90:
                frontPosition = worldPos + new Vector3(0.25f, 0, 0);
                break;
            case 180:
                frontPosition = worldPos + new Vector3(0, 0, -0.25f);
                break;
            case 270:
                frontPosition = worldPos + new Vector3(-0.25f, 0, 0);
                break;
            default:
                frontPosition = worldPos + new Vector3(0, 0, -0.25f);
                break;
        }
        GameObject frontPart = Instantiate(
            conveyorDB.conveyorParts[conveyorPartIds.x].Prefab,
            frontPosition,
            Quaternion.Euler(0, currentRotation, 0)
        );

        Vector3 backPosition;
        switch (currentRotation)
        {
            case 0:
                backPosition = worldPos + new Vector3(0, 0, -0.25f);
                break;
            case 90:
                backPosition = worldPos + new Vector3(-0.25f, 0, 0);
                break;
            case 180:
                backPosition = worldPos + new Vector3(0, 0, 0.25f);
                break;
            case 270:
                backPosition = worldPos + new Vector3(0.25f, 0, 0);
                break;
            default:
                backPosition = worldPos + new Vector3(0, 0, -0.25f);
                break;
        }
        GameObject backPart = Instantiate(
            conveyorDB.conveyorParts[conveyorPartIds.y].Prefab,
            backPosition,
            Quaternion.Euler(0, currentRotation, 0)
        );

        frontPart.transform.parent = conveyorParent.transform;
        frontPart.layer = LayerMask.NameToLayer("Objects");
        backPart.transform.parent = conveyorParent.transform;
        backPart.layer = LayerMask.NameToLayer("Objects");

        conveyorParent.AddComponent<ObjectHighlighter>();

        return conveyorParent;
    }
}
