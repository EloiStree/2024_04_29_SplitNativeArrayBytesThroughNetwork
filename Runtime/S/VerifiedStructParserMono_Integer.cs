using UnityEngine;

public class VerifiedStructParserMono_Integer : VerifiedStructParserMono<int, StructParserJob_Integer> {
    [ContextMenu("Tick")]
    public override void CallParseCheck()
    {
        base.ParseCheck();
    }
}
