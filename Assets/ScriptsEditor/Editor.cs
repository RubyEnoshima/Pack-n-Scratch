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

    public List<bool> Ple; // True si el slot i és ple
    public int paginaAct = 0; // Pàgina actual
    public int nPagines = 1; // Pàgina actual
    public GameObject PagSeg;
    public GameObject PagAnt;

    public string ResultatEsperat = "";
    public int nScripts = 0;
    public int nIncorrectes = 0;
    public int nCorrectes = 0;

    public Cafeina CafeinaScr;
    public int Cafeina = 5;

    public Collider2D ObtSlot(Vector3 posicioRatoli){
        if(Slot1.OverlapPoint(posicioRatoli)) return Slot1;
        else if(Slot2.OverlapPoint(posicioRatoli)) return Slot2;
        else if(Slot3.OverlapPoint(posicioRatoli)) return Slot3;
        else if(Slot4.OverlapPoint(posicioRatoli)) return Slot4;
        else return GetComponent<Collider2D>();
    }

    public void PaginaAnterior(){
        Blocs.transform.GetChild(paginaAct).gameObject.SetActive(false);

        paginaAct--;
        if(paginaAct<0) paginaAct = nPagines-1;

        Blocs.transform.GetChild(paginaAct).gameObject.SetActive(true);

        bool[] aux = {false,false,false,false};
        Ple = new List<bool>(aux);
        for(int i=0;i<Blocs.transform.GetChild(paginaAct).childCount;i++){
            Ple[i] = true;
        }

    }

    public void PaginaSeguent(){
        Blocs.transform.GetChild(paginaAct).gameObject.SetActive(false);

        paginaAct++;
        if(paginaAct>=nPagines) paginaAct = 0;

        Blocs.transform.GetChild(paginaAct).gameObject.SetActive(true);

        bool[] aux = {false,false,false,false};
        Ple = new List<bool>(aux);
        for(int i=0;i<Blocs.transform.GetChild(paginaAct).childCount;i++){
            Ple[i] = true;
        }
    }

    public void AfegirPagina(){
        GameObject novaPagina = new GameObject(nPagines.ToString());
        novaPagina.transform.parent = Blocs.transform;
        novaPagina.SetActive(false);
        nPagines++;
        if(nPagines>1){
            PagSeg.SetActive(true);
            PagAnt.SetActive(true);
        }
    }

    public void TreurePagina(){
        nPagines--;
        Blocs.transform.GetChild(nPagines).parent = null;
        if(nPagines<=1){
            PagSeg.SetActive(false);
            PagAnt.SetActive(false);
        }
    }

    public dynamic ResultatBloc(int i){
        if(i>=0 && i<Blocs.GetComponentsInChildren<Bloc>(true).Length)
            return Blocs.GetComponentsInChildren<Bloc>(true)[i].ResultatBloc();

        return null;
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
        foreach(Bloc bloc in Blocs.GetComponentsInChildren<Bloc>(true)){
            llista.Add(new Dropdown.OptionData("#"+bloc.nBloc.ToString()));
        }
        return llista;
    }

    public void AfegirBlocPagina(Bloc bloc, int pag){
        Transform pagina = Blocs.transform.GetChild(pag);
        if(pagina.childCount<4)
            bloc.transform.parent = pagina;
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
                GameObject SlotDefinitiu = GameObject.Find("Slot"+(aux+1).ToString());

                // Afegir el bloc
                bloc.transform.position = SlotDefinitiu.transform.position;
                bloc.Slot = SlotDefinitiu;
                bloc.CanviarNombre(aux+1+paginaAct*4);
                AfegirBlocPagina(bloc,paginaAct);
                foreach(Bloc fill in Blocs.GetComponentsInChildren<Bloc>(true)){
                    fill.ActualitzarBloc();
                }
                
                // Afegim una pàgina si és necessari
                if(Blocs.transform.GetChild(paginaAct).childCount==4){
                    AfegirPagina();
                }

            }else{
                Destroy(bloc.gameObject);

            }

        }else{
            Destroy(bloc.gameObject);
        }
    }

    public void TreureBloc(Bloc bloc){
        int n = 1;
        GameObject Slot = bloc.Slot;
        foreach(Bloc b in Blocs.GetComponentsInChildren<Bloc>()){
            PujarSlot(b);
            b.CanviarNombre(n);
            b.ActualitzarBloc();
            b.ActualitzarVariables();
            n++;
        }

        if(Blocs.transform.GetChild(paginaAct).childCount==4){
            TreurePagina();
        }
    }

    public void TreureSlot(GameObject Slot){
        int ultimChar = (Slot.name[Slot.name.Length-1] - '0')-1;
        Ple[ultimChar] = false;
    }

    // Puja un slot el bloc si es pot
    public void PujarSlot(Bloc bloc){
        GameObject Slot = bloc.Slot;
        int ultimChar = (Slot.name[Slot.name.Length-1] - '0')-1;
        if(ultimChar==0 || Ple[ultimChar-1]) return;

        Ple[ultimChar] = false;
        Ple[ultimChar-1] = true;
        GameObject SlotFinal = GameObject.Find("Slot"+ultimChar);
        bloc.transform.position = SlotFinal.transform.position;
        bloc.Slot = SlotFinal;
    }

    

    public void Compilar(){
        foreach(Transform pagina in Blocs.transform){
            foreach(Transform bloc in pagina){
                bloc.GetComponent<Bloc>().Executar();

            }
        }
        if(EsCorrecte()){
            nScripts++;
            Cafeina++;
        }else{
            nIncorrectes++;
            Cafeina--;
        }
        CafeinaScr.ActualitzarCafeina(Cafeina);
        // Mostrar gag
    }

    public float CalcularMitjana(){
        return (nIncorrectes+nCorrectes)/nScripts;
    }

    public void UtilitzarCafeina(){

    }

    public void CarregarScript(){

    }

    public bool EsCorrecte(){
        return true;//Pantalla.text.text == ResultatEsperat;
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
