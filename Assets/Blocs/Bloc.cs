using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bloc : MonoBehaviour
{
    public Arrastrador Clicker;
    public string Funcio;
    public bool colocat = false;
    Collider2D Collider;
    Collider2D Canvas;
    // Start is called before the first frame update
    void Start()
    {
        Clicker = FindObjectOfType<Arrastrador>();
        Collider = GetComponent<Collider2D>();
        Canvas = GameObject.Find("Canvas").GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(colocat && Clicker.selectedObject && Clicker.selectedObject==this.gameObject){
            colocat = false;
        }else if(!colocat && !Clicker.selectedObject){
            if(!Canvas.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition))){
                Destroy(this.gameObject);
            }
            // else{
            //     Collider2D[] res = new Collider2D[1];
            //     ContactFilter2D c = new ContactFilter2D();
            //     Collider.OverlapCollider(c,res);
            //     List<Collider2D> cols = new List<Collider2D>(res);
            //     Debug.Log(cols[0]);
            //     if(cols.IndexOf(Canvas)<0){
            //         Destroy(this.gameObject);
            //     }

            // }
            colocat = true;
        }
    }
}
