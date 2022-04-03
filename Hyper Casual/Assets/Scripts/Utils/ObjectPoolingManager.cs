using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager
{
    private GameObject _prefab;
    private Transform _parent;

    private List<Projectile> _objectPool = new List<Projectile>(32);
    private Queue<int> _disabledObjectIndexes = new Queue<int>(32);
    private int _count = 0;

    public ObjectPoolingManager(GameObject prefab)
    {
        _prefab = prefab;

        _parent = new GameObject(_prefab.name).transform;
    }

    public void CreateObjectPool(int count)
    {
        for (int i = 0; i < count; ++i)
        {
            CreateObject();
        }
    }

    public List<Projectile> GetObjectPool()
    {
        return _objectPool;
    }

    public Projectile GetObject()
    {
        if (0 == _disabledObjectIndexes.Count)
        {
            CreateObject();
        }

        return _objectPool[_disabledObjectIndexes.Dequeue()];
    }

    private void CreateObject()
    {
        GameObject obj = GameObject.Instantiate(_prefab, _parent);

        ObjectPooling pooling = obj.AddComponent<ObjectPooling>();

        Projectile projectile = obj.GetComponent<Projectile>();

        _objectPool.Add(projectile);

        pooling.Init(_count);
        pooling.OnDisableEvent -= PushIndex;
        pooling.OnDisableEvent += PushIndex;

        _disabledObjectIndexes.Enqueue(_count);

        ++_count;
    }

    private void PushIndex(object sender, ObjectPoolingEventArgs args)
    {
        _disabledObjectIndexes.Enqueue(args.Index);
    }
}
