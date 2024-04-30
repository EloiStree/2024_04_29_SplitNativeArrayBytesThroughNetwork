using System;
using System.Collections.Generic;
using UnityEngine;

public class PackageChunkToBytesCollectionMono :MonoBehaviour { 

    public List<LinkedByteIdToBytesArrayMono> m_byteArrayIdToCollection= new List<LinkedByteIdToBytesArrayMono>();


    public void PushIn(PackageChunkReceivedWithBytes chunk) {

        foreach (LinkedByteIdToBytesArrayMono linked in m_byteArrayIdToCollection)
        {
            if (linked.m_arrayByteId == chunk.m_arrayGivenId)
            {

                AbstractByteArrayReferenceSetGetMono array = linked.m_arrayReference;
                if (array == null)
                    return;

                byte[] bytes =array.GetBytesArray();

                if(bytes.Length < chunk.m_offsetInArray + chunk.m_offsetLenght)
                    return;

                Buffer.BlockCopy(chunk.m_allBytesReceived, 41, bytes, chunk.m_offsetInArray, chunk.m_offsetLenght);
                return;
            }
        }
    }


}
