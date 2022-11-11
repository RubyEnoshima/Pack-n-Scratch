using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BlocVariable : Bloc
{
    public Dropdown Tipus;
    public InputField NomVariable;
    public InputField ContingutVariable;
    Variable v;
    int nVariable = -1;

    protected override void Start(){
        base.Start();
        Funcio = "Variable";
        v = new FloatVariable();
        v.Crear("",0,nBloc);
        nVariable = Editor.AfegirVariable(v);
    }

    public void CanviarContingut(){
        v.Modificar(ContingutVariable.text);
        Editor.ModificarVariable(nVariable,v);
    }

    public void CanviarNom(){
        v.nom = NomVariable.text;
        Editor.ModificarVariable(nVariable,v);
    }

    public void CanviarTipus(){
        if(Tipus.value==0){
            v = new FloatVariable();
            CanviarContingut();
            FerVisible(0);
        }else if(Tipus.value==1){
            v = new StringVariable();
            CanviarContingut();
            FerVisible(0);
        }else if(Tipus.value==2){
            FerVisible(1);
            // CanviarContingutBloc();
        }else{
            FerVisible(2);
        }

        CanviarNom();
        
        
    }

    public void CanviarContingutBloc(){
        if(Blocs.options.Count>0){
            int i = int.Parse(Blocs.options[Blocs.value].text.Remove(0,1));
            dynamic res = Editor.ResultatBloc(i-1);
            if(res is float){
                v = new FloatVariable();
                Debug.Log("resultat: "+res);
            }else if(res is string){
                v = new StringVariable();
            }else if(res is bool){
                v = new BoolVariable();
            }
            CanviarNom();
            v.Modificar(res);
            Editor.ModificarVariable(nVariable,v);
        }
    }

    void FerVisible(int n){
        if(n==0){
            ContingutVariable.gameObject.SetActive(true);
        }else if(n==1){
            ContingutVariable.gameObject.SetActive(false);
            Blocs.gameObject.SetActive(true);
        }else{
            ContingutVariable.gameObject.SetActive(false);
            Blocs.gameObject.SetActive(false);
        }
    }

    public override void Executar()
    {
        base.Executar();
        if(Tipus.value==2){
            CanviarContingutBloc();
        }
    }

    protected override void OnDestroy() {
        Editor.EsborrarVariable(nVariable);
    }
}
