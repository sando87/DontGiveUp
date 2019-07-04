using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Linq;

public class Loader
{
    const string PathMetaInfo = "/MyRes/MetaInfo/";
    const string PathGeoMetry = "/MyRes/Geometry/";
    const string PathImages = "/MyRes/Images/";
    const string PathAnimations = "/MyRes/Animations/";

    private RenderContext Info = null;
    private List<Vector3> Positions = new List<Vector3>();
    private List<Vector3> Normals = new List<Vector3>();
    private List<BoneWeight> Weights = new List<BoneWeight>();
    private List<int> Triangles = new List<int>();
    private List<Matrix4x4[]> Matrixs = new List<Matrix4x4[]>();
    private MyRes_CreateLayout Layout = null;

    public bool LoadRenderInfo(string filename)
    {
        string fullname = Application.dataPath + PathMetaInfo + filename;
        if (!File.Exists(fullname))
            return false;

        byte[] data = File.ReadAllBytes(fullname);
        Info = new RenderContext();
        Utils.Deserialize(ref Info, data);

        if (!LoadLayout(Info.layout_addr))
            return false;
        if (!LoadPositions(Info.vb[0].addr))
            return false;
        if (!LoadNormals(Info.vb[0].addr))
            return false;
        if (!LoadWeights(Info.vb[0].addr))
            return false;
        if (!LoadTriangles(Info.ib_addr))
            return false;
        if (!LoadMatrixs())
            return false;

        return true;
    }

    bool LoadLayout(UInt64 addr)
    {
        string filename = FIndResourceFile(addr);
        if (filename.Length <= 0)
            return false;

        string fullname = Application.dataPath + PathGeoMetry + filename;
        byte[] data = File.ReadAllBytes(fullname);
        Layout = new MyRes_CreateLayout();
        Utils.Deserialize(ref Layout, data);
        return true;
    }

    bool LoadPositions(UInt64 addr)
    {
        string filename = FIndResourceFile(addr);
        if (filename.Length <= 0)
            return false;

        string fullname = Application.dataPath + PathGeoMetry + filename;
        byte[] data = File.ReadAllBytes(fullname);
        MyRes_CreateBuffer vert = new MyRes_CreateBuffer();
        int headerSize = Marshal.SizeOf(vert);
        Utils.Deserialize(ref vert, data, headerSize);
        int stride = (int)Info.vb[0].strides[0];
        uint vertCount = vert.desc.ByteWidth / (uint)stride;
        int startIdx = headerSize;
        for(int i=0;i<vertCount; ++i)
        {
            Vector3 pos = new Vector3();
            pos.x = BitConverter.ToSingle(data, startIdx);
            pos.y = BitConverter.ToSingle(data, startIdx + 4);
            pos.z = BitConverter.ToSingle(data, startIdx + 8);
            Positions.Add(pos);
            startIdx += stride;
        }
        return true;
    }

    bool LoadTriangles(UInt64 addr)
    {
        string filename = FIndResourceFile(addr);
        if (filename.Length <= 0)
            return false;

        string fullname = Application.dataPath + PathGeoMetry + filename;
        byte[] data = File.ReadAllBytes(fullname);
        MyRes_CreateBuffer vert = new MyRes_CreateBuffer();
        int headerSize = Marshal.SizeOf(vert);
        Utils.Deserialize(ref vert, data, headerSize);
        int startIdx = headerSize;
        for (int i = 0; i < Info.draw_IndexCount; ++i)
        {
            UInt16 index = BitConverter.ToUInt16(data, startIdx);
            Triangles.Add(index);
            startIdx += 2;
        }
        return true;
    }

    bool LoadNormals(UInt64 addr)
    {
        string filename = FIndResourceFile(addr);
        if (filename.Length <= 0)
            return false;

        string fullname = Application.dataPath + PathGeoMetry + filename;
        byte[] data = File.ReadAllBytes(fullname);
        MyRes_CreateBuffer vert = new MyRes_CreateBuffer();
        int headerSize = Marshal.SizeOf(vert);
        Utils.Deserialize(ref vert, data, headerSize);
        int stride = (int)Info.vb[0].strides[0];
        uint vertCount = vert.desc.ByteWidth / (uint)stride;
        int startIdx = headerSize + 0xc;
        for (int i = 0; i < vertCount; ++i)
        {
            Vector3 nor = new Vector3();
            nor.x = (data[startIdx + 0] / 128.0f) - 1.0f;
            nor.y = (data[startIdx + 4] / 128.0f) - 1.0f;
            nor.z = (data[startIdx + 8] / 128.0f) - 1.0f;
            Normals.Add(nor);
            startIdx += stride;
        }
        return true;
    }

    bool LoadWeights(UInt64 addr)
    {
        string filename = FIndResourceFile(addr);
        if (filename.Length <= 0)
            return false;

        string fullname = Application.dataPath + PathGeoMetry + filename;
        byte[] data = File.ReadAllBytes(fullname);
        MyRes_CreateBuffer vert = new MyRes_CreateBuffer();
        int headerSize = Marshal.SizeOf(vert);
        Utils.Deserialize(ref vert, data, headerSize);
        int stride = (int)Info.vb[0].strides[0];
        uint vertCount = vert.desc.ByteWidth / (uint)stride;
        int startIdxOfIdx = headerSize + 0x20;
        int startIdxOfWeight = headerSize + 0x24;
        for (int i = 0; i < vertCount; ++i)
        {
            BoneWeight info = new BoneWeight();
            info.boneIndex0 = data[startIdxOfIdx + 0]; //BitConverter.ToSingle(vert.data, startIdxOfIdx);
            info.boneIndex1 = data[startIdxOfIdx + 1]; //BitConverter.ToSingle(vert.data, startIdxOfIdx + 4);
            info.boneIndex2 = data[startIdxOfIdx + 2]; //BitConverter.ToSingle(vert.data, startIdxOfIdx + 8);
            info.boneIndex3 = data[startIdxOfIdx + 3];  //BitConverter.ToSingle(vert.data, startIdxOfIdx + 8);

            info.weight0 = BitConverter.ToSingle(data, startIdxOfWeight);
            info.weight1 = BitConverter.ToSingle(data, startIdxOfWeight + 4);
            info.weight2 = BitConverter.ToSingle(data, startIdxOfWeight + 8);
            info.weight3 = 1.0f - (info.weight0 + info.weight1 + info.weight2);
            info.weight3 = info.weight3 < 0 ? 0 : info.weight3;
            info.weight3 = info.weight3 > 1 ? 1 : info.weight3;

            Weights.Add(info);
            startIdxOfIdx += stride;
            startIdxOfWeight += stride;
        }
        return true;
    }

    bool LoadMatrixs()
    {
        string fullname = Application.dataPath + PathAnimations + "CBMatrix.bin";
        if (!File.Exists(fullname))
            return false;

        byte[] data = File.ReadAllBytes(fullname);
        int packSize = Marshal.SizeOf(new MyRes_CreateMatrix());
        int cnt = data.Length / packSize;
        int off = 0;
        for(int i = 0; i<cnt; ++i)
        {
            MyRes_CreateMatrix matInfo = new MyRes_CreateMatrix();
            byte[] buf = new byte[packSize];
            Array.Copy(data, off, buf, 0, packSize);
            Utils.Deserialize(ref matInfo, buf);
            Matrixs.Add(matInfo.ConvertTo());
            off += packSize;
        }

        return true;
    }

    string FIndResourceFile(UInt64 addr)
    {
        string dirPath = Application.dataPath + PathGeoMetry;
        string[] entries = Directory.GetFiles(dirPath);
        foreach (string name in entries)
        {
            UInt64 cur = UInt64.Parse(name.Split('_')[1], System.Globalization.NumberStyles.HexNumber);
            if (cur == addr)
            {
                var list = name.Split('/');
                return list[list.Length - 1];
            }
        }
        return "";
    }

    public Vector3[] GetPositions()
    {
        return Positions.ToArray();
    }
    public Vector3[] GetNormals()
    {
        return Normals.ToArray();
    }
    public int[] GetTriangles()
    {
        return Triangles.ToArray();
    }
    public BoneWeight[] GetWeights()
    {
        return Weights.ToArray();
    }
    public Matrix4x4[] GetMatrixs(int index)
    {
        if (Matrixs.Count <= 0)
            return null;

        int idx = index % Matrixs.Count;
        return Matrixs[idx];
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    struct MyResBase
    {
        public UInt32 type;
        public UInt64 addr;
        public UInt32 crc;
        public UInt32 totalSize;
        public UInt32 reserve1;
        public UInt32 reserve2;
        public UInt32 reserve3;
    };
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    struct MyResDescBuffer
    {
        public UInt32 ByteWidth;
        public UInt32 Usage;
        public UInt32 BindFlags;
        public UInt32 CPUAccessFlags;
        public UInt32 MiscFlags;
        public UInt32 StructureByteStride;
    };
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    class MyRes_CreateMatrix
    {
        public MyResBase head; // reserve1 = vertexstride;
        public MyResDescBuffer desc;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 45 * 3)]
        public Vector4[] vectors;
        public Matrix4x4[] ConvertTo()
        {
            Matrix4x4[] list = new Matrix4x4[45];
            for (int i = 0; i < 45; ++i)
            {
                list[i] = new Matrix4x4(
                    vectors[i * 3 + 0],
                    vectors[i * 3 + 1],
                    vectors[i * 3 + 2],
                    new Vector4(0, 0, 0, 1)
                    );
            }
            return list;
        }
    };
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    class MyRes_CreateBuffer
    {
        public MyResBase head; // reserve1 = vertexstride;
        public MyResDescBuffer desc;
    };
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    struct MyResLayout
    {
        public UInt64 SemanticName;
        public UInt32 SemanticIndex;
        public UInt32 Format;
        public UInt32 InputSlot;
        public UInt32 AlignedByteOffset;
        public UInt32 InputSlotClass;
        public UInt32 InstanceDataStepRate;
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    class MyRes_CreateLayout
    {
        public MyResBase head; // reserve1 = numElements;
        [MarshalAs(UnmanagedType.ByValArray)]
        public Byte[] layouts;
    };



    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    struct VBInfo
    {
        public UInt64 addr;
        public UInt32 numBuf;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public UInt32[] strides;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public UInt32[] offset;
        public bool isDirty;
    };
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    struct TEXInfo
    {
        public UInt64 addr;
        public UInt32 NumViews;
        public bool isDirty;
    };
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    struct SSInfo
    {
        public UInt64 addr;
        public UInt32 NumSamplers;
        public bool isDirty;
    };
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    struct MapInfo
    {
        public UInt64 addr;
        public UInt32 subRes;
        public UInt32 type;
        public UInt32 flags;
        public UInt64 pData;
        public UInt32 RowPitch;
        public UInt32 DepthPitch;
        public bool isDirty;
    };
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    class RenderContext
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public VBInfo[] vb;

        public UInt64 ib_addr;
        public UInt32 ib_format;
        public UInt32 ib_offset;
        public bool ib_isDirty;

        public UInt32 prim_topology;
        public bool prim_isDirty;

        public MapInfo CBMain;
        public MapInfo CBBones;
        public MapInfo CBLights;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public TEXInfo[] tex;

        public UInt64 layout_addr;
        public bool layout_isDirty;

        public UInt64 vs_addr;
        public UInt32 vs_NumClassInstances;
        public UInt64 vs_pClassInstances;
        public bool vs_isDirty;

        public UInt64 ps_addr;
        public UInt32 ps_NumClassInstances;
        public UInt64 ps_pClassInstances;
        public bool ps_isDirty;

        public UInt64 bs_addr;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] bs_factor;
        public UInt32 bs_mask;
        public bool bs_isDirty;

        public UInt64 ds_addr;
        public UInt32 ds_ref;
        public bool ds_isDirty;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public SSInfo[] ss;

        public UInt32 draw_IndexCount;
        public UInt32 draw_StartIndex;
        public UInt32 draw_BaseVertex;
        public bool draw_isDirty;
    };


}
