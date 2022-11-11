using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BlocModVariable : Bloc
{
    public Dropdown Tipus;
    public Dropdown VariablesInput;
    public InputField NomVariable;
    public InputField ContingutVariable;

    protected override void Start(){
        base.Start();
        Funcio = "Variable";
        
    }

    public override void ActualitzarVariables(){
        VariablesInput.AddOptions(Variables.options);
    }

    public void ModeVariable(){
        bool active = true;
        if(Variables.value!=0){
            active = false;
        }
        NomVariable.gameObject.SetActive(active);
    }

    public override void Executar()
    {
        base.Executar();
    }
}
