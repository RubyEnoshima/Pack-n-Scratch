using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generador : MonoBehaviour
{
    public Arrastrador Clicker;
    public GameObject ObjGenerar;
    bool generat = false;

    public void Generar(){
        GameObject bloc = Instantiate(ObjGenerar);
        Clicker.selectedObject = bloc;
    }

    void Start()
    {
        Clicker = FindObjectOfType<Arrastrador>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!generat && Clicker.selectedObject == this.gameObject){
            Generar();
            generat = true;
        }else if(generat && !Clicker.selectedObject){
            generat = false;
        }
    }
}
