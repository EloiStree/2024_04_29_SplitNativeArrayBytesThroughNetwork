using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugRangeChunkableBytesArrayMono : MonoBehaviour
{

    public AbstractByteArrayReferenceGetMono m_chunkByteArray;

    public int m_startIndex = 0;
    public int m_endIndex = 100;

    public byte[] m_array;


    [ContextMenu("Refresh")]
    public void Refresh()
    {

        byte[] reference = m_chunkByteArray.GetBytesArray();
        if (m_array.Length != (m_endIndex - m_startIndex))
        {
            m_array = new byte[m_endIndex - m_startIndex];
        }
        Buffer.BlockCopy(reference, m_startIndex, m_array, 0, m_endIndex - m_startIndex);
    }
    [ContextMenu("Set Random Bytes")]
    public void SetRandomBytes()
    {

        byte[] reference = m_chunkByteArray.GetBytesArray();

        for (int i = m_startIndex; i < m_endIndex; i++)
        {
            reference[i] = (byte)UnityEngine.Random.Range(0, 255);
        }
        Refresh();
    }
    [ContextMenu("Set Order Bytes")]
    public void SetOrderBytes()
    {

        byte[] reference = m_chunkByteArray.GetBytesArray();

        for (int i = 0; i < (m_endIndex - m_startIndex); i++)
        {
            reference[m_startIndex+ i] = (byte)(i % 255);
        }
        Refresh();

    }


}
