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
            Debug.Log("S'ha tractat de posar aixo: "+contingut);
            try{
                contingut = float.Parse(_contingut);

            }catch(Exception e){
                
                MalFormatejat = true;
                contingut = float.PositiveInfinity;
            }
            inicialitzat = true;
        }else{
            contingut = float.PositiveInfinity;
            inicialitzat = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        tipus = (int)TIPUSVAR.NOMBRE;
        contingut = 0;
    }
}
