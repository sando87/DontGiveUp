using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/*
 * MyBtnBuilding 설명
 * - 다수의 버튼이 하나의 그룹으로 묶여있어 마치 라디오 버튼처럼 동작함
 * - 버튼 클릭 이벤트로는 현재 최종 선택된 버튼이 전달됨
 * - 해당 클래스에는 해당 버튼에서 생성되어야 하는 빌딩 prefab 정보도 같이 관리됨
 */

public class MyBtnBuilding : MonoBehaviour
{
    [Serializable]
    public class UnityEventButton : UnityEvent<MyBtnBuilding> { }
    public UnityEventButton mButtonClick = null;
    public GameObject mBuilding;

    private bool mIsChecked = false;
    private Image mImage;
    private PanelBuildings mGroupUI;


    private void Start()
    {
        mGroupUI = GetComponentInParent<PanelBuildings>();
        mImage = GetComponent<Image>();
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        MyBtnBuilding selectedBtn = mGroupUI.SelectedButton(this);
        mButtonClick.Invoke(selectedBtn);
    }

    public void Check(bool isCheck)
    {
        mIsChecked = isCheck;
        mImage.color = mIsChecked ? Color.green : Color.white;
    }

    public GameObject GetBuilding()
    {
        return mBuilding;
    }
}
