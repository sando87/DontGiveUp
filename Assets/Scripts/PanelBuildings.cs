using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelBuildings : MonoBehaviour
{
    private MyBtnBuilding mSelectedButton = null;
    public MyBtnBuilding SelectedButton(MyBtnBuilding btn)
    {
        if (mSelectedButton == null)
        {
            mSelectedButton = btn;
            mSelectedButton.Check(true);
        }
        else if(mSelectedButton == btn)
        {
            mSelectedButton.Check(false);
            mSelectedButton = null;
        }
        else
        {
            mSelectedButton.Check(false);
            mSelectedButton = btn;
            mSelectedButton.Check(true);
        }
        return mSelectedButton;
    }
    public void OnCancle()
    {
        if(mSelectedButton != null)
        {
            mSelectedButton.Check(false);
            mSelectedButton = null;
        }
    }
}
