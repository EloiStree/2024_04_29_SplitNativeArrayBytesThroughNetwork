using UnityEngine;

[System.Serializable]
public class TDD_HoldBytesForChunkColorRenderElement : TDD_HoldBytesForChunkGenericElement<Color32, StructParserJob_Color32, StructRandomizerJob_Color32>
{

    public RenderTexture m_source;
    public Texture2D m_textureHolder;

    public void Awake()
    {
        m_holder = new HoldBytesForChunkGenericElement<Color32, StructParserJob_Color32>
            (base.m_elementPerChunk, m_source.width * m_source.height, base.m_arrayIdOfReconstruction);
    }

    [ContextMenu("Push Color From Renderer")]
    public void PushColorFromRendererTexture2D()
    {
        if (m_textureHolder == null)
            m_textureHolder = new Texture2D(m_source.width, m_source.height);
        RenderTexture.active = m_source;
        m_textureHolder.ReadPixels(new Rect(0, 0, m_source.width, m_source.height), 0, 0);
        m_textureHolder.Apply();
        Color32[] pixels = m_textureHolder.GetPixels32();
        RenderTexture.active = null;
        base.m_elementMaxInArray = m_source.width * m_source.height;
        base.m_arrayIn = pixels;
        base.SetWithJob(pixels);
        base.RefreshChunksFromFullByteArray();
        base.PushChunksAsBytesRef();
    }

   
}

