using System;
using Unity.Collections;
using UnityEngine;

public struct DemoPriStructParserJob : I_HowToParseElementInByteNativeArray<PrimitiveStructToParseSample>
{
    public static int m_elementSize = 44;
    public bool m_useBitConverterForTheTest;

    public int GetSizeOfElementInBytesCount()
    {
        return m_elementSize;
    }

    public void ParseBytesFromElement(NativeArray<byte> source, in int indexElement, in PrimitiveStructToParseSample toSet)
    {
        int offset = m_elementSize * indexElement;

        if (m_useBitConverterForTheTest)
        {
            byte[] targetArray = new byte[44];
            BitConverter.GetBytes(toSet.m_uint).CopyTo(targetArray, 6);
            BitConverter.GetBytes(toSet.m_long).CopyTo(targetArray, 10);
            BitConverter.GetBytes(toSet.m_ulong).CopyTo(targetArray, 18);
            BitConverter.GetBytes(toSet.m_short).CopyTo(targetArray, 26);
            BitConverter.GetBytes(toSet.m_ushort).CopyTo(targetArray, 28);
            BitConverter.GetBytes(toSet.m_float).CopyTo(targetArray, 30);
            BitConverter.GetBytes(toSet.m_double).CopyTo(targetArray, 34);
            for (int i = 6; i < 42; i++)
            {
                source[offset + i] = targetArray[i];
            }
        }

        source[offset + 0] = toSet.m_byte;
        source[offset + 1] = (byte)toSet.m_sbyte;
        source[offset + 2] = (byte)(toSet.m_int & 0xFF);
        source[offset + 3] = (byte)((toSet.m_int >> 8) & 0xFF);
        source[offset + 4] = (byte)((toSet.m_int >> 16) & 0xFF);
        source[offset + 5] = (byte)((toSet.m_int >> 24) & 0xFF);
        source[offset + 42] = (byte)toSet.m_char0;
        source[offset + 43] = (byte)toSet.m_char1;

        
        //DemoGenericBiDirectionalParseByte p = new DemoGenericBiDirectionalParseByte();
        //p.ParseToBytes(source, in indexElement, in element);
    }
}



public struct StructParserJob_Integer : I_HowToParseElementInByteNativeArray<int>, I_HowToParseByteNativeArrayToElement<int>
{
    public static int m_elementSize = 4;
    public bool m_useBitConverterForTheTest;

    public int GetSizeOfElementInBytesCount()
    {
        return m_elementSize;
    }

    public void ParseBytesFromElement(NativeArray<byte> source, in int indexElement, in int toSet)
    {
        int offset = m_elementSize * indexElement;
        source[offset + 0] = (byte)(toSet & 0xFF);
        source[offset + 1] = (byte)((toSet >> 8) & 0xFF);
        source[offset + 2] = (byte)((toSet >> 16) & 0xFF);
        source[offset + 3] = (byte)((toSet >> 24) & 0xFF);
    }

    public void ParseBytesToElement(NativeArray<byte> source, in int indexElement, out int element)
    {
        int offset = m_elementSize * indexElement;
        element = (source[offset + 3] << 24) | (source[offset + 2] << 16) | (source[offset + 1] << 8) | source[offset + 0];
    }
}

public struct StructParserJob_Vector3 : I_HowToParseElementInByteNativeArray<Vector3>, I_HowToParseByteNativeArrayToElement<Vector3>
{
    public static int m_elementSize = 12;
    public bool m_useBitConverterForTheTest;
    public int GetSizeOfElementInBytesCount()
    {
        return m_elementSize;
    }

    public void ParseBytesFromElement(NativeArray<byte> source, in int indexElement, in Vector3 toSet)
    {
        int offset = m_elementSize * indexElement;
        byte[] bytes = BitConverter.GetBytes(toSet.x);
        source[offset + 0] = bytes[0];
        source[offset + 1] = bytes[1];
        source[offset + 2] = bytes[2];
        source[offset + 3] = bytes[3];
         bytes = BitConverter.GetBytes(toSet.y);
        source[offset + 4] = bytes[0];
        source[offset + 5] = bytes[1];
        source[offset + 6] = bytes[2];
        source[offset + 7] = bytes[3];
        bytes = BitConverter.GetBytes(toSet.z);
        source[offset + 8]  = bytes[0];
        source[offset + 9]  = bytes[1];
        source[offset + 10] = bytes[2];
        source[offset + 11] = bytes[3];
    }

 
    public void ParseBytesToElement(NativeArray<byte> source, in int indexElement, out Vector3 element)
    {
        int offset = m_elementSize * indexElement;
        element = new Vector3();
        byte[] b4 = new byte[4];
        b4[0] = source[offset + 0];
        b4[1] = source[offset + 1];
        b4[2] = source[offset + 2];
        b4[3] = source[offset + 3];
        element.x = BitConverter.ToSingle(b4, 0);
        b4[0] = source[offset + 4];
        b4[1] = source[offset + 5];
        b4[2] = source[offset + 6];
        b4[3] = source[offset + 7];
        element.y = BitConverter.ToSingle(b4, 0);
        b4[0] = source[offset + 8];
        b4[1] = source[offset + 9];
        b4[2] = source[offset + 10];
        b4[3] = source[offset + 11];
        element.z = BitConverter.ToSingle(b4, 0);
    }
}

