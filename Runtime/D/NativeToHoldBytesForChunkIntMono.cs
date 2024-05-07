using UnityEngine;

public class NativeToHoldBytesForChunkIntMono:
    NativeToHoldBytesForChunkGenericElementMono<int, StructParserJob_Integer, StructRandomizerJob_Integer>
{


    [ContextMenu("Create or Refresh Holder")]
    public new void CreateOrRefreshHolder() {
        base.CreateOrRefreshHolder();
    }

}

