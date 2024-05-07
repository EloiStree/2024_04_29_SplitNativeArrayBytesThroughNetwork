using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Events;

public class Quick_FullBytesToElementMono : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}



[System.Serializable]
public class ReconstructionByteToNativeArray<T, J> : I_HoldNativeArrayReconstructed<T>  where T : struct where J : struct, I_HowToParseByteNativeArrayToElement<T>, I_ProvideRandomAndDefaultElementInJob<T>
{

    private static J m_structParser = new();

    public NativeArray<T> m_elements;


    public ReconstructionByteToNativeArray(int arrayInitialSize) {
        m_elements = new NativeArray<T>(arrayInitialSize, Allocator.Persistent);
    }
    ~ReconstructionByteToNativeArray() {
        if(m_elements!=null && m_elements.IsCreated)
            m_elements.Dispose();
    } 

    public int m_elementInMaxByteArrayReceived;
    public void SetWithJobFromByte(NativeArray<byte> value)
    {
        int sizeOfElement =m_structParser.GetSizeOfElementInBytesCount();
        m_elementInMaxByteArrayReceived = value.Length / sizeOfElement;
        m_structParser.GetDefault(out T defaultStruct);

        if (m_elements.Length != m_elementInMaxByteArrayReceived) {

            if (m_elements == null || m_elements.IsCreated == false)
            {
                m_elements = new NativeArray<T>(m_elementInMaxByteArrayReceived, Allocator.Persistent);
                Debug.Log("New native array");
            }
            else
            {
                m_elements.Dispose();
                m_elements = new NativeArray<T>(m_elementInMaxByteArrayReceived, Allocator.Persistent);
                Debug.Log("New native array, Dispose previous");
            }
        }


        STRUCTJOB_ParseGenericBytesToStruct<T, J> job = new STRUCTJOB_ParseGenericBytesToStruct<T, J>()
        {
            m_toCopyInBytes = value,
            m_toCopyStruct = m_elements,
            m_default = defaultStruct,
            m_parser = m_structParser,
            m_maxElement = m_elementInMaxByteArrayReceived,
            m_elementToReconstruct= m_elementToReconstruct,
        };
        job.Schedule(m_elementInMaxByteArrayReceived, 64).Complete();
        m_onComplete.Invoke(m_elements);
    }
    public UnityEvent<NativeArray<T>> m_onComplete= new UnityEvent<NativeArray<T>>();
    public int m_elementToReconstruct;

    public NativeArray<T> GetCurrentNativeArray()
    {
        return m_elements;
    }
}


public interface I_HoldNativeArrayReconstructed <T> where T:struct
{
     
    NativeArray<T> GetCurrentNativeArray();
}

public class ReconstructionByteToNativeArrayMono<T,J> : MonoBehaviour, I_HoldNativeArrayReconstructed<T> where T : struct where J : struct, I_HowToParseByteNativeArrayToElement<T>, I_ProvideRandomAndDefaultElementInJob<T>
{
    public int m_initSize = 128;
    public int m_elementToReconstruct;
    public ReconstructionByteToNativeArray<T, J> m_reconstructionNativerArray = new ReconstructionByteToNativeArray<T,J>(128);

    public NativeArray<T> GetCurrentNativeArray() {
       return  m_reconstructionNativerArray.GetCurrentNativeArray();
    }

    public void PushIn(NativeArray<byte> value) {
        m_reconstructionNativerArray.m_elementToReconstruct = m_elementToReconstruct;
        m_reconstructionNativerArray.SetWithJobFromByte(value);
    }

    public void PushIn(byte[] value) {

        NativeArray<byte> t = new NativeArray<byte>(value, Allocator.TempJob);
        PushIn(t);
        t.Dispose();
    }
}


public class RangeDebugReconstructionByteToNativeArrayMono<T> : MonoBehaviour where T:struct{

   
    public int m_startIndex=10, m_endIndex = 100;
    public T[] m_sample;

   

    public  void PushIn(NativeArray<T> values)
    {
        int l = m_endIndex - m_startIndex;
        if (m_sample.Length != l)
            m_sample = new T[l];

        int j = 0;
        for (int i = m_startIndex; i < m_endIndex && i< values.Length; i++)
        {
            m_sample[j] = values[i];
            j++;
        }
    }

 
}