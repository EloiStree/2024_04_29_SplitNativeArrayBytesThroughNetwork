using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;


public abstract class VerifiedStructParserMono<T, D>: MonoBehaviour where D:struct,
    I_HowToParseElementInByteNativeArray<T>, I_HowToParseByteNativeArrayToElement<T> , I_ProvideRandomAndDefaultElementInJob<T>
    where T:struct
{

    public T [] m_in, m_out;
    public D m_parser;


   // [ContextMenu("Tick")]
    public abstract void CallParseCheck();

    //[ContextMenu("Tick")]
    public virtual void ParseCheck() {

        GenerateRandom();
        TestTheParserWithCurrentInValue();
    }


    //[ContextMenu("Generate Random")]
    public virtual void GenerateRandom() {

        for (int i = 0; i < m_in.Length; i++)
        {
            m_parser.GetRandom(out m_in[i]);
        }
    }


    //[ContextMenu("Test Parser")]
    public virtual void TestTheParserWithCurrentInValue()
    {
        if (m_in.Length != m_out.Length) {
            m_out = new T[m_in.Length];
        }
        I_HowToParseElementInByteNativeArray<T> t = (I_HowToParseElementInByteNativeArray<T>)m_parser;
        int size = t.GetSizeOfElementInBytesCount();
        NativeArray<byte> nArray = new NativeArray<byte>( size * m_in.Length , Allocator.TempJob);
        for (int i = 0; i < m_in.Length; i++)
        {
            m_parser.ParseBytesFromElement(nArray, i, m_in[i]);
            m_parser.ParseBytesToElement(nArray, i, out m_out[i]);
        }
        nArray.Dispose();

    }
}