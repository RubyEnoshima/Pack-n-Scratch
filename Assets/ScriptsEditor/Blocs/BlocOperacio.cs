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
    public Dropdown VariablesB;
    public Dropdown TipusA;
    public Dropdown TipusB;

    public float A = 0;
    public float B = 0;
    public string Operacio;
    public float Resultat = 0;

    // Excepcions
    public bool DividintZero = false;
    public bool MalFormatejat = false;

    public int VariableValorB;
    bool tipusA = false;
    bool tipusB = false;

    public override void ActualitzarVariables()
    {
        base.ActualitzarVariables();
        VariablesB.ClearOptions();
        VariablesB.AddOptions(Variables.options);
        VariablesB.value = VariableValorB;
    }

    public override bool TeErrors()
    {
        return DividintZero || MalFormatejat;
    }

    public override string ObtenirError()
    {
        if(DividintZero) return "Dividir per zero dona infinit...";
        return "Potser el nombre esta mal escrit...";
    }

    public override void Iniciar(){
        base.Iniciar();
        VariablesB.value = VariableValorB;
        CanviarOperacio();
        CanviarTipusA();
        CanviarA();
        CanviarTipusB();
        CanviarB();
    }
    
    public void CanviarA(){
        try{
            if(!tipusA){
                A = float.Parse(InputA.text);

            }else{
                A = float.Parse(Editor.Variables[Variables.value].Get().ToString());
            }
            MalFormatejat = false;
        }catch(Exception e){
            MalFormatejat = true;
        }
    }

    public void CanviarB(){
        try{
            if(!tipusB){
                B = float.Parse(InputB.text);

            }else{
                B = float.Parse(Editor.Variables[VariablesB.value].Get().ToString());
            }
            MalFormatejat = false;
        }catch(Exception e){
            MalFormatejat = true;
        }
    }

    public void CanviarTipusA(){
        if(TipusA.value==0) {
            tipusA = false;
            InputA.gameObject.SetActive(true);

        }else{
            InputA.gameObject.SetActive(false);
            tipusA = true;
            ActualitzarVariables();
        }
    }

    public void CanviarTipusB(){
        if(TipusB.value==0) {
            tipusB = false;
            InputB.gameObject.SetActive(true);
        }else{
            InputB.gameObject.SetActive(false);
            tipusB = true;
            ActualitzarVariables();
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
        VariablesB.onValueChanged.AddListener(delegate {CanviarVarNum();});
    }

    void CanviarVarNum(){
        VariableValorB = VariablesB.value;
    }

    public override void Executar()
    {
        base.Executar();

        if(MalFormatejat){
            Resultat = float.PositiveInfinity;
            return;
        }

        if(tipusA) CanviarA();
        if(tipusB) CanviarB();

        CanviarOperacio();

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

            case "%":
                Resultat = A % B;
                break;
        }
        
//        Debug.Log(Resultat);
    }

    public override dynamic ResultatBloc(){
        return Resultat;
    }
}
