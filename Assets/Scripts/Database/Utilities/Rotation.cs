/*
 *  Copyright Chamber Designs 2024. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Rotation
{
    public static LayoutData rotate(LayoutData layout, int rotation)
    {
        switch (layout.LayoutName)
        {
            case "1x2":
                return rotate1x2(layout, rotation);
            case "2x2L":
                return rotate2x2L(layout, rotation);
            default:
                return layout;
        }
    }

    private static LayoutData rotate1x2(LayoutData layout, int rotation)
    {
        List<Vector3Int> rotatedLayout = new List<Vector3Int>();
        switch (rotation)
        {
            case 0:
                rotatedLayout.Add(new Vector3Int(0, 0, 0));
                rotatedLayout.Add(new Vector3Int(-1, 0, 0));
                break;
            case 90:
                rotatedLayout.Add(new Vector3Int(0, 0, 0));
                rotatedLayout.Add(new Vector3Int(0, 0, 1));
                break;
            case 180:
                rotatedLayout.Add(new Vector3Int(1, 0, 0));
                rotatedLayout.Add(new Vector3Int(0, 0, 0));
                break;
            case 270:
                rotatedLayout.Add(new Vector3Int(0, 0, 0));
                rotatedLayout.Add(new Vector3Int(0, 0, -1));
                break;
            default:
                rotatedLayout = layout.Layout;
                break;
        }
        return new LayoutData(layout.LayoutName, rotatedLayout);
    }

    private static LayoutData rotate2x2L(LayoutData layout, int rotation)
    {
        List<Vector3Int> rotatedLayout = new List<Vector3Int>();
        switch (rotation)
        {
            case 0:
                rotatedLayout.Add(new Vector3Int(0, 0, 0));
                rotatedLayout.Add(new Vector3Int(1, 0, 0));
                rotatedLayout.Add(new Vector3Int(0, 0, -1));
                break;
            case 90:
                rotatedLayout.Add(new Vector3Int(-1, 0, 0));
                rotatedLayout.Add(new Vector3Int(0, 0, 0));
                rotatedLayout.Add(new Vector3Int(0, 0, -1));
                break;
            case 180:
                rotatedLayout.Add(new Vector3Int(0, 0, 1));
                rotatedLayout.Add(new Vector3Int(-1, 0, 0));
                rotatedLayout.Add(new Vector3Int(0, 0, 0));
                break;
            case 270:
                rotatedLayout.Add(new Vector3Int(0, 0, 1));
                rotatedLayout.Add(new Vector3Int(0, 0, 0));
                rotatedLayout.Add(new Vector3Int(1, 0, 0));
                break;
            default:
                rotatedLayout = layout.Layout;
                break;
        }
        return new LayoutData(layout.LayoutName, rotatedLayout);
    }
}
