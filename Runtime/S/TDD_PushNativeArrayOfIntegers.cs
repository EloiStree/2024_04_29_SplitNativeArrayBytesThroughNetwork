using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

public class TDD_PushNativeArrayOfIntegers : MonoBehaviour
{

    public int m_nativeArraySize=128*128;

    public UnityEvent<NativeArray<int>> m_onPush;

    [ContextMenu("Push random array")]
    public void PushRandomArray()
    {

        NativeArray<int> valuePerIndex = new NativeArray<int>(m_nativeArraySize, Allocator.TempJob);
        for (int i = 0; i < valuePerIndex.Length; i++)
        {
            valuePerIndex[i] = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        }
        m_onPush.Invoke(valuePerIndex);
        valuePerIndex.Dispose();

    }
    [ContextMenu("Push orderby array")]
    public void PushOrderByArray()
    {

        NativeArray<int> valuePerIndex = new NativeArray<int>(m_nativeArraySize, Allocator.TempJob);
        for (int i = 0; i < valuePerIndex.Length; i++)
        {
            valuePerIndex[i] = i;
        }
        m_onPush.Invoke(valuePerIndex);
        valuePerIndex.Dispose();

    }

    [ContextMenu("Push zero array")]
    public void PushZeroArray()
    {

        NativeArray<int> valuePerIndex = new NativeArray<int>(m_nativeArraySize, Allocator.TempJob);
        for (int i = 0; i < valuePerIndex.Length; i++)
        {
            valuePerIndex[i] = 0;
        }
        m_onPush.Invoke(valuePerIndex);
        valuePerIndex.Dispose();

    }
    public int m_sameValueForAll = 42;
    [ContextMenu("Push all to same value array")]
    public void PushSameValueArray()
    {

        NativeArray<int> valuePerIndex = new NativeArray<int>(m_nativeArraySize, Allocator.TempJob);
        for (int i = 0; i < valuePerIndex.Length; i++)
        {
            valuePerIndex[i] = m_sameValueForAll;
        }
        m_onPush.Invoke(valuePerIndex);
        valuePerIndex.Dispose();

    }
}
