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

    public int m_maxElement;
    public J m_parser;

    public void Execute(int index)
    {
        if (index < m_maxElement)
            m_parser.ParseBytesFromElement(m_toCopyInBytes, index, m_toCopyStruct[index]);
    }
}
