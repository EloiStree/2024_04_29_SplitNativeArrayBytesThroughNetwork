using Eloi.WatchAndDate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using UnityEngine;
using UnityEngine.Events;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

public class SplitAndPushNativeBytesFrameMono
    : MonoBehaviour
{


    [Tooltip("Set here how many user are currently registered in your game to know what chunk don't need to be sent.")]
    public int m_currentElementsInUseInGame;

    public AbstractByteArrayReferenceGetMono m_source;
    public UnityEvent<byte[]> m_onByteChunkPush;



    [Tooltip("Use reconstruct array on the receiver")]
    public byte m_arrayUniqueIdInProject;
    [Tooltip("How many element you want to send per chunk (max 65000 bytes per chunk)")]
    public int m_elementSendPerPackage ;
  

    [Header("Don't touch")]
    [Tooltip("Estimation of how many elements witll be sent")]
    int m_elementSendPerChunk;
    [Tooltip("Elements sent in previous chunks")]
    public int m_elementsSentInChunks;
    [Header("Debug")]


    [Tooltip("The Chunk start with head information")]
    int m_chunkStartByteSize =1 + 4 + 4 + 4 + 4 + 8 + 8 + 8;

    [Tooltip("Size of a chunk content")]
    public int m_bytePerChunkWithoutStart;

    [Tooltip("Size of a chunk in total")]
    public int m_bytePerChunkWithStart;

    [Tooltip("Every time a full byte array is sent is a frame")]
    public uint m_frameIndex = 0;
    [Tooltip("What is the chunk current index in the frame push sent")]
    public uint m_chunkIndex;

    [Tooltip("Compting Compute, Job system, etc. When did the game start building the frame. (Define by developer)")]
    public long m_startConstructingFrameDateTime;
    [Tooltip("During the split and push when de we start sending the current frame")]
    public long m_startSendingFrameDatetime;

    [Tooltip("During the split and push when de we start sending the current chunk of the frame")]
    public long m_startSendingChunkDatetime;
    


    private void OnValidate()
    {
        RefreshComputeValue();
    }

    private void RefreshComputeValue()
    {
        m_elementSendPerChunk = m_elementSendPerPackage ;
        //FRAME INDEX    ,  CHUNK INDEX, Block TIME A UTC NOW, Chunk TIME A UTC NOW
        m_bytePerChunkWithoutStart =  (m_elementSendPerChunk * m_source.GetSizeOfElementInByteStored());
        m_bytePerChunkWithStart = m_chunkStartByteSize + m_bytePerChunkWithoutStart;
        m_chunkToSend = m_source.GetHowManyElementMaxAreStoreInCurrentArray() / m_elementSendPerChunk;
    }



    public WatchAndDateTimeActionResult m_pushFrame;
    public WatchAndDateTimeActionResult m_pushChunk;
    public int m_chunkToSend;

    public byte[] m_pushed;

    public void SetDateTimeOfWhenTheFrameStartToBuild(DateTime time)
    {
        m_startConstructingFrameDateTime = time.Ticks;

    }
    public void SetDateTimeOfWhenTheFrameStartToBuildNow()
    {
        m_startConstructingFrameDateTime = DateTime.UtcNow.Ticks;

    }

    //Should be store out of MonoBehaviour;
    public List<ChunkOfByte> m_chunkSent = new List<ChunkOfByte>();

    [System.Serializable]
    public class ChunkOfByte {
        public ChunkOfByte(int size) { 
        
            m_lastChunkSent = new byte[size];
        }
        public byte[] m_lastChunkSent;
    }
    [ContextMenu("Push Frame")]
    public void PushFrame()
    {
        RefreshComputeValue();
        byte [] source = m_source.GetBytesArray();
        m_pushFrame.StartCounting();
        m_frameIndex++;
        m_chunkIndex=0;
        m_startSendingFrameDatetime = DateTime.UtcNow.Ticks;

        m_elementsSentInChunks = 0;
        for (int i = 0; i < m_chunkToSend; i++)
        {
           
            m_chunkIndex =(uint) i;
            int copyOffset = i * m_bytePerChunkWithoutStart;
            int copyLength = m_bytePerChunkWithoutStart;
            m_startSendingChunkDatetime = DateTime.UtcNow.Ticks;
            m_pushChunk.StartCounting();
            while (i >= m_chunkSent.Count)
                m_chunkSent.Add(new ChunkOfByte( m_bytePerChunkWithStart));

            byte[] inProcessChunk = m_chunkSent[i].m_lastChunkSent ;
            inProcessChunk[0] = m_arrayUniqueIdInProject;
            BitConverter.GetBytes(m_frameIndex).CopyTo(inProcessChunk, 1);
            BitConverter.GetBytes((uint)i).CopyTo(inProcessChunk, 5);
            BitConverter.GetBytes(copyOffset).CopyTo(inProcessChunk, 9);
            BitConverter.GetBytes(copyLength ).CopyTo(inProcessChunk, 13);
            BitConverter.GetBytes(m_startConstructingFrameDateTime>0? m_startConstructingFrameDateTime: m_startSendingFrameDatetime).CopyTo(inProcessChunk, 17);
            BitConverter.GetBytes(m_startSendingFrameDatetime ).CopyTo(inProcessChunk, 25);
            BitConverter.GetBytes(m_startSendingChunkDatetime).CopyTo(inProcessChunk, 33);
            Buffer.BlockCopy(source, copyOffset, inProcessChunk, 41, m_bytePerChunkWithoutStart);


            m_onByteChunkPush.Invoke(inProcessChunk);


            m_pushed = inProcessChunk;
            m_pushChunk.StopCounting();
            m_chunkIndex++;
          

            m_elementsSentInChunks += m_elementSendPerChunk;
            if (m_elementsSentInChunks >= m_currentElementsInUseInGame)
                break;
            if (m_elementsSentInChunks >= m_source.GetHowManyElementMaxAreStoreInCurrentArray())
                break;
        }
        m_pushFrame.StopCounting();
    }
    
}
