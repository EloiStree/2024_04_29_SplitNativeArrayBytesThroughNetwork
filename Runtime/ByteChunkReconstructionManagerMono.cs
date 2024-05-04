using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class ByteChunkReconstructionManagerMono : MonoBehaviour
{
    public byte m_filterChunkByBytes;
    public byte[] m_chunkLastReceivedFiltered;

    public byte[] m_chunkLastReceived;


    public ReconstructionFullByteRange m_reconstruction;

    public uint m_lastChunkArrayId;
    public uint m_lastChunkId;
    public uint m_lastChunkFrame;


    public ReconstructBytesArray.EventChunkCopied m_onChunkCopiedEvent;
    public ReconstructBytesArray.EventChangeOfArraySize m_onChangeOfArraySizeEvent;
    public ReconstructBytesArray.EventNewFrameDetected m_onNewFrameDetectedEvent;
    public ReconstructBytesArray.EventChunkEndReception m_onChunkEndReceivedEvent;

    private void Awake()
    {
        foreach (var item in m_reconstruction.m_collectionOfMergeByteArray)
        {
            item.m_onChangeOfArraySizeEvent += (s,o, n)=>
            {
                if (m_onChangeOfArraySizeEvent != null)
                    m_onChangeOfArraySizeEvent(s, o, n);
            };

            item.m_onChunkCopiedEvent += (s, a,b,c) => {
                m_lastChunkArrayId = a;
                m_lastChunkFrame = b;
                m_lastChunkId = c;
                if (m_onChunkCopiedEvent != null)
                    m_onChunkCopiedEvent(s, a, b, c);
            };

            item.m_onChunkEndReceivedEvent += (s) => {
                if (m_onChunkEndReceivedEvent != null)
                    m_onChunkEndReceivedEvent(s);
            };

            item.m_onNewFrameDetectedEvent += (s, o, n) => {

                if (m_onNewFrameDetectedEvent != null)
                    m_onNewFrameDetectedEvent(s, o, n);
            };
        }
    }



    public void PushToParse(byte[] chunkOfBytes) {

        m_chunkLastReceived = chunkOfBytes;
        m_reconstruction.PushInByteChunk(chunkOfBytes);
        if (chunkOfBytes != null && chunkOfBytes.Length > 0) {
            if (m_filterChunkByBytes == chunkOfBytes[0]) { 
                m_chunkLastReceivedFiltered = chunkOfBytes;
            }
        }

    }
    
}





[System.Serializable]
public class ReconstructionFullByteRange {

    
    public ReconstructBytesArray[] m_collectionOfMergeByteArray = new ReconstructBytesArray[255];

    public void PushInByteChunk(byte[] receivedRawChunkWithHead)
    {
        if (receivedRawChunkWithHead == null)
            return;
        if (receivedRawChunkWithHead.Length < 21)
            return;
        byte arrayId = receivedRawChunkWithHead[0];
        
        ChunkByteArrayWithReconstructionId.GetMaxSizeOfArrayForChunk(receivedRawChunkWithHead, 
            out bool foundSizeMaxArray,out uint maxArraySize);
        if (foundSizeMaxArray == false)
            return;

        if (m_collectionOfMergeByteArray[arrayId] == null)
            m_collectionOfMergeByteArray[arrayId] = new ReconstructBytesArray(arrayId,(int)Math.Abs (maxArraySize));
        else
            m_collectionOfMergeByteArray[arrayId].m_arrayIdOfReconstruction = arrayId;
        m_collectionOfMergeByteArray[arrayId].PushInByteChunk(receivedRawChunkWithHead);
    }
}

[System.Serializable]
public class ReconstructBytesArray {

    public byte m_arrayIdOfReconstruction=0;
    public byte[] m_fullArrayOfMergedChunkContent;
    public NativeArray<byte> m_fullArrayAsNativePerma;
    public uint m_lastFrameReceived;
    public Action m_endReceiving;
    public ReconstructBytesArray(byte arrayIdOfReconstruction, int arrayMaxSize) {
        m_arrayIdOfReconstruction = arrayIdOfReconstruction;
        m_fullArrayOfMergedChunkContent = new byte[arrayMaxSize];
        m_fullArrayAsNativePerma = new NativeArray<byte>(m_fullArrayOfMergedChunkContent, Allocator.Persistent);
    }

    public byte[] GetFullBytesArray()
    {
        return m_fullArrayOfMergedChunkContent;
    }
    public NativeArray<byte> GetNativeArrayAndRefreshBefore()
    {
        if (m_fullArrayOfMergedChunkContent.Length != m_fullArrayAsNativePerma.Length)
        {
            if (m_fullArrayAsNativePerma == null && m_fullArrayAsNativePerma.IsCreated)
                m_fullArrayAsNativePerma.Dispose();
            m_fullArrayAsNativePerma = new NativeArray<byte>(m_fullArrayOfMergedChunkContent.Length, Allocator.Persistent);
        }    
        m_fullArrayAsNativePerma.CopyFrom(m_fullArrayOfMergedChunkContent);
        return m_fullArrayAsNativePerma;
    }

    public void PushInByteChunk( byte [] receivedRawChunkWithHead) {
        if (receivedRawChunkWithHead == null)
            return;
        if (receivedRawChunkWithHead.Length < 21)
            return;
        if (receivedRawChunkWithHead[0] != m_arrayIdOfReconstruction)
            return;

        //ChunkByteArrayWithReconstructionId.GetFramedIdOfChunkHead(receivedRawChunkWithHead,
        //    out uint frameIdCheck);

        //if (frameIdCheck < m_lastFrameReceived)
        //    return;

        ChunkByteArrayWithReconstructionId.GetHeaderOfChunkReceived(receivedRawChunkWithHead,
            out byte arrayId,
            out uint frameId,
            out uint chunkId,
            out uint arrayOffset,
            out uint arrayOffsetLength,
            out uint arrayMaxLength
            ) ;

        
        if (arrayMaxLength != m_fullArrayOfMergedChunkContent.Length) {
            int oldArraySize = m_fullArrayOfMergedChunkContent.Length;
            int maxLenghtCopy = (int) Math.Abs(arrayMaxLength);
            byte[] newArray = new byte[maxLenghtCopy];
            if (m_fullArrayOfMergedChunkContent.Length < maxLenghtCopy)
                maxLenghtCopy = m_fullArrayOfMergedChunkContent.Length;
            Array.Copy(m_fullArrayOfMergedChunkContent, newArray, maxLenghtCopy);
            m_fullArrayOfMergedChunkContent = newArray;
            NotifyChangeOfArraySize(oldArraySize, maxLenghtCopy);
        }
        Array.Copy(receivedRawChunkWithHead, 21, m_fullArrayOfMergedChunkContent, arrayOffset, arrayOffsetLength);
        

        if (frameId > m_lastFrameReceived)
        {
            uint oldFrame = m_lastFrameReceived;
            m_lastFrameReceived = frameId;
            NotifyNewFrameDetected(oldFrame, frameId);

        }

        if (chunkId == 0)
        {
            NotifyEndOfReception();
        }
        NotifyChunkCopied(arrayId, frameId, chunkId);

    }


    public delegate void EventChunkCopied(ReconstructBytesArray source, byte arrayId, uint frameId, uint chunkId);
    public delegate void EventChangeOfArraySize(ReconstructBytesArray source, int oldArraySize, int maxLenghtCopy);
    public delegate void EventNewFrameDetected(ReconstructBytesArray source, uint oldFrameId, uint newFrameId);
    public delegate void EventChunkEndReception(ReconstructBytesArray source );

    public EventChunkCopied         m_onChunkCopiedEvent;
    public EventChangeOfArraySize   m_onChangeOfArraySizeEvent;
    public EventNewFrameDetected    m_onNewFrameDetectedEvent;
    public EventChunkEndReception   m_onChunkEndReceivedEvent;

    private void NotifyChunkCopied(byte arrayId, uint frameId, uint chunkId)
    {
        if (m_onChunkCopiedEvent != null)
            m_onChunkCopiedEvent(this,arrayId, frameId, chunkId);
    }

    private void NotifyChangeOfArraySize(int oldArraySize, int maxLenghtCopy)
    {
        if(m_onChangeOfArraySizeEvent!=null)
            m_onChangeOfArraySizeEvent(this, oldArraySize, maxLenghtCopy);
    }

    /// <summary>
    /// As chunk can vary with number of player.
    /// The tool push from farest chunk to chunk id 0
    /// 0= last chunk of the new frame has been received (except udp lost package and connection speed exception)
    /// </summary>
    private void NotifyEndOfReception()
    {
        if (m_onChunkEndReceivedEvent!=null)
            m_onChunkEndReceivedEvent(this);
    }

    private void NotifyNewFrameDetected(uint oldFrameId, uint newFrameId)
    {
        if (m_onNewFrameDetectedEvent != null)
            m_onNewFrameDetectedEvent(this, oldFrameId, newFrameId);
    }
}