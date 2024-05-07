using UnityEngine;

public class DebugInspectorStaticDicoHoldBytesChunkIntMono : DebugInspectorStaticDicoValueGeneric<
    HoldBytesForChunkGenericElement<int, StructParserJob_Integer>, 
    CreateDefaultValue<HoldBytesForChunkGenericElement<int, StructParserJob_Integer>>>
{
    [ContextMenu("Update Ref")]
    public new void UpdateReference()
    {
        base.UpdateReference();
    }
}

