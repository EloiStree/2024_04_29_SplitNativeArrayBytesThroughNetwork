using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using Eloi.WatchAndDate;
using UnityEngine.Events;
using System.Net.Sockets;
using System.Linq;

public class Demo_ParsingToChunkFromArrayMono : MonoBehaviour
{
    public int m_elementPerChunk;
    public byte m_arrayReconstructionId = 3;
    public WatchAndDateTimeActionResult m_processEstimationTime;
    public PrimitiveStructToParseSample[] m_elementsToConvert;
    public DemoHoldBytesForChunkGenericElement m_parserSent;


    public WatchAndDateTimeActionResult m_bytePush;

    public UnityEvent<byte[]> m_onPush;

    private void Awake()
    {
        Refresh();
        //  m_parserSent.SetWithJob(m_elementToConvert);
    }

    [ContextMenu("Randomized")]
    public void Randomize()
    {
        Randomize(ref m_elementsToConvert);
    }

    [ContextMenu("Job Parsing")]
    public void JobParsing()
    {

        m_parserSent.SetWithJob(m_elementsToConvert);
    }
    [ContextMenu("Test Random Job Parsing")]
    public void RandomJobParsing()
    {
        Refresh();
        Randomize(ref m_elementsToConvert);
        m_processEstimationTime.StartCounting();
        m_parserSent.SetWithJob(m_elementsToConvert);
        m_parserSent.RefreshChunksFromFullByteArray();
        m_processEstimationTime.StopCounting();

        m_bytePush.StartCounting();
        string targetIpAddress = "192.168.0.1";
        int targetPort = 1234;

        // Create a UDP client
        using (UdpClient client = new UdpClient())
        {
            
                byte[] b = new byte[41 + m_parserSent.m_chunkByteSize];
                for (int i = 0; i < m_parserSent.m_numberOfChunk; i++)
                {
                    Buffer.BlockCopy(m_parserSent.m_groupOfChunkArray[i].m_chunkArray, 0, b, 41, m_parserSent.m_chunkByteSize);
                    m_onPush.Invoke(b);
                    client.Send(b, b.Length, targetIpAddress, targetPort);

                }
        }
       

        m_bytePush.StopCounting();
    }
    [ContextMenu("Test Random Job B")]
    public void RandomJobParsingB()
    {
        DemoHoldBytesForChunkGenericElement test = new DemoHoldBytesForChunkGenericElement(m_elementPerChunk, m_elementsToConvert.Length,
            m_arrayReconstructionId);
        Randomize(ref m_elementsToConvert);
        m_processEstimationTime.StartCounting();
        test.SetWithJob(m_elementsToConvert);
        test.RefreshChunksFromFullByteArray();
        m_processEstimationTime.StopCounting();
    }
    [ContextMenu("Test Random Job C")]
    public void RandomJobParsingC()
    {
        DemoHoldBytesForChunkGenericElement test = new DemoHoldBytesForChunkGenericElement(m_elementPerChunk, m_elementsToConvert.Length,
            m_arrayReconstructionId);
        Randomize(ref m_elementsToConvert);
        m_processEstimationTime.StartCounting();
        NativeArray<PrimitiveStructToParseSample> temp = new NativeArray<PrimitiveStructToParseSample>(m_elementsToConvert, Allocator.TempJob);
        test.SetWithJob(temp);
        test.RefreshChunksFromFullByteArray();
        temp.Dispose();
        m_processEstimationTime.StopCounting();
    }
  
    private void Randomize(ref PrimitiveStructToParseSample[] m_elementToConvert)
    {
        for (int i = 0; i < m_elementToConvert.Length; i++)
        {
            Randomize(ref m_elementToConvert[i]);
        }
    }

    private void Randomize(ref PrimitiveStructToParseSample item)
    {
        item.m_byte = (byte)UnityEngine.Random.Range(0, byte.MaxValue);
        item.m_sbyte = (sbyte)UnityEngine.Random.Range(0, byte.MaxValue);
        item.m_int  = (int)UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        item.m_uint = (uint)UnityEngine.Random.Range(uint.MinValue, uint.MaxValue);
        item.m_long = (long)UnityEngine.Random.Range(long.MinValue, long.MaxValue);
        item.m_ulong = (ulong)UnityEngine.Random.Range(ulong.MinValue, ulong.MaxValue);
        item.m_short = (short)UnityEngine.Random.Range(short.MinValue, short.MaxValue);
        item.m_ushort = (ushort)UnityEngine.Random.Range(ushort.MinValue, ushort.MaxValue);
        item.m_float = (float)UnityEngine.Random.Range(float.MinValue, float.MaxValue);
        item.m_double = (double)UnityEngine.Random.Range(float.MinValue, float.MaxValue);
        item.m_char0= (char)UnityEngine.Random.Range(40,128);
        item.m_char1 = (char)UnityEngine.Random.Range(40, 128);    
    }

    [ContextMenu("Refresh")]
    public void Refresh()
    {
        m_parserSent = new DemoHoldBytesForChunkGenericElement(m_elementPerChunk, m_elementsToConvert.Length,
            m_arrayReconstructionId);

        //m_parserReceived = new DemoHoldBytesForChunkGenericElement(m_elementPerChunk, m_elementsToConvert.Length,
        //    m_arrayReconstructionId,
        //  new DemoGenericBiDirectionalParseByte());
    }
}
