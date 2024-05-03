using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

public class TDD_PushNativeArrayOfVector3 : MonoBehaviour
{

    public int m_nativeArraySize = 128 * 128;

    public UnityEvent<NativeArray<Vector3>> m_onPush;

    [ContextMenu("Push random array")]
    public void PushRandomArray()
    {

        NativeArray<Vector3> valuePerIndex = new NativeArray<Vector3>(m_nativeArraySize, Allocator.TempJob);
        for (int i = 0; i < valuePerIndex.Length; i++)
        {
            valuePerIndex[i] = new Vector3(R(), R(), R());
        }
        m_onPush.Invoke(valuePerIndex);
        valuePerIndex.Dispose();

    }

    private float R()
    {
        return UnityEngine.Random.value;
    }

    [ContextMenu("Push orderby array")]
    public void PushOrderByArray()
    {

        NativeArray<Vector3> valuePerIndex = new NativeArray<Vector3>(m_nativeArraySize, Allocator.TempJob);
        for (int i = 0; i < valuePerIndex.Length; i++)
        {
            valuePerIndex[i] = Vector3.one*i;
        }
        m_onPush.Invoke(valuePerIndex);
        valuePerIndex.Dispose();

    }

    [ContextMenu("Push zero array")]
    public void PushZeroArray()
    {

        NativeArray<Vector3> valuePerIndex = new NativeArray<Vector3>(m_nativeArraySize, Allocator.TempJob);
        for (int i = 0; i < valuePerIndex.Length; i++)
        {
            valuePerIndex[i] = Vector3.zero;
        }
        m_onPush.Invoke(valuePerIndex);
        valuePerIndex.Dispose();

    }
    public Vector3 m_sameValueForAll = Vector3.one;
    [ContextMenu("Push all to same value array")]
    public void PushSameValueArray()
    {

        NativeArray<Vector3> valuePerIndex = new NativeArray<Vector3>(m_nativeArraySize, Allocator.TempJob);
        for (int i = 0; i < valuePerIndex.Length; i++)
        {
            valuePerIndex[i] = m_sameValueForAll;
        }
        m_onPush.Invoke(valuePerIndex);
        valuePerIndex.Dispose();

    }
}
