using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pantalla : MonoBehaviour
{
    
    public Text text;

    public void CanviarText(string s){
        text.text = s;
    }
}
