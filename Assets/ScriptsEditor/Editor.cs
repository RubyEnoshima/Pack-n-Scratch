using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Editor : MonoBehaviour
{
    public GameObject Blocs;
    Pantalla Pantalla;
    public List<Variable> Variables;
    public int MaxVariables = 10;

    public dynamic ResultatBloc(int i){
        return Blocs.transform.GetChild(i).GetComponent<Bloc>().ResultatBloc();
    }

    public int AfegirVariable(Variable variable){
        if(Variables.Count<MaxVariables){
            Variables.Add(variable);
            return Variables.Count-1;
        }
        return -1;
    }

    public void ModificarVariable(int i, Variable variable){
        if(i>=0 && i<Variables.Count){
            Variables[i] = variable;
            foreach(Bloc b in Blocs.GetComponentsInChildren<Bloc>()){
                b.ActualitzarVariables();
            }
        }
    }

    public void EsborrarVariable(int i){
        if(i>=0 && i<Variables.Count){
            Variables.RemoveAt(i);
            foreach(Bloc b in Blocs.GetComponentsInChildren<Bloc>()){
                b.ActualitzarVariables();
            }
        }
    }

    // Retorna una llista d'OptionData pels dropdowns amb el nom de les variables
    public List<Dropdown.OptionData> DropVariables(){
        List<Dropdown.OptionData> llista = new List<Dropdown.OptionData>();
        foreach(var variable in Variables){
            llista.Add(new Dropdown.OptionData(variable.nom));
        }
        return llista;
    }

    // Retorna una llista d'OptionData pels dropdowns amb el numero dels blocs 
    public List<Dropdown.OptionData> DropBlocs(){
        List<Dropdown.OptionData> llista = new List<Dropdown.OptionData>();
        foreach(Bloc bloc in Blocs.GetComponentsInChildren<Bloc>()){
            llista.Add(new Dropdown.OptionData("#"+bloc.nBloc.ToString()));
        }
        return llista;
    }

    public void AfegirBloc(Bloc bloc){
        bloc.transform.parent = Blocs.transform;
        bloc.CanviarNombre(Blocs.transform.childCount);
    }

    public void TreureBloc(Bloc bloc){
        int n = 1;
        foreach(Bloc b in Blocs.GetComponentsInChildren<Bloc>()){
            b.CanviarNombre(n);
            b.ActualitzarBloc();
            b.ActualitzarVariables();
            n++;
        }
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
        Variables = new List<Variable>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
