using Unity.Collections;

public interface I_HowToParseByteNativeArrayToElement<T>
{
    void ParseBytesToElement(NativeArray<byte> source, in int indexElement, out T element);
    int GetSizeOfElementInBytesCount();
}
