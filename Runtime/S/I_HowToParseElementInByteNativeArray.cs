using Unity.Collections;
/// <summary>
/// This interface is to define how in a parallels job system, an given element should be parse in a byte array with the T element 
/// </summary>
/// <typeparam name="T"></typeparam>
public interface I_HowToParseElementInByteNativeArray<T>
{
    void ParseBytesFromElement(NativeArray<byte> source, in int indexElement, in T element);
    int GetSizeOfElementInBytesCount();

}
