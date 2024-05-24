/*
 *  Copyright Chamber Designs 2024. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Layouts
{
    private static Vector3Int zeroLocation = new Vector3Int(0, 0, 0);

    public static LayoutData get1x1Layout()
    {
        List<Vector3Int> Layout1x1 = new List<Vector3Int>();
        Layout1x1.Add(zeroLocation);
        return new LayoutData("1x1", Layout1x1);
    }

    public static LayoutData get2x2Layout()
    {
        List<Vector3Int> Layout2x2 = new List<Vector3Int>();
        Layout2x2.Add(zeroLocation);
        Layout2x2.Add(new Vector3Int(1, 0, 0));
        Layout2x2.Add(new Vector3Int(0, 0, 1));
        Layout2x2.Add(new Vector3Int(1, 0, 1));
        return new LayoutData("2x2", Layout2x2);
    }

    public static LayoutData get1x2Layout()
    {
        List<Vector3Int> Layout1x2 = new List<Vector3Int>();
        Layout1x2.Add(zeroLocation);
        Layout1x2.Add(new Vector3Int(-1, 0, 0));
        return new LayoutData("1x2", Layout1x2);
    }

    public static LayoutData get2x2LLayout()
    {
        List<Vector3Int> Layout2x2L = new List<Vector3Int>();
        Layout2x2L.Add(zeroLocation);
        Layout2x2L.Add(new Vector3Int(1, 0, 0));
        Layout2x2L.Add(new Vector3Int(0, 0, 1));
        return new LayoutData("2x2L", Layout2x2L);
    }
}

public class LayoutData
{
    public string Id { get; private set; }
    public string LayoutName { get; set; }
    public List<Vector3Int> Layout { get; set; }

    public LayoutData(string layoutName, List<Vector3Int> layout)
    {
        Id = Guid.NewGuid().ToString();
        LayoutName = layoutName;
        Layout = layout;
    }

    public bool needsRotation()
    {
        // Only need to rotate the non-symmetrical layouts
        return new List<string> { "1x2", "2x2L" }.Contains(LayoutName);
    }
}
