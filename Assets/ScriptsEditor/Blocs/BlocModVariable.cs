using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BlocModVariable : BlocVariable
{
    public Dropdown VariablesInput;
    int VariableValorInput;
    public bool DiferentTipus = false;
    public bool UtilitzantInput = false;

    protected override void Start(){
        Clicker = FindObjectOfType<Arrastrador>();
        Collider = GetComponent<Collider2D>();
        EditorCol = GameObject.Find("Editor").GetComponent<Collider2D>();
        Editor = GameObject.Find("Editor").GetComponent<Editor>();
        Variables.onValueChanged.AddListener(delegate {CanviarVariableNum();});
        Funcio = "Variable";
        VariablesInput.onValueChanged.AddListener(delegate {CanviarVariableInputNum();});
    }

    public override bool TeErrors()
    {
        return DiferentTipus || UtilitzantInput;
    }

    public override string ObtenirError()
    {
        if(DiferentTipus) return "No crec que pugui modificar el tipus d'una variable...";
        return "No crec que pugui modificar l'Input...";
    }

    public void CanviarVariable(){
        if(Editor.Variables.Count > 0){
            v = Editor.Variables[Variables.value];
            nVariable = v.BlocIni;
        }
    }

    public override void CanviarContingutVariable(){
        string nom = v.nom;
        v = Editor.Variables[VariablesInput.value].Copiar();
        v.BlocIni = nVariable;
        v.nom = nom;
        Editor.ModificarVariable(Variables.value,v);
    } 

    void CanviarVariableInputNum(){
        VariableValorInput = VariablesInput.value;
    }

    public override void ActualitzarVariables()
    {
        base.ActualitzarVariables();
        VariablesInput.ClearOptions();
        VariablesInput.AddOptions(Variables.options);
        VariablesInput.value = VariableValorInput;
    }

    public override void CanviarTipus()
    {
        
        if(Tipus.value==0){
            FerVisible(0);
        }else if(Tipus.value==1){
            FerVisible(0);
        }else if(Tipus.value==2){
            FerVisible(1);
        }else{
            ActualitzarVariables();
            FerVisible(2);
        }
    }

    public override void Executar()
    {
        if(Variables.value==0) UtilitzantInput=true;
        else{
            base.Executar();
            if(Tipus.value==0 && v is FloatVariable){
                CanviarContingut();
            }else if(Tipus.value==1 && v is StringVariable){
                CanviarContingut();
            }else if(Tipus.value!=2 && Tipus.value!=3){
                DiferentTipus = true;
            }

        }


    }
}
