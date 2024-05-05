using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

[System.Serializable]
public class TDD_HoldBytesForChunkGenericElement <T,J,D> : 
    MonoBehaviour where T: struct where J : struct, I_HowToParseElementInByteNativeArray<T>
    where D:struct, I_ProvideRandomAndDefaultElementInJob<T> {

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

    public void SetWithJob(NativeArray<T> array)
    {

        m_holder.SetWithJob(array);
    }
    public void SetWithJob(T[] array)
    {
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

    [ContextMenu("Refresh Chunk from byte array")]
    public void RefreshChunksFromFullByteArray() {
        m_holder.RefreshChunksFromFullByteArray();
    }

    public void RandomizeArrayWithForLoop()
    {
        for (int i = 0; i < m_arrayIn.Length; i++)
        {
            m_randomizer.GetRandom(out m_arrayIn[i]);
        }

        m_holder.SetWithJob(m_arrayIn);
        m_holder.RefreshChunksFromFullByteArray();
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
