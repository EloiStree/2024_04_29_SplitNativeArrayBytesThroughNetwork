[System.Serializable]
public class HoldVector3BytesAsChunkToPush : HoldBytesForChunkGenericElement<UnityEngine.Vector3, StructParserJob_Vector3>
{
    public HoldVector3BytesAsChunkToPush(int chunkElementSize, int totaleElementSize, byte reconstructionArrayId) :
        base(chunkElementSize, totaleElementSize, reconstructionArrayId)
    { }
}


