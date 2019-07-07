using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using LSJ;

public class Unit : MonoBehaviour
{
    public FSM FSM { get; set; }
    public Animator Anim { get; set; }
    public GridColliderNode GridNode { get; set; }

    public int HP = 0;
    public int PointAttack = 0;
    public TypePeer Peer;
    public Unit Target = null;

    private void Awake()
    {
        Anim = GetComponent<Animator>();
        GridNode = GetComponent<GridColliderNode>();
        FSM = new FSM();
    }
    private void Update()
    {

    }

    public Unit DetectEnemy(float range)
    {
        GameObject[] objs = GridNode.FindObjectsNear(range);
        foreach (GameObject obj in objs)
        {
            Unit unit = obj.GetComponent<Unit>();
            if (unit == null)
                continue;

            if (unit.Peer != Peer)
                return unit;
        }
        return null;
    }
    public void Damaged(int point)
    {
        HP -= point;
        if (HP < 0) HP = 0;
    }
    public void Attack(Unit unit)
    {
        unit.Damaged(PointAttack);
    }
    public void OnEventAttack()
    {
        if (Target != null)
            Attack(Target);
    }
}
