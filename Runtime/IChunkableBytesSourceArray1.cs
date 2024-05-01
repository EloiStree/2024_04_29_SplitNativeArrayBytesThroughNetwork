public interface IChunkableBytesSourceArray
{
    byte[] GetBytesArray();
    int GetBytesArrayLenght();
    int GetHowManyElementMaxAreStoreInCurrentArray();
    int GetSizeOfElementInByteStored();
}