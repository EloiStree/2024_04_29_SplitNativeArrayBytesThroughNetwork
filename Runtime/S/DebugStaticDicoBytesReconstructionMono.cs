using UnityEngine;

public class DebugStaticDicoBytesReconstructionMono : DebugInspectorStaticDicoValueGeneric<ReconstructionFullByteRange,
    CreateDefaultValue<ReconstructionFullByteRange>>
{
    [ContextMenu("Update Ref")]
    public new void UpdateReference()
    {
        base.UpdateReference();
    }
}