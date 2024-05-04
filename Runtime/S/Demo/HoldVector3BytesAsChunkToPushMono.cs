using Unity.Collections;
using UnityEngine;

public class HoldVector3BytesAsChunkToPushMono : MonoBehaviour
{

    public int m_elementPerChunk;
    public int m_maxElementInArray;
    public byte m_reconstructionArrayId;

    public HoldVector3BytesAsChunkToPush m_integersAsChunk;

    public void Awake()
    {
        AllocateMemoryAndReset();
    }

    public void PushIn(NativeArray<Vector3> value)
    {
        m_integersAsChunk.SetWithJob(value);
    }

    [ContextMenu("Allocate memory and reset")]
    private void AllocateMemoryAndReset()
    {
        m_integersAsChunk = new HoldVector3BytesAsChunkToPush(m_elementPerChunk, m_maxElementInArray, m_reconstructionArrayId);
        m_integersAsChunk.RefreshChunksFromFullByteArray();
    }
}
