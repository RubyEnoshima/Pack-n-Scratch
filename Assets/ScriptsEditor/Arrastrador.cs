using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrastrador : MonoBehaviour
{
    public GameObject selectedObject;
    public Bloc selectedBloc;
    Vector3 offset;
    Vector3 lastPos;

    Collider2D GetHighestObject(Collider2D[] results)
    {
        int highestValue = 0;
        Collider2D highestObject = results[0];
        foreach(Collider2D col in results)
        {
            Renderer ren = col.gameObject.GetComponent<Renderer>();
            if(ren && ren.sortingOrder > highestValue)
            {
                highestValue = ren.sortingOrder;
                highestObject = col;
            }
        }
        
        return highestObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 mouseDelta = Input.mousePosition - lastPos;
        if (Input.GetMouseButtonDown(0))
        {
            if(Vector3.Distance(Input.mousePosition,lastPos)>=0.075f){
                Collider2D[] results = Physics2D.OverlapPointAll(mousePosition);
                if(results.Length!=0){
                    Collider2D highestCollider = GetHighestObject(results);
                    selectedObject = highestCollider.transform.gameObject;
                    if(selectedObject.tag == "Bloc" || selectedObject.tag == "Generador"){
                        selectedBloc = selectedObject.GetComponent<Bloc>();
                        offset = selectedObject.transform.position - mousePosition;

                    }else{
                        selectedObject = null;

                    } 

                }

            }
            lastPos = Input.mousePosition;
        }
        if (selectedObject)
        {
            selectedObject.transform.position = mousePosition + offset;
        }
        if (Input.GetMouseButtonUp(0) && selectedObject)
        {
            selectedObject = null;
        }
    }
}
