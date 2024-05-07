using UnityEngine;

public class ArrayToNativeArrayColor32Mono : SplitDebugArrayToNativeArrayGenericMono<Color32>
{
    public override Color32 GetRandomValue()
    {
        return new Color32((byte)Random.Range(0, 256), (byte)Random.Range(0, 256), (byte)Random.Range(0, 256),255);
    }
}
