using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

[System.Serializable]
public class HoldBytesForChunkGenericElement <T,J> where T: struct where J : struct, I_HowToParseElementInByteNativeArray<T>
{

    private static J m_structParser = new();


    public int m_currentFrameId;

    public void SetWithJob(NativeArray<T> elements)
    {
       

        STRUCTJOB_ParseGenericStructToBytes<T, J> job = new STRUCTJOB_ParseGenericStructToBytes<T, J>()
        {
            m_toCopyStruct = elements,
            m_toCopyInBytes = m_byteNativeArray,
            m_maxElement = m_elementTotaleInCollection,
            m_parser = m_structParser,
        };
        JobHandle handle =  job.Schedule(elements.Length, 64);

        
        handle.Complete();
       
        AddChunkFrame();
    }
    public void SetWithJob(T[] elements)
    {

        NativeArray<T> temp = new NativeArray<T>(elements, Allocator.TempJob);
        SetWithJob(temp);
        temp.Dispose();
    }



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

        bool isModuloZero = (m_elementTotaleInCollection % m_elementInChunk) == 0;
        int c = m_elementTotaleInCollection / m_elementInChunk;
        if (!isModuloZero)
            c +=1;
        m_numberOfChunk = c;
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
