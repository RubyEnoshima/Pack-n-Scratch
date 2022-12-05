using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BlocIf : Bloc
{
    public Dropdown Tipus;

    int TextVarBloc = 0;

    bool EsBoolea = true;
    bool resultat;
    
    protected override void Start(){
        base.Start();
        Funcio = "If";
        TextVarBloc = Tipus.value;
    }

    public override bool TeErrors()
    {
        return !EsBoolea;
    }

    public override string ObtenirError()
    {
        return "Sembla que no he ficat una condicio...";
    }

    bool BlocValid(Bloc bloc){
        return bloc.nBloc != nBloc;
    }
    
    public override void Executar()
    {
        base.Executar();
        if(TextVarBloc==0) {
            int i = int.Parse(Blocs.options[Blocs.value].text.Remove(0,1));
            if(Editor.ResultatBloc(i-1) is bool){
                resultat = Editor.ResultatBloc(i-1);
            }else{
                EsBoolea = false;
            }

        }
        else{
            if(Editor.Variables[Variables.value] is BoolVariable){
                resultat = Editor.Variables[Variables.value].Get();
            }else{
                EsBoolea = false;
            }
        }

        if(resultat && EsBoolea){

        }
    }

    public void CanviarTipus(){
        if(Tipus.value == 0){
            Blocs.gameObject.SetActive(true);

            ActualitzarBloc();
        }else{
            Blocs.gameObject.SetActive(false);

            ActualitzarVariables();

        }
        TextVarBloc = Tipus.value;
    }

    public override dynamic ResultatBloc(){
        return resultat;
    }
}
