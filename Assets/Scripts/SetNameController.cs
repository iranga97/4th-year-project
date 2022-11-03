using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetNameController : MonoBehaviour
{
    public TextMeshProUGUI organName;
    public TMP_InputField inputField;

    public void setName(){
        Debug.Log("Text: " + inputField.text);
    } 
}
