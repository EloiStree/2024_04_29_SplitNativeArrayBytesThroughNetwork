using UnityEngine;

public class VerifiedStructParserMono_Vector3 : VerifiedStructParserMono<Vector3, StructParserJob_Vector3> {
    [ContextMenu("Tick")]
    public override void CallParseCheck()
    {
        base.ParseCheck();
    }
}
