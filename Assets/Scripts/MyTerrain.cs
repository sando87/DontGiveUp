using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MyTerrain : MonoBehaviour
{
    Material mat;
    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
        MeshFilter mf = GetComponent<MeshFilter>();
        string objName = mf.mesh.name.Split(' ')[0];
        string path = Application.dataPath + "/objects/" + objName + "/";
        string[] lines = File.ReadAllLines(path + objName + ".txt");
        List<Vector4> list = new List<Vector4>();
        int num = 1;
        while(true)
        {
            Vector4 vec = new Vector4();
            if (!ToVector(lines[num], ref vec))
                break;
            list.Add(vec);
            num++;
        }

        if (list.Count == 12)
            mat.SetVectorArray("texelVector", list.ToArray());

        int imgIdx = 0;
        while (num < lines.Length)
        {
            string imageName = path + lines[num];
            Texture2D tex = TGALoader.LoadTGA(imageName);
            mat.SetTexture("_Tex" + imgIdx, tex);
            num++;
            imgIdx++;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool ToVector(string line, ref Vector4 vec)
    {
        string[] col = line.Split(' ');
        if (col.Length != 3)
            return false;

        vec.x = float.Parse(col[0]);
        vec.y = float.Parse(col[1]);
        vec.z = float.Parse(col[2]);
        vec.w = 0.0f;
        return true;
    }
}
