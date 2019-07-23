using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MyTerrainObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Material mat = GetComponent<MeshRenderer>().material;
        MeshFilter mf = GetComponent<MeshFilter>();
        string objName = mf.mesh.name.Split(' ')[0];
        string path = Application.dataPath + "/objects/" + objName + "/";
        string[] lines = File.ReadAllLines(path + objName + ".txt");
        List<Vector4> list = new List<Vector4>();
        int num = 1;
        while (true)
        {
            if (lines[num].Split(' ').Length == 1)
                break;
            num++;
        }

        string imageName = path + lines[num];
        Texture2D tex = TGALoader.LoadTGA(imageName);
        mat.mainTexture = tex;

    }
}
