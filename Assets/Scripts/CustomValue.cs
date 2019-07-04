using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomValue : MonoBehaviour
{
    object Value;
    public void SetValue(object value) { Value = value; }
    public object GetValue() { return Value; }
}
