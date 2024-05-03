public class DemoHoldBytesForChunkGenericElement : HoldBytesForChunkGenericElement<PrimitiveStructToParseSample, DemoPriStructParserJob>
{
    public DemoHoldBytesForChunkGenericElement(int chunkElementSize, int totaleElementSize, byte reconstructionArrayId) :
        base(chunkElementSize, totaleElementSize, reconstructionArrayId)
    { }
}

