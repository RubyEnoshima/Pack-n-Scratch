using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BlocPrint : Bloc
{
    public InputField input;
    public Pantalla Pantalla;
    public string TextMostrat;

    public Dropdown TipusPrint;

    int TextVarBloc = 0;
    
    protected override void Start(){
        base.Start();
        Funcio = "Print";
        input = GetComponentInChildren<InputField>();
        Pantalla = FindObjectOfType<Pantalla>();
        
    }

    bool BlocValid(Bloc bloc){
        return bloc.nBloc != nBloc;
    }
    
    public override void Executar()
    {
        base.Executar();
        if(TextVarBloc==0) 
            TextMostrat = input.text;
        else if(TextVarBloc==2){
            if(Editor.Variables.Count != 0){
                Debug.Log(Editor.Variables[Variables.value].nom);
                TextMostrat = Editor.Variables[Variables.value].Get().ToString();  
            }
            else TextMostrat = "";
        }
        else{
            int i = int.Parse(Blocs.options[Blocs.value].text.Remove(0,1));
            TextMostrat = Editor.ResultatBloc(i-1).ToString();
        }
        Debug.Log("Mostrant "+TextMostrat);
        Pantalla.CanviarText(TextMostrat);
    }

    public void CanviarTipus(){
        if(TipusPrint.value == 0){
            input.gameObject.SetActive(true);
            Variables.gameObject.SetActive(false);
            Blocs.gameObject.SetActive(false);

        }else if(TipusPrint.value == 1){
            input.gameObject.SetActive(false);
            Variables.gameObject.SetActive(false);
            Blocs.gameObject.SetActive(true);

            ActualitzarBloc();
            

        }else{
            input.gameObject.SetActive(false);
            Variables.gameObject.SetActive(true);
            Blocs.gameObject.SetActive(false);

            ActualitzarVariables();
        }
        TextVarBloc = TipusPrint.value;
    }

    public override dynamic ResultatBloc(){
        return TextMostrat;
    }
}
