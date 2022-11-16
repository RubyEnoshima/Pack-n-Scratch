using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BlocModVariable : BlocVariable
{
    public Dropdown VariablesInput;
    int VariableValorInput;
    public bool DiferentTipus = false;

    protected override void Start(){
        Clicker = FindObjectOfType<Arrastrador>();
        Collider = GetComponent<Collider2D>();
        EditorCol = GameObject.Find("Editor").GetComponent<Collider2D>();
        Editor = GameObject.Find("Editor").GetComponent<Editor>();
        Variables.onValueChanged.AddListener(delegate {CanviarVariableNum();});
        Funcio = "Variable";
        VariablesInput.onValueChanged.AddListener(delegate {CanviarVariableInputNum();});
    }

    public void CanviarVariable(){
        if(Editor.Variables.Count > 0)
            v = Editor.Variables[VariablesInput.value];
    }

    public override void CanviarContingutVariable(){
        v = Editor.Variables[VariablesInput.value].Copiar();
        v.BlocIni = nVariable;
        CanviarNom();
    } 

    void CanviarVariableInputNum(){
        VariableValorInput = VariablesInput.value;
    }

    public override void ActualitzarVariables()
    {
        base.ActualitzarVariables();
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
        base.Executar();
        if(Tipus.value==0 && v is FloatVariable){
            CanviarContingut();
        }else if(Tipus.value==1 && v is StringVariable){
            CanviarContingut();
        }else{
            DiferentTipus = true;
        }


    }
}
