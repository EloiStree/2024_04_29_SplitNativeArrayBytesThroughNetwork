using UnityEngine;

public class ByteCountMono : MonoBehaviour
{

    public ulong m_lastReceived;
    public ulong m_byteCount;
    public double m_kiloByte;
    public double m_megaByte;
    public double m_gigaByte;
    public double m_teraByte;

    public void AddBytesCount(int byteCount)
    {

        AddBytesCount((ulong)byteCount);
    }
    public void AddBytesCount(ulong byteCount)
    {
        m_lastReceived = byteCount;
        m_byteCount += (ulong)byteCount;

       m_kiloByte= m_byteCount / 1024.0;
        if (m_kiloByte < 0.00001) m_kiloByte = 0.00001;
       m_megaByte= m_byteCount / 1024.0 / 1024.0;
        if (m_megaByte < 0.00001) m_megaByte = 0.00001;
        m_gigaByte = m_byteCount / 1024.0 / 1024.0 / 1024.0;
        if (m_gigaByte < 0.00001) m_gigaByte = 0.00001;
        m_teraByte = m_byteCount / 1024.0 / 1024.0 / 1024.0 / 1024.0;
        if (m_teraByte < 0.00001) m_teraByte = 0.00001;
    }
}
