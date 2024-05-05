using Unity.Collections;

public interface I_ProvideRandomAndDefaultElementInJob<T> where T:struct
{
    void SetWithRandom(NativeArray<T> source, in int indexElement);
    void SetWithDefault(NativeArray<T> source, in int indexElement);
    void GetDefault( out T element);
    void GetRandom(out T element);
}
