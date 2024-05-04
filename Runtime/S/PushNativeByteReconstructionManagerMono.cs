using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PushNativeByteReconstructionManagerMono : MonoBehaviour
{

    public ByteChunkReconstructionManagerMono m_source;

    public ByteReconstructionToNativeArray[] m_redirection;

    [System.Serializable]
    public class ByteReconstructionToNativeArray {
        public string m_reminder = "Redirection";
        public byte m_arrayReconstructionId;
        public UnityEvent<NativeArray<byte>> m_onFrameReceived;
    }

    void Awake()
    {
        m_source.m_onChunkEndReceivedEvent += EndChunkReceived;
    }

    private void EndChunkReceived(ReconstructBytesArray source)
    {
        if (source == null)
            return;
        foreach (var item in m_redirection)
        {
            if (item.m_arrayReconstructionId == source.m_arrayIdOfReconstruction) {
                item.m_onFrameReceived.Invoke(source.GetNativeArrayAndRefreshBefore());
            }
        }
    }

}
