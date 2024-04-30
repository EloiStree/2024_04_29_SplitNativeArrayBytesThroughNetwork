using System.Collections.Generic;
/// <summary>
/// Reason of it is that having big array in Mono in version 2022 of Unity create editor and runtime slow down bugs.
/// May be corrected in future but in waiting of it I stor the min a static dico.
/// </summary>
public class StaticByteArrayToChunkDicoStored { 

    public  static Dictionary<string, byte[]> m_dictionaryOfBytes = new Dictionary<string, byte[]>();
    public static Dictionary<string, byte[]> GetDictionary() { return m_dictionaryOfBytes; }
}
