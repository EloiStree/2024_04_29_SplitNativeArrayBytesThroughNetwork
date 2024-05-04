using System;
using Unity.Collections;
using UnityEngine;

[System.Serializable]
public class TDD_HoldBytesForChunkVector3Element : TDD_HoldBytesForChunkGenericElement<Vector3, StructParserJob_Vector3, StructRandomizerJob_Vector3>
{


    [ContextMenu("Refresh")]
    public new void Refresh()
    {
        base.Refresh();
    }
    [ContextMenu("Random")]
    public new void RandomizeArrayWithForLoop()
    {
        base.RandomizeArrayWithForLoop();
    }
    [ContextMenu("Push Ref")]
    public new void PushChunksAsBytesRef()
    {
        base.PushChunksAsBytesRef();
    }
     [ContextMenu("Push Random With Ref")]
    public void PushRandomWithRef() {

        base.RandomizeArrayWithForLoop();
        base.PushChunksAsBytesRef();
    }
    [ContextMenu("Push Random With copy")]
    public void PushRandomWithCopy()
    {

        base.RandomizeArrayWithForLoop();
        base.PushChunksAsBytesCopy();
    }
}




public struct StructRandomizerJob_Color32: I_ProvideRandomAndDefaultElementInJob<Color32>
{
    public void GetDefault(out Color32 element) =>
        element = new Color32(0,0,0,255);

    public void GetRandom(out Color32 element)
    {
        System.Random r = new System.Random();
        element = new Color32((byte)r.Next(0, 255), (byte)r.Next(0, 255), (byte)r.Next(0, 255), 255);
    }

    public void SetWithDefault(NativeArray<Color32> source, in int indexElement)
    {
        GetDefault(out Color32 v);
        source[indexElement] = v;
    }

    public void SetWithRandom(NativeArray<Color32> source, in int indexElement)
    {

        GetRandom(out Color32 v);
        source[indexElement] = v;
    }
}


public struct StructParserJob_Color32 : I_HowToParseElementInByteNativeArray<Color32>, I_HowToParseByteNativeArrayToElement<Color32>, I_ProvideRandomAndDefaultElementInJob<Color32>
{
    public static int m_elementSize = 3;
    public bool m_useBitConverterForTheTest;

    public Color32 m_default;


    public int GetSizeOfElementInBytesCount()
    {
        return m_elementSize;
    }

    public void ParseBytesFromElement(NativeArray<byte> source, in int indexElement, in Color32 toSet)
    {
        int offset = m_elementSize * indexElement;
        source[offset + 0] = toSet.r;
        source[offset + 1] = toSet.g;
        source[offset + 2] = toSet.b;
    }


    public void ParseBytesToElement(NativeArray<byte> source, in int indexElement, out Color32 element)
    {
        int offset = m_elementSize * indexElement;
        element = new Color32(source[offset + 0], source[offset + 1], source[offset + 2],255);    
    }

    public void GetDefault(out Color32 element)
    {
        element = m_default;
    }

    public void GetRandom(out Color32 element)
    {
        element = new Color32(R(), R(), R(),255);
    }

    private byte R()
    {
        return (byte) UnityEngine.Random.Range(0,255);
    }

    public void SetWithDefault(NativeArray<Color32> source, in int indexElement)
    {
        GetDefault(out Color32 value);
        source[indexElement] = value;
    }

    public void SetWithRandom(NativeArray<Color32> source, in int indexElement)
    {
        GetRandom(out Color32 value);
        source[indexElement] = value;
    }
}

