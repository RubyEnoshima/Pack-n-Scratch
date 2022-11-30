using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Editor : MonoBehaviour
{
    public GameObject MAIN;
    public GameObject Blocs;
    public Pantalla Pantalla;
    public List<Variable> Variables;
    public int MaxVariables = 10;

    public dynamic Input = 10;
    public Pantalla InputPantalla;

    public Collider2D Slot1;
    public Collider2D Slot2;
    public Collider2D Slot3;
    public Collider2D Slot4;

    public List<bool> Ple; // True si el slot i és ple
    public int paginaAct = 0; // Pàgina actual
    public int nPagines = 1; // Pàgina actual
    public GameObject PagSeg;
    public GameObject PagAnt;

    public int TotalScripts = 2; // De moment
    public int nScripts = 0;
    public float nIncorrectes = 0;
    public int nCorrectes = 0;
    public int intents = 3;
    bool executant = false;

    public Cafeina CafeinaScr;
    public int Cafeina = 5;
    public Dia Dia;
    public Notificacio Notificacio;
    public Script ScriptActual;
    int nScriptActual = -1;
    public Arrastrador Arrastrador;

    public GameObject FletxaExecucio;
    List<string> Outputs;
    bool error = false;
    bool destruint = false;
    public Pensament Pensament;

    public void CanviarInput(Script s, int inputNum){
        if(s.SonInputsNombres){
            float inputVal = s.Inputs[inputNum];
            InputPantalla.CanviarText(inputVal.ToString());
            FloatVariable input = new FloatVariable();
            input.Crear("Input",inputVal,0);
            if(Variables.Count==0)
                Variables.Add(input);
            else
                Variables[0] = input;

        }else{
            string inputVal = s.InputsString[inputNum];
            InputPantalla.CanviarText(inputVal);
            StringVariable input = new StringVariable();
            input.Crear("Input",inputVal,0);
            if(Variables.Count==0)
                Variables.Add(input);
            else
                Variables[0] = input;
        }
    }

    public Collider2D ObtSlot(Vector3 posicioRatoli){
        if(Slot1.OverlapPoint(posicioRatoli)) return Slot1;
        else if(Slot2.OverlapPoint(posicioRatoli)) return Slot2;
        else if(Slot3.OverlapPoint(posicioRatoli)) return Slot3;
        else if(Slot4.OverlapPoint(posicioRatoli)) return Slot4;
        else return GetComponent<Collider2D>();
    }

    public void PaginaAnterior(){
        if(FletxaExecucio.activeSelf) FletxaExecucio.SetActive(false);
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
        if(FletxaExecucio.activeSelf) FletxaExecucio.SetActive(false);

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

    public void TornarPrimeraPagina(){
        while(paginaAct!=0){
            PaginaSeguent();
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
        Destroy(Blocs.transform.GetChild(nPagines).gameObject);
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
        if(i>0 && i<Variables.Count){
            Variables[i] = variable;
            foreach(Bloc b in Blocs.GetComponentsInChildren<Bloc>()){
                b.ActualitzarVariables();
            }
        }
    }

    public void EsborrarVariable(int i){
        if(i>0 && i<Variables.Count){
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
        if(!destruint){
            int n = 1;
            GameObject Slot = bloc.Slot;
            foreach(Bloc b in Blocs.GetComponentsInChildren<Bloc>(true)){
                if(b==bloc) continue;

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
    }

    public void TreureSlot(GameObject Slot){
        int ultimChar = (Slot.name[Slot.name.Length-1] - '0')-1;
        Ple[ultimChar] = false;
        
    }

    // Puja un slot el bloc si es pot
    public void PujarSlot(Bloc bloc){
        GameObject Slot = bloc.Slot;
        if(Slot==null) return;
        int ultimChar = (Slot.name[Slot.name.Length-1] - '0')-1;
        if(ultimChar==0 && bloc.nBloc==1 || ultimChar!=0 && Ple[ultimChar-1]) return;

        GameObject SlotFinal;
        if(ultimChar==0 && bloc.nBloc>=4){ // Si es troba en una pagina >1 i al slot 1
            int paginaNova = bloc.nBloc/4 - 1;
            bloc.transform.parent = Blocs.transform.GetChild(paginaNova);
            // if(Blocs.transform.GetChild(paginaNova+1).childCount==0) TreurePagina();
            SlotFinal = Slot4.gameObject;
            Ple[3] = true;
        }else{
            Ple[ultimChar] = false;
            if(ultimChar!=0) Ple[ultimChar-1] = true;
            SlotFinal = GameObject.Find("Slot"+ultimChar);

        }
        
        bloc.transform.position = SlotFinal.transform.position;
        bloc.Slot = SlotFinal;
        bloc.nBloc--;
    }

    public void PostCompilar(){
        TotalScripts--;
        nScripts++;
        if(TotalScripts==0){
            Dia.AcabarDia(nCorrectes,(int)nIncorrectes,CalcularMitjana());
            this.gameObject.SetActive(false);
            Arrastrador.Actiu = false;
        }
        else{
            Notificacio.CanviarNotis(TotalScripts);
            // Agafar un script al atzar
            CarregarScript(Global.ScriptAleatori());
            intents = 3;
        }
    }

    public void GuardarOutput(Bloc b){
        if(b is BlocPrint){
            Outputs.Add(b.ResultatBloc());
        }
    }

    public IEnumerator ExecutarLent(){
        TornarPrimeraPagina();
        Outputs = new List<string>();
        FletxaExecucio.SetActive(true);
        error = false;
        foreach(Transform pagina in Blocs.transform){
            foreach(Transform bloc in pagina){
                Bloc b = bloc.GetComponent<Bloc>();
                b.Executar();
                GuardarOutput(b);

                FletxaExecucio.transform.position = new Vector3(FletxaExecucio.transform.position.x,
                                                                bloc.transform.position.y,
                                                                FletxaExecucio.transform.position.z);
    
                if(b.TeErrors()){
                    Debug.Log(b.ObtenirError());
                    Pensament.CanviarText(b.ObtenirError());
                    Pensament.FerApareixer();
                    error = true;
                    break;
                }


                yield return new WaitForSeconds(0.75f);
            }
            if(error) break;
            PaginaSeguent();
        }
        if(!error){
            FletxaExecucio.SetActive(false);
            TornarPrimeraPagina();
        }
    }

    public void ExecutarRapid(){
        Outputs = new List<string>();
        foreach(Transform pagina in Blocs.transform){
            foreach(Transform bloc in pagina){
                bloc.GetComponent<Bloc>().Executar();
                GuardarOutput(bloc.GetComponent<Bloc>());

            }
        }
    }

    public IEnumerator CompilarAux(){
        if(!executant){
            executant = true;

            yield return StartCoroutine(ExecutarLent());

            if(EsCorrecte()){
                Debug.Log("Bien");
                nCorrectes++;
                if(Cafeina<10) Cafeina++;
                if(ScriptActual.esDesbloqueig){
                    Global.estaDesbloquejat = true;
                }
                Debug.Log(nScriptActual);
                Global.Fet(nScriptActual);
                PostCompilar();
            }else{
                Debug.Log("Mal");
                CanviarInput(ScriptActual,0);
                intents--;
                nIncorrectes += 0.3f;
                if(intents==0){
                    if(Cafeina>0) Cafeina--;
                    PostCompilar();
                }
            }
            CafeinaScr.ActualitzarCafeina(Cafeina);
            // Mostrar gag
            Arrastrador.Actiu = true;
            executant = false;
        }
    }

    public void Compilar(){
        StartCoroutine(CompilarAux());
        Arrastrador.Actiu = false;
    }

    public float CalcularMitjana(){
        return 1-nIncorrectes/nScripts;
    }

    public void UtilitzarCafeina(){
        if(Cafeina>=3){
            Cafeina -= 3;
            CafeinaScr.ActualitzarCafeina(Cafeina);
            Debug.Log(ScriptActual.Inspiracio);
            Pensament.CanviarText(ScriptActual.Inspiracio);
            Pensament.FerApareixer();
        }
    }

    public void Resetejar(){
        CarregarScript("Script"+nScriptActual);
    }

    public void CarregarScript(string scriptNom){
        Variables = new List<Variable>();
        Pantalla.CanviarText("");
        destruint = true;
        foreach(Bloc child in Blocs.GetComponentsInChildren<Bloc>(true)){
            DestroyImmediate(child.gameObject);
        }
        while(nPagines>1) TreurePagina();
        destruint = false;
        nScriptActual = int.Parse(scriptNom[scriptNom.Length-1].ToString());
        GameObject scriptPrefab = (GameObject)Resources.Load("Scripts/"+Global.Departament+"/"+scriptNom, typeof(GameObject));
        
        GameObject script = Instantiate(scriptPrefab);

        ScriptActual = script.GetComponent<Script>();
        CanviarInput(ScriptActual,0);

        bool[] aux1 = {false,false,false,false};
        bool[] aux = {false,false,false,false};
        Ple = new List<bool>(aux);
        int i = 0, j = 1;
        foreach(Bloc fill in script.GetComponentsInChildren<Bloc>(true)){
            fill.Editor = this;
            fill.Pantalla = Pantalla;
            fill.colocat = true;
            fill.enEditor = true;
            
            if(i<4) {
                aux[i] = true;
                i++;
            }

            GameObject slot = GameObject.Find("Slot"+(j).ToString());
            
            AfegirBloc(fill,slot);
            fill.Iniciar();
            j++;
            if(j>4){
                j=1;
                PaginaSeguent();
                Ple = new List<bool>(aux1);
            }

        }
        Ple = new List<bool>(aux);
        
        Notificacio.CarregarScript(ScriptActual);
        Destroy(script);
        PaginaSeguent();
    }

    public bool EsCorrecte(){
        if(!error && Enumerable.SequenceEqual(Outputs, ScriptActual.ResultatsEsperats1)){
            CanviarInput(ScriptActual,1);
            ExecutarRapid();
            if(Enumerable.SequenceEqual(Outputs, ScriptActual.ResultatsEsperats2)){
                CanviarInput(ScriptActual,2);
                ExecutarRapid();
                return Enumerable.SequenceEqual(Outputs, ScriptActual.ResultatsEsperats3);
            }
            Debug.Log("El programa no funciona amb altres inputs...");
            Pensament.CanviarText("Sembla que el programa no funciona amb inputs diferents...");
            Pensament.FerApareixer();
        }
        else if(!error) {
            Debug.Log("Hm, l'output no es el que es demanava");
            Pensament.CanviarText("Hm, l'output no es el que es demanava...");
            Pensament.FerApareixer();
        }
        return false;
    }

    public void Sortir(){
        SceneManager.LoadScene("Mapa");
    }
    
    public void Tancar(){
        SceneManager.LoadScene("Oficina",LoadSceneMode.Additive);
        MAIN.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        Blocs = transform.Find("Blocs").gameObject;
        Variables = new List<Variable>();
        
        bool[] aux = {false,false,false,false};
        Ple = new List<bool>(aux);

        // Agafar un al atzar
        // CarregarScript(Global.ScriptAleatori());
        CarregarScript("Script1");
        Notificacio.CanviarNotis(TotalScripts);
        // ACORDARSE DE DESCOMENTAR!!!!!!!!!!!!!!!!!!!
        SceneManager.sceneUnloaded += new UnityEngine.Events.UnityAction<Scene>(delegate {MAIN.SetActive(true);});
        Tancar();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
