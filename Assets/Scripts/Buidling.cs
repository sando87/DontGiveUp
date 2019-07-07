using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSJ;

public class Buidling : MonoBehaviour
{
    private float mRegenSec = 10;
    private float mAccTime = 0;
    public GameObject mUnit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CreateUnit();
    }

    bool CreateUnit()
    {
        mAccTime += Time.deltaTime;
        if (mAccTime > mRegenSec)
        {
            mAccTime = 0;
            var obj = Instantiate(mUnit, transform.position + new Vector3(-1, 0, -1), Quaternion.identity);
            obj.tag = "Red";
            return true;
        }
        return false;
    }
}
