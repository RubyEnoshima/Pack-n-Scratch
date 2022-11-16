using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bloc : MonoBehaviour
{
    public static Arrastrador Clicker;
    public Text Label;

    public Dropdown Variables = null;
    public Dropdown Blocs = null;

    public string Funcio;
    public bool colocat = false;
    public int nBloc = -1;
    public int VariableValor = -1;
    public int BlocValor = -1;

    protected Collider2D Collider;
    protected static Collider2D EditorCol;
    protected static Editor Editor;

    public Canvas Canvas;
    public SpriteRenderer Sprite;

    public GameObject Slot = null;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Clicker = FindObjectOfType<Arrastrador>();
        Collider = GetComponent<Collider2D>();
        EditorCol = GameObject.Find("Editor").GetComponent<Collider2D>();
        Editor = GameObject.Find("Editor").GetComponent<Editor>();
        Sprite = transform.gameObject.GetComponent<SpriteRenderer>();

        if(Variables)
            Variables.onValueChanged.AddListener(delegate {CanviarVariableNum();});
        if(Blocs)
            Blocs.onValueChanged.AddListener(delegate {CanviarBlocNum();});
    }

    public virtual void Executar(){

    }

    public virtual void CanviarVariableNum(){
        VariableValor = Variables.value;
    }

    public virtual void CanviarBlocNum(){
        BlocValor = Blocs.value;
    }

    // Actualitzem la llista de variables
    public virtual void ActualitzarVariables(){
        if(Variables){
            Variables.ClearOptions();
            List<Dropdown.OptionData> llista = Editor.DropVariables();
            List<Dropdown.OptionData> res = new List<Dropdown.OptionData>();
            int i = 0;
            foreach(var variable in llista){
                if(Editor.Variables[i].BlocIni >= nBloc)
                    break;

                res.Add(variable);
                i++;
            }
            Variables.AddOptions(res);
            if(Variables.options.Count!=0)
                Variables.value = VariableValor;
            else
                Variables.value = 0;
            
        }
    }

    // Actualitzem la llista de blocs
    public virtual void ActualitzarBloc(){
        if(Blocs){
            Blocs.ClearOptions();
            
            List<Dropdown.OptionData> llista = Editor.DropBlocs();
            Blocs.AddOptions(llista.GetRange(0,nBloc-1));
            Blocs.value = BlocValor;
            if(Blocs.options.Count==0){
                Blocs.value = 0;
            }

        }
    }

    public void CanviarNombre(int n){
        nBloc = n;
        Label.text = "#"+nBloc.ToString();
    }

    protected virtual void OnDestroy() {
        Editor.TreureBloc(this);
    }

    public virtual dynamic ResultatBloc(){
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        if(colocat && Clicker.selectedObject && Clicker.selectedObject==this.gameObject){
            //transform.parent = null;
            colocat = false;

            if(Slot){
                Editor.TreureSlot(Slot);
                Slot = null;
            }
        }else if(!colocat && !Clicker.selectedObject){
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // No és a sobre de l'editor en sí
            if(!EditorCol.OverlapPoint(pos)){
                Destroy(this.gameObject);
            }

            GameObject slot = Editor.ObtSlot(pos).gameObject;
            

            colocat = true;
            
            Editor.AfegirBloc(this,slot);
            ActualitzarBloc();
            ActualitzarVariables();
            
        }
    }
}
