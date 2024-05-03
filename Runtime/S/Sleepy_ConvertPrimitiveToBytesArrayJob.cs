using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Sleepy_ConvertPrimitiveToBytesArray : MonoBehaviour
{
   
}


public interface I_GenericBiDirectionalParseByteInArray<T> where T : struct
{

    public void ParseToBytes(ref byte[] targetArray, in int elementIndex, in T toSet);
    public void ParseFromBytes(in byte[] targetArray, in int elementIndex, out T toSet);
    public int GetElementBytesSize();

}

public abstract class A_GenericBiDirectionalParseByteInArray<T> : I_GenericBiDirectionalParseByteInArray<T> where T : struct
{
    public abstract void ParseFromBytes(in byte[] targetArray, in int elementIndex, out T toSet);
    public abstract void ParseToBytes(ref byte[] targetArray, in int elementIndex, in T toSet);
    public abstract int GetElementBytesSize();
    public void GetOffsetInArray(in int index, out int offset) => offset = index * GetElementBytesSize();
    public void GetArrayBytesCountFor(in int numberOfElements, out int arraySizeInBytes) => arraySizeInBytes = numberOfElements * GetElementBytesSize();

}

public class DemoPriStructParserArray : A_GenericBiDirectionalParseByteInArray<PrimitiveStructToParseSample>
{
    public override void ParseToBytes(ref byte[] targetArray, in int elementIndex, in PrimitiveStructToParseSample toSet)
    {
        int offset = GetElementBytesSize() * elementIndex;
        targetArray[offset] = toSet.m_byte;
        targetArray[offset+1] = (byte)toSet.m_sbyte;
        BitConverter.GetBytes(toSet.m_int).CopyTo(targetArray, offset + 2);
        BitConverter.GetBytes(toSet.m_uint).CopyTo(targetArray, offset + 6);
        BitConverter.GetBytes(toSet.m_long).CopyTo(targetArray, offset + 10);
        BitConverter.GetBytes(toSet.m_ulong).CopyTo(targetArray, offset + 18);
        BitConverter.GetBytes(toSet.m_short).CopyTo(targetArray, offset + 26);
        BitConverter.GetBytes(toSet.m_ushort).CopyTo(targetArray, offset + 28);
        BitConverter.GetBytes(toSet.m_float).CopyTo(targetArray, offset + 30);
        BitConverter.GetBytes(toSet.m_double).CopyTo(targetArray, offset + 34);
        targetArray[offset+42] = (byte)toSet.m_char0;
        targetArray[offset+43] = (byte)toSet.m_char1;
    }
    public override void ParseFromBytes(in byte[] targetArray, in int elementIndex, out PrimitiveStructToParseSample toSet)
    {
        toSet = new PrimitiveStructToParseSample();
        int offset = GetElementBytesSize() * elementIndex;
        toSet.m_byte = targetArray[offset];
        toSet.m_sbyte = (sbyte)targetArray[offset + 1];
        BitConverter.ToInt32(targetArray, offset + 2);
        BitConverter.ToUInt32(targetArray, offset + 6);
        BitConverter.ToInt64(targetArray, offset + 10);
        BitConverter.ToUInt64(targetArray, offset + 18);
        BitConverter.ToInt16(targetArray, offset + 26);
        BitConverter.ToUInt16(targetArray, offset + 28);
        BitConverter.ToSingle(targetArray, offset + 30);
        BitConverter.ToDouble(targetArray, offset + 34);
        toSet.m_char0 = (char)targetArray[offset + 42];
        toSet.m_char1 = (char)targetArray[offset + 43];
    }
    public override int GetElementBytesSize()
    {
        return 44;
    }
}




[System.Serializable]
public struct PrimitiveStructToParseSample {

    public byte m_byte;        // 1 bytes
    public sbyte m_sbyte;      // 1 bytes
    public int m_int;          // 4 bytes
    public uint m_uint;        // 4 bytes 10
    public long m_long;        // 8 bytes
    public ulong m_ulong;      // 8 bytes 26
    public short m_short;      // 2 bytes
    public ushort m_ushort;    // 2 bytes 30
    public float m_float;      // 4 bytes
    public double m_double;    // 8 bytes 42
    public char m_char0;       // 1 bytes
    public char m_char1;       // 1 bytes 44
}
