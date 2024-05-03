using Unity.Collections;
using UnityEngine;

public struct StructRandomizerJob_Vector3 : I_ProvideRandomAndDefaultElementInJob<Vector3>
{
    public void GetDefault(out Vector3 element) =>
        element = new Vector3();

    public void GetRandom(out Vector3 element)
    {
        System.Random r = new System.Random();
        element = new Vector3((float)r.NextDouble(), (float)r.NextDouble(), (float)r.NextDouble());

    }

    public void SetWithDefault(NativeArray<Vector3> source, in int indexElement)
    {
        GetDefault(out Vector3 v);
        source[indexElement] = v;
    }

    public void SetWithRandom(NativeArray<Vector3> source, in int indexElement)
    {

        GetRandom(out Vector3 v);
        source[indexElement] = v;
    }
}