using System;

[System.Serializable]
public struct ChunkByteArrayWithReconstructionId
{


    public byte[] m_chunkArray;
    public byte m_arrayId;
    public uint m_frameId;
    public uint m_chunkId;
    public uint m_offsetStart;
    public uint m_offsetLength;

    public int GetHeadSizeInBytes() { return 21; }
    public ChunkByteArrayWithReconstructionId(int chunkSizeToStore)
    {
        m_chunkArray = new byte[chunkSizeToStore + 21];
        m_arrayId = 0;
        m_frameId = 0;
        m_chunkId = 0;
        m_offsetStart = 0;
        m_offsetLength = (uint)Math.Abs(chunkSizeToStore);
    }



    public void SetChunkArray(byte[] chunkByteArray)
    {
        m_chunkArray = chunkByteArray;
    }

    public void SetArrayId(byte arrayId)
    {
        m_arrayId = arrayId;
        m_chunkArray[0] = arrayId;
    }

    public void SetFrameId(uint frameId)
    {
        m_frameId = frameId;
        m_chunkArray[1] = (byte)(frameId & 0xFF);
        m_chunkArray[2] = (byte)((frameId >> 8) & 0xFF);
        m_chunkArray[3] = (byte)((frameId >> 16) & 0xFF);
        m_chunkArray[4] = (byte)((frameId >> 24) & 0xFF);
    }
    public void SetChunkID(uint chunkId)
    {
        m_chunkId = chunkId;
        m_chunkArray[5] = (byte)(chunkId & 0xFF);
        m_chunkArray[6] = (byte)((chunkId >> 8) & 0xFF);
        m_chunkArray[7] = (byte)((chunkId >> 16) & 0xFF);
        m_chunkArray[8] = (byte)((chunkId >> 24) & 0xFF);
    }
    public void SetByteArrayOffset(uint offsetInArray)
    {
        m_offsetStart = offsetInArray;
        m_chunkArray[9] = (byte)(offsetInArray & 0xFF);
        m_chunkArray[10] = (byte)((offsetInArray >> 8) & 0xFF);
        m_chunkArray[11] = (byte)((offsetInArray >> 16) & 0xFF);
        m_chunkArray[12] = (byte)((offsetInArray >> 24) & 0xFF);
    }
    public void SetByteArrayOffsetLength(uint offsetLength)
    {
        m_offsetLength = offsetLength;
        m_chunkArray[13] = (byte)(offsetLength & 0xFF);
        m_chunkArray[14] = (byte)((offsetLength >> 8) & 0xFF);
        m_chunkArray[15] = (byte)((offsetLength >> 16) & 0xFF);
        m_chunkArray[16] = (byte)((offsetLength >> 24) & 0xFF);
    }
    public void SetByteArrayMaxLenght(uint maxLenght)
    {
        m_offsetLength = maxLenght;
        m_chunkArray[17] = (byte)(maxLenght & 0xFF);
        m_chunkArray[18] = (byte)((maxLenght >> 8) & 0xFF);
        m_chunkArray[19] = (byte)((maxLenght >> 16) & 0xFF);
        m_chunkArray[20] = (byte)((maxLenght >> 24) & 0xFF);
    }
    public uint GetFrameId()
    {
        return (uint)((m_chunkArray[4] << 24) | (m_chunkArray[3] << 16) | (m_chunkArray[2] << 8) | m_chunkArray[1]);
    }
    public uint GetChunkId()
    {
        return (uint)((m_chunkArray[8] << 24) | (m_chunkArray[7] << 16) | (m_chunkArray[6] << 8) | m_chunkArray[5]);
    }
    public uint GetArrayOffset()
    {
        return (uint)((m_chunkArray[12] << 24) | (m_chunkArray[11] << 16) | (m_chunkArray[10] << 8) | m_chunkArray[9]);
    }
    public uint GetArrayOffestToCopy()
    {
        return (uint)((m_chunkArray[16] << 24) | (m_chunkArray[15] << 16) | (m_chunkArray[14] << 8) | m_chunkArray[13]);
    }
    public uint GetArrayMaxLength()
    {
        return (uint)((m_chunkArray[20] << 24) | (m_chunkArray[19] << 16) | (m_chunkArray[18] << 8) | m_chunkArray[17]);
    }

    public static void GetHeaderOfChunkReceived(in byte[] m_chunkArray, out byte arrayId,
        out uint framedId, out uint chunkId, out uint arrayOffset, out uint arrayOffsetLenght, out uint arrayMaxLength)
    {
        arrayId = m_chunkArray[0];
        framedId = (uint)((m_chunkArray[4] << 24) | (m_chunkArray[3] << 16) | (m_chunkArray[2] << 8) | m_chunkArray[1]);
        chunkId = (uint)((m_chunkArray[8] << 24) | (m_chunkArray[7] << 16) | (m_chunkArray[6] << 8) | m_chunkArray[5]);
        arrayOffset = (uint)((m_chunkArray[12] << 24) | (m_chunkArray[11] << 16) | (m_chunkArray[10] << 8) | m_chunkArray[9]);
        arrayOffsetLenght = (uint)((m_chunkArray[16] << 24) | (m_chunkArray[15] << 16) | (m_chunkArray[14] << 8) | m_chunkArray[13]);
        arrayMaxLength = (uint)((m_chunkArray[20] << 24) | (m_chunkArray[19] << 16) | (m_chunkArray[18] << 8) | m_chunkArray[17]);
    }

    public static void GetFramedIdOfChunkHead(byte[] chunkWithHeadRecevied, out uint framedId)
    {
        framedId = (uint)((chunkWithHeadRecevied[4] << 24) | (chunkWithHeadRecevied[3] << 16) | (chunkWithHeadRecevied[2] << 8) | chunkWithHeadRecevied[1]);
        
    }

    public static void GetMaxSizeOfArrayForChunk(byte[] chunkWithHeadRecevied, out bool found,out  uint maxSize)
    {
        if (chunkWithHeadRecevied == null || chunkWithHeadRecevied.Length < 21) {
            found = false;
            maxSize = 0;
            return;
        }
        else
        {
           found = true;
           maxSize = (uint)((chunkWithHeadRecevied[20] << 24) | (chunkWithHeadRecevied[19] << 16) | (chunkWithHeadRecevied[18] << 8) | chunkWithHeadRecevied[17]);

        }
    }
}