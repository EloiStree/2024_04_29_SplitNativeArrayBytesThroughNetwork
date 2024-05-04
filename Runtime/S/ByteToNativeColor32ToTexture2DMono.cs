using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ByteToNativeColor32ToTexture2DMono : MonoBehaviour
{
    public ByteToNativeColor32ArrayMono m_source;
    public Texture2D m_rawTexture;


    public int m_textureSize;
    public int m_arraySize;
    public long m_lastUpdate;


    public UnityEvent<Texture2D> m_onImageUpdate;
    [ContextMenu("update image")]
    public void UpdateImage()
    {
        
        NativeArray<Color32> c = m_source.GetCurrentNativeArray();
        if (c == null)
            return;
        int lenght = c.Length;
        m_arraySize = lenght;
        if (lenght <= 0)
            return;

        if (m_rawTexture == null) { 
            float w = Mathf.Sqrt(lenght);
            int x = (int) w;
            float m = w - x;
            if (m == 0) { 
            
                m_rawTexture = new Texture2D(x, x);        
            }
        }
        if (m_rawTexture == null)
            return;
        m_textureSize = m_rawTexture.width * m_rawTexture.height;
        if (m_rawTexture != null && c.Length== m_rawTexture.width*m_rawTexture.height) {
            m_lastUpdate = (long)Time.time;
            m_rawTexture.SetPixels32(c.ToArray());
            m_rawTexture.Apply();
            m_onImageUpdate.Invoke(m_rawTexture);
        }
    }
}
