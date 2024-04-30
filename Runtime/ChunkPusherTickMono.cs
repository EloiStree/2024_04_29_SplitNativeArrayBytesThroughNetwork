using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ChunkPusherTickMono :MonoBehaviour{

    public bool m_usePushRepeatDefault = true;
    public float m_timeBetweenPush = 1f / 4f;
    public UnityEvent m_onTick;
    private IEnumerator Start()
    {

        while (true) { 
            yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(m_timeBetweenPush);
            if (m_usePushRepeatDefault)
            {
                m_onTick.Invoke();
            }
        }
       
    }

}
