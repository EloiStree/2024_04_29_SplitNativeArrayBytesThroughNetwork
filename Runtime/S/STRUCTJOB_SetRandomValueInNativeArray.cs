using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

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
