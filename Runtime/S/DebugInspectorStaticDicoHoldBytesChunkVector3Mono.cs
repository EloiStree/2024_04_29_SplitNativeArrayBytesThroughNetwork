using UnityEngine;

public class DebugInspectorStaticDicoHoldBytesChunkVector3Mono : DebugInspectorStaticDicoValueGeneric<
    HoldBytesForChunkGenericElement<Vector3, StructParserJob_Vector3>,
    CreateDefaultValue<HoldBytesForChunkGenericElement<Vector3, StructParserJob_Vector3>>>
{
    [ContextMenu("Update Ref")]
    public new void UpdateReference()
    {
        base.UpdateReference();
    }
}
