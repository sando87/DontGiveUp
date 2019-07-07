using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ClickEventSystem : MonoBehaviour
{
    [Serializable]
    public class UnityEventClick : UnityEvent<Vector3> { }
    public UnityEventClick mMouseClick = null;

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if(!EventSystem.current.IsPointerOverGameObject()) //UI 클릭시 RayCast는 무시
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo = new RaycastHit();
                if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
                {
                    int cnt = mMouseClick.GetPersistentEventCount();
                    for(int i = 0; i<cnt; ++i) //여러개의 콜백함수중 hit된 오브젝트와 동일할 경우만 Invoke호출
                    {
                        Component com = (Component)mMouseClick.GetPersistentTarget(i);
                        if (com.gameObject == hitInfo.transform.gameObject)
                            mMouseClick.Invoke(hitInfo.point);
                    }
                }
            }
        }
    }


}
