using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

[BurstCompile]
public struct STRUCTJOB_ParseGenericStructToBytes<T, J> : IJobParallelFor where T : struct where J : struct, I_HowToParseElementInByteNativeArray<T>
{
    
    [ReadOnly]
    public NativeArray<T> m_toCopyStruct;

    [NativeDisableParallelForRestriction]
    [WriteOnly]
    public NativeArray<byte> m_toCopyInBytes;

    public int m_maxElementToSent;
    public int m_currentElemenToSent;
    public J m_parser;
    public bool m_writeEmpty;

    public void Execute(int index)
    {
        if (index < m_maxElementToSent && index< m_currentElemenToSent)
        {
            m_parser.ParseBytesFromElement(m_toCopyInBytes, index, m_toCopyStruct[index]);


        }
        else {
            if (m_writeEmpty)
            {
                m_parser.ParseBytesToUnusedValue(m_toCopyInBytes, index);
            }
        }
    }
}
