using System;
using Unity.Collections;

[System.Serializable]
public class HoldIntegerBytesAsChunkToPush : HoldBytesForChunkGenericElement<int, StructParserJob_Integer>
{
    public HoldIntegerBytesAsChunkToPush(int chunkElementSize, int totaleElementSize, byte reconstructionArrayId) :
        base(chunkElementSize, totaleElementSize, reconstructionArrayId)
    { }
}


