using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class ReceivedBytesToPackageChunkMono : MonoBehaviour {


    [Tooltip("What array id are allowed. None means all.")]
    public List<byte> m_allowedArrayId= new List<byte>();
    public PackageChunkReceived m_lastChunkReceived;
    public UnityEvent<PackageChunkReceivedWithBytes> m_onChunkReceived;

    private void Reset()
    {
        m_allowedArrayId.Add(1);
        m_allowedArrayId.Add(2);
        m_allowedArrayId.Add(3);
    }

    public byte[] m_chunkReceivedDebug;

    public void PushInBytes(byte[] receivedChunk)
    {
                m_chunkReceivedDebug = receivedChunk;

                /// Need to start with 41 header bytes 
                if (receivedChunk == null || receivedChunk.Length<42)
                    return;

                int copyLenght = BitConverter.ToInt32(receivedChunk, 13);
                /// Need to start with 41 header bytes and have exact copy lenght in the chunk
                if (copyLenght + 41 != receivedChunk.Length)
                    return;


                if ( m_allowedArrayId.Count > 0) { 

                        bool contains = false;
                        foreach (byte id in m_allowedArrayId)
                        {
                            if (receivedChunk[0] == id)
                            {
                                contains = true;
                                break;
                            }
                        }
                        if(contains == false)
                            return;
                }

                PackageChunkReceivedWithBytes p = new PackageChunkReceivedWithBytes(); 
                p.m_arrayGivenId = receivedChunk[0];
                p.m_frameIndex = BitConverter.ToInt32(receivedChunk, 1);
                p.m_chunkIndex = BitConverter.ToInt32(receivedChunk, 5);
                p.m_offsetInArray = BitConverter.ToInt32(receivedChunk, 9);
                p.m_offsetLenght = copyLenght;
                p.m_whenBlockStartToBeBuild = BitConverter.ToInt64(receivedChunk, 25);
                p.m_whenFrameWasSent = BitConverter.ToInt64(receivedChunk, 17);
                p.m_whenChunkWasSent = BitConverter.ToInt64(receivedChunk, 33);
                p.m_receivedPackageTime = DateTime.UtcNow.Ticks;
                p.m_allBytesReceived = receivedChunk;
                Copy(p, m_lastChunkReceived);
                m_onChunkReceived.Invoke(p);
    }

    private void Copy(PackageChunkReceivedWithBytes from, PackageChunkReceived to)
    {
        to.m_arrayGivenId = from.m_arrayGivenId;
        to.m_frameIndex = from.m_frameIndex;
        to.m_chunkIndex = from.m_chunkIndex;
        to.m_offsetInArray = from.m_offsetInArray;
        to.m_offsetLenght = from.m_offsetLenght;
        to.m_whenBlockStartToBeBuild = from.m_whenBlockStartToBeBuild;
        to.m_whenFrameWasSent = from.m_whenFrameWasSent;
        to.m_whenChunkWasSent = from.m_whenChunkWasSent;
        to.m_receivedPackageTime = from.m_receivedPackageTime;
    }
}
