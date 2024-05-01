using UnityEngine;

public class ChunkableBytesSourceArrayDefaultMbpsMon : MonoBehaviour {

    public AbstractByteArrayReferenceSetGetMono m_source;
    

    [Header("Debug estimation")]
    public int m_arraySizeInBits;
    public int m_arraySizeInKiloBits;
    public int m_arraySizeInMegaBits;
    public int m_arraySizeInByte;
    public int m_arraySizeInKiloBytes;
    public int m_arraySizeInMegaByte;


    [Header("Debug per second estimation")]
    public int m_frameRate = 8;
    public int m_arraySizeInBitsPerSecond;
    public int m_arraySizeInKiloBitsPerSecond;
    public int m_arraySizeInMegaBitsPerSecond;
    public int m_arraySizeInBytePerSecond;
    public int m_arraySizeInKiloBytesPerSecond;
    public int m_arraySizeInMegaBytePerSecond;


    public void Reset()
    {
        m_source = GetComponent<AbstractByteArrayReferenceSetGetMono>();
    }


    private void OnValidate()
    {
        if (m_source == null)
            return;
        m_arraySizeInByte = m_source.GetSizeOfElementInByteStored() * m_source.GetHowManyElementMaxAreStoreInCurrentArray();
        m_arraySizeInKiloBytes = m_arraySizeInByte / 1024;
        m_arraySizeInMegaByte = m_arraySizeInKiloBytes / 1024;
        m_arraySizeInBits = m_arraySizeInByte * 8;
        m_arraySizeInKiloBits = m_arraySizeInBits / 1024;
        m_arraySizeInMegaBits = m_arraySizeInKiloBits / 1024;

        m_arraySizeInBitsPerSecond = m_arraySizeInBits * m_frameRate;
        m_arraySizeInKiloBitsPerSecond = m_arraySizeInBitsPerSecond / 1024;
        m_arraySizeInMegaBitsPerSecond = m_arraySizeInKiloBitsPerSecond / 1024;
        m_arraySizeInBytePerSecond = m_arraySizeInByte * m_frameRate;
        m_arraySizeInKiloBytesPerSecond = m_arraySizeInBytePerSecond / 1024;
        m_arraySizeInMegaBytePerSecond = m_arraySizeInKiloBytesPerSecond / 1024;

    }
}
