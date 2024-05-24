using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]
public class ObjectsDatabaseSO : ScriptableObject
{
    public List<ObjectData> objectsData;

    public ObjectData GetObjectData(string objectId)
    {
        return objectsData.Find(data => data.Id == objectId);
    }

    public ObjectData GetObjectDataByName(string objectName)
    {
        return objectsData.Find(data => data.ObjectName == objectName);
    }

    public List<ObjectData> GetAllObjects()
    {
        return objectsData;
    }

    public void AddObject(ObjectData newObject)
    {
        objectsData.Add(newObject);
    }

    public void RemoveObject(string objectId)
    {
        objectsData.Remove(GetObjectData(objectId));
    }

    public void UpdateObject(string objectId, ObjectData updatedData)
    {
        var index = objectsData.FindIndex(data => data.Id == objectId);
        objectsData[index] = updatedData;
    }
}
