using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomMesh : MonoBehaviour
{
    static Mesh mMesh = null;
    public string ResourceName = "";
    Loader Load = new Loader();
    Material Mat = null;
    public int animFactor = 0;

    void Start()
    {
        Mat = GetComponent<MeshRenderer>().material;

        if (mMesh == null && ResourceName.Length > 0)
        {
            mMesh = new Mesh();

            Load.LoadRenderInfo(ResourceName);
            mMesh.vertices = Load.GetPositions();
            mMesh.normals = Load.GetNormals();
            mMesh.triangles = Load.GetTriangles();
            mMesh.boneWeights = Load.GetWeights();

            MeshFilter mf = GetComponent<MeshFilter>();
            mf.mesh = mMesh;
        }
        else
        {
            MeshFilter mf = GetComponent<MeshFilter>();
            mf.mesh = mMesh;
        }

    }

    void Update()
    {
        Matrix4x4[] mats = Load.GetMatrixs(animFactor);
        if(mats != null)
            Mat.SetMatrixArray("_MatBones", mats);
    }
}
