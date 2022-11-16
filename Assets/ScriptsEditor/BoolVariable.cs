using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoolVariable : Variable
{
    bool contingut;

    public override dynamic Get(){
        return contingut;
    }

    public override void Crear(string _nom, dynamic _contingut, int bloc){
        nom = _nom;
        BlocIni = bloc;
        Modificar(_contingut);
    }

    public override void Modificar(dynamic _contingut){
        if(_contingut is bool){
            contingut = _contingut;
            inicialitzat = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        tipus = (int)TIPUSVAR.BOOL;
        contingut = false;
    }

    public override Variable Copiar()
    {
        BoolVariable copia = new BoolVariable();
        copia.Crear(nom,contingut,BlocIni);
        return copia;
    }
}
