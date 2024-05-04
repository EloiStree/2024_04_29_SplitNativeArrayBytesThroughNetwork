using UnityEngine;

public class ByteToNativeVector3Array : ReconstructionByteToNativeArray<Vector3, StructParserJob_Vector3>
{
    public ByteToNativeVector3Array(int arrayInitialSize) : base(arrayInitialSize)
    {
    }
}
