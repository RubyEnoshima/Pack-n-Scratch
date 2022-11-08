using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Editor : MonoBehaviour
{
    GameObject Blocs;
    Pantalla Pantalla;
    public List<Variable> Variables;
    public int MaxVariables = 10;

    public void AfegirVariable(Variable variable){
        if(Variables.Count<MaxVariables){
            Variables.Add(variable);
        }
    }

    public void AfegirBloc(Bloc bloc){
        bloc.transform.parent = Blocs.transform;
        bloc.CanviarNombre(Blocs.transform.childCount);
    }

    public void Compilar(){
        foreach(Transform bloc in Blocs.transform){
            bloc.GetComponent<Bloc>().Executar();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Blocs = transform.Find("Blocs").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
