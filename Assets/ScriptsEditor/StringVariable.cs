using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringVariable : Variable
{
    string contingut;

    public override dynamic Get<T>(){
        return contingut;
    }

    public override void Crear(string _nom, dynamic _contingut){
        nom = _nom;
        Modificar((string)_contingut);
        
    }

    public override void Modificar(dynamic _contingut){
        if(_contingut is string){
            contingut = (string)_contingut;
            inicialitzat = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        tipus = (int)TIPUSVAR.STRING;
        contingut = "";
    }
}
