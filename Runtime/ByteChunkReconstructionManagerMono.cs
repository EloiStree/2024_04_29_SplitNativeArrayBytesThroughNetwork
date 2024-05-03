using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ByteChunkReconstructionManagerMono : MonoBehaviour
{

    public byte[] m_chunkLastReceived;
    public ReconstructionFullByteRange m_reconstruction;
    public void PushToParse(byte[] chunkOfBytes) {
        m_chunkLastReceived = chunkOfBytes;
        m_reconstruction.PushInByteChunk(chunkOfBytes);
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

        if (m_collectionOfMergeByteArray[arrayId] == null)
            m_collectionOfMergeByteArray[arrayId] = new ReconstructBytesArray(arrayId);
        else
            m_collectionOfMergeByteArray[arrayId].m_arrayIdOfReconstruction = arrayId;
        m_collectionOfMergeByteArray[arrayId].PushInByteChunk(receivedRawChunkWithHead);
    }
}

[System.Serializable]
public class ReconstructBytesArray {

    public byte m_arrayIdOfReconstruction=0;
    public byte[] m_fullArrayOfMergedChunkContent;
    public uint m_lastFrameReceived;

    public ReconstructBytesArray(byte arrayIdOfReconstruction) {
        m_arrayIdOfReconstruction = arrayIdOfReconstruction;

    }

    public void PushInByteChunk( byte [] receivedRawChunkWithHead) {
        if (receivedRawChunkWithHead == null)
            return;
        if (receivedRawChunkWithHead.Length < 21)
            return;
        if (receivedRawChunkWithHead[0] != m_arrayIdOfReconstruction)
            return;

        ChunkByteArrayWithReconstructionId.GetFramedIdOfChunkHead(receivedRawChunkWithHead,
            out uint frameIdCheck);

        if (frameIdCheck < m_lastFrameReceived)
            return;

        ChunkByteArrayWithReconstructionId.GetHeaderOfChunkReceived(receivedRawChunkWithHead,
            out byte arrayId,
            out uint frameId,
            out uint chunkId,
            out uint arrayOffset,
            out uint arrayOffsetLength,
            out uint arrayMaxLength
            ) ;

        if (frameId > m_lastFrameReceived) {
            NotifyNewFrameDetected(m_lastFrameReceived, frameId);
            if (chunkId == 0)
                NotifyEndOfReception();
        }
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

        try
        {
            Array.Copy(receivedRawChunkWithHead, 21, m_fullArrayOfMergedChunkContent, arrayOffset, arrayOffsetLength);
        }
        catch (Exception e) {

            Debug.Log($"{e.StackTrace}\n{ e.Message}" +
                $"\n=>{ receivedRawChunkWithHead.Length} - {m_fullArrayOfMergedChunkContent.Length}" +
                $"\n=>{ arrayId} - {frameId} - {chunkId}"+
                $"\n=>{ arrayOffset} - {arrayOffsetLength} - {arrayMaxLength}");
        }
        NotifyChunkCopied(arrayId, frameId, chunkId);

    }

    private void NotifyChunkCopied(byte arrayId, uint frameId, uint chunkId)
    {
        //throw new NotImplementedException();
    }

    private void NotifyChangeOfArraySize(int oldArraySize, int maxLenghtCopy)
    {
        //throw new NotImplementedException();
    }

    /// <summary>
    /// As chunk can vary with number of player.
    /// The tool push from farest chunk to chunk id 0
    /// 0= last chunk of the new frame has been received (except udp lost package and connection speed exception)
    /// </summary>
    private void NotifyEndOfReception()
    {
        //throw new NotImplementedException();
    }

    private void NotifyNewFrameDetected(uint oldFrameId, uint newFrameId)
    {
       // throw new NotImplementedException();
    }
}