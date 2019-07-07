using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSJ;

public class FSM  //finite-state machine
{
    public delegate AIState FSMFunc(AIState state);
    public FSMFunc FuncSwitch { get; set; }
    public FSMFunc FuncEnter { get; set; }
    public FSMFunc FuncLeave { get; set; }
    public FSMFunc FuncStay { get; set; }

    private AIState mState;
    public AIState State { get { return mState; } }
    public void Init(AIState state)
    {
        mState = state;
        FuncEnter(mState);
    }

    public void Update()
    {
        FuncStay(mState);
        AIState toState = FuncSwitch(mState);
        if(toState != mState)
        {
            FuncLeave(mState);
            mState = toState;
            FuncEnter(mState);
        }
    }
}
