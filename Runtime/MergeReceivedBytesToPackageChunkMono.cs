using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct PackageChunkReceived
{
    public byte m_arrayGivenId;
    public int m_frameIndex;
    public int m_chunkIndex;
    public int m_offsetInArray;
    public int m_offsetLenght;
    public long m_receivedPackageTime;
    public long m_whenFrameWasSent;
    public long m_whenBlockStartToBeBuild;
    public long m_whenChunkWasSent;
}

[System.Serializable]
public struct PackageChunkReceivedWithBytes
{
    public byte m_arrayGivenId;
    public int m_frameIndex;
    public int m_chunkIndex;
    public int m_offsetInArray;
    public int m_offsetLenght;
    public long m_receivedPackageTime;
    public long m_whenFrameWasSent;
    public long m_whenBlockStartToBeBuild;
    public long m_whenChunkWasSent;
    public byte[] m_allBytesReceived;
}



[System.Serializable]
public class LinkedByteIdToBytesArrayMono
{
    public byte m_arrayByteId;
    public AbstractByteArrayReferenceSetGetMono m_arrayReference;
}

public class MergeReceivedBytesToPackageChunkMono : MonoBehaviour
{

   
    public LinkedByteIdToBytesArrayMono[] m_source;

    public int m_lastFrame;
    public int m_lastChunkIndex;

    public long m_nowUtc;
    public long m_lastStartPushing;
    public long m_tickServerSideProduction; 
    public double m_millisecondsServerSideProduction;
    public long m_tickFromBuildToReceived;
    public double m_millisecondsfromBuildToReceived;


    public int m_currentFrameIndex;
    public int m_chunkCurrentMaxIndex;
    public List<PackageChunkReceivedWithBytes> m_receivedPackage = new List<PackageChunkReceivedWithBytes>();

    public UnityEvent<PackageChunkReceivedWithBytes> m_onChunkReceived;
    public UnityEvent m_lastChunkReceived;
    public bool m_storeChunkBytesToDebugInStruct;

    public bool m_lastContains;
    public AbstractByteArrayReferenceSetGetMono m_lastArraySelected;


    public byte[] m_lastReceivedFullChunk;
    
    public void PushIn(byte [] receivedChunk)
    {
        if(receivedChunk==null )
            return;
        m_lastReceivedFullChunk = receivedChunk; 

        ContainArrayUniqueId(receivedChunk[0], out  m_lastContains, out AbstractByteArrayReferenceSetGetMono array);
        m_lastArraySelected = array;
        if (m_lastContains == false)
            return;

        if(m_lastArraySelected == null)
            return;


        if (receivedChunk!=null && receivedChunk.Length > 16)
        {
            int copyLenght = BitConverter.ToInt32(receivedChunk, 13);
            if (copyLenght + 41 == receivedChunk.Length) 
            { 
                int indexFrame=BitConverter.ToInt32(receivedChunk, 1);
                int indexChunk= BitConverter.ToInt32(receivedChunk, 5);
                
                //if (indexFrame > m_lastFrame)
                   m_lastFrame = indexFrame;
                //if (indexChunk > m_lastChunkIndex)
                   m_lastChunkIndex = indexChunk;

                while (m_receivedPackage.Count <=indexChunk)
                {
                    m_receivedPackage.Add(new PackageChunkReceivedWithBytes());
                }
                PackageChunkReceivedWithBytes p = m_receivedPackage[indexChunk];
                p.m_arrayGivenId = receivedChunk[0];
                p.m_frameIndex = indexFrame;
                p.m_chunkIndex = indexChunk;
                p.m_offsetInArray = BitConverter.ToInt32(receivedChunk, 9);
                p.m_offsetLenght =copyLenght;
                p.m_whenFrameWasSent = BitConverter.ToInt64(receivedChunk, 17);
                p.m_whenBlockStartToBeBuild = BitConverter.ToInt64(receivedChunk, 25);
                p.m_whenChunkWasSent = BitConverter.ToInt64(receivedChunk, 33);
                p.m_receivedPackageTime = DateTime.UtcNow.Ticks;

                byte[] bytes = array.GetBytesArray();
                Buffer.BlockCopy(receivedChunk, 41, bytes, p.m_offsetInArray, p.m_offsetLenght);
                
                if (m_storeChunkBytesToDebugInStruct) { 
                    m_lastReceivedFullChunk = receivedChunk;
                    p.m_allBytesReceived = receivedChunk;
                }

                if ( indexChunk ==m_receivedPackage.Count-1 && p.m_whenFrameWasSent > m_lastStartPushing) {

                    m_nowUtc = DateTime.UtcNow.Ticks;
                    m_lastStartPushing = p.m_whenFrameWasSent;
                    m_tickFromBuildToReceived = m_nowUtc - m_lastStartPushing;
                    m_tickServerSideProduction = p.m_whenChunkWasSent - p.m_whenFrameWasSent;
                    m_millisecondsServerSideProduction = (double)m_tickServerSideProduction / TimeSpan.TicksPerMillisecond;
                    m_millisecondsfromBuildToReceived = (double)m_tickFromBuildToReceived / TimeSpan.TicksPerMillisecond;
                    m_lastChunkReceived.Invoke();
                }
                m_receivedPackage[indexChunk] = p;
                m_onChunkReceived.Invoke(p);
            }

        }
    }

    public void ContainArrayUniqueId(byte idArray, out bool contains, out AbstractByteArrayReferenceSetGetMono getSetArray)
    {
        for (int i = 0; i < m_source.Length; i++)
        {
            if (m_source[i].m_arrayByteId == idArray) {
                contains = true;
                getSetArray = m_source[i].m_arrayReference; 
                return;
            }
        }
        contains = false;
        getSetArray = null;
        return ;
    }

}
