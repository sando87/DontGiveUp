using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridHashmap<T>
{
    private bool mIsInit = false;
    private int GridSize = 0;
    private Rect BaseGround;
    private int RowGridCount = 0;
    private int PitchGridCount = 0;
    Dictionary<int, LinkedList<T>> mTable = new Dictionary<int, LinkedList<T>>();

    public void Init(Vector2 basePos, Vector2 size, int gridSize)
    {
        if (mIsInit)
            return;

        mIsInit = true;
        GridSize = gridSize;
        BaseGround = new Rect(basePos, size);
        RowGridCount = (int)BaseGround.width / GridSize;
        PitchGridCount = (int)BaseGround.height / GridSize;
    }
    public bool UpdateTable(LinkedListNode<T> node, int prekey, int newkey)
    {
        if (mTable.ContainsKey(prekey))
            mTable[prekey].Remove(node);

        if (!mTable.ContainsKey(newkey))
            mTable[newkey] = new LinkedList<T>();

        mTable[newkey].AddLast(node);
        return true;
    }
    public T[] Find(Rect worldRect)
    {
        List<T> rets = new List<T>();

        int minX = CalcIndexX(worldRect.xMin);
        int maxX = CalcIndexX(worldRect.xMax);
        int minY = CalcIndexY(worldRect.yMin);
        int maxY = CalcIndexY(worldRect.yMax);
        for(int y = minY; y <= maxY; ++y)
        {
            for (int x = minX; x <= maxX; ++x)
            {
                int key = x + (RowGridCount * y);
                if (!mTable.ContainsKey(key))
                    continue;

                T[] objs = Find(key);
                rets.AddRange(objs);
            }
        }
        return rets.ToArray();
    }
    public T[] Find(int key)
    {
        if (!mTable.ContainsKey(key))
            return null;

        List<T> rets = new List<T>();
        LinkedList<T> list = mTable[key];
        foreach (var obj in list)
            rets.Add(obj);

        return rets.ToArray();
    }

    public int CalcKey(float worldPosX, float worldPosY)
    {
        //Plane의 (0,0)을 기준으로 gridsize로 나눈 2차배열 index를 key값으로 환산한다.
        int idxX = CalcIndexX(worldPosX);
        int idxY = CalcIndexY(worldPosY);
        return idxX + (RowGridCount * idxY);
    }
    public int CalcIndexX(float x)
    {
        int idx = (int)(x - BaseGround.xMin) / GridSize;
        return idx < 0 ? 0 : (idx >= RowGridCount? RowGridCount - 1 : idx);
    }
    public int CalcIndexY(float y)
    {
        int idx = (int)(y - BaseGround.yMin) / GridSize;
        return idx < 0 ? 0 : (idx >= PitchGridCount ? PitchGridCount - 1 : idx);
    }
}
