using UnityEngine;

public class ArrayToNativeArrayVector3Mono : SplitDebugArrayToNativeArrayGenericMono<Vector3>
{
    public override Vector3 GetRandomValue()
    {
        return new Vector3(Random.Range(float.MinValue, float.MaxValue), Random.Range(float.MinValue, float.MaxValue), Random.Range(float.MinValue, float.MaxValue));
    }
}
