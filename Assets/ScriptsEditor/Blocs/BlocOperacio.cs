using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BlocOperacio : Bloc
{
    public InputField InputA;
    public InputField InputB;
    public Dropdown DropOperacio;

    public float A = 0;
    public float B = 0;
    public string Operacio;
    public float Resultat = 0;

    // Excepcions
    public bool DividintZero = false;
    public bool MalFormatejat = false;
    
    public void CanviarA(){
        try{
            A = float.Parse(InputA.text);
            MalFormatejat = false;
        }catch(Exception e){
            MalFormatejat = true;
        }
    }

    public void CanviarB(){
        try{
            B = float.Parse(InputB.text);
            MalFormatejat = false;
        }catch(Exception e){
            MalFormatejat = true;
        }
    }

    public void CanviarValors(bool esA){
        if(esA){
            CanviarA();
        }else{
            CanviarB();
        }
    }
    
    public void CanviarOperacio(){
        Operacio = DropOperacio.options[DropOperacio.value].text;
    }

    protected override void Start(){
        base.Start();
        Funcio = "Operar";
        Operacio = "+";
    }

    public override void Executar()
    {
        base.Executar();

        if(MalFormatejat){
            Resultat = float.PositiveInfinity;
            return;
        }

        switch(Operacio){
            case "+":
                Resultat = A + B;
                break;
            
            case "-":
                Resultat = A - B;
                break;

            case "x":
                Resultat = A * B;
                break;

            case "/":
                if(B==0){
                    DividintZero = true;
                    Resultat = float.PositiveInfinity;
                }else{
                    Resultat = A / B;
                }
                break;
        }
        
        Debug.Log(Resultat);
    }
}
