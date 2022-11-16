using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BlocBoolea : Bloc
{

    public Dropdown Tipus;
    public InputField Contingut;
    public Dropdown TipusDreta;
    public InputField ContingutDreta;
    public Dropdown VariablesDreta;
    public Dropdown BlocsDreta;
    public int BlocsDretaValor;
    public int VariablesDretaValor;
    public Dropdown Comparador;

    bool resultat;

    protected override void Start(){
        base.Start();
        Funcio = "Boolea";
        BlocsDreta.onValueChanged.AddListener(delegate {CanviarBlocsNum();});
        VariablesDreta.onValueChanged.AddListener(delegate {CanviarVarNum();});
    }

    void CanviarBlocsNum(){
        BlocsDretaValor = BlocsDreta.value;
    }
    void CanviarVarNum(){
        VariablesDretaValor = VariablesDreta.value;
    }

    public override void ActualitzarBloc(){
        base.ActualitzarBloc();
        BlocsDreta.ClearOptions();
        BlocsDreta.AddOptions(Blocs.options);
        BlocsDreta.value = BlocsDretaValor;
    }

    public override void ActualitzarVariables(){
        base.ActualitzarVariables();
        VariablesDreta.ClearOptions();
        VariablesDreta.AddOptions(Variables.options);
        VariablesDreta.value = VariablesDretaValor;
    }

    void FerVisible(int n, bool dreta){
        if(!dreta){
            if(n==0){
                Contingut.gameObject.SetActive(true);
            }else if(n==1){
                Contingut.gameObject.SetActive(false);
                Blocs.gameObject.SetActive(true);
            }else{
                Contingut.gameObject.SetActive(false);
                Blocs.gameObject.SetActive(false);
            }

        }else{
            if(n==0){
                ContingutDreta.gameObject.SetActive(true);
            }else if(n==1){
                ContingutDreta.gameObject.SetActive(false);
                BlocsDreta.gameObject.SetActive(true);
            }else{
                ContingutDreta.gameObject.SetActive(false);
                BlocsDreta.gameObject.SetActive(false);
            }
        }
    }

    public void CanviarTipus(bool dreta)
    {
        if(!dreta){
            if(Tipus.value==0){
                FerVisible(0,false);
            }else if(Tipus.value==1){
                FerVisible(0,false);
            }else if(Tipus.value==2){
                FerVisible(1,false);
            }else{
                ActualitzarVariables();
                FerVisible(2,false);
            }

        }else{
            if(TipusDreta.value==0){
                FerVisible(0,true);
            }else if(TipusDreta.value==1){
                FerVisible(0,true);
            }else if(TipusDreta.value==2){
                FerVisible(1,true);
            }else{
                ActualitzarVariables();
                FerVisible(2,true);
            }
        }
    }

    public dynamic ObtEsquerra(){
        if(Tipus.value==0){
            float res;
            try{
                res = float.Parse(Contingut.text);
            }catch(Exception e){
                res = float.PositiveInfinity;
            }
            return res;
        }else if(Tipus.value==1){
            return Contingut.text;
        }else if(Tipus.value==2){
            if(Blocs.options.Count>0){
                int i = int.Parse(Blocs.options[Blocs.value].text.Remove(0,1));
                return Editor.ResultatBloc(i-1);
            }else{
                return null;
            }
        }else{
            return Editor.Variables[Variables.value].Get();
        }
    }

    public dynamic ObtDreta(){
        if(TipusDreta.value==0){
            float res;
            try{
                res = float.Parse(ContingutDreta.text);
            }catch(Exception e){
                res = float.PositiveInfinity;
            }
            return res;
        }else if(TipusDreta.value==1){
            return ContingutDreta.text;
        }else if(TipusDreta.value==2){
            if(BlocsDreta.options.Count>0){
                int i = int.Parse(BlocsDreta.options[BlocsDreta.value].text.Remove(0,1));
                return Editor.ResultatBloc(i-1);
            }else{
                return null;
            }
        }else{
            return Editor.Variables[VariablesDreta.value].Get();
        }
    }

    public override void Executar()
    {
        base.Executar();
        dynamic esq = ObtEsquerra(), dre = ObtDreta();

        if(esq is string && dre is float){
            dre = dre.ToString();
        }else if(esq is float && dre is string){
            esq = esq.ToString();
        }
        
        if(esq is string && dre is string){
            int res = string.Compare(esq,dre);
            if(Comparador.value==0){
                resultat = res == 0;
            }else if(Comparador.value==1){
                resultat = res != 0;
            }else if(Comparador.value==2){
                resultat = res >= 0;
            }else if(Comparador.value==3){
                resultat = res <= 0;
            }else if(Comparador.value==4){
                resultat = res > 0;
            }else{
                resultat = res < 0;
            }
            return;
        }

        if(Comparador.value==0){
            resultat = esq == dre;
        }else if(Comparador.value==1){
            resultat = esq != dre;
        }else if(Comparador.value==2){
            resultat = esq >= dre;
        }else if(Comparador.value==3){
            resultat = esq <= dre;
        }else if(Comparador.value==4){
            resultat = esq > dre;
        }else{
            resultat = esq < dre;
        }
    }

    public override dynamic ResultatBloc(){
        return resultat;
    }
}
