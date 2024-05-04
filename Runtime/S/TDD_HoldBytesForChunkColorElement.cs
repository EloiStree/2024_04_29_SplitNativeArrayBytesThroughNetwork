using UnityEngine;

[System.Serializable]
public class TDD_HoldBytesForChunkColorElement : TDD_HoldBytesForChunkGenericElement<Color32, StructParserJob_Color32, StructRandomizerJob_Color32>
{

    public Texture2D m_source;


    public void Awake()
    {
        m_holder = new HoldBytesForChunkGenericElement<Color32, StructParserJob_Color32>
            (base.m_elementPerChunk, m_source.width * m_source.height, base.m_arrayIdOfReconstruction);
    }

    [ContextMenu("Refresh")]
    public new void Refresh()
    {
        base.m_elementMaxInArray = m_source.width*m_source.height;
        base.m_arrayIn = m_source.GetPixels32();
        base.SetWithJob(base.m_arrayIn);
        base.Refresh();
    }
    [ContextMenu("Random")]
    public new void RandomizeArrayWithForLoop()
    {
        base.RandomizeArrayWithForLoop();
    }
    [ContextMenu("Push Ref")]
    public new void PushChunksAsBytesRef()
    {
        base.PushChunksAsBytesRef();
    }
    [ContextMenu("Push Copy")]
    public new void PushChunksAsBytesCopy()
    {
        base.PushChunksAsBytesCopy();
    }
}

