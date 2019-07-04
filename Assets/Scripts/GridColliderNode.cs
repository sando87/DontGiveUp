using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridColliderNode : MonoBehaviour
{
    static GridHashmap<GameObject> mSystem = new GridHashmap<GameObject>();
    int mKey = -1;
    LinkedListNode<GameObject> mNode;
    // Start is called before the first frame update
    void Start()
    {
        mSystem.Init(new Vector2(-1000, -300), new Vector2(2000, 600), 5);
        mNode = new LinkedListNode<GameObject>(gameObject);
        mKey = mSystem.CalcKey(transform.position.x, transform.position.z);
        mSystem.UpdateTable(mNode, -1, mKey);
    }

    // Update is called once per frame
    void Update()
    {
        int key = mSystem.CalcKey(transform.position.x, transform.position.z);
        if(key != mKey)
        {
            if(mSystem.UpdateTable(mNode, mKey, key) )
                mKey = key;
        }

        //GameObject[] objs = FindObjectsNear(3);
        //foreach (var item in objs)
        //    Debug.Log(item.name);
    }

    public GameObject[] FindObjectsNear(float round)
    {
        List<GameObject> rets = new List<GameObject>();
        Rect rect = new Rect();
        rect.xMin = transform.position.x - round;
        rect.xMax = transform.position.x + round;
        rect.yMin = transform.position.z - round;
        rect.yMax = transform.position.z + round;
        GameObject[] objs = mSystem.Find(rect);
        foreach(var item in objs)
        {
            if (item == gameObject)
                continue;
            if ((item.transform.position - transform.position).magnitude > round)
                continue;
            rets.Add(item);
        }
        return rets.ToArray();
    }

}
