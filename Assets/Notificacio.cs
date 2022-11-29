using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notificacio : MonoBehaviour
{
    public GameObject Popup;
    public Text Nom;
    public Text Assumpte;
    public Text Descripcio;
    public Text nNotis;
    bool obert = false;
    
    public void SwitchPopup(){
        obert = !obert;
        Popup.SetActive(obert);
    }
    
    public void CarregarScript(Script s){
        Nom.text = s.Client;
        Assumpte.text = s.Assumpte;
        Descripcio.text = s.Descripcio;
    }

    public void CanviarNotis(int n){
        nNotis.text = n.ToString();
    }
}
