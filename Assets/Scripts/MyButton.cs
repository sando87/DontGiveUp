using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MyButton : MonoBehaviour
{
    [Serializable]
    public class UnityEventButton : UnityEvent<MyButton> { }
    public UnityEventButton mButtonClick = null;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        mButtonClick.Invoke(this);
    }
}
