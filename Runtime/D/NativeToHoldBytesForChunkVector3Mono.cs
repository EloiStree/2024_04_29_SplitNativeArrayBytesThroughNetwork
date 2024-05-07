using UnityEngine;

public class NativeToHoldBytesForChunkVector3Mono :
    NativeToHoldBytesForChunkGenericElementMono<Vector3, StructParserJob_Vector3, StructRandomizerJob_Vector3>
{


    [ContextMenu("Create or Refresh Holder")]
    public new void CreateOrRefreshHolder()
    {
        base.CreateOrRefreshHolder();
    }

}

