using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DebugChunkReceivedMono : MonoBehaviour
{

    public byte m_chunkByteIdFilter=0;
    public PackageChunkReceived m_lastReceivedChunk;
    public PackageChunkReceived m_lastReceivedChunkWithId;

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
    public List<PackageChunkReceived> m_receivedPackage = new List<PackageChunkReceived>();

    public UnityEvent m_lastChunkReceived;

   

    public byte[] m_lastReceivedFullChunk;

    public void PushInReceivedPackage(PackageChunkReceived chunk)
    {

        m_lastReceivedChunk = chunk;
        if(chunk.m_arrayGivenId == m_chunkByteIdFilter)
            m_lastReceivedChunkWithId = chunk;

                while (m_receivedPackage.Count <= chunk.m_chunkIndex)
                {
                    m_receivedPackage.Add(new PackageChunkReceived());
                }

                if (chunk.m_chunkIndex == m_receivedPackage.Count - 1 && chunk.m_whenFrameWasSent > m_lastStartPushing)
                {

                    m_nowUtc = DateTime.UtcNow.Ticks;
                    m_lastStartPushing = chunk.m_whenFrameWasSent;
                    m_tickFromBuildToReceived = m_nowUtc - m_lastStartPushing;
                    m_tickServerSideProduction = chunk.m_whenChunkWasSent - chunk.m_whenFrameWasSent;
                    m_millisecondsServerSideProduction = (double)m_tickServerSideProduction / TimeSpan.TicksPerMillisecond;
                    m_millisecondsfromBuildToReceived = (double)m_tickFromBuildToReceived / TimeSpan.TicksPerMillisecond;
                    m_lastChunkReceived.Invoke();
                }
    }
}
