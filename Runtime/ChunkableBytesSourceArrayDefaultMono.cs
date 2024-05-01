using System;
using Unity.Collections;
using UnityEngine;



public class ChunkableBytesSourceArrayDefaultMono: AbstractByteArrayReferenceSetGetMono
{
    public int m_elementByteSize = 11;
    public int m_elementMaxWantedInByteArray = 128 * 128;

    string m_storeGuid = "";

    public void Awake() {

        CheckExistance();
    }
    private void CheckExistance()
    {
        if (string.IsNullOrEmpty(m_storeGuid) || !StaticByteArrayToChunkDicoStored.GetDictionary().ContainsKey(m_storeGuid))
        {
            CreateOrOverwrite();
        }
    }

    private void CreateOrOverwrite()
    {
        m_storeGuid = Guid.NewGuid().ToString();
        StaticByteArrayToChunkDicoStored.GetDictionary().Add(m_storeGuid, new byte[m_elementByteSize * m_elementMaxWantedInByteArray]);
    }

    public override int GetSizeOfElementInByteStored()
    {
        CheckExistance();
        return m_elementByteSize;
    }

    

    public override int GetHowManyElementMaxAreStoreInCurrentArray()
    {
        CheckExistance();
        return m_elementMaxWantedInByteArray;
    }

    public override byte[] GetBytesArray()
    {
        CheckExistance();
        return StaticByteArrayToChunkDicoStored.GetDictionary()[m_storeGuid];
    }

    public override int GetBytesArrayLenght()
    {
       return m_elementByteSize * m_elementMaxWantedInByteArray;
    }

    public override void SetFromEqualSizeNativeArray(NativeArray<byte> array)
    {
        CheckExistance();
        array.CopyTo(StaticByteArrayToChunkDicoStored.GetDictionary()[m_storeGuid]);
    }
    public override void SetFromClassicArray(byte[] arrayOrChunk, int offsetOfStoreArray=0)
    {
        CheckExistance();
        byte[] refArray = StaticByteArrayToChunkDicoStored.GetDictionary()[m_storeGuid];
        Buffer.BlockCopy(arrayOrChunk, 0, refArray,offsetOfStoreArray, arrayOrChunk.Length);
    }

}
