using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSJ;

public class MovableUnit : MonoBehaviour
{
    private Unit mBaseUnit = null;

    private Vector3 mPosLastTarget = new Vector3(-1000, 0, 0);
    public float mMoveSpeed = 3.0f;
    public float mRangeDetect = 10.0f;
    public float mRangeAttack = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        mBaseUnit = GetComponent<Unit>();

        FSM fsm = mBaseUnit.FSM;
        fsm.FuncSwitch = FSM_Switch;
        fsm.FuncStay = FSM_Stay;
        fsm.FuncEnter = FSM_Enter;
        fsm.FuncLeave = FSM_Leave;
        fsm.Init(AIState.PATROL);
    }

    // Update is called once per frame
    void Update()
    {
        mBaseUnit.FSM.Update();
        //if (Input.GetKeyDown(KeyCode.D))
        //{
        //    anim.SetTrigger("Attack");
        //    Debug.Log(gameObject.name);
        //}
        //else if (Input.GetKey(KeyCode.A))
        //    anim.SetBool("Walk", true);
        //else
        //    anim.SetBool("Walk", false);
    }

    private AIState FSM_Switch(AIState state)
    {
        AIState ret = state;
        float dist = 0;
        Unit unit = null;
        switch (state)
        {
            case AIState.PATROL:
                unit = mBaseUnit.DetectEnemy(mRangeDetect);
                if (unit != null)
                {
                    mBaseUnit.Target = unit;
                    dist = (unit.transform.position - transform.position).magnitude;
                    return (dist < mRangeAttack) ? AIState.ATTACK : AIState.TRACE;
                }
                break;
            case AIState.TRACE:
                dist = (mBaseUnit.Target.transform.position - transform.position).magnitude;
                if (dist < mRangeAttack)        return AIState.ATTACK;
                if (dist > mRangeDetect * 2)    return AIState.PATROL;
                break;
            case AIState.ATTACK:
                if (mBaseUnit.Target == null || mBaseUnit.FSM.State == AIState.DEATH || mBaseUnit.FSM.State == AIState.DYING)
                {
                    mBaseUnit.Target = null;
                    return AIState.PATROL;
                }
                else
                {
                    dist = (mBaseUnit.Target.transform.position - transform.position).magnitude;
                    if (dist > mRangeAttack)
                        return AIState.TRACE;
                }
                break;
            case AIState.DEATH:
                break;
            default:
                break;
        }
        return ret;
    }
    private AIState FSM_Stay(AIState state)
    {
        Animator anim = mBaseUnit.Anim;
        switch (state)
        {
            case AIState.PATROL:
                {
                    Vector3 dir = mPosLastTarget - transform.position;
                    dir.Normalize();
                    transform.Translate(dir * mMoveSpeed * Time.deltaTime);
                    anim.SetBool("Walk", true);
                }
                break;
            case AIState.TRACE:
                {
                    Vector3 dir = mBaseUnit.Target.transform.position - transform.position;
                    dir.Normalize();
                    transform.Translate(dir * mMoveSpeed * Time.deltaTime);
                    anim.SetBool("Walk", true);
                }
                break;
            case AIState.ATTACK:
                {
                    anim.SetTrigger("Attack");
                }
                break;
            case AIState.DEATH: break;
            default: break;
        }
        return state;
    }
    private AIState FSM_Enter(AIState state)
    {
        switch (state)
        {
            case AIState.PATROL: break;
            case AIState.TRACE: break;
            case AIState.ATTACK: break;
            case AIState.DEATH: break;
            default: break;
        }
        return state;
    }
    private AIState FSM_Leave(AIState state)
    {
        switch (state)
        {
            case AIState.PATROL: break;
            case AIState.TRACE: break;
            case AIState.ATTACK: break;
            case AIState.DEATH: break;
            default: break;
        }
        return state;
    }


    private void OnTriggerStay(Collider other)
    {
        //Vector3 dir = other.transform.position - transform.position;
        //dir.y = 0;
        //dir.Normalize();
        //Vector3 pos = transform.position;
        //pos.y = other.transform.position.y;
        //Ray ray = new Ray(pos, dir);
        //float pushPower = 0;
        //RaycastHit hit = new RaycastHit();
        //if (other.Raycast(ray, out hit, Mathf.Infinity))
        //    pushPower = hit.distance;
        //
        //pushPower = pushPower < 0.1 ? 0.1f : pushPower;
        //pushPower = (1 / (pushPower * 5)) + 1.0f; //0.1f~ 1.0f 스케일을 3.0f~1.25f스케일로 변환
        //Vector3 Force = -dir * pushPower;
        //transform.Translate(Force * mMoveSpeed * Time.deltaTime);
    }


}
