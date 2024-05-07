using UnityEngine;

public class NativeToHoldBytesForChunkColor32Mono :
    NativeToHoldBytesForChunkGenericElementMono<Color32, StructParserJob_Color32, StructRandomizerJob_Color32>
{


    [ContextMenu("Create or Refresh Holder")]
    public new void CreateOrRefreshHolder()
    {
        base.CreateOrRefreshHolder();
    }

}

