using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bloc : MonoBehaviour
{
    public Arrastrador Clicker;
    public Text Label;

    public string Funcio;
    public bool colocat = false;
    protected int nBloc = 0;

    Collider2D Collider;
    Collider2D EditorCol;
    Editor Editor;

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

    public void CanviarNombre(int n){
        nBloc = n;
        Label.text = "#"+nBloc.ToString();
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
            Editor.AfegirBloc(this);
        }
    }
}
