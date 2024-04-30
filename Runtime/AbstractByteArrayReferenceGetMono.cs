using System;
using Unity.Collections;
using UnityEngine;

public abstract class AbstractByteArrayReferenceGetMono : MonoBehaviour, IChunkableBytesSourceArray
{
    public abstract byte[] GetBytesArray();
    public abstract int GetBytesArrayLenght();
    public abstract int SizeOfElementInByteStored();
    public abstract int HowManyElementMaxAreStoreInCurrentArray();
}


public abstract class AbstractByteArrayReferenceSetGetMono : AbstractByteArrayReferenceGetMono, IChunkableBytesSettableArray
{
    public abstract void SetFromEqualSizeNativeArray(NativeArray<byte> array);
    public abstract void SetFromClassicArray(byte[] arrayOrChunk, int offsetOfStoreArray);
}
public interface IChunkableBytesSettableArray : IChunkableBytesSourceArray {

    public void SetFromEqualSizeNativeArray(NativeArray<byte> array);
    public void SetFromClassicArray(byte[] arrayOrChunk, int offsetOfStoreArray);
}