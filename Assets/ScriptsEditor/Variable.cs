using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Variable
{
    public enum TIPUSVAR{
        NOMBRE = 0,
        STRING = 1,
        BOOL = 2
    }
    protected int tipus;
    public string nom;
    public bool inicialitzat = false;
    public int BlocIni = -1;

    public abstract dynamic Get();
    public abstract void Crear(string _nom, dynamic _contingut, int bloc);
    public abstract void Modificar(dynamic _contingut);
}
