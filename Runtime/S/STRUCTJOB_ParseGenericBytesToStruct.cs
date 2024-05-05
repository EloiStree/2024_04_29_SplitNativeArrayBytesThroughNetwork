using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

[BurstCompile]
public struct STRUCTJOB_ParseGenericBytesToStruct<T, J> : IJobParallelFor where T : struct where J : struct, I_HowToParseByteNativeArrayToElement<T>, I_ProvideRandomAndDefaultElementInJob<T>
{
    [ReadOnly]
    [NativeDisableParallelForRestriction]
    public NativeArray<byte> m_toCopyInBytes;

    [WriteOnly]
    public NativeArray<T> m_toCopyStruct;

    public int m_maxElement;
    public J m_parser;
    public T m_default;

    public void Execute(int index)
    {
        if (index < m_maxElement)
        {
            m_parser.ParseBytesToElement(m_toCopyInBytes, index, out T v);
            m_toCopyStruct[index] = v;
        }
        else {
            m_toCopyStruct[index] = m_default;
        }
    }
}
