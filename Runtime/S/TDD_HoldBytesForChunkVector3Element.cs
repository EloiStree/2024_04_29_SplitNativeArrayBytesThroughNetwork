using UnityEngine;

[System.Serializable]
public class TDD_HoldBytesForChunkVector3Element : TDD_HoldBytesForChunkGenericElement<Vector3, StructParserJob_Vector3, StructRandomizerJob_Vector3>
{


    [ContextMenu("Refresh")]
    public new void Refresh()
    {
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
