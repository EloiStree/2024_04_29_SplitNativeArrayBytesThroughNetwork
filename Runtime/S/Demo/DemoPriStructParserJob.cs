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

    public void ParseBytesToUnusedValue(NativeArray<byte> source, in int indexElement)
    {
        throw new NotImplementedException();
    }
}



public struct StructParserJob_Integer : I_HowToParseElementInByteNativeArray<int>, I_HowToParseByteNativeArrayToElement<int>, I_ProvideRandomAndDefaultElementInJob<int>
{
    public static int m_elementSize = 4;
    public bool m_useBitConverterForTheTest;

    public void GetDefault(out int element)
    {
        element = 0;
    }

    public void GetRandom(out int element)
    {
        element = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
    }

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

    public void ParseBytesToUnusedValue(NativeArray<byte> source, in int indexElement)
    {
        int offset = m_elementSize * indexElement;
        source[offset + 0] = 0;
        source[offset + 1] = 0;
        source[offset + 2] = 0;
        source[offset + 3] = 0;
    }

    public void SetWithDefault(NativeArray<int> source, in int indexElement)
    {
        GetDefault(out int value);
        source[indexElement] = value;
    }

    public void SetWithRandom(NativeArray<int> source, in int indexElement)
    {
        GetRandom(out int value);
        source[indexElement] = value;
    }
}

public struct StructParserJob_Vector3 : I_HowToParseElementInByteNativeArray<Vector3>, I_HowToParseByteNativeArrayToElement<Vector3>, I_ProvideRandomAndDefaultElementInJob<Vector3>
{
    public static int m_elementSize = 12;
    public bool m_useBitConverterForTheTest;

    public Vector3 m_default;
  

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
    public void GetDefault(out Vector3 element)
    {
        element = m_default;
    }

    public void GetRandom(out Vector3 element)
    {
        element = new Vector3(R(), R(), R());
    }

    private float R()
    {
        return UnityEngine.Random.Range(float.MinValue, float.MaxValue);
    }

    public void SetWithDefault(NativeArray<Vector3> source, in int indexElement)
    {
        GetDefault(out Vector3 value);
        source[indexElement] = value;
    }

    public void SetWithRandom(NativeArray<Vector3> source, in int indexElement)
    {
        GetRandom(out Vector3 value);
        source[indexElement] = value;
    }

    public void ParseBytesToUnusedValue(NativeArray<byte> source, in int indexElement)
    {
        int offset = m_elementSize * indexElement;
        source[offset + 0] =0;
        source[offset + 1] =0;
        source[offset + 2] =0;
        source[offset + 3] =0;
        source[offset + 4] =0;
        source[offset + 5] =0;
        source[offset + 6] =0;
        source[offset + 7] =0;
        source[offset + 8] =0;
        source[offset + 9] =0;
        source[offset + 10]=0;
        source[offset + 11]=0;
    }
}

