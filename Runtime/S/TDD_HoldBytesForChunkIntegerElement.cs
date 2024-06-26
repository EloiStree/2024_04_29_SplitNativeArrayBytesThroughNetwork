﻿using System.Collections;
using Unity.Collections;
using UnityEngine;

[System.Serializable]
public class TDD_HoldBytesForChunkIntegerElement : TDD_HoldBytesForChunkGenericElement<int, StructParserJob_Integer, StructRandomizerJob_Integer>
{
    [ContextMenu("Refresh")]
    public new void Refresh()
    {
        base.Refresh();
    }
    [ContextMenu("Random")]
    public new void RandomizeArrayWithForLoop()
    {
        base.RandomizeArrayWithForLoop();
    }
    [ContextMenu("Push Ref")]
    public new void PushChunksAsBytesRef()
    {
        base.PushChunksAsBytesRef();
    }
    [ContextMenu("Push Copy")]
    public new void PushChunksAsBytesCopy()
    {
        base.PushChunksAsBytesCopy();
    }

    [ContextMenu("Push Random With Ref")]
    public void PushRandomWithRef()
    {

        base.RandomizeArrayWithForLoop();
        base.PushChunksAsBytesRef();
    }
    [ContextMenu("Push Random With copy")]
    public void PushRandomWithCopy()
    {

        base.RandomizeArrayWithForLoop();
        base.PushChunksAsBytesCopy();
    }

}


