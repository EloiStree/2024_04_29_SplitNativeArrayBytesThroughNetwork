using UnityEngine;

public class DebugInspectorStaticDicoHoldBytesChunkColor32Mono : DebugInspectorStaticDicoValueGeneric<
    HoldBytesForChunkGenericElement<Color32, StructParserJob_Color32>,
    CreateDefaultValue<HoldBytesForChunkGenericElement<Color32, StructParserJob_Color32>>>
{
    [ContextMenu("Update Ref")]
    public new void UpdateReference()
    {
        base.UpdateReference();
    }
}

