using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    public event EventHandler<ObjectPoolingEventArgs> OnDisableEvent;

    private ObjectPoolingEventArgs _objectPoolingEventArgs = new ObjectPoolingEventArgs();

    public void Init(int index)
    {
        _objectPoolingEventArgs.Index = index;
    }

    private void OnDisable()
    {
        OnDisableEvent?.Invoke(this, _objectPoolingEventArgs);
    }
}
