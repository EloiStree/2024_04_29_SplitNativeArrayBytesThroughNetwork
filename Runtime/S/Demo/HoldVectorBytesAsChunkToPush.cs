[System.Serializable]
public class HoldVectorBytesAsChunkToPush : HoldBytesForChunkGenericElement<UnityEngine.Vector3, StructParserJob_Vector3>
{
    public HoldVectorBytesAsChunkToPush(int chunkElementSize, int totaleElementSize, byte reconstructionArrayId) :
        base(chunkElementSize, totaleElementSize, reconstructionArrayId)
    { }
}


