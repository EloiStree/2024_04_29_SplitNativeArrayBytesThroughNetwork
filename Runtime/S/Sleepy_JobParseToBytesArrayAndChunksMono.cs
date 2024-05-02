using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using Eloi.WatchAndDate;

public class Sleepy_JobParseToBytesArrayAndChunksMono : MonoBehaviour
{
    public int m_elementPerChunk;
    public WatchAndDateTimeActionResult m_estimation;
    public PrimitiveStructToParseSample[] m_elementToConvert;
    public DemoHoldBytesForChunkGenericElement m_parserSent;
    public DemoHoldBytesForChunkGenericElement m_parserReceived;
    public PrimitiveStructToParseSample[] m_elementReceived;

    private void Awake()
    {
        Refresh();
        //  m_parserSent.SetWithJob(m_elementToConvert);

    }

    [ContextMenu("Randomized")]
    public void Randomize()
    {

        Randomize(ref m_elementToConvert);
    }

    [ContextMenu("Job Parsing")]
    public void JobParsing()
    {

        m_parserSent.SetWithJob(m_elementToConvert);
    }
    [ContextMenu("Test Random Job Parsing")]
    public void RandomJobParsing()
    {
        Refresh();
        Randomize(ref m_elementToConvert);
        m_estimation.StartCounting();
        m_parserSent.SetWithJob(m_elementToConvert);
        m_parserSent.RefreshChunkFromFullByteArray();
        m_estimation.StopCounting();
    }
    [ContextMenu("Test Random Job B")]
    public void RandomJobParsingB()
    {
        DemoHoldBytesForChunkGenericElement test = new DemoHoldBytesForChunkGenericElement(m_elementPerChunk, m_elementToConvert.Length,
          new DemoGenericBiDirectionalParseByte());
        Randomize(ref m_elementToConvert);
        m_estimation.StartCounting();
        test.SetWithJob(m_elementToConvert);
        test.RefreshChunkFromFullByteArray();
        m_estimation.StopCounting();
    }
    [ContextMenu("Test Random Job C")]
    public void RandomJobParsingC()
    {
        DemoHoldBytesForChunkGenericElement test = new DemoHoldBytesForChunkGenericElement(m_elementPerChunk, m_elementToConvert.Length,
          new DemoGenericBiDirectionalParseByte());
        Randomize(ref m_elementToConvert);
        m_estimation.StartCounting();
        NativeArray<PrimitiveStructToParseSample> temp = new NativeArray<PrimitiveStructToParseSample>(m_elementToConvert, Allocator.TempJob);
        test.SetWithJob(temp);
        test.RefreshChunkFromFullByteArray();
        temp.Dispose();
        m_estimation.StopCounting();
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
        item. m_sbyte = (sbyte)UnityEngine.Random.Range(0, byte.MaxValue);
        item. m_int  = (int)UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        item.  m_uint = (uint)UnityEngine.Random.Range(uint.MinValue, uint.MaxValue);
        item. m_long = (long)UnityEngine.Random.Range(long.MinValue, long.MaxValue);
        item. m_ulong = (ulong)UnityEngine.Random.Range(ulong.MinValue, ulong.MaxValue);
        item. m_short = (short)UnityEngine.Random.Range(short.MinValue, short.MaxValue);
        item. m_ushort = (ushort)UnityEngine.Random.Range(ushort.MinValue, ushort.MaxValue);
        item. m_float = (float)UnityEngine.Random.Range(float.MinValue, float.MaxValue);
        item.  m_double = (double)UnityEngine.Random.Range(float.MinValue, float.MaxValue);
        item.  m_char0= (char)UnityEngine.Random.Range(40,128);
        item.m_char1 = (char)UnityEngine.Random.Range(40, 128);    
    }

    [ContextMenu("Refresh")]
    public void Refresh()
    {
        m_parserSent = new DemoHoldBytesForChunkGenericElement(m_elementPerChunk, m_elementToConvert.Length,
          new DemoGenericBiDirectionalParseByte());

        m_parserReceived = new DemoHoldBytesForChunkGenericElement(m_elementPerChunk, m_elementToConvert.Length,
          new DemoGenericBiDirectionalParseByte());
    }

}

public class DemoHoldBytesForChunkGenericElement : HoldBytesForChunkGenericElement<PrimitiveStructToParseSample, PrimitiveStructToParseSampleJobMethode>
{
    public DemoHoldBytesForChunkGenericElement(int chunkElementSize, int totaleElementSize, A_GenericBiDirectionalParseByte<PrimitiveStructToParseSample> parser) : base(chunkElementSize, totaleElementSize, parser)
    {}

}


public struct PrimitiveStructToParseSampleJobMethode : I_HowToParseElementInByteNativeArray<PrimitiveStructToParseSample>
{
    public static int m_elementSize = 44;
    public void Parse(NativeArray<byte> source, in int indexElement, in PrimitiveStructToParseSample toSet)
    {
        byte[] b=null;
        //byte[] targetArray = new byte[44];
        int offset = m_elementSize * indexElement;
        source[offset+0] = toSet.m_byte;
        source[offset + 1] = (byte)toSet.m_sbyte;

        source[offset + 2] = (byte)(toSet.m_int & 0xFF);
        source[offset + 3] = (byte)((toSet.m_int >> 8) & 0xFF);
        source[offset + 4] = (byte)((toSet.m_int >> 16) & 0xFF);
        source[offset + 5] = (byte)((toSet.m_int >> 24) & 0xFF);

        ////BitConverter.GetBytes(toSet.m_uint).CopyTo(targetArray,  6);
        ////BitConverter.GetBytes(toSet.m_long).CopyTo(targetArray,   10);
        ////BitConverter.GetBytes(toSet.m_ulong).CopyTo(targetArray,   18);
        ////BitConverter.GetBytes(toSet.m_short).CopyTo(targetArray,   26);
        ////BitConverter.GetBytes(toSet.m_ushort).CopyTo(targetArray,   28);
        ////BitConverter.GetBytes(toSet.m_float).CopyTo(targetArray,   30);
        ////BitConverter.GetBytes(toSet.m_double).CopyTo(targetArray,   34);
        source[offset + 42] = (byte)toSet.m_char0;
        source[offset + 43] = (byte)toSet.m_char1;

        ////for (int i = 0; i < 44; i++)
        ////{
        ////    targetNativeArray[offset + i] = targetArray[i];
        ////}
        //DemoGenericBiDirectionalParseByte p = new DemoGenericBiDirectionalParseByte();
        //p.ParseToBytes(source, in indexElement, in element);
    }
}


/// <summary>
/// This interface is to define how in a parallels job system, an given element should be parse in a byte array with the T element 
/// </summary>
/// <typeparam name="T"></typeparam>
public interface I_HowToParseElementInByteNativeArray<T> {
    void Parse(NativeArray<byte> source, in int indexElement, in T element);
}



[BurstCompile]
public struct STRUCTJOB_ParseGenericStructToBytes<T,J> : IJobParallelFor where T :struct where J:struct, I_HowToParseElementInByteNativeArray<T>
{
    [ReadOnly]
    public NativeArray<T> m_toCopyStruct;

    [NativeDisableParallelForRestriction]
    [WriteOnly]
    public NativeArray<byte> m_toCopyInBytes;

    public int m_maxElement;
    public J m_parser;

    public void Execute(int index)
    {
        if(index<m_maxElement)
            m_parser.Parse(m_toCopyInBytes, index, m_toCopyStruct[index]);
    }
}





[System.Serializable]
public class HoldBytesForChunkGenericElement <T,J> where T: struct where J : struct, I_HowToParseElementInByteNativeArray<T>
{

    public static Dictionary<string, HoldBytesForChunkGenericElement<T, J>> m_staticDico = new Dictionary<string, HoldBytesForChunkGenericElement<T, J>>();

    public static void SetOrAddInDico(string key, HoldBytesForChunkGenericElement<T, J> value) {
        if (m_staticDico.ContainsKey(key))
            m_staticDico[key] = value;
        else m_staticDico.Add(key,value);
    }
    public static HoldBytesForChunkGenericElement<T, J> GetFromKey(string key)
    {
        if (m_staticDico.ContainsKey(key))
            return m_staticDico[key] ;
        return null;
    }
    private static J m_structParser = new();

    public void SetWithJob(NativeArray<T> elements)
    {

        STRUCTJOB_ParseGenericStructToBytes<T, J> job = new STRUCTJOB_ParseGenericStructToBytes<T, J>()
        {
            m_toCopyStruct = elements,
            m_toCopyInBytes = m_byteNativeArray,
            m_maxElement = m_elementTotale,
            m_parser = m_structParser,
        };
        job.Schedule(elements.Length, 64).Complete();
    }
    public void SetWithJob(T[] elements)
    {
        NativeArray<T> temp = new NativeArray<T>(elements.Length, Allocator.TempJob);
        temp.CopyFrom(elements);
        STRUCTJOB_ParseGenericStructToBytes<T,J> job = new STRUCTJOB_ParseGenericStructToBytes<T,J>()
        {
            m_toCopyStruct = temp,
            m_toCopyInBytes = m_byteNativeArray,
            m_maxElement = m_elementTotale,
            m_parser = m_structParser,
        };
        job.Schedule(elements.Length, 64).Complete();
        temp.Dispose();
    }


    public void SetAllWithoutJob(T[] elementToSet)
    {
        for (int i = 0; i < elementToSet.Length; i++)
        {
            m_parser.ParseToBytes(m_byteNativeArray, in i, in elementToSet[i]);
        }
    }
    public void SetAtIndexWithoutJob(int index, T[] elementToSet)
    {
        for (int i = index; i < elementToSet.Length && i < m_elementTotale; i++)
        {
            m_parser.ParseToBytes( m_byteNativeArray, in i, in elementToSet[i]);
        }
    }
    public void RefreshChunkFromFullByteArray() {

        m_fullByteArray = m_byteNativeArray.ToArray();
        for (int i = 0; i < m_numberOfChunk; i++)
        {
            int endIndexAt = (i * m_chunkByteSize) + m_chunkByteSize;
            if (endIndexAt < m_fullByteArray.Length)
                Buffer.BlockCopy(m_fullByteArray, i * m_chunkByteSize, m_groupOfChunkArray[i].m_chunkArray, 0, m_chunkByteSize);
            else
                Buffer.BlockCopy(m_fullByteArray, i * m_chunkByteSize, m_groupOfChunkArray[i].m_chunkArray, 0, m_fullByteArray.Length - (i * m_chunkByteSize));
        }
    }

    public HoldBytesForChunkGenericElement(int chunkElementSize,int totaleElementSize, A_GenericBiDirectionalParseByte<T> parser) {

        m_elementInChunk = chunkElementSize;
        m_elementTotale = totaleElementSize;
        m_parser = parser;

       
        m_numberOfChunk =1+( m_elementTotale / m_elementInChunk);
        

        m_fullByteArray = new byte[totaleElementSize * parser.GetElementBytesSize()];
        m_byteNativeArray = new NativeArray<byte>(m_fullByteArray, Allocator.Persistent);
        m_groupOfChunkArray.Clear();
        for (int i = 0; i < m_numberOfChunk; i++)
        {
            m_groupOfChunkArray.Add(new ChunkByteArray(m_elementInChunk * parser.GetElementBytesSize()));
        }
        m_elementByteSize = parser.GetElementBytesSize();
        m_chunkByteSize = m_elementByteSize * m_elementInChunk;
    }

    ~HoldBytesForChunkGenericElement()
    {
        Dispose();
    }

    public void Dispose() {

     
        m_fullByteArray = null;
        m_groupOfChunkArray.Clear(); 
        if (m_byteNativeArray != null && m_byteNativeArray.IsCreated)
            m_byteNativeArray.Dispose();
    }

    private A_GenericBiDirectionalParseByte<T> m_parser;
    public int m_elementByteSize = 128;
    public int m_chunkByteSize = 128;

    public int m_elementInChunk = 128;
    public int m_elementTotale = 128;
    public int m_numberOfChunk = 128;
    public NativeArray<byte> m_byteNativeArray;
    public byte[] m_fullByteArray= new byte[0];
    public List<ChunkByteArray> m_groupOfChunkArray= new List<ChunkByteArray>();
    [System.Serializable]
    public struct ChunkByteArray {

        public byte[] m_chunkArray;

        public ChunkByteArray(int size)
        {
            m_chunkArray = new byte[size];
        }
    }
}
