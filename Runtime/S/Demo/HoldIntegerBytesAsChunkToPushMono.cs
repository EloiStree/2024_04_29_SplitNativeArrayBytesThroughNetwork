using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldIntegerBytesAsChunkToPushMono : MonoBehaviour
{

    public int m_elementPerChunk;
    public int m_maxElementInArray;
    public byte m_reconstructionArrayId;

    public HoldIntegerBytesAsChunkToPush m_integersAsChunk;

    public void Awake()
    {
        AllocateMemoryAndReset();
    }

    [ContextMenu("Allocate memory and reset")]
    private void AllocateMemoryAndReset()
    {
        m_integersAsChunk = new HoldIntegerBytesAsChunkToPush(m_elementPerChunk, m_maxElementInArray, m_reconstructionArrayId );
        m_integersAsChunk.RefreshChunksFromFullByteArray();
    }
}
