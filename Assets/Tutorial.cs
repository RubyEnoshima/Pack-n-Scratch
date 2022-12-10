using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public List<Pista> Pistas;
    public bool desbloquejat = false;
    int n = 0;

    private void Start() {
        foreach(Pista p in Pistas){
            p.gameObject.SetActive(false);
        }
        Pistas[0].gameObject.SetActive(true);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!desbloquejat && Input.GetMouseButtonDown(0)){
            if(Pistas[n]!=null) Pistas[n].gameObject.SetActive(false);
            n++;
            desbloquejat = n>=Pistas.Count;
            if(!desbloquejat && Pistas[n]!=null) Pistas[n].gameObject.SetActive(true);

        }
    }
}
