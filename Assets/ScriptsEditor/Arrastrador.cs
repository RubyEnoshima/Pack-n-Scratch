using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Arrastrador : MonoBehaviour
{
    int UILayer;
    public GameObject selectedObject;
    public GameObject Fletxa;
    public Bloc selectedBloc;
    public bool Actiu = true;
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
        UILayer = LayerMask.NameToLayer("UI");
    }

    //Returns 'true' if we touched or hovering on Unity UI element.
    public bool IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }

    private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.layer == UILayer)
                return true;
        }
        return false;
    }
 
 
    //Gets all event system raycast results of current mouse or touch position.
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }

    // Update is called once per frame
    void Update()
    {
        if(Actiu){

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 mouseDelta = Input.mousePosition - lastPos;
            if (Input.GetMouseButtonDown(0))
            {
                if(Fletxa.activeSelf) Fletxa.SetActive(false);
                if(Vector3.Distance(Input.mousePosition,lastPos)>=0.075f && !IsPointerOverUIElement() ){
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
}
