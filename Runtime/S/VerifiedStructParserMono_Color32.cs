using UnityEngine;

public class VerifiedStructParserMono_Color32 : VerifiedStructParserMono<Color32, StructParserJob_Color32>
{
    [ContextMenu("Tick")]
    public override void CallParseCheck()
    {
        base.ParseCheck();
    }
}



