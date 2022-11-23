using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Llibre : MonoBehaviour
{
    int pagAct = 0;
    public int nPagines = 2;
    Transform canvas;

    bool aSobre = false;

    public void PagSeg(){
        if(pagAct+1<nPagines){
            canvas.GetChild(pagAct).gameObject.SetActive(false);
            pagAct++;
            canvas.GetChild(pagAct).gameObject.SetActive(true);
        }
    }

    public void PagAnt(){
        if(pagAct-1>=0){
            canvas.GetChild(pagAct).gameObject.SetActive(false);
            pagAct--;
            canvas.GetChild(pagAct).gameObject.SetActive(true);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        canvas = transform.GetChild(0);
    }

    void OnMouseOver()
    {
        aSobre = true;
    }

    void OnMouseExit()
    {
        aSobre = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !aSobre)
        {
            gameObject.SetActive(false);
        }
    }
}
