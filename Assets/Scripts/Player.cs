using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject UIManager;
    public GameObject NewBuilding;

    public class MyLayerMask
    {
        public const int Ground = 1 << 8;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(TryToCreateNewBuidling())
        {
            CreateNewBuildingOnClickPoint();
            /*
             * if( not enough money )
             *      Mode back();
             */
        }
    }

    bool TryToCreateNewBuidling()
    {
        Defines.UserInfo info = UIManager.GetComponent<UIManager>().GetUserInfo();
        return info.IsTryToCreateNewBuilding;
    }

    bool CreateNewBuildingOnClickPoint()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, MyLayerMask.Ground))
            {
                GameObject obj = Instantiate(NewBuilding, new Vector3(hitInfo.point.x, 0, hitInfo.point.z), Quaternion.identity);
                Defines.UserInfo info = UIManager.GetComponent<UIManager>().GetUserInfo();
                switch(info.buildingName)
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
        return true;
    }
}
