using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager<T> where T : MonoBehaviour
{
    private GameObject _prefab;
    private Transform _parent;

    private List<T> _objectPool = new List<T>(32);
    private Queue<int> _disabledObjectIndexes = new Queue<int>(32);
    private int _index = 0;

    public ObjectPoolingManager(GameObject prefab, Transform parent = null)
    {
        _prefab = prefab;

        _parent = (null == parent) ? new GameObject(_prefab.name).transform : parent;
    }

    public void CreateObjectPool(int count)
    {
        for (int i = 0; i < count; ++i)
        {
            CreateObject();
        }
    }

    public T GetObject()
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

        T objComponent = obj.GetComponent<T>();

        _objectPool.Add(objComponent);

        pooling.Init(_index);
        pooling.OnDisableEvent -= PushIndex;
        pooling.OnDisableEvent += PushIndex;

        _disabledObjectIndexes.Enqueue(_index);

        ++_index;
    }

    private void PushIndex(object sender, ObjectPoolingEventArgs args)
    {
        _disabledObjectIndexes.Enqueue(args.Index);
    }
}
