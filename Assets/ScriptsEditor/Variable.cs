using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Variable : MonoBehaviour
{
    public enum TIPUSVAR{
        NOMBRE = 0,
        STRING = 1,
        BOOL = 2
    }
    protected int tipus;
    public string nom;
    public bool inicialitzat = false;

    public abstract dynamic Get<T>();
    public abstract void Crear(string _nom, dynamic _contingut);
    public abstract void Modificar(dynamic _contingut);
}
