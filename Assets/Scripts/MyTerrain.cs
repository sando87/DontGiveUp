using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTerrain : MonoBehaviour
{
    Material mat;
    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
        Vector4[] vectors = new Vector4[12]{
            new Vector4(10.0f, 0.0f, 0.0f, 0.0f),
            new Vector4(0.0f, 10.0f, 0.0f, 0.0f),
            new Vector4(5.0f, 0.0f, 0.0f, 0.0f),
            new Vector4(0.0f, 5.0f, 0.0f, 0.0f),
            new Vector4(5.0f, 0.0f, 0.0f, 0.0f),
            new Vector4(0.0f, 5.0f, 0.0f, 0.0f),
            new Vector4(10.0f, 0.0f, 0.0f, 0.0f),
            new Vector4(0.0f, 10.0f, 0.0f, 0.0f),
            new Vector4(10.0f, 0.0f, 0.0f, 0.0f),
            new Vector4(0.0f, 10.0f, 0.0f, 0.0f),
            new Vector4(1.0f, 0.0f, 0.0f, 0.0f),
            new Vector4(0.0f, 1.0f, 0.0f, 0.0f),
        };
        mat.SetVectorArray("texelVector", vectors);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
