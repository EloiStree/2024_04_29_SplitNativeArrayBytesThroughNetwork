using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChunkStaticBetweenSceneTunnelMono : MonoBehaviour
{

    public static Action<byte[]> m_onChunkActionReceived;
    public UnityEvent<byte[]> m_onChunkUnityReceived;


    public void Awake()
    {
        m_onChunkActionReceived+= m_onChunkUnityReceived.Invoke;
    }
    private void OnDestroy()
    {
        m_onChunkActionReceived -= m_onChunkUnityReceived.Invoke;
    }

    public void PushIn(byte[] bytesToPush) {

        if (m_onChunkActionReceived != null)
            m_onChunkActionReceived.Invoke(bytesToPush);

    }

}
