using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BlocVariable : Bloc
{
    public InputField input;
    public Pantalla Pantalla;
    
    protected override void Start(){
        base.Start();
        Funcio = "Print";
        input = GetComponentInChildren<InputField>();
        Pantalla = FindObjectOfType<Pantalla>();
    }

    public override void Executar()
    {
        base.Executar();
        Debug.Log(input.text);
        Pantalla.CanviarText(input.text);
    }
}
