using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private GameObject ActivatedBuidling;

    public Defines.UserInfo GetUserInfo()
    {
        Defines.UserInfo userInfo = new Defines.UserInfo();
        userInfo.IsTryToCreateNewBuilding = (ActivatedBuidling == null) ? false : true;
        if (ActivatedBuidling != null)
            userInfo.buildingName = ActivatedBuidling.name;
        return userInfo;
    }
    public void OnClickSelectBuilding(GameObject building)
    {
        if (ActivatedBuidling == null)
        {
            ActivatedBuidling = building;
            ActivateButton(ActivatedBuidling, true);
            return;
        }

        if (ActivatedBuidling == building)
        {
            ActivateButton(ActivatedBuidling, false);
            ActivatedBuidling = null;
            return;
        }

        if(ActivatedBuidling != null)
        {
            ActivateButton(ActivatedBuidling, false);
            ActivatedBuidling = building;
            ActivateButton(ActivatedBuidling, true);
            return;
        }
    }
    public void OnClickCancel()
    {
        if (ActivatedBuidling != null)
        {
            ActivateButton(ActivatedBuidling, false);
            ActivatedBuidling = null;
            return;
        }
    }
    private void ActivateButton(GameObject button, bool IsActivate)
    {
        button.GetComponent<CustomValue>().SetValue(IsActivate);
        button.GetComponent<Image>().color = IsActivate ? Color.green : Color.white;
    }
}

