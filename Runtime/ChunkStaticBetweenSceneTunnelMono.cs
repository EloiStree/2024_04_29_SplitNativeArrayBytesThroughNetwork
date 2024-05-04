using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ChunkStaticBetweenSceneTunnelMono : MonoBehaviour
{

    public static Action<byte[]> m_onChunkActionReceived;
    public UnityEvent<byte[]> m_onChunkUnityReceived;

    //public bool m_useCopyQueue;

    //private Queue<byte[]> m_copyQueue = new Queue<byte[]>();

    public void Awake()
    {
        m_onChunkActionReceived+= m_onChunkUnityReceived.Invoke;
    }
    private void OnDestroy()
    {
        m_onChunkActionReceived -= m_onChunkUnityReceived.Invoke;
    }

    //public void Update()
    //{
    //    while (m_copyQueue.Count > 0)
    //    {

    //        byte[] b = m_copyQueue.Dequeue();
    //        if (m_onChunkActionReceived != null)
    //            m_onChunkActionReceived.Invoke(b);

    //    }
    //}

    public void PushIn(byte[] bytesToPush) {

        //if (m_useCopyQueue)
        //{
        //    m_copyQueue.Enqueue(bytesToPush.ToArray());
        //    while ( m_copyQueue.Count > 0 ) {

        //        byte[] b = m_copyQueue.Dequeue();
        //        if (m_onChunkActionReceived != null)
        //            m_onChunkActionReceived.Invoke(b);

        //    }
        //}
        //else { 
            if (m_onChunkActionReceived != null)
                m_onChunkActionReceived(bytesToPush);
        //}

    }

}
