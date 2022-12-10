using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Final : MonoBehaviour
{
    public Text mitjana;
    // Start is called before the first frame update
    void Start()
    {
        mitjana.text = (Global.mitjana*100f).ToString()+"%";
    }

    public void Sortir(){
        Application.Quit();
    }
}
