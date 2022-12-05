using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pensament : MonoBehaviour
{
    public Text text;
    public bool apareix = false;
    public GameObject Bombolla;
    public GameObject Boto;

    public void CanviarText(string s){
        text.text = s;
    }

    public void SwitchPensament(){
        apareix = !apareix;
        Bombolla.SetActive(apareix);
        Boto.SetActive(!apareix);
    }

    public void FerApareixer(){
        apareix = true;
        Bombolla.SetActive(apareix);
        Boto.SetActive(!apareix);
    }
}
