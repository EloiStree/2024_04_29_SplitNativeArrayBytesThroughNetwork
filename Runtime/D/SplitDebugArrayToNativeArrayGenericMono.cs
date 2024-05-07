using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

public abstract  class SplitDebugArrayToNativeArrayGenericMono<T> : MonoBehaviour where T:struct
{
    public T []  m_arrayToPush;

    public UnityEvent<NativeArray<T>> m_onNativeArrayPushed;


    [ContextMenu("Push as temp native array")]
    public void PushNativeArray() {
        NativeArray<T> temp = new NativeArray<T>(m_arrayToPush, Allocator.TempJob);
        m_onNativeArrayPushed.Invoke(temp);
        temp.Dispose();
    }

    public void Randomized() {

        for (int i = 0; i < m_arrayToPush.Length; i++)
        {
            m_arrayToPush[i] = GetRandomValue();
        }
    }

    public abstract T GetRandomValue();
}


public abstract class SplitDebugNativeArrayToArrayGenericMono<T> : MonoBehaviour where T : struct
{
    public T[] m_arrayToPush;


    public void PushNativeArray(NativeArray<T> value)
    {
        m_arrayToPush = value.ToArray();
    }
    public void PushNativeArray(T[] value)
    {
        m_arrayToPush = value;
    }

}
