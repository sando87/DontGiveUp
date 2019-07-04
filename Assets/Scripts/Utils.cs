using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public static byte[] Serialize(object obj, int size)
    {
        var buffer = new byte[size];
        var gch = GCHandle.Alloc(buffer, GCHandleType.Pinned);
        var pBuffer = gch.AddrOfPinnedObject();
        Marshal.StructureToPtr(obj, pBuffer, false);
        gch.Free();

        return buffer;
    }
    public static void Deserialize<T>(ref T obj, byte[] data, int size = 0)
    {
        if(size > 0)
        {
            byte[] partData = new byte[size];
            Array.Copy(data, 0, partData, 0, size);
            var gch = GCHandle.Alloc(partData, GCHandleType.Pinned);
            Marshal.PtrToStructure(gch.AddrOfPinnedObject(), obj);
            gch.Free();
        }
        else
        {
            var gch = GCHandle.Alloc(data, GCHandleType.Pinned);
            Marshal.PtrToStructure(gch.AddrOfPinnedObject(), obj);
            gch.Free();
        }
    }
}
