using Unity.Collections;

public struct StructRandomizerJob_Integer : I_ProvideRandomAndDefaultElementInJob<int>
{
    public void GetDefault(out int element) =>
        element =0;

    public void GetRandom(out int element) {

        System.Random r = new System.Random();
       element= r.Next(int.MinValue, int.MaxValue);

    
    }

    public void SetWithDefault(NativeArray<int> source, in int indexElement)
    {
        GetDefault(out int v);
        source[indexElement] = v;
    }

    public void SetWithRandom(NativeArray<int> source, in int indexElement)
    {

        GetRandom(out int v);
        source[indexElement] = v;
    }
}
