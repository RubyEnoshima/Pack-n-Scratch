using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bloc : MonoBehaviour
{
    public Arrastrador Clicker;
    public Text Label;

    public Dropdown Variables = null;
    public Dropdown Blocs = null;

    public string Funcio;
    public bool colocat = false;
    public int nBloc = -1;

    Collider2D Collider;
    Collider2D EditorCol;
    protected Editor Editor;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Clicker = FindObjectOfType<Arrastrador>();
        Collider = GetComponent<Collider2D>();
        EditorCol = GameObject.Find("Editor").GetComponent<Collider2D>();
        Editor = GameObject.Find("Editor").GetComponent<Editor>();
        
    }

    public virtual void Executar(){

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
        }
    }

    // Actualitzem la llista de blocs
    public virtual void ActualitzarBloc(){
        if(Blocs){
            Blocs.ClearOptions();
            List<Dropdown.OptionData> llista = Editor.DropBlocs();
            Blocs.AddOptions(llista.GetRange(0,nBloc-1));
            if(Blocs.options.Count==0) Blocs.ClearOptions();
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
            colocat = false;
        }else if(!colocat && !Clicker.selectedObject){
            if(!EditorCol.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition))){
                Destroy(this.gameObject);
            }
            // else{
            //     Collider2D[] res = new Collider2D[1];
            //     ContactFilter2D c = new ContactFilter2D();
            //     Collider.OverlapCollider(c,res);
            //     List<Collider2D> cols = new List<Collider2D>(res);
            //     Debug.Log(cols[0]);
            //     if(cols.IndexOf(Editor)<0){
            //         Destroy(this.gameObject);
            //     }

            // }
            colocat = true;
            if(nBloc==-1){
                Editor.AfegirBloc(this);
                ActualitzarBloc();
                ActualitzarVariables();
            }
            
        }
    }
}
