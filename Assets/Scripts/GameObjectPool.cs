using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool:MonoBehaviour
{
    private int maxPoolCount;
    private List<GameObject> objectPrefabs;
    public List<Queue<GameObject>> objects;

    public bool canSpawnObject;

    public void Initialize(List<GameObject> prefabs, int maxCount)
    {
        objectPrefabs = prefabs;
        maxPoolCount = maxCount;
        objects = new List<Queue<GameObject>>(objectPrefabs.Count);
        for (int i = 0; i < objects.Capacity; i++)
        {
            var q = new Queue<GameObject>();
            for (int j = 0; j < maxPoolCount; j++)
            {
                var obj = Instantiate(objectPrefabs[i]);
                obj.SetActive(false);
                q.Enqueue(obj);
                canSpawnObject = true;
            }
            objects.Add(q);
        }
    }

    // 최대 카운트보다 높은 경우도 추가할 수 있도록 느슨한 제한 적용
    public GameObject GetObject(int idx)
    {
        if (0 <= idx && idx < objects.Count && objects[idx].Count > 0)
        {
            var obj = objects[idx].Dequeue();
            obj.SetActive(true);
            return obj;
        }
        return null;
    }

    public void ReturnObject(int idx, GameObject obj)
    {
        if (0 <= idx && idx < objects.Count)
        {
            if (objects[idx].Count >= maxPoolCount)
            {
                Destroy(obj);
            }
            else
            {
                obj.SetActive(false);
                obj.transform.parent = null;
                objects[idx].Enqueue(obj);
            }
        }
    }
}
