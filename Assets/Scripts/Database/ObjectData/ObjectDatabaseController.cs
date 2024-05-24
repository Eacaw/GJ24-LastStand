/*
 *  Copyright Chamber Designs 2024. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDatabaseController : MonoBehaviour
{
    [SerializeField]
    private ObjectsDatabaseSO objectDatabase;

    public ObjectData GetObjectData(string objectId)
    {
        return objectDatabase.GetObjectData(objectId);
    }

    public ObjectData GetObjectDataByName(string objectName)
    {
        return objectDatabase.GetObjectDataByName(objectName);
    }

    public List<ObjectData> GetAllObjects()
    {
        return objectDatabase.GetAllObjects();
    }

    public void AddObject(ObjectData newObject)
    {
        objectDatabase.AddObject(newObject);
    }

    public void RemoveObject(string objectId)
    {
        objectDatabase.RemoveObject(objectId);
    }

    public void UpdateObject(string objectId, ObjectData updatedData)
    {
        objectDatabase.UpdateObject(objectId, updatedData);
    }
}
