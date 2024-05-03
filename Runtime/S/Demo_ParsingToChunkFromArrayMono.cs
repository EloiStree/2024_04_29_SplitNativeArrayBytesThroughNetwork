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


/// <summary>
/// This interface is to define how in a parallels job system, an given element should be parse in a byte array with the T element 
/// </summary>
/// <typeparam name="T"></typeparam>
public interface I_HowToParseElementInByteNativeArray<T>
{
    void ParseBytesFromElement(NativeArray<byte> source, in int indexElement, in T element);
    int GetSizeOfElementInBytesCount();

}
public interface I_HowToParseByteNativeArrayToElement<T>
{
    void ParseBytesToElement(NativeArray<byte> source, in int indexElement, out T element);
    int GetSizeOfElementInBytesCount();
}

public interface I_ProvideRandomAndDefaultElementInJob<T> where T:struct
{
    void SetWithRandom(NativeArray<T> source, in int indexElement);
    void SetWithDefault(NativeArray<T> source, in int indexElement);
    void GetDefault( out T element);
    void GetRandom(out T element);
}





[BurstCompile]
public struct STRUCTJOB_ParseGenericStructToBytes<T, J> : IJobParallelFor where T : struct where J : struct, I_HowToParseElementInByteNativeArray<T>
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
        if (index < m_maxElement)
            m_parser.ParseBytesFromElement(m_toCopyInBytes, index, m_toCopyStruct[index]);
    }
}

[BurstCompile]
public struct STRUCTJOB_SetRandomValueInNativeArray<T, J> : IJobParallelFor where T : struct where J : struct, I_ProvideRandomAndDefaultElementInJob<T>
{
    public NativeArray<T> m_toRandomized;
    public int m_maxElement;
    public bool m_useDefaultForAll;
    public bool m_resetToDefaultOutOfRange;
    public J m_parser;

    public void Execute(int index)
    {
        if (m_useDefaultForAll) {
            m_parser.GetDefault( out T a);
            m_toRandomized[index] = a;
            return;
        }

        if (index < m_maxElement)
        {
            m_parser.GetRandom( out T a);
            m_toRandomized[index] = a;

        }
        else {

            if (m_resetToDefaultOutOfRange) { 
                m_parser.GetDefault( out T a);
                m_toRandomized[index] = a;
            }
        }

    }
}





[System.Serializable]
public class TDD_HoldBytesForChunkGenericElement <T,J,D> : MonoBehaviour where T: struct where J : struct, I_HowToParseElementInByteNativeArray<T> where D:struct, I_ProvideRandomAndDefaultElementInJob<T> {

    public int m_elementPerChunk = 512;
    public int m_elementMaxInArray=128*128;
    public byte m_arrayIdOfReconstruction = 5;

    public T[] m_arrayIn;
    public NativeArray<T> m_nativeArrayIn;
    public HoldBytesForChunkGenericElement<T, J> m_holder;
    public D m_randomizer;

    public UnityEvent<byte[]> m_onPushAllChunkAsBytesRef;
    public UnityEvent<byte[]> m_onPushFullByteArrayRef;

    private void Awake()
    {
        Refresh();
    }

    public void PushIn(NativeArray<T> array) {

        m_holder.SetWithJob(array);
    }

    public void AddChunkFrame() {
        m_holder.AddChunkFrame();
    }



    [ContextMenu("Push Chunks As Bytes Ref")]
    public void PushChunksAsBytesRef()
    {

        for (int i = m_holder.m_groupOfChunkArray.Count-1; i >=0; i--)
        {
            m_onPushAllChunkAsBytesRef.Invoke(m_holder.m_groupOfChunkArray[i].m_chunkArray);
        }
        m_onPushFullByteArrayRef.Invoke(m_holder.m_fullByteArray);
    }
    [ContextMenu("Push Chunks As Bytes Copy")]
    public void PushChunksAsBytesCopy()
    {

        for (int i = m_holder.m_groupOfChunkArray.Count - 1; i >= 0; i--)
        {
            m_onPushAllChunkAsBytesRef.Invoke(m_holder.m_groupOfChunkArray[i].m_chunkArray.ToArray());
        }
        m_onPushFullByteArrayRef.Invoke(m_holder.m_fullByteArray.ToArray());
    }

    public void RandomizeArrayWithForLoop() {

        for (int i = 0; i < m_arrayIn.Length; i++)
        {
            m_randomizer.GetRandom(out m_arrayIn[i]);
        }

        m_nativeArrayIn = new NativeArray<T>(m_arrayIn, Allocator.TempJob);

        m_holder.SetWithJob(m_nativeArrayIn);
        m_holder.RefreshChunksFromFullByteArray();
        m_nativeArrayIn.Dispose();
    }

    public void Refresh()
    {
        m_nativeArrayIn = new NativeArray<T>(m_arrayIn, Allocator.TempJob);
        m_holder = new HoldBytesForChunkGenericElement<T, J>(m_elementPerChunk, m_elementMaxInArray, m_arrayIdOfReconstruction);
        m_holder.SetWithJob(m_nativeArrayIn);
        m_holder.RefreshChunksFromFullByteArray();
        m_nativeArrayIn.Dispose();

    }
}



[System.Serializable]
public class HoldBytesForChunkGenericElement <T,J> where T: struct where J : struct, I_HowToParseElementInByteNativeArray<T>
{

    private static J m_structParser = new();

    public void SetWithJob(NativeArray<T> elements)
    {

        STRUCTJOB_ParseGenericStructToBytes<T, J> job = new STRUCTJOB_ParseGenericStructToBytes<T, J>()
        {
            m_toCopyStruct = elements,
            m_toCopyInBytes = m_byteNativeArray,
            m_maxElement = m_elementTotaleInCollection,
            m_parser = m_structParser,
        };
        job.Schedule(elements.Length, 64).Complete();
        AddChunkFrame();
    }
    public void SetWithJob(T[] elements)
    {
        NativeArray<T> temp = new NativeArray<T>(elements.Length, Allocator.TempJob);
        temp.CopyFrom(elements);
        STRUCTJOB_ParseGenericStructToBytes<T,J> job = new STRUCTJOB_ParseGenericStructToBytes<T,J>()
        {
            m_toCopyStruct = temp,
            m_toCopyInBytes = m_byteNativeArray,
            m_maxElement = m_elementTotaleInCollection,
            m_parser = m_structParser,
        };
        job.Schedule(elements.Length, 64).Complete();
        temp.Dispose();
        AddChunkFrame();
    }


    public int m_currentFrameId;

    public void AddChunkFrame() => SetChunkFrame(m_currentFrameId + 1);

    public void SetChunkFrame(int frame) {
        m_currentFrameId = frame;
        for (int i = 0; i < m_groupOfChunkArray.Count; i++)
        {
            m_groupOfChunkArray[i].SetFrameId((uint)Math.Abs(frame));
        }
    }
    public void RefreshChunksFromFullByteArray() {

        m_fullByteArray = m_byteNativeArray.ToArray();
        for (int i = 0; i < m_numberOfChunk; i++)
        {
            int endIndexAt = (i * m_chunkByteSize) + m_chunkByteSize;
            int headerSizeInChunk= m_groupOfChunkArray[i].GetHeadSizeInBytes();
            if (endIndexAt < m_fullByteArray.Length)
                Buffer.BlockCopy(m_fullByteArray, i * m_chunkByteSize, m_groupOfChunkArray[i].m_chunkArray, 0+ headerSizeInChunk, m_chunkByteSize);
            else
                Buffer.BlockCopy(m_fullByteArray, i * m_chunkByteSize, m_groupOfChunkArray[i].m_chunkArray, 0+ headerSizeInChunk, m_fullByteArray.Length - (i * m_chunkByteSize));
        }
    }

    public HoldBytesForChunkGenericElement(int numberOfElementInChunk,int numberOfElementTotaleInCollection, byte idOfArrayUseInReconstruction) {

        m_elementInChunk = numberOfElementInChunk;
        m_elementTotaleInCollection = numberOfElementTotaleInCollection;
        m_elementByteSize = m_structParser.GetSizeOfElementInBytesCount();

        m_numberOfChunk = 1 + ( m_elementTotaleInCollection / m_elementInChunk);
        m_chunkByteSize = m_elementByteSize * m_elementInChunk;
        m_allChunkByteSize = m_chunkByteSize * m_numberOfChunk;
        m_percentOfUdpFullPackage = m_chunkByteSize / (float) (65536 - 17);

        m_fullByteArray = new byte[m_allChunkByteSize];
        m_byteNativeArray = new NativeArray<byte>(m_fullByteArray, Allocator.Persistent);
        m_groupOfChunkArray.Clear();
        for (int i = 0; i < m_numberOfChunk; i++)
        {
            ChunkByteArrayWithReconstructionId chunk = new ChunkByteArrayWithReconstructionId(m_chunkByteSize);
            chunk.SetArrayId(idOfArrayUseInReconstruction);
            chunk.SetByteArrayOffset((uint)(i * m_chunkByteSize));
            chunk.SetByteArrayOffsetLength((uint)(m_chunkByteSize));
            chunk.SetFrameId(0);
            chunk.SetChunkID((uint)i);
            chunk.SetByteArrayMaxLenght((uint)m_allChunkByteSize);
            m_groupOfChunkArray.Add(chunk);
        }

        if (m_chunkByteSize >= (65536 - 17))
            throw new Exception("A chunk of byte can't be that big because it need to fit in the max size of a UDP: 65536 bytes with a header of the tool in front.");
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

    //private A_GenericBiDirectionalParseByteInArray<T> m_parser;
    public int m_elementByteSize = 128;
    public int m_chunkByteSize = 128;
    public int m_allChunkByteSize = 128 * 128;
    [Range(0f,1f)]
    public float m_percentOfUdpFullPackage;

    public int m_elementInChunk = 128;
    public int m_elementTotaleInCollection = 128;
    public int m_numberOfChunk = 128;

    public NativeArray<byte> m_byteNativeArray;
    public byte[] m_fullByteArray= new byte[0];
    public List<ChunkByteArrayWithReconstructionId> m_groupOfChunkArray = new List<ChunkByteArrayWithReconstructionId>();
   
}
[System.Serializable]
public struct ChunkByteArrayWithReconstructionId
{


    public byte[] m_chunkArray;
    public byte m_arrayId;
    public uint m_frameId;
    public uint m_chunkId;
    public uint m_offsetStart;
    public uint m_offsetLength;

    public int GetHeadSizeInBytes() { return 21; }
    public ChunkByteArrayWithReconstructionId(int chunkSizeToStore)
    {
        m_chunkArray = new byte[chunkSizeToStore + 21];
        m_arrayId = 0;
        m_frameId = 0;
        m_chunkId = 0;
        m_offsetStart = 0;
        m_offsetLength = (uint)Math.Abs(chunkSizeToStore);
    }



    public void SetChunkArray(byte[] chunkByteArray)
    {
        m_chunkArray = chunkByteArray;
    }

    public void SetArrayId(byte arrayId)
    {
        m_arrayId = arrayId;
        m_chunkArray[0] = arrayId;
    }

    public void SetFrameId(uint frameId)
    {
        m_frameId = frameId;
        m_chunkArray[1] = (byte)(frameId & 0xFF);
        m_chunkArray[2] = (byte)((frameId >> 8) & 0xFF);
        m_chunkArray[3] = (byte)((frameId >> 16) & 0xFF);
        m_chunkArray[4] = (byte)((frameId >> 24) & 0xFF);
    }
    public void SetChunkID(uint chunkId)
    {
        m_chunkId = chunkId;
        m_chunkArray[5] = (byte)(chunkId & 0xFF);
        m_chunkArray[6] = (byte)((chunkId >> 8) & 0xFF);
        m_chunkArray[7] = (byte)((chunkId >> 16) & 0xFF);
        m_chunkArray[8] = (byte)((chunkId >> 24) & 0xFF);
    }
    public void SetByteArrayOffset(uint offsetInArray)
    {
        m_offsetStart = offsetInArray;
        m_chunkArray[9] = (byte)(offsetInArray & 0xFF);
        m_chunkArray[10] = (byte)((offsetInArray >> 8) & 0xFF);
        m_chunkArray[11] = (byte)((offsetInArray >> 16) & 0xFF);
        m_chunkArray[12] = (byte)((offsetInArray >> 24) & 0xFF);
    }
    public void SetByteArrayOffsetLength(uint offsetLength)
    {
        m_offsetLength = offsetLength;
        m_chunkArray[13] = (byte)(offsetLength & 0xFF);
        m_chunkArray[14] = (byte)((offsetLength >> 8) & 0xFF);
        m_chunkArray[15] = (byte)((offsetLength >> 16) & 0xFF);
        m_chunkArray[16] = (byte)((offsetLength >> 24) & 0xFF);
    }
    public void SetByteArrayMaxLenght(uint maxLenght)
    {
        m_offsetLength = maxLenght;
        m_chunkArray[17] = (byte)(maxLenght & 0xFF);
        m_chunkArray[18] = (byte)((maxLenght >> 8) & 0xFF);
        m_chunkArray[19] = (byte)((maxLenght >> 16) & 0xFF);
        m_chunkArray[20] = (byte)((maxLenght >> 24) & 0xFF);
    }
    public uint GetFrameId()
    {
        return (uint)((m_chunkArray[4] << 24) | (m_chunkArray[3] << 16) | (m_chunkArray[2] << 8) | m_chunkArray[1]);
    }
    public uint GetChunkId()
    {
        return (uint)((m_chunkArray[8] << 24) | (m_chunkArray[7] << 16) | (m_chunkArray[6] << 8) | m_chunkArray[5]);
    }
    public uint GetArrayOffset()
    {
        return (uint)((m_chunkArray[12] << 24) | (m_chunkArray[11] << 16) | (m_chunkArray[10] << 8) | m_chunkArray[9]);
    }
    public uint GetArrayOffestToCopy()
    {
        return (uint)((m_chunkArray[16] << 24) | (m_chunkArray[15] << 16) | (m_chunkArray[14] << 8) | m_chunkArray[13]);
    }
    public uint GetArrayMaxLength()
    {
        return (uint)((m_chunkArray[20] << 24) | (m_chunkArray[19] << 16) | (m_chunkArray[18] << 8) | m_chunkArray[17]);
    }

    public static void GetHeaderOfChunkReceived(in byte[] m_chunkArray, out byte arrayId,
        out uint framedId, out uint chunkId, out uint arrayOffset, out uint arrayOffsetLenght, out uint arrayMaxLength)
    {
        arrayId = m_chunkArray[0];
        framedId = (uint)((m_chunkArray[4] << 24) | (m_chunkArray[3] << 16) | (m_chunkArray[2] << 8) | m_chunkArray[1]);
        chunkId = (uint)((m_chunkArray[8] << 24) | (m_chunkArray[7] << 16) | (m_chunkArray[6] << 8) | m_chunkArray[5]);
        arrayOffset = (uint)((m_chunkArray[12] << 24) | (m_chunkArray[11] << 16) | (m_chunkArray[10] << 8) | m_chunkArray[9]);
        arrayOffsetLenght = (uint)((m_chunkArray[16] << 24) | (m_chunkArray[15] << 16) | (m_chunkArray[14] << 8) | m_chunkArray[13]);
        arrayMaxLength = (uint)((m_chunkArray[20] << 24) | (m_chunkArray[19] << 16) | (m_chunkArray[18] << 8) | m_chunkArray[17]);
    }

    public static void GetFramedIdOfChunkHead(byte[] chunkWithHeadRecevied, out uint framedId)
    {
        framedId = (uint)((chunkWithHeadRecevied[4] << 24) | (chunkWithHeadRecevied[3] << 16) | (chunkWithHeadRecevied[2] << 8) | chunkWithHeadRecevied[1]);
        
    }
}