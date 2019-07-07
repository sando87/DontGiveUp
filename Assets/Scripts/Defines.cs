using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


namespace LSJ
{
    public enum AIState
    {
        CREATE, //생성중(생성 모션)
        IDLE,   //서있는 상태(주변 적 감지)
        HOLD,   //서있는 상태(주변 적 무시)
        PATROL, //이동 중인 상태(주변 적 감지)
        MOVE,   //이동 중인 상태(주변 적 무시)
        TRACE,  //타게팅된 적을 따라가는 중
        ATTACK, //공격중
        DYING,  //죽는중(죽는 모션)
        DEATH,   //죽은상태(화면에 안보임)
    }
    public enum TypePeer
    {
        Neutral = 0x1,
        Red     = 0x2,
        Blue    = 0x4,
    }
}

public class Defines : MonoBehaviour
{
    public const string BuildingTypeA = "TypeA";
    public const string BuildingTypeB = "TypeB";
    public const string BuildingTypeC = "TypeC";
}
