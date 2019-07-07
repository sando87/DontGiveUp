using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    MyBtnBuilding mSelectedButton = null;

    //MyBtnBuilding로 관리되는 빌딩 생성 UI 버튼을 클릭시 콜백됨
    public void OnButtonClick(MyBtnBuilding btn)
    {
        mSelectedButton = btn;
    }
    //UI상 취소 버튼 클릭 시
    public void OnCancle()
    {
        mSelectedButton = null;
    }

    //마우스로 Plane지면을 클릭할때 콜백됨
    public void OnMouseClick(Vector3 clickPoint)
    {
        if (mSelectedButton == null)
            return;

        GameObject prefab = mSelectedButton.GetBuilding();
        GameObject obj = Instantiate(prefab, new Vector3(clickPoint.x, 0, clickPoint.z), Quaternion.identity);
        string name = mSelectedButton.gameObject.name;
        switch (name)
        {
            case Defines.BuildingTypeA:
                obj.GetComponentInChildren<MeshRenderer>().material.color = Color.red;
                break;
            case Defines.BuildingTypeB:
                obj.GetComponentInChildren<MeshRenderer>().material.color = Color.green;
                break;
            case Defines.BuildingTypeC:
                obj.GetComponentInChildren<MeshRenderer>().material.color = Color.blue;
                break;
            default:
                Debug.Log("Undefine Building");
                break;
        }
    }


}
