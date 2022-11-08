using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatVariable : Variable
{
    float contingut;

    public override dynamic Get<T>(){
        return contingut;
    }

    public override void Crear(string _nom, dynamic _contingut){
        nom = _nom;
        Modificar((float)_contingut);
    }

    public override void Modificar(dynamic _contingut){
        if(_contingut is float){
            contingut = _contingut;
            inicialitzat = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        tipus = (int)TIPUSVAR.NOMBRE;
        contingut = 0;
    }
}
