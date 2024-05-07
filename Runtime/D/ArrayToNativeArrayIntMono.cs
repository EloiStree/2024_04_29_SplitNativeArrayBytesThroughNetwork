using UnityEngine;

public class ArrayToNativeArrayIntMono : SplitDebugArrayToNativeArrayGenericMono<int>
{
    public override int GetRandomValue()
    {
        return UnityEngine.Random.Range(int.MinValue, int.MaxValue);
    }
}
