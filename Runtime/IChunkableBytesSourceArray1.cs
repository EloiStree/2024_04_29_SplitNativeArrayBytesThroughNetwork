public interface IChunkableBytesSourceArray
{
    byte[] GetBytesArray();
    int GetBytesArrayLenght();
    int HowManyElementMaxAreStoreInCurrentArray();
    int SizeOfElementInByteStored();
}