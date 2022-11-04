using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bloc : MonoBehaviour
{
    public Arrastrador Clicker;
    public string Funcio;
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
        if(Clicker.selectedObject && Clicker.selectedObject!=this.gameObject){
            // ContactFilter2D c = new ContactFilter2D();
            // c.minDepth = 0;
            // c.maxDepth = 90;
            // Collider2D[] res = new Collider2D[1];
            // Collider.OverlapCollider(c,res);
            // List<Collider2D> cols = new List<Collider2D>(res);
            // if(cols.IndexOf(Clicker.selectedBloc.Collider)!=-1){
            //     Debug.Log("esta per sobre"+name);
            // }
        }else if(!Clicker.selectedObject){
            Collider2D[] res = new Collider2D[1];
            Collider.OverlapCollider(new ContactFilter2D(),res);
            List<Collider2D> cols = new List<Collider2D>(res);
            if(cols.IndexOf(Canvas)<0){
                Destroy(this.gameObject);
            }
            
        }
    }
}
