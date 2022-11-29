using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FloatVariable : Variable
{
    float contingut;
    public bool MalFormatejat = false;

    public override dynamic Get(){
        return contingut;
    }

    public override void Crear(string _nom, dynamic _contingut, int bloc){
        nom = _nom;
        BlocIni = bloc;
        Modificar(_contingut);
    }

    public override void Modificar(dynamic _contingut){
        if(_contingut is float || _contingut is int){
            contingut = (_contingut);
            
            inicialitzat = true;
        }else{
            try{
                contingut = float.Parse(_contingut);
                inicialitzat = true;

            }catch(Exception e){
                Debug.Log(e);
                MalFormatejat = true;
                contingut = float.PositiveInfinity;
                inicialitzat = false;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        tipus = (int)TIPUSVAR.NOMBRE;
        contingut = 0;
    }

    public override Variable Copiar()
    {
        FloatVariable copia = new FloatVariable();
        copia.Crear(nom,contingut,BlocIni);
        return copia;
    }
}
