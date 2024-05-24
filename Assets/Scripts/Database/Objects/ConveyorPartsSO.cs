using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]
public class ConveyorPartsSO : ScriptableObject
{
    public List<ConveyorPart> conveyorParts;
}

[Serializable]
public class ConveyorPart
{
    [field: SerializeField]
    public int Id { get; private set; }

    [field: SerializeField]
    public string Name { get; private set; }

    [field: SerializeField]
    public GameObject Prefab { get; private set; }
}
