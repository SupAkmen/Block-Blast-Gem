using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour
{
     protected static readonly Dictionary<string, PoolObject> pools = new();
     
     public GameObject prefab;

     protected Queue<GameObject> pool = new();

     private void Awake()
     {
          if (prefab != null)
          {
               SetPrefab(prefab);
          }
     }

     public void SetPrefab(GameObject newPrefab)
     {
          pools[newPrefab.name] = this;
          prefab = newPrefab;
     }

     private void OnDestroy()
     {
          pools.Clear();
     }

     protected GameObject Create()
     {
          var item = Instantiate(prefab, transform);
          item.name = prefab.name;
          return item;
     }

     private GameObject Get()
     {
          return null;
     }
     
     
}
