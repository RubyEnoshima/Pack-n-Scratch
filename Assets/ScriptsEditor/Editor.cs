using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Editor : MonoBehaviour
{
    class IFStruct{
        public BlocIf IF;
        public bool saltantIf = false;
        public bool dinsIf = false;
    };
    public GameObject MAIN;
    public VideoPlayer GAG;
    public GameObject Text1;
    public GameObject Text2;
    public GameObject OCULTARGAG;
    public GameObject OCULTARCANVAS;
    public Transform MainPos;
    public Transform GagPos;

    public GameObject Blocs;
    public Pantalla Pantalla;
    public List<Variable> Variables;
    public int MaxVariables = 10;
    public GameObject Generadors;

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

    public int TotalScripts;
    public int nScripts = 0;
    public float nIncorrectes = 0;
    public int nCorrectes = 0;
    public int intents = 3;
    bool executant = false;
    bool esProva = false;

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
    public Tutorial tutorial;
    public Button TancarBoto;
    public Button ResetejarBoto;
    public Button CafeinaBoto;
    public Button NotificacioCorreu;
    public Button NotificacioPaquet;
    public Button PensamentBoto;
    public Button SortirBoto;
    public GameObject PistaCafeina;
    public bool PlaygroundMode = false;

    void OnDestroy() {
        destruint = true;
        Global.pistaOfi = false;
    }

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
        else return null;
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
        if(nPagines==1) return;
        nPagines--;
        Transform pagina = Blocs.transform.GetChild(nPagines);
        pagina.parent = null;
        Destroy(pagina.gameObject);
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

                bloc.enEditor = true;

            }else{
                //bloc.enEditor = false;
                Destroy(bloc.gameObject);

            }

        }else{
            //bloc.enEditor = false;
            Destroy(bloc.gameObject);
        }
    }

    public void TreureBloc(Bloc bloc){
        if(!destruint){
            GameObject Slot = bloc.Slot;
            
            foreach(Bloc b in Blocs.GetComponentsInChildren<Bloc>(true)){
                
                if(b.nBloc<=bloc.nBloc) continue;

                PujarSlot(b);
                b.ActualitzarBloc();
                b.ActualitzarVariables();

            }

            int index = Blocs.transform.childCount-2;
            if(index>=0){
                Transform ultimapagina = Blocs.transform.GetChild(index);
                int pagBloc = Mathf.CeilToInt(bloc.nBloc/4);
                int count = ultimapagina.childCount;
                if(index==pagBloc) count--;
                if(count==3) TreurePagina();

            }

            bool[] aux = {false,false,false,false};
            Ple = new List<bool>(aux);
            for(int i=0;i<Blocs.transform.GetChild(paginaAct).childCount-1;i++){
                Ple[i] = true;
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
        if(ultimChar==0 && bloc.nBloc==1) return;

        GameObject SlotFinal;
        int pagBloc = Mathf.CeilToInt(bloc.nBloc/4);
        if(ultimChar==0 && pagBloc>paginaAct){ // Si es troba en una pagina >a l'actual i al slot 1
            int paginaNova = bloc.nBloc/4 - 1;
            bloc.transform.parent = Blocs.transform.GetChild(paginaNova);
            // if(Blocs.transform.GetChild(paginaNova+1).childCount==0) TreurePagina();
            SlotFinal = Slot4.gameObject;
        }else{
            SlotFinal = GameObject.Find("Slot"+ultimChar);
        }

        
        
        bloc.transform.position = SlotFinal.transform.position;
        bloc.Slot = SlotFinal;
        bloc.nBloc--;
        bloc.CanviarNombre(bloc.nBloc);
    }

    public void MoureEditor(Transform posicio){
        this.transform.position = posicio.position;
        this.transform.localScale = posicio.localScale;
    }

    public void StopGag(){
        GAG.gameObject.SetActive(false);
        GAG.Stop();
        GAG.clip = null;
        OCULTARGAG.SetActive(true);
        OCULTARCANVAS.SetActive(true);
        MoureEditor(MainPos);
    }

    public void PlayGag(bool correcte){
        GAG.gameObject.SetActive(true);
        OCULTARGAG.SetActive(false);
        OCULTARCANVAS.SetActive(false);
        MoureEditor(GagPos);
        GAG.clip = Global.ObtenirGag(correcte);
        GAG.Play();
    }

    public void PostCompilar(){
        TotalScripts--;
        nScripts++;
        if(TotalScripts==0){
            Dia.AcabarDia(nCorrectes,(int)nIncorrectes,CalcularMitjana());
            NotificacioCorreu.gameObject.SetActive(false);
            NotificacioPaquet.gameObject.SetActive(false);
            Notificacio.gameObject.SetActive(false);
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
            Outputs.Add(((string)b.ResultatBloc()).ToLower());
        }
    }

    public IEnumerator EsperarGag(){
        float temps = (float)(GAG.length - GAG.time);
        Debug.Log(temps);
        yield return new WaitForSeconds(temps);
        StopGag();
        if(!error){
            FletxaExecucio.SetActive(false);
            TornarPrimeraPagina();
        }
    }

    public IEnumerator ExecutarLent(){
        TornarPrimeraPagina();

        ExecutarRapid(); // Per saber quin gag hem de posar, si un correcte o incorrecte
        List<string> res = new List<string>();
        if(!PlaygroundMode) res = ScriptActual.ResultatsEsperats1.ConvertAll(d => d.ToLower());
        bool correcte = PlaygroundMode || (!error && Enumerable.SequenceEqual(Outputs, res));
        PlayGag(correcte);

        Outputs = new List<string>();
        FletxaExecucio.SetActive(true);
        Pantalla.CanviarText("");
        Pantalla.gameObject.SetActive(correcte);
        error = false;

        int ifs = 0;
        int ifsAux = 0;
        List<IFStruct> IFS = new List<IFStruct>();
        IFStruct actual = null;

        Pantalla Pista = FletxaExecucio.GetComponent<Pantalla>();
        foreach(Transform pagina in Blocs.transform){
            FletxaExecucio.SetActive(true);
            foreach(Transform bloc in pagina){
                Bloc b = bloc.GetComponent<Bloc>();

                if(ifs>0) actual = IFS.Last();
                if(b is BlocIf) ifsAux++;
                // Si hem de saltar, saltem fins que trobem el tope
                if(( actual != null && actual.saltantIf && !(b is BlocElse) && !(b is BlocEndIf))) continue;
                
                b.Executar();
                if(b is BlocIf || b is BlocElse){
                    if(b is BlocIf){
                        ifs++;
                        IFStruct ifstruct = new IFStruct();
                        ifstruct.IF = b as BlocIf;
                        IFS.Add(new IFStruct());
                        actual = ifstruct;
                    }
                    else if(b is BlocElse && ifs<=0) (b as BlocElse).PosarError();

                    if(b is BlocIf && b.ResultatBloc() || b is BlocElse && actual != null && actual.saltantIf){
                        actual.dinsIf = true;
                        actual.saltantIf = false;
                        IFS[ifs-1] = actual;
                    }else{
                        actual.dinsIf = false;
                        actual.saltantIf = true;
                        IFS[ifs-1] = actual;

                    }
                }else if(b is BlocEndIf && actual != null && (actual.dinsIf || actual.saltantIf)){
                    ifs--;
                    ifsAux--;
                    IFS.Remove(actual);
                    if(ifsAux < 0) (b as BlocEndIf).PosarError(); // No hauria de passar
                }
                
                GuardarOutput(b);

                FletxaExecucio.transform.position = new Vector3(FletxaExecucio.transform.position.x,
                                                                bloc.transform.position.y,
                                                                FletxaExecucio.transform.position.z);
                Pista.CanviarText(b.ResultatBloc().ToString());

                if(b.TeErrors()){
                    Debug.Log(b.ObtenirError());
                    Pensament.CanviarText(b.ObtenirError());
                    Pensament.FerApareixer();
                    error = true;
                    break;
                }


                yield return new WaitForSeconds(1.2f);
            }
            if(error) break;
            PaginaSeguent();
        }

        // Ifs no tancats
        if(ifs > 0){
            Debug.Log("Hi ha un if que no esta ben tancat...");
            Pensament.CanviarText("Hi ha un if que no esta ben tancat...");
            Pensament.FerApareixer();
            error = true;
            FletxaExecucio.transform.position = new Vector3(FletxaExecucio.transform.position.x,
                                                                IFS[ifs-1].IF.transform.position.y,
                                                                FletxaExecucio.transform.position.z);
            Pista.CanviarText("ups");
            FletxaExecucio.SetActive(true);
        }

        yield return StartCoroutine(EsperarGag());
        
    }

    public void ExecutarRapid(){
        Outputs = new List<string>();
        int ifs = 0;
        int ifsAux = 0;
        List<IFStruct> IFS = new List<IFStruct>();
        IFStruct actual = null;
        foreach(Transform pagina in Blocs.transform){
            foreach(Transform bloc in pagina){
                Bloc b = bloc.GetComponent<Bloc>();

                if(ifs>0) actual = IFS.Last();
                if(b is BlocIf) ifsAux++;
                // Si hem de saltar, saltem fins que trobem el tope
                if(( actual != null && actual.saltantIf && !(b is BlocElse) && !(b is BlocEndIf))) continue;

                b.Executar();

                if(b is BlocIf || b is BlocElse){
                    if(b is BlocIf){
                        ifs++;
                        IFStruct ifstruct = new IFStruct();
                        ifstruct.IF = b as BlocIf;
                        IFS.Add(new IFStruct());
                        actual = ifstruct;
                    }
                    else if(b is BlocElse && ifs<=0) (b as BlocElse).PosarError();

                    if(b is BlocIf && b.ResultatBloc() || b is BlocElse && actual != null && actual.saltantIf){
                        actual.dinsIf = true;
                        actual.saltantIf = false;
                        IFS[ifs-1] = actual;
                    }else{
                        actual.dinsIf = false;
                        actual.saltantIf = true;
                        IFS[ifs-1] = actual;

                    }
                }else if(b is BlocEndIf && actual != null && (actual.dinsIf || actual.saltantIf)){
                    ifs--;
                    ifsAux--;
                    IFS.Remove(actual);
                    if(ifsAux < 0) (b as BlocEndIf).PosarError(); // No hauria de passar
                }

                GuardarOutput(bloc.GetComponent<Bloc>());
                
                // Ho comprovem, però en iteracions seguents no hi hauria d'haver
                if(b.TeErrors()){ 
                    Debug.Log(b.ObtenirError());
                    Pensament.CanviarText(b.ObtenirError());
                    Pensament.FerApareixer();
                    error = true;
                }
            }
            if(error) break;
        }

        // Ifs no tancats
        if(ifs > 0){
            Debug.Log("Hi ha un if que no esta ben tancat...");
            Pensament.CanviarText("Hi ha un if que no esta ben tancat...");
            Pensament.FerApareixer();
            error = true;
        }
    }

    public IEnumerator CompilarAux(){
        if(!executant){
            executant = true;

            yield return StartCoroutine(ExecutarLent());

            if(EsCorrecte()){
                if(!PlaygroundMode){
                    Debug.Log("Bien");
                    nCorrectes++;
                    if(Cafeina<10) Cafeina+=3;
                    else Cafeina = 10;
                
                    Pensament.CanviarText("Sembla que ho he fet be!");
                    Pensament.FerApareixer();
                    Global.Fet(nScriptActual);
                    Global.EsPotDesbloquejar();
                    PostCompilar();

                }
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
        if(Global.tutoEditor){
            StartCoroutine(CompilarAux());
            Arrastrador.Actiu = false;

        }
    }

    public float CalcularMitjana(){
        return 1-nIncorrectes/nScripts;
    }

    public void UtilitzarCafeina(){
        if(Cafeina>=3 && !PlaygroundMode){
            Cafeina -= 3;
            CafeinaScr.ActualitzarCafeina(Cafeina);
            Debug.Log(ScriptActual.Inspiracio);
            Pensament.CanviarText(ScriptActual.Inspiracio);
            Pensament.FerApareixer();
        }
    }

    public void Resetejar(){
        if(esProva) CarregarScript("Prova");
        else CarregarScript("Script"+nScriptActual);
    }

    public bool EsCorrecte(){
        if(PlaygroundMode) return true;
        List<string> res = ScriptActual.ResultatsEsperats1.ConvertAll(d => d.ToLower());
        if(!error && Enumerable.SequenceEqual(Outputs, res)){
            CanviarInput(ScriptActual,1);
            ExecutarRapid();
            res = ScriptActual.ResultatsEsperats2.ConvertAll(d => d.ToLower());
            if(Enumerable.SequenceEqual(Outputs, res)){
                CanviarInput(ScriptActual,2);
                ExecutarRapid();
                res = ScriptActual.ResultatsEsperats3.ConvertAll(d => d.ToLower());
                return Enumerable.SequenceEqual(Outputs, res);
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
    
    public void CarregarScript(string scriptNom){
        if(PlaygroundMode){
            Notificacio.Assumpte.text = "Playground";
            Notificacio.Nom.text = "Playground";
            Notificacio.Descripcio.text = "Fes el que vulguis sense preocupar-te de cap restriccio ni script.";
            float inputVal = Mathf.Round(UnityEngine.Random.Range(0f,100f) * 100f) / 100f;
            InputPantalla.CanviarText(inputVal.ToString());
            FloatVariable input = new FloatVariable();
            input.Crear("Input",inputVal,0);
            if(Variables.Count==0)
                Variables.Add(input);
            else
                Variables[0] = input;
            return;
        }

        Variables = new List<Variable>();
        Pantalla.CanviarText("");
        destruint = true;
        TornarPrimeraPagina();
        foreach(Bloc child in Blocs.GetComponentsInChildren<Bloc>(true)){
            child.DestruirFlag = true;
            child.transform.parent = null;
            DestroyImmediate(child.gameObject);
        }
        while(nPagines>1) TreurePagina();
        destruint = false;

        if(esProva) nScriptActual = -1;
        else nScriptActual = int.Parse(scriptNom[scriptNom.Length-1].ToString());

        GameObject scriptPrefab = (GameObject)Resources.Load("Scripts/"+Global.Departament+"/"+scriptNom, typeof(GameObject));
        
        GameObject script = Instantiate(scriptPrefab);

        ScriptActual = script.GetComponent<Script>();
        CanviarInput(ScriptActual,0);
        bool[] aux = {false,false,false,false};
        Ple = new List<bool>(aux);
        int j = 1;
        foreach(Bloc fill in script.GetComponentsInChildren<Bloc>(true)){
            fill.Editor = this;
            fill.Pantalla = Pantalla;
            fill.colocat = true;
            fill.enEditor = true;

            GameObject slot = GameObject.Find("Slot"+(j).ToString());
            
            AfegirBloc(fill,slot);
            fill.Iniciar();
            j++;
            if(j>4){
                j=1;
                PaginaSeguent();
                //Ple = new List<bool>(aux1);
            }

        }
        
        for(int i=0;i<Blocs.transform.GetChild(paginaAct).childCount-1;i++){
            Ple[i] = true;
        }
        
        Notificacio.CarregarScript(ScriptActual);
        Destroy(script);
        TornarPrimeraPagina();
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
        if(Global.tutoEditor){
            tutorial.Pistas[0].gameObject.SetActive(false);
            PistaCafeina.SetActive(true);
            Destroy(tutorial.gameObject);
        }else{
            Arrastrador.Actiu = false;
            TancarBoto.interactable = false;
            ResetejarBoto.interactable = false;
            CafeinaBoto.interactable = false;
            NotificacioCorreu.interactable = false;
            NotificacioPaquet.interactable = false;
            PensamentBoto.interactable = false;
            SortirBoto.interactable = false;
        }

        Blocs = transform.Find("Blocs").gameObject;
        Variables = new List<Variable>();
        
        bool[] aux = {false,false,false,false};
        Ple = new List<bool>(aux);

        GAG.isLooping = false;

        if(!Global.EsPotFerProva() && !PlaygroundMode){
            // Agafar un al atzar
            CarregarScript(Global.ScriptAleatori());
            // CarregarScript("Script2");
            TotalScripts = Global.ScriptsRestants()-1;
            if(TotalScripts==0) TotalScripts = 1;
            else if(Global.ScriptsRestants()==2) TotalScripts = 2;

        }else{
            esProva = true;
            CarregarScript("Prova");
            TotalScripts = 1;
        }

        if(Global.Departament=="Departament2"){
            Text1.SetActive(false);
            Text2.SetActive(true);
            Pantalla.text = Text2.GetComponent<Text>();
        }else if(!PlaygroundMode){
            Generadors.transform.GetChild(6).gameObject.SetActive(false);
            Generadors.transform.GetChild(7).gameObject.SetActive(false);
            Generadors.transform.GetChild(8).gameObject.SetActive(false);
        }

        Notificacio.CanviarNotis(TotalScripts);

        if(Global.ModeDaltonic){
            foreach(Transform g in Generadors.transform){
                if(g.name=="GeneradorComentaris") continue;
                SpriteRenderer generador = g.GetChild(0).GetComponent<SpriteRenderer>();
                generador.sprite = Resources.Load<Sprite>("Sprites/"+generador.sprite.name+"_daltonic");
            }
        }

        // ACORDARSE DE DESCOMENTAR!!!!!!!!!!!!!!!!!!!
        SceneManager.sceneUnloaded += new UnityEngine.Events.UnityAction<Scene>(delegate {if(MAIN) MAIN.SetActive(true);});
        Tancar();
    }

    // Update is called once per frame
    void Update()
    {
        if(!Global.tutoEditor && tutorial.desbloquejat){
            Global.tutoEditor = true;
            Arrastrador.Actiu = true;
            TancarBoto.interactable = true;
            ResetejarBoto.interactable = true;
            CafeinaBoto.interactable = true;
            NotificacioCorreu.interactable = true;
            NotificacioPaquet.interactable = true;
            PensamentBoto.interactable = true;
            SortirBoto.interactable = true;

        }
    }
}
