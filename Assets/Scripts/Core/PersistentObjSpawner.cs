using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PersistentObjSpawner : MonoBehaviour
{
    [SerializeField] GameObject persistentObjPrefab;

    // statics persists with the application rather than class instance
    static bool hasSpawned = false;

    private void Awake()
    {
        if (hasSpawned) return;
        SpawnPersistentObjects();
        hasSpawned = true;
    }

    private void SpawnPersistentObjects()
    {
        GameObject persistentObj = Instantiate(persistentObjPrefab);
        DontDestroyOnLoad(persistentObj);
    }
}
