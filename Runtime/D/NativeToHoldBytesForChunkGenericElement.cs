

using Eloi.WatchAndDate;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;



[System.Serializable]
public class NativeToHoldBytesForChunkGenericElementMono<T, J, D> :
    MonoBehaviour where T : struct where J : struct, I_HowToParseElementInByteNativeArray<T>
    where D : struct, I_ProvideRandomAndDefaultElementInJob<T>
{

    [Header("Don't change when started or call refresh")]
    public int m_elementPerChunk = 512;
    public byte m_arrayIdOfReconstruction = 5;
    public int m_elementMaxInArray = 128 * 128;
    public int m_currentElemenToSent = 128 * 128;
    public bool m_useOverrideUnusedValue;

    public StaticDicoStorageHoldBytesChunkGenericMono<T, J, D> m_staticHolder;
    public D m_randomizer;

    public UnityEvent<byte[]> m_onPushAllChunkAsBytesRef;
    public UnityEvent<byte[]> m_onPushFullByteArrayRef;
    public UnityEvent<int> m_onNumberOfBytePushed;
    public WatchAndDateTimeActionResult m_watchTime;
    private void Awake()
    {
        CreateOrRefreshHolder();
    }

    


    public void SetNativeSourceToUse(NativeArray<T> giveNativeArray) {
        m_watchTime.StartCounting();
        var holder = GetReference().m_value;

        holder.m_useOverrideUnusedValue = m_useOverrideUnusedValue;
        holder.m_currentElemenToSent = m_currentElemenToSent;
        holder.SetWithJob(giveNativeArray);
        holder.RefreshChunksFromFullByteArray();
        
        
        int maxChunk = 1+ (holder.m_currentElemenToSent / m_elementPerChunk);
        if (maxChunk > holder.m_groupOfChunkArray.Count - 1)
            maxChunk = holder.m_groupOfChunkArray.Count - 1;

        m_onNumberOfBytePushed.Invoke(maxChunk* m_elementPerChunk);
        for (int i = maxChunk; i >= 0; i--)
        {
            m_onPushAllChunkAsBytesRef.Invoke(holder.m_groupOfChunkArray[i].m_chunkArray);
        }
        m_onPushFullByteArrayRef.Invoke(holder.m_fullByteArray);
        m_watchTime.StopCounting();
    }

   

    public void    CreateOrRefreshHolder()
    {
        m_staticHolder.CreatEmptyIfNotExisting();
        m_staticHolder.GetValue(out bool found,
            out StaticDicoStorageGeneric<HoldBytesForChunkGenericElement<T, J>,
            CreateDefaultValue<HoldBytesForChunkGenericElement<T, J>>>.RefClass reference);
      
            reference.m_value = new HoldBytesForChunkGenericElement<T, J>(m_elementPerChunk, m_elementMaxInArray, m_arrayIdOfReconstruction);
        
    }
    public StaticDicoStorageGeneric<HoldBytesForChunkGenericElement<T, J>,
           CreateDefaultValue<HoldBytesForChunkGenericElement<T, J>>>.RefClass
       GetReference()
    {
        m_staticHolder.GetValue(out bool found,
            out StaticDicoStorageGeneric<HoldBytesForChunkGenericElement<T, J>,
            CreateDefaultValue<HoldBytesForChunkGenericElement<T, J>>>.RefClass reference);
       
        return reference;
    }
}

