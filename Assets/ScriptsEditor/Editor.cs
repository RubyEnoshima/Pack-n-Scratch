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

    public Collider2D Slot1;
    public Collider2D Slot2;
    public Collider2D Slot3;
    public Collider2D Slot4;

    public List<bool> Ple;
    public int nPagina = 0;

    public Collider2D ObtSlot(Vector3 posicioRatoli){
        if(Slot1.OverlapPoint(posicioRatoli)) return Slot1;
        else if(Slot2.OverlapPoint(posicioRatoli)) return Slot2;
        else if(Slot3.OverlapPoint(posicioRatoli)) return Slot3;
        else if(Slot4.OverlapPoint(posicioRatoli)) return Slot4;
        else return GetComponent<Collider2D>();
    }

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


    public void AfegirBloc(Bloc bloc, GameObject Slot){
        char ultimChar = Slot.name[Slot.name.Length-1];
        if(ultimChar>='0' && ultimChar<='9'){
            int pos = (ultimChar - '0')-1;
            bool ple = Ple[pos];
            if(!ple){
                int aux = pos-1;
                while(aux>=0 && !Ple[aux]) aux--;
                aux++;
                Ple[aux] = true;
                Debug.Log("ME voy al slot "+aux);
                GameObject SlotDefinitiu = GameObject.Find("Slot"+(aux+1).ToString());
                // Afegir el bloc
                bloc.transform.position = SlotDefinitiu.transform.position;
                bloc.Slot = SlotDefinitiu;
                bloc.CanviarNombre(aux+1+nPagina);
                bloc.transform.parent = Blocs.transform;
                foreach(Bloc fill in Blocs.GetComponentsInChildren<Bloc>()){
                    fill.ActualitzarBloc();
                }
            }else{
                Destroy(bloc.gameObject);

            }

        }else{
            Destroy(bloc.gameObject);
        }



        // MIERDA PURA

        // List<Transform> fills = new List<Transform>();
        // foreach(Transform fill in Blocs.transform){
        //     fills.Add(fill);
        //     fill.parent = null;
        // }
        // fills.Add(bloc.transform);
        // Debug.Log(fills.Count);
        // fills.Sort((Transform t1, Transform t2) => {return t2.position.y==t1.position.y ? t2.position.x.CompareTo(t1.position.x) : t2.position.y.CompareTo(t1.position.y);});
        // int i=1;
        // foreach(Transform fill in fills){
        //     fill.name = i.ToString();
        //     Debug.Log(fill.name);
        //     fill.GetComponent<Bloc>().CanviarNombre(i);
        //     fill.parent = Blocs.transform;
        //     i++;
        // }
    }

    public void TreureSlot(GameObject Slot){
        int ultimChar = (Slot.name[Slot.name.Length-1] - '0')-1;
        Ple[ultimChar] = false;
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

    public void CarregarScript(){

    }

    public bool EsCorrecte(){
        return true;
    }

    // Start is called before the first frame update
    void Start()
    {
        Blocs = transform.Find("Blocs").gameObject;
        Variables = new List<Variable>();
        bool[] aux = {false,false,false,false};
        Ple = new List<bool>(aux);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
