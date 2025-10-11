using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;

public class QuestObjectRegistry : MonoBehaviour
{
    // Реестр объектов - нужен для того что-бы можно было обращаться к существующим объектам на основе их id
    [SerializeField] private List<ObjectRecord> _objectRegistry;
    public static QuestObjectRegistry Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public GameObject GetObject(string name)
    {
        foreach (var item in _objectRegistry)
        {
            if (item.name == name) return item.linkedObject;
        }
        return null;
    }

    public bool IsObjectInRegistry(string name)
    {
        return GetObject(name) != null;
    }

    public void AddObject(string name, GameObject addedObject)
    {
        _objectRegistry.Add(new ObjectRecord(name, addedObject));
    }
}

[Serializable]
public struct ObjectRecord
{
    public string name;
    public GameObject linkedObject;

    public ObjectRecord(string newName, GameObject newLinkedOnject)
    {
        name = newName;
        linkedObject = newLinkedOnject;
    }
}
